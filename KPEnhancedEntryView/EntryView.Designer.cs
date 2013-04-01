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
			this.components = new System.ComponentModel.Container();
			this.mTabs = new System.Windows.Forms.TabControl();
			this.mFieldsTab = new System.Windows.Forms.TabPage();
			this.mSplitGridPanels = new System.Windows.Forms.SplitContainer();
			this.mFieldsGrid = new BrightIdeasSoftware.FastObjectListView();
			this.mFieldNames = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.mFieldValues = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.mSplitNotesAttachements = new System.Windows.Forms.SplitContainer();
			this.mNotesBorder = new System.Windows.Forms.Panel();
			this.mNotes = new KeePass.UI.CustomRichTextBoxEx();
			this.mAttachments = new KPEnhancedEntryView.AttachmentsListView();
			this.mPropertiesTab = new System.Windows.Forms.TabPage();
			this.mValidationMessage = new System.Windows.Forms.ToolTip(this.components);
			this.mTabs.SuspendLayout();
			this.mFieldsTab.SuspendLayout();
			this.mSplitGridPanels.Panel1.SuspendLayout();
			this.mSplitGridPanels.Panel2.SuspendLayout();
			this.mSplitGridPanels.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.mFieldsGrid)).BeginInit();
			this.mSplitNotesAttachements.Panel1.SuspendLayout();
			this.mSplitNotesAttachements.Panel2.SuspendLayout();
			this.mSplitNotesAttachements.SuspendLayout();
			this.mNotesBorder.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.mAttachments)).BeginInit();
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
			this.mFieldsTab.Controls.Add(this.mSplitGridPanels);
			this.mFieldsTab.Location = new System.Drawing.Point(4, 22);
			this.mFieldsTab.Name = "mFieldsTab";
			this.mFieldsTab.Padding = new System.Windows.Forms.Padding(3);
			this.mFieldsTab.Size = new System.Drawing.Size(365, 316);
			this.mFieldsTab.TabIndex = 0;
			this.mFieldsTab.Text = "Fields";
			// 
			// mSplitGridPanels
			// 
			this.mSplitGridPanels.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mSplitGridPanels.Location = new System.Drawing.Point(3, 3);
			this.mSplitGridPanels.Name = "mSplitGridPanels";
			this.mSplitGridPanels.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// mSplitGridPanels.Panel1
			// 
			this.mSplitGridPanels.Panel1.Controls.Add(this.mFieldsGrid);
			// 
			// mSplitGridPanels.Panel2
			// 
			this.mSplitGridPanels.Panel2.Controls.Add(this.mSplitNotesAttachements);
			this.mSplitGridPanels.Size = new System.Drawing.Size(359, 310);
			this.mSplitGridPanels.SplitterDistance = 240;
			this.mSplitGridPanels.TabIndex = 2;
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
			this.mFieldsGrid.Location = new System.Drawing.Point(0, 0);
			this.mFieldsGrid.Name = "mFieldsGrid";
			this.mFieldsGrid.ShowGroups = false;
			this.mFieldsGrid.Size = new System.Drawing.Size(359, 240);
			this.mFieldsGrid.TabIndex = 0;
			this.mFieldsGrid.UseCellFormatEvents = true;
			this.mFieldsGrid.UseCompatibleStateImageBehavior = false;
			this.mFieldsGrid.View = System.Windows.Forms.View.Details;
			this.mFieldsGrid.VirtualMode = true;
			this.mFieldsGrid.CellEditFinishing += new BrightIdeasSoftware.CellEditEventHandler(this.mFieldsGrid_CellEditFinishing);
			this.mFieldsGrid.CellEditStarting += new BrightIdeasSoftware.CellEditEventHandler(this.mFieldsGrid_CellEditStarting);
			this.mFieldsGrid.CellEditValidating += new BrightIdeasSoftware.CellEditEventHandler(this.mFieldsGrid_CellEditValidating);
			this.mFieldsGrid.FormatCell += new System.EventHandler<BrightIdeasSoftware.FormatCellEventArgs>(this.mFieldsGrid_FormatCell);
			// 
			// mFieldNames
			// 
			this.mFieldNames.AspectName = "DisplayName";
			this.mFieldNames.CellPadding = null;
			this.mFieldNames.Text = "Name";
			// 
			// mFieldValues
			// 
			this.mFieldValues.AspectName = "DisplayValue";
			this.mFieldValues.CellPadding = null;
			this.mFieldValues.FillsFreeSpace = true;
			this.mFieldValues.Hideable = false;
			this.mFieldValues.Text = "Value";
			// 
			// mSplitNotesAttachements
			// 
			this.mSplitNotesAttachements.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mSplitNotesAttachements.Location = new System.Drawing.Point(0, 0);
			this.mSplitNotesAttachements.Name = "mSplitNotesAttachements";
			// 
			// mSplitNotesAttachements.Panel1
			// 
			this.mSplitNotesAttachements.Panel1.Controls.Add(this.mNotesBorder);
			// 
			// mSplitNotesAttachements.Panel2
			// 
			this.mSplitNotesAttachements.Panel2.Controls.Add(this.mAttachments);
			this.mSplitNotesAttachements.Size = new System.Drawing.Size(359, 66);
			this.mSplitNotesAttachements.SplitterDistance = 250;
			this.mSplitNotesAttachements.TabIndex = 0;
			// 
			// mNotesBorder
			// 
			this.mNotesBorder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.mNotesBorder.Controls.Add(this.mNotes);
			this.mNotesBorder.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mNotesBorder.Location = new System.Drawing.Point(0, 0);
			this.mNotesBorder.Name = "mNotesBorder";
			this.mNotesBorder.Size = new System.Drawing.Size(250, 66);
			this.mNotesBorder.TabIndex = 2;
			// 
			// mNotes
			// 
			this.mNotes.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.mNotes.DetectUrls = false;
			this.mNotes.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mNotes.Location = new System.Drawing.Point(0, 0);
			this.mNotes.Name = "mNotes";
			this.mNotes.Size = new System.Drawing.Size(248, 64);
			this.mNotes.TabIndex = 1;
			this.mNotes.Text = "";
			// 
			// mAttachments
			// 
			this.mAttachments.Binaries = null;
			this.mAttachments.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mAttachments.EmptyListMsg = "Attachments";
			this.mAttachments.Location = new System.Drawing.Point(0, 0);
			this.mAttachments.Name = "mAttachments";
			this.mAttachments.ShowGroups = false;
			this.mAttachments.Size = new System.Drawing.Size(105, 66);
			this.mAttachments.TabIndex = 0;
			this.mAttachments.UseCompatibleStateImageBehavior = false;
			this.mAttachments.View = System.Windows.Forms.View.SmallIcon;
			this.mAttachments.VirtualMode = true;
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
			this.mSplitGridPanels.Panel1.ResumeLayout(false);
			this.mSplitGridPanels.Panel2.ResumeLayout(false);
			this.mSplitGridPanels.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.mFieldsGrid)).EndInit();
			this.mSplitNotesAttachements.Panel1.ResumeLayout(false);
			this.mSplitNotesAttachements.Panel2.ResumeLayout(false);
			this.mSplitNotesAttachements.ResumeLayout(false);
			this.mNotesBorder.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.mAttachments)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl mTabs;
		private System.Windows.Forms.TabPage mFieldsTab;
		private System.Windows.Forms.TabPage mPropertiesTab;
		private BrightIdeasSoftware.FastObjectListView mFieldsGrid;
		private BrightIdeasSoftware.OLVColumn mFieldNames;
		private BrightIdeasSoftware.OLVColumn mFieldValues;
		private System.Windows.Forms.SplitContainer mSplitGridPanels;
		private System.Windows.Forms.SplitContainer mSplitNotesAttachements;
		private KeePass.UI.CustomRichTextBoxEx mNotes;
		private AttachmentsListView mAttachments;
		private System.Windows.Forms.Panel mNotesBorder;
		private System.Windows.Forms.ToolTip mValidationMessage;
	}
}
