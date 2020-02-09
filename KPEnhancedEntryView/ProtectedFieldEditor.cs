using System;
using System.Drawing;
using System.Windows.Forms;
using KeePass.App;
using KeePass.UI;
using KeePassLib.Security;

namespace KPEnhancedEntryView
{
	public partial class ProtectedFieldEditor : Control
	{
		public ProtectedFieldEditor()
		{
			InitializeComponent();

			mToggleHidden.Width = Properties.Resources.B17x05_3BlackDots.Width + DpiUtil.ScaleIntX(16);

			mTextBox.EnableProtection(mToggleHidden.Checked);
		}

		protected override void Select(bool directed, bool forward)
		{
			base.Select(directed, forward);
			mTextBox.Select();
		}

		public override Size GetPreferredSize(Size proposedSize)
		{
			var size = mTextBox.GetPreferredSize(proposedSize);
			return new Size(size.Width + mToggleHidden.Width, size.Height);
		}

		public bool HidePassword
		{
			get { return mToggleHidden.Checked; }
			set { mToggleHidden.Checked = value; }
		}

		public ProtectedString Value
		{
			get { return mTextBox.TextEx; }
			set { mTextBox.TextEx = value; }
		}

		private void mToggleHidden_CheckedChanged(object sender, EventArgs e)
		{
			if (!mToggleHidden.Checked && !AppPolicy.Try(AppPolicyId.UnhidePasswords))
			{
				mToggleHidden.Checked = true;
				return;
			}
			mTextBox.EnableProtection(mToggleHidden.Checked);
		}
	}
}
