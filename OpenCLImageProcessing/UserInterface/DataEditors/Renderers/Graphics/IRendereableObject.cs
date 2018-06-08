using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace UserInterface.DataEditors.Renderers.Graphics
{
    public interface IRenderableObject
    {
        void Draw();
    }

    public abstract class RenderableObjectBase: IRenderableObject
    {
        public abstract void Draw();
    }
}
