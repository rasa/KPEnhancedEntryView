using System;
using System.Drawing;
using System.Windows.Forms;
using KeePass.App;
using KeePass.UI;
using KeePassLib.Security;

namespace KPEnhancedEntryView
{
	public partial class ProtectedFieldEditor : UserControl
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

		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			//height = mTextBox.Height;
			base.SetBoundsCore(x, y, width, height, specified);
		}

		private void mTextBox_SizeChanged(object sender, EventArgs e)
		{
			// Resize this control to accomodate it
			if (IsHandleCreated)
			{
				Height = mTextBox.Height;
				mToggleHidden.Height = mTextBox.PreferredHeight; // One line
			}
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
