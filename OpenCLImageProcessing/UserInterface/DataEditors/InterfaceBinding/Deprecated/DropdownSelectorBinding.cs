﻿using System;
using System.Reflection;
using System.Windows.Forms;
using Processing.DataBinding;
using UserInterface.DataEditors.InterfaceBinding.Controls;

namespace UserInterface.DataEditors.InterfaceBinding.Deprecated
{
    public class DropdownSelectorBinding : PropertyBindingBase
    {
        private readonly DropdownWithLabel _dropdown;
        private readonly IPropertyWithAvailableValuesList _property;
        public override IBindableControl Control => _dropdown;

        public DropdownSelectorBinding(DropdownSelectorAttribute dropdownSelectorAttribute, MemberInfo memberInfo, object target) : base(dropdownSelectorAttribute, memberInfo, target)
        {
            if (!typeof(IPropertyWithAvailableValuesList).IsAssignableFrom(_propertyInfo.PropertyType))
                throw new NotSupportedException($"На данный момент для {nameof(DropdownSelectorBinding)} поддерживается только свойства типов реализующих интерфейс {typeof(IPropertyWithAvailableValuesList).Name}.");

            DisplayGroup = dropdownSelectorAttribute.Group;

            _dropdown = new DropdownWithLabel
            {
                Title = dropdownSelectorAttribute.TooltipText
            };
            _dropdown.SelectedIndexChanged += DropdownOnSelectedIndexChanged;

            _property = (IPropertyWithAvailableValuesList)_propertyInfo.GetValue(target);
            _property.OnAvailableValuesUpdated += PropertyOnOnAvailableValuesUpdated;
            _property.OnValueCleared += PropertyOnValueCleared;
            _property.OnValueSelected += PropertyOnOnValueSelected;

            UpdateDropdown();
        }

        private void PropertyOnOnValueSelected(object value, object sender)
        {
            if (sender != this)
                _dropdown.SetValue(value, sender);
        }

        private void DropdownOnSelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            var value = _dropdown.Value;
            _property.SetValue(value, this);
        }

        private void PropertyOnValueCleared()
        {
           
        }

        private void PropertyOnOnAvailableValuesUpdated()
        {
            UpdateDropdown();
        }

        private void UpdateDropdown()
        {
            _dropdown.Items.Clear();

            var values = _property.GetAvailableValues();

            foreach (var value in values)
            {
                _dropdown.Items.Add(value);
            }

            _dropdown.SetValue(_property.GetValue(), this);
        }
    }
}