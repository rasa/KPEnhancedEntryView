﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using KeePass.Forms;
using KeePass.Resources;
using KeePass.UI;
using KeePass.Util;
using KeePass.Util.Spr;
using KeePassLib;
using KeePassLib.Cryptography.PasswordGenerator;
using KeePassLib.Security;
using KeePass.App.Configuration;
using KeePassLib.Utility;
using System.Threading;

namespace KPEnhancedEntryView
{
	internal class MultipleEntriesFieldsListView : FieldsListView
	{
		private readonly ProtectedString mMultipleValues = new ProtectedString(false, Properties.Resources.MultipleValues);
		private readonly ProtectedString mMultipleProtectedValues = new ProtectedString(true, Properties.Resources.MultipleValues);

		public MultipleEntriesFieldsListView()
		{
		}

		#region Entry
		private PwEntry[] mEntries;
		public PwEntry[] Entries
		{
			get { return mEntries; }
			set
			{
				mEntries = value;

				TopItemIndex = 0; // Reset scroll when actually changing entries
				SelectedIndex = -1;
				OnEntriesChanged(EventArgs.Empty);
			}
		}

		protected virtual void OnEntriesChanged(EventArgs e)
		{
			ClearObjects();

			// Perform population on separate thread
			ThreadPool.QueueUserWorkItem(Populate, mEntries);
		}

		protected override void Repopulate()
		{
			ThreadPool.QueueUserWorkItem(Populate, mEntries);
		}

		private void Populate(object state)
		{
			var entries = (PwEntry[])state;

			if (entries != null && entries.Length > 0)
			{
				var unionOfFields = new Dictionary<string, ProtectedString>();
				var fieldOrder = new List<string>(new[] { PwDefs.TitleField, PwDefs.UserNameField, PwDefs.PasswordField, PwDefs.UrlField }); // Prepopulate order with fields that should appear first. Other fields will be added in the order in which they are encountered

				var firstEntry = true;
				foreach (var entry in entries)
				{
					if (!Object.ReferenceEquals(entries, mEntries))
					{
						// Entries has changed, so abort this population
						return;
					}

					foreach (var field in entry.Strings)
					{
						if (!IsExcludedField(field.Key))
						{
							var multipleValues = field.Value.IsProtected ? mMultipleProtectedValues : mMultipleValues;

							ProtectedString existingValue;
							if (!unionOfFields.TryGetValue(field.Key, out existingValue))
							{
								if (!firstEntry || // If this isn't the first entry, then the fact that previous entries didn't have a value for this field means that it counts as multiple.
									ShouldHideValue(field.Key, field.Value)) // If it's a hidden value, then always count it as multiple without reading (and therefore unprotecting) the value
								{
									unionOfFields.Add(field.Key, multipleValues);
								}
								else
								{
									unionOfFields.Add(field.Key, field.Value);
								}
								if (!PwDefs.IsStandardField(field.Key))
								{
									fieldOrder.Add(field.Key);
								}
							}
							else
							{
								if (existingValue != mMultipleProtectedValues &&
									existingValue != mMultipleValues)
								{
									if (ShouldHideValue(field.Key, field.Value)) // If it's a hidden value, then always count it as multiple without reading (and therefore unprotecting) the value
									{
										unionOfFields[field.Key] = multipleValues;
									}
									else
									{
										System.Diagnostics.Debug.Assert(!ShouldHideValue(field.Key, field.Value) && !ShouldHideValue(field.Key, existingValue), "Unnecessary reading of in-memory protected strings");
										if (!existingValue.ReadString().Equals(field.Value.ReadString(), StringComparison.Ordinal))	// No longer consistent
										{
											// Mark it as a field with multiple values.
											unionOfFields[field.Key] = multipleValues;
										}
									}
								}
							}
						}
					}

					// Any fields which aren't on this entry now count as multiple
					var absentFields = new List<KeyValuePair<String, ProtectedString>>(unionOfFields.Count);
					foreach (var existingField in unionOfFields)
					{
						var multipleValues = existingField.Value.IsProtected ? mMultipleProtectedValues : mMultipleValues;  // Keep the same protection state, so that the right editor can be used

						if (existingField.Value != multipleValues &&
							!entry.Strings.Exists(existingField.Key))
						{
							absentFields.Add(new KeyValuePair<String, ProtectedString>(existingField.Key, multipleValues));
						}
					}
					foreach (var absentField in absentFields)
					{
						unionOfFields[absentField.Key] = absentField.Value;
					}

					firstEntry = false;
				}

				var rows = new List<RowObject>(unionOfFields.Count + 1);

				// Add all the fields, in the right order
				foreach (var fieldName in fieldOrder)
				{
					ProtectedString value;
					if (unionOfFields.TryGetValue(fieldName, out value))
					{
						rows.Add(new RowObject(fieldName, value));
					}
				}

				if (!mOptions.ReadOnly)
				{
					// Finally, an empty "add new" row
					rows.Add(RowObject.CreateInsertionRow());
				}

				BeginInvoke(new Action(delegate
				{
					if (Object.ReferenceEquals(entries, mEntries)) // Final guard against repopulation
					{
						SetRows(rows);
						AllowCreateHistoryNow = true; // Whenever the entries are replaced, it counts as not having been edited yet (so the first edit is always given a history backup)
					}
				}));
			}
		}
		#endregion

