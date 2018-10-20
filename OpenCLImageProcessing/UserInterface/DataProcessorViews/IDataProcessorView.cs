using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Processing;

namespace UserInterface.DataProcessorViews
{
    public interface IDataProcessorView
    {
        DataProcessorInfo Info { get; }

        event Action<IImageHandler> OnImageCreate;
        event Action OnUpdated;
        void Initialize();
        void Compute();

        IEnumerable<object> GetOutputValues();
    }
}
