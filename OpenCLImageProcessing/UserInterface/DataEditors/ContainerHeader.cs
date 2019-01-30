using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UserInterface.DataEditors
{
    public partial class ContainerHeader : UserControl
    {
        private bool _active;
        private bool _closeEnabled;
        private bool _splitEnabled;
        private bool _newWindowEnabled;
        public Color ActiveFontColor => Color.White;
        public Color ActiveBackgroundColor => Color.SteelBlue;
        public Color InactiveFontColor => Color.Black;
        public Color InactiveBackgroundColor => Color.LightGray;

        public bool CloseEnabled
        {
            get => _closeEnabled;
            set
            {
                btnClose.Enabled = value;
                _closeEnabled = value;
            }
        }

        public bool SplitEnabled
        {
            get => _splitEnabled;
            set
            {
                btnSplitRight.Enabled = value;
                btnSplitBottom.Enabled = value;
                _splitEnabled = value;
            }
        }

        public bool NewWindowEnabled
        {
            get => _newWindowEnabled;
            set
            {
                _newWindowEnabled = value;
                btnNewWindow.Enabled = value;
            }
        }

        public event Action CloseClicked;
        public event Action NewWindowClicked;
        public event Action SplitBottomClicked;
        public event Action SplitRightClicked;
        public event Action HeaderClicked;


        public bool Active
        {
            get => _active;
            set
            {
                _active = value;

                HeaderLabel.ForeColor = _active ? ActiveFontColor : InactiveFontColor;
                BackColor = _active ? ActiveBackgroundColor : InactiveBackgroundColor;
                btnClose.BackColor = BackColor;
                btnNewWindow.BackColor = BackColor;
                btnSplitBottom.BackColor = BackColor;
                btnSplitRight.BackColor = BackColor;
                OnPaint(null);
            }
        }

        public override string Text
        {
            get => HeaderLabel.Text;
            set => HeaderLabel.Text = value;
        }

        public ContainerHeader()
        {
            InitializeComponent();

        }

        private void ContainerHeader_Load(object sender, EventArgs e)
        {
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            CloseClicked?.Invoke();
        }

        private void btnSplitRight_Click(object sender, EventArgs e)
        {
            SplitRightClicked?.Invoke();
        }

        private void btnSplitBottom_Click(object sender, EventArgs e)
        {
            SplitBottomClicked?.Invoke();
        }

        private void btnNewWindow_Click(object sender, EventArgs e)
        {
            NewWindowClicked?.Invoke();
        }

        private void ContainerHeader_Click(object sender, EventArgs e)
        {
            HeaderClicked?.Invoke();
        }

        private void HeaderLabel_Click(object sender, EventArgs e)
        {
            HeaderClicked?.Invoke();
        }
    }
}