﻿namespace rab1.Forms
{
    partial class FileMaker
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
            this.textBoxDirectory1 = new System.Windows.Forms.TextBox();
            this.textBoxDirectory2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonSelectDirectory1 = new System.Windows.Forms.Button();
            this.buttonSelectDirectory2 = new System.Windows.Forms.Button();
            this.textBoxStartRowNumber = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxEndRowNumber = new System.Windows.Forms.TextBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.textBoxOutputFile = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxDirectory1
            // 
            this.textBoxDirectory1.Location = new System.Drawing.Point(108, 12);
            this.textBoxDirectory1.Name = "textBoxDirectory1";
            this.textBoxDirectory1.Size = new System.Drawing.Size(382, 20);
            this.textBoxDirectory1.TabIndex = 0;
            // 
            // textBoxDirectory2
            // 
            this.textBoxDirectory2.Location = new System.Drawing.Point(108, 47);
            this.textBoxDirectory2.Name = "textBoxDirectory2";
            this.textBoxDirectory2.Size = new System.Drawing.Size(382, 20);
            this.textBoxDirectory2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Directory 1:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(42, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Directory 2:";
            // 
            // buttonSelectDirectory1
            // 
            this.buttonSelectDirectory1.Location = new System.Drawing.Point(505, 12);
            this.buttonSelectDirectory1.Name = "buttonSelectDirectory1";
            this.buttonSelectDirectory1.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectDirectory1.TabIndex = 4;
            this.buttonSelectDirectory1.Text = "Select";
            this.buttonSelectDirectory1.UseVisualStyleBackColor = true;
            // 
            // buttonSelectDirectory2
            // 
            this.buttonSelectDirectory2.Location = new System.Drawing.Point(506, 45);
            this.buttonSelectDirectory2.Name = "buttonSelectDirectory2";
            this.buttonSelectDirectory2.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectDirectory2.TabIndex = 5;
            this.buttonSelectDirectory2.Text = "Select";
            this.buttonSelectDirectory2.UseVisualStyleBackColor = true;
            // 
            // textBoxStartRowNumber
            // 
            this.textBoxStartRowNumber.Location = new System.Drawing.Point(108, 92);
            this.textBoxStartRowNumber.Name = "textBoxStartRowNumber";
            this.textBoxStartRowNumber.Size = new System.Drawing.Size(100, 20);
            this.textBoxStartRowNumber.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Start row number:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 135);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "End row number:";
            // 
            // textBoxEndRowNumber
            // 
            this.textBoxEndRowNumber.Location = new System.Drawing.Point(108, 132);
            this.textBoxEndRowNumber.Name = "textBoxEndRowNumber";
            this.textBoxEndRowNumber.Size = new System.Drawing.Size(100, 20);
            this.textBoxEndRowNumber.TabIndex = 9;
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(404, 218);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(176, 23);
            this.buttonOk.TabIndex = 10;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // textBoxOutputFile
            // 
            this.textBoxOutputFile.Location = new System.Drawing.Point(108, 175);
            this.textBoxOutputFile.Name = "textBoxOutputFile";
            this.textBoxOutputFile.Size = new System.Drawing.Size(382, 20);
            this.textBoxOutputFile.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(45, 179);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Output file:";
            // 
            // FileMaker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 253);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxOutputFile);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.textBoxEndRowNumber);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxStartRowNumber);
            this.Controls.Add(this.buttonSelectDirectory2);
            this.Controls.Add(this.buttonSelectDirectory1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxDirectory2);
            this.Controls.Add(this.textBoxDirectory1);
            this.Name = "FileMaker";
            this.Text = "FileMaker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxDirectory1;
        private System.Windows.Forms.TextBox textBoxDirectory2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonSelectDirectory1;
        private System.Windows.Forms.Button buttonSelectDirectory2;
        private System.Windows.Forms.TextBox textBoxStartRowNumber;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxEndRowNumber;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.TextBox textBoxOutputFile;
        private System.Windows.Forms.Label label5;
    }
}