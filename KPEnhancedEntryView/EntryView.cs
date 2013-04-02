using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KeePassLib;
using BrightIdeasSoftware;
using KeePassLib.Security;
using KeePass.UI;
using KeePass.Resources;
using System.Reflection;
using KeePass.Util;
using System.Diagnostics;
using System.Threading;
using KeePass.Forms;

namespace KPEnhancedEntryView
{
	public partial class EntryView : UserControl
	{
		private PwDatabase mDatabase;
		private MainForm mMainForm;

		#region Initialisation

		public EntryView() : this(null, null)
		{
		}

		public EntryView(PwDatabase database, MainForm mainForm)
		{
			InitializeComponent();

			mDatabase = database;
			mMainForm = mainForm;

			mFieldValues.AspectPutter = new AspectPutterDelegate(SetFieldValue);
			mFieldNames.AspectPutter = new AspectPutterDelegate(SetFieldName);
			
			ObjectListView.EditorRegistry.RegisterFirstChance(GetCellEditor);

			if (KeePass.Program.Config.MainWindow.EntryListAlternatingBgColors)
			{
				mFieldsGrid.AlternateRowBackColor = UIUtil.GetAlternateColor(mFieldsGrid.BackColor);
				mFieldsGrid.UseAlternatingBackColors = true;
			}

			mNotes.SimpleTextOnly = true;
		}

		#endregion

		#region Hyperlinks
		private void mFieldsGrid_HotItemChanged(object sender, HotItemChangedEventArgs e)
		{
			// HACK: Prevent Hyperlink handling from flickering first colum
			e.Handled = e.HotColumnIndex == 0;
		}

		private void mFieldsGrid_IsHyperlink(object sender, IsHyperlinkEventArgs e)
		{
			e.Url = null;

			var rowObject = (RowObject)e.Model;

			if (e.Column == mFieldValues)
			{
				if (rowObject.Value != null && !rowObject.Value.IsProtected)
				{
					Uri uri;
					if (Uri.TryCreate(rowObject.Value.ReadString(), UriKind.Absolute, out uri))
					{
						e.Url = uri.AbsoluteUri;
					}
				}
			}
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

		#region Font and formatting

		private Font mBoldFont = null;
		private Font mItalicFont = null;

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);

			if (mBoldFont != null) mBoldFont.Dispose();
			if (mItalicFont != null) mItalicFont.Dispose();

			mBoldFont = new Font(Font, FontStyle.Bold);
			mItalicFont = new Font(Font, FontStyle.Italic);

			mAttachments.EmptyListMsgFont = mItalicFont;
			mFieldsGrid.HyperlinkStyle.Over.Font = Font;
		}

