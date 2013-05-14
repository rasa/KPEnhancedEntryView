using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using KeePass.Plugins;

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
			mEntryView = new EntryView(mHost.MainWindow)
			{
				Name = "m_KPEnhancedEntryView",
				Dock = DockStyle.Fill,
				AutoValidate = AutoValidate.Disable // Don't allow our internal validation to bubble upwards to KeePass
			};

			entryViewContainer.Controls.Add(mEntryView);

			// Move the original entry view into a tab on the new view
			entryViewContainer.Controls.Remove(mOriginalEntryView);
			mEntryView.AllTextControl = mOriginalEntryView;

			// Font is assigned, not inherited. So assign here too, and follow any changes
			mOriginalEntryView.FontChanged += mOriginalEntryView_FontChanged;
			mOriginalEntryView_FontChanged(null, EventArgs.Empty);

			// Hook UIStateUpdated to watch for current entry changing.
			mHost.MainWindow.UIStateUpdated += this.OnUIStateUpdated;

			// HACK: UIStateUpdated isn't called when navigating a reference link in the entry view, so grab that too.
			mOriginalEntryView.LinkClicked += this.OnUIStateUpdated;
			
			// Hook events to update the UI when the entry is modified
			mEntryView.EntryModified += this.mEntryView_EntryModified;

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

			// Restore original entry view to it's normal place
			mEntryView.Parent.Controls.Add(mOriginalEntryView);
			mEntryView.Parent.Controls.Remove(mEntryView);
			mOriginalEntryView = null;

			mEntryView.Dispose();
			mEntryView = null;

			mHost = null;
		}

		private void OnUIStateUpdated(object sender, EventArgs e)
		{
			mEntryView.Entry = mHost.MainWindow.GetSelectedEntry(true);
		}

		private void mEntryView_EntryModified(object sender, EventArgs e)
		{
			mHost.MainWindow.UpdateUI(false, null, false, null, false, null, true);
			mHost.MainWindow.RefreshEntriesList();
		}
	}
}
