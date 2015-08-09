using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace KPEnhancedEntryView
{
	public class DynamicMultiLineTextBox : TextBox
	{
		public DynamicMultiLineTextBox()
		{
			Multiline = true;
			WordWrap = false;
		}

		private const int DefaultMaxDisplayedLineCount = 4;
		private int mMaxDisplayedLineCount = DefaultMaxDisplayedLineCount;

		[DefaultValue(DefaultMaxDisplayedLineCount)]
		public int MaxDisplayedLineCount
		{
			get { return mMaxDisplayedLineCount; }
			set
			{
				if (value != mMaxDisplayedLineCount)
				{
					mMaxDisplayedLineCount = value;
					AdjustHeight();
				}
			}
		}

		public override string Text
		{
			get { return base.Text; }
			set
			{
				base.Text = MultiLineHelper.ToTextBoxText(value);
			}
		}

		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);

			AdjustHeight();
		}

		private void AdjustHeight()
		{
			var lineCount = 0;
			var index = -1;
			var text = base.Text;
			do
			{
				lineCount++;
				if (index < text.Length)
				{
					index = text.IndexOf('\n', index + 1);
				}
			} while (index >= 0 && lineCount < MaxDisplayedLineCount);

			ScrollBars = lineCount > 1 ? ScrollBars.Both : ScrollBars.None;

			Height = PreferredHeight +
			         TextRenderer.MeasureText(new String('\n', lineCount - 1), Font, System.Drawing.Size.Empty, TextFormatFlags.TextBoxControl).Height +
			         (lineCount > 1 ? SystemInformation.HorizontalScrollBarHeight : 0);
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			AdjustHeight();
		}
	}
}