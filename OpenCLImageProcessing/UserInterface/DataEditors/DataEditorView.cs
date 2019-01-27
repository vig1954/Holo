using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infrastructure;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Processing.Computing;
using UserInterface.DataEditors.InterfaceBinding;
using UserInterface.DataEditors.Renderers;
using UserInterface.DataEditors.Tools;

// ReSharper disable All

namespace UserInterface.DataEditors
{
    public partial class DataEditorView : UserControl
    {
        private InterfaceController _rightPanelInterfaceController;
        private bool IsInDesignMode = false;
        private IDataRenderer _renderer;
        private GLControl _glControl;
        private ITool _tool;

        private Control rightPanel => splitContainer1.Panel2;

        public object Data => _renderer?.GetData();
        public bool HasData => Data != null;

        public DataEditorView()
        {
            InitializeComponent();
            IsInDesignMode = LicenseManager.UsageMode == LicenseUsageMode.Designtime;
            _rightPanelInterfaceController = new InterfaceController(rightPanel);
        }

        public void SetData(object data)
        {
            var renderer = DataRendererUtil.GetRendererFor(data.GetType());
            if (renderer == null)
                return;

            renderer.SetData(data);
            SetRenderer(renderer);

            containerHeader1.Text = $"[{renderer.GetType().Name} - {renderer.GetData().GetType().Name}] {renderer.GetTitle()}";

            Redraw();
        }

        public void UpdateRendererControls()
        {
            _rightPanelInterfaceController.BindObjectToInterface(_renderer);

            rightPanel_Resize(null, null);
        }

        public void UpdateRendererTools()
        {
            tsTools.Items.Clear();

            var tools = _renderer.GetTools();

            foreach (var tool in tools)
            {
                var button = new ToolStripButton(tool.ButtonInfo.Text, tool.ButtonInfo.Icon);
                tsTools.Items.Add(button);

                button.Click += (sender, args) =>
                {
                    if (_tool != null)
                    {
                        _tool.Deactivate();
                        ClearToolPanel();
                        foreach (var toolStripButton in tsTools.Items.OfType<ToolStripButton>())
                        {
                            toolStripButton.Checked = false;
                        }
                    }

                    _tool = tool;
                    FillToolPanel(_tool);
                    button.Checked = true;
                    _tool.Activate();
                };
            }

            tsTools.Items.Add(new ToolStripSeparator());
        }

        private void ClearToolPanel()
        {
            tsToolPanel.Items.Clear();
        }

        private void FillToolPanel(ITool tool)
        {
            tool.PopulateToolstrip(tsToolPanel);
        }

        public void SetFirstEmptyDataPropertyIfExist(object value)
        {
            _rightPanelInterfaceController.BindingProvider.SetFirstEmptyValue(value, this);
        }

        private void SetRenderer(IDataRenderer renderer)
        {
            Clear();
            //if (_renderer != null && _renderer != renderer)
            //    _renderer.OnUpdateRequest -= RendererOnUpdateRequest;

            _renderer = renderer;
            renderer.Resize(_glControl.ClientSize);

            UpdateRendererControls();
            UpdateRendererTools();

            _renderer.OnUpdateRequest += RendererOnUpdateRequest;
            _renderer.UpdateControlsRequest += UpdateRendererControls;
        }

        private void RendererOnUpdateRequest()
        {
            Redraw();
        }

        private void Clear()
        {
            _tool?.Deactivate();
            _tool = null;
            _renderer?.Dispose();
            _renderer = null;

            ClearToolPanel();
            rightPanel.Controls.Clear();
        }

        private void DataEditorView_Load(object sender, EventArgs e)
        {
            if (IsInDesignMode)
            {
                containerHeader1.Text = "DESIGNER MODE";
                return;
            }

            containerHeader1.Text = "";

            InitializeGlControl();
        }

        private void InitializeGlControl()
        {
            // TODO: в более надлежащее место
            OpenGlApplication.Setup();

            _glControl = new GLControl
            {
                Parent = openTkControlContainer,
                Dock = DockStyle.Fill
            };

            _glControl.CreateControl();
            _glControl.Resize += GlControlOnResize;

            GL.Viewport(Point.Empty, _glControl.Size);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            OpenGlApplication.EnableDebugMessages();

            // TODO: в более надлежащее место
            Singleton.Get<OpenClApplication>().SetupUsingOpenGLContext();

            _glControl.Paint += GlControlOnPaint;

            _glControl.MouseClick += (sender, args) => _tool?.MouseEvent(MouseEventData.FromMouseClickEvent(args));
            _glControl.MouseDoubleClick += (sender, args) => _tool?.MouseEvent(MouseEventData.FromMouseDoubleClickEvent(args));
            _glControl.MouseDown += (sender, args) => _tool?.MouseEvent(MouseEventData.FromMouseDownEvent(args));
            _glControl.MouseUp += (sender, args) => _tool?.MouseEvent(MouseEventData.FromMouseUpEvent(args));
            _glControl.MouseMove += (sender, args) => _tool?.MouseEvent(MouseEventData.FromMouseMoveEvent(args));
            _glControl.MouseWheel += (sender, args) => _tool?.MouseEvent(MouseEventData.FromMouseWheelEvent(args));
        }

        private void GlControlOnPaint(object sender, PaintEventArgs paintEventArgs)
        {
            Redraw();
        }

        private void GlControlOnResize(object sender, EventArgs eventArgs)
        {
            GL.Viewport(Point.Empty, _glControl.Size);
            _renderer?.Resize(_glControl.ClientSize);
        }

        public void Redraw()
        {
            _glControl.MakeCurrent();
            _renderer?.Update();
            _glControl.SwapBuffers();
        }

        private void rightPanel_Resize(object sender, EventArgs e)
        {
//            foreach (var childControl in rightPanel.Controls.Cast<Control>())
//            {
//                childControl.Dock = DockStyle.None;
//                childControl.Width = rightPanel.ClientSize.Width - rightPanel.Padding.Left - rightPanel.Padding.Right - 10;
//                childControl.Height = childControl.PreferredSize.Height;
//            }
        }
    }
}