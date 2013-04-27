using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using BrightIdeasSoftware;
using KeePass.Forms;
using KeePass.Resources;
using KeePass.UI;
using KeePass.Util;
using KeePassLib;
using KeePassLib.Security;
using KeePassLib.Utility;

namespace KPEnhancedEntryView
{
	public partial class EntryView : UserControl
	{
		private readonly MainForm mMainForm;
		private readonly RichTextBoxContextMenu mNotesContextMenu;

		#region Initialisation

		public EntryView() : this(null)
		{
		}

		public EntryView(MainForm mainForm)
		{
			InitializeComponent();

			mMainForm = mainForm;

			mFieldsGrid.Initialise(mMainForm);
			
			if (KeePass.Program.Config.MainWindow.EntryListAlternatingBgColors)
			{
				mFieldsGrid.AlternateRowBackColor = UIUtil.GetAlternateColor(mFieldsGrid.BackColor);
				mFieldsGrid.UseAlternatingBackColors = true;
			}

			mNotesContextMenu = new RichTextBoxContextMenu();
			mNotesContextMenu.Attach(mNotes, mMainForm);
			mNotes.SimpleTextOnly = true;

			SetLabel(mCreationTimeLabel, KPRes.CreationTime);
			SetLabel(mAccessTimeLabel, KPRes.LastAccessTime);
			SetLabel(mModificationTimeLabel, KPRes.LastModificationTime);
			SetLabel(mExpiryTimeLabel, KPRes.ExpiryTime);
			SetLabel(mTagsLabel, KPRes.Tags);
			SetLabel(mOverrideUrlLabel, KPRes.UrlOverride);

			mEditFieldCommand.ShortcutKeyDisplayString = KPRes.KeyboardKeyReturn;
			mDeleteFieldCommand.ShortcutKeyDisplayString = UIUtil.GetKeysName(Keys.Delete);
			mCopyCommand.ShortcutKeys = Keys.Control | Keys.C;

			mOpenURLCommand.Click += mFieldsGrid.OpenURLCommand_Click;
			mCopyCommand.Click += mFieldsGrid.CopyCommand_Click;
			mEditFieldCommand.Click += mFieldsGrid.EditFieldCommand_Click;
			mProtectFieldCommand.Click += mFieldsGrid.ProtectFieldCommand_Click;
			mPasswordGeneratorCommand.Click += mFieldsGrid.PasswordGeneratorCommand_Click;
			mDeleteFieldCommand.Click += mFieldsGrid.DeleteFieldCommand_Click;
			mAddNewCommand.Click += mFieldsGrid.AddNewCommand_Click;
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
			}
			base.Dispose(disposing);
		}
		#endregion

		#region Hyperlinks
		
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
		
		private PwEntry mEntry;
		public PwEntry Entry 
		{
			get { return mEntry; }
			set
			{
				mEntry = value;
				OnEntryChanged(EventArgs.Empty);
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

			mFieldsGrid.Entry = Entry;
			mAttachments.Entry = Entry;

			PopulateProperties();
		}
		#endregion

		#region Notes
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
				if (Entry == null)
				{
					value = false; // Can't edit if no entry
				}
				if (value != mNotesEditingActive)
				{
					using (new NotesRtfHelpers.SaveSelectionState(mNotes))
					{
						if (value)
						{
							mNotes.Text = Entry.Strings.ReadSafe(PwDefs.NotesField);
							mNotes.ReadOnly = false;
							mNotesBorder.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
							mNotesBorder.Padding = new Padding(0);
							mNotesEditingActive = true;
						}
						else
						{
							var existingValue = Entry.Strings.ReadSafe(PwDefs.NotesField);
							var newValue = mNotes.Text;
							if (newValue != existingValue)
							{
								// Save changes
								Entry.Strings.Set(PwDefs.NotesField, new ProtectedString(Database.MemoryProtection.ProtectNotes, newValue));
								OnEntryModified(EventArgs.Empty);
							}

							mNotesEditingActive = false;
							PopulateNotes(newValue);
							mNotes.ReadOnly = true;
							mNotesBorder.Padding = new Padding(1);
							mNotesBorder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
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
				mOpenURLCommand.Visible = false;
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
				mOpenURLCommand.Visible = e.Item.SubItems.Count == 2 && e.Item.GetSubItem(1).Url != null;
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

		private void mFieldsGrid_EntryModified(object sender, EventArgs e)
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
				mAccessTime.Text = TimeUtil.ToDisplayString(Entry.LastAccessTime);
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
				Entry.OverrideUrl = mOverrideUrl.Text;
				OnEntryModified(EventArgs.Empty);
			}
		}

		private void mTags_Validated(object sender, EventArgs e)
		{
			if (Entry != null)
			{
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
	}
}
