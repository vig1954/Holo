namespace rab1
{
    partial class CurvesGraph
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CurvesGraph));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelFrom = new System.Windows.Forms.Label();
            this.labelTo = new System.Windows.Forms.Label();
            this.labelArrow = new System.Windows.Forms.Label();
            this.labelHint1 = new System.Windows.Forms.Label();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApplyAll = new System.Windows.Forms.Button();
            this.txtStartImageNumber = new System.Windows.Forms.TextBox();
            this.txtEndImageNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);
            this.pictureBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDoubleClick);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseLeave += new System.EventHandler(this.pictureBox1_MouseLeave);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // labelFrom
            // 
            resources.ApplyResources(this.labelFrom, "labelFrom");
            this.labelFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelFrom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labelFrom.Name = "labelFrom";
            // 
            // labelTo
            // 
            resources.ApplyResources(this.labelTo, "labelTo");
            this.labelTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelTo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labelTo.Name = "labelTo";
            // 
            // labelArrow
            // 
            resources.ApplyResources(this.labelArrow, "labelArrow");
            this.labelArrow.Name = "labelArrow";
            // 
            // labelHint1
            // 
            resources.ApplyResources(this.labelHint1, "labelHint1");
            this.labelHint1.Name = "labelHint1";
            // 
            // btnApply
            // 
            resources.ApplyResources(this.btnApply, "btnApply");
            this.btnApply.Name = "btnApply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnApplyAll
            // 
            resources.ApplyResources(this.btnApplyAll, "btnApplyAll");
            this.btnApplyAll.Name = "btnApplyAll";
            this.btnApplyAll.UseVisualStyleBackColor = true;
            this.btnApplyAll.Click += new System.EventHandler(this.btnApplyAll_Click);
            // 
            // txtStartImageNumber
            // 
            resources.ApplyResources(this.txtStartImageNumber, "txtStartImageNumber");
            this.txtStartImageNumber.Name = "txtStartImageNumber";
            // 
            // txtEndImageNumber
            // 
            resources.ApplyResources(this.txtEndImageNumber, "txtEndImageNumber");
            this.txtEndImageNumber.Name = "txtEndImageNumber";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // CurvesGraph
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtEndImageNumber);
            this.Controls.Add(this.txtStartImageNumber);
            this.Controls.Add(this.btnApplyAll);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.labelHint1);
            this.Controls.Add(this.labelArrow);
            this.Controls.Add(this.labelTo);
            this.Controls.Add(this.labelFrom);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CurvesGraph";
            this.Opacity = 0.9D;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CurvesGraph_FormClosing);
            this.Load += new System.EventHandler(this.CurvesGraph_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelFrom;
        private System.Windows.Forms.Label labelTo;
        private System.Windows.Forms.Label labelArrow;
        private System.Windows.Forms.Label labelHint1;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApplyAll;
        private System.Windows.Forms.TextBox txtStartImageNumber;
        private System.Windows.Forms.TextBox txtEndImageNumber;
        private System.Windows.Forms.Label label1;
    }
}