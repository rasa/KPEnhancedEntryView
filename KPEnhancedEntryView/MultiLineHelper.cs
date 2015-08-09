using System.Text.RegularExpressions;

namespace KPEnhancedEntryView
{
	internal static class MultiLineHelper
	{
		private static readonly Regex MultiToSingleLineRegEx = new Regex(@"\r?\n|\r");

		public static string ToSingleLine(string multiLine)
		{
			return MultiToSingleLineRegEx.Replace(multiLine, " ");
		}

		public static bool IsMultiLine(string value)
		{
			return MultiToSingleLineRegEx.IsMatch(value);
		}

		public static string ToTextBoxText(string multiLine)
		{
			// TextBox supports only \r\n, so normalize line breaks to that.
			return MultiToSingleLineRegEx.Replace(multiLine, "\r\n");
		}
	}
}
