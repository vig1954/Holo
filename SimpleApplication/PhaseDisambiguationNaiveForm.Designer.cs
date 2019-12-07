namespace SimpleApplication
{
    partial class PhaseDisambiguationNaiveForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.SourceSelector = new System.Windows.Forms.ComboBox();
            this.ProcessButton = new System.Windows.Forms.Button();
            this.ResultPicture = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.ResultPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Источник";
            // 
            // SourceSelector
            // 
            this.SourceSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SourceSelector.FormattingEnabled = true;
            this.SourceSelector.Location = new System.Drawing.Point(15, 25);
            this.SourceSelector.Name = "SourceSelector";
            this.SourceSelector.Size = new System.Drawing.Size(133, 21);
            this.SourceSelector.TabIndex = 1;
            // 
            // ProcessButton
            // 
            this.ProcessButton.Location = new System.Drawing.Point(154, 9);
            this.ProcessButton.Name = "ProcessButton";
            this.ProcessButton.Size = new System.Drawing.Size(105, 37);
            this.ProcessButton.TabIndex = 2;
            this.ProcessButton.Text = "Пуск";
            this.ProcessButton.UseVisualStyleBackColor = true;
            // 
            // ResultPicture
            // 
            this.ResultPicture.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ResultPicture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ResultPicture.Location = new System.Drawing.Point(15, 52);
            this.ResultPicture.Name = "ResultPicture";
            this.ResultPicture.Size = new System.Drawing.Size(773, 386);
            this.ResultPicture.TabIndex = 3;
            this.ResultPicture.TabStop = false;
            // 
            // PhaseDisambiguationNaiveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ResultPicture);
            this.Controls.Add(this.ProcessButton);
            this.Controls.Add(this.SourceSelector);
            this.Controls.Add(this.label1);
            this.Name = "PhaseDisambiguationNaiveForm";
            this.Text = "Устранение фазовой неоднозначности";
            this.Load += new System.EventHandler(this.PhaseDisambiguationNaiveForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ResultPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox SourceSelector;
        private System.Windows.Forms.Button ProcessButton;
        private System.Windows.Forms.PictureBox ResultPicture;
    }
}