namespace WinFormsContentLoading
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openPhysXDescriptionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.objectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.collisionFaceNormalsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cullModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miCullBack = new System.Windows.Forms.ToolStripMenuItem();
            this.miCullFront = new System.Windows.Forms.ToolStripMenuItem();
            this.miCullBoth = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quickHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.btnPreviewPhysX = new System.Windows.Forms.Button();
            this.chkFlipNormals = new System.Windows.Forms.CheckBox();
            this.btnSavePhysXMesh = new System.Windows.Forms.Button();
            this.modelViewerControl = new WinFormsContentLoading.ModelViewerControl();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.openPhysXDescriptionToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.openToolStripMenuItem.Text = "Open Model...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenMenuClicked);
            // 
            // openPhysXDescriptionToolStripMenuItem
            // 
            this.openPhysXDescriptionToolStripMenuItem.Name = "openPhysXDescriptionToolStripMenuItem";
            this.openPhysXDescriptionToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.openPhysXDescriptionToolStripMenuItem.Text = "Open PhysX Description...";
            this.openPhysXDescriptionToolStripMenuItem.Click += new System.EventHandler(this.OnOpenPhysXDescription);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitMenuClicked);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.objectsToolStripMenuItem,
            this.cullModelToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // objectsToolStripMenuItem
            // 
            this.objectsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.collisionFaceNormalsToolStripMenuItem});
            this.objectsToolStripMenuItem.Name = "objectsToolStripMenuItem";
            this.objectsToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.objectsToolStripMenuItem.Text = "PhysX";
            // 
            // collisionFaceNormalsToolStripMenuItem
            // 
            this.collisionFaceNormalsToolStripMenuItem.Checked = true;
            this.collisionFaceNormalsToolStripMenuItem.CheckOnClick = true;
            this.collisionFaceNormalsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.collisionFaceNormalsToolStripMenuItem.Name = "collisionFaceNormalsToolStripMenuItem";
            this.collisionFaceNormalsToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.collisionFaceNormalsToolStripMenuItem.Text = "Collision Face Normals";
            this.collisionFaceNormalsToolStripMenuItem.CheckedChanged += new System.EventHandler(this.OnCheckedChange);
            // 
            // cullModelToolStripMenuItem
            // 
            this.cullModelToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miCullBack,
            this.miCullFront,
            this.miCullBoth});
            this.cullModelToolStripMenuItem.Name = "cullModelToolStripMenuItem";
            this.cullModelToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.cullModelToolStripMenuItem.Text = "Cull Mode";
            // 
            // miCullBack
            // 
            this.miCullBack.CheckOnClick = true;
            this.miCullBack.Name = "miCullBack";
            this.miCullBack.Size = new System.Drawing.Size(102, 22);
            this.miCullBack.Text = "Back";
            this.miCullBack.CheckedChanged += new System.EventHandler(this.OnCullBack);
            // 
            // miCullFront
            // 
            this.miCullFront.CheckOnClick = true;
            this.miCullFront.Name = "miCullFront";
            this.miCullFront.Size = new System.Drawing.Size(102, 22);
            this.miCullFront.Text = "Front";
            this.miCullFront.CheckedChanged += new System.EventHandler(this.OnCullFront);
            // 
            // miCullBoth
            // 
            this.miCullBoth.Checked = true;
            this.miCullBoth.CheckOnClick = true;
            this.miCullBoth.CheckState = System.Windows.Forms.CheckState.Checked;
            this.miCullBoth.Name = "miCullBoth";
            this.miCullBoth.Size = new System.Drawing.Size(102, 22);
            this.miCullBoth.Text = "Both";
            this.miCullBoth.CheckedChanged += new System.EventHandler(this.OnCullBoth);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quickHelpToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // quickHelpToolStripMenuItem
            // 
            this.quickHelpToolStripMenuItem.Name = "quickHelpToolStripMenuItem";
            this.quickHelpToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.quickHelpToolStripMenuItem.Text = "Quick Help";
            this.quickHelpToolStripMenuItem.Click += new System.EventHandler(this.OnClick);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.aboutToolStripMenuItem.Text = "About..";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.OnAboutClick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(792, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.splitter1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(594, 24);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(198, 549);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // btnPreviewPhysX
            // 
            this.btnPreviewPhysX.Location = new System.Drawing.Point(644, 192);
            this.btnPreviewPhysX.Name = "btnPreviewPhysX";
            this.btnPreviewPhysX.Size = new System.Drawing.Size(99, 34);
            this.btnPreviewPhysX.TabIndex = 3;
            this.btnPreviewPhysX.Text = "Preview PhysX Mesh";
            this.btnPreviewPhysX.UseVisualStyleBackColor = true;
            this.btnPreviewPhysX.Click += new System.EventHandler(this.OnPreviewPhysX);
            // 
            // chkFlipNormals
            // 
            this.chkFlipNormals.AutoSize = true;
            this.chkFlipNormals.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.chkFlipNormals.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.chkFlipNormals.Location = new System.Drawing.Point(644, 44);
            this.chkFlipNormals.Name = "chkFlipNormals";
            this.chkFlipNormals.Size = new System.Drawing.Size(83, 17);
            this.chkFlipNormals.TabIndex = 4;
            this.chkFlipNormals.Text = "Flip Normals";
            this.chkFlipNormals.UseVisualStyleBackColor = false;
            // 
            // btnSavePhysXMesh
            // 
            this.btnSavePhysXMesh.Location = new System.Drawing.Point(644, 119);
            this.btnSavePhysXMesh.Name = "btnSavePhysXMesh";
            this.btnSavePhysXMesh.Size = new System.Drawing.Size(99, 36);
            this.btnSavePhysXMesh.TabIndex = 5;
            this.btnSavePhysXMesh.Text = "Save PhysX Mesh";
            this.btnSavePhysXMesh.UseVisualStyleBackColor = true;
            this.btnSavePhysXMesh.Click += new System.EventHandler(this.OnSavePhysXMesh);
            // 
            // modelViewerControl
            // 
            this.modelViewerControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelViewerControl.Location = new System.Drawing.Point(0, 24);
            this.modelViewerControl.Model = null;
            this.modelViewerControl.Name = "modelViewerControl";
            this.modelViewerControl.Size = new System.Drawing.Size(792, 549);
            this.modelViewerControl.TabIndex = 1;
            this.modelViewerControl.Text = "modelViewerControl";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(792, 573);
            this.Controls.Add(this.btnSavePhysXMesh);
            this.Controls.Add(this.chkFlipNormals);
            this.Controls.Add(this.btnPreviewPhysX);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.modelViewerControl);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "XNA Model to PhysX";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem objectsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem collisionFaceNormalsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quickHelpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private ModelViewerControl modelViewerControl;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Button btnPreviewPhysX;
        private System.Windows.Forms.CheckBox chkFlipNormals;
        private System.Windows.Forms.Button btnSavePhysXMesh;
        private System.Windows.Forms.ToolStripMenuItem openPhysXDescriptionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cullModelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miCullBack;
        private System.Windows.Forms.ToolStripMenuItem miCullFront;
        private System.Windows.Forms.ToolStripMenuItem miCullBoth;



    }
}

