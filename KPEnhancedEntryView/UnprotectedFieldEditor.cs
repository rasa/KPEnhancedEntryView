using System;
using KeePassLib.Security;

namespace KPEnhancedEntryView
{
	public class UnprotectedFieldEditor : DynamicMultiLineTextBox
	{
		public ProtectedString Value
		{
			get
			{
				return new ProtectedString(false, base.Text);
			}
			set
			{
				base.Text = value.ReadString();
			}
		}

		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				// Text may not be set directly. Use .Value instead
			}
		}
	}
}
