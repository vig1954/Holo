using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace UserInterface.DataEditors
{
    public class DataEditorManager
    {
        private List<DataEditorView> _editors = new List<DataEditorView>();
        private Dictionary<int, SplitContainer> _editorsSplitContainers = new Dictionary<int, SplitContainer>();

        public DataEditorManager(DataEditorView root)
        {
            _editors.Add(root);
            root.CloseEnabled = false;

            RegisterEvents(root);
        }

        public DataEditorView Add(DataEditorView where, Orientation orientation)
        {
            var parent = where.Parent;
            if (parent == null)
                throw new InvalidOperationException();

            var splitContainer = new SplitContainer
            {
                Width = where.Width,
                Height = where.Height,
                Top = where.Top,
                Left = where.Left,
                Anchor = where.Anchor,
                Dock = where.Dock,
                Orientation = orientation
            };
            parent.Controls.Remove(where);
            parent.Controls.Add(splitContainer);

            splitContainer.Panel1.Controls.Add(where);
            where.Dock = DockStyle.Fill;

            _editorsSplitContainers.Add(where.Id, splitContainer);

            var newEditor = new DataEditorView();
            splitContainer.Panel2.Controls.Add(newEditor);
            newEditor.Dock = DockStyle.Fill;
            splitContainer.Panel2Collapsed = false;
            splitContainer.Panel1Collapsed = false;
            splitContainer.SplitterDistance = splitContainer.Width / 2;

            _editors.Add(newEditor);
            SetActive(newEditor);
            
            if (!where.CloseEnabled)
                where.CloseEnabled = true;

            RegisterEvents(newEditor);
            newEditor.SplitEnabled = true;

            return newEditor;
        }

        public void Remove(DataEditorView what)
        {
            var parent = what.Parent;
            if (parent is SplitterPanel panel)
            {
                if (!(panel.Parent is SplitContainer splitContainer))
                    throw new InvalidOperationException();

                var otherPanel = panel == splitContainer.Panel1 ? splitContainer.Panel2 : splitContainer.Panel1;
                if (!(otherPanel.Controls[0] is DataEditorView otherEditor))
                    throw new InvalidOperationException();

                panel.Controls.Remove(what);
                otherPanel.Controls.Remove(otherEditor);

                var splitContainerParent = splitContainer.Parent;
                splitContainerParent.Controls.Remove(splitContainer);
                splitContainerParent.Controls.Add(otherEditor);
                otherEditor.Dock = splitContainer.Dock;

                if (_editorsSplitContainers.ContainsKey(otherEditor.Id))
                    _editorsSplitContainers.Remove(otherEditor.Id);

                if (_editorsSplitContainers.ContainsKey(what.Id))
                    _editorsSplitContainers.Remove(what.Id);

                _editors.Remove(what);

                if (what.Active)
                    SetActive(otherEditor);

                splitContainer.Dispose();

                if (_editors.Count == 1)
                    otherEditor.CloseEnabled = false;
            }
        }
        
        public void SetActive(DataEditorView view)
        {
            if (view.Active)
                return;

            var activeEditor = GetActive();
            if (activeEditor != null)
                activeEditor.Active = false;

            view.Active = true;
        }

        public DataEditorView GetActive()
        {
            return _editors.SingleOrDefault(v => v.Active);
        }

        private void RegisterEvents(DataEditorView view)
        {
            view.Close += () => Remove(view);
            view.SplitRight += () => Add(view, Orientation.Vertical);
            view.SplitBottom += () => Add(view, Orientation.Horizontal);
            view.HeaderClicked += () => SetActive(view);
        }

        public string GetSettings()
        {
            var settings = new Settings
            {
                EditorsSplitterDistances =
                    _editorsSplitContainers.ToDictionary(p => p.Key, p => p.Value.SplitterDistance)
            };

            return JsonConvert.SerializeObject(settings);
        }

        public void ApplySettings(string settingsJson, bool throwIfAny = false)
        {
            var settings = JsonConvert.DeserializeObject<Settings>(settingsJson);

            foreach (var editorSplitterDistance in settings.EditorsSplitterDistances)
            {
                if (!_editorsSplitContainers.TryGetValue(editorSplitterDistance.Key, out var splitContainer))
                {
                    if (throwIfAny)
                        throw new InvalidOperationException("Не удалось применить настройки");
                    else
                        return;
                }

                splitContainer.SplitterDistance = editorSplitterDistance.Value;
            }
        }

        private class Settings
        {
            public Dictionary<int, int> EditorsSplitterDistances { get; set; }
        }
    }
}