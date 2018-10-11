namespace UserInterface.DataEditors.InterfaceBinding.Controls
{
    partial class ImageHandlerControl
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
            this.picAddImage = new System.Windows.Forms.PictureBox();
            this.lblImageTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picAddImage)).BeginInit();
            this.SuspendLayout();
            // 
            // picAddImage
            // 
            this.picAddImage.BackgroundImage = global::UserInterface.Properties.Resources.icons8_add_new_16__1_;
            this.picAddImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picAddImage.Location = new System.Drawing.Point(1, 1);
            this.picAddImage.Name = "picAddImage";
            this.picAddImage.Size = new System.Drawing.Size(18, 18);
            this.picAddImage.TabIndex = 0;
            this.picAddImage.TabStop = false;
            this.picAddImage.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picAddImage_MouseClick);
            // 
            // lblImageTitle
            // 
            this.lblImageTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblImageTitle.Location = new System.Drawing.Point(22, 3);
            this.lblImageTitle.Name = "lblImageTitle";
            this.lblImageTitle.Size = new System.Drawing.Size(125, 16);
            this.lblImageTitle.TabIndex = 1;
            this.lblImageTitle.Text = "Image";
            // 
            // ImageHandlerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblImageTitle);
            this.Controls.Add(this.picAddImage);
            this.Name = "ImageHandlerControl";
            this.Size = new System.Drawing.Size(150, 20);
            ((System.ComponentModel.ISupportInitialize)(this.picAddImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picAddImage;
        private System.Windows.Forms.Label lblImageTitle;
    }
}
