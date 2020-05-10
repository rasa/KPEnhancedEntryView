using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KeePass.App;
using KeePass.Plugins;

namespace KPEnhancedEntryView
{
	public class Options
	{
		private const string OptionsConfigRoot = "KPEnhancedEntryView.";
		public static class OptionName
		{
			public const string HideEmptyFields = "HideEmptyFields";
			public const string ReadOnly = "ReadOnly";
			public const string FieldNotesSplit = "FieldNotesSplit";
			public const string NotesAttachmentsSplit = "NotesAttachmentsSplit";
		}

		private readonly IPluginHost mHost;

		public Options(IPluginHost host)
		{
			mHost = host;
		}

		public ToolStripMenuItem CreateMenu()
		{
			var root = new ToolStripMenuItem
			{
				Text = Properties.Resources.OptionsMenuItem
			};

			var hideEmptyStandardFields = new ToolStripMenuItem
			{
				Text = Properties.Resources.HideEmptyStandardFieldsOptionMenuItem,
				CheckOnClick = true,
			};

			hideEmptyStandardFields.Click += (o, e) => SetOption(OptionName.HideEmptyFields, ((ToolStripMenuItem)o).Checked);
			root.DropDownItems.Add(hideEmptyStandardFields);

			var readOnly = new ToolStripMenuItem
			{
				Text = Properties.Resources.ReadOnlyOptionMenuItem,
				CheckOnClick = true,
			};

			readOnly.Click += (o, e) => SetOption(OptionName.ReadOnly, ((ToolStripMenuItem)o).Checked);
			root.DropDownItems.Add(readOnly);

			var reveal = new ToolStripMenuItem
			{
				Text = Properties.Resources.RevealMenuItem,
				Image = Properties.Resources.Reveal,
				ImageScaling = ToolStripItemImageScaling.None,
				ShortcutKeys = Keys.F9
			};

			reveal.Click += (o, e) => OnRevealProtectedFields();
			root.DropDownItems.Add(reveal);

			root.DropDownOpening += delegate(object sender, EventArgs args)
			{
				readOnly.Checked = ReadOnly;
				hideEmptyStandardFields.Checked = HideEmptyFields;
			};

			return root;
		}

		// Reveal is one-shot rather than being persisted
		public event EventHandler RevealProtectedFields;

		private void OnRevealProtectedFields()
		{
			if (AppPolicy.Try(AppPolicyId.UnhidePasswords))
			{
				var handler = RevealProtectedFields;
				if (handler != null)
				{
					handler(this, EventArgs.Empty);
				}
			}
		}

		public event EventHandler<OptionChangedEventArgs> OptionChanged;

		public class OptionChangedEventArgs : EventArgs
		{
			private readonly string mOptionName;

			public OptionChangedEventArgs(string optionName)
			{
				mOptionName = optionName;
			}

			public string OptionName { get { return mOptionName; } }
		}

		private void SetOption(string option, bool value)
		{
			mHost.CustomConfig.SetBool(OptionsConfigRoot + option, value);
			OnOptionChanged(option);
		}

		private void SetOption(string option, long value)
		{
			mHost.CustomConfig.SetLong(OptionsConfigRoot + option, value);
			OnOptionChanged(option);
		}

		private void OnOptionChanged(string option)
		{
			var temp = OptionChanged;
			if (temp != null)
			{
				temp(this, new OptionChangedEventArgs(option));
			}
		}

		private bool GetOption(string option, bool defaultValue)
		{
			return mHost.CustomConfig.GetBool(OptionsConfigRoot + option, defaultValue);
		}

		private long GetOption(string option, long defaultValue)
		{
			return mHost.CustomConfig.GetLong(OptionsConfigRoot + option, defaultValue);
		}

		#region Options
		public bool HideEmptyFields { get { return GetOption(OptionName.HideEmptyFields, false); } }

		public bool ReadOnly
		{
			get { return GetOption(OptionName.ReadOnly, false); }
			set { SetOption(OptionName.ReadOnly, value); }
		}

		public long FieldsNotesSplitPosition
		{
			get { return GetOption(OptionName.FieldNotesSplit, -1L); }
			set { SetOption(OptionName.FieldNotesSplit, value);}
		}

		public long NotesAttachmentsSplitPosition
		{
			get { return GetOption(OptionName.NotesAttachmentsSplit, -1L); }
			set { SetOption(OptionName.NotesAttachmentsSplit, value); }
		}
		#endregion
	}
}
