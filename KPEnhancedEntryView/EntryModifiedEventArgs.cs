using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using BrightIdeasSoftware;
using KeePass;
using KeePass.Forms;
using KeePass.Resources;
using KeePass.UI;
using KeePass.Util;
using KeePassLib;
using KeePassLib.Security;
using KeePassLib.Utility;
using System.Collections.Generic;
using KPEnhancedEntryView.Properties;

namespace KPEnhancedEntryView
{

	public class EntryModifiedEventArgs : EventArgs
	{
		private readonly PwEntry[] mEntries;

		public EntryModifiedEventArgs(IEnumerable<PwEntry> entries)
		{
			mEntries = entries.ToArray();
		}

		public EntryModifiedEventArgs(PwEntry entry)
		{
			mEntries = new[] { entry };
		}

		public IEnumerable<PwEntry> Entries
		{
			get { return mEntries; }
		}
	}
}
