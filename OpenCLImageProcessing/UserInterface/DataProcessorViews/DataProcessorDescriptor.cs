using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Common;

namespace UserInterface.DataProcessorViews
{
    public class DataProcessorDescriptor
    {
        private MethodInfo _processorMethod;

        protected IReadOnlyCollection<DataProcessorParameterBase> AllParameters { get; }
        public IReadOnlyCollection<DataProcessorParameterBase> Parameters { get; }
        public DataProcessorParameterBase Output { get; }

        public string ProcessorName { get; }

        public DataProcessorDescriptor(Expression<Action> targetMethodInvocationExpression) : this(((MethodCallExpression) targetMethodInvocationExpression.Body).Method)
        {
        }

        public DataProcessorDescriptor(MethodInfo processorMethod)
        {
            if (!processorMethod.IsStatic)
                throw new NotSupportedException("Non-static methods are not supported.");

            _processorMethod = processorMethod;

            ProcessorName = processorMethod.Name.SeparateUpperCase();

            var parameterInfos = processorMethod.GetParameters();
            AllParameters = parameterInfos.Select(DataProcessorParameterFactory.CreateFor).ToArray();

            Output = AllParameters.SingleOrDefault(v => v.IsOutput);

            if (Output == null && _processorMethod.ReturnType != typeof(void))
            {
                throw new NotSupportedException();
//                Output = DataProcessorParameterFactory.CreateFor(processorMethod.ReturnType);
//                Output.IsMethodResult = true;
            }
            else if (Output == null)
                throw new InvalidOperationException("Processor method should have return value or parameter marked with 'Output' attribute.");

            Parameters = AllParameters.Where(v => !v.IsOutput).ToArray();

            foreach (var parameter in Parameters.Where(p => !p.HasValue))
            {
                parameter.SetValue(parameter.ValueType.GetDefaultValue(), this);
            }
        }

        public void Invoke()
        {
            _processorMethod.Invoke(null, AllParameters.Select(p => p.GetValue()).ToArray());
        }

        protected bool AreAllParametersSet()
        {
            return Parameters.All(p => p.HasValue);
        }
    }
}