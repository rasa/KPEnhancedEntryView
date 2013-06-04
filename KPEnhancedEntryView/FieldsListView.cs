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
using KeePass.App.Configuration;

namespace KPEnhancedEntryView
{
	internal abstract class FieldsListView : ObjectListView
	{
		protected readonly BrightIdeasSoftware.OLVColumn mFieldNames;
		protected readonly BrightIdeasSoftware.OLVColumn mFieldValues;

		protected MainForm mMainForm;
		protected Options mOptions;

		public FieldsListView() 
		{
			CellEditActivation = CellEditActivateMode.DoubleClick;
			CellEditTabChangesRows = true;
			CopySelectionOnControlC = false;
			FullRowSelect = true;
			MultiSelect = false;
			HeaderStyle = ColumnHeaderStyle.Nonclickable;
			ShowGroups = false;
			UseCellFormatEvents = true;
			UseHyperlinks = true;
			DragSource = new FieldValueDragSource();

			if (KeePass.Program.Config.MainWindow.EntryListAlternatingBgColors)
			{
				AlternateRowBackColor = UIUtil.GetAlternateColor(BackColor);
				UseAlternatingBackColors = true;
			}

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
		}

		// Disallow setting of IsSimpleDragSource (as it breaks the file dragging, and is sometimes automatically set by the designer for some reason)
		public override bool IsSimpleDragSource
		{
			get { return false; }
			set { }
		}

		/// <summary>
		/// Ensure this is called before using the control
		/// </summary>
		public void Initialise(MainForm mainForm, Options options)
		{
			mMainForm = mainForm;
			mOptions = options;
		}

		protected PwDatabase Database { get { return mMainForm.ActiveDatabase; } }

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

		public void RefreshItems()
		{
			foreach (OLVListItem item in Items)
			{
				RefreshItem(item);
			}
		}

		protected void SetRows(IEnumerable<RowObject> rows)
		{
			BeginUpdate();
			SetObjects(rows);
			mFieldNames.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
			mFieldNames.Width += 10; // Give it a bit of a margin to make it look better
			EndUpdate();
		}

		protected bool IsExcludedField(string fieldName)
		{
			return fieldName == "KPRPC JSON";			 // Exclude KeyFox's custom field (not intended to be user visible or directly editable)
		}

		#region EntryModified event
		public event EventHandler Modified;
		protected virtual void OnModified(EventArgs e)
		{
			var temp = Modified;
			if (temp != null)
			{
				temp(this, e);
			}
		}
		#endregion

		#region Font and formatting

		protected Font mBoldFont = null;
		protected Font mItalicFont = null;

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);

			if (mBoldFont != null) mBoldFont.Dispose();
			if (mItalicFont != null) mItalicFont.Dispose();

			mBoldFont = new Font(Font, FontStyle.Bold);
			mItalicFont = new Font(Font, FontStyle.Italic);

			// This doesn't inherit automatically, for some reason
			HyperlinkStyle.Over.Font = Font;

			RefreshItems();
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

			var rowObject = (RowObject)e.Model;

