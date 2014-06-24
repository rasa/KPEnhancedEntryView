using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using KeePass.Forms;
using KeePass.Plugins;
using KeePass.UI;
using KeePassLib;
using KeePassLib.Utility;

namespace KPEnhancedEntryView
{
	public sealed class KPEnhancedEntryViewExt : Plugin
	{
		private IPluginHost mHost;
		private EntryView mEntryView;
		private RichTextBox mOriginalEntryView;
		private ListView mEntriesListView;
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
			mEntriesListView = FindControl<ListView>("m_lvEntries");
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

		private static readonly TimeSpan EntitiesListViewInvalidationTimeout = TimeSpan.FromMilliseconds(250); // Consolidate any Invalidated events that occur within 250ms of each other
		private readonly object mEntitiesListViewInvalidationTimerLock = new object();
		private Stopwatch mEntitiesListViewInvalidationTimer;
		
		private void mEntitiesListView_Invalidated(object sender, InvalidateEventArgs e)
		{
			// Whenever the entities list is invalidated, refresh the items of the entry view too (so that changes like column value hiding get reflected)

			// For performance, throttle refreshes to consume multiple invalidated events that occur within a short space of each other.
			lock (mEntitiesListViewInvalidationTimerLock)
			{
				if (mEntitiesListViewInvalidationTimer == null)
				{
					mEntitiesListViewInvalidationTimer = Stopwatch.StartNew();
					ThreadPool.QueueUserWorkItem(o => ConsolidateEntitiesListViewInvaidatedEvents());
				}
				else
				{
					// There's already a timer running, so just reset it counting from 0 again
					mEntitiesListViewInvalidationTimer.Reset();
					mEntitiesListViewInvalidationTimer.Start();
				}
			}
		}

		private void ConsolidateEntitiesListViewInvaidatedEvents()
		{
			// Wait for timeout to expire
			do
			{
				TimeSpan remainingTime;
				lock (mEntitiesListViewInvalidationTimerLock)
				{
					remainingTime = EntitiesListViewInvalidationTimeout - mEntitiesListViewInvalidationTimer.Elapsed;

					if (remainingTime <= TimeSpan.Zero)
					{
						// Discard the timer - subsequent Invalidated events will create a new one
						mEntitiesListViewInvalidationTimer = null;
						break;
					}
				}
				
				Thread.Sleep(remainingTime);
			} while (true);

			mEntryView.BeginInvoke(new Action(mEntryView.RefreshItems));
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
			IEnumerable<PwEntry> selectedEntries;

			// Don't use MainForm.GetSSelectedEntries, it has perf issues.
			if (mEntriesListView != null)
			{
				selectedEntries = from ListViewItem lvi in mEntriesListView.SelectedItems select ((PwListItem)lvi.Tag).Entry;
			}
			else
			{
				// Fall back on GetSelectedEntries - can't read from list view directly
				selectedEntries = mHost.MainWindow.GetSelectedEntries();
			}
			mEntryView.Entries = selectedEntries;
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
