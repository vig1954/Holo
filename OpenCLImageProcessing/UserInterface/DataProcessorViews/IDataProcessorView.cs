using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Processing;

namespace UserInterface.DataProcessorViews
{
    public interface IDataProcessorView: IDisposable
    {
        DataProcessorParameterBase this[string parameterName] { get; }

        event Action OnValueUpdated;

        bool AutoCompute { get; set; }
        IReadOnlyCollection<DataProcessorParameterBase> Parameters { get; }
        DataProcessorInfo Info { get; }

        event Action<IImageHandler> OnImageCreate;
        void Initialize();
        void Compute();

        IEnumerable<DataProcessorParameterBase> GetOutputs();
        IEnumerable<object> GetOutputValues();
    }
}
