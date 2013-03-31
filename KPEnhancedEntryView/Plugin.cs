using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeePass.Plugins;
using System.Windows.Forms;
using KeePassLib;
using System.Diagnostics;

namespace KPEnhancedEntryView
{
	public sealed class KPEnhancedEntryViewExt : Plugin
	{
		private IPluginHost mHost;
		private EntryView mEntryView;
		private RichTextBox mOriginalEntryView;

		public override bool Initialize(IPluginHost host)
		{
			if (host == null) return false;

			// Ensure terminate before initialise, in unlikely case of double initialisation
			Terminate();

			mHost = host;

			mOriginalEntryView = FindControl<RichTextBox>("m_richEntryView");
			var entryViewContainer = mOriginalEntryView.Parent;
			if (mOriginalEntryView == null || entryViewContainer == null)
			{
				Debug.Fail("Couldn't find existing entry view to replace");
				mHost = null;
				return false;
			}

			// Replace existing entry view with new one
			mEntryView = new EntryView(mHost.Database)
			{
				Name = "m_KPEnhancedEntryView",
				Dock = DockStyle.Fill,
			};

			entryViewContainer.Controls.Add(mEntryView);

			// Font is assigned, not inherited. So assign here too, and follow any changes
			mOriginalEntryView.FontChanged += mOriginalEntryView_FontChanged;
			mOriginalEntryView_FontChanged(null, EventArgs.Empty);

			
#if DEBUG
			// While debugging, show the old entry view too, for comparison purposes
			mOriginalEntryView.Dock = DockStyle.Bottom;
			mOriginalEntryView.SendToBack();
#else
			mOriginalEntryView.Enabled = false; // Prevent attempts to give this control focus
			entryViewContainer.Controls.Remove(mOriginalEntryView);
#endif

			// Hook UIStateUpdated to watch for current entry changing.
			mHost.MainWindow.UIStateUpdated += this.OnUIStateUpdated;

			return true;
		}

		private void mOriginalEntryView_FontChanged(object sender, EventArgs e)
		{
			//mEntryView.Font = new System.Drawing.Font(mOriginalEntryView.Font, System.Drawing.FontStyle.Strikeout);
			mEntryView.Font = mOriginalEntryView.Font;
		}

		private TControl FindControl<TControl>(string name)
			where TControl : Control
		{
			return mHost.MainWindow.Controls.Find(name, true).SingleOrDefault() as TControl;
		}

		public override void Terminate()
		{
			if (mHost == null) return;

			mOriginalEntryView.FontChanged -= mOriginalEntryView_FontChanged;
			mHost.MainWindow.UIStateUpdated -= this.OnUIStateUpdated;

			mOriginalEntryView.Enabled = true;
			mOriginalEntryView = null;

			mHost = null;
		}

		private void OnUIStateUpdated(object sender, EventArgs e)
		{
			mEntryView.Entry = mHost.MainWindow.GetSelectedEntry(true);
		}
	}
}
