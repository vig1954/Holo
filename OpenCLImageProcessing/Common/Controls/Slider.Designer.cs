﻿namespace Common.Controls
{
    partial class Slider
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
            this.SuspendLayout();
            // 
            // Slider
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.DoubleBuffered = true;
            this.Name = "Slider";
            this.Size = new System.Drawing.Size(433, 41);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Slider_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Slider_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Slider_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Slider_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
