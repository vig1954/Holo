using UserInterface.ImageSeries;

namespace Camera
{
    public interface IImageSeriesProvider
    {
        ImageSeries ImageSeries { get; set; }
    }
}