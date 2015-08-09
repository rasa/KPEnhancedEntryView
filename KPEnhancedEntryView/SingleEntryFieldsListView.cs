using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BrightIdeasSoftware;
using KeePass.Resources;
using KeePass.Util;
using KeePass.Util.Spr;
using KeePassLib;
using KeePassLib.Security;
using KeePassLib.Utility;

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

			AllowCreateHistoryNow = true; // Whenever the entry is replaced, it counts as not having been edited yet (so the first edit is always given a history backup)
		}

		private void AddFieldIfNotEmpty(List<RowObject> rows, string fieldName)
		{
			var value = Entry.Strings.Get(fieldName);
			if (!mOptions.HideEmptyFields || (value != null && !value.IsEmpty))
			{
				rows.Add(new RowObject(fieldName, value ?? ProtectedString.Empty));
			}
		}

		protected override void Repopulate()
		{
			OnEntryChanged(EventArgs.Empty);
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

			if (PwDefs.GetStandardFields().Any(standardField => standardField.Equals(newValue, StringComparison.OrdinalIgnoreCase)))
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

			if (Entry.Strings.Exists(newValue) ||
				(!newValue.Equals((string)e.Value, StringComparison.OrdinalIgnoreCase) && Entry.Strings.GetKeys().Any(key => key.Equals(newValue, StringComparison.OrdinalIgnoreCase))))
			{
				ReportValidationFailure(e.Control, KPRes.FieldNameExistsAlready);
				e.Cancel = true;
				return;
			}
		}

		protected override void SetFieldValueInternal(RowObject rowObject, ProtectedString newValue)
		{
			if (newValue.ReadString() != rowObject.Value.ReadString() ||
				newValue.IsProtected != rowObject.Value.IsProtected)
			{
				CreateHistoryEntry();

				Entry.Strings.Set(rowObject.FieldName, newValue);
				rowObject.Value = newValue;

				OnModified(EventArgs.Empty);
			}
		}

		protected override void SetFieldNameInternal(RowObject rowObject, string newName)
		{
			if (newName != rowObject.FieldName)
			{
				CreateHistoryEntry();

				if (rowObject.IsInsertionRow)
				{
					// Coerce to standard field value casing
					newName = PwDefs.GetStandardFields().FirstOrDefault(standardField => standardField.Equals(newName, StringComparison.OrdinalIgnoreCase)) ?? newName;
			
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
		}
		#endregion

		#region Commands
		protected override void CopyCommand(RowObject rowObject)
		{
			if (ClipboardUtil.CopyAndMinimize(rowObject.Value, true, mMainForm, Entry, Database))
			{
				mMainForm.StartClipboardCountdown();
			}
			Repopulate();
		}

		protected override void AutoTypeCommand(RowObject rowObject)
		{
			AutoTypeField(Entry, rowObject.FieldName);
		}

		protected override void DeleteFieldCommand(RowObject rowObject)
		{
			CreateHistoryEntry();

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
		protected override string GetDisplayValue(ProtectedString value, bool revealValues, SprCompileFlags compileFlags)
		{
			return SprEngine.Compile(value.ReadString(), new SprContext(Entry, Database, compileFlags) { ForcePlainTextPasswords = revealValues });
		}
		#endregion

		#region History Backup
		private void CreateHistoryEntry()
		{
			if (AllowCreateHistoryNow)
			{
				Entry.CreateBackup(Database);
			}
		}
		#endregion
	}
}
