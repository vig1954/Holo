using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UserInterface.DataEditors.InterfaceBinding.ControlsV2
{
    public interface IGroupControl
    {
        public string Title { get; set; }
        public void AddControl(Control control);
    }
}
