namespace UserInterface.WorkspacePanel.ImageSeries
{
    partial class InputImage
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
            this.OpenInEditorButton = new System.Windows.Forms.Button();
            this.Preview = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Preview)).BeginInit();
            this.SuspendLayout();
            // 
            // OpenInEditorButton
            // 
            this.OpenInEditorButton.Image = global::UserInterface.Properties.Resources.arrow_180;
            this.OpenInEditorButton.Location = new System.Drawing.Point(3, 41);
            this.OpenInEditorButton.Name = "OpenInEditorButton";
            this.OpenInEditorButton.Size = new System.Drawing.Size(32, 23);
            this.OpenInEditorButton.TabIndex = 1;
            this.OpenInEditorButton.UseVisualStyleBackColor = true;
            this.OpenInEditorButton.Click += new System.EventHandler(this.OpenInEditorButton_Click);
            // 
            // Preview
            // 
            this.Preview.Location = new System.Drawing.Point(3, 3);
            this.Preview.Name = "Preview";
            this.Preview.Size = new System.Drawing.Size(32, 32);
            this.Preview.TabIndex = 0;
            this.Preview.TabStop = false;
            // 
            // InputImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.OpenInEditorButton);
            this.Controls.Add(this.Preview);
            this.Name = "InputImage";
            this.Size = new System.Drawing.Size(39, 70);
            ((System.ComponentModel.ISupportInitialize)(this.Preview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Preview;
        private System.Windows.Forms.Button OpenInEditorButton;
    }
}
