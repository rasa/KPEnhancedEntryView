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
		private SecureEdit mSecureEdit;
		public ProtectedFieldEditor()
		{
			InitializeComponent();

			mToggleHidden.Width = Properties.Resources.B17x05_3BlackDots.Width + DpiUtil.ScaleIntX(16);

			mSecureEdit = new SecureEdit();
			mSecureEdit.Attach(mTextBox, OnPasswordTextChanged, mToggleHidden.Checked);
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
			height = mTextBox.Height;
			base.SetBoundsCore(x, y, width, height, specified);
		}

		public bool HidePassword
		{
			get { return mToggleHidden.Checked; }
			set { mToggleHidden.Checked = value; }
		}

		public ProtectedString Value
		{
			get
			{
				return new ProtectedString(true, mSecureEdit.ToUtf8());
			}
			set
			{
				mSecureEdit.SetPassword(value.ReadUtf8());
			}
		}

		private void OnPasswordTextChanged(object sender, EventArgs e)
		{
		}

		private void mToggleHidden_CheckedChanged(object sender, EventArgs e)
		{
			if (!mToggleHidden.Checked && !AppPolicy.Try(AppPolicyId.UnhidePasswords))
			{
				mToggleHidden.Checked = true;
				return;
			}
			mSecureEdit.EnableProtection(mToggleHidden.Checked);
		}
	}
}
