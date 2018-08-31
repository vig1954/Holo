using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Processing;
using Processing.DataBinding;

namespace UserInterface.DataEditors.InterfaceBinding.Controls
{
    public interface IBindableControlFactory
    {
        IBindableControl Get(Type forType, IEnumerable<Attribute> customAttributes);
    }
    public class BindableControlFactory : IBindableControlFactory
    {
        public IBindableControl Get(Type forType, IEnumerable<Attribute> customAttributes)
        {
            if (forType == typeof(bool))
                return new CheckboxControl();

            if (forType == typeof(float) || forType == typeof(int))
                return new NumberControl();

            if (typeof(Enum).IsAssignableFrom(forType))
                return new EnumRadioGroupControl(forType);

            if (typeof(IImageHandler).IsAssignableFrom(forType))
                return GetImageHandlerControl(customAttributes);

            throw new NotImplementedException();
        }

        private IBindableControl GetImageHandlerControl(IEnumerable<Attribute> customAttributes)
        {
            var imageParametersAttribute = customAttributes.OfType<ImageParametersAttribute>().SingleOrDefault();

            return null;    // proceed
        }
    }
}
