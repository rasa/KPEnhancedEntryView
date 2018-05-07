using KeePass.UI;

namespace KPEnhancedEntryView
{
	partial class ProtectedFieldEditor
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.mToggleHidden = new System.Windows.Forms.CheckBox();
			this.layout = new System.Windows.Forms.TableLayoutPanel();
			this.mTextBox = new SecureTextBoxEx();
			this.layout.SuspendLayout();
			this.SuspendLayout();
			// 
			// mToggleHidden
			// 
			this.mToggleHidden.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.mToggleHidden.Appearance = System.Windows.Forms.Appearance.Button;
			this.mToggleHidden.Image = global::KPEnhancedEntryView.Properties.Resources.B17x05_3BlackDots;
			this.mToggleHidden.Location = new System.Drawing.Point(243, 0);
			this.mToggleHidden.Margin = new System.Windows.Forms.Padding(0);
			this.mToggleHidden.Name = "mToggleHidden";
			this.mToggleHidden.Size = new System.Drawing.Size(60, 24);
			this.mToggleHidden.TabIndex = 1;
			this.mToggleHidden.UseVisualStyleBackColor = true;
			this.mToggleHidden.CheckedChanged += new System.EventHandler(this.mToggleHidden_CheckedChanged);
			// 
			// layout
			// 
			this.layout.ColumnCount = 2;
			this.layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.layout.Controls.Add(this.mTextBox, 0, 0);
			this.layout.Controls.Add(this.mToggleHidden, 1, 0);
			this.layout.Dock = System.Windows.Forms.DockStyle.Fill;
			this.layout.Location = new System.Drawing.Point(0, 0);
			this.layout.Name = "layout";
			this.layout.RowCount = 1;
			this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.layout.Size = new System.Drawing.Size(303, 25);
			this.layout.TabIndex = 0;
			// 
			// mTextBox
			// 
			this.mTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.mTextBox.Location = new System.Drawing.Point(0, 0);
			this.mTextBox.Margin = new System.Windows.Forms.Padding(0);
			this.mTextBox.Multiline = true;
			this.mTextBox.Name = "mTextBox";
			this.mTextBox.Size = new System.Drawing.Size(243, 20);
			this.mTextBox.TabIndex = 0;
			this.mTextBox.WordWrap = false;
			this.mTextBox.SizeChanged += new System.EventHandler(this.mTextBox_SizeChanged);
			// 
			// ProtectedFieldEditor
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.layout);
			this.Name = "ProtectedFieldEditor";
			this.Size = new System.Drawing.Size(303, 25);
			this.layout.ResumeLayout(false);
			this.layout.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private SecureTextBoxEx mTextBox;
		private System.Windows.Forms.CheckBox mToggleHidden;
		private System.Windows.Forms.TableLayoutPanel layout;
	}
}
