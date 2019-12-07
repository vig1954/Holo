namespace SimpleApplication
{
    partial class DataPlotForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.ChannelSelector = new System.Windows.Forms.ComboBox();
            this.ReadDataButton = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.SuspendLayout();
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
            this.SourceSelector.FormattingEnabled = true;
            this.SourceSelector.Location = new System.Drawing.Point(12, 25);
            this.SourceSelector.Name = "SourceSelector";
            this.SourceSelector.Size = new System.Drawing.Size(440, 21);
            this.SourceSelector.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(456, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Канал";
            // 
            // ChannelSelector
            // 
            this.ChannelSelector.FormattingEnabled = true;
            this.ChannelSelector.Location = new System.Drawing.Point(459, 25);
            this.ChannelSelector.Name = "ChannelSelector";
            this.ChannelSelector.Size = new System.Drawing.Size(121, 21);
            this.ChannelSelector.TabIndex = 3;
            // 
            // ReadDataButton
            // 
            this.ReadDataButton.Location = new System.Drawing.Point(586, 9);
            this.ReadDataButton.Name = "ReadDataButton";
            this.ReadDataButton.Size = new System.Drawing.Size(98, 37);
            this.ReadDataButton.TabIndex = 4;
            this.ReadDataButton.Text = "Загрузить";
            this.ReadDataButton.UseVisualStyleBackColor = true;
            this.ReadDataButton.Click += new System.EventHandler(this.ReadDataButton_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(3, 52);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Size = new System.Drawing.Size(796, 397);
            this.splitContainer1.SplitterDistance = 265;
            this.splitContainer1.TabIndex = 5;
            // 
            // DataPlotForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.ReadDataButton);
            this.Controls.Add(this.ChannelSelector);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SourceSelector);
            this.Controls.Add(this.label1);
            this.Name = "DataPlotForm";
            this.Text = "Построение графика по данным";
            this.Load += new System.EventHandler(this.DataPlotForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox SourceSelector;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ChannelSelector;
        private System.Windows.Forms.Button ReadDataButton;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}