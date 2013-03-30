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

namespace KPEnhancedEntryView
{
	public partial class EntryView : UserControl
	{
		private static KeyValuePair<string, ProtectedString> sEmptyRow = new KeyValuePair<string, ProtectedString>(Properties.Resources.NewFieldRowName, null);

		private PwEntry mEntry;

		private Font mBoldFont = null;
		private Font mItalicFont = null;

		public EntryView()
		{
			InitializeComponent();

			mFieldValues.AspectGetter = new AspectGetterDelegate(GetFieldValue);
			ObjectListView.EditorRegistry.RegisterFirstChance(GetCellEditor);

			if (KeePass.Program.Config.MainWindow.EntryListAlternatingBgColors)
			{
				mFieldsGrid.AlternateRowBackColor = UIUtil.GetAlternateColor(mFieldsGrid.BackColor);
				mFieldsGrid.UseAlternatingBackColors = true;
			}
		}

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);

			if (mBoldFont != null) mBoldFont.Dispose();
			if (mItalicFont != null) mItalicFont.Dispose();

			mBoldFont = new Font(Font, FontStyle.Bold);
			mItalicFont = new Font(Font, FontStyle.Italic);
		}

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
			}
			else
			{
				mFieldsGrid.SetObjects(Entry.Strings.Concat(Enumerable.Repeat(sEmptyRow, 1))); // Add one empty row for adding a new value
				mFieldNames.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
				mFieldNames.Width += 10; // Give it a bit of a margin to make it look better
			}
		}

		private static object GetFieldValue(object rowObject)
		{
			var protectedString = GetProtectedString(rowObject);
			if (protectedString != null)
			{
				if (protectedString.IsProtected)
				{
					return PwDefs.HiddenPassword;
				}
				else
				{
					return protectedString.ReadString();
				}
			}

			return null;
		}

		private static ProtectedString GetProtectedString(object rowObject)
		{
			var protectedStringEntry = rowObject as Nullable<KeyValuePair<string, ProtectedString>>;
			if (protectedStringEntry.HasValue)
			{
				return protectedStringEntry.Value.Value;
			}

			return null;
		}

		private void mFieldsGrid_FormatCell(object sender, FormatCellEventArgs e)
		{
			if (e.Column == mFieldNames)
			{
				if (sEmptyRow.Equals(e.Model))
				{
					e.SubItem.Font = mItalicFont;
				}
				else
				{
					e.SubItem.Font = mBoldFont;
				}
			}
		}

		public Control GetCellEditor(Object model, OLVColumn column, Object value)
		{
			if (column == mFieldValues)
			{
				var protectedString = GetProtectedString(model);

				if (protectedString != null && protectedString.IsProtected)
				{
					return new ProtectedFieldEditor
					{
						Value = protectedString,
						HidePassword = true
					};
				}
			}

			return null;
		}
	}
}
