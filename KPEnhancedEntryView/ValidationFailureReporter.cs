using System;
using System.Windows.Forms;

namespace KPEnhancedEntryView
{
	internal class ValidationFailureReporter : ToolTip
	{
		public ValidationFailureReporter() : base() { }
		public ValidationFailureReporter(System.ComponentModel.IContainer cont) : base(cont) { }
		
		public void ReportValidationFailure(Control control, string message)
		{
			Show(message, control, 0, control.Height);
			control.KeyPress += ClearValidationFailureMessage;
			control.VisibleChanged += ClearValidationFailureMessage;
		}

		private void ClearValidationFailureMessage(object sender, EventArgs e)
		{
			var control = (Control)sender;
			control.KeyPress -= ClearValidationFailureMessage;
			control.VisibleChanged -= ClearValidationFailureMessage;

			Hide(control);
		}
	}
}
