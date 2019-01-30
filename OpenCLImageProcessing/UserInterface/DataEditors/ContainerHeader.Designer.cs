namespace UserInterface.DataEditors
{
    partial class ContainerHeader
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
            this.HeaderLabel = new System.Windows.Forms.Label();
            this.btnSplitBottom = new System.Windows.Forms.Button();
            this.btnSplitRight = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnNewWindow = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // HeaderLabel
            // 
            this.HeaderLabel.AutoSize = true;
            this.HeaderLabel.Location = new System.Drawing.Point(3, 3);
            this.HeaderLabel.Name = "HeaderLabel";
            this.HeaderLabel.Size = new System.Drawing.Size(51, 13);
            this.HeaderLabel.TabIndex = 0;
            this.HeaderLabel.Text = "Header 1";
            this.HeaderLabel.Click += new System.EventHandler(this.HeaderLabel_Click);
            // 
            // btnSplitBottom
            // 
            this.btnSplitBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSplitBottom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSplitBottom.Image = global::UserInterface.Properties.Resources.layout_split_vertical;
            this.btnSplitBottom.Location = new System.Drawing.Point(408, 0);
            this.btnSplitBottom.Name = "btnSplitBottom";
            this.btnSplitBottom.Size = new System.Drawing.Size(24, 24);
            this.btnSplitBottom.TabIndex = 3;
            this.btnSplitBottom.UseVisualStyleBackColor = true;
            this.btnSplitBottom.Click += new System.EventHandler(this.btnSplitBottom_Click);
            // 
            // btnSplitRight
            // 
            this.btnSplitRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSplitRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSplitRight.Image = global::UserInterface.Properties.Resources.layout_split;
            this.btnSplitRight.Location = new System.Drawing.Point(433, 0);
            this.btnSplitRight.Name = "btnSplitRight";
            this.btnSplitRight.Size = new System.Drawing.Size(24, 24);
            this.btnSplitRight.TabIndex = 2;
            this.btnSplitRight.UseVisualStyleBackColor = true;
            this.btnSplitRight.Click += new System.EventHandler(this.btnSplitRight_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Image = global::UserInterface.Properties.Resources.cross;
            this.btnClose.Location = new System.Drawing.Point(458, 0);
            this.btnClose.Margin = new System.Windows.Forms.Padding(0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(24, 24);
            this.btnClose.TabIndex = 1;
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnNewWindow
            // 
            this.btnNewWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNewWindow.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnNewWindow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewWindow.Image = global::UserInterface.Properties.Resources.application__plus;
            this.btnNewWindow.Location = new System.Drawing.Point(383, 0);
            this.btnNewWindow.Name = "btnNewWindow";
            this.btnNewWindow.Size = new System.Drawing.Size(24, 24);
            this.btnNewWindow.TabIndex = 4;
            this.btnNewWindow.UseVisualStyleBackColor = false;
            this.btnNewWindow.Visible = false;
            this.btnNewWindow.Click += new System.EventHandler(this.btnNewWindow_Click);
            // 
            // ContainerHeader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CausesValidation = false;
            this.Controls.Add(this.btnNewWindow);
            this.Controls.Add(this.btnSplitBottom);
            this.Controls.Add(this.btnSplitRight);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.HeaderLabel);
            this.Name = "ContainerHeader";
            this.Size = new System.Drawing.Size(482, 24);
            this.Load += new System.EventHandler(this.ContainerHeader_Load);
            this.Click += new System.EventHandler(this.ContainerHeader_Click);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label HeaderLabel;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSplitRight;
        private System.Windows.Forms.Button btnSplitBottom;
        private System.Windows.Forms.Button btnNewWindow;
    }
}
