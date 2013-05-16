using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KeePass.Plugins;

namespace KPEnhancedEntryView
{
	public class Options
	{
		private const string OptionsConfigRoot = "KPEnhancedEntryView.";
		public static class OptionName
		{
			public const string HideEmptyFields = "HideEmptyFields";
		}

		private readonly IPluginHost mHost;
		private readonly ToolStripMenuItem mMenu;

		public Options(IPluginHost host)
		{
			mHost = host;
			mMenu = CreateMenu();
		}

		public ToolStripMenuItem Menu { get { return mMenu; } }

		private ToolStripMenuItem CreateMenu()
		{
			var root = new ToolStripMenuItem
			{
				Text = Properties.Resources.OptionsMenuItem
			};

			var hideEmptyStandardFields = new ToolStripMenuItem
			{
				Text = Properties.Resources.HideEmptyStandardFieldsOptionMenuItem,
				CheckOnClick = true,
				Checked = HideEmptyFields
			};

			hideEmptyStandardFields.CheckedChanged += (o, e) => SetOption(OptionName.HideEmptyFields, ((ToolStripMenuItem)o).Checked);
			root.DropDownItems.Add(hideEmptyStandardFields);

			return root;
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

		#region Options
		public bool HideEmptyFields { get { return GetOption(OptionName.HideEmptyFields, false); } }
		#endregion
	}
}
