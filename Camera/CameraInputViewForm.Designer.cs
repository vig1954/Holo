namespace Camera
{
    partial class CameraInputViewForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CameraInputViewForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ControlPanel = new System.Windows.Forms.Panel();
            this.PreviewPanel = new System.Windows.Forms.Panel();
            this.Preview = new System.Windows.Forms.PictureBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.DockLeftButton = new System.Windows.Forms.ToolStripButton();
            this.DockTopButton = new System.Windows.Forms.ToolStripButton();
            this.DockRightButton = new System.Windows.Forms.ToolStripButton();
            this.DockBottomButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ZoomFit = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ToggleRectangleSelectionTool = new System.Windows.Forms.ToolStripButton();
            this.SelectionSizeLabel = new System.Windows.Forms.ToolStripLabel();
            this.LockSelectionSizeButton = new System.Windows.Forms.ToolStripButton();
            this.IncreaseSelectionSize = new System.Windows.Forms.ToolStripButton();
            this.DecreaseSelectionSize = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ToggleSegmentSelection = new System.Windows.Forms.ToolStripButton();
            this.ShowChart = new System.Windows.Forms.ToolStripButton();
            this.TestWavefront = new System.Windows.Forms.ToolStripButton();
            this.PhaseShiftDeviceCalibration = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.PreviewPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Preview)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ControlPanel);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.PreviewPanel);
            this.splitContainer1.Size = new System.Drawing.Size(800, 450);
            this.splitContainer1.SplitterDistance = 136;
            this.splitContainer1.TabIndex = 0;
            // 
            // ControlPanel
            // 
            this.ControlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ControlPanel.Location = new System.Drawing.Point(0, 0);
            this.ControlPanel.Name = "ControlPanel";
            this.ControlPanel.Size = new System.Drawing.Size(796, 132);
            this.ControlPanel.TabIndex = 0;
            // 
            // PreviewPanel
            // 
            this.PreviewPanel.Controls.Add(this.Preview);
            this.PreviewPanel.Controls.Add(this.toolStrip1);
            this.PreviewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PreviewPanel.Location = new System.Drawing.Point(0, 0);
            this.PreviewPanel.Name = "PreviewPanel";
            this.PreviewPanel.Size = new System.Drawing.Size(796, 306);
            this.PreviewPanel.TabIndex = 1;
            // 
            // Preview
            // 
            this.Preview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Preview.Location = new System.Drawing.Point(0, 25);
            this.Preview.Name = "Preview";
            this.Preview.Size = new System.Drawing.Size(796, 281);
            this.Preview.TabIndex = 2;
            this.Preview.TabStop = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DockLeftButton,
            this.DockTopButton,
            this.DockRightButton,
            this.DockBottomButton,
            this.toolStripSeparator1,
            this.ZoomFit,
            this.toolStripSeparator2,
            this.ToggleRectangleSelectionTool,
            this.SelectionSizeLabel,
            this.LockSelectionSizeButton,
            this.IncreaseSelectionSize,
            this.DecreaseSelectionSize,
            this.toolStripSeparator3,
            this.ToggleSegmentSelection,
            this.ShowChart,
            this.TestWavefront,
            this.PhaseShiftDeviceCalibration});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(796, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // DockLeftButton
            // 
            this.DockLeftButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DockLeftButton.Image = global::Camera.Properties.Resources.application_dock_180;
            this.DockLeftButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DockLeftButton.Name = "DockLeftButton";
            this.DockLeftButton.Size = new System.Drawing.Size(23, 22);
            this.DockLeftButton.Text = "Показывать слева";
            this.DockLeftButton.Click += new System.EventHandler(this.DockLeftButton_Click);
            // 
            // DockTopButton
            // 
            this.DockTopButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DockTopButton.Image = global::Camera.Properties.Resources.application_dock_090;
            this.DockTopButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DockTopButton.Name = "DockTopButton";
            this.DockTopButton.Size = new System.Drawing.Size(23, 22);
            this.DockTopButton.Text = "Показывать сверху";
            this.DockTopButton.Click += new System.EventHandler(this.DockTopButton_Click);
            // 
            // DockRightButton
            // 
            this.DockRightButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DockRightButton.Image = global::Camera.Properties.Resources.application_dock;
            this.DockRightButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DockRightButton.Name = "DockRightButton";
            this.DockRightButton.Size = new System.Drawing.Size(23, 22);
            this.DockRightButton.Text = "Показывать справа";
            this.DockRightButton.Click += new System.EventHandler(this.DockRightButton_Click);
            // 
            // DockBottomButton
            // 
            this.DockBottomButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DockBottomButton.Image = global::Camera.Properties.Resources.application_dock_270;
            this.DockBottomButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DockBottomButton.Name = "DockBottomButton";
            this.DockBottomButton.Size = new System.Drawing.Size(23, 22);
            this.DockBottomButton.Text = "Показывать снизу";
            this.DockBottomButton.Click += new System.EventHandler(this.DockBottomButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // ZoomFit
            // 
            this.ZoomFit.CheckOnClick = true;
            this.ZoomFit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ZoomFit.Image = global::Camera.Properties.Resources.magnifier_zoom_fit;
            this.ZoomFit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ZoomFit.Name = "ZoomFit";
            this.ZoomFit.Size = new System.Drawing.Size(23, 22);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // ToggleRectangleSelectionTool
            // 
            this.ToggleRectangleSelectionTool.CheckOnClick = true;
            this.ToggleRectangleSelectionTool.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToggleRectangleSelectionTool.Image = global::Camera.Properties.Resources.selection;
            this.ToggleRectangleSelectionTool.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToggleRectangleSelectionTool.Name = "ToggleRectangleSelectionTool";
            this.ToggleRectangleSelectionTool.Size = new System.Drawing.Size(23, 22);
            this.ToggleRectangleSelectionTool.Text = "Рабочая область";
            this.ToggleRectangleSelectionTool.Click += new System.EventHandler(this.ToggleSelectionTool_Click);
            // 
            // SelectionSizeLabel
            // 
            this.SelectionSizeLabel.Name = "SelectionSizeLabel";
            this.SelectionSizeLabel.Size = new System.Drawing.Size(86, 22);
            this.SelectionSizeLabel.Text = "[Selection Size]";
            // 
            // LockSelectionSizeButton
            // 
            this.LockSelectionSizeButton.CheckOnClick = true;
            this.LockSelectionSizeButton.Image = global::Camera.Properties.Resources._lock;
            this.LockSelectionSizeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.LockSelectionSizeButton.Name = "LockSelectionSizeButton";
            this.LockSelectionSizeButton.Size = new System.Drawing.Size(23, 22);
            this.LockSelectionSizeButton.Click += new System.EventHandler(this.LockSelectionSizeButton_Click);
            // 
            // IncreaseSelectionSize
            // 
            this.IncreaseSelectionSize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.IncreaseSelectionSize.Image = global::Camera.Properties.Resources.selection_resize;
            this.IncreaseSelectionSize.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.IncreaseSelectionSize.Name = "IncreaseSelectionSize";
            this.IncreaseSelectionSize.Size = new System.Drawing.Size(23, 22);
            this.IncreaseSelectionSize.Click += new System.EventHandler(this.IncreaseSelectionSize_Click);
            // 
            // DecreaseSelectionSize
            // 
            this.DecreaseSelectionSize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DecreaseSelectionSize.Image = global::Camera.Properties.Resources.selection_resize_actual;
            this.DecreaseSelectionSize.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DecreaseSelectionSize.Name = "DecreaseSelectionSize";
            this.DecreaseSelectionSize.Size = new System.Drawing.Size(23, 22);
            this.DecreaseSelectionSize.Click += new System.EventHandler(this.DecreaseSelectionSize_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // ToggleSegmentSelection
            // 
            this.ToggleSegmentSelection.CheckOnClick = true;
            this.ToggleSegmentSelection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToggleSegmentSelection.Image = global::Camera.Properties.Resources.layer_shape_line;
            this.ToggleSegmentSelection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToggleSegmentSelection.Name = "ToggleSegmentSelection";
            this.ToggleSegmentSelection.Size = new System.Drawing.Size(23, 22);
            this.ToggleSegmentSelection.Click += new System.EventHandler(this.ToggleSegmentSelection_Click);
            // 
            // ShowChart
            // 
            this.ShowChart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ShowChart.Image = global::Camera.Properties.Resources.chart_curve;
            this.ShowChart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ShowChart.Name = "ShowChart";
            this.ShowChart.Size = new System.Drawing.Size(23, 22);
            this.ShowChart.Click += new System.EventHandler(this.ShowChart_Click);
            // 
            // TestWavefront
            // 
            this.TestWavefront.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TestWavefront.Image = ((System.Drawing.Image)(resources.GetObject("TestWavefront.Image")));
            this.TestWavefront.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TestWavefront.Name = "TestWavefront";
            this.TestWavefront.Size = new System.Drawing.Size(23, 22);
            this.TestWavefront.Click += new System.EventHandler(this.TestWavefront_Click);
            // 
            // PhaseShiftDeviceCalibration
            // 
            this.PhaseShiftDeviceCalibration.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.PhaseShiftDeviceCalibration.Image = ((System.Drawing.Image)(resources.GetObject("PhaseShiftDeviceCalibration.Image")));
            this.PhaseShiftDeviceCalibration.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.PhaseShiftDeviceCalibration.Name = "PhaseShiftDeviceCalibration";
            this.PhaseShiftDeviceCalibration.Size = new System.Drawing.Size(23, 22);
            this.PhaseShiftDeviceCalibration.Click += new System.EventHandler(this.PhaseShiftDeviceCalibration_Click);
            // 
            // CameraInputViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.Name = "CameraInputViewForm";
            this.Text = "CameraInputViewForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CameraInputViewForm_FormClosing);
            this.Load += new System.EventHandler(this.CameraInputViewForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.PreviewPanel.ResumeLayout(false);
            this.PreviewPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Preview)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel ControlPanel;
        private System.Windows.Forms.Panel PreviewPanel;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton DockLeftButton;
        private System.Windows.Forms.ToolStripButton DockTopButton;
        private System.Windows.Forms.ToolStripButton DockRightButton;
        private System.Windows.Forms.ToolStripButton DockBottomButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton ToggleRectangleSelectionTool;
        private System.Windows.Forms.ToolStripButton ZoomFit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel SelectionSizeLabel;
        private System.Windows.Forms.ToolStripButton LockSelectionSizeButton;
        private System.Windows.Forms.ToolStripButton IncreaseSelectionSize;
        private System.Windows.Forms.ToolStripButton DecreaseSelectionSize;
        private System.Windows.Forms.PictureBox Preview;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton ToggleSegmentSelection;
        private System.Windows.Forms.ToolStripButton ShowChart;
        private System.Windows.Forms.ToolStripButton TestWavefront;
        private System.Windows.Forms.ToolStripButton PhaseShiftDeviceCalibration;
    }
}