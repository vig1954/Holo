using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UserInterface.DataProcessorViews
{
    public interface IDataProcessorView
    {
        void PopulateControl(Control container);
    }
}
