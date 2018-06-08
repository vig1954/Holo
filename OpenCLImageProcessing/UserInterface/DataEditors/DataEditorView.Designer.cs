﻿namespace UserInterface.DataEditors
{
    partial class DataEditorView
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.rightPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.containerHeader1 = new UserInterface.DataEditors.ContainerHeader();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 20);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.rightPanel);
            this.splitContainer1.Size = new System.Drawing.Size(751, 474);
            this.splitContainer1.SplitterDistance = 514;
            this.splitContainer1.TabIndex = 1;
            // 
            // rightPanel
            // 
            this.rightPanel.AutoScroll = true;
            this.rightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightPanel.Location = new System.Drawing.Point(0, 0);
            this.rightPanel.Name = "rightPanel";
            this.rightPanel.Size = new System.Drawing.Size(233, 474);
            this.rightPanel.TabIndex = 0;
            this.rightPanel.Resize += new System.EventHandler(this.rightPanel_Resize);
            // 
            // containerHeader1
            // 
            this.containerHeader1.Active = true;
            this.containerHeader1.BackColor = System.Drawing.Color.SteelBlue;
            this.containerHeader1.Dock = System.Windows.Forms.DockStyle.Top;
            this.containerHeader1.Location = new System.Drawing.Point(0, 0);
            this.containerHeader1.Name = "containerHeader1";
            this.containerHeader1.Size = new System.Drawing.Size(751, 20);
            this.containerHeader1.TabIndex = 0;
            // 
            // DataEditorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.containerHeader1);
            this.Name = "DataEditorView";
            this.Size = new System.Drawing.Size(751, 494);
            this.Load += new System.EventHandler(this.DataEditorView_Load);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ContainerHeader containerHeader1;
        private System.Windows.Forms.SplitContainer splitContainer1;

        private void InitializeComponentCustom()
        {
            this.containerHeader1 = new UserInterface.DataEditors.ContainerHeader();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // containerHeader1
            // 
            this.containerHeader1.Active = true;
            this.containerHeader1.BackColor = System.Drawing.Color.DarkGreen;
            this.containerHeader1.Dock = System.Windows.Forms.DockStyle.Top;
            this.containerHeader1.Location = new System.Drawing.Point(0, 0);
            this.containerHeader1.Name = "containerHeader1";
            this.containerHeader1.Size = new System.Drawing.Size(751, 20);
            this.containerHeader1.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 15);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Size = new System.Drawing.Size(751, 479);
            this.splitContainer1.SplitterDistance = 562;
            this.splitContainer1.TabIndex = 1;
            // 
            // DataEditorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.containerHeader1);
            this.Name = "DataEditorView";
            this.Size = new System.Drawing.Size(751, 494);
            this.Load += new System.EventHandler(this.DataEditorView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.FlowLayoutPanel rightPanel;
    }
}
