using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using BrightIdeasSoftware;
using KeePass.Forms;
using KeePass.Resources;
using KeePass.UI;
using KeePass.Util;
using KeePassLib;
using KeePassLib.Security;
using KeePassLib.Utility;
using System.Collections.Generic;

namespace KPEnhancedEntryView
{
	public partial class EntryView : UserControl
	{
		public static Regex MarkedLinkRegex = new Regex(@"<[^<>\s](?:[^<>\s]| )*[^<>\s]>", RegexOptions.Singleline);

		private const uint ShowLastAccessTimeUIFlag = 0x20000;

		private readonly MainForm mMainForm;
		private readonly Options mOptions;
		private readonly MethodInfo mHandleMainWindowKeyMessageMethod;
		private readonly RichTextBoxContextMenu mNotesContextMenu;
		private readonly OpenWithMenu mURLDropDownMenu;
		private readonly bool mShowAccessTime;

		/// <summary>When a context menu is shown for a field value with a URL, that URL will be stored in this variable for use with the OpenWith menu</summary>
		private string mLastContextMenuUrl;

		/// <summary>When a context menu is shown for a fields grid, that grid control is stored here so the commands can act on the appropriate target</summary>
		private FieldsListView mFieldGridContextMenuTarget;

		#region Initialisation

		public EntryView() : this(null, null)
		{
		}

		public EntryView(MainForm mainForm, Options options)
		{
			InitializeComponent();

			mMainForm = mainForm;
			mOptions = options;

			mFieldsGrid.Initialise(mMainForm, mOptions);
			mMultipleSelectionFields.Initialise(mMainForm, mOptions);

			// KeePass 2.24 and above deprecates last access time
			mShowAccessTime = (PwDefs.FileVersion64 < 0x0002001800000000UL) || ((KeePass.Program.Config.UI.UIFlags & 0x20000) != 0);
		
			mAccessTimeLabel.Visible = mAccessTime.Visible = mShowAccessTime;
			
			// HACK: MainForm doesn't expose HandleMainWindowKeyMessage, so grab it via reflection
			mHandleMainWindowKeyMessageMethod = mMainForm.GetType().GetMethod("HandleMainWindowKeyMessage", BindingFlags.Instance | BindingFlags.NonPublic);
			if (mHandleMainWindowKeyMessageMethod != null)
			{
				mTabs.KeyDown += HandleMainWindowShortcutKeyDown;
				mTabs.KeyUp += HandleMainWindowShortcutKeyUp;
			}

			mNotesContextMenu = new RichTextBoxContextMenu();
			mNotesContextMenu.Attach(mNotes, mMainForm);
			mNotes.SimpleTextOnly = true;

			SetLabel(mCreationTimeLabel, KPRes.CreationTime);
			if (mShowAccessTime)
			{
				SetLabel(mAccessTimeLabel, KPRes.LastAccessTime);
			}
			SetLabel(mModificationTimeLabel, KPRes.LastModificationTime);
			SetLabel(mExpiryTimeLabel, KPRes.ExpiryTime);
			SetLabel(mTagsLabel, KPRes.Tags);
			SetLabel(mOverrideUrlLabel, KPRes.UrlOverride);

			mEditFieldCommand.ShortcutKeyDisplayString = KPRes.KeyboardKeyReturn;
			mDeleteFieldCommand.ShortcutKeyDisplayString = UIUtil.GetKeysName(Keys.Delete);
			mCopyCommand.ShortcutKeys = Keys.Control | Keys.C;

			mURLDropDownMenu = new OpenWithMenu(mURLDropDown);
			CustomizeOnClick(mURLDropDownMenu);

			mSplitGridPanels.SplitRatio = mOptions.FieldsNotesSplitPosition;
			mSplitNotesAttachements.SplitRatio = mOptions.NotesAttachmentsSplitPosition;
		}

		private static void SetLabel(Label label, string text)
		{
			label.Text = text + ":";
		}

		public Control AllTextControl 
		{
			get { return mAllTextTab.Controls.Cast<Control>().FirstOrDefault(); }
			set
			{
				mAllTextTab.Controls.Clear();
				mAllTextTab.Controls.Add(value);
			}
		}

		private PwDatabase Database { get { return mMainForm.ActiveDatabase; } }

		#endregion

		#region Disposal
		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}

				if (mNotesContextMenu != null)
				{
					mNotesContextMenu.Detach();
				}

				if (mURLDropDownMenu != null)
				{
					mURLDropDownMenu.Destroy();
				}

				// Ensure all tabs are disposed, even if they aren't currentl visible
				mMultipleSelectionTab.Dispose();
				mFieldsTab.Dispose();
				mPropertiesTab.Dispose();
				mAllTextTab.Dispose();
			}
			base.Dispose(disposing);
		}
		#endregion

		#region Hyperlinks

		private void CustomizeOnClick(OpenWithMenu openWithMenu)
		{
			// The OpenWithMenu will only open the entry main URL field when clicked, and it's sealed, so have to use reflection to hack it open
			
			var dynMenu = GetDynamicMenu(openWithMenu);
			if (dynMenu != null)
			{
				var onOpenUrlMethodInfo = typeof(OpenWithMenu).GetMethod("OnOpenUrl", BindingFlags.Instance | BindingFlags.NonPublic);
				if (onOpenUrlMethodInfo != null)
				{
					// Detach the original handler
					var onOpenUrlDelegate = Delegate.CreateDelegate(typeof(EventHandler<DynamicMenuEventArgs>), openWithMenu, onOpenUrlMethodInfo) as EventHandler<DynamicMenuEventArgs>;
					if (onOpenUrlDelegate != null)
					{
						dynMenu.MenuClick -= onOpenUrlDelegate;

						// Attach our handler
						dynMenu.MenuClick += mURLDropDownMenu_MenuClick;
					}
				}
			}
		}

		private void DetachOnClick(OpenWithMenu openWithMenu)
		{
			var dynMenu = GetDynamicMenu(openWithMenu);
			if (dynMenu != null)
			{
				// Detach our handler
				dynMenu.MenuClick -= mURLDropDownMenu_MenuClick;
			}
		}

		private DynamicMenu GetDynamicMenu(OpenWithMenu openWithMenu)
		{
			var dynMenuFieldInfo = typeof(OpenWithMenu).GetField("m_dynMenu", BindingFlags.Instance | BindingFlags.NonPublic);
			if (dynMenuFieldInfo != null)
			{
				return dynMenuFieldInfo.GetValue(openWithMenu) as DynamicMenu;
			}
			return null;
		}

		private void mURLDropDownMenu_MenuClick(object sender, DynamicMenuEventArgs e)
		{
			var filePath = GetFilePath(e.Tag);
			if (filePath != null && mLastContextMenuUrl != null)
			{
				WinUtil.OpenUrlWithApp(mLastContextMenuUrl, Entry, filePath);
			}
		}

		private string GetFilePath(object openWithItemTag)
		{
			if (openWithItemTag != null)
			{
				var openWithItemType = openWithItemTag.GetType();
				Debug.Assert(openWithItemType.Name == "OpenWithItem");
				var filePathPropertyInfo = openWithItemType.GetProperty("FilePath", BindingFlags.Public | BindingFlags.Instance);
				if (filePathPropertyInfo != null)
				{
					return filePathPropertyInfo.GetValue(openWithItemTag, null) as string;
				}
			}

			return null;
		}
		
		private void mFieldsGrid_HyperlinkClicked(object sender, HyperlinkClickedEventArgs e)
		{
			e.Handled = true; // Disable default processing

			// Defer until double click time to ensure that if double clicking, URL isn't opened.
			mDoubleClickTimer.Interval = SystemInformation.DoubleClickTime;
			mClickedUrl = e.Url;
			mDoubleClickTimer.Start();
		}

		private string mClickedUrl;
		private void mDoubleClickTimer_Tick(object sender, EventArgs e)
		{
			mDoubleClickTimer.Stop(); // Tick once only.

			if (!mFieldsGrid.IsCellEditing) // If they are now editing a cell, then don't visit the URL
			{
				var url = mClickedUrl;
				mClickedUrl = null;
				if (url != null)
				{
					WinUtil.OpenUrl(url, Entry);
				}
			}
		}

		private MethodInfo mMainFormOnEntryViewLinkClicked;
		private void mNotes_LinkClicked(object sender, LinkClickedEventArgs e)
		{
			mNotes.Parent.Focus();

			// OnEntryViewLinkClicked is not exposed so grab it through reflection. There is no other exposure of reference link following
			if (mMainFormOnEntryViewLinkClicked == null)
			{
				mMainFormOnEntryViewLinkClicked = mMainForm.GetType().GetMethod("OnEntryViewLinkClicked", BindingFlags.Instance | BindingFlags.NonPublic);

				if (mMainFormOnEntryViewLinkClicked == null)
				{
					Debug.Fail("Couldn't find MainForm.OnEntryViewLinkClicked");
					return;
				}
			}

			mMainFormOnEntryViewLinkClicked.Invoke(mMainForm, new object[] { sender, e });

			// UpdateUI isn't triggered for moving to the target of the link, if it's a reference link. Internal code manually updates the entry view, so do that here too
			var selectedEntry = mMainForm.GetSelectedEntry(true);
			if (selectedEntry != Entry) // Only bother if we've actually moved
			{
				Entry = selectedEntry;
			}
		}
		#endregion

		#region Population
		private bool mSuspendEntryChangedPopulation;
		private DateTime? mEntryLastModificationTime;
		private PwEntry mEntry;
		private IEnumerable<PwEntry> mEntries;

		/// <summary>
		/// Gets or sets a multiple selection of entries. If only a single entry is set, then this will set
		/// <see cref="Entry"/>, otherwise <see cref="Entry"/> will be null.
		/// </summary>
		public IEnumerable<PwEntry> Entries
		{
			get
			{
				if (mEntries == null)
				{
					if (Entry == null)
					{
						return Enumerable.Empty<PwEntry>();
					}
					else
					{
						return Enumerable.Repeat(Entry, 1);
					}
				}
				else
				{
					return mEntries;
				}
			}
			set
			{
				if (value == null)
				{
					Entry = null;
				}
				else if (value.Count() == 1)
				{
					// Single selection
					Entry = value.First();
				}
				else
				{
					// Multiple selection
					mEntry = null;
					mEntryLastModificationTime = null;
					mEntries = value;

					// TODO: Extra checking needed to see if this has actually changed?
					OnEntryChanged(EventArgs.Empty);
				}
			}
		}

		private bool IsMultipleSelection
		{
			get { return mEntries != null; }
		}
		
		/// <summary>
		/// Gets or sets a single selected entry. Will clear any previous value set to <see cref="Entries"/>
		/// Returns null if a multiple selection has been set using <see cref="Entries"/>
		/// </summary>
		public PwEntry Entry
		{
			get { return mEntry; }
			set
			{
				if (value != mEntry ||
					(value == null || value.LastModificationTime != mEntryLastModificationTime))
				{
					mEntry = value;
					mEntries = null;

					mEntryLastModificationTime = mEntry == null ? null : (DateTime?)mEntry.LastModificationTime;
					OnEntryChanged(EventArgs.Empty);
				}
			}
		}

		protected virtual void OnEntryChanged(EventArgs e)
		{
			if (mSuspendEntryChangedPopulation)
			{
				return;
			}

			mAttachments.CancelCellEdit();
			mFieldsGrid.CancelCellEdit();
			NotesEditingActive = false;

			if (IsMultipleSelection)
			{
				mTabs.SuspendLayout();
				SetTabVisibility(mMultipleSelectionTab, true);
				SetTabVisibility(mFieldsTab, false);
				SetTabVisibility(mPropertiesTab, false);
				SetTabVisibility(mAllTextTab, false);
				mTabs.ResumeLayout();
			}
			else
			{
				mTabs.SuspendLayout();
				SetTabVisibility(mFieldsTab, true);
				SetTabVisibility(mPropertiesTab, true);
				SetTabVisibility(mAllTextTab, true);
				SetTabVisibility(mMultipleSelectionTab, false);
				mTabs.ResumeLayout();
			}

			mMultipleSelectionFields.Entries = mEntries; // Use mEntries rather than Entries, so get the raw null/no entries case when only a single entity is selected - no need to populate in that case.
			mFieldsGrid.Entry = Entry;
			mAttachments.Entry = Entry;
			mAttachments.Database = Database;

			PopulateProperties();

			if (Entry == null)
			{
				PopulateNotes(null);
			}
			else
			{
				using (var selection = new NotesRtfHelpers.SaveSelectionState(mNotes, true))
				{
					PopulateNotes(Entry.Strings.ReadSafe(PwDefs.NotesField));
				}
			}
		}

		private void SetTabVisibility(TabPage tab, bool visible)
		{
			if (visible && tab.Parent != mTabs)
			{
				mTabs.TabPages.Add(tab);
			}
			else if (!visible && tab.Parent == mTabs)
			{
				mTabs.TabPages.Remove(tab);
			}
		}

		public void RefreshItems()
		{
			mFieldsGrid.RefreshItems();
			mMultipleSelectionFields.RefreshItems();
		}
		#endregion

		#region Splitters

		private void mSplitGridPanels_SplitterMoved(object sender, SplitterEventArgs e)
		{
			// Special case as fields grid does *not* like being small
			if (mSplitGridPanels.SplitterDistance < mSplitGridPanels.MinimumSplitSize)
			{
				mSplitGridPanels.SplitterDistance = 0;
				mFieldsGrid.Visible = false;
			}
			else
			{
				mFieldsGrid.Visible = true;
			}

			mOptions.FieldsNotesSplitPosition = mSplitGridPanels.SplitRatio;
		}

		private void mSplitNotesAttachements_SplitterMoved(object sender, SplitterEventArgs e)
		{
			mOptions.NotesAttachmentsSplitPosition = mSplitNotesAttachements.SplitRatio;
		}

		#endregion

		#region Notes

		public void FinishEditingNotes()
		{
			NotesEditingActive = false;
		}

		private void PopulateNotes(string value)
		{
			Debug.Assert(!mNotesEditingActive, "Can't populate while editing!");

			var builder = new RichTextBuilder { DefaultFont = Font };
				
			if (String.IsNullOrEmpty(value))
			{
				// Populate it with a watermark
				builder.Append(KPRes.Notes, FontStyle.Italic);
				builder.Build(mNotes);
				mNotes.SelectAll();
				mNotes.SelectionColor = SystemColors.GrayText;
				mNotes.Select(0, 0);
				mNotes.ReadOnly = true;
			}
			else
			{
				builder.Append(NotesRtfHelpers.ReplaceFormattingTags(value));
				builder.Build(mNotes);

				UIUtil.RtfLinkifyReferences(mNotes, false);
				NotesRtfHelpers.RtfLinkifyUrls(mNotes);
			}
		}

		private bool mNotesEditingActive;
		
		private bool NotesEditingActive
		{
			get { return mNotesEditingActive; }
			set
			{
				var entry = Entry;
				
				if (entry == null)
				{
					value = false; // Can't edit if no entry
				}
				if (value != mNotesEditingActive)
				{
					using (new NotesRtfHelpers.SaveSelectionState(mNotes))
					{
						if (value)
						{
							mNotes.Text = entry.Strings.ReadSafe(PwDefs.NotesField);
							mNotes.ReadOnly = false;
							mNotesBorder.BorderStyle = BorderStyle.Fixed3D;
							mNotesBorder.Padding = new Padding(0);
							mNotesEditingActive = true;
						}
						else
						{
							mNotesEditingActive = false;

							if (entry == null)
							{
								PopulateNotes(null);
							}
							else
							{
								var existingValue = entry.Strings.ReadSafe(PwDefs.NotesField);
								var newValue = mNotes.Text;
								if (newValue != existingValue)
								{
									// Save changes
									entry.CreateBackup(Database);
									entry.Strings.Set(PwDefs.NotesField, new ProtectedString(Database.MemoryProtection.ProtectNotes, newValue));
									OnEntryModified(EventArgs.Empty);
								}

								PopulateNotes(newValue);
							}

							mNotes.ReadOnly = true;
							mNotesBorder.Padding = new Padding(1);
							mNotesBorder.BorderStyle = BorderStyle.FixedSingle;
						}
					}
				}
			}
		}

		private void mNotes_KeyDown(object sender, KeyEventArgs e)
		{
			if (!NotesEditingActive && e.KeyData == Keys.Enter)
			{
				e.Handled = true;
				NotesEditingActive = true;
			}

			if (NotesEditingActive && e.KeyData == Keys.Escape)
			{
				e.Handled = true;
				// Should escape discard any changes made?
				NotesEditingActive = false;
			}
		}

		private void mNotes_Enter(object sender, EventArgs e)
		{
			// Defer briefly so that there's time for link clicking to invoke
			BeginInvoke((MethodInvoker)delegate
			{
				if (mNotes.Focused)
				{
					NotesEditingActive = true;
				}
			});
		}

		private void mNotes_DoubleClick(object sender, EventArgs e)
		{
			NotesEditingActive = true;
		}

		private void mNotes_Leave(object sender, EventArgs e)
		{
			NotesEditingActive = false;
		}
		#endregion

		#region Context Menu
		private void mFieldsGrid_CellRightClick(object sender, CellRightClickEventArgs e)
		{
			var rowObject = (FieldsListView.RowObject)e.Model;

			if (rowObject == null || rowObject.IsInsertionRow)
			{
				mURLDropDown.Visible = false;
				mCopyCommand.Enabled = false;
				mEditFieldCommand.Enabled = false;
				mProtectFieldCommand.Enabled = false;
				mPasswordGeneratorCommand.Enabled = false;
				mDeleteFieldCommand.Enabled = false;
				mAddNewCommand.Enabled = Entry != null;

				mProtectFieldCommand.Checked = false;
				mCopyCommand.Text = String.Format(Properties.Resources.CopyCommand, Properties.Resources.Field);
			}
			else
			{
				var url = e.Item.SubItems.Count == 2 ? e.Item.GetSubItem(1).Url : null;
				mLastContextMenuUrl = url;
				mURLDropDown.Visible = url != null;
				mCopyCommand.Enabled = true;
				mEditFieldCommand.Enabled = true;
				mProtectFieldCommand.Enabled = true;
				mPasswordGeneratorCommand.Enabled = true;
				mDeleteFieldCommand.Enabled = true;
				mAddNewCommand.Enabled = true;
			
				mProtectFieldCommand.Checked = rowObject.Value.IsProtected;
				mCopyCommand.Text = String.Format(Properties.Resources.CopyCommand, rowObject.DisplayName);
			}
			e.MenuStrip = mFieldGridContextMenu;
			mFieldGridContextMenuTarget = mFieldsGrid;
		}

		private void mMultipleSelectionFields_CellRightClick(object sender, CellRightClickEventArgs e)
		{
			var rowObject = (FieldsListView.RowObject)e.Model;

			if (rowObject == null || rowObject.IsInsertionRow)
			{
				mURLDropDown.Visible = false;
				mCopyCommand.Enabled = false;
				mEditFieldCommand.Enabled = false;
				mProtectFieldCommand.Enabled = false;
				mPasswordGeneratorCommand.Enabled = false;
				mDeleteFieldCommand.Enabled = false;
				mAddNewCommand.Enabled = Entry != null;

				mProtectFieldCommand.Checked = false;
				mCopyCommand.Text = String.Format(Properties.Resources.CopyCommand, Properties.Resources.Field);
			}
			else
			{
				if (mMultipleSelectionFields.IsMultiValuedField(rowObject))
				{
					mURLDropDown.Visible = false;
					mCopyCommand.Enabled = false;
					mProtectFieldCommand.Enabled = false;
					
					mProtectFieldCommand.Checked = false;
					mCopyCommand.Text = String.Format(Properties.Resources.CopyCommand, Properties.Resources.Field);
				}
				else
				{
					var url = e.Item.SubItems.Count == 2 ? e.Item.GetSubItem(1).Url : null;
					mLastContextMenuUrl = url;
					mURLDropDown.Visible = url != null;
					mCopyCommand.Enabled = true;
					mProtectFieldCommand.Enabled = true;
					
					mProtectFieldCommand.Checked = rowObject.Value.IsProtected;

					mCopyCommand.Text = String.Format(Properties.Resources.CopyCommand, rowObject.DisplayName);
				}

				mEditFieldCommand.Enabled = true;
				mPasswordGeneratorCommand.Enabled = true;
				mDeleteFieldCommand.Enabled = true;
				mAddNewCommand.Enabled = true;
			}
			e.MenuStrip = mFieldGridContextMenu;
			mFieldGridContextMenuTarget = mMultipleSelectionFields;
		}

		private void mAttachments_CellRightClick(object sender, CellRightClickEventArgs e)
		{
			var singleItemSelected = mAttachments.SelectedObjects.Count == 1;
			var anyItemSelected = mAttachments.SelectedObjects.Count > 0;

			mViewBinaryCommand.Enabled = singleItemSelected;
			mRenameBinaryCommand.Enabled = singleItemSelected;
			mSaveBinaryCommand.Enabled = anyItemSelected;
			mDeleteBinaryCommand.Enabled = anyItemSelected;
			mAttachBinaryCommand.Enabled = Entry != null;

			e.MenuStrip = mAttachmentsContextMenu;
		}
		#endregion

		#region EntryModified event
		public event EventHandler EntryModified;
		protected virtual void OnEntryModified(EventArgs e)
		{
			if (Entry != null)
			{
				Entry.Touch(true, false);
			}

			var temp = EntryModified;
			if (temp != null)
			{
				try
				{
					mSuspendEntryChangedPopulation = true; // We have already made the change to the UI, don't need to repopulate in response to notifying the main window of the change
					temp(this, e);
				}
				finally
				{
					mSuspendEntryChangedPopulation = false;
				}
			}
		}

		private void mAttachments_EntryModified(object sender, EventArgs e)
		{
			OnEntryModified(e);
		}

		private void mFieldsGrid_Modified(object sender, EventArgs e)
		{
			OnEntryModified(e);
		}

		private void mMultipleSelectionFields_Modified(object sender, EventArgs e)
		{
			OnEntryModified(e);
		}
		#endregion

		#region Properties Tab
		private void PopulateProperties()
		{
			if (Entry == null)
			{
				mPropertiesTabLayout.Visible = false;
			}
			else
			{
				mGroupButton.Text = Entry.ParentGroup.Name;
				UIUtil.SetButtonImage(mGroupButton, GetImage(Entry.ParentGroup.CustomIconUuid, Entry.ParentGroup.IconId), true);

				UIUtil.SetButtonImage(m_btnIcon, GetImage(Entry.CustomIconUuid, Entry.IconId), true);

				mCreationTime.Text = TimeUtil.ToDisplayString(Entry.CreationTime);
				if (mShowAccessTime)
				{
					mAccessTime.Text = TimeUtil.ToDisplayString(Entry.LastAccessTime);
				}
				mModificationTime.Text = TimeUtil.ToDisplayString(Entry.LastModificationTime);

				if (Entry.Expires)
				{
					mExpiryTime.Text = TimeUtil.ToDisplayString(Entry.ExpiryTime);

					mExpiryTimeLabel.Visible = mExpiryTime.Visible = true;
				}
				else
				{
					mExpiryTimeLabel.Visible = mExpiryTime.Visible = false;
				}

				mOverrideUrl.Text = Entry.OverrideUrl;
				mTags.Text = StrUtil.TagsToString(Entry.Tags, true);

				mPropertiesTabLayout.Visible = true;
			}
		}

		// Strangely, there doesn't appear to already exist a helper to get an image for a group. If one appears, then that should be used instead of this custom one.
		private Image GetImage(PwUuid customIconId, PwIcon iconId)
		{
			Image image = null;
			if (Database != null)
			{
				if (!customIconId.EqualsValue(PwUuid.Zero))
				{
					image = Database.GetCustomIcon(customIconId);
				}
				if (image == null)
				{
					try { image = mMainForm.ClientIcons.Images[(int)iconId]; }
					catch (Exception) { Debug.Assert(false); }
				}
			}

			return image;
		}

		private void mOverrideUrl_Validated(object sender, EventArgs e)
		{
			if (Entry != null)
			{
				Entry.CreateBackup(Database);
				Entry.OverrideUrl = mOverrideUrl.Text;
				OnEntryModified(EventArgs.Empty);
			}
		}

		private void mTags_Validated(object sender, EventArgs e)
		{
			if (Entry != null)
			{
				Entry.CreateBackup(Database);
				Entry.Tags.Clear();
				Entry.Tags.AddRange(StrUtil.StringToTags(mTags.Text));
				OnEntryModified(EventArgs.Empty);
			}
		}

		private void mGroupButton_Click(object sender, EventArgs e)
		{
			mMainForm.UpdateUI(false, null, true, Entry.ParentGroup, true, null, false);
		}

		#region Icon Picking
		// Logic from PwEntryForm.OnBtnPickIcon
		private void m_btnIcon_Click(object sender, EventArgs e)
		{
			var iconPicker = new IconPickerForm();
			iconPicker.InitEx(mMainForm.ClientIcons, (uint)PwIcon.Count, Database, (uint)Entry.IconId, Entry.CustomIconUuid);

			if (iconPicker.ShowDialog() == DialogResult.OK)
			{
				Entry.CreateBackup(Database);

				if (iconPicker.ChosenCustomIconUuid != PwUuid.Zero)
				{
					Entry.CustomIconUuid = iconPicker.ChosenCustomIconUuid;
				}
				else
				{
					Entry.CustomIconUuid = PwUuid.Zero;
					Entry.IconId = (PwIcon)iconPicker.ChosenIconId;
				}

				UIUtil.SetButtonImage(m_btnIcon, GetImage(Entry.CustomIconUuid, Entry.IconId), true);

				OnEntryModified(EventArgs.Empty);
			}

			UIUtil.DestroyForm(iconPicker);
		}
		#endregion

		#endregion

		#region Fields Menu Event handlers
		private void mCopyCommand_Click(object sender, EventArgs e)
		{
			mFieldGridContextMenuTarget.DoCopy();
		}

		private void mEditFieldCommand_Click(object sender, EventArgs e)
		{
			mFieldGridContextMenuTarget.DoEditField();
		}

		private void mProtectFieldCommand_Click(object sender, EventArgs e)
		{
			var protect = ((ToolStripMenuItem)sender).Checked;
			mFieldGridContextMenuTarget.DoSetProtected(protect);
		}

		private void mPasswordGeneratorCommand_Click(object sender, EventArgs e)
		{
			mFieldGridContextMenuTarget.DoPasswordGenerator();
		}

		private void mDeleteFieldCommand_Click(object sender, EventArgs e)
		{
			mFieldGridContextMenuTarget.DoDeleteField();
		}

		private void mAddNewCommand_Click(object sender, EventArgs e)
		{
			mFieldGridContextMenuTarget.DoAddNew();
		}

		private void mOpenURLCommand_Click(object sender, EventArgs e)
		{
			mFieldGridContextMenuTarget.DoOpenUrl();
		}
		#endregion

		#region Attachments Menu Event Handlers
		private void mViewBinaryCommand_Click(object sender, EventArgs e)
		{
			mAttachments.ViewSelected();
		}

		private void mRenameBinaryCommand_Click(object sender, EventArgs e)
		{
			mAttachments.RenameSelected();
		}

		private void mSaveBinaryCommand_Click(object sender, EventArgs e)
		{
			mAttachments.SaveSelected();
		}

		private void mDeleteBinaryCommand_Click(object sender, EventArgs e)
		{
			mAttachments.DeleteSelected();
		}

		private void mAttachBinaryCommand_Click(object sender, EventArgs e)
		{
			mAttachments.AttachFiles();
		}
		#endregion

		#region Keyboard Shortcuts
		private void HandleMainWindowShortcutKeyDown(object sender, KeyEventArgs e)
		{
			HandleMainWindowShortcutKey(e, true);
		}

		private void HandleMainWindowShortcutKeyUp(object sender, KeyEventArgs e)
		{
			HandleMainWindowShortcutKey(e, false);
		}

		private void HandleMainWindowShortcutKey(KeyEventArgs e, bool keyDown)
		{
			try 
			{
				mHandleMainWindowKeyMessageMethod.Invoke(mMainForm, new object[] { e, keyDown });
			}
			catch (Exception)
			{
				// Ignore it
				Debug.Fail("Could not pass on main window key shortcut");
			}
		}
		#endregion

		private void mSplitNotesAttachements_SplitterMoving(object sender, SplitterCancelEventArgs e)
		{

		}

		private void mSplitGridPanels_SplitterMoving(object sender, SplitterCancelEventArgs e)
		{

		}
	}
}
