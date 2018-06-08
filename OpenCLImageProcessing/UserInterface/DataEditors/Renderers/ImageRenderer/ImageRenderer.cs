using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Common;
using Infrastructure;
using OpenTK.Graphics.OpenGL;
using Processing;
using Processing.DataBinding;
using UserInterface.DataEditors.Renderers.Graphics;
using UserInterface.DataEditors.Renderers.Shaders;

namespace UserInterface.DataEditors.Renderers.ImageRenderer
{
    [DataRendererFor(typeof(IImageHandler))]
    public class ImageRenderer: IDataRenderer
    {
        private bool zoomIsSet;
        
        private IImageHandler _imageHandler;
        private ImagePlane imagePlane;
        private IImageShader _shader;
        private ViewParametres _viewParametres;

        private IDataRendererControlMode _controlMode;
        private IDataRendererViewMode _viewMode;

        [MergeSubfields]
        public IImageHandler ImageHandler => _imageHandler;
        [MergeSubfields]
        public IDataRendererControlMode ControlMode => _controlMode;
        [MergeSubfields]
        public IDataRendererViewMode ViewMode => _viewMode;

        public Type DataType => typeof(IImageHandler);

        public event Action OnUpdateRequest;
        public event Action UpdateControlsRequest;

        public ImageRenderer()
        {
            imagePlane = new ImagePlane();
            _viewParametres = new ViewParametres();
        }

        public void Resize(Size size)
        {
            _viewParametres.ViewportSize = size;

            if (zoomIsSet)
                return;

            zoomIsSet = true;
            ZoomFit();
        }

        public void SetData(object data)
        {
            var imageHandler = data as IImageHandler;

            if (imageHandler == null)
                throw new InvalidOperationException($"Аргумент {nameof(data)} должен наследовать интерфейс {nameof(IImageHandler)}");

            if (_imageHandler != null)
                _imageHandler.ImageUpdated -= ImageHandlerImageUpdated;

            _imageHandler = imageHandler;
            _imageHandler.UploadToComputingDevice();
            imagePlane.SetImage(_imageHandler);

            _imageHandler.ImageUpdated += ImageHandlerImageUpdated;

            UpdateControls();
        }

        private void ImageHandlerImageUpdated(ImageUpdatedEventData obj)
        {
            UpdateShaderValuesRange();
            RequestUpdate();

            if (obj.ReloadControls)
                UpdateControls();
        }

        public object GetData()
        {
            return _imageHandler;
        }

        public void Update()
        {
            if (!_imageHandler.IsReady())
                return;

            _shader.Use();
            _viewMode.SetViewParametres(_viewParametres);
            _shader.SetModelMatrix(imagePlane.GetModelMatrix(_viewParametres));
            
            GL.ClearColor(Color.AntiqueWhite);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            imagePlane.Draw();
        }

        public string GetTitle()
        {
            if (_imageHandler.Tags.TryGetValue(ImageHandlerTagKeys.Title, out object title))
                return (string) title;

            return "Image Editor";
        }

        public void RequestUpdate()
        {
            OnUpdateRequest?.Invoke();
        }

        private void UpdateControls()
        {
            SetControlMode();
            SetViewMode();
            UpdateControlsRequest?.Invoke();
        }
        private void SetControlMode()
        {
            if (_controlMode == null)
                _controlMode = new DefaultControlMode(this);
        }

        private void SetViewMode()
        {
            if (_imageHandler.IsReady())
            {
                var availableShaders = Singleton.Get<ShaderProvider>().GetFor(_imageHandler);

                if (availableShaders.Any())
                    _viewMode = new DefaultViewMode(this, availableShaders.First());
            }
            else
                _viewMode = new DefaultViewMode(this);

            _shader = _viewMode.Shader;
            UpdateShaderValuesRange();
        }

        public void ZoomEquals()
        {
            _viewParametres.Zoom = 1f;
        }

        public void ZoomFit()
        {
            if (!_imageHandler.IsReady())
                return;

            var iRatio = (float)_imageHandler.Width / _imageHandler.Height;
            var vRatio = _viewParametres.ViewportRatio;
            float zoom;
            if (iRatio > vRatio)
                zoom = _imageHandler.Width / _viewParametres.ViewportSize.Width;
            else
                zoom = _imageHandler.Height / _viewParametres.ViewportSize.Height;

            _viewParametres.Zoom = 1/zoom;
        }

        public void Dispose()
        {
            if (_imageHandler != null)
                _imageHandler.ImageUpdated -= ImageHandlerImageUpdated;

            _imageHandler = null;
            _shader = null;
            _controlMode = null;
            _viewMode = null;
            _viewParametres = null;
        }

        private void UpdateShaderValuesRange()
        {
            if (!_imageHandler.IsReady())
                return;

            var range1 = _imageHandler.GetValueRangeForChannel(0);

            var hasTwoChannels = _imageHandler.GetChannelsCount() == 2;
            var range2 = hasTwoChannels ? _imageHandler.GetValueRangeForChannel(1) : new FloatRange();

            _shader.SetValueRange(range1, range2);
            //Debug.WriteLine($"Image values range: {range1}; {range2}");
        }
    }
}
