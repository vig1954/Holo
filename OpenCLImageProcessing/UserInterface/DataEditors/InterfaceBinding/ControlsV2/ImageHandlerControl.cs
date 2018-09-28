using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;
using Processing;
using UserInterface.DataEditors.InterfaceBinding.Attributes;
using UserInterface.Utility;

namespace UserInterface.DataEditors.InterfaceBinding.ControlsV2
{
    public partial class ImageHandlerControl : UserControl, IBindableControl
    {
        private IValueBinding _binding;
        private IImageHandler _imageHandler;

        public List<ImageFormat> AllowedImageFormats = new List<ImageFormat>();
        public List<ImagePixelFormat> AllowedPixelFormats = new List<ImagePixelFormat>();
        public int? RequiredChannelCount;
        public bool OnlyImages;

        public bool HideLabel { get; private set; }
        public IBinding Binding => _binding;

        public ImageHandlerControl()
        {
            InitializeComponent();
        }

        public void SetBinding(IBinding binding)
        {
            _binding = BindingUtil.PrepareValueBinding(binding, _binding, BindingOnValueUpdated, t => true);

            HideLabel = _binding.GetAttribute<BindToUIAttribute>().HideLabel;

            var imageHandlerFilter = _binding.GetAttribute<ImageHandlerFilterAttribute>();
            if (imageHandlerFilter != null)
            {
                AllowedImageFormats = imageHandlerFilter.AllowedImageFormats;
                AllowedPixelFormats = imageHandlerFilter.AllowedPixelFormats;
                RequiredChannelCount = imageHandlerFilter.RequiredChannelCount;
                OnlyImages = imageHandlerFilter.OnlyImages;
            }

            SetValue((IImageHandler) _binding.GetValue());
        }

        private void BindingOnValueUpdated(ValueUpdatedEventArgs e)
        {
            if (e.Sender == this)
                return;

            SetValue((IImageHandler) _binding.GetValue());
        }

        private void SetValue(IImageHandler value)
        {
            _imageHandler = value;

            if (_imageHandler == null)
            {
                picAddImage.BackgroundImage = Properties.Resources.icons8_add_new_16__1_;
                picAddImage.Image = null;
                lblImageTitle.Text = AllowedImageFormats.Any()
                    ? AllowedImageFormats.Select(f => f.ToString()).ToCommaSeparated() + "\n"
                    : "";
                lblImageTitle.Text += AllowedPixelFormats.Any()
                    ? AllowedPixelFormats.Select(f => f.ToString()).ToCommaSeparated() + "\n"
                    : "";
                lblImageTitle.Text += RequiredChannelCount.HasValue ? "Каналов: " + RequiredChannelCount.Value : "";
            }
            else
            {
                lblImageTitle.Text = $"{_imageHandler.GetTitle()}\n{_imageHandler.Width} x {_imageHandler.Height}";
                if (_imageHandler.Tags.TryGetValue(ImageHandlerTagKeys.Thumbnail, out object thumbnail))
                {
                    picAddImage.BackgroundImage = null;
                    picAddImage.Image = (Image) thumbnail;
                    picAddImage.Refresh();
                }
            }
        }

        private bool ImageHandlerPredicate(IImageHandler imageHandler)
        {
            if (!imageHandler.IsReady())
                return false;

            if (OnlyImages && imageHandler.GetType() != typeof(ImageHandler))

                if (AllowedImageFormats.Any() && !AllowedImageFormats.Contains(imageHandler.Format))
                    return false;

            if (AllowedPixelFormats.Any() && !AllowedPixelFormats.Contains(imageHandler.PixelFormat))
                return false;

            if (RequiredChannelCount.HasValue && imageHandler.GetChannelsCount() != RequiredChannelCount.Value)
                return false;

            return true;
        }

        private void ShowImageHandlerSelector()
        {
            if (_imageHandler != null)
                return;

            if (Application.OpenForms.OfType<SelectImageHandlerPopupForm>().Any())
            {
                var forms = Application.OpenForms.OfType<SelectImageHandlerPopupForm>().ToArray();
                if (forms.Any(f => f.Visible))
                    return;

                foreach (var form in forms)
                {
                    form.Close();
                    form.Dispose();
                }
            }

            var selectImageHandlerForm = new SelectImageHandlerPopupForm(ImageHandlerPredicate);
            selectImageHandlerForm.Show();
            selectImageHandlerForm.Location = Cursor.Position;

            selectImageHandlerForm.OnImageHandlerSelected += handler => _binding.SetValue(handler, selectImageHandlerForm);
        }

        private void picAddImage_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                ShowImageHandlerSelector();
            else if (e.Button == MouseButtons.Middle)
                _binding.SetValue(null, sender == this ? null : sender);
        }
    }
}