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

            var allVariables = processorMethod.GetParameters().Select(DataProcessorParameterFactory.CreateFor).ToArray();

            Output = allVariables.SingleOrDefault(v => v.IsOutput);

            if (Output == null && _processorMethod.ReturnType != typeof(void))
                Output = DataProcessorParameterFactory.CreateFor(processorMethod.ReturnType);
            else if (Output == null)
                throw new InvalidOperationException("Processor method should have return value or parameter marked with 'Output' attribute.");

            Parameters = allVariables.Where(v => !v.IsOutput).ToArray();

            foreach (var parameter in Parameters)
            {
                parameter.SetValue(parameter.ValueType.GetDefaultValue(), this);
            }
        }

        public void Invoke()
        {
            _processorMethod.Invoke(null, Parameters.Select(p => p.GetValue()).Concat(new[] { Output.GetValue() }).ToArray());
        }

        protected bool AreAllParametersSet()
        {
            return Parameters.All(p => p.HasValue);
        }
    }
}