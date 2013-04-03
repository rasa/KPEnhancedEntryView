namespace KPEnhancedEntryView
{
	partial class EntryView
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

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
			this.mFieldsGrid = new KPEnhancedEntryView.FieldsListView();
			this.mSplitNotesAttachements = new System.Windows.Forms.SplitContainer();
			this.mNotesBorder = new System.Windows.Forms.Panel();
			this.mNotes = new KeePass.UI.CustomRichTextBoxEx();
			this.mAttachments = new KPEnhancedEntryView.AttachmentsListView();
			this.mValidationFailureReporter = new KPEnhancedEntryView.ValidationFailureReporter(this.components);
			this.mPropertiesTab = new System.Windows.Forms.TabPage();
			this.mPropertiesTabLayout = new System.Windows.Forms.TableLayoutPanel();
			this.mOverrideUrl = new System.Windows.Forms.TextBox();
			this.mCreationTimeLabel = new System.Windows.Forms.Label();
			this.mCreationTime = new System.Windows.Forms.Label();
			this.mAccessTimeLabel = new System.Windows.Forms.Label();
			this.mAccessTime = new System.Windows.Forms.Label();
			this.mModificationTimeLabel = new System.Windows.Forms.Label();
			this.mModificationTime = new System.Windows.Forms.Label();
			this.mExpiryTimeLabel = new System.Windows.Forms.Label();
			this.mExpiryTime = new System.Windows.Forms.Label();
			this.mSeparator = new System.Windows.Forms.Label();
			this.mSeparator2 = new System.Windows.Forms.Label();
			this.mGroupButton = new System.Windows.Forms.Button();
			this.mTagsLabel = new System.Windows.Forms.Label();
			this.mTags = new System.Windows.Forms.TextBox();
			this.mOverrideUrlLabel = new System.Windows.Forms.Label();
			this.mIconPanel = new System.Windows.Forms.Panel();
			this.m_lblIcon = new System.Windows.Forms.Label();
			this.m_btnIcon = new System.Windows.Forms.Button();
			this.mAllTextTab = new System.Windows.Forms.TabPage();
			this.mDoubleClickTimer = new System.Windows.Forms.Timer(this.components);
			this.mFieldGridContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mCopyCommand = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.mEditFieldCommand = new System.Windows.Forms.ToolStripMenuItem();
			this.mProtectFieldCommand = new System.Windows.Forms.ToolStripMenuItem();
			this.mPasswordGeneratorCommand = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.mDeleteFieldCommand = new System.Windows.Forms.ToolStripMenuItem();
			this.mAddNewCommand = new System.Windows.Forms.ToolStripMenuItem();
			this.mAttachmentsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mViewBinaryCommand = new System.Windows.Forms.ToolStripMenuItem();
			this.mRenameBinaryCommand = new System.Windows.Forms.ToolStripMenuItem();
			this.mSaveBinaryCommand = new System.Windows.Forms.ToolStripMenuItem();
			this.mDeleteBinaryCommand = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.mAttachBinaryCommand = new System.Windows.Forms.ToolStripMenuItem();
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
			this.mPropertiesTab.SuspendLayout();
			this.mPropertiesTabLayout.SuspendLayout();
			this.mIconPanel.SuspendLayout();
			this.mFieldGridContextMenu.SuspendLayout();
			this.mAttachmentsContextMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// mTabs
			// 
			this.mTabs.Controls.Add(this.mFieldsTab);
			this.mTabs.Controls.Add(this.mPropertiesTab);
			this.mTabs.Controls.Add(this.mAllTextTab);
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
			this.mFieldsTab.Size = new System.Drawing.Size(365, 316);
			this.mFieldsTab.TabIndex = 0;
			this.mFieldsTab.Text = "Fields";
			// 
			// mSplitGridPanels
			// 
			this.mSplitGridPanels.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mSplitGridPanels.Location = new System.Drawing.Point(0, 0);
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
			this.mSplitGridPanels.Size = new System.Drawing.Size(365, 316);
			this.mSplitGridPanels.SplitterDistance = 243;
			this.mSplitGridPanels.TabIndex = 2;
			// 
			// mFieldsGrid
			// 
			this.mFieldsGrid.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
			this.mFieldsGrid.CellEditTabChangesRows = true;
			this.mFieldsGrid.CopySelectionOnControlC = false;
			this.mFieldsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mFieldsGrid.Entry = null;
			this.mFieldsGrid.FullRowSelect = true;
			this.mFieldsGrid.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.mFieldsGrid.Location = new System.Drawing.Point(0, 0);
			this.mFieldsGrid.Name = "mFieldsGrid";
			this.mFieldsGrid.ShowGroups = false;
			this.mFieldsGrid.Size = new System.Drawing.Size(365, 243);
			this.mFieldsGrid.TabIndex = 0;
			this.mFieldsGrid.UseCellFormatEvents = true;
			this.mFieldsGrid.UseCompatibleStateImageBehavior = false;
			this.mFieldsGrid.UseHyperlinks = true;
			this.mFieldsGrid.ValidationFailureReporter = null;
			this.mFieldsGrid.View = System.Windows.Forms.View.Details;
			this.mFieldsGrid.VirtualMode = true;
			this.mFieldsGrid.CellRightClick += new System.EventHandler<BrightIdeasSoftware.CellRightClickEventArgs>(this.mFieldsGrid_CellRightClick);
			this.mFieldsGrid.HyperlinkClicked += new System.EventHandler<BrightIdeasSoftware.HyperlinkClickedEventArgs>(this.mFieldsGrid_HyperlinkClicked);
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
			this.mSplitNotesAttachements.Size = new System.Drawing.Size(365, 69);
			this.mSplitNotesAttachements.SplitterDistance = 254;
			this.mSplitNotesAttachements.TabIndex = 0;
			// 
			// mNotesBorder
			// 
			this.mNotesBorder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.mNotesBorder.Controls.Add(this.mNotes);
			this.mNotesBorder.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mNotesBorder.Location = new System.Drawing.Point(0, 0);
			this.mNotesBorder.Name = "mNotesBorder";
			this.mNotesBorder.Padding = new System.Windows.Forms.Padding(1);
			this.mNotesBorder.Size = new System.Drawing.Size(254, 69);
			this.mNotesBorder.TabIndex = 2;
			// 
			// mNotes
			// 
			this.mNotes.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.mNotes.DetectUrls = false;
			this.mNotes.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mNotes.Location = new System.Drawing.Point(1, 1);
			this.mNotes.Name = "mNotes";
			this.mNotes.Size = new System.Drawing.Size(250, 65);
			this.mNotes.TabIndex = 1;
			this.mNotes.Text = "";
			this.mNotes.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.mNotes_LinkClicked);
			this.mNotes.DoubleClick += new System.EventHandler(this.mNotes_DoubleClick);
			this.mNotes.Enter += new System.EventHandler(this.mNotes_Enter);
			this.mNotes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mNotes_KeyDown);
			this.mNotes.Leave += new System.EventHandler(this.mNotes_Leave);
			// 
			// mAttachments
			// 
			this.mAttachments.AllowDrop = true;
			this.mAttachments.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.F2Only;
			this.mAttachments.CopySelectionOnControlC = false;
			this.mAttachments.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mAttachments.EmptyListMsg = "Attachments";
			this.mAttachments.Entry = null;
			this.mAttachments.IsSimpleDragSource = true;
			this.mAttachments.Location = new System.Drawing.Point(0, 0);
			this.mAttachments.Name = "mAttachments";
			this.mAttachments.ShowGroups = false;
			this.mAttachments.Size = new System.Drawing.Size(107, 69);
			this.mAttachments.TabIndex = 0;
			this.mAttachments.UseCompatibleStateImageBehavior = false;
			this.mAttachments.ValidationFailureReporter = this.mValidationFailureReporter;
			this.mAttachments.View = System.Windows.Forms.View.SmallIcon;
			this.mAttachments.VirtualMode = true;
			this.mAttachments.EntryModified += new System.EventHandler(this.mAttachments_EntryModified);
			this.mAttachments.CellRightClick += new System.EventHandler<BrightIdeasSoftware.CellRightClickEventArgs>(this.mAttachments_CellRightClick);
			// 
			// mPropertiesTab
			// 
			this.mPropertiesTab.Controls.Add(this.mPropertiesTabLayout);
			this.mPropertiesTab.Location = new System.Drawing.Point(4, 22);
			this.mPropertiesTab.Name = "mPropertiesTab";
			this.mPropertiesTab.Size = new System.Drawing.Size(365, 316);
			this.mPropertiesTab.TabIndex = 1;
			this.mPropertiesTab.Text = "Properties";
			// 
			// mPropertiesTabLayout
			// 
			this.mPropertiesTabLayout.ColumnCount = 2;
			this.mPropertiesTabLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.mPropertiesTabLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.mPropertiesTabLayout.Controls.Add(this.mOverrideUrl, 1, 7);
			this.mPropertiesTabLayout.Controls.Add(this.mCreationTimeLabel, 0, 2);
			this.mPropertiesTabLayout.Controls.Add(this.mCreationTime, 1, 2);
			this.mPropertiesTabLayout.Controls.Add(this.mAccessTimeLabel, 0, 3);
			this.mPropertiesTabLayout.Controls.Add(this.mAccessTime, 1, 3);
			this.mPropertiesTabLayout.Controls.Add(this.mModificationTimeLabel, 0, 4);
			this.mPropertiesTabLayout.Controls.Add(this.mModificationTime, 1, 4);
			this.mPropertiesTabLayout.Controls.Add(this.mExpiryTimeLabel, 0, 5);
			this.mPropertiesTabLayout.Controls.Add(this.mExpiryTime, 1, 5);
			this.mPropertiesTabLayout.Controls.Add(this.mSeparator, 0, 1);
			this.mPropertiesTabLayout.Controls.Add(this.mSeparator2, 0, 6);
			this.mPropertiesTabLayout.Controls.Add(this.mGroupButton, 0, 0);
			this.mPropertiesTabLayout.Controls.Add(this.mTagsLabel, 0, 8);
			this.mPropertiesTabLayout.Controls.Add(this.mTags, 1, 8);
			this.mPropertiesTabLayout.Controls.Add(this.mOverrideUrlLabel, 0, 7);
			this.mPropertiesTabLayout.Controls.Add(this.mIconPanel, 1, 0);
			this.mPropertiesTabLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mPropertiesTabLayout.Location = new System.Drawing.Point(0, 0);
			this.mPropertiesTabLayout.Name = "mPropertiesTabLayout";
			this.mPropertiesTabLayout.RowCount = 10;
			this.mPropertiesTabLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mPropertiesTabLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mPropertiesTabLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mPropertiesTabLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mPropertiesTabLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mPropertiesTabLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mPropertiesTabLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mPropertiesTabLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mPropertiesTabLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mPropertiesTabLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.mPropertiesTabLayout.Size = new System.Drawing.Size(365, 316);
			this.mPropertiesTabLayout.TabIndex = 0;
			// 
			// mOverrideUrl
			// 
			this.mOverrideUrl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mOverrideUrl.Location = new System.Drawing.Point(98, 157);
			this.mOverrideUrl.Name = "mOverrideUrl";
			this.mOverrideUrl.Size = new System.Drawing.Size(264, 20);
			this.mOverrideUrl.TabIndex = 12;
			this.mOverrideUrl.Validated += new System.EventHandler(this.mOverrideUrl_Validated);
			// 
			// mCreationTimeLabel
			// 
			this.mCreationTimeLabel.AutoSize = true;
			this.mCreationTimeLabel.Location = new System.Drawing.Point(3, 59);
			this.mCreationTimeLabel.Margin = new System.Windows.Forms.Padding(3);
			this.mCreationTimeLabel.Name = "mCreationTimeLabel";
			this.mCreationTimeLabel.Size = new System.Drawing.Size(78, 13);
			this.mCreationTimeLabel.TabIndex = 2;
			this.mCreationTimeLabel.Text = "<dynamic text>";
			// 
			// mCreationTime
			// 
			this.mCreationTime.AutoSize = true;
			this.mCreationTime.Location = new System.Drawing.Point(98, 59);
			this.mCreationTime.Margin = new System.Windows.Forms.Padding(3);
			this.mCreationTime.Name = "mCreationTime";
			this.mCreationTime.Size = new System.Drawing.Size(78, 13);
			this.mCreationTime.TabIndex = 3;
			this.mCreationTime.Text = "<dynamic text>";
			// 
			// mAccessTimeLabel
			// 
			this.mAccessTimeLabel.AutoSize = true;
			this.mAccessTimeLabel.Location = new System.Drawing.Point(3, 78);
			this.mAccessTimeLabel.Margin = new System.Windows.Forms.Padding(3);
			this.mAccessTimeLabel.Name = "mAccessTimeLabel";
			this.mAccessTimeLabel.Size = new System.Drawing.Size(78, 13);
			this.mAccessTimeLabel.TabIndex = 4;
			this.mAccessTimeLabel.Text = "<dynamic text>";
			// 
			// mAccessTime
			// 
			this.mAccessTime.AutoSize = true;
			this.mAccessTime.Location = new System.Drawing.Point(98, 78);
			this.mAccessTime.Margin = new System.Windows.Forms.Padding(3);
			this.mAccessTime.Name = "mAccessTime";
			this.mAccessTime.Size = new System.Drawing.Size(78, 13);
			this.mAccessTime.TabIndex = 5;
			this.mAccessTime.Text = "<dynamic text>";
			// 
			// mModificationTimeLabel
			// 
			this.mModificationTimeLabel.AutoSize = true;
			this.mModificationTimeLabel.Location = new System.Drawing.Point(3, 97);
			this.mModificationTimeLabel.Margin = new System.Windows.Forms.Padding(3);
			this.mModificationTimeLabel.Name = "mModificationTimeLabel";
			this.mModificationTimeLabel.Size = new System.Drawing.Size(78, 13);
			this.mModificationTimeLabel.TabIndex = 6;
			this.mModificationTimeLabel.Text = "<dynamic text>";
			// 
			// mModificationTime
			// 
			this.mModificationTime.AutoSize = true;
			this.mModificationTime.Location = new System.Drawing.Point(98, 97);
			this.mModificationTime.Margin = new System.Windows.Forms.Padding(3);
			this.mModificationTime.Name = "mModificationTime";
			this.mModificationTime.Size = new System.Drawing.Size(78, 13);
			this.mModificationTime.TabIndex = 7;
			this.mModificationTime.Text = "<dynamic text>";
			// 
			// mExpiryTimeLabel
			// 
			this.mExpiryTimeLabel.AutoSize = true;
			this.mExpiryTimeLabel.Location = new System.Drawing.Point(3, 116);
			this.mExpiryTimeLabel.Margin = new System.Windows.Forms.Padding(3);
			this.mExpiryTimeLabel.Name = "mExpiryTimeLabel";
			this.mExpiryTimeLabel.Size = new System.Drawing.Size(78, 13);
			this.mExpiryTimeLabel.TabIndex = 8;
			this.mExpiryTimeLabel.Text = "<dynamic text>";
			// 
			// mExpiryTime
			// 
			this.mExpiryTime.AutoSize = true;
			this.mExpiryTime.Location = new System.Drawing.Point(98, 116);
			this.mExpiryTime.Margin = new System.Windows.Forms.Padding(3);
			this.mExpiryTime.Name = "mExpiryTime";
			this.mExpiryTime.Size = new System.Drawing.Size(78, 13);
			this.mExpiryTime.TabIndex = 9;
			this.mExpiryTime.Text = "<dynamic text>";
			// 
			// mSeparator
			// 
			this.mSeparator.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.mPropertiesTabLayout.SetColumnSpan(this.mSeparator, 2);
			this.mSeparator.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mSeparator.Location = new System.Drawing.Point(3, 44);
			this.mSeparator.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
			this.mSeparator.Name = "mSeparator";
			this.mSeparator.Size = new System.Drawing.Size(359, 2);
			this.mSeparator.TabIndex = 1;
			// 
			// mSeparator2
			// 
			this.mSeparator2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.mPropertiesTabLayout.SetColumnSpan(this.mSeparator2, 2);
			this.mSeparator2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mSeparator2.Location = new System.Drawing.Point(3, 142);
			this.mSeparator2.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
			this.mSeparator2.Name = "mSeparator2";
			this.mSeparator2.Size = new System.Drawing.Size(359, 2);
			this.mSeparator2.TabIndex = 10;
			// 
			// mGroupButton
			// 
			this.mGroupButton.AutoSize = true;
			this.mGroupButton.Location = new System.Drawing.Point(3, 8);
			this.mGroupButton.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
			this.mGroupButton.Name = "mGroupButton";
			this.mGroupButton.Size = new System.Drawing.Size(89, 23);
			this.mGroupButton.TabIndex = 0;
			this.mGroupButton.Text = "<dynamic text>";
			this.mGroupButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.mGroupButton.UseVisualStyleBackColor = true;
			this.mGroupButton.Click += new System.EventHandler(this.mGroupButton_Click);
			// 
			// mTagsLabel
			// 
			this.mTagsLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.mTagsLabel.AutoSize = true;
			this.mTagsLabel.Location = new System.Drawing.Point(3, 186);
			this.mTagsLabel.Margin = new System.Windows.Forms.Padding(3);
			this.mTagsLabel.Name = "mTagsLabel";
			this.mTagsLabel.Size = new System.Drawing.Size(78, 13);
			this.mTagsLabel.TabIndex = 13;
			this.mTagsLabel.Text = "<dynamic text>";
			// 
			// mTags
			// 
			this.mTags.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mTags.Location = new System.Drawing.Point(98, 183);
			this.mTags.Name = "mTags";
			this.mTags.Size = new System.Drawing.Size(264, 20);
			this.mTags.TabIndex = 14;
			this.mTags.Validated += new System.EventHandler(this.mTags_Validated);
			// 
			// mOverrideUrlLabel
			// 
			this.mOverrideUrlLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.mOverrideUrlLabel.AutoSize = true;
			this.mOverrideUrlLabel.Location = new System.Drawing.Point(3, 160);
			this.mOverrideUrlLabel.Margin = new System.Windows.Forms.Padding(3);
			this.mOverrideUrlLabel.Name = "mOverrideUrlLabel";
			this.mOverrideUrlLabel.Size = new System.Drawing.Size(78, 13);
			this.mOverrideUrlLabel.TabIndex = 11;
			this.mOverrideUrlLabel.Text = "<dynamic text>";
			// 
			// mIconPanel
			// 
			this.mIconPanel.AutoSize = true;
			this.mIconPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.mIconPanel.Controls.Add(this.m_lblIcon);
			this.mIconPanel.Controls.Add(this.m_btnIcon);
			this.mIconPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mIconPanel.Location = new System.Drawing.Point(95, 0);
			this.mIconPanel.Margin = new System.Windows.Forms.Padding(0);
			this.mIconPanel.Name = "mIconPanel";
			this.mIconPanel.Size = new System.Drawing.Size(270, 34);
			this.mIconPanel.TabIndex = 15;
			// 
			// m_lblIcon
			// 
			this.m_lblIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_lblIcon.AutoSize = true;
			this.m_lblIcon.Location = new System.Drawing.Point(198, 13);
			this.m_lblIcon.Name = "m_lblIcon";
			this.m_lblIcon.Size = new System.Drawing.Size(31, 13);
			this.m_lblIcon.TabIndex = 4;
			this.m_lblIcon.Text = "Icon:";
			this.m_lblIcon.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// m_btnIcon
			// 
			this.m_btnIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_btnIcon.Location = new System.Drawing.Point(235, 8);
			this.m_btnIcon.Name = "m_btnIcon";
			this.m_btnIcon.Size = new System.Drawing.Size(32, 23);
			this.m_btnIcon.TabIndex = 5;
			this.m_btnIcon.UseVisualStyleBackColor = true;
			this.m_btnIcon.Click += new System.EventHandler(this.m_btnIcon_Click);
			// 
			// mAllTextTab
			// 
			this.mAllTextTab.Location = new System.Drawing.Point(4, 22);
			this.mAllTextTab.Name = "mAllTextTab";
			this.mAllTextTab.Size = new System.Drawing.Size(365, 316);
			this.mAllTextTab.TabIndex = 2;
			this.mAllTextTab.Text = "All Text";
			// 
			// mDoubleClickTimer
			// 
			this.mDoubleClickTimer.Tick += new System.EventHandler(this.mDoubleClickTimer_Tick);
			// 
			// mFieldGridContextMenu
			// 
			this.mFieldGridContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mCopyCommand,
            this.toolStripSeparator1,
            this.mEditFieldCommand,
            this.mProtectFieldCommand,
            this.mPasswordGeneratorCommand,
            this.toolStripSeparator2,
            this.mDeleteFieldCommand,
            this.mAddNewCommand});
			this.mFieldGridContextMenu.Name = "mFieldGridContextMenu";
			this.mFieldGridContextMenu.Size = new System.Drawing.Size(221, 148);
			// 
			// mCopyCommand
			// 
			this.mCopyCommand.Image = global::KPEnhancedEntryView.Properties.Resources.B16x16_KGPG_Info;
			this.mCopyCommand.Name = "mCopyCommand";
			this.mCopyCommand.Size = new System.Drawing.Size(220, 22);
			this.mCopyCommand.Text = "Copy {0}";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(217, 6);
			// 
			// mEditFieldCommand
			// 
			this.mEditFieldCommand.Image = global::KPEnhancedEntryView.Properties.Resources.B16x16_KGPG_Sign;
			this.mEditFieldCommand.Name = "mEditFieldCommand";
			this.mEditFieldCommand.Size = new System.Drawing.Size(220, 22);
			this.mEditFieldCommand.Text = "&Edit Field";
			// 
			// mProtectFieldCommand
			// 
			this.mProtectFieldCommand.CheckOnClick = true;
			this.mProtectFieldCommand.Name = "mProtectFieldCommand";
			this.mProtectFieldCommand.Size = new System.Drawing.Size(220, 22);
			this.mProtectFieldCommand.Text = "&Protect Field";
			// 
			// mPasswordGeneratorCommand
			// 
			this.mPasswordGeneratorCommand.Image = global::KPEnhancedEntryView.Properties.Resources.B16x16_Key_New;
			this.mPasswordGeneratorCommand.Name = "mPasswordGeneratorCommand";
			this.mPasswordGeneratorCommand.Size = new System.Drawing.Size(220, 22);
			this.mPasswordGeneratorCommand.Text = "&Open Password Generator...";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(217, 6);
			// 
			// mDeleteFieldCommand
			// 
			this.mDeleteFieldCommand.Image = global::KPEnhancedEntryView.Properties.Resources.B16x16_DeleteEntry;
			this.mDeleteFieldCommand.Name = "mDeleteFieldCommand";
			this.mDeleteFieldCommand.Size = new System.Drawing.Size(220, 22);
			this.mDeleteFieldCommand.Text = "&Delete Field";
			// 
			// mAddNewCommand
			// 
			this.mAddNewCommand.Image = global::KPEnhancedEntryView.Properties.Resources.B16x16_KGPG_Import;
			this.mAddNewCommand.Name = "mAddNewCommand";
			this.mAddNewCommand.Size = new System.Drawing.Size(220, 22);
			this.mAddNewCommand.Text = "&Add New Field";
			// 
			// mAttachmentsContextMenu
			// 
			this.mAttachmentsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mViewBinaryCommand,
            this.mRenameBinaryCommand,
            this.mSaveBinaryCommand,
            this.mDeleteBinaryCommand,
            this.toolStripSeparator3,
            this.mAttachBinaryCommand});
			this.mAttachmentsContextMenu.Name = "mAttachmentsContextMenu";
			this.mAttachmentsContextMenu.Size = new System.Drawing.Size(153, 142);
			// 
			// mViewBinaryCommand
			// 
			this.mViewBinaryCommand.Name = "mViewBinaryCommand";
			this.mViewBinaryCommand.Size = new System.Drawing.Size(152, 22);
			this.mViewBinaryCommand.Text = "&View";
			this.mViewBinaryCommand.Click += new System.EventHandler(this.mViewBinaryCommand_Click);
			// 
			// mRenameBinaryCommand
			// 
			this.mRenameBinaryCommand.Name = "mRenameBinaryCommand";
			this.mRenameBinaryCommand.Size = new System.Drawing.Size(152, 22);
			this.mRenameBinaryCommand.Text = "&Rename";
			this.mRenameBinaryCommand.Click += new System.EventHandler(this.mRenameBinaryCommand_Click);
			// 
			// mSaveBinaryCommand
			// 
			this.mSaveBinaryCommand.Image = global::KPEnhancedEntryView.Properties.Resources.B16x16_Attach;
			this.mSaveBinaryCommand.Name = "mSaveBinaryCommand";
			this.mSaveBinaryCommand.Size = new System.Drawing.Size(152, 22);
			this.mSaveBinaryCommand.Text = "&Save";
			this.mSaveBinaryCommand.Click += new System.EventHandler(this.mSaveBinaryCommand_Click);
			// 
			// mDeleteBinaryCommand
			// 
			this.mDeleteBinaryCommand.Image = global::KPEnhancedEntryView.Properties.Resources.B16x16_EditDelete;
			this.mDeleteBinaryCommand.Name = "mDeleteBinaryCommand";
			this.mDeleteBinaryCommand.Size = new System.Drawing.Size(152, 22);
			this.mDeleteBinaryCommand.Text = "&Delete";
			this.mDeleteBinaryCommand.Click += new System.EventHandler(this.mDeleteBinaryCommand_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(149, 6);
			// 
			// mAttachBinaryCommand
			// 
			this.mAttachBinaryCommand.Image = global::KPEnhancedEntryView.Properties.Resources.B16x16_Folder_Yellow_Open;
			this.mAttachBinaryCommand.Name = "mAttachBinaryCommand";
			this.mAttachBinaryCommand.Size = new System.Drawing.Size(152, 22);
			this.mAttachBinaryCommand.Text = "Attach &File(s)...";
			this.mAttachBinaryCommand.Click += new System.EventHandler(this.mAttachBinaryCommand_Click);
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
			this.mPropertiesTab.ResumeLayout(false);
			this.mPropertiesTabLayout.ResumeLayout(false);
			this.mPropertiesTabLayout.PerformLayout();
			this.mIconPanel.ResumeLayout(false);
			this.mIconPanel.PerformLayout();
			this.mFieldGridContextMenu.ResumeLayout(false);
			this.mAttachmentsContextMenu.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl mTabs;
		private System.Windows.Forms.TabPage mFieldsTab;
		private System.Windows.Forms.TabPage mPropertiesTab;
		private FieldsListView mFieldsGrid;
		private System.Windows.Forms.SplitContainer mSplitGridPanels;
		private System.Windows.Forms.SplitContainer mSplitNotesAttachements;
		private KeePass.UI.CustomRichTextBoxEx mNotes;
		private AttachmentsListView mAttachments;
		private System.Windows.Forms.Panel mNotesBorder;
		private ValidationFailureReporter mValidationFailureReporter;
		private System.Windows.Forms.Timer mDoubleClickTimer;
		private System.Windows.Forms.TabPage mAllTextTab;
		private System.Windows.Forms.TableLayoutPanel mPropertiesTabLayout;
		private System.Windows.Forms.Button mGroupButton;
		private System.Windows.Forms.Label mCreationTimeLabel;
		private System.Windows.Forms.Label mCreationTime;
		private System.Windows.Forms.Label mAccessTimeLabel;
		private System.Windows.Forms.Label mAccessTime;
		private System.Windows.Forms.Label mModificationTimeLabel;
		private System.Windows.Forms.Label mModificationTime;
		private System.Windows.Forms.Label mExpiryTimeLabel;
		private System.Windows.Forms.Label mExpiryTime;
		private System.Windows.Forms.Label mSeparator;
		private System.Windows.Forms.Label mSeparator2;
		private System.Windows.Forms.Label mTagsLabel;
		private System.Windows.Forms.TextBox mTags;
		private System.Windows.Forms.TextBox mOverrideUrl;
		private System.Windows.Forms.Label mOverrideUrlLabel;
		private System.Windows.Forms.Panel mIconPanel;
		private System.Windows.Forms.Label m_lblIcon;
		private System.Windows.Forms.Button m_btnIcon;
		private System.Windows.Forms.ContextMenuStrip mFieldGridContextMenu;
		private System.Windows.Forms.ToolStripMenuItem mCopyCommand;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem mEditFieldCommand;
		private System.Windows.Forms.ToolStripMenuItem mProtectFieldCommand;
		private System.Windows.Forms.ToolStripMenuItem mPasswordGeneratorCommand;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem mDeleteFieldCommand;
		private System.Windows.Forms.ToolStripMenuItem mAddNewCommand;
		private System.Windows.Forms.ContextMenuStrip mAttachmentsContextMenu;
		private System.Windows.Forms.ToolStripMenuItem mViewBinaryCommand;
		private System.Windows.Forms.ToolStripMenuItem mSaveBinaryCommand;
		private System.Windows.Forms.ToolStripMenuItem mDeleteBinaryCommand;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem mAttachBinaryCommand;
		private System.Windows.Forms.ToolStripMenuItem mRenameBinaryCommand;
	}
}
