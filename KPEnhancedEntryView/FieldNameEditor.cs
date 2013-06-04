using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using KeePassLib;
using KeePass.Resources;

namespace KPEnhancedEntryView
{
	public class FieldNameEditor : ComboBox
	{
		private static readonly TimeSpan PopulationUIUpdateFrequency = TimeSpan.FromSeconds(0.5);

		private Options mOptions;
		private Thread mPopulationThread;

		public FieldNameEditor(PwEntry entry, Options options) : this (Enumerable.Repeat(entry, 1), options)
		{
		}

		public FieldNameEditor(IEnumerable<PwEntry> entries, Options options)
		{
			if (!entries.Any())
			{
				throw new ArgumentException("Expecting at least one entry");
			}

			AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			AutoCompleteSource = AutoCompleteSource.ListItems;
			DropDownStyle = ComboBoxStyle.DropDown;

			mOptions = options;

			Populate(entries);
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

		public string FieldName
		{
			get
			{
				var selectedFieldName = SelectedItem as FieldNameItem;
				if (selectedFieldName != null)
				{
					return selectedFieldName.FieldName;
				}

				var enteredText = Text;

				// Double-check that the text they entered wasn't a field name:
				var matchingFieldName = Items.Cast<FieldNameItem>().FirstOrDefault(item => item.DisplayName == enteredText);
				if (matchingFieldName != null)
				{
					return matchingFieldName.FieldName;
				}

				// No match, so it's a new one
				return enteredText;
			}
		}

		private void Populate(IEnumerable<PwEntry> entries)
		{
			// Do population asynchronously
			mPopulationThread = new Thread(PopulationWorker) { Name = "PopulationWorker", IsBackground = true };
			mPopulationThread.Start(entries);
		}

		private void PopulationWorker(object state)
		{
			var entries = (IEnumerable<PwEntry>)state;
			var multipleEntries = entries.Skip(1).Any();

			var fieldNames = new HashSet<string>();
			var fieldNamesForPopulation = new List<FieldNameItem>();

			if (!multipleEntries)
			{
				// Add any standard fields that are blank (as they will be hidden)
				if (mOptions.HideEmptyFields)
				{
					var entry = entries.First();
					AddFieldNameIfEmpty(entry, fieldNamesForPopulation, new FieldNameItem(PwDefs.TitleField, KPRes.Title, 0));
					AddFieldNameIfEmpty(entry, fieldNamesForPopulation, new FieldNameItem(PwDefs.UserNameField, KPRes.UserName, 1));
					AddFieldNameIfEmpty(entry, fieldNamesForPopulation, new FieldNameItem(PwDefs.PasswordField, KPRes.Password, 2));
					AddFieldNameIfEmpty(entry, fieldNamesForPopulation, new FieldNameItem(PwDefs.UrlField, KPRes.Url, 3));
				}
			}
			
			// Start with all the field names that are already on these entries
			foreach (var entry in entries)
			{
				fieldNames.UnionWith(entry.Strings.GetKeys());
			}

			var populationUpdateUI = new Action<List<FieldNameItem>>(PopulationUpdateUI);
			
			// Now find other field names on other entries to suggest
			var parentGroups = (from entry in entries select entry.ParentGroup).Distinct();

			var lastUIUpdate = DateTime.Now;
			foreach (var otherEntry in from parentGroup in parentGroups 
									   from entry in parentGroup.Entries
										   select entry)
			{
				foreach (var fieldName in otherEntry.Strings.GetKeys())
				{
					if (fieldNames.Add(fieldName))
					{
						// This is a new field name, add it to the list to be added
						fieldNamesForPopulation.Add(new FieldNameItem(fieldName, fieldName, 4));

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

		private void AddFieldNameIfEmpty(PwEntry entry, List<FieldNameItem> fieldNameItems, FieldNameItem fieldNameItem)
		{
			var value = entry.Strings.Get(fieldNameItem.FieldName);
			if (value == null || value.IsEmpty)
			{
				fieldNameItems.Add(fieldNameItem);
			}
		}

		private object mPendingPopulationUpdateLock = new object();
		private List<FieldNameItem> mPendingPopulationUpdate;
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

		private void PopulationUpdateUI(List<FieldNameItem> fieldNamesForPopulation)
		{
			if (!DroppedDown) // Don't update while dropped down, it's distracting
			{
				// Grab existing items, and sort the whole lot.
				fieldNamesForPopulation.AddRange(Items.Cast<FieldNameItem>());
				fieldNamesForPopulation.Sort();
				Items.AddRange(fieldNamesForPopulation.ToArray());
				fieldNamesForPopulation.Clear();
			}
		}

		private class FieldNameItem : IComparable<FieldNameItem>
		{
			private readonly string mFieldName;
			private readonly string mDisplayName;
			private readonly int mSortPosition;

			public FieldNameItem(string fieldName, string displayName, int sortPosition)
			{
				mFieldName = fieldName;
				mDisplayName = displayName;
				mSortPosition = sortPosition;
			}

			public string FieldName { get { return mFieldName; } }
			public string DisplayName { get { return mDisplayName; } }

			public override string ToString()
			{
				return DisplayName;
			}

			public int CompareTo(FieldNameItem other)
			{
				// First key is sort position, second is display name
				var result = mSortPosition.CompareTo(other.mSortPosition);
				if (result == 0)
				{
					result = mDisplayName.CompareTo(other.mDisplayName);
				}

				return result;
			}
		}
	}
}
