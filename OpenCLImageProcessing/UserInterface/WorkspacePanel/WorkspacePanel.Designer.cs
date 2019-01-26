namespace UserInterface.WorkspacePanel
{
    partial class WorkspacePanel
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.AddImageButton = new System.Windows.Forms.ToolStripButton();
            this.AddFolderButton = new System.Windows.Forms.ToolStripButton();
            this.MainLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Right;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddImageButton,
            this.AddFolderButton});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            this.toolStrip1.Location = new System.Drawing.Point(155, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(37, 411);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // AddImageButton
            // 
            this.AddImageButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddImageButton.Image = global::UserInterface.Properties.Resources.icons8_add_image_40;
            this.AddImageButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddImageButton.Name = "AddImageButton";
            this.AddImageButton.Size = new System.Drawing.Size(36, 36);
            this.AddImageButton.Text = "toolStripButton1";
            this.AddImageButton.Click += new System.EventHandler(this.AddImageButton_Click);
            // 
            // AddFolderButton
            // 
            this.AddFolderButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddFolderButton.Image = global::UserInterface.Properties.Resources.icons8_add_folder_40;
            this.AddFolderButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddFolderButton.Name = "AddFolderButton";
            this.AddFolderButton.Size = new System.Drawing.Size(36, 36);
            this.AddFolderButton.Text = "Add Folder";
            this.AddFolderButton.Click += new System.EventHandler(this.AddFolderButton_Click);
            // 
            // MainLayoutPanel
            // 
            this.MainLayoutPanel.AutoScroll = true;
            this.MainLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.MainLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.MainLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.MainLayoutPanel.Name = "MainLayoutPanel";
            this.MainLayoutPanel.Size = new System.Drawing.Size(155, 411);
            this.MainLayoutPanel.TabIndex = 1;
            this.MainLayoutPanel.Resize += new System.EventHandler(this.MainLayoutPanel_Resize);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // WorkspacePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MainLayoutPanel);
            this.Controls.Add(this.toolStrip1);
            this.Name = "WorkspacePanel";
            this.Size = new System.Drawing.Size(192, 411);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton AddImageButton;
        private System.Windows.Forms.FlowLayoutPanel MainLayoutPanel;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripButton AddFolderButton;
    }
}
