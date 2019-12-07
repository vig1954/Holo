using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Processing;

namespace SimpleApplication
{
    public partial class DataPlotForm : Form
    {
        private readonly ImageHandlerDataSource[] _imageHandlerDataSources;
        private float[] _data;

        public DataPlotForm(Dictionary<string, ImageHandler> dataSources)
        {
            InitializeComponent();
            _imageHandlerDataSources = dataSources.Select(p => new ImageHandlerDataSource(p.Key, p.Value)).ToArray();
        }

        private void DataPlotForm_Load(object sender, EventArgs e)
        {
            SourceSelector.Items.AddRange(_imageHandlerDataSources);

            ChannelSelector.Items.Add(new ChannelReadMethodWrapper("Амплитуда", h => h.ReadAmplitudeFromComputingDevice()));
            ChannelSelector.Items.Add(new ChannelReadMethodWrapper("Фаза", h => h.ReadPhaseFromComputingDevice()));
            ChannelSelector.Items.Add(new ChannelReadMethodWrapper("Действительные значения", h => h.ReadRealFromComputingDevice()));
            ChannelSelector.Items.Add(new ChannelReadMethodWrapper("Мнимые значения", h => h.ReadImaginativeFromComputingDevice()));
        }

        private void ReadDataButton_Click(object sender, EventArgs e)
        {
            if (SourceSelector.SelectedItem == null)
                return;

            if (ChannelSelector.SelectedItem == null)
                return;

            var handler = (SourceSelector.SelectedItem as ImageHandlerDataSource)?.ImageHandler;
            if (handler == null || !handler.Ready)
                throw new InvalidOperationException();

            var channelReader = (ChannelSelector.SelectedItem as ChannelReadMethodWrapper)?.ReadChannel;
            if (channelReader == null)
                throw new InvalidOperationException();

            _data = channelReader(handler);

            // todo: draw data
        }

        private class ChannelReadMethodWrapper
        {
            public string Title { get; }
            public Func<ImageHandler, float[]> ReadChannel { get; }

            public ChannelReadMethodWrapper(string title, Func<ImageHandler, float[]> readChannel)
            {
                Title = title;
                ReadChannel = readChannel;
            }
        }
    }
}
