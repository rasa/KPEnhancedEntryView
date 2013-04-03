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
			this.mTextBox = new System.Windows.Forms.TextBox();
			this.mToggleHidden = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// mTextBox
			// 
			this.mTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mTextBox.Location = new System.Drawing.Point(0, 0);
			this.mTextBox.Name = "mTextBox";
			this.mTextBox.Size = new System.Drawing.Size(271, 20);
			this.mTextBox.TabIndex = 0;
			// 
			// mToggleHidden
			// 
			this.mToggleHidden.Appearance = System.Windows.Forms.Appearance.Button;
			this.mToggleHidden.Dock = System.Windows.Forms.DockStyle.Right;
			this.mToggleHidden.Image = global::KPEnhancedEntryView.Properties.Resources.B17x05_3BlackDots;
			this.mToggleHidden.Location = new System.Drawing.Point(271, 0);
			this.mToggleHidden.Name = "mToggleHidden";
			this.mToggleHidden.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
			this.mToggleHidden.Size = new System.Drawing.Size(32, 20);
			this.mToggleHidden.TabIndex = 1;
			this.mToggleHidden.UseVisualStyleBackColor = true;
			this.mToggleHidden.CheckedChanged += new System.EventHandler(this.mToggleHidden_CheckedChanged);
			// 
			// ProtectedFieldEditor
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.mTextBox);
			this.Controls.Add(this.mToggleHidden);
			this.Name = "ProtectedFieldEditor";
			this.Size = new System.Drawing.Size(303, 20);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox mTextBox;
		private System.Windows.Forms.CheckBox mToggleHidden;
	}
}
