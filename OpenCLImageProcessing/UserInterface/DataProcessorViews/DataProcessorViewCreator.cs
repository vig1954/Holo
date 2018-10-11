using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Common;
using Processing.DataAttributes;

namespace UserInterface.DataProcessorViews
{
    public class DataProcessorViewCreator
    {
        private MethodInfo _methodInfo;

        public DataProcessorInfo DataProcessorInfo { get; set; }

        public DataProcessorViewCreator(MethodInfo methodInfo)
        {
            var dataProcessorAttribute = methodInfo.GetCustomAttribute<DataProcessorAttribute>();

            if (dataProcessorAttribute == null)
                throw new InvalidOperationException("Метод должен иметь DataProcessor аттрибут.");

            _methodInfo = methodInfo;
            DataProcessorInfo = new DataProcessorInfo(methodInfo);
        }

        public IDataProcessorView Create()
        {
            return new SingleImageOutputDataProcessorView(_methodInfo);
        }

        public static IReadOnlyCollection<DataProcessorViewCreator> For<T>()
        {
            return For(typeof(T));
        }

        public static IReadOnlyCollection<DataProcessorViewCreator> For(Type type)
        {
            var staticMethods = type.GetMethods().Where(m => m.IsStatic && m.IsPublic);
            var dataProcessorMethods = staticMethods.Where(m => m.HastAttribute<DataProcessorAttribute>());

            return dataProcessorMethods.Select(dp => new DataProcessorViewCreator(dp)).ToArray();
        }
    }
}
