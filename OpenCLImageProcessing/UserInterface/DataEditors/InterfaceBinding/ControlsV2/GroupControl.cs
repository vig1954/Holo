using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserInterface.DataEditors.InterfaceBinding.ControlsV2
{
    public partial class GroupControl : UserControl, IGroupControl
    {
        private bool _isExpanded = false;

        public Image CollapsedIcon { get; set; }
        public Image ExpandedIcon { get; set; }

        public string Title
        {
            get => lblTitle.Text;
            set => lblTitle.Text = value;
        }

        public GroupControl()
        {
            InitializeComponent();
        }

        public void AddControl(Control control)
        {
            throw new NotImplementedException();
        }
    }
}