using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;
using Infrastructure;
using OpenTK;
using Processing;
using UserInterface.DataEditors.Renderers.Graphics.Selection;
using UserInterface.DataEditors.Renderers.ImageRenderer;
using UserInterface.Properties;

namespace UserInterface.DataEditors.Tools
{
    public class SelectionTool : ToolBase
    {
        public enum SelectionMode
        {
            Square512X512,
            Square1024X1024,
            Square2048X2048
        }

        private const string NewSelectionName = "<Новая область>";

        private ToolboxButtonInfo _buttonInfo;

        private ImageSelection _newSelection;   // TODO: можно избавиться от этого
        private ImageSelection _currentSelection;
        private SelectionMode _currentSelectionMode = SelectionMode.Square2048X2048;

        private ToolStripComboBox _selectionDropdown;
        private ToolStripComboBox _selectionModeDropdown;
        private ToolStripButton _saveSelectionButton;

        private SelectionDrawable _drawable;
        private IReadOnlyCollection<ImageSelection> AvailableSelections => Singleton.Get<ImageSelectionManager>().GetAllSelections();
        public override ToolboxButtonInfo ButtonInfo => _buttonInfo;
        
        private ImageRenderer _renderer;

        public SelectionTool(ImageRenderer renderer)
        {
            _renderer = renderer;
            _buttonInfo = new ToolboxButtonInfo
            {
                Icon = Resources.select,
                Text = "Выделение"
            };

            if (!_renderer.ImageHandler.IsReady() || !Singleton.Get<ImageSelectionManager>().TryGetSelection(_renderer.ImageHandler, out _newSelection))
            {
                _newSelection = new ImageSelection
                {
                    Name = NewSelectionName
                };
                _newSelection.Width = 2048;
                _newSelection.Height = 2048;
            }

            _currentSelection = _newSelection;
            _drawable = new SelectionDrawable();
            _drawable.Selection = _currentSelection;
        }


        public void Save()
        {
            
        }

        public override void MouseEvent(MouseEventData eventData)
        {
            if ((eventData.Event == MouseEventData.EventType.Down ||
                 eventData.Event == MouseEventData.EventType.Move)
                && eventData.Args.Button == MouseButtons.Left)
            {
                if (_currentSelection == _newSelection)
                {
                    var imagePos = _renderer.GetImageCoordinate(new Vector2(eventData.Args.X, eventData.Args.Y));
                    _currentSelection.MoveTo(imagePos);
                    _drawable.SelectionUpdate();
                }
            }
        }

        public override void Activate()
        {
            Cursor.Current = Cursors.Cross;
            _renderer.AddDrawable(_drawable);
        }

        public override void Deactivate()
        {
            Cursor.Current = Cursors.Default;
            _renderer.RemoveDrawable(_drawable);
        }

        public override void PopulateToolstrip(ToolStrip toolStrip)
        {
            _selectionDropdown = new ToolStripComboBox("Selection");
            _selectionDropdown.Items.AddRange(new[] {_newSelection}.Concat(AvailableSelections).Select(s => (object) s).ToArray());
            _selectionDropdown.DropDownStyle = ComboBoxStyle.DropDownList;
            _selectionDropdown.SelectedItem = _currentSelection;
            _selectionDropdown.SelectedIndexChanged += (sender, args) =>
            {
                _currentSelection = (ImageSelection) _selectionDropdown.SelectedItem;
                _saveSelectionButton.Enabled = _currentSelection.Name == NewSelectionName;
                _drawable.Selection = _currentSelection;
                _drawable.SelectionUpdate();
            };

            _selectionModeDropdown =new ToolStripComboBox("Selection Mode");
            _selectionModeDropdown.Items.AddRange(EnumExtensions.GetValues<SelectionMode>().Select(v => (object)v).ToArray());
            _selectionModeDropdown.DropDownStyle = ComboBoxStyle.DropDownList;
            _selectionModeDropdown.SelectedItem = _currentSelectionMode;
            _selectionModeDropdown.SelectedIndexChanged += (sender, args) =>
            {
                _currentSelectionMode = (SelectionMode) _selectionModeDropdown.SelectedItem;

                int size = 2048;
                if (_currentSelectionMode == SelectionMode.Square512X512)
                    size = 512;
                else if (_currentSelectionMode == SelectionMode.Square1024X1024)
                    size = 1024;

                _currentSelection.Width = size;
                _currentSelection.Height = size;
                _drawable.SelectionUpdate();
            };

            _saveSelectionButton = new ToolStripButton("Сохранить")
            {
                Enabled = _currentSelection.Name == NewSelectionName
            };
            _saveSelectionButton.Click += (sender, args) =>
            {
                var imageName = _renderer.ImageHandler?.GetTitle() ?? "Область";
                _currentSelection.Name = $"{imageName} {AvailableSelections.Count(s => s.Name.StartsWith(imageName)) + 1}";
                Singleton.Get<ImageSelectionManager>().SetSelection(_renderer.ImageHandler, _currentSelection);
                _saveSelectionButton.Enabled = false;
                _selectionDropdown.SelectedItem = _currentSelection;
            };

            toolStrip.Items.Add(_selectionDropdown);
            toolStrip.Items.Add(_selectionModeDropdown);
            toolStrip.Items.Add(_saveSelectionButton);
        }
    }
}