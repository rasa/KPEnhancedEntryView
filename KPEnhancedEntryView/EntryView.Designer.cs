using System.Drawing;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EntryView));
			this.mSingleEntryTabs = new System.Windows.Forms.TabControl();
			this.mFieldsTab = new System.Windows.Forms.TabPage();
			this.mPropertiesTab = new System.Windows.Forms.TabPage();
			this.mPropertiesTabScrollPanel = new System.Windows.Forms.Panel();
			this.mAutoTypeLayout = new System.Windows.Forms.TableLayoutPanel();
			this.m_cbAutoTypeObfuscation = new System.Windows.Forms.CheckBox();
			this.m_cbAutoTypeEnabled = new System.Windows.Forms.CheckBox();
			this.mSeparator3 = new System.Windows.Forms.Label();
			this.mTextPropertiesLayout = new System.Windows.Forms.TableLayoutPanel();
			this.mUUID = new System.Windows.Forms.TextBox();
			this.mUUIDLabel = new System.Windows.Forms.Label();
			this.mOverrideUrlLabel = new System.Windows.Forms.Label();
			this.m_cmbOverrideUrl = new KeePass.UI.ImageComboBoxEx();
			this.mTagsLabel = new System.Windows.Forms.Label();
			this.mTags = new System.Windows.Forms.TextBox();
			this.mCustomColoursLayout = new System.Windows.Forms.TableLayoutPanel();
			this.mSeparator2 = new System.Windows.Forms.Label();
			this.m_btnPickFgColor = new System.Windows.Forms.Button();
			this.m_cbCustomForegroundColor = new System.Windows.Forms.CheckBox();
			this.m_cbCustomBackgroundColor = new System.Windows.Forms.CheckBox();
			this.m_btnPickBgColor = new System.Windows.Forms.Button();
			this.mTimestampsLayout = new System.Windows.Forms.TableLayoutPanel();
			this.mCreationTimeLabel = new System.Windows.Forms.Label();
			this.mCreationTime = new System.Windows.Forms.Label();
			this.mAccessTimeLabel = new System.Windows.Forms.Label();
			this.mAccessTime = new System.Windows.Forms.Label();
			this.mModificationTimeLabel = new System.Windows.Forms.Label();
			this.mModificationTime = new System.Windows.Forms.Label();
			this.mSeparator = new System.Windows.Forms.Label();
			this.m_cbExpires = new System.Windows.Forms.CheckBox();
			this.m_dtExpireDateTime = new System.Windows.Forms.DateTimePicker();
			this.m_btnStandardExpires = new System.Windows.Forms.Button();
			this.m_ctxDefaultTimes = new KeePass.UI.CustomContextMenuStripEx(this.components);
			this.m_menuExpireNow = new System.Windows.Forms.ToolStripMenuItem();
			this.m_menuExpireSep0 = new System.Windows.Forms.ToolStripSeparator();
			this.m_menuExpire1Week = new System.Windows.Forms.ToolStripMenuItem();
			this.m_menuExpire2Weeks = new System.Windows.Forms.ToolStripMenuItem();
			this.m_menuExpireSep1 = new System.Windows.Forms.ToolStripSeparator();
			this.m_menuExpire1Month = new System.Windows.Forms.ToolStripMenuItem();
			this.m_menuExpire3Months = new System.Windows.Forms.ToolStripMenuItem();
			this.m_menuExpire6Months = new System.Windows.Forms.ToolStripMenuItem();
			this.m_menuExpireSep2 = new System.Windows.Forms.ToolStripSeparator();
			this.m_menuExpire1Year = new System.Windows.Forms.ToolStripMenuItem();
			this.mIconPanel = new System.Windows.Forms.Panel();
			this.mGroupButton = new System.Windows.Forms.Button();
			this.m_lblIcon = new System.Windows.Forms.Label();
			this.m_btnIcon = new System.Windows.Forms.Button();
			this.mAllTextTab = new System.Windows.Forms.TabPage();
			this.mMultipleSelectionTab = new System.Windows.Forms.TabPage();
			this.mDoubleClickTimer = new System.Windows.Forms.Timer(this.components);
			this.mFieldGridContextMenu = new KeePass.UI.CustomContextMenuStripEx(this.components);
			this.mURLDropDown = new System.Windows.Forms.ToolStripMenuItem();
			this.mOpenURLCommand = new System.Windows.Forms.ToolStripMenuItem();
			this.mCopyCommand = new System.Windows.Forms.ToolStripMenuItem();
			this.mAutoTypeCommand = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.mEditFieldCommand = new System.Windows.Forms.ToolStripMenuItem();
			this.mProtectFieldCommand = new System.Windows.Forms.ToolStripMenuItem();
			this.mPasswordGeneratorCommand = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.mDeleteFieldCommand = new System.Windows.Forms.ToolStripMenuItem();
			this.mAddNewCommand = new System.Windows.Forms.ToolStripMenuItem();
			this.mAttachmentsContextMenu = new KeePass.UI.CustomContextMenuStripEx(this.components);
			this.mViewBinaryCommand = new System.Windows.Forms.ToolStripMenuItem();
			this.mRenameBinaryCommand = new System.Windows.Forms.ToolStripMenuItem();
			this.mSaveBinaryCommand = new System.Windows.Forms.ToolStripMenuItem();
			this.mDeleteBinaryCommand = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.mAttachBinaryCommand = new System.Windows.Forms.ToolStripMenuItem();
			this.mMultipleEntriesTabs = new System.Windows.Forms.TabControl();
			this.mLockButton = new System.Windows.Forms.CheckBox();
			this.mLockImages = new System.Windows.Forms.ImageList(this.components);
			this.mSplitGridPanels = new KPEnhancedEntryView.CollapsibleSplitContainer();
			this.mFieldsGrid = new KPEnhancedEntryView.SingleEntryFieldsListView();
			this.mValidationFailureReporter = new KPEnhancedEntryView.ValidationFailureReporter(this.components);
			this.mSplitNotesAttachements = new KPEnhancedEntryView.CollapsibleSplitContainer();
			this.mNotesBorder = new System.Windows.Forms.Panel();
			this.mNotes = new KeePass.UI.CustomRichTextBoxEx();
			this.mAttachments = new KPEnhancedEntryView.AttachmentsListView();
			this.mMultipleSelectionFields = new KPEnhancedEntryView.MultipleEntriesFieldsListView();
			this.mSingleEntryTabs.SuspendLayout();
			this.mFieldsTab.SuspendLayout();
			this.mPropertiesTab.SuspendLayout();
			this.mPropertiesTabScrollPanel.SuspendLayout();
			this.mAutoTypeLayout.SuspendLayout();
			this.mTextPropertiesLayout.SuspendLayout();
			this.mCustomColoursLayout.SuspendLayout();
			this.mTimestampsLayout.SuspendLayout();
			this.m_ctxDefaultTimes.SuspendLayout();
			this.mIconPanel.SuspendLayout();
			this.mMultipleSelectionTab.SuspendLayout();
			this.mFieldGridContextMenu.SuspendLayout();
			this.mAttachmentsContextMenu.SuspendLayout();
			this.mMultipleEntriesTabs.SuspendLayout();
			this.mSplitGridPanels.Panel1.SuspendLayout();
			this.mSplitGridPanels.Panel2.SuspendLayout();
			this.mSplitGridPanels.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.mFieldsGrid)).BeginInit();
			this.mSplitNotesAttachements.Panel1.SuspendLayout();
			this.mSplitNotesAttachements.Panel2.SuspendLayout();
			this.mSplitNotesAttachements.SuspendLayout();
			this.mNotesBorder.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.mAttachments)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.mMultipleSelectionFields)).BeginInit();
			this.SuspendLayout();
			// 
			// mSingleEntryTabs
			// 
			this.mSingleEntryTabs.Controls.Add(this.mFieldsTab);
			this.mSingleEntryTabs.Controls.Add(this.mPropertiesTab);
			this.mSingleEntryTabs.Controls.Add(this.mAllTextTab);
			this.mSingleEntryTabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mSingleEntryTabs.Location = new System.Drawing.Point(0, 0);
			this.mSingleEntryTabs.Name = "mSingleEntryTabs";
			this.mSingleEntryTabs.SelectedIndex = 0;
			this.mSingleEntryTabs.Size = new System.Drawing.Size(373, 436);
			this.mSingleEntryTabs.TabIndex = 1;
			// 
			// mFieldsTab
			// 
			this.mFieldsTab.Controls.Add(this.mSplitGridPanels);
			this.mFieldsTab.Location = new System.Drawing.Point(4, 22);
			this.mFieldsTab.Name = "mFieldsTab";
			this.mFieldsTab.Size = new System.Drawing.Size(365, 410);
			this.mFieldsTab.TabIndex = 0;
			this.mFieldsTab.Text = "Fields";
			// 
			// mPropertiesTab
			// 
			this.mPropertiesTab.Controls.Add(this.mPropertiesTabScrollPanel);
			this.mPropertiesTab.Location = new System.Drawing.Point(4, 22);
			this.mPropertiesTab.Name = "mPropertiesTab";
			this.mPropertiesTab.Size = new System.Drawing.Size(365, 410);
			this.mPropertiesTab.TabIndex = 1;
			this.mPropertiesTab.Text = "Properties";
			// 
			// mPropertiesTabScrollPanel
			// 
			this.mPropertiesTabScrollPanel.AutoScroll = true;
			this.mPropertiesTabScrollPanel.AutoScrollMinSize = new System.Drawing.Size(220, 0);
			this.mPropertiesTabScrollPanel.Controls.Add(this.mAutoTypeLayout);
			this.mPropertiesTabScrollPanel.Controls.Add(this.mTextPropertiesLayout);
			this.mPropertiesTabScrollPanel.Controls.Add(this.mCustomColoursLayout);
			this.mPropertiesTabScrollPanel.Controls.Add(this.mTimestampsLayout);
			this.mPropertiesTabScrollPanel.Controls.Add(this.mIconPanel);
			this.mPropertiesTabScrollPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mPropertiesTabScrollPanel.Location = new System.Drawing.Point(0, 0);
			this.mPropertiesTabScrollPanel.Name = "mPropertiesTabScrollPanel";
			this.mPropertiesTabScrollPanel.Size = new System.Drawing.Size(365, 410);
			this.mPropertiesTabScrollPanel.TabIndex = 16;
			// 
			// mAutoTypeLayout
			// 
			this.mAutoTypeLayout.AutoSize = true;
			this.mAutoTypeLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.mAutoTypeLayout.ColumnCount = 1;
			this.mAutoTypeLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.mAutoTypeLayout.Controls.Add(this.m_cbAutoTypeObfuscation, 0, 2);
			this.mAutoTypeLayout.Controls.Add(this.m_cbAutoTypeEnabled, 0, 1);
			this.mAutoTypeLayout.Controls.Add(this.mSeparator3, 0, 0);
			this.mAutoTypeLayout.Dock = System.Windows.Forms.DockStyle.Top;
			this.mAutoTypeLayout.Location = new System.Drawing.Point(0, 291);
			this.mAutoTypeLayout.Name = "mAutoTypeLayout";
			this.mAutoTypeLayout.RowCount = 3;
			this.mAutoTypeLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mAutoTypeLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mAutoTypeLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mAutoTypeLayout.Size = new System.Drawing.Size(365, 63);
			this.mAutoTypeLayout.TabIndex = 4;
			// 
			// m_cbAutoTypeObfuscation
			// 
			this.m_cbAutoTypeObfuscation.AutoSize = true;
			this.m_cbAutoTypeObfuscation.Location = new System.Drawing.Point(7, 43);
			this.m_cbAutoTypeObfuscation.Margin = new System.Windows.Forms.Padding(7, 3, 3, 3);
			this.m_cbAutoTypeObfuscation.Name = "m_cbAutoTypeObfuscation";
			this.m_cbAutoTypeObfuscation.Size = new System.Drawing.Size(193, 17);
			this.m_cbAutoTypeObfuscation.TabIndex = 13;
			this.m_cbAutoTypeObfuscation.Text = "Two-channel auto-type obfuscation";
			this.m_cbAutoTypeObfuscation.UseVisualStyleBackColor = true;
			this.m_cbAutoTypeObfuscation.Click += new System.EventHandler(this.m_cbAutoTypeObfuscation_Click);
			// 
			// m_cbAutoTypeEnabled
			// 
			this.m_cbAutoTypeEnabled.AutoSize = true;
			this.m_cbAutoTypeEnabled.Location = new System.Drawing.Point(7, 20);
			this.m_cbAutoTypeEnabled.Margin = new System.Windows.Forms.Padding(7, 3, 3, 3);
			this.m_cbAutoTypeEnabled.Name = "m_cbAutoTypeEnabled";
			this.m_cbAutoTypeEnabled.Size = new System.Drawing.Size(166, 17);
			this.m_cbAutoTypeEnabled.TabIndex = 2;
			this.m_cbAutoTypeEnabled.Text = "Enable auto-type for this entry";
			this.m_cbAutoTypeEnabled.UseVisualStyleBackColor = true;
			this.m_cbAutoTypeEnabled.Click += new System.EventHandler(this.m_cbAutoTypeEnabled_Click);
			// 
			// mSeparator3
			// 
			this.mSeparator3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.mAutoTypeLayout.SetColumnSpan(this.mSeparator3, 2);
			this.mSeparator3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mSeparator3.Location = new System.Drawing.Point(3, 10);
			this.mSeparator3.Margin = new System.Windows.Forms.Padding(3, 10, 3, 5);
			this.mSeparator3.Name = "mSeparator3";
			this.mSeparator3.Size = new System.Drawing.Size(359, 2);
			this.mSeparator3.TabIndex = 1;
			// 
			// mTextPropertiesLayout
			// 
			this.mTextPropertiesLayout.AutoSize = true;
			this.mTextPropertiesLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.mTextPropertiesLayout.ColumnCount = 2;
			this.mTextPropertiesLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.mTextPropertiesLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.mTextPropertiesLayout.Controls.Add(this.mUUID, 1, 2);
			this.mTextPropertiesLayout.Controls.Add(this.mUUIDLabel, 0, 2);
			this.mTextPropertiesLayout.Controls.Add(this.mOverrideUrlLabel, 0, 1);
			this.mTextPropertiesLayout.Controls.Add(this.m_cmbOverrideUrl, 1, 1);
			this.mTextPropertiesLayout.Controls.Add(this.mTagsLabel, 0, 0);
			this.mTextPropertiesLayout.Controls.Add(this.mTags, 1, 0);
			this.mTextPropertiesLayout.Dock = System.Windows.Forms.DockStyle.Top;
			this.mTextPropertiesLayout.Location = new System.Drawing.Point(0, 209);
			this.mTextPropertiesLayout.Name = "mTextPropertiesLayout";
			this.mTextPropertiesLayout.RowCount = 3;
			this.mTextPropertiesLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mTextPropertiesLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mTextPropertiesLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mTextPropertiesLayout.Size = new System.Drawing.Size(365, 82);
			this.mTextPropertiesLayout.TabIndex = 3;
			// 
			// mUUID
			// 
			this.mUUID.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mUUID.Location = new System.Drawing.Point(87, 59);
			this.mUUID.Name = "mUUID";
			this.mUUID.ReadOnly = true;
			this.mUUID.Size = new System.Drawing.Size(275, 20);
			this.mUUID.TabIndex = 5;
			this.mUUID.GotFocus += new System.EventHandler(this.mUUID_Enter);
			// 
			// mUUIDLabel
			// 
			this.mUUIDLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.mUUIDLabel.AutoSize = true;
			this.mUUIDLabel.Location = new System.Drawing.Point(3, 61);
			this.mUUIDLabel.Margin = new System.Windows.Forms.Padding(3, 1, 3, 3);
			this.mUUIDLabel.Name = "mUUIDLabel";
			this.mUUIDLabel.Size = new System.Drawing.Size(78, 13);
			this.mUUIDLabel.TabIndex = 4;
			this.mUUIDLabel.Text = "<dynamic text>";
			// 
			// mOverrideUrlLabel
			// 
			this.mOverrideUrlLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.mOverrideUrlLabel.AutoSize = true;
			this.mOverrideUrlLabel.Location = new System.Drawing.Point(3, 35);
			this.mOverrideUrlLabel.Margin = new System.Windows.Forms.Padding(3, 1, 3, 3);
			this.mOverrideUrlLabel.Name = "mOverrideUrlLabel";
			this.mOverrideUrlLabel.Size = new System.Drawing.Size(78, 13);
			this.mOverrideUrlLabel.TabIndex = 2;
			this.mOverrideUrlLabel.Text = "<dynamic text>";
			// 
			// m_cmbOverrideUrl
			// 
			this.m_cmbOverrideUrl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_cmbOverrideUrl.Location = new System.Drawing.Point(87, 32);
			this.m_cmbOverrideUrl.Name = "m_cmbOverrideUrl";
			this.m_cmbOverrideUrl.Size = new System.Drawing.Size(275, 21);
			this.m_cmbOverrideUrl.TabIndex = 3;
			this.m_cmbOverrideUrl.LostFocus += new System.EventHandler(this.m_cmbOverrideUrl_LostFocus);
			// 
			// mTagsLabel
			// 
			this.mTagsLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.mTagsLabel.AutoSize = true;
			this.mTagsLabel.Location = new System.Drawing.Point(3, 8);
			this.mTagsLabel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 3);
			this.mTagsLabel.Name = "mTagsLabel";
			this.mTagsLabel.Size = new System.Drawing.Size(78, 13);
			this.mTagsLabel.TabIndex = 0;
			this.mTagsLabel.Text = "<dynamic text>";
			// 
			// mTags
			// 
			this.mTags.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mTags.Location = new System.Drawing.Point(87, 6);
			this.mTags.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
			this.mTags.Name = "mTags";
			this.mTags.Size = new System.Drawing.Size(275, 20);
			this.mTags.TabIndex = 1;
			this.mTags.LostFocus += new System.EventHandler(this.mTags_LostFocus);
			// 
			// mCustomColoursLayout
			// 
			this.mCustomColoursLayout.AutoSize = true;
			this.mCustomColoursLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.mCustomColoursLayout.ColumnCount = 2;
			this.mCustomColoursLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.mCustomColoursLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.mCustomColoursLayout.Controls.Add(this.mSeparator2, 0, 0);
			this.mCustomColoursLayout.Controls.Add(this.m_btnPickFgColor, 1, 1);
			this.mCustomColoursLayout.Controls.Add(this.m_cbCustomForegroundColor, 0, 1);
			this.mCustomColoursLayout.Controls.Add(this.m_cbCustomBackgroundColor, 0, 2);
			this.mCustomColoursLayout.Controls.Add(this.m_btnPickBgColor, 1, 2);
			this.mCustomColoursLayout.Dock = System.Windows.Forms.DockStyle.Top;
			this.mCustomColoursLayout.Location = new System.Drawing.Point(0, 140);
			this.mCustomColoursLayout.Name = "mCustomColoursLayout";
			this.mCustomColoursLayout.RowCount = 3;
			this.mCustomColoursLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mCustomColoursLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mCustomColoursLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mCustomColoursLayout.Size = new System.Drawing.Size(365, 69);
			this.mCustomColoursLayout.TabIndex = 2;
			// 
			// mSeparator2
			// 
			this.mSeparator2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.mCustomColoursLayout.SetColumnSpan(this.mSeparator2, 2);
			this.mSeparator2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mSeparator2.Location = new System.Drawing.Point(3, 10);
			this.mSeparator2.Margin = new System.Windows.Forms.Padding(3, 10, 3, 5);
			this.mSeparator2.Name = "mSeparator2";
			this.mSeparator2.Size = new System.Drawing.Size(359, 2);
			this.mSeparator2.TabIndex = 0;
			// 
			// m_btnPickFgColor
			// 
			this.m_btnPickFgColor.Enabled = false;
			this.m_btnPickFgColor.Location = new System.Drawing.Point(163, 20);
			this.m_btnPickFgColor.Name = "m_btnPickFgColor";
			this.m_btnPickFgColor.Size = new System.Drawing.Size(48, 20);
			this.m_btnPickFgColor.TabIndex = 1;
			this.m_btnPickFgColor.UseVisualStyleBackColor = true;
			this.m_btnPickFgColor.Click += new System.EventHandler(this.m_btnPickFgColor_Click);
			// 
			// m_cbCustomForegroundColor
			// 
			this.m_cbCustomForegroundColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_cbCustomForegroundColor.AutoSize = true;
			this.m_cbCustomForegroundColor.Location = new System.Drawing.Point(7, 23);
			this.m_cbCustomForegroundColor.Margin = new System.Windows.Forms.Padding(7, 3, 3, 3);
			this.m_cbCustomForegroundColor.Name = "m_cbCustomForegroundColor";
			this.m_cbCustomForegroundColor.Size = new System.Drawing.Size(144, 17);
			this.m_cbCustomForegroundColor.TabIndex = 0;
			this.m_cbCustomForegroundColor.Text = "Custom foreground color:";
			this.m_cbCustomForegroundColor.UseVisualStyleBackColor = true;
			this.m_cbCustomForegroundColor.CheckedChanged += new System.EventHandler(this.m_cbCustomForegroundColor_CheckedChanged);
			this.m_cbCustomForegroundColor.Click += new System.EventHandler(this.m_cbCustomForegroundColor_Click);
			// 
			// m_cbCustomBackgroundColor
			// 
			this.m_cbCustomBackgroundColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_cbCustomBackgroundColor.AutoSize = true;
			this.m_cbCustomBackgroundColor.Location = new System.Drawing.Point(7, 49);
			this.m_cbCustomBackgroundColor.Margin = new System.Windows.Forms.Padding(7, 3, 3, 3);
			this.m_cbCustomBackgroundColor.Name = "m_cbCustomBackgroundColor";
			this.m_cbCustomBackgroundColor.Size = new System.Drawing.Size(150, 17);
			this.m_cbCustomBackgroundColor.TabIndex = 2;
			this.m_cbCustomBackgroundColor.Text = "Custom background color:";
			this.m_cbCustomBackgroundColor.UseVisualStyleBackColor = true;
			this.m_cbCustomBackgroundColor.CheckedChanged += new System.EventHandler(this.m_cbCustomBackgroundColor_CheckedChanged);
			this.m_cbCustomBackgroundColor.Click += new System.EventHandler(this.m_cbCustomBackgroundColor_Click);
			// 
			// m_btnPickBgColor
			// 
			this.m_btnPickBgColor.Enabled = false;
			this.m_btnPickBgColor.Location = new System.Drawing.Point(163, 46);
			this.m_btnPickBgColor.Name = "m_btnPickBgColor";
			this.m_btnPickBgColor.Size = new System.Drawing.Size(48, 20);
			this.m_btnPickBgColor.TabIndex = 3;
			this.m_btnPickBgColor.UseVisualStyleBackColor = true;
			this.m_btnPickBgColor.Click += new System.EventHandler(this.m_btnPickBgColor_Click);
			// 
			// mTimestampsLayout
			// 
			this.mTimestampsLayout.AutoSize = true;
			this.mTimestampsLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.mTimestampsLayout.ColumnCount = 3;
			this.mTimestampsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.mTimestampsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.mTimestampsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.mTimestampsLayout.Controls.Add(this.mCreationTimeLabel, 0, 2);
			this.mTimestampsLayout.Controls.Add(this.mCreationTime, 1, 2);
			this.mTimestampsLayout.Controls.Add(this.mAccessTimeLabel, 0, 3);
			this.mTimestampsLayout.Controls.Add(this.mAccessTime, 1, 3);
			this.mTimestampsLayout.Controls.Add(this.mModificationTimeLabel, 0, 4);
			this.mTimestampsLayout.Controls.Add(this.mModificationTime, 1, 4);
			this.mTimestampsLayout.Controls.Add(this.mSeparator, 0, 0);
			this.mTimestampsLayout.Controls.Add(this.m_cbExpires, 0, 5);
			this.mTimestampsLayout.Controls.Add(this.m_dtExpireDateTime, 1, 5);
			this.mTimestampsLayout.Controls.Add(this.m_btnStandardExpires, 2, 5);
			this.mTimestampsLayout.Dock = System.Windows.Forms.DockStyle.Top;
			this.mTimestampsLayout.Location = new System.Drawing.Point(0, 34);
			this.mTimestampsLayout.Name = "mTimestampsLayout";
			this.mTimestampsLayout.RowCount = 10;
			this.mTimestampsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mTimestampsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mTimestampsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mTimestampsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mTimestampsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mTimestampsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mTimestampsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mTimestampsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mTimestampsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mTimestampsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.mTimestampsLayout.Size = new System.Drawing.Size(365, 106);
			this.mTimestampsLayout.TabIndex = 1;
			// 
			// mCreationTimeLabel
			// 
			this.mCreationTimeLabel.AutoSize = true;
			this.mCreationTimeLabel.Location = new System.Drawing.Point(3, 25);
			this.mCreationTimeLabel.Margin = new System.Windows.Forms.Padding(3);
			this.mCreationTimeLabel.Name = "mCreationTimeLabel";
			this.mCreationTimeLabel.Size = new System.Drawing.Size(78, 13);
			this.mCreationTimeLabel.TabIndex = 2;
			this.mCreationTimeLabel.Text = "<dynamic text>";
			// 
			// mCreationTime
			// 
			this.mCreationTime.AutoSize = true;
			this.mTimestampsLayout.SetColumnSpan(this.mCreationTime, 2);
			this.mCreationTime.Location = new System.Drawing.Point(87, 25);
			this.mCreationTime.Margin = new System.Windows.Forms.Padding(3);
			this.mCreationTime.Name = "mCreationTime";
			this.mCreationTime.Size = new System.Drawing.Size(78, 13);
			this.mCreationTime.TabIndex = 3;
			this.mCreationTime.Text = "<dynamic text>";
			// 
			// mAccessTimeLabel
			// 
			this.mAccessTimeLabel.AutoSize = true;
			this.mAccessTimeLabel.Location = new System.Drawing.Point(3, 44);
			this.mAccessTimeLabel.Margin = new System.Windows.Forms.Padding(3);
			this.mAccessTimeLabel.Name = "mAccessTimeLabel";
			this.mAccessTimeLabel.Size = new System.Drawing.Size(78, 13);
			this.mAccessTimeLabel.TabIndex = 4;
			this.mAccessTimeLabel.Text = "<dynamic text>";
			// 
			// mAccessTime
			// 
			this.mAccessTime.AutoSize = true;
			this.mTimestampsLayout.SetColumnSpan(this.mAccessTime, 2);
			this.mAccessTime.Location = new System.Drawing.Point(87, 44);
			this.mAccessTime.Margin = new System.Windows.Forms.Padding(3);
			this.mAccessTime.Name = "mAccessTime";
			this.mAccessTime.Size = new System.Drawing.Size(78, 13);
			this.mAccessTime.TabIndex = 5;
			this.mAccessTime.Text = "<dynamic text>";
			// 
			// mModificationTimeLabel
			// 
			this.mModificationTimeLabel.AutoSize = true;
			this.mModificationTimeLabel.Location = new System.Drawing.Point(3, 63);
			this.mModificationTimeLabel.Margin = new System.Windows.Forms.Padding(3);
			this.mModificationTimeLabel.Name = "mModificationTimeLabel";
			this.mModificationTimeLabel.Size = new System.Drawing.Size(78, 13);
			this.mModificationTimeLabel.TabIndex = 6;
			this.mModificationTimeLabel.Text = "<dynamic text>";
			// 
			// mModificationTime
			// 
			this.mModificationTime.AutoSize = true;
			this.mTimestampsLayout.SetColumnSpan(this.mModificationTime, 2);
			this.mModificationTime.Location = new System.Drawing.Point(87, 63);
			this.mModificationTime.Margin = new System.Windows.Forms.Padding(3);
			this.mModificationTime.Name = "mModificationTime";
			this.mModificationTime.Size = new System.Drawing.Size(78, 13);
			this.mModificationTime.TabIndex = 7;
			this.mModificationTime.Text = "<dynamic text>";
			// 
			// mSeparator
			// 
			this.mSeparator.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.mTimestampsLayout.SetColumnSpan(this.mSeparator, 3);
			this.mSeparator.Dock = System.Windows.Forms.DockStyle.Top;
			this.mSeparator.Location = new System.Drawing.Point(3, 10);
			this.mSeparator.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
			this.mSeparator.Name = "mSeparator";
			this.mSeparator.Size = new System.Drawing.Size(359, 2);
			this.mSeparator.TabIndex = 1;
			// 
			// m_cbExpires
			// 
			this.m_cbExpires.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.m_cbExpires.AutoSize = true;
			this.m_cbExpires.Location = new System.Drawing.Point(7, 85);
			this.m_cbExpires.Margin = new System.Windows.Forms.Padding(7, 5, 3, 3);
			this.m_cbExpires.Name = "m_cbExpires";
			this.m_cbExpires.Size = new System.Drawing.Size(63, 17);
			this.m_cbExpires.TabIndex = 10;
			this.m_cbExpires.Text = "Expires:";
			this.m_cbExpires.UseVisualStyleBackColor = true;
			this.m_cbExpires.CheckedChanged += new System.EventHandler(this.m_cbExpires_CheckedChanged);
			this.m_cbExpires.Click += new System.EventHandler(this.m_cbExpires_Click);
			this.m_cbExpires.LostFocus += new System.EventHandler(this.m_cbExpires_LostFocus);
			// 
			// m_dtExpireDateTime
			// 
			this.m_dtExpireDateTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.m_dtExpireDateTime.CustomFormat = " ";
			this.m_dtExpireDateTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.m_dtExpireDateTime.Location = new System.Drawing.Point(87, 82);
			this.m_dtExpireDateTime.Name = "m_dtExpireDateTime";
			this.m_dtExpireDateTime.Size = new System.Drawing.Size(237, 20);
			this.m_dtExpireDateTime.TabIndex = 20;
			this.m_dtExpireDateTime.GotFocus += new System.EventHandler(this.m_dtExpireDateTime_GotFocus);
			this.m_dtExpireDateTime.LostFocus += new System.EventHandler(this.m_dtExpireDateTime_LostFocus);
			// 
			// m_btnStandardExpires
			// 
			this.m_btnStandardExpires.ContextMenuStrip = this.m_ctxDefaultTimes;
			this.m_btnStandardExpires.Location = new System.Drawing.Point(330, 80);
			this.m_btnStandardExpires.Margin = new System.Windows.Forms.Padding(3, 1, 3, 3);
			this.m_btnStandardExpires.Name = "m_btnStandardExpires";
			this.m_btnStandardExpires.Size = new System.Drawing.Size(32, 23);
			this.m_btnStandardExpires.TabIndex = 21;
			this.m_btnStandardExpires.UseVisualStyleBackColor = true;
			this.m_btnStandardExpires.Click += new System.EventHandler(this.OnBtnStandardExpiresClick);
			// 
			// m_ctxDefaultTimes
			// 
			this.m_ctxDefaultTimes.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_menuExpireNow,
            this.m_menuExpireSep0,
            this.m_menuExpire1Week,
            this.m_menuExpire2Weeks,
            this.m_menuExpireSep1,
            this.m_menuExpire1Month,
            this.m_menuExpire3Months,
            this.m_menuExpire6Months,
            this.m_menuExpireSep2,
            this.m_menuExpire1Year});
			this.m_ctxDefaultTimes.Name = "m_ctxDefaultTimes";
			this.m_ctxDefaultTimes.Size = new System.Drawing.Size(125, 176);
			// 
			// m_menuExpireNow
			// 
			this.m_menuExpireNow.Name = "m_menuExpireNow";
			this.m_menuExpireNow.Size = new System.Drawing.Size(124, 22);
			this.m_menuExpireNow.Text = "&Now";
			this.m_menuExpireNow.Click += new System.EventHandler(this.OnMenuExpireNow);
			// 
			// m_menuExpireSep0
			// 
			this.m_menuExpireSep0.Name = "m_menuExpireSep0";
			this.m_menuExpireSep0.Size = new System.Drawing.Size(121, 6);
			// 
			// m_menuExpire1Week
			// 
			this.m_menuExpire1Week.Name = "m_menuExpire1Week";
			this.m_menuExpire1Week.Size = new System.Drawing.Size(124, 22);
			this.m_menuExpire1Week.Text = "&1 Week";
			this.m_menuExpire1Week.Click += new System.EventHandler(this.OnMenuExpire1Week);
			// 
			// m_menuExpire2Weeks
			// 
			this.m_menuExpire2Weeks.Name = "m_menuExpire2Weeks";
			this.m_menuExpire2Weeks.Size = new System.Drawing.Size(124, 22);
			this.m_menuExpire2Weeks.Text = "&2 Weeks";
			this.m_menuExpire2Weeks.Click += new System.EventHandler(this.OnMenuExpire2Weeks);
			// 
			// m_menuExpireSep1
			// 
			this.m_menuExpireSep1.Name = "m_menuExpireSep1";
			this.m_menuExpireSep1.Size = new System.Drawing.Size(121, 6);
			// 
			// m_menuExpire1Month
			// 
			this.m_menuExpire1Month.Name = "m_menuExpire1Month";
			this.m_menuExpire1Month.Size = new System.Drawing.Size(124, 22);
			this.m_menuExpire1Month.Text = "1 &Month";
			this.m_menuExpire1Month.Click += new System.EventHandler(this.OnMenuExpire1Month);
			// 
			// m_menuExpire3Months
			// 
			this.m_menuExpire3Months.Name = "m_menuExpire3Months";
			this.m_menuExpire3Months.Size = new System.Drawing.Size(124, 22);
			this.m_menuExpire3Months.Text = "&3 Months";
			this.m_menuExpire3Months.Click += new System.EventHandler(this.OnMenuExpire3Months);
			// 
			// m_menuExpire6Months
			// 
			this.m_menuExpire6Months.Name = "m_menuExpire6Months";
			this.m_menuExpire6Months.Size = new System.Drawing.Size(124, 22);
			this.m_menuExpire6Months.Text = "&6 Months";
			this.m_menuExpire6Months.Click += new System.EventHandler(this.OnMenuExpire6Months);
			// 
			// m_menuExpireSep2
			// 
			this.m_menuExpireSep2.Name = "m_menuExpireSep2";
			this.m_menuExpireSep2.Size = new System.Drawing.Size(121, 6);
			// 
			// m_menuExpire1Year
			// 
			this.m_menuExpire1Year.Name = "m_menuExpire1Year";
			this.m_menuExpire1Year.Size = new System.Drawing.Size(124, 22);
			this.m_menuExpire1Year.Text = "1 &Year";
			this.m_menuExpire1Year.Click += new System.EventHandler(this.OnMenuExpire1Year);
			// 
			// mIconPanel
			// 
			this.mIconPanel.AutoSize = true;
			this.mIconPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.mIconPanel.Controls.Add(this.mGroupButton);
			this.mIconPanel.Controls.Add(this.m_lblIcon);
			this.mIconPanel.Controls.Add(this.m_btnIcon);
			this.mIconPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.mIconPanel.Location = new System.Drawing.Point(0, 0);
			this.mIconPanel.Margin = new System.Windows.Forms.Padding(0);
			this.mIconPanel.Name = "mIconPanel";
			this.mIconPanel.Size = new System.Drawing.Size(365, 34);
			this.mIconPanel.TabIndex = 0;
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
			// m_lblIcon
			// 
			this.m_lblIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_lblIcon.AutoSize = true;
			this.m_lblIcon.Location = new System.Drawing.Point(293, 13);
			this.m_lblIcon.Name = "m_lblIcon";
			this.m_lblIcon.Size = new System.Drawing.Size(31, 13);
			this.m_lblIcon.TabIndex = 1;
			this.m_lblIcon.Text = "Icon:";
			this.m_lblIcon.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// m_btnIcon
			// 
			this.m_btnIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_btnIcon.Location = new System.Drawing.Point(330, 8);
			this.m_btnIcon.Name = "m_btnIcon";
			this.m_btnIcon.Size = new System.Drawing.Size(32, 23);
			this.m_btnIcon.TabIndex = 2;
			this.m_btnIcon.UseVisualStyleBackColor = true;
			this.m_btnIcon.Click += new System.EventHandler(this.m_btnIcon_Click);
			// 
			// mAllTextTab
			// 
			this.mAllTextTab.Location = new System.Drawing.Point(4, 22);
			this.mAllTextTab.Name = "mAllTextTab";
			this.mAllTextTab.Size = new System.Drawing.Size(365, 410);
			this.mAllTextTab.TabIndex = 2;
			this.mAllTextTab.Text = "All Text";
			// 
			// mMultipleSelectionTab
			// 
			this.mMultipleSelectionTab.Controls.Add(this.mMultipleSelectionFields);
			this.mMultipleSelectionTab.Location = new System.Drawing.Point(4, 22);
			this.mMultipleSelectionTab.Name = "mMultipleSelectionTab";
			this.mMultipleSelectionTab.Size = new System.Drawing.Size(365, 410);
			this.mMultipleSelectionTab.TabIndex = 3;
			this.mMultipleSelectionTab.Text = "Multiple Selection";
			// 
			// mDoubleClickTimer
			// 
			this.mDoubleClickTimer.Tick += new System.EventHandler(this.mDoubleClickTimer_Tick);
			// 
			// mFieldGridContextMenu
			// 
			this.mFieldGridContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mURLDropDown,
            this.mCopyCommand,
            this.mAutoTypeCommand,
            this.toolStripSeparator1,
            this.mEditFieldCommand,
            this.mProtectFieldCommand,
            this.mPasswordGeneratorCommand,
            this.toolStripSeparator2,
            this.mDeleteFieldCommand,
            this.mAddNewCommand});
			this.mFieldGridContextMenu.Name = "mFieldGridContextMenu";
			this.mFieldGridContextMenu.Size = new System.Drawing.Size(221, 192);
			// 
			// mURLDropDown
			// 
			this.mURLDropDown.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mOpenURLCommand});
			this.mURLDropDown.Name = "mURLDropDown";
			this.mURLDropDown.Size = new System.Drawing.Size(220, 22);
			this.mURLDropDown.Text = "&Link";
			// 
			// mOpenURLCommand
			// 
			this.mOpenURLCommand.Image = global::KPEnhancedEntryView.Properties.Resources.B16x16_Browser;
			this.mOpenURLCommand.Name = "mOpenURLCommand";
			this.mOpenURLCommand.Size = new System.Drawing.Size(103, 22);
			this.mOpenURLCommand.Text = "&Open";
			this.mOpenURLCommand.Click += new System.EventHandler(this.mOpenURLCommand_Click);
			// 
			// mCopyCommand
			// 
			this.mCopyCommand.Image = global::KPEnhancedEntryView.Properties.Resources.B16x16_KGPG_Info;
			this.mCopyCommand.Name = "mCopyCommand";
			this.mCopyCommand.Size = new System.Drawing.Size(220, 22);
			this.mCopyCommand.Text = "Copy {0}";
			this.mCopyCommand.Click += new System.EventHandler(this.mCopyCommand_Click);
			// 
			// mAutoTypeCommand
			// 
			this.mAutoTypeCommand.Image = global::KPEnhancedEntryView.Properties.Resources.B16x16_KTouch;
			this.mAutoTypeCommand.Name = "mAutoTypeCommand";
			this.mAutoTypeCommand.Size = new System.Drawing.Size(220, 22);
			this.mAutoTypeCommand.Text = "Auto-Type {0}";
			this.mAutoTypeCommand.Click += new System.EventHandler(this.mAutoTypeCommand_Click);
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
			this.mEditFieldCommand.Click += new System.EventHandler(this.mEditFieldCommand_Click);
			// 
			// mProtectFieldCommand
			// 
			this.mProtectFieldCommand.CheckOnClick = true;
			this.mProtectFieldCommand.Name = "mProtectFieldCommand";
			this.mProtectFieldCommand.Size = new System.Drawing.Size(220, 22);
			this.mProtectFieldCommand.Text = "&Protect Field";
			this.mProtectFieldCommand.Click += new System.EventHandler(this.mProtectFieldCommand_Click);
			// 
			// mPasswordGeneratorCommand
			// 
			this.mPasswordGeneratorCommand.Image = global::KPEnhancedEntryView.Properties.Resources.B16x16_Key_New;
			this.mPasswordGeneratorCommand.Name = "mPasswordGeneratorCommand";
			this.mPasswordGeneratorCommand.Size = new System.Drawing.Size(220, 22);
			this.mPasswordGeneratorCommand.Text = "&Open Password Generator...";
			this.mPasswordGeneratorCommand.Click += new System.EventHandler(this.mPasswordGeneratorCommand_Click);
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
			this.mDeleteFieldCommand.Click += new System.EventHandler(this.mDeleteFieldCommand_Click);
			// 
			// mAddNewCommand
			// 
			this.mAddNewCommand.Image = global::KPEnhancedEntryView.Properties.Resources.B16x16_KGPG_Import;
			this.mAddNewCommand.Name = "mAddNewCommand";
			this.mAddNewCommand.Size = new System.Drawing.Size(220, 22);
			this.mAddNewCommand.Text = "&Add New Field";
			this.mAddNewCommand.Click += new System.EventHandler(this.mAddNewCommand_Click);
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
			this.mAttachmentsContextMenu.Size = new System.Drawing.Size(153, 120);
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
			// mMultipleEntriesTabs
			// 
			this.mMultipleEntriesTabs.Controls.Add(this.mMultipleSelectionTab);
			this.mMultipleEntriesTabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mMultipleEntriesTabs.Location = new System.Drawing.Point(0, 0);
			this.mMultipleEntriesTabs.Name = "mMultipleEntriesTabs";
			this.mMultipleEntriesTabs.SelectedIndex = 0;
			this.mMultipleEntriesTabs.Size = new System.Drawing.Size(373, 436);
			this.mMultipleEntriesTabs.TabIndex = 2;
			this.mMultipleEntriesTabs.Visible = false;
			// 
			// mLockButton
			// 
			this.mLockButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.mLockButton.Appearance = System.Windows.Forms.Appearance.Button;
			this.mLockButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.mLockButton.FlatAppearance.BorderSize = 0;
			this.mLockButton.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.Control;
			this.mLockButton.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ActiveCaption;
			this.mLockButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
			this.mLockButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.mLockButton.ImageIndex = 1;
			this.mLockButton.ImageList = this.mLockImages;
			this.mLockButton.Location = new System.Drawing.Point(354, 3);
			this.mLockButton.Name = "mLockButton";
			this.mLockButton.Padding = new System.Windows.Forms.Padding(0, 0, 1, 1);
			this.mLockButton.Size = new System.Drawing.Size(14, 14);
			this.mLockButton.TabIndex = 0;
			this.mLockButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.mLockButton.UseVisualStyleBackColor = false;
			this.mLockButton.CheckedChanged += new System.EventHandler(this.mLockButton_CheckedChanged);
			this.mLockButton.Click += new System.EventHandler(this.mLockButton_Click);
			// 
			// mLockImages
			// 
			this.mLockImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("mLockImages.ImageStream")));
			this.mLockImages.TransparentColor = System.Drawing.Color.Transparent;
			this.mLockImages.Images.SetKeyName(0, "Lock.png");
			this.mLockImages.Images.SetKeyName(1, "Unlock.png");
			// 
			// mSplitGridPanels
			// 
			this.mSplitGridPanels.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mSplitGridPanels.Location = new System.Drawing.Point(0, 0);
			this.mSplitGridPanels.MinimumSplitSize = 50;
			this.mSplitGridPanels.Name = "mSplitGridPanels";
			this.mSplitGridPanels.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// mSplitGridPanels.Panel1
			// 
			this.mSplitGridPanels.Panel1.Controls.Add(this.mFieldsGrid);
			this.mSplitGridPanels.Panel1MinSize = 0;
			// 
			// mSplitGridPanels.Panel2
			// 
			this.mSplitGridPanels.Panel2.Controls.Add(this.mSplitNotesAttachements);
			this.mSplitGridPanels.Panel2MinSize = 0;
			this.mSplitGridPanels.Size = new System.Drawing.Size(365, 410);
			this.mSplitGridPanels.SplitRatio = ((long)(7788461));
			this.mSplitGridPanels.SplitterDistance = 316;
			this.mSplitGridPanels.TabIndex = 2;
			this.mSplitGridPanels.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.mSplitGridPanels_SplitterMoved);
			// 
			// mFieldsGrid
			// 
			this.mFieldsGrid.AllowCreateHistoryNow = true;
			this.mFieldsGrid.AlternateRowBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(255)))));
			this.mFieldsGrid.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
			this.mFieldsGrid.CellEditTabChangesRows = true;
			this.mFieldsGrid.CopySelectionOnControlC = false;
			this.mFieldsGrid.Cursor = System.Windows.Forms.Cursors.Default;
			this.mFieldsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mFieldsGrid.Entry = null;
			this.mFieldsGrid.FullRowSelect = true;
			this.mFieldsGrid.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.mFieldsGrid.Location = new System.Drawing.Point(0, 0);
			this.mFieldsGrid.MultiSelect = false;
			this.mFieldsGrid.Name = "mFieldsGrid";
			this.mFieldsGrid.ShowGroups = false;
			this.mFieldsGrid.ShowItemToolTips = true;
			this.mFieldsGrid.Size = new System.Drawing.Size(365, 316);
			this.mFieldsGrid.TabIndex = 0;
			this.mFieldsGrid.UseAlternatingBackColors = true;
			this.mFieldsGrid.UseCellFormatEvents = true;
			this.mFieldsGrid.UseCompatibleStateImageBehavior = false;
			this.mFieldsGrid.UseHyperlinks = true;
			this.mFieldsGrid.ValidationFailureReporter = this.mValidationFailureReporter;
			this.mFieldsGrid.View = System.Windows.Forms.View.Details;
			this.mFieldsGrid.Modified += new System.EventHandler(this.mFieldsGrid_Modified);
			this.mFieldsGrid.CellRightClick += new System.EventHandler<BrightIdeasSoftware.CellRightClickEventArgs>(this.mFieldsGrid_CellRightClick);
			this.mFieldsGrid.HyperlinkClicked += new System.EventHandler<BrightIdeasSoftware.HyperlinkClickedEventArgs>(this.mFieldsGrid_HyperlinkClicked);
			// 
			// mSplitNotesAttachements
			// 
			this.mSplitNotesAttachements.ButtonSize = 42;
			this.mSplitNotesAttachements.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mSplitNotesAttachements.Location = new System.Drawing.Point(0, 0);
			this.mSplitNotesAttachements.MinimumSplitSize = 50;
			this.mSplitNotesAttachements.Name = "mSplitNotesAttachements";
			// 
			// mSplitNotesAttachements.Panel1
			// 
			this.mSplitNotesAttachements.Panel1.Controls.Add(this.mNotesBorder);
			this.mSplitNotesAttachements.Panel1MinSize = 0;
			// 
			// mSplitNotesAttachements.Panel2
			// 
			this.mSplitNotesAttachements.Panel2.Controls.Add(this.mAttachments);
			this.mSplitNotesAttachements.Panel2MinSize = 0;
			this.mSplitNotesAttachements.Size = new System.Drawing.Size(365, 90);
			this.mSplitNotesAttachements.SplitRatio = ((long)(7036011));
			this.mSplitNotesAttachements.SplitterDistance = 254;
			this.mSplitNotesAttachements.TabIndex = 0;
			this.mSplitNotesAttachements.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.mSplitNotesAttachements_SplitterMoved);
			// 
			// mNotesBorder
			// 
			this.mNotesBorder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.mNotesBorder.Controls.Add(this.mNotes);
			this.mNotesBorder.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mNotesBorder.Location = new System.Drawing.Point(0, 0);
			this.mNotesBorder.Name = "mNotesBorder";
			this.mNotesBorder.Padding = new System.Windows.Forms.Padding(1);
			this.mNotesBorder.Size = new System.Drawing.Size(254, 90);
			this.mNotesBorder.TabIndex = 2;
			// 
			// mNotes
			// 
			this.mNotes.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.mNotes.DetectUrls = false;
			this.mNotes.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mNotes.Location = new System.Drawing.Point(1, 1);
			this.mNotes.Name = "mNotes";
			this.mNotes.Size = new System.Drawing.Size(250, 86);
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
			this.mAttachments.Database = null;
			this.mAttachments.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mAttachments.EmptyListMsg = "Attachments";
			this.mAttachments.EmptyListMsgFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic);
			this.mAttachments.Entry = null;
			this.mAttachments.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.mAttachments.IsReadOnly = false;
			this.mAttachments.Location = new System.Drawing.Point(0, 0);
			this.mAttachments.Name = "mAttachments";
			this.mAttachments.ShowGroups = false;
			this.mAttachments.Size = new System.Drawing.Size(107, 90);
			this.mAttachments.TabIndex = 0;
			this.mAttachments.UseCompatibleStateImageBehavior = false;
			this.mAttachments.ValidationFailureReporter = this.mValidationFailureReporter;
			this.mAttachments.View = System.Windows.Forms.View.SmallIcon;
			this.mAttachments.EntryModified += new System.EventHandler(this.mAttachments_EntryModified);
			this.mAttachments.CellRightClick += new System.EventHandler<BrightIdeasSoftware.CellRightClickEventArgs>(this.mAttachments_CellRightClick);
			// 
			// mMultipleSelectionFields
			// 
			this.mMultipleSelectionFields.AllowCreateHistoryNow = true;
			this.mMultipleSelectionFields.AlternateRowBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(255)))));
			this.mMultipleSelectionFields.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
			this.mMultipleSelectionFields.CellEditTabChangesRows = true;
			this.mMultipleSelectionFields.CopySelectionOnControlC = false;
			this.mMultipleSelectionFields.Cursor = System.Windows.Forms.Cursors.Default;
			this.mMultipleSelectionFields.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mMultipleSelectionFields.Entries = new KeePassLib.PwEntry[0];
			this.mMultipleSelectionFields.FullRowSelect = true;
			this.mMultipleSelectionFields.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.mMultipleSelectionFields.Location = new System.Drawing.Point(0, 0);
			this.mMultipleSelectionFields.MultiSelect = false;
			this.mMultipleSelectionFields.Name = "mMultipleSelectionFields";
			this.mMultipleSelectionFields.ShowGroups = false;
			this.mMultipleSelectionFields.ShowItemToolTips = true;
			this.mMultipleSelectionFields.Size = new System.Drawing.Size(365, 410);
			this.mMultipleSelectionFields.TabIndex = 1;
			this.mMultipleSelectionFields.UseAlternatingBackColors = true;
			this.mMultipleSelectionFields.UseCellFormatEvents = true;
			this.mMultipleSelectionFields.UseCompatibleStateImageBehavior = false;
			this.mMultipleSelectionFields.UseHyperlinks = true;
			this.mMultipleSelectionFields.ValidationFailureReporter = this.mValidationFailureReporter;
			this.mMultipleSelectionFields.View = System.Windows.Forms.View.Details;
			this.mMultipleSelectionFields.Modified += new System.EventHandler(this.mMultipleSelectionFields_Modified);
			this.mMultipleSelectionFields.CellRightClick += new System.EventHandler<BrightIdeasSoftware.CellRightClickEventArgs>(this.mMultipleSelectionFields_CellRightClick);
			// 
			// EntryView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.mLockButton);
			this.Controls.Add(this.mSingleEntryTabs);
			this.Controls.Add(this.mMultipleEntriesTabs);
			this.Name = "EntryView";
			this.Size = new System.Drawing.Size(373, 436);
			this.mSingleEntryTabs.ResumeLayout(false);
			this.mFieldsTab.ResumeLayout(false);
			this.mPropertiesTab.ResumeLayout(false);
			this.mPropertiesTabScrollPanel.ResumeLayout(false);
			this.mPropertiesTabScrollPanel.PerformLayout();
			this.mAutoTypeLayout.ResumeLayout(false);
			this.mAutoTypeLayout.PerformLayout();
			this.mTextPropertiesLayout.ResumeLayout(false);
			this.mTextPropertiesLayout.PerformLayout();
			this.mCustomColoursLayout.ResumeLayout(false);
			this.mCustomColoursLayout.PerformLayout();
			this.mTimestampsLayout.ResumeLayout(false);
			this.mTimestampsLayout.PerformLayout();
			this.m_ctxDefaultTimes.ResumeLayout(false);
			this.mIconPanel.ResumeLayout(false);
			this.mIconPanel.PerformLayout();
			this.mMultipleSelectionTab.ResumeLayout(false);
			this.mFieldGridContextMenu.ResumeLayout(false);
			this.mAttachmentsContextMenu.ResumeLayout(false);
			this.mMultipleEntriesTabs.ResumeLayout(false);
			this.mSplitGridPanels.Panel1.ResumeLayout(false);
			this.mSplitGridPanels.Panel2.ResumeLayout(false);
			this.mSplitGridPanels.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.mFieldsGrid)).EndInit();
			this.mSplitNotesAttachements.Panel1.ResumeLayout(false);
			this.mSplitNotesAttachements.Panel2.ResumeLayout(false);
			this.mSplitNotesAttachements.ResumeLayout(false);
			this.mNotesBorder.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.mAttachments)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.mMultipleSelectionFields)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl mSingleEntryTabs;
		private System.Windows.Forms.TabPage mFieldsTab;
		private System.Windows.Forms.TabPage mPropertiesTab;
		private SingleEntryFieldsListView mFieldsGrid;
		private CollapsibleSplitContainer mSplitGridPanels;
		private CollapsibleSplitContainer mSplitNotesAttachements;
		private KeePass.UI.CustomRichTextBoxEx mNotes;
		private AttachmentsListView mAttachments;
		private System.Windows.Forms.Panel mNotesBorder;
		private ValidationFailureReporter mValidationFailureReporter;
		private System.Windows.Forms.Timer mDoubleClickTimer;
		private System.Windows.Forms.TabPage mAllTextTab;
		private System.Windows.Forms.TableLayoutPanel mTimestampsLayout;
		private System.Windows.Forms.Button mGroupButton;
		private System.Windows.Forms.Label mCreationTimeLabel;
		private System.Windows.Forms.Label mCreationTime;
		private System.Windows.Forms.Label mAccessTimeLabel;
		private System.Windows.Forms.Label mAccessTime;
		private System.Windows.Forms.Label mModificationTimeLabel;
		private System.Windows.Forms.Label mModificationTime;
		private System.Windows.Forms.Label mSeparator;
		private System.Windows.Forms.Label mSeparator2;
		private System.Windows.Forms.Label mTagsLabel;
		private System.Windows.Forms.TextBox mTags;
		private KeePass.UI.ImageComboBoxEx m_cmbOverrideUrl;
		private System.Windows.Forms.Label mOverrideUrlLabel;
		private System.Windows.Forms.Panel mIconPanel;
		private System.Windows.Forms.Label m_lblIcon;
		private System.Windows.Forms.Button m_btnIcon;
		private KeePass.UI.CustomContextMenuStripEx mFieldGridContextMenu;
		private System.Windows.Forms.ToolStripMenuItem mCopyCommand;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem mEditFieldCommand;
		private System.Windows.Forms.ToolStripMenuItem mProtectFieldCommand;
		private System.Windows.Forms.ToolStripMenuItem mPasswordGeneratorCommand;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem mDeleteFieldCommand;
		private System.Windows.Forms.ToolStripMenuItem mAddNewCommand;
		private KeePass.UI.CustomContextMenuStripEx mAttachmentsContextMenu;
		private System.Windows.Forms.ToolStripMenuItem mViewBinaryCommand;
		private System.Windows.Forms.ToolStripMenuItem mSaveBinaryCommand;
		private System.Windows.Forms.ToolStripMenuItem mDeleteBinaryCommand;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem mAttachBinaryCommand;
		private System.Windows.Forms.ToolStripMenuItem mRenameBinaryCommand;
		private System.Windows.Forms.ToolStripMenuItem mURLDropDown;
		private System.Windows.Forms.ToolStripMenuItem mOpenURLCommand;
		private System.Windows.Forms.TabPage mMultipleSelectionTab;
		private MultipleEntriesFieldsListView mMultipleSelectionFields;
		private System.Windows.Forms.Panel mPropertiesTabScrollPanel;
		private System.Windows.Forms.TableLayoutPanel mTextPropertiesLayout;
		private System.Windows.Forms.TextBox mUUID;
		private System.Windows.Forms.Label mUUIDLabel;
		private System.Windows.Forms.TableLayoutPanel mCustomColoursLayout;
		private System.Windows.Forms.Button m_btnPickFgColor;
		private System.Windows.Forms.CheckBox m_cbCustomForegroundColor;
		private System.Windows.Forms.CheckBox m_cbCustomBackgroundColor;
		private System.Windows.Forms.Button m_btnPickBgColor;
		private System.Windows.Forms.TabControl mMultipleEntriesTabs;
		private System.Windows.Forms.ToolStripMenuItem mAutoTypeCommand;
		private System.Windows.Forms.CheckBox m_cbExpires;
		private System.Windows.Forms.DateTimePicker m_dtExpireDateTime;
		private System.Windows.Forms.Button m_btnStandardExpires;
		private KeePass.UI.CustomContextMenuStripEx m_ctxDefaultTimes;
		private System.Windows.Forms.ToolStripMenuItem m_menuExpireNow;
		private System.Windows.Forms.ToolStripSeparator m_menuExpireSep0;
		private System.Windows.Forms.ToolStripMenuItem m_menuExpire1Week;
		private System.Windows.Forms.ToolStripMenuItem m_menuExpire2Weeks;
		private System.Windows.Forms.ToolStripSeparator m_menuExpireSep1;
		private System.Windows.Forms.ToolStripMenuItem m_menuExpire1Month;
		private System.Windows.Forms.ToolStripMenuItem m_menuExpire3Months;
		private System.Windows.Forms.ToolStripMenuItem m_menuExpire6Months;
		private System.Windows.Forms.ToolStripSeparator m_menuExpireSep2;
		private System.Windows.Forms.ToolStripMenuItem m_menuExpire1Year;
		private System.Windows.Forms.TableLayoutPanel mAutoTypeLayout;
		private System.Windows.Forms.Label mSeparator3;
		private System.Windows.Forms.CheckBox m_cbAutoTypeEnabled;
		private System.Windows.Forms.CheckBox m_cbAutoTypeObfuscation;
		private System.Windows.Forms.CheckBox mLockButton;
		private System.Windows.Forms.ImageList mLockImages;
	}
}
