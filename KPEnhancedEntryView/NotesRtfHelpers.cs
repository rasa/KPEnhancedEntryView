using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using KeePass.UI;
using System.Runtime.InteropServices;

namespace KPEnhancedEntryView
{
	public static class NotesRtfHelpers
	{
		public static string ReplaceFormattingTags(string strNotes)
		{
			// This code copied from KeePass.Forms.MainForm.ShowEntryDetails (MainForm_Functions.cs). It is not otherwise exposed.
			KeyValuePair<string, string> kvpBold = RichTextBuilder.GetStyleIdCodes(
					FontStyle.Bold);
			KeyValuePair<string, string> kvpItalic = RichTextBuilder.GetStyleIdCodes(
				FontStyle.Italic);
			KeyValuePair<string, string> kvpUnderline = RichTextBuilder.GetStyleIdCodes(
				FontStyle.Underline);

			strNotes = strNotes.Replace(@"<b>", kvpBold.Key);
			strNotes = strNotes.Replace(@"</b>", kvpBold.Value);
			strNotes = strNotes.Replace(@"<i>", kvpItalic.Key);
			strNotes = strNotes.Replace(@"</i>", kvpItalic.Value);
			strNotes = strNotes.Replace(@"<u>", kvpUnderline.Key);
			strNotes = strNotes.Replace(@"</u>", kvpUnderline.Value);

			return strNotes;
		}

		internal static void RtfLinkifyUrls(RichTextBox rtb)
		{
			using(var linkDetector = new RichTextBox())
			{
				linkDetector.Text = rtb.Text;

				// Optimised search for links - keep splitting the area to find linkless parts

				// Start with a selection of everything, and split until no section is mixed
				var sections = SplitToSections(new TextSection(linkDetector, 0, linkDetector.TextLength));
				
				// Coalesce adjacent sections of the same status
				sections = CoalesceSections(sections);

				// Also linkify specially marked links (between < >)
				var markedSections = new List<TextSection>();
				foreach (Match linkMatch in EntryView.MarkedLinkRegex.Matches(rtb.Text))
				{
					markedSections.Add(new TextSection(linkDetector, linkMatch.Index + 1, linkMatch.Index + linkMatch.Length - 1, true));
				}
				
				// Linkify sections that are links
				foreach (var section in sections.Concat(markedSections))
				{
					section.LinkifyIfLink(rtb);
				}
			}
			rtb.Select(0, 0);
		}

		#region TextSection linkification algorithm
		/// <summary>
		/// Combines all adjecent sections with the same link status.
		/// <param name="orderedSections">The sections to coalesce, in order of position within the text</param>
		/// </summary>
		private static List<TextSection> CoalesceSections(IEnumerable<TextSection> orderedSections)
		{
			var sections = new List<TextSection>();

			TextSection currentSection = null;
			foreach (var section in orderedSections)
			{
				if (currentSection == null)
				{
					currentSection = section;
				}
				else
				{
					if (!currentSection.TryCoalesce(section))
					{
						// Sections were not adjacent, or not the same link status, so return finish the current one and start a new current
						sections.Add(currentSection);
						currentSection = section;
					}
				}
			}

			sections.Add(currentSection);
			return sections;
		}

		/// <summary>
		/// Splits the specified section into descendants where each descendant is either wholly a link, or wholly not a link.
		/// </summary>
		private static List<TextSection> SplitToSections(TextSection section)
		{
			var sections = new List<TextSection>();
			if (section.IsMixed)
			{
				// This is a mixed selection, so split further
				foreach (var subSection in section.Split())
				{
					sections.AddRange(SplitToSections(subSection));
				}
			}
			else
			{
				// This is a non-mixed selection, so no need to further split
				sections.Add(section);
			}
			return sections;
		}

		private class TextSection
		{
			private int mStartIndex;
			private int mEndIndex;
			private readonly bool? mIsLink;
			private readonly RichTextBox mRtb;

			public TextSection(RichTextBox rtb, int startIndex, int endIndex) : this(rtb, startIndex, endIndex, null)
			{
				mRtb.Select(mStartIndex, Length);
				mIsLink = GetSelectionIsLink(mRtb);
			}
			public TextSection(RichTextBox rtb, int startIndex, int endIndex, bool? isLink)
			{
				mStartIndex = startIndex;
				mEndIndex = endIndex;
				mRtb = rtb;

				mIsLink = isLink;
			}

			public bool IsMixed { get { return !mIsLink.HasValue; } }
		
			private int Length { get { return mEndIndex - mStartIndex; } }
			
			public IEnumerable<TextSection> Split()
			{
				if (Length < 2)
				{
					return null;
				}
				int splitLength = Length / 2;
				return new[] { new TextSection(mRtb, mStartIndex, mStartIndex + splitLength),
							   new TextSection(mRtb, mStartIndex + splitLength, mEndIndex) };
			}