			if (e.Column == mFieldValues)
			{
				if (rowObject.Value != null && !rowObject.Value.IsProtected)
				{
					var value = rowObject.Value.ReadString();
					Uri uri;
					if (rowObject.FieldName == PwDefs.UrlField || // Assume a URL if in the URL field, even if it doesn't look like one
						Uri.TryCreate(value, UriKind.Absolute, out uri))
					{
						e.Url = value;
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

		protected override void SetControlValue(Control control, object value, string stringValue)
		{
			if (control is ProtectedFieldEditor ||
				control is UnprotectedFieldEditor ||
				control is FieldNameEditor)
			{
				// These controls will already have had their values set
				return;
			}
			base.SetControlValue(control, value, stringValue);
		}

		protected override Control GetCellEditor(OLVListItem item, int subItemIndex)
		{
			var column = GetColumn(subItemIndex);
			var rowObject = item.RowObject as RowObject;
			
			if (rowObject != null)
			{
				if (column == mFieldValues && !rowObject.IsInsertionRow)
				{
					if (rowObject.Value.IsProtected)
					{
						return new ProtectedFieldEditor
						{
							Value = rowObject.Value,
							HidePassword = rowObject.HideValue
						};
					}
					else
					{
						return new UnprotectedFieldEditor
						{
							Value = rowObject.Value
						};
					}
				}
				else if (column == mFieldNames)
				{
					return GetFieldNameEditor(rowObject);
				}
			}

			return base.GetCellEditor(item, subItemIndex);
		}

		protected abstract FieldNameEditor GetFieldNameEditor(RowObject rowObject);

		protected override void OnCellEditorValidating(CellEditEventArgs e)
		{
			base.OnCellEditorValidating(e);
		
			if (e.Column == mFieldNames)
			{
				var newValue = ((FieldNameEditor)e.Control).FieldName;
				var rowObject = (RowObject)e.RowObject;

				if (newValue != rowObject.FieldName)
				{
					ValidateFieldName(e, newValue);
				}
			}
		}

		protected virtual void ValidateFieldName(CellEditEventArgs e, string newValue)
		{
			// Logic copied from EditStringForm.ValidateStringName (EditStringForm.cs)

			if (newValue.IndexOfAny(new[] { '{', '}' }) >= 0)
			{
				ReportValidationFailure(e.Control, KPRes.FieldNameInvalid);
				e.Cancel = true;
				return;
			}
			if (String.IsNullOrEmpty(newValue))
			{
				ReportValidationFailure(e.Control, KPRes.FieldNamePrompt);
				e.Cancel = true;
				return;
			}
		}

		protected override void OnCellEditFinishing(CellEditEventArgs e)
		{
			base.OnCellEditFinishing(e);
		
			if (e.Column == mFieldNames)
			{
				e.NewValue = ((FieldNameEditor)e.Control).FieldName;
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

				SetFieldValueInternal(rowObject, newProtectedString);
			}
		}

		protected abstract void SetFieldValueInternal(RowObject rowObject, ProtectedString newValue);

		private void SetFieldName(Object model, Object newValue)
		{
			var rowObject = (RowObject)model;
			var newName = (string)newValue;

			if (rowObject.IsInsertionRow)
			{
				// Create a new insertion row to replace this one
				AddObject(RowObject.CreateInsertionRow());
			}

			SetFieldNameInternal(rowObject, newName);
		}

		protected abstract void SetFieldNameInternal(RowObject rowObject, string newName);
		#endregion

		#region Validation Failure Reporting
		public ValidationFailureReporter ValidationFailureReporter { get; set; }

		protected void ReportValidationFailure(Control control, string message)
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
						if (rowObject.IsInsertionRow)
						{
							// For the insertion row only, start editing the name on Enter
							AddNewCommand();
						}
						else
						{
							EditFieldCommand(rowObject);
						}
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

		public void DoOpenUrl() { DoCommandOnSelected(OpenURLCommand); }
		public void DoCopy() { DoCommandOnSelected(CopyCommand); }
		public void DoEditField() { DoCommandOnSelected(EditFieldCommand); }
		public void DoSetProtected(bool value) { DoCommandOnSelected(rowObject => ProtectFieldCommand(rowObject, value)); }
		public void DoPasswordGenerator() { DoCommandOnSelected(PasswordGeneratorCommand); }
		public void DoDeleteField() { DoCommandOnSelected(DeleteFieldCommand); }
		public void DoAddNew() { AddNewCommand(); }

		private void DoCommandOnSelected(Action<RowObject> command)
		{
			var rowObject = (RowObject)SelectedObject;

			if (rowObject != null && !rowObject.IsInsertionRow)
			{
				command(rowObject);
			}
		}

		private void OpenURLCommand(RowObject rowObject)
		{
			OnHyperlinkClicked(new HyperlinkClickedEventArgs { Url = rowObject.Value.ReadString() });
		}

		protected abstract void CopyCommand(RowObject rowObject);

		private void EditFieldCommand(RowObject rowObject)
		{
			StartCellEdit(ModelToItem(rowObject), 1);
		}

		private void ProtectFieldCommand(RowObject rowObject, bool isChecked)
		{
			SetFieldValue(rowObject, rowObject.Value.WithProtection(isChecked));
			RefreshObject(rowObject);
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

				SetFieldValue(rowObject, newPassword);
				
				RefreshObject(rowObject);
			}

			UIUtil.DestroyForm(passwordGenerator);
		}

		protected abstract void DeleteFieldCommand(RowObject rowObject);

		private void AddNewCommand()
		{
			StartCellEdit(GetLastItemInDisplayOrder(), 0);
		}
		#endregion

		#region Drag and drop
		protected virtual string GetDragValue(RowObject rowObject)
		{
			if (rowObject.Value != null)
			{
				return rowObject.Value.ReadString();
			}
			return null;
		}

		internal class FieldValueDragSource : IDragSource
		{
			public object StartDrag(ObjectListView olv, System.Windows.Forms.MouseButtons button, OLVListItem item)
			{
				if (button == MouseButtons.Left)
				{
					string dragText = null;
					if (!KeePass.App.AppPolicy.Current.DragDrop)
					{
						dragText = KeePass.App.AppPolicy.RequiredPolicyMessage(KeePass.App.AppPolicyId.DragDrop);
					}
					else
					{
						var rowObject = item.RowObject as RowObject;
						var fieldsListView = olv as FieldsListView;
						if (rowObject != null && fieldsListView != null)
						{
							dragText = fieldsListView.GetDragValue(rowObject);
						}
					}

					if (!String.IsNullOrEmpty(dragText))
					{
						var dataObject = new DataObject();
						dataObject.SetText(dragText);

						return dataObject;
					}
				}

				return null;
			}

			public System.Windows.Forms.DragDropEffects GetAllowedEffects(object dragObject)
			{
				return DragDropEffects.Copy;
			}

			public void EndDrag(object dragObject, System.Windows.Forms.DragDropEffects effect)
			{
			}
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

					if (HideValue)
					{
						return PwDefs.HiddenPassword;
					}
					else
					{
						return Value.ReadString();
					}
				}
			}

			public bool HideValue
			{
				get
				{
					return ShouldHideValue(FieldName, Value);
				}
			}
			
			public bool IsInsertionRow { get { return FieldName == null; } }
		}

		protected static bool ShouldHideValue(string fieldName, ProtectedString value)
		{
			var columnType = GetColumnType(fieldName);
			return (columnType == AceColumnType.CustomString && KeePass.Program.Config.MainWindow.ShouldHideCustomString(fieldName, value)) ||
									KeePass.Program.Config.MainWindow.IsColumnHidden(columnType);
		}

		private static AceColumnType GetColumnType(string fieldName)
		{
			switch (fieldName)
			{
				case PwDefs.TitleField:
					return AceColumnType.Title;
				case PwDefs.UserNameField:
					return AceColumnType.UserName;
				case PwDefs.PasswordField:
					return AceColumnType.Password;
				case PwDefs.UrlField:
					return AceColumnType.Url;
				default:
					return AceColumnType.CustomString;
			}
		}
		#endregion
	}
}
