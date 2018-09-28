using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;
using UserInterface.DataEditors.InterfaceBinding.Attributes;
using UserInterface.DataEditors.InterfaceBinding.ControlsV2;

namespace UserInterface.DataEditors.InterfaceBinding
{
    public class PropertyTableManager
    {
        private TableLayoutPanel _table;
        private TreeNode _rootNode;

        public TableLayoutPanel Render(IReadOnlyCollection<ControlsV2.IBindableControl> bindableControls)
        {
            _rootNode = BuildTree(bindableControls);

            _table = new TableLayoutPanel
            {
                ColumnCount = 2,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None
            };

            _table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 0));
            _table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            _table.AutoScroll = true;

            RenderTable();

            return _table;
        }

        private void RenderTable()
        {
            var row = 0;
            _table.Controls.Clear();

            var nodeCount = _rootNode.GetChildNodeCount();
            _table.RowCount = nodeCount + 1;

            _rootNode.UpdateLabelText();
            RenderNodes(_rootNode.Children, ref row);

            _table.Controls.Add(new Label
            {
                Text = "",
                TextAlign = ContentAlignment.TopCenter,
                Dock = DockStyle.Fill
            }, 0, row);

            _table.ColumnStyles[0].Width = _rootNode.GetPreferredLabelWidth();
        }

        private void RenderNodes(IReadOnlyCollection<TreeNode> nodes, ref int row)
        {
            foreach (var node in nodes)
            {
                if (node.IsGroupTitle)
                {
                    _table.Controls.Add(node.Label, 0, row);
                    node.Label.Dock = DockStyle.Fill;
                    _table.SetColumnSpan(node.Label, 2);
                }
                else
                    InsertBindableControl(node.BindableControl, node.Label, row);

                if (node.Children.Any() && !node.LabelClickHandlerAttached)
                {
                    node.Label.Click += (sender, args) =>
                    {
                        node.Expanded = !node.Expanded;
                        RenderTable();
                    };

                    node.LabelClickHandlerAttached = true;
                }

                row++;

                if (node.Children.Count > 0 && node.Expanded)
                {
                    RenderNodes(node.Children, ref row);
                }
            }
        }

        private void InsertBindableControl(ControlsV2.IBindableControl bindableControl, Label label, int row)
        {
            if (!(bindableControl is Control control))
                throw new InvalidOperationException();

            var hideLabel = bindableControl.HideLabel;

            _table.Controls.Add(control, hideLabel ? 0 : 1, row);

            if (!hideLabel)
            {
                _table.Controls.Add(label, 0, row);
                label.Dock = DockStyle.Fill;
            }
            else
                _table.SetColumnSpan(control, 2);

            control.Dock = DockStyle.Fill;
        }

        private TreeNode BuildTree(IReadOnlyCollection<ControlsV2.IBindableControl> bindableControls)
        {
            var nodeList = new List<TreeNode>();

            foreach (var bindableControl in bindableControls)
            {
                var group = bindableControl.Binding.DisplayGroup;
                var treeNode = new TreeNode
                {
                    Label = new Label { TextAlign = ContentAlignment.MiddleRight },
                    BindableControl = bindableControl,
                    Title = bindableControl.Binding.DisplayName
                };

                if (bindableControl.Binding.GetAttribute<BindMembersToUIAttribute>() != null)
                {
                    var propertyBinding = (PropertyBinding) bindableControl.Binding;

                    bindMembersToUi();

                    propertyBinding.ValueUpdated += sender =>
                    {
                        bindMembersToUi();
                        RenderTable();
                    };

                    void bindMembersToUi()
                    {
                        // TODO: дублирование с InterfaceController

                        var value = propertyBinding.GetValue();

                        if (value == null)
                        {
                            treeNode.Children = new List<TreeNode>();
                            return;
                        }

                        var bindingProvider = new BindingProviderFactory().Get(value);
                        var bindings = bindingProvider.GetBindings();

                        var bindableControlFactory = new BindableControlFactory();
                        var controls = bindings.Select(bindableControlFactory.Get).ToArray();

                        var valueRootNode = BuildTree(controls);
                        treeNode.Children = valueRootNode.Children;
                        treeNode.Label.Font = new Font(treeNode.Label.Font, FontStyle.Bold);
                    }
                }

                if (group == "")
                    nodeList.Add(treeNode);
                else
                {
                    var groupNode = nodeList.SingleOrDefault(n => n.IsGroupTitle && n.Title == group);
                    if (groupNode == null)
                    {
                        groupNode = new TreeNode
                        {
                            IsGroupTitle = true,
                            Title = group,
                            Label = new Label { TextAlign = ContentAlignment.BottomLeft }
                        };

                        groupNode.Label.Font = new Font(groupNode.Label.Font, FontStyle.Bold);

                        nodeList.Add(groupNode);
                    }

                    groupNode.Children.Add(treeNode);
                }
            }

            return new TreeNode
            {
                IsGroupTitle = true,
                Title = "root",
                Children = nodeList
            };
        }

        private class TreeNode
        {
            private const string CollapsedMarker = "▶"; // U+25B6
            private const string ExpandedMarker = "▼"; // U+25BC

            private int? level;

            public Label Label { get; set; }
            public ControlsV2.IBindableControl BindableControl { get; set; }
            public bool IsGroupTitle { get; set; }
            public string Title { get; set; }
            public bool Expanded { get; set; }
            public bool LabelClickHandlerAttached { get; set; } // TODO: это ужасно
            public List<TreeNode> Children { get; set; } = new List<TreeNode>();

            public int GetChildNodeCount(bool countCollapsedItems = false)
            {
                if (!countCollapsedItems && !Expanded)
                    return 0;

                var count = Children.Count;

                foreach (var child in Children)
                {
                    count += child.GetChildNodeCount(countCollapsedItems);
                }

                return count;
            }

            public void UpdateLabelText()
            {
                if (Title == "root")
                    UpdateLevels(-1);

                foreach (var child in Children)
                {
                    child.UpdateLabelText();
                }

                if (Label == null)
                    return;

                var padding = " ".Repeat(level.Value);
                var marker = IsGroupTitle
                    ? Expanded ? ExpandedMarker : CollapsedMarker
                    : "";

                Label.Text = padding + marker + Title;
            }

            public int GetPreferredLabelWidth()
            {
                var maxLabelWidth = IsGroupTitle ? 0 : Label.PreferredWidth + Label.Margin.Left + Label.Margin.Right + 3;

                foreach (var child in Children)
                {
                    maxLabelWidth = Math.Max(maxLabelWidth, child.GetPreferredLabelWidth());
                }

                return maxLabelWidth;
            }

            private void UpdateLevels(int myLevel)
            {
                level = myLevel;

                foreach (var child in Children)
                {
                    child.UpdateLevels(level.Value + 1);
                }
            }
        }
    }
}