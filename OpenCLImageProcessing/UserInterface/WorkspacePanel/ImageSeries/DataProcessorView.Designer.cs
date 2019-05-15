namespace UserInterface.WorkspacePanel.ImageSeries
{
    partial class DataProcessorView
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
            this.DataProcessorName = new System.Windows.Forms.Label();
            this.OpenSettings = new System.Windows.Forms.Button();
            this.OpenInEditor = new System.Windows.Forms.Button();
            this.OpenMenu = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // DataProcessorName
            // 
            this.DataProcessorName.AutoSize = true;
            this.DataProcessorName.Location = new System.Drawing.Point(3, 0);
            this.DataProcessorName.Name = "DataProcessorName";
            this.DataProcessorName.Size = new System.Drawing.Size(83, 13);
            this.DataProcessorName.TabIndex = 0;
            this.DataProcessorName.Text = "[DataProcessor]";
            // 
            // OpenSettings
            // 
            this.OpenSettings.Image = global::UserInterface.Properties.Resources.gear;
            this.OpenSettings.Location = new System.Drawing.Point(89, 16);
            this.OpenSettings.Name = "OpenSettings";
            this.OpenSettings.Size = new System.Drawing.Size(89, 23);
            this.OpenSettings.TabIndex = 2;
            this.OpenSettings.Text = "Настройки";
            this.OpenSettings.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.OpenSettings.UseVisualStyleBackColor = true;
            this.OpenSettings.Click += new System.EventHandler(this.OpenSettings_Click);
            // 
            // OpenInEditor
            // 
            this.OpenInEditor.Enabled = false;
            this.OpenInEditor.Image = global::UserInterface.Properties.Resources.arrow_180;
            this.OpenInEditor.Location = new System.Drawing.Point(3, 16);
            this.OpenInEditor.Name = "OpenInEditor";
            this.OpenInEditor.Size = new System.Drawing.Size(83, 23);
            this.OpenInEditor.TabIndex = 1;
            this.OpenInEditor.Text = "Просмотр";
            this.OpenInEditor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.OpenInEditor.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.OpenInEditor.UseVisualStyleBackColor = true;
            this.OpenInEditor.Click += new System.EventHandler(this.OpenInEditor_Click);
            // 
            // OpenMenu
            // 
            this.OpenMenu.Image = global::UserInterface.Properties.Resources.chevron_5834;
            this.OpenMenu.Location = new System.Drawing.Point(178, 16);
            this.OpenMenu.Name = "OpenMenu";
            this.OpenMenu.Size = new System.Drawing.Size(26, 23);
            this.OpenMenu.TabIndex = 3;
            this.OpenMenu.UseVisualStyleBackColor = true;
            this.OpenMenu.Visible = false;
            this.OpenMenu.Click += new System.EventHandler(this.OpenMenu_Click);
            // 
            // DataProcessorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.OpenMenu);
            this.Controls.Add(this.OpenSettings);
            this.Controls.Add(this.OpenInEditor);
            this.Controls.Add(this.DataProcessorName);
            this.Name = "DataProcessorView";
            this.Size = new System.Drawing.Size(204, 43);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label DataProcessorName;
        private System.Windows.Forms.Button OpenInEditor;
        private System.Windows.Forms.Button OpenSettings;
        private System.Windows.Forms.Button OpenMenu;
    }
}
