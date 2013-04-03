using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeePass.UI;
using System.Drawing;
using System.Windows.Forms;

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

				var textLength = linkDetector.Text.Length;
				int? currentLinkStart = null;
				for (int i = 0; i < textLength; i++)
				{
					linkDetector.Select(i, 1);
					var isPartOfLink = UIUtil.RtfIsFirstCharLink(linkDetector);

					if (isPartOfLink)
					{
						if (!currentLinkStart.HasValue)
						{
							currentLinkStart = i;
						}
					}
					else
					{
						if (currentLinkStart.HasValue)
						{
							rtb.Select(currentLinkStart.Value, i - currentLinkStart.Value);
							UIUtil.RtfSetSelectionLink(rtb);
							currentLinkStart = null;
						}
					}
				}
			}
			rtb.Select(0, 0);
		}

		public class SaveSelectionState : IDisposable
		{
			private readonly RichTextBox mRichTextBox;
			private readonly bool mRestoreOnlyIfTextUnchanged;
			private readonly int mSelectionStart;
			private readonly int mSelectionLength;
			private readonly Point? mScrollPos;

			private readonly string mInitialText;

			public SaveSelectionState(RichTextBox richTextBox, bool restoreOnlyIfITextUnchanged = false)
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
