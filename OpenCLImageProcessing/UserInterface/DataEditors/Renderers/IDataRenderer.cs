using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UserInterface.DataEditors.Tools;

namespace UserInterface.DataEditors.Renderers
{
    public interface IDataRenderer : IDisposable
    {
        //ITool CurrentTool { get; set; }
        Type DataType { get; }
        void Resize(Size size);

        object GetData();
        void SetData(object data);
        void Update();
        string GetTitle();  // TODO: заменить на OnTitleUpdate?

        event Action OnUpdateRequest;
        event Action UpdateControlsRequest;
    }
}