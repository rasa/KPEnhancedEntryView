namespace KPEnhancedEntryView
{
	partial class EntryView
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
			this.mTabs = new System.Windows.Forms.TabControl();
			this.mFieldsTab = new System.Windows.Forms.TabPage();
			this.mFieldsGrid = new BrightIdeasSoftware.FastObjectListView();
			this.mFieldNames = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.mFieldValues = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.mPropertiesTab = new System.Windows.Forms.TabPage();
			this.mTabs.SuspendLayout();
			this.mFieldsTab.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.mFieldsGrid)).BeginInit();
			this.SuspendLayout();
			// 
			// mTabs
			// 
			this.mTabs.Controls.Add(this.mFieldsTab);
			this.mTabs.Controls.Add(this.mPropertiesTab);
			this.mTabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mTabs.Location = new System.Drawing.Point(0, 0);
			this.mTabs.Name = "mTabs";
			this.mTabs.SelectedIndex = 0;
			this.mTabs.Size = new System.Drawing.Size(373, 342);
			this.mTabs.TabIndex = 1;
			// 
			// mFieldsTab
			// 
			this.mFieldsTab.Controls.Add(this.mFieldsGrid);
			this.mFieldsTab.Location = new System.Drawing.Point(4, 22);
			this.mFieldsTab.Name = "mFieldsTab";
			this.mFieldsTab.Padding = new System.Windows.Forms.Padding(3);
			this.mFieldsTab.Size = new System.Drawing.Size(365, 316);
			this.mFieldsTab.TabIndex = 0;
			this.mFieldsTab.Text = "Fields";
			// 
			// mFieldsGrid
			// 
			this.mFieldsGrid.AllColumns.Add(this.mFieldNames);
			this.mFieldsGrid.AllColumns.Add(this.mFieldValues);
			this.mFieldsGrid.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
			this.mFieldsGrid.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.mFieldNames,
            this.mFieldValues});
			this.mFieldsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mFieldsGrid.FullRowSelect = true;
			this.mFieldsGrid.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.mFieldsGrid.Location = new System.Drawing.Point(3, 3);
			this.mFieldsGrid.Name = "mFieldsGrid";
			this.mFieldsGrid.ShowGroups = false;
			this.mFieldsGrid.Size = new System.Drawing.Size(359, 310);
			this.mFieldsGrid.TabIndex = 0;
			this.mFieldsGrid.UseCellFormatEvents = true;
			this.mFieldsGrid.UseCompatibleStateImageBehavior = false;
			this.mFieldsGrid.View = System.Windows.Forms.View.Details;
			this.mFieldsGrid.VirtualMode = true;
			this.mFieldsGrid.FormatCell += new System.EventHandler<BrightIdeasSoftware.FormatCellEventArgs>(this.mFieldsGrid_FormatCell);
			// 
			// mFieldNames
			// 
			this.mFieldNames.AspectName = "Key";
			this.mFieldNames.CellPadding = null;
			this.mFieldNames.Text = "Name";
			// 
			// mFieldValues
			// 
			this.mFieldValues.CellPadding = null;
			this.mFieldValues.FillsFreeSpace = true;
			this.mFieldValues.Hideable = false;
			this.mFieldValues.Text = "Value";
			// 
			// mPropertiesTab
			// 
			this.mPropertiesTab.Location = new System.Drawing.Point(4, 22);
			this.mPropertiesTab.Name = "mPropertiesTab";
			this.mPropertiesTab.Padding = new System.Windows.Forms.Padding(3);
			this.mPropertiesTab.Size = new System.Drawing.Size(365, 316);
			this.mPropertiesTab.TabIndex = 1;
			this.mPropertiesTab.Text = "Properties";
			// 
			// EntryView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.mTabs);
			this.Name = "EntryView";
			this.Size = new System.Drawing.Size(373, 342);
			this.mTabs.ResumeLayout(false);
			this.mFieldsTab.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.mFieldsGrid)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl mTabs;
		private System.Windows.Forms.TabPage mFieldsTab;
		private System.Windows.Forms.TabPage mPropertiesTab;
		private BrightIdeasSoftware.FastObjectListView mFieldsGrid;
		private BrightIdeasSoftware.OLVColumn mFieldNames;
		private BrightIdeasSoftware.OLVColumn mFieldValues;
	}
}
