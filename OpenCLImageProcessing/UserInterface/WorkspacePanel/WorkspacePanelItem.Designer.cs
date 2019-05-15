namespace UserInterface.WorkspacePanel
{
    partial class WorkspacePanelItem
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
            this.components = new System.ComponentModel.Container();
            this.TitleLabel = new System.Windows.Forms.Label();
            this.ChangeNameTextBox = new System.Windows.Forms.TextBox();
            this.IconToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.IconPictureBox = new System.Windows.Forms.PictureBox();
            this.ShowInEditor = new System.Windows.Forms.Button();
            this.OpenSettings = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.IconPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // TitleLabel
            // 
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TitleLabel.Location = new System.Drawing.Point(3, 0);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(47, 13);
            this.TitleLabel.TabIndex = 1;
            this.TitleLabel.Text = "[Name]";
            this.TitleLabel.DoubleClick += new System.EventHandler(this.TitleLabel_DoubleClick);
            // 
            // ChangeNameTextBox
            // 
            this.ChangeNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChangeNameTextBox.Location = new System.Drawing.Point(3, 0);
            this.ChangeNameTextBox.Name = "ChangeNameTextBox";
            this.ChangeNameTextBox.Size = new System.Drawing.Size(502, 20);
            this.ChangeNameTextBox.TabIndex = 3;
            this.ChangeNameTextBox.Visible = false;
            this.ChangeNameTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ChangeNameTextBox_KeyDown);
            // 
            // IconPictureBox
            // 
            this.IconPictureBox.Location = new System.Drawing.Point(3, 16);
            this.IconPictureBox.Name = "IconPictureBox";
            this.IconPictureBox.Size = new System.Drawing.Size(32, 32);
            this.IconPictureBox.TabIndex = 0;
            this.IconPictureBox.TabStop = false;
            this.IconToolTip.SetToolTip(this.IconPictureBox, "Тест");
            this.IconPictureBox.Click += new System.EventHandler(this.IconPictureBox_Click);
            this.IconPictureBox.DoubleClick += new System.EventHandler(this.IconPictureBox_DoubleClick);
            // 
            // ShowInEditor
            // 
            this.ShowInEditor.Image = global::UserInterface.Properties.Resources.arrow_180;
            this.ShowInEditor.Location = new System.Drawing.Point(41, 25);
            this.ShowInEditor.Name = "ShowInEditor";
            this.ShowInEditor.Size = new System.Drawing.Size(83, 23);
            this.ShowInEditor.TabIndex = 4;
            this.ShowInEditor.Text = "Просмотр";
            this.ShowInEditor.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ShowInEditor.UseVisualStyleBackColor = true;
            this.ShowInEditor.Click += new System.EventHandler(this.ShowInEditor_Click);
            // 
            // OpenSettings
            // 
            this.OpenSettings.Image = global::UserInterface.Properties.Resources.gear;
            this.OpenSettings.Location = new System.Drawing.Point(130, 25);
            this.OpenSettings.Name = "OpenSettings";
            this.OpenSettings.Size = new System.Drawing.Size(23, 23);
            this.OpenSettings.TabIndex = 5;
            this.OpenSettings.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.OpenSettings.UseVisualStyleBackColor = true;
            this.OpenSettings.Visible = false;
            this.OpenSettings.Click += new System.EventHandler(this.OpenSettings_Click);
            // 
            // WorkspacePanelItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.OpenSettings);
            this.Controls.Add(this.ShowInEditor);
            this.Controls.Add(this.ChangeNameTextBox);
            this.Controls.Add(this.TitleLabel);
            this.Controls.Add(this.IconPictureBox);
            this.Name = "WorkspacePanelItem";
            this.Size = new System.Drawing.Size(508, 54);
            ((System.ComponentModel.ISupportInitialize)(this.IconPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox IconPictureBox;
        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.TextBox ChangeNameTextBox;
        private System.Windows.Forms.ToolTip IconToolTip;
        private System.Windows.Forms.Button ShowInEditor;
        private System.Windows.Forms.Button OpenSettings;
    }
}
