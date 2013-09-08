using System;
using System.Collections.Generic;
using System.Linq;
using BrightIdeasSoftware;
using KeePass.Resources;
using KeePass.Util;
using KeePass.Util.Spr;
using KeePassLib;
using KeePassLib.Security;

namespace KPEnhancedEntryView
{
	internal class SingleEntryFieldsListView : FieldsListView
	{
		public SingleEntryFieldsListView() 
		{
		}

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

				// First, the standard fields, where present, in the standard order
				AddFieldIfNotEmpty(rows, PwDefs.TitleField);
				AddFieldIfNotEmpty(rows, PwDefs.UserNameField);
				AddFieldIfNotEmpty(rows, PwDefs.PasswordField);
				AddFieldIfNotEmpty(rows, PwDefs.UrlField);

				// Then, all custom strings
				rows.AddRange(from kvp in Entry.Strings where !PwDefs.IsStandardField(kvp.Key) && !IsExcludedField(kvp.Key) select new RowObject(kvp));

				// Finally, an empty "add new" row
				rows.Add(RowObject.CreateInsertionRow());

				SetRows(rows);
			}
		}

		private void AddFieldIfNotEmpty(List<RowObject> rows, string fieldName)
		{
			var value = Entry.Strings.Get(fieldName);
			if (!mOptions.HideEmptyFields || (value != null && !value.IsEmpty))
			{
				rows.Add(new RowObject(fieldName, value ?? ProtectedString.Empty));
			}
		}
		#endregion

		#region Cell Editing

		protected override FieldNameEditor GetFieldNameEditor(RowObject rowObject)
		{
			return new FieldNameEditor(Entry, mOptions) { Text = rowObject.FieldName };
		}

		protected override void ValidateFieldName(CellEditEventArgs e, string newValue)
		{
			base.ValidateFieldName(e, newValue);

			if (PwDefs.IsStandardField(newValue))
			{
				// Allow if the standard field on the entry is currently blank and hidden
				if (mOptions.HideEmptyFields && Entry.Strings.GetSafe(newValue).IsEmpty)
				{
					return;
				}

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

		protected override void SetFieldValueInternal(RowObject rowObject, ProtectedString newValue)
		{
			Entry.CreateBackup(Database);

			Entry.Strings.Set(rowObject.FieldName, newValue);
			rowObject.Value = newValue;

			OnModified(EventArgs.Empty);
		}

		protected override void SetFieldNameInternal(RowObject rowObject, string newName)
		{
			Entry.CreateBackup(Database);

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
			}
			else
			{
				// Ensure value is up to date
				rowObject.Value = Entry.Strings.Get(rowObject.FieldName);

				// Remove existing value
				Entry.Strings.Remove(rowObject.FieldName);
			}

			Entry.Strings.Set(newName, rowObject.Value);
			OnModified(EventArgs.Empty);

			rowObject.FieldName = newName;
		}
		#endregion

		#region Commands
		protected override void CopyCommand(RowObject rowObject)
		{
			if (ClipboardUtil.CopyAndMinimize(rowObject.Value, true, mMainForm, Entry, Database))
			{
				mMainForm.StartClipboardCountdown();
			}
		}

		protected override void DeleteFieldCommand(RowObject rowObject)
		{
			Entry.CreateBackup(Database);

			if (PwDefs.IsStandardField(rowObject.FieldName))
			{
				var blankValue = new ProtectedString(rowObject.Value.IsProtected, new byte[0]);

				Entry.Strings.Set(rowObject.FieldName, blankValue);

				if (mOptions.HideEmptyFields)
				{
					RemoveObject(rowObject);
				}
				else
				{
					rowObject.Value = blankValue;
					RefreshObject(rowObject);
				}
			}
			else
			{
				Entry.Strings.Remove(rowObject.FieldName);
				RemoveObject(rowObject);
			}

			OnModified(EventArgs.Empty);
		}

		#endregion

		#region Field dereferencing
		protected override string GetDisplayValue(ProtectedString value)
		{
			return SprEngine.Compile(value.ReadString(), new SprContext(Entry, Database, SprCompileFlags.All));
		}
		#endregion
	}
}
