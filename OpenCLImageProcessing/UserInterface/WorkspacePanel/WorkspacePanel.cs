using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Processing.ImageReaders;
using Common;
using Processing;
using Processing.DataProcessors;
using UserInterface.DataProcessorViews;
using ContextMenu = System.Windows.Forms.ContextMenu;

namespace UserInterface.WorkspacePanel
{
    public partial class WorkspacePanel : UserControl
    {
        public class Item
        {
            public object Data { get; set; }
            public WorkspacePanelItem View { get; set; }
            public WorkspacePanelGroupableItemControllerBase Controller { get; set; }
        }

        private List<FolderWorkspacePanelItemController> folders = new List<FolderWorkspacePanelItemController>();
        private readonly List<Item> items = new List<Item>();
        private bool _showToolbar = true;

        public event Action<Item, MouseEventArgs> OnItemDoubleClick;
        public event Action<Item, MouseEventArgs> OnItemClick;
        public event Action<Item> OnItemAdded;

        public List<ContextMenuAction> ContextMenuActions { get; } = new List<ContextMenuAction>();

        public bool SelectItemOnClick { get; set; }
        public Item SelectedItem => items.SingleOrDefault(i => i.View.Selected);

        public bool ShowToolbar
        {
            get => _showToolbar;
            set
            {
                _showToolbar = value;
                if (_showToolbar)
                    toolStrip1.Show();
                else
                    toolStrip1.Hide();
            }
        }

        public WorkspacePanel()
        {
            InitializeComponent();
        }

        public void MarkItemSelected(WorkspacePanelItem item)
        {
            items.ForEach(i => i.View.SetSelectionState(false));
            item.SetSelectionState(true);
        }

        public void AddItem(object item)
        {
            var view = new WorkspacePanelItem();
            MainLayoutPanel.Controls.Add(view);

            var itemController = WorkspacePanelItemControllerFactory.Get(item, view);
            itemController.UpdateView();

            var itemData = new Item
            {
                Data = item,
                Controller = itemController,
                View = view
            };
            items.Add(itemData);

            view.Click += (sender, args) =>
            {
                if (args is MouseEventArgs me)
                {
                    if (SelectItemOnClick && me.Button == MouseButtons.Left)
                        MarkItemSelected(view);

                    OnItemClick?.Invoke(itemData, me);

                    if (me.Button == MouseButtons.Right)
                        ShowMenu(itemData, me.Location);
                }
            };

            view.DoubleClick += (o, args) => { OnItemDoubleClick?.Invoke(itemData, args as MouseEventArgs); };

            OnItemAdded?.Invoke(itemData);

            ResizeItems();
        }

        public void AddFolder(string title)
        {
            var view = new WorkspacePanelItem();
            MainLayoutPanel.Controls.Add(view);

            var folderController = new FolderWorkspacePanelItemController(title, view);
            folderController.Expanded = true;

            folders.Add(folderController);

            view.DoubleClick += (o, args) =>
            {
                folderController.Expanded = !folderController.Expanded;
                UpdateItemsVisibility();
            };

            view.Click += (o, args) =>
            {
                if (args is MouseEventArgs me && me.Button == MouseButtons.Right)
                {
                    var removeFolderMenuItem = new MenuItem("Удалить");
                    removeFolderMenuItem.Click += (sender, eventArgs) =>
                    {
                        folders.Remove(folderController);
                        MainLayoutPanel.Controls.Remove(folderController.View);

                        foreach (var item in items.Where(i => i.Controller.Parent == folderController))
                        {
                            item.Controller.Parent = null;
                        }

                        UpdateItemsVisibility();
                        SortItems();
                    };

                    var contextMenu = new ContextMenu(new MenuItem[]
                    {
                        removeFolderMenuItem
                    });

                    contextMenu.Show(folderController.View, me.Location);
                }
            };

            SortItems();
            ResizeItems();
        }

        /// <summary>
        /// Удаляет изображение или обработчик с панели
        /// </summary>
        /// <param name="item">Изображение или обработчик</param>
        public void Remove(object item)
        {
            var itemData = items.SingleOrDefault(i => i.Data == item);
            if (itemData == null)
                return;

            MainLayoutPanel.Controls.Remove(itemData.View);
        }

        public void OpenImage(string fileName)
        {
            var imageReaders = ImageReaderProvider.GetAll();
            IImageHandler imageHandler;
            using (var stream = File.OpenRead(fileName))
            {
                var reader = imageReaders.First(r => r.CanRead(stream));
                imageHandler = reader.Read(stream);
                stream.Close();
            }

            imageHandler.Tags[ImageHandlerTagKeys.Title] = new FileInfo(fileName).Name;
            imageHandler.Tags[ImageHandlerTagKeys.Source] = "filesystem";
            imageHandler.Tags[ImageHandlerTagKeys.FileSystemPath] = fileName;

            AddItem(imageHandler);
        }

        public void ShowMenu(Item item, Point point)
        {
            var menuItemGroups = ContextMenuActions
                .Where(a => a.DisplayCondition?.Invoke(item) ?? true)
                .GroupBy(a => a.Group);

            var contextMenu = new ContextMenu();

            var groupContextMenuActions = GetGroupContextMenuActions(item);
            if (groupContextMenuActions.Any())
                contextMenu.MenuItems.Add("Группа", groupContextMenuActions.Select(a => CreateMenuItemFromContextMenuAction(a, item)).ToArray());

            foreach (var group in menuItemGroups)
            {
                if (contextMenu.MenuItems.Count > 0)
                    contextMenu.MenuItems.Add("-");

                foreach (var groupItem in group)
                {
                    contextMenu.MenuItems.Add(CreateMenuItemFromContextMenuAction(groupItem, item));
                }
            }

            contextMenu.Show(item.View, point);
        }

        private ContextMenuAction[] GetGroupContextMenuActions(Item item)
        {
            var actions = new List<ContextMenuAction>();
            if (item.Controller.Parent != null)
            {
                actions.Add(new ContextMenuAction
                {
                    Action = i =>
                    {
                        i.Controller.Parent = null;
                        UpdateItemsVisibility();
                        SortItems();
                        ResizeItems();
                    },
                    Text = $"Убрать из '{item.Controller.Parent.Title}'"
                });
            }

            if (folders.Any())
            {
                foreach (var folder in folders)
                {
                    actions.Add(new ContextMenuAction
                    {
                        Action = i =>
                        {
                            i.Controller.Parent = folder;
                            UpdateItemsVisibility();
                            SortItems();
                            ResizeItems();
                        },
                        Text = $"Поместить в '{folder.Title}'"
                    });
                }
            }

            return actions.ToArray();
        }

        private MenuItem CreateMenuItemFromContextMenuAction(ContextMenuAction action, Item item)
        {
            var menuItem = new MenuItem(action.Text);
            menuItem.Click += (sender, args) => action.Action(item);
            return menuItem;
        }

        private void UpdateItemsVisibility()
        {
            foreach (var item in items)
            {
                item.View.Visible = item.Controller.Visible;
            }
        }

        private void SortItems()
        {
            var sortedControls = new List<Control>();
            sortedControls.AddRange(folders.Select(f => f.View));

            foreach (var item in items)
            {
                if (item.Controller.Parent != null)
                {
                    var parentIndex = sortedControls.IndexOf(item.Controller.Parent.View);
                    var parentSubItemsCount = sortedControls.Count(c => items.SingleOrDefault(i => i.View == c)?.Controller.Parent == item.Controller.Parent);
                    if (parentIndex < sortedControls.Count - 1)
                        sortedControls.Insert(parentIndex + parentSubItemsCount + 1, item.View);
                    else
                        sortedControls.Add(item.View);
                }
                else
                {
                    sortedControls.Add(item.View);
                }
            }

            var counter = 0;
            foreach (var control in sortedControls)
            {
                MainLayoutPanel.Controls.SetChildIndex(control, counter++);
            }
        }

        private void AddFolderButton_Click(object sender, EventArgs e)
        {
            AddFolder("Новая группа");
        }

        private void AddImageButton_Click(object sender, EventArgs e)
        {
            var imageReaders = ImageReaderProvider.GetAll();

            openFileDialog1.Filter = imageReaders.Select(r => r.GetFileFilter()).Join("|");
            openFileDialog1.FileName = "";
            openFileDialog1.Multiselect = true;
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;

            foreach (var fileName in openFileDialog1.FileNames)
            {
                OpenImage(fileName);
            }
        }

        private void ResizeItems()
        {
            for (var i = 0; i < MainLayoutPanel.Controls.Count; i++)
            {
                var control = MainLayoutPanel.Controls[i];
                control.Width = MainLayoutPanel.Width - MainLayoutPanel.Padding.Left - MainLayoutPanel.Padding.Right - control.Margin.Left - control.Margin.Right;
            }
        }

        public class ContextMenuAction
        {
            public Bitmap Image { get; set; }
            public string Text { get; set; }
            public Func<Item, bool> DisplayCondition { get; set; }
            public Action<Item> Action { get; set; }
            public Shortcut Shortcut { get; set; }
            public string Group { get; set; }
        }

        private void MainLayoutPanel_Resize(object sender, EventArgs e)
        {
            ResizeItems();
        }
    }
}