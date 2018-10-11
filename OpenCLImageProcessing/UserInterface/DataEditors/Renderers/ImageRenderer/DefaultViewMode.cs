using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Processing.ImageReaders;
using UserInterface.DataEditors.InterfaceBinding;
using UserInterface.DataEditors.InterfaceBinding.Attributes;
using UserInterface.DataEditors.Renderers.Shaders;

namespace UserInterface.DataEditors.Renderers.ImageRenderer
{
    // TODO: возможно этот класс бесполезен
    public class DefaultViewMode : IDataRendererViewMode
    {
        private ImageRenderer _imageRenderer;
        
        [BindToUI, BindMembersToUI(HideProperty = true, MergeMembers = true)]
        public IImageShader Shader { get; }
        
        public void SetViewParametres(IViewParametres viewParams)
        {
            Shader.SetProjectionMatrix(viewParams.ProjectionMatrix);
            Shader.SetViewMatrix(viewParams.ViewMatrix);
        }

        public DefaultViewMode(ImageRenderer renderer) : this(renderer, new SimpleShader())
        {
        }

        public DefaultViewMode(ImageRenderer renderer, IImageShader shader)
        {
            Shader = shader;
            Shader.PropertiesChanged += ShaderPropertiesChanged;
            _imageRenderer = renderer;
        }

        public void ShaderPropertiesChanged()
        {
            _imageRenderer.RequestUpdate();
        }
    }
}