		private void mFieldsGrid_FormatCell(object sender, FormatCellEventArgs e)
		{
			var rowObject = (RowObject)e.Model;
			if (e.Column == mFieldNames)
			{
				if (rowObject.IsInsertionRow)
				{
					e.SubItem.Font = mItalicFont;
				}
				else
				{
					e.SubItem.Font = mBoldFont;
				}
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

			mFieldsGrid.CancelCellEdit();
			NotesEditingActive = false;

			if (Entry == null)
			{
				mFieldsGrid.ClearObjects();
				PopulateNotes(null);
			}
			else
			{
				var rows = new List<RowObject>();
				
				// First, the standard fields, in the standard order
				rows.Add(GetStandardField(PwDefs.TitleField));
				rows.Add(GetStandardField(PwDefs.UserNameField));
				rows.Add(GetStandardField(PwDefs.PasswordField));
				rows.Add(GetStandardField(PwDefs.UrlField));

				// Then, all custom strings
				rows.AddRange(from kvp in Entry.Strings where !PwDefs.IsStandardField(kvp.Key) select new RowObject(kvp));

				// Finally, an empty "add new" row
				rows.Add(RowObject.CreateInsertionRow());

				mFieldsGrid.SetObjects(rows);
				mFieldNames.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
				mFieldNames.Width += 10; // Give it a bit of a margin to make it look better

				PopulateNotes(Entry.Strings.ReadSafe(PwDefs.NotesField));

				mAttachments.Entry = Entry;
			}
		}

		private RowObject GetStandardField(string fieldName)
		{
			return new RowObject(fieldName, Entry.Strings.GetSafe(fieldName));
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
				if (value != mNotesEditingActive)
				{
					using (new NotesRtfHelpers.SaveSelectionState(mNotes))
					{
						if (value)
						{
							mNotes.Text = Entry.Strings.ReadSafe(PwDefs.NotesField);
							mNotes.ReadOnly = false;
							mNotesBorder.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
							mNotesEditingActive = true;
						}
						else
						{
							var existingValue = Entry.Strings.ReadSafe(PwDefs.NotesField);
							var newValue = mNotes.Text;
							if (newValue != existingValue)
							{
								// Save changes
								Entry.Strings.Set(PwDefs.NotesField, new ProtectedString(mDatabase.MemoryProtection.ProtectNotes, newValue));
								OnEntryModified(EventArgs.Empty);
							}

							mNotesEditingActive = false;
							PopulateNotes(newValue);
							mNotes.ReadOnly = true;
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

		#region Cell Editing
		private void mFieldsGrid_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				mFieldsGrid.StartCellEdit(mFieldsGrid.SelectedItem, 1);
			}
		}

		private void mFieldsGrid_CellEditStarting(object sender, CellEditEventArgs e)
		{
			var rowObject = (RowObject)e.RowObject;
			// Disallow editing of standard field names
			if (e.Column == mFieldNames)
			{
				if (rowObject.FieldName != null && PwDefs.IsStandardField(rowObject.FieldName))
				{
					e.Cancel = true;
				}
			}
			
			// Disallow editing of the insertion row value
			if (e.Column == mFieldValues && rowObject.IsInsertionRow)
			{
				e.Cancel = true;
			}
		}

		public Control GetCellEditor(Object model, OLVColumn column, Object value)
		{
			var rowObject = (RowObject)model;

			if (column == mFieldValues && !rowObject.IsInsertionRow)
			{
				if (rowObject.Value.IsProtected)
				{
					return new ProtectedFieldEditor
					{
						Value = rowObject.Value,
						HidePassword = true
					};
				}
			}
			else if (column == mFieldNames)
			{
				var editor = new FieldNameEditor(Entry) { Text = rowObject.FieldName };
				return editor;
			}

			return null;
		}

		private void mFieldsGrid_CellEditValidating(object sender, CellEditEventArgs e)
		{
			if (e.Column == mFieldNames)
			{
				var newValue = ((FieldNameEditor)e.Control).Text;
				var rowObject = (RowObject)e.RowObject;

				if (newValue != rowObject.FieldName)
				{
					// Logic copied from EditStringForm.ValidateStringName (EditStringForm.cs)
					if (String.IsNullOrEmpty(newValue))
					{
						ReportValidationFailure(e.Control, KPRes.FieldNamePrompt);
						e.Cancel = true;
						return;
					}
					if (PwDefs.IsStandardField(newValue))
					{
						ReportValidationFailure(e.Control, KPRes.FieldNameInvalid);
						e.Cancel = true;
						return;
					}
					if (newValue.IndexOfAny(new[] { '{', '}' }) >= 0)
					{
						ReportValidationFailure(e.Control, KPRes.FieldNameInvalid);
						e.Cancel = true;
						return;
					}
					if (Entry.Strings.Exists(newValue))
					{
						ReportValidationFailure(e.Control, KPRes.FieldNameExistsAlready);
						e.Cancel = true;
						return;
					}
				}
			}
		}

		private void ReportValidationFailure(Control control, string message)
		{
			mValidationMessage.Show(message, control, 0, control.Height);
			control.KeyPress += ClearValidationFailureMessage;
		}

		private void ClearValidationFailureMessage(object sender, EventArgs e)
		{
			var control = (Control)sender;
			control.KeyPress -= ClearValidationFailureMessage;
			mValidationMessage.Hide(control);
		}

		private void mFieldsGrid_CellEditFinishing(object sender, CellEditEventArgs e)
		{
			if (e.Column == mFieldNames)
			{
				e.NewValue = ((FieldNameEditor)e.Control).Text;
			}

			mFieldsGrid.SelectedObject = e.RowObject;
		}


		private void SetFieldValue(Object model, Object newValue)
		{
			var rowObject = (RowObject)model;

			if (!rowObject.IsInsertionRow)
			{
				var newProtectedString = newValue as ProtectedString ??
										 new ProtectedString(rowObject.Value.IsProtected, (string)newValue);

				Entry.Strings.Set(rowObject.FieldName, newProtectedString);
				OnEntryModified(EventArgs.Empty);

				rowObject.Value = newProtectedString;
			}
		}

		private void SetFieldName(Object model, Object newValue)
		{
			var rowObject = (RowObject)model;
			var newName = (string)newValue;

			if (rowObject.IsInsertionRow)
			{
				// Check if this should be a protected string
				var isProtected = false; // Default to not protected
				var fieldOnOtherEntry = (from otherEntry in Entry.ParentGroup.Entries select otherEntry.Strings.Get(newName)).FirstOrDefault();
				if (fieldOnOtherEntry != null)
				{
					isProtected = fieldOnOtherEntry.IsProtected;
				}

				rowObject.Value = new ProtectedString(isProtected, String.Empty);
				Entry.Strings.Set(newName, rowObject.Value);
				OnEntryModified(EventArgs.Empty);

				// Create a new insertion row to replace this one
				mFieldsGrid.AddObject(RowObject.CreateInsertionRow());
			}
			else
			{
				var fieldValue = Entry.Strings.Get(rowObject.FieldName);
				Entry.Strings.Remove(rowObject.FieldName);
				Entry.Strings.Set(newName, fieldValue);
				OnEntryModified(EventArgs.Empty);
			}

			rowObject.FieldName = newName;
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
		#endregion

		#region RowObject
		private class RowObject
		{
			public static RowObject CreateInsertionRow()
			{
				return new RowObject(null, null);
			}

			public RowObject(KeyValuePair<string, ProtectedString> keyValuePair) : this (keyValuePair.Key, keyValuePair.Value)
			{
			}

			public RowObject(string fieldName, ProtectedString protectedString)
			{
				FieldName = fieldName;
				Value = protectedString;
			}

			public string FieldName { get; set; }
			public ProtectedString Value { get; set; }

			public string DisplayName
			{
				get
				{
					if (IsInsertionRow)
					{
						return Properties.Resources.NewFieldRowName;
					}

					switch (FieldName)
					{
						case PwDefs.TitleField:
							return KPRes.Title;
						case PwDefs.UserNameField:
							return KPRes.UserName;
						case PwDefs.PasswordField:
							return KPRes.Password;
						case PwDefs.UrlField:
							return KPRes.Url;
						default:
							return FieldName;
					}
				}
			}

			public string DisplayValue
			{
				get
				{
					if (Value == null)
					{
						return null;
					}
					
					if (Value.IsProtected)
					{
						return PwDefs.HiddenPassword;
					}
					else
					{
						return Value.ReadString();
					}
				}
			}

			public bool IsInsertionRow { get { return FieldName == null; } }
		}
		#endregion		
	}
}
