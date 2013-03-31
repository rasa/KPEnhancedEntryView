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

namespace KPEnhancedEntryView
{
	public partial class EntryView : UserControl
	{
		private static RowObject sInsertionRow = new RowObject(Properties.Resources.NewFieldRowName, null);

		private PwDatabase mDatabase;

		#region Initialisation

		public EntryView() : this(null)
		{
		}

		public EntryView(PwDatabase database)
		{
			InitializeComponent();

			mDatabase = database;

			mFieldValues.AspectPutter = new AspectPutterDelegate(SetFieldValue);
			
			ObjectListView.EditorRegistry.RegisterFirstChance(GetCellEditor);

			if (KeePass.Program.Config.MainWindow.EntryListAlternatingBgColors)
			{
				mFieldsGrid.AlternateRowBackColor = UIUtil.GetAlternateColor(mFieldsGrid.BackColor);
				mFieldsGrid.UseAlternatingBackColors = true;
			}

			var overlay = (TextOverlay)mAttachments.EmptyListMsgOverlay;
			overlay.Alignment = ContentAlignment.TopLeft;
			overlay.InsetX = 0;
			overlay.InsetY = 0;
			overlay.BackColor = Color.Empty;
			overlay.BorderColor = Color.Empty;
			overlay.TextColor = SystemColors.GrayText;

			mNotes.GotFocus += new EventHandler(mNotes_GotFocus);
			mNotes.LostFocus += new EventHandler(mNotes_LostFocus);
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
		}

		private void mFieldsGrid_FormatCell(object sender, FormatCellEventArgs e)
		{
			if (e.Column == mFieldNames)
			{
				if (sInsertionRow.Equals(e.Model))
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
				rows.Add(sInsertionRow);

				mFieldsGrid.SetObjects(rows);
				mFieldNames.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
				mFieldNames.Width += 10; // Give it a bit of a margin to make it look better

				PopulateNotes(Entry.Strings.ReadSafe(PwDefs.NotesField));
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
				builder.Append(ReplaceFormattingTags(value));
				builder.Build(mNotes);
			}
		}

		private static string ReplaceFormattingTags(string strNotes)
		{
			// This code copied from KeePass.Forms.MainForm.ShowEntryDetails (MainForm_Functions.cs). It is not otherwise exposed.
			KeyValuePair<string, string> kvpBold = RichTextBuilder.GetStyleIdCodes(
					FontStyle.Bold);
			KeyValuePair<string, string> kvpItalic = RichTextBuilder.GetStyleIdCodes(
				FontStyle.Italic);
			KeyValuePair<string, string> kvpUnderline = RichTextBuilder.GetStyleIdCodes(
				FontStyle.Underline);

			strNotes = strNotes.Replace(@"<b>", kvpBold.Key);
			strNotes = strNotes.Replace(@"</b>", kvpBold.Value);
			strNotes = strNotes.Replace(@"<i>", kvpItalic.Key);
			strNotes = strNotes.Replace(@"</i>", kvpItalic.Value);
			strNotes = strNotes.Replace(@"<u>", kvpUnderline.Key);
			strNotes = strNotes.Replace(@"</u>", kvpUnderline.Value);

			return strNotes;
		}

		private void mNotes_LostFocus(object sender, EventArgs e)
		{
			var existingValue = Entry.Strings.ReadSafe(PwDefs.NotesField);
			var newValue = mNotes.Text;
			if (newValue != existingValue)
			{
				// Save changes
				Entry.Strings.Set(PwDefs.NotesField, new ProtectedString(mDatabase.MemoryProtection.ProtectNotes, newValue));
			}
			PopulateNotes(newValue);
		}

		private void mNotes_GotFocus(object sender, EventArgs e)
		{
			// Show unformatted text
			mNotes.Text = Entry.Strings.ReadSafe(PwDefs.NotesField);
		}
		#endregion

		#region Cell Editing
		private void mFieldsGrid_CellEditStarting(object sender, CellEditEventArgs e)
		{
			// Disallow editing of standard field names
			if (e.Column == mFieldNames)
			{
				var protectedStringEntry = e.RowObject as Nullable<KeyValuePair<string, ProtectedString>>;
				if (protectedStringEntry.HasValue)
				{
					if (PwDefs.IsStandardField(protectedStringEntry.Value.Key))
					{
						e.Cancel = true;
					}
				}
			}
		}

		public Control GetCellEditor(Object model, OLVColumn column, Object value)
		{
			var rowObject = (RowObject)model;

			if (column == mFieldValues)
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
				var editor = new FieldNameEditor(Entry) { Text = sInsertionRow.Equals(model) ? String.Empty : rowObject.FieldName };
				return editor;
			}

			return null;
		}

		private void SetFieldValue(Object model, Object newValue)
		{
			var rowObject = (RowObject)model;

			var newProtectedString = newValue as ProtectedString ??
									 new ProtectedString(rowObject.Value.IsProtected, (string)newValue);

			Entry.Strings.Set(rowObject.FieldName, newProtectedString);
			rowObject.Value = newProtectedString;
		}
		#endregion	

		#region RowObject
		private class RowObject
		{
			public RowObject(KeyValuePair<string, ProtectedString> keyValuePair)
			{
				FieldName = keyValuePair.Key;
				Value = keyValuePair.Value;
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
		}
		#endregion
	}
}
