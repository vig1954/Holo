using System;

namespace Camera
{
    public interface IFloatArrayDataProvider
    {
        float[] Data { get; }

        event Action<float[]> OnDataUpdated;
    }
}