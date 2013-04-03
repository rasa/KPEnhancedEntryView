using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using KeePass.Forms;
using KeePass.Resources;
using KeePass.UI;
using KeePass.Util;
using KeePassLib;
using KeePassLib.Cryptography.PasswordGenerator;
using KeePassLib.Security;

namespace KPEnhancedEntryView
{
	internal class FieldsListView : FastObjectListView
	{
		private readonly BrightIdeasSoftware.OLVColumn mFieldNames;
		private readonly BrightIdeasSoftware.OLVColumn mFieldValues;

		private MainForm mMainForm;

		public FieldsListView() 
		{
			CellEditActivation = CellEditActivateMode.DoubleClick;
			CellEditTabChangesRows = true;
			CopySelectionOnControlC = false;
			FullRowSelect = true;
			HeaderStyle = ColumnHeaderStyle.Nonclickable;
			ShowGroups = false;
			UseCellFormatEvents = true;
			UseHyperlinks = true;

			mFieldNames = new OLVColumn
			{
				Text = Properties.Resources.Name,
				AutoCompleteEditor = false,
				AspectGetter = rowObject => ((RowObject)rowObject).DisplayName,
				AspectPutter = SetFieldName
			};
			mFieldValues = new OLVColumn
			{
				Text = Properties.Resources.Value,
				AutoCompleteEditor = false,
				FillsFreeSpace = true,
				Hyperlink = true,
				AspectGetter = rowObject => ((RowObject)rowObject).DisplayValue,
				AspectPutter = SetFieldValue
			};
			
			Columns.Add(mFieldNames);
			Columns.Add(mFieldValues);

			ObjectListView.EditorRegistry.RegisterFirstChance(GetCellEditor);
		}

		/// <summary>
		/// Ensure this is called before using the control
		/// </summary>
		public void Initialise(MainForm mainForm)
		{
			mMainForm = mainForm;
		}

		private PwDatabase Database { get { return mMainForm.ActiveDatabase; } }