			/// <summary>
			/// Attempt to coalesce the specified section into this one. If they aren't suitable for coalescing, return false.
			/// </summary>
			/// <param name="section"></param>
			/// <returns></returns>
			internal bool TryCoalesce(TextSection section)
			{
				System.Diagnostics.Debug.Assert(mRtb == section.mRtb);
				System.Diagnostics.Debug.Assert(!IsMixed);
				System.Diagnostics.Debug.Assert(!section.IsMixed);

				if (mIsLink == section.mIsLink && // Only coalesce sections with the same link status
					mEndIndex == section.mStartIndex) // Only coalesce adjacent sections
				{
					mEndIndex = section.mEndIndex; // Coalesce
					return true;
				}

				return false;
			}

			/// <summary>
			/// If this section is a (non-mixed) link, linkify it in the specified rich text box
			/// </summary>
			/// <param name="rtb"></param>
			internal void LinkifyIfLink(RichTextBox rtb)
			{
				if (mIsLink ?? false)
				{
					rtb.Select(mStartIndex, Length);
					UIUtil.RtfSetSelectionLink(rtb);
				}
			}

			public override string ToString()
			{
				return String.Format("{0} - {1}: {2}", mStartIndex, mEndIndex, mRtb.Text.Substring(mStartIndex, Length));
			}
		}

		/// <returns>True if the selection is entirely a link, false if none of it is, null if it is a mix</returns>
		private static bool? GetSelectionIsLink(RichTextBox rtb)
		{
			bool? containsLink = false;

			var cf = new NativeMethods.CHARFORMAT2();
			cf.cbSize = (uint)Marshal.SizeOf(cf);

			try
			{
				IntPtr wParam = (IntPtr)NativeMethods.SCF_SELECTION;
				IntPtr lParam = Marshal.AllocCoTaskMem(Marshal.SizeOf(cf));
				Marshal.StructureToPtr(cf, lParam, false);

				NativeMethods.SendMessage(rtb.Handle, NativeMethods.EM_GETCHARFORMAT, wParam, lParam);

				cf = (NativeMethods.CHARFORMAT2)Marshal.PtrToStructure(lParam, typeof(NativeMethods.CHARFORMAT2));

				if ((cf.dwMask & NativeMethods.CFE_LINK) == NativeMethods.CFE_LINK)
				{
					containsLink = (cf.dwEffects & NativeMethods.CFM_LINK) == NativeMethods.CFM_LINK;
				}
				else
				{
					// mixed selection
					containsLink = null;
				}

				Marshal.FreeCoTaskMem(lParam);
			}
			catch (Exception) { System.Diagnostics.Debug.Assert(false); }

			return containsLink;
		}
		#endregion

		public class SaveSelectionState : IDisposable
		{
			private readonly RichTextBox mRichTextBox;
			private readonly bool mRestoreOnlyIfTextUnchanged;
			private readonly int mSelectionStart;
			private readonly int mSelectionLength;
			private readonly Point? mScrollPos;

			private readonly string mInitialText;

			public SaveSelectionState(RichTextBox richTextBox) : this(richTextBox, false) { } // Optional parameters not supported with PlgX compiler

			public SaveSelectionState(RichTextBox richTextBox, bool restoreOnlyIfITextUnchanged)
			{
				mRichTextBox = richTextBox;
				mRestoreOnlyIfTextUnchanged = restoreOnlyIfITextUnchanged;

				if (mRestoreOnlyIfTextUnchanged)
				{
					mInitialText = richTextBox.Text;
				}

				try
				{
					var scrollPos = Point.Empty;
					NativeMethods.SendMessage(mRichTextBox.Handle, NativeMethods.EM_GETSCROLLPOS, 0, ref scrollPos);
					mScrollPos = scrollPos;
				}
				catch (Exception) { }

				mSelectionStart = mRichTextBox.SelectionStart;
				mSelectionLength = mRichTextBox.SelectionLength;

			}
			public void Dispose()
			{
				if (mRestoreOnlyIfTextUnchanged && mRichTextBox.Text != mInitialText)
				{
					// Do not restore - text is changed.
					return;
				}

				mRichTextBox.SelectionStart = mSelectionStart;
				mRichTextBox.SelectionLength = mSelectionLength;

				if (mScrollPos.HasValue)
				{
					try
					{
						var scrollPos = mScrollPos.Value;
						NativeMethods.SendMessage(mRichTextBox.Handle, NativeMethods.EM_SETSCROLLPOS, 0, ref scrollPos);
					}
					catch (Exception) { }
				}
			}
		}
	}
}
