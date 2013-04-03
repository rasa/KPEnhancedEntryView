using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using KeePassLib;

namespace KPEnhancedEntryView
{
	public class FieldNameEditor : ComboBox
	{
		private static readonly TimeSpan PopulationUIUpdateFrequency = TimeSpan.FromSeconds(0.5);
		
		private Thread mPopulationThread;

		public FieldNameEditor(PwEntry entry)
		{
			AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			AutoCompleteSource = AutoCompleteSource.ListItems;
			DropDownStyle = ComboBoxStyle.DropDown;
			Sorted = true;

			Populate(entry);
		}

		protected override void Dispose(bool disposing)
		{
			if (mPopulationThread != null)
			{
				mPopulationThread.Abort();
				mPopulationThread = null;
			}

			base.Dispose(disposing);
		}

		private void Populate(PwEntry entry)
		{
			// Do population asynchronously
			mPopulationThread = new Thread(PopulationWorker) { Name = "PopulationWorker", IsBackground = true };
			mPopulationThread.Start(entry);
		}

		private void PopulationWorker(object state)
		{
			var entry = (PwEntry)state;

			var fieldNames = new HashSet<string>();
			var fieldNamesForPopulation = new List<string>();

			// Start with all the field names that are already on this entry
			fieldNames.UnionWith(entry.Strings.GetKeys());

			var populationUpdateUI = new Action<List<String>>(PopulationUpdateUI);
			
			// Now find other field names on other entries to suggest
			var lastUIUpdate = DateTime.Now;
			foreach (var otherEntry in entry.ParentGroup.Entries)
			{
				foreach (var fieldName in otherEntry.Strings.GetKeys())
				{
					if (fieldNames.Add(fieldName))
					{
						// This is a new field name, add it to the list to be added
						fieldNamesForPopulation.Add(fieldName);

						// Update the UI periodically
						if (Created && DateTime.Now - lastUIUpdate > PopulationUIUpdateFrequency)
						{
							Invoke(populationUpdateUI, fieldNamesForPopulation);
							lastUIUpdate = DateTime.Now;
						}
					}
				}
			}

			// Final update, regardless of timing
			if (fieldNamesForPopulation.Any())
			{
				lock (mPendingPopulationUpdateLock)
				{
					if (Created)
					{
						BeginInvoke(populationUpdateUI, fieldNamesForPopulation);
					}
					else
					{
						mPendingPopulationUpdate = fieldNamesForPopulation;
					}
				}
			}
		}

		private object mPendingPopulationUpdateLock = new object();
		private List<String> mPendingPopulationUpdate;
		protected override void OnCreateControl()
		{
			base.OnCreateControl();

			lock (mPendingPopulationUpdateLock)
			{
				if (mPendingPopulationUpdate != null)
				{
					PopulationUpdateUI(mPendingPopulationUpdate);
					mPendingPopulationUpdate = null;
				}
			}
		}

		private void PopulationUpdateUI(List<String> fieldNamesForPopulation)
		{
			if (!DroppedDown) // Don't update while dropped down, it's distracting
			{
				Items.AddRange(fieldNamesForPopulation.ToArray());
				fieldNamesForPopulation.Clear();
			}
		}
	}
}