		protected override void OnIsHyperlink(IsHyperlinkEventArgs e)
		{
			if (IsMultiValuedField((RowObject)e.Model))
			{
				e.Url = null;
			}
			else
			{
				base.OnIsHyperlink(e);
			}
		}

		protected override void OnFormatCell(FormatCellEventArgs args)
		{
			var rowObject = args.Model as RowObject;
			if (rowObject != null && IsMultiValuedField(rowObject))
			{
				// Format multiple values in grey and italic
				args.SubItem.Font = mItalicFont;
				args.SubItem.ForeColor = SystemColors.GrayText;
			}

			base.OnFormatCell(args);
		}

		#region Cell Editing

		public bool IsMultiValuedField(RowObject rowObject)
		{
			return rowObject.Value == mMultipleValues ||
					rowObject.Value == mMultipleProtectedValues;
		}

		protected override Control GetCellEditor(OLVListItem item, int subItemIndex)
		{
			var control = base.GetCellEditor(item, subItemIndex);

			// If it's a multi-valued field, blank the value first.
			if (IsMultiValuedField((RowObject)item.RowObject))
			{
				var fieldEditor = control as UnprotectedFieldEditor;
				if (fieldEditor != null)
				{
					fieldEditor.Value = ProtectedString.Empty;
				}
				else
				{
					var protectedFieldEditor = control as ProtectedFieldEditor;
					if (protectedFieldEditor != null)
					{
						protectedFieldEditor.Value = ProtectedString.Empty;
					}
				}
			}

			return control;
		}

		protected override FieldNameEditor GetFieldNameEditor(RowObject rowObject)
		{
			return new FieldNameEditor(Entries, mOptions) { Text = rowObject.FieldName };
		}

		protected override void ValidateFieldName(CellEditEventArgs e, string newValue)
		{
			base.ValidateFieldName(e, newValue);

			if (e.Cancel)
			{
				return;
			}

			if (PwDefs.GetStandardFields().Any(standardField => standardField.Equals(newValue, StringComparison.OrdinalIgnoreCase)))
			{
				ReportValidationFailure(e.Control, KPRes.FieldNameInvalid);
				e.Cancel = true;
				return;
			}

			var rowObject = (RowObject)e.RowObject;
			IEnumerable<PwEntry> entriesWithField;
			if (rowObject.IsInsertionRow)
			{
				entriesWithField = Entries;
			}
			else
			{
				entriesWithField = Entries.Where(entry => entry.Strings.Exists(rowObject.FieldName));
			}

			// Disallow the field name if it already exists on any of the entries which have that field
			foreach (var entry in entriesWithField)
			{
				if (entry.Strings.Exists(newValue) ||
					(!newValue.Equals(rowObject.FieldName, StringComparison.OrdinalIgnoreCase) && entry.Strings.GetKeys().Any(key => key.Equals(newValue, StringComparison.OrdinalIgnoreCase))))
				{
					ReportValidationFailure(e.Control, KPRes.FieldNameExistsAlready);
					e.Cancel = true;
					return;
				}
			}
		}

		protected override void SetFieldValueInternal(RowObject rowObject, ProtectedString newValue)
		{
			var allEntries = Entries.ToArray();
			if (!IsMultiValuedField(rowObject) || // No need to confirm to change the value of a field that isn't multi-valued
				ConfirmOperationOnAllEntries(String.Format(Properties.Resources.MultipleEntryFieldSetValueQuestion, rowObject.DisplayName), Properties.Resources.MultpleEntryFieldSetValueCommand, allEntries))
			{
				var createBackups = AllowCreateHistoryNow;
				foreach (var entry in allEntries)
				{
					if (createBackups)
					{
						entry.CreateBackup(Database);
					}

					entry.Strings.Set(rowObject.FieldName, newValue); // ProtectedStrings are immutable, so OK to assign the same one to all entries
				}
				rowObject.Value = newValue;

				OnModified(new EntryModifiedEventArgs(allEntries));
			}
		}

