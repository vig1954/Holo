using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cloo;
using OpenTK;
using Processing;

namespace IntegrationTests.KernelTests
{
    public class SplitFftTest : KernelTestBase
    {
        private int[] _testInput;
        private int[] _testOutput;

        private IImageHandler _imageHandler;
        private ComputeBuffer<Vector2> _outputBuffer;
        private int _n;
        private int _m;
        private int _l;
        private int _t;

        public override void SetupTestData()
        {
            _imageHandler = CreateImageFromIntArray(_testInput);

        }

        public override void Execute()
        {
        }
    }
}
