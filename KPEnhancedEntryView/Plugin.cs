using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using KeePass.Forms;
using KeePass.Plugins;
using KeePassLib;
using KeePassLib.Utility;

namespace KPEnhancedEntryView
{
	public sealed class KPEnhancedEntryViewExt : Plugin
	{
		private IPluginHost mHost;
		private EntryView mEntryView;
		private RichTextBox mOriginalEntryView;
		private Control mEntriesListView;
		private Options mOptions;
		
		public override bool Initialize(IPluginHost host)
		{
			if (host == null) return false;

			// Ensure terminate before initialise, in unlikely case of double initialisation
			Terminate();

			mHost = host;

			// Add an Options menu
			mOptions = new Options(mHost);
			mHost.MainWindow.ToolsMenu.DropDownItems.Add(mOptions.Menu);

			mOptions.OptionChanged += mOptions_OptionChanged;

			mOriginalEntryView = FindControl<RichTextBox>("m_richEntryView");
			var entryViewContainer = mOriginalEntryView.Parent;
			if (mOriginalEntryView == null || entryViewContainer == null)
			{
				Debug.Fail("Couldn't find existing entry view to replace");
				mHost = null;
				return false;
			}

			// Replace existing entry view with new one
			mEntryView = new EntryView(mHost.MainWindow, mOptions)
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

			// Database may be saved while in the middle of editing Notes. Watch for that and commit the current edit if that happens
			mHost.MainWindow.FileSaving += this.OnFileSaving;

			if (PwDefs.FileVersion64 < StrUtil.ParseVersion("2.22")) // Fixed in KeePass 2.22
			{
				// HACK: UIStateUpdated isn't called when navigating a reference link in the entry view, so grab that too.
				mOriginalEntryView.LinkClicked += this.OnUIStateUpdated;
			}

			// HACK: UIStateUpdated isn't called when toggling column value hiding on and off, so monitor the entries list for being invalidated
			mEntriesListView = FindControl<Control>("m_lvEntries");
			if (mEntriesListView != null)
			{
				mEntriesListView.Invalidated += mEntitiesListView_Invalidated;
			}

			// Hook events to update the UI when the entry is modified
			mEntryView.EntryModified += this.mEntryView_EntryModified;
			
			return true;
		}

		public override string UpdateUrl
		{
			get { return "sourceforge-version://KPEnhancedEntryView/kpenhentryview?-v(%5B%5Cd.%5D%2B)%5C.zip"; }
		}

		private void mEntitiesListView_Invalidated(object sender, InvalidateEventArgs e)
		{
			// Whenever the entities list is invalidated, refresh the items of the entry view too (so that changes like column value hiding get reflected)
			mEntryView.RefreshItems();
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

			if (mEntriesListView != null)
			{
				mEntriesListView.Invalidated -= mEntitiesListView_Invalidated;
				mEntriesListView = null;
			}

			mEntryView.Dispose();
			mEntryView = null;

			mHost.MainWindow.ToolsMenu.DropDownItems.Remove(mOptions.Menu);

			mHost = null;
		}

		private void OnUIStateUpdated(object sender, EventArgs e)
		{
			mEntryView.Entries = mHost.MainWindow.GetSelectedEntries();
		}

		private void OnFileSaving(object sender, FileSavingEventArgs e)
		{
			mEntryView.FinishEditingNotes();
		}

		private void mEntryView_EntryModified(object sender, EventArgs e)
		{
			mHost.MainWindow.UpdateUI(false, null, false, null, false, null, true);
			mHost.MainWindow.RefreshEntriesList();
		}

		private void mOptions_OptionChanged(object sender, Options.OptionChangedEventArgs e)
		{
			switch (e.OptionName)
			{
				case Options.OptionName.HideEmptyFields:
					// Force a refresh of the entry
					mEntryView.Entry = null;
					OnUIStateUpdated(null, EventArgs.Empty);
					break;
			}
		}
	}
}