		protected override void SetFieldNameInternal(RowObject rowObject, string newName)
		{
			ProtectedString blankValue = null;
			IEnumerable<PwEntry> entriesWithField;

			if (rowObject.IsInsertionRow)
			{
				// Check if this should be a protected string
				var isProtected = false; // Default to not protected
				var fieldOnOtherEntry = (from entry in Entries
										 from groupEntry in entry.ParentGroup.Entries
											select groupEntry.Strings.Get(newName)).FirstOrDefault();

				if (fieldOnOtherEntry != null)
				{
					isProtected = fieldOnOtherEntry.IsProtected;
				}

				blankValue = new ProtectedString(isProtected, new byte[0]);
				rowObject.Value = blankValue;

				entriesWithField = Entries;
			}
			else
			{
				entriesWithField = Entries.Where(entry => entry.Strings.Exists(rowObject.FieldName));
			}

			var createBackups = AllowCreateHistoryNow;
			foreach (var entry in entriesWithField)
			{
				if (createBackups)
				{
					entry.CreateBackup(Database);
				}

				ProtectedString value;
				if (rowObject.IsInsertionRow)
				{
					value = blankValue;
				}
				else
				{
					// Ensure value is up to date
					value = entry.Strings.Get(rowObject.FieldName);

					// Remove existing value
					entry.Strings.Remove(rowObject.FieldName);
				}

				entry.Strings.Set(newName, value);
			}

			rowObject.FieldName = newName;

			OnModified(new EntryModifiedEventArgs(entriesWithField));
		}
		#endregion

		#region Commands
		protected override void CopyCommand(RowObject rowObject)
		{
			if (!IsMultiValuedField(rowObject))
			{
				CopyField(Entries[0], rowObject);
			}
		}

		protected override void AutoTypeCommand(RowObject rowObject)
		{
			if (!IsMultiValuedField(rowObject))
			{
				AutoTypeField(Entries[0], rowObject.FieldName);
			}
		}

		protected override void DeleteFieldCommand(RowObject rowObject)
		{
			var isStandardField = PwDefs.IsStandardField(rowObject.FieldName);
			var entriesWithField = Entries.Where(entry => entry.Strings.Exists(rowObject.FieldName)).ToArray();
			if (ConfirmOperationOnAllEntries(String.Format(Properties.Resources.MultipleEntryFieldDeleteQuestion, rowObject.DisplayName), KPRes.Delete, entriesWithField))
			{
				var blankValue = new ProtectedString(rowObject.Value.IsProtected, new byte[0]); // ProtectedStrings are immutable, so OK to assign the same one to all entries

				var createBackups = AllowCreateHistoryNow;
				foreach (var entry in entriesWithField)
				{
					if (createBackups)
					{
						entry.CreateBackup(Database);
					}

					if (isStandardField)
					{
						entry.Strings.Set(rowObject.FieldName, blankValue);
					}
					else
					{
						entry.Strings.Remove(rowObject.FieldName);
					}
				}

				if (isStandardField)
				{
					rowObject.Value = blankValue;
					RefreshObject(rowObject);
				}
				else
				{
					RemoveObject(rowObject);
				}

				OnModified(new EntryModifiedEventArgs(entriesWithField));
			}
		}

		protected override void ProtectFieldCommand(RowObject rowObject, bool isChecked)
		{
			var entriesWithFieldToChange = Entries.Where(entry =>
			{
				var property = entry.Strings.Get(rowObject.FieldName);
				return property != null && property.IsProtected != isChecked;
			}).ToArray();
			if (ConfirmOperationOnAllEntries(String.Format(isChecked ? Properties.Resources.MultipleEntryFieldProtectQuestion : Properties.Resources.MultipleEntryFieldUnprotectQuestion, rowObject.DisplayName), Properties.Resources.MultipleEntryFieldProtectCommand, entriesWithFieldToChange))
			{
				var createBackups = AllowCreateHistoryNow;
				foreach (var entry in entriesWithFieldToChange)
				{
					entry.Strings.Set(rowObject.FieldName, entry.Strings.Get(rowObject.FieldName).WithProtection(isChecked));
				}

				OnModified(new EntryModifiedEventArgs(entriesWithFieldToChange));
				Repopulate();
			}
		}

		private bool ConfirmOperationOnAllEntries(string question, string command, PwEntry[] entries)
		{
			var dlg = new VistaTaskDialog
			{
				CommandLinks = false,
				Content = EntryUtil.CreateSummaryList(null, entries),
				MainInstruction = question,
				WindowTitle = PwDefs.ShortProductName
			};
			dlg.SetIcon(VtdCustomIcon.Question);
			dlg.AddButton((int)DialogResult.OK, command, null);
			dlg.AddButton((int)DialogResult.Cancel, KPRes.Cancel, null);

			if (dlg.ShowDialog())
			{
				return dlg.Result == (int)DialogResult.OK;
			}
			else
			{
				return MessageService.AskYesNo(question);
			}
		}
		#endregion

		#region Field dereferencing
		protected override string GetDragValue(RowObject rowObject)
		{
			if (IsMultiValuedField(rowObject))
			{
				return null;
			}

			return base.GetDragValue(rowObject);
		}

		protected override string GetDisplayValue(ProtectedString value, bool revealValues, SprCompileFlags compileFlags)
		{
			return SprEngine.Compile(value.ReadString(), new SprContext(null, Database, compileFlags) { ForcePlainTextPasswords = revealValues });
		}
		#endregion
	}
}
