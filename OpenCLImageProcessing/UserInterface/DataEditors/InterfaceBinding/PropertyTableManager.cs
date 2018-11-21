using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;
using UserInterface.DataEditors.InterfaceBinding.Attributes;
using UserInterface.DataEditors.InterfaceBinding.Controls;

namespace UserInterface.DataEditors.InterfaceBinding
{
    public class PropertyTableManager
    {
        private const int _iconWidth = 24;
        private Bitmap _synchronizeIcon => Properties.Resources.chain;
        private Bitmap _unsynchronizeIcon => Properties.Resources.chain_unchain;
        private IBindingProvider _bindingProvider;
        private TableLayoutPanel _table;
        private TreeNode _rootNode;

        public TableLayoutPanel Render(IBindingProvider bindingProvider)
        {
            _bindingProvider = bindingProvider;

            ReRender();

            return _table;
        }

        public void ReRender()
        {
            _rootNode = BuildTree(GetBindableControls(_bindingProvider));

            _table = new TableLayoutPanel
            {
                ColumnCount = 3,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None
            };

            _table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute));
            _table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute));
            _table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute));
            //_table.AutoScroll = true;

            _rootNode.ExpandAll();
            RenderTable();

            _table.Resize += TableOnResize;
        }

        public void ExpandAll()
        {
            _rootNode.ExpandAll();
            RenderTable();
        }

        public void CollapseAll()
        {
            _rootNode.CollapseAll();
            RenderTable();
        }

        private void TableOnResize(object sender, EventArgs e)
        {
            _table.ColumnStyles[2].Width = _table.Width - _table.ColumnStyles[1].Width - _iconWidth;
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

            var preferredLabelWidth = _rootNode.GetPreferredLabelWidth();
            _table.ColumnStyles[0].Width = _iconWidth;
            _table.ColumnStyles[1].Width = preferredLabelWidth;
            _table.ColumnStyles[2].Width = _table.Width - _table.ColumnStyles[1].Width - _iconWidth;
        }

        private void RenderNodes(IReadOnlyCollection<TreeNode> nodes, ref int row)
        {
            foreach (var node in nodes)
            {
                var showAsGroupTitle = node.BindableControl?.Binding.GetAttribute<BindMembersToUIAttribute>()?.HideProperty ?? false;
                if (node.IsGroupTitle || showAsGroupTitle)
                {
                    _table.Controls.Add(node.Label, 0, row);
                    node.Label.Dock = DockStyle.Fill;
                    _table.SetColumnSpan(node.Label, 3);
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

        private void InsertBindableControl(IBindableControl bindableControl, Label label, int row)
        {
            if (!(bindableControl is Control control))
                throw new InvalidOperationException();

            var hideLabel = bindableControl.HideLabel;

            _table.Controls.Add(control, hideLabel ? 0 : 2, row);

            if (!hideLabel)
            {
                _table.Controls.Add(label, 1, row);
                label.Dock = DockStyle.Fill;
            }
            else
                _table.SetColumnSpan(control, 3);

            if (!hideLabel && bindableControl.Binding is ISynchronizableBinding synchronizableBinding && synchronizableBinding.ValueType.IsPrimitive && synchronizableBinding.Synchronizer != null)
            {
                var synchronizeIcon = new PictureBox
                {
                    Width = _iconWidth,
                    Height = _iconWidth,
                    Image = synchronizableBinding.Synchronizer.Enabled ? _synchronizeIcon : _unsynchronizeIcon 
                };

                synchronizeIcon.Click += (s, e) =>
                {
                    synchronizableBinding.Synchronizer.Enabled = !synchronizableBinding.Synchronizer.Enabled;
                    synchronizeIcon.Image = synchronizableBinding.Synchronizer.Enabled ? _synchronizeIcon : _unsynchronizeIcon;
                };

                _table.Controls.Add(synchronizeIcon, 0, row);
            }

            control.Dock = DockStyle.Fill;
        }

        private TreeNode BuildTree(IReadOnlyCollection<IBindableControl> bindableControls)
        {
            var nodeList = new List<TreeNode>();

            foreach (var bindableControl in bindableControls)
            {
                AddTreeNodeAndCreateGroupTreeNodeIfNeeded(bindableControl, nodeList);
            }

            return new TreeNode
            {
                IsGroupTitle = true,
                Title = "root",
                Children = nodeList,
                Expanded = true
            };
        }

        private void AddTreeNodeAndCreateGroupTreeNodeIfNeeded(IBindableControl bindableControl, List<TreeNode> nodeList)
        {
            var group = bindableControl.Binding.DisplayGroup;
            if (!group.IsNullOrEmpty())
            {
                var groupNode = nodeList.SingleOrDefault(n => n.IsGroupTitle && n.Title == group);
                if (groupNode == null)
                {
                    groupNode = new TreeNode
                    {
                        IsGroupTitle = true,
                        Title = group,
                        Label = new Label { TextAlign = ContentAlignment.MiddleLeft }
                    };

                    groupNode.Label.Font = new Font(groupNode.Label.Font, FontStyle.Bold);

                    nodeList.Add(groupNode);
                }

                nodeList = groupNode.Children;
            }
            
            var bindMembersToUiAttribute = bindableControl.Binding.GetAttribute<BindMembersToUIAttribute>();
            if (bindMembersToUiAttribute != null && bindMembersToUiAttribute.HideProperty && bindMembersToUiAttribute.MergeMembers)
            {
                var valueBinding = (IValueBinding) bindableControl.Binding;
                var controls = GetBindableControls(valueBinding.GetValue());

                valueBinding.ValueUpdated += sender =>
                {
                    ReRender();
                };

                foreach (var control in controls)
                {
                    AddTreeNodeAndCreateGroupTreeNodeIfNeeded(control, nodeList);
                }
            }
            else
            {
                var treeNode = new TreeNode
                {
                    Label = new Label { TextAlign = ContentAlignment.MiddleRight },
                    BindableControl = bindableControl,
                    Title = bindableControl.Binding.DisplayName
                };

                if (bindMembersToUiAttribute != null)
                {
                    treeNode.Label.TextAlign = ContentAlignment.MiddleLeft;
                    
                    var valueBinding = (IValueBinding) bindableControl.Binding;
                    bindMembersToUi();

                    valueBinding.ValueUpdated += sender =>
                    {
                        bindMembersToUi();
                        RenderTable();
                    };

                    void bindMembersToUi()
                    {
                        // TODO: дублирование с InterfaceController

                        var value = valueBinding.GetValue();

                        if (value == null)
                        {
                            treeNode.Children = new List<TreeNode>();
                            return;
                        }

                        var controls = GetBindableControls(value);

                        var valueRootNode = BuildTree(controls);
                        treeNode.Children = valueRootNode.Children;
                        treeNode.Label.Font = new Font(treeNode.Label.Font, FontStyle.Bold);
                    }
                }

                nodeList.Add(treeNode);
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

        

        private class TreeNode
        {
            private const string CollapsedMarker = "▶"; // U+25B6
            private const string ExpandedMarker = "▼"; // U+25BC

            private int? level;

            public Label Label { get; set; }
            public IBindableControl BindableControl { get; set; }
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
                var marker = IsGroupTitle || Children.Any()
                    ? Expanded ? ExpandedMarker : CollapsedMarker
                    : "";

                Label.Text = padding + marker + Title;
            }

            public int GetPreferredLabelWidth()
            {
                var maxLabelWidth = IsGroupTitle || BindableControl.HideLabel ? 0 : Label.PreferredWidth + Label.Margin.Left + Label.Margin.Right + 3;

                if (Expanded)
                {
                    foreach (var child in Children)
                    {
                        maxLabelWidth = Math.Max(maxLabelWidth, child.GetPreferredLabelWidth());
                    }
                }

                return maxLabelWidth;

                int GetLabelPreferredWidth(System.Windows.Forms.Label lbl)
                {
                    var font = lbl.Font;
                    var text = lbl.Text;
                    var g = lbl.CreateGraphics();
                    return (int)g.MeasureString(text, font).Width;
                }
            }

            public void ExpandAll()
            {
                Expanded = true;

                foreach (var child in Children)
                {
                    child.ExpandAll();
                }
            }

            public void CollapseAll()
            {
                Expanded = false;

                foreach (var child in Children)
                {
                    child.CollapseAll();
                }
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