namespace ManualTesting
{
    partial class frmValueBindingTesting
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

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.testHostPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // testHostPanel
            // 
            this.testHostPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.testHostPanel.Location = new System.Drawing.Point(0, 0);
            this.testHostPanel.Name = "testHostPanel";
            this.testHostPanel.Size = new System.Drawing.Size(342, 450);
            this.testHostPanel.TabIndex = 0;
            // 
            // frmValueBindingTesting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 450);
            this.Controls.Add(this.testHostPanel);
            this.Name = "frmValueBindingTesting";
            this.Text = "Value Binding Testing";
            this.Load += new System.EventHandler(this.frmValueBindingTesting_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel testHostPanel;
    }
}