		#region Disposal
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (mItalicFont != null)
				{
					mItalicFont.Dispose();
					mItalicFont = null;
				}
				if (mBoldFont != null)
				{
					mBoldFont.Dispose();
					mBoldFont = null;
				}
			}
			base.Dispose(disposing);
		}
		#endregion

		#region Entry
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
				ClearObjects();
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

				SetObjects(rows);
				mFieldNames.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
				mFieldNames.Width += 10; // Give it a bit of a margin to make it look better
			}
		}

		private RowObject GetStandardField(string fieldName)
		{
			return new RowObject(fieldName, Entry.Strings.GetSafe(fieldName));
		}
		#endregion

		#region EntryModified event
		public event EventHandler EntryModified;
		protected virtual void OnEntryModified(EventArgs e)
		{
			var temp = EntryModified;
			if (temp != null)
			{
				temp(this, e);
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

			// This doesn't inherit automatically, for some reason
			HyperlinkStyle.Over.Font = Font;
		}

		protected override void OnFormatCell(FormatCellEventArgs args)
		{
			base.OnFormatCell(args);
		
			var rowObject = (RowObject)args.Model;
			if (args.Column == mFieldNames)
			{
				if (rowObject.IsInsertionRow)
				{
					args.SubItem.Font = mItalicFont;
				}
				else
				{
					args.SubItem.Font = mBoldFont;
				}
			}
		}
		#endregion

		#region Hyperlinks
		protected override void OnHotItemChanged(HotItemChangedEventArgs e)
		{
			base.OnHotItemChanged(e);
			// HACK: Prevent Hyperlink handling from flickering first colum
			e.Handled = e.HotColumnIndex == 0;
		}

		protected override void OnIsHyperlink(IsHyperlinkEventArgs e)
		{
			base.OnIsHyperlink(e);

			e.Url = null;

			var rowObject = (FieldsListView.RowObject)e.Model;

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
		#endregion

		#region Cell Editing
		protected override void OnCellEditStarting(CellEditEventArgs e)
		{
			base.OnCellEditStarting(e);

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

		private Control GetCellEditor(Object model, OLVColumn column, Object value)
		{
			var rowObject = model as RowObject;

			if (rowObject != null)
			{
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
			}

			return null;
		}

		protected override void OnCellEditorValidating(CellEditEventArgs e)
		{
			base.OnCellEditorValidating(e);
		
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

		protected override void OnCellEditFinishing(CellEditEventArgs e)
		{
			base.OnCellEditFinishing(e);
		
			if (e.Column == mFieldNames)
			{
				e.NewValue = ((FieldNameEditor)e.Control).Text;
			}

			SelectedObject = e.RowObject;
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

				rowObject.Value = new ProtectedString(isProtected, new byte[0]);
				Entry.Strings.Set(newName, rowObject.Value);
				OnEntryModified(EventArgs.Empty);

				// Create a new insertion row to replace this one
				AddObject(RowObject.CreateInsertionRow());
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

		#region Validation Failure Reporting
		public ValidationFailureReporter ValidationFailureReporter { get; set; }

		private void ReportValidationFailure(Control control, string message)
		{
			if (ValidationFailureReporter != null)
			{
				ValidationFailureReporter.ReportValidationFailure(control, message);
			}
		}
		#endregion

		#region Commands
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if (!IsCellEditing)
			{
				var rowObject = (RowObject)SelectedObject;

				if (rowObject != null)
				{
					if (keyData == (Keys.Control | Keys.C))
					{
						CopyCommand(rowObject);
						return true;
					}
					else if (keyData == Keys.Return)
					{
						EditFieldCommand(rowObject);
						return true;
					}
					else if (keyData == Keys.Delete)
					{
						DeleteFieldCommand(rowObject);
						return true;
					}
				}
			}
			return base.ProcessDialogKey(keyData);
		}

		public void CopyCommand_Click(object sender, EventArgs e) { DoFieldCommand(CopyCommand); }
		public void EditFieldCommand_Click(object sender, EventArgs e) { DoFieldCommand(EditFieldCommand); }
		public void ProtectFieldCommand_Click(object sender, EventArgs e) { DoFieldCommand(rowObject => ProtectFieldCommand(rowObject, ((ToolStripMenuItem)sender).Checked)); }
		public void PasswordGeneratorCommand_Click(object sender, EventArgs e) { DoFieldCommand(PasswordGeneratorCommand); }
		public void DeleteFieldCommand_Click(object sender, EventArgs e) { DoFieldCommand(DeleteFieldCommand); }
		public void AddNewCommand_Click(object sender, EventArgs e) { AddNewCommand(); }

		private void DoFieldCommand(Action<RowObject> command)
		{
			var rowObject = (RowObject)SelectedObject;

			if (rowObject != null && !rowObject.IsInsertionRow)
			{
				command(rowObject);
			}
		}

		private void CopyCommand(RowObject rowObject)
		{
			if (ClipboardUtil.CopyAndMinimize(rowObject.Value, true, mMainForm, Entry, Database))
			{
				mMainForm.StartClipboardCountdown();
			}
		}

		private void EditFieldCommand(RowObject rowObject)
		{
			StartCellEdit(ModelToItem(rowObject), 1);
		}

		private void ProtectFieldCommand(RowObject rowObject, bool isChecked)
		{
			rowObject.Value = new ProtectedString(isChecked, rowObject.Value.ReadUtf8());

			OnEntryModified(EventArgs.Empty);
		}

		private void PasswordGeneratorCommand(RowObject rowObject)
		{
			var passwordGenerator = new PwGeneratorForm();

			PwProfile profile = null;
			if (!rowObject.Value.IsEmpty)
			{
				profile = KeePassLib.Cryptography.PasswordGenerator.PwProfile.DeriveFromPassword(rowObject.Value);
			}
			passwordGenerator.InitEx(profile, true, false);

			if (passwordGenerator.ShowDialog() == DialogResult.OK)
			{
				// Logic copied from PwEntryForm.OnPwGenOpen (PwEntryForm.cs)
				var entropy = EntropyForm.CollectEntropyIfEnabled(passwordGenerator.SelectedProfile);
				ProtectedString newPassword;
				PwGenerator.Generate(out newPassword, passwordGenerator.SelectedProfile, entropy, KeePass.Program.PwGeneratorPool);

				Entry.Strings.Set(rowObject.FieldName, newPassword);
				rowObject.Value = newPassword;

				OnEntryModified(EventArgs.Empty);
			}

			UIUtil.DestroyForm(passwordGenerator);
		}

		private void DeleteFieldCommand(RowObject rowObject)
		{
			if (PwDefs.IsStandardField(rowObject.FieldName))
			{
				var blankValue = new ProtectedString(rowObject.Value.IsProtected, new byte[0]);
				rowObject.Value = blankValue;
			}
			else
			{
				Entry.Strings.Remove(rowObject.FieldName);

				RemoveObject(rowObject);
			}

			OnEntryModified(EventArgs.Empty);
		}

		private void AddNewCommand()
		{
			StartCellEdit(GetLastItemInDisplayOrder(), 0);
		}
		#endregion

		#region RowObject
		internal class RowObject
		{
			public static RowObject CreateInsertionRow()
			{
				return new RowObject(null, null);
			}

			public RowObject(KeyValuePair<string, ProtectedString> keyValuePair)
				: this(keyValuePair.Key, keyValuePair.Value)
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
