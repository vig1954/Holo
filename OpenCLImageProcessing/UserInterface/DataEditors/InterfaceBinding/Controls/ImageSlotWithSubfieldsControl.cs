using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Processing;
using UserInterface.DataEditors.InterfaceBinding.Deprecated;

namespace UserInterface.DataEditors.InterfaceBinding.Controls
{
    public partial class ImageSlotWithSubfieldsControl : UserControl, IBindableControl
    {
        public event Action<BindableControlValueUpdatedEventArgs> ValueUpdated;
        
        public List<ImageFormat> AllowedImageFormats
        {
            get => imageSlotControl1.AllowedImageFormats;
            set => imageSlotControl1.AllowedImageFormats = value;
        }

        public List<ImagePixelFormat> AllowedPixelFormats
        {
            get => imageSlotControl1.AllowedPixelFormats;
            set => imageSlotControl1.AllowedPixelFormats = value;
        }

        public int? RequiredChannelCount
        {
            get => imageSlotControl1.RequiredChannelCount;
            set => imageSlotControl1.RequiredChannelCount = value;
        }

        public string Title
        {
            get => imageSlotControl1.Title;
            set => imageSlotControl1.Title = value;
        }

        public object Value
        {
            get => imageSlotControl1.Value;
        }

        public ImageSlotWithSubfieldsControl()
        {
            InitializeComponent();
            RecalculateControlsSize();
            subfieldGroupControl1.Resize += (sender, args) => RecalculateControlsSize();

            imageSlotControl1.ValueUpdated += (e) =>
            {
                if (e.Sender != this)
                {
                    if (imageSlotControl1.Value == null)
                    {
                        subfieldGroupControl1.Hide();
                        RecalculateControlsSize();
                    }
                    else
                    {
                        subfieldGroupControl1.Show();
                        subfieldGroupControl1.Title = ((IImageHandler) imageSlotControl1.Value).GetTitle();
                        subfieldGroupControl1.FillControls(new Binder(imageSlotControl1.Value));
                    }
                }

                ValueUpdated?.Invoke(e);
            };
        }

        public void SetValue(object value, object sender)
        {
            imageSlotControl1.SetValue(value, sender);
        }

        private void subfieldGroupControl1_Resize(object sender, EventArgs e)
        {
            Height = subfieldGroupControl1.Top + subfieldGroupControl1.Height + Padding.Bottom;
            OnResize(new EventArgs());
        }

        private void RecalculateControlsSize()
        {
            if (subfieldGroupControl1.Visible)
            {
                Height = subfieldGroupControl1.Top + subfieldGroupControl1.Height + Padding.Bottom;
            }
            else
            {
                Height = subfieldGroupControl1.Top;
            }
        }
    }
}
