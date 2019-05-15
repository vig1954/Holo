using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Common;
using UserInterface.DataEditors.InterfaceBinding.Attributes;
using UserInterface.DataEditors.InterfaceBinding.Controls;

namespace UserInterface.DataEditors.InterfaceBinding
{
    public class PropertyListManager : IPropertyRenderer
    {
        private IBindingProvider _bindingProvider;
        private FlowLayoutPanel _flowLayoutPanel;
        private bool _justifyGroupHeight = true;
        private IReadOnlyCollection<ControlGroup> _controlGroups;
        public Size PreferredSize { get; private set; }
        public bool AutoWidth { get; set; }

        private int GroupWidth => AutoWidth ? _flowLayoutPanel.Width - _flowLayoutPanel.Padding.Left - _flowLayoutPanel.Padding.Right : 200;

        public Control Render(IBindingProvider bindingProvider)
        {
            _flowLayoutPanel = new FlowLayoutPanel
            {
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight
            };

            _flowLayoutPanel.Resize += FlowLayoutPanelOnResize;
            _bindingProvider = bindingProvider;

            ReRender();

            return _flowLayoutPanel;
        }

        private void FlowLayoutPanelOnResize(object sender, EventArgs e)
        {
            if (_controlGroups.IsNullOrEmpty() || !AutoWidth)
                return;

            foreach (var controlGroup in _controlGroups)
            {
                controlGroup.GroupBox.Width = GroupWidth - controlGroup.GroupBox.Margin.Left - controlGroup.GroupBox.Margin.Right;
            }
        }

        public void ReRender()
        {
            _flowLayoutPanel.Controls.Clear();
            _controlGroups = BuildGontrolGroups(GetBindableControls(_bindingProvider));

            var maxHeight = 0;
            var maxGroupBoxHorizontalMargins = 0;
            foreach (var controlGroup in _controlGroups)
            {
                controlGroup.Render(GroupWidth);
                maxHeight = Math.Max(maxHeight, controlGroup.GroupBox.Height);
                maxGroupBoxHorizontalMargins = Math.Max(maxGroupBoxHorizontalMargins, controlGroup.GroupBox.Margin.Left + controlGroup.GroupBox.Margin.Right);
            }

            var totalHeight = 0;
            foreach (var controlGroup in _controlGroups)
            {
                if (_justifyGroupHeight)
                    controlGroup.GroupBox.Height = maxHeight;

                totalHeight += controlGroup.GroupBox.Height;
                _flowLayoutPanel.Controls.Add(controlGroup.GroupBox);
            }

            PreferredSize = new Size(GroupWidth + _flowLayoutPanel.Padding.Left + _flowLayoutPanel.Padding.Right + maxGroupBoxHorizontalMargins, totalHeight + _flowLayoutPanel.Padding.Top + _flowLayoutPanel.Padding.Bottom);
        }

        private IReadOnlyCollection<ControlGroup> BuildGontrolGroups(IReadOnlyCollection<IBindableControl> bindableControls)
        {
            var controlGroups = new Dictionary<string, ControlGroup>();

            foreach (var bindableControl in bindableControls)
            {
                HandleBindableControl(controlGroups, bindableControl);
            }

            return controlGroups.Values.ToArray();
        }

        private void HandleBindableControl(Dictionary<string, ControlGroup> controlGroups, IBindableControl bindableControl)
        {
            var groupName = bindableControl.Binding.DisplayGroup;

            if (!controlGroups.TryGetValue(groupName, out var group))
            {
                group = new ControlGroup(groupName);
                controlGroups.Add(groupName, group);
            }

            AddBindableControlAndAllChildControlsToControlGroup(group, bindableControl);
        }

        private void AddBindableControlAndAllChildControlsToControlGroup(ControlGroup group, IBindableControl bindableControl)
        {
            var bindMembersToUiAttribute = bindableControl.Binding.GetAttribute<BindMembersToUIAttribute>();

            if (bindMembersToUiAttribute == null || !bindMembersToUiAttribute.HideProperty)
                group.ControlNodes.Add(new ControlNode(bindableControl));

            if (bindMembersToUiAttribute != null && bindMembersToUiAttribute.HideProperty && bindMembersToUiAttribute.MergeMembers)
            {
                var valueBinding = (IValueBinding) bindableControl.Binding;
                var controls = GetBindableControls(valueBinding.GetValue());

                valueBinding.ValueUpdated += sender => { group.Render(GroupWidth); };

                foreach (var control in controls)
                {
                    AddBindableControlAndAllChildControlsToControlGroup(group, control);
                }
            }
        }

        private IReadOnlyCollection<IBindableControl> GetBindableControls(object value)
        {
            if (value == null)
                return new IBindableControl[] { };

            var bindingProvider = new BindingProviderFactory().Get(value);
            return GetBindableControls(bindingProvider);
        }

        private IReadOnlyCollection<IBindableControl> GetBindableControls(IBindingProvider bindingProvider)
        {
            var bindings = bindingProvider.GetBindings();

            var bindableControlFactory = new BindableControlFactory();
            return bindings.Select(bindableControlFactory.Get).ToArray();
        }

        private class ControlGroup
        {
            public int Width { get; private set; }
            public string Title { get; }

            public GroupBox GroupBox { get; }

            public List<ControlNode> ControlNodes { get; } = new List<ControlNode>();

            public ControlGroup(string title)
            {
                Title = title;
                GroupBox = new GroupBox
                {
                    Text = title
                };
            }

            public void Render(int width)
            {
                Width = width;
                GroupBox.Controls.Clear();
                GroupBox.Width = width;

                const int paddingLeft = 5;
                const int paddingRight = 5;
                const int paddingTop = 20;
                const int labelMarginBottom = 2;
                const int verticalInterval = 5;
                var innerWidth = width - paddingLeft - paddingRight;
                int y = paddingTop;
                int x = paddingLeft;
                foreach (var node in ControlNodes)
                {
                    if (!(node.BindableControl is Control control))
                        throw new InvalidOperationException();

                    node.Label.TextAlign = ContentAlignment.MiddleLeft;
                    if (node.BindableControl.LabelMode == UiLabelMode.Title)
                    {
                        GroupBox.Controls.Add(node.Label);
                        node.Label.Top = y;
                        node.Label.Left = x;
                        y += labelMarginBottom + node.Label.Height;
                    }

                    GroupBox.Controls.Add(control);
                    control.Top = y;
                    control.Left = x;
                    control.Width = innerWidth;
                    control.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

                    if (node.BindableControl.LabelMode == UiLabelMode.Inline)
                    {
                        control.Width = control.PreferredSize.Width;
                        control.Anchor = AnchorStyles.Top | AnchorStyles.Left;

                        GroupBox.Controls.Add(node.Label);

                        node.Label.Top = y;
                        node.Label.Left = x + control.Width;
                        node.Label.Width = innerWidth - control.Width;
                        node.Label.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                    }

                    y += control.Height + verticalInterval;
                }

                GroupBox.Height = y;
            }
        }

        private class ControlNode
        {
            public Label Label { get; }
            public IBindableControl BindableControl { get; }

            public ControlNode(IBindableControl control)
            {
                BindableControl = control;
                Label = new Label
                {
                    Text = control.Binding.DisplayName
                };
            }
        }
    }
}