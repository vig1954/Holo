﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Infrastructure;
using Processing;
using Processing.Computing;
using Processing.DataProcessors;
using UserInterface.DataProcessorViews;
using UserInterface.Utility;
using UserInterface.WorkspacePanel;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace HolographicInterferometryVNext
{
    public partial class MainForm : Form
    {
        public DataProcessorViewRepository DataProcessorViewRepository { get; set; }
        public ImageHandlerRepository ImageHandlerRepository { get; set; }

        public MainForm()
        {
            InitializeComponent();

            DataProcessorViewRepository = Singleton.Get<DataProcessorViewRepository>();
            ImageHandlerRepository = Singleton.Get<ImageHandlerRepository>();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            AppDomain.CurrentDomain.Load(new AssemblyName("Camera"));

            workspacePanel1.OnItemAdded += item =>
            {
                dataEditor1.EditorView.SetData(item.Data);
                workspacePanel1.MarkItemSelected(item.View);
            };

            workspacePanel1.OnItemDoubleClick += (item, me) =>
            {
                if (!item.View.Selected && me.Button == MouseButtons.Left)
                {
                    dataEditor1.EditorView.SetData(item.Data);
                    workspacePanel1.MarkItemSelected(item.View);
                }
            };

            workspacePanel1.OnItemClick += (item, me) =>
            {
                // TODO: move to settings
                if (ModifierKeys == Keys.Control && me.Button == MouseButtons.Left
                    && dataEditor1.EditorView.HasData && dataEditor1.EditorView.Data is IDataProcessorView)
                {
                    dataEditor1.EditorView.SetFirstEmptyDataPropertyIfExist(item.Data);
                }
            };

            FillMenus();

            workspacePanel1.ContextMenuActions.Add(new WorkspacePanel.ContextMenuAction
            {
                DisplayCondition = item => item.Data is ImageHandler,
                Text = "Сохранить",
                Action = item =>
                {
                    var imageHandler = item.Data as ImageHandler;
                    imageHandler.DownloadFromComputingDevice();
                    var bitmap = imageHandler.ToBitmap();

                    saveFileDialog1.AddExtension = true;
                    saveFileDialog1.DefaultExt = ".png";

                    var result = saveFileDialog1.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        var fileName = saveFileDialog1.FileName;
                        bitmap.Save(fileName, ImageFormat.Png);
                    }
                }
            });

            workspacePanel1.ContextMenuActions.Add(new WorkspacePanel.ContextMenuAction
            {
                Text = "Удалить",
                Action = item =>
                {
                    ImageHandlerRepository.Remove(item.Data as IImageHandler);
                    workspacePanel1.Remove(item.Data);
                }
            });

            timer1.Start();
        }

        private void FillMenus()
        {
            var dataProcessorViewCreators = GetDataProcessorViewCreators(typeof(Fourier), typeof(Freshnel), typeof(Psi), typeof(Wavefront), typeof(ImageProcessing));

            if (!dataProcessorViewCreators.Any())
                return;

            var groups = dataProcessorViewCreators.ToLookup(c => c.DataProcessorInfo.Group);

            foreach (var group in groups)
            {
                foreach (var dataProcessorViewCreator in group)
                {
                    var menuItem = new ToolStripMenuItem(dataProcessorViewCreator.DataProcessorInfo.Name)
                    {
                        ToolTipText = dataProcessorViewCreator.DataProcessorInfo.Tooltip,
                    };

                    menuItem.Click += (sender, args) =>
                    {
                        var dataProcessorView = dataProcessorViewCreator.Create();
                        dataProcessorView.Initialize();
                        workspacePanel1.AddDataProcessorView(dataProcessorView);

                        dataProcessorView.OnImageCreate += image =>
                        {
                            workspacePanel1.AddImageHandler(image);
                            var imageHandlerRepository = Singleton.Get<ImageHandlerRepository>();

                            imageHandlerRepository.Add(image);
                        };
                    };

                    if (dataProcessorViewCreator.DataProcessorInfo.MenuItem == "Input")
                        inputMenuItem.DropDownItems.Add(menuItem);
                    else
                        processingMenuItem.DropDownItems.Add(menuItem);
                }

                processingMenuItem.DropDownItems.Add(new ToolStripSeparator());
            }

            var lastSeparator = processingMenuItem.DropDownItems.OfType<ToolStripSeparator>().Last();
            processingMenuItem.DropDownItems.Remove(lastSeparator);
        }

        private IReadOnlyCollection<DataProcessorViewCreator> GetDataProcessorViewCreators(params Type[] dataProcessorContainerTypes)
        {
            var creators = new List<DataProcessorViewCreator>();

            foreach (var dataProcessorContainerType in dataProcessorContainerTypes)
            {
                creators.AddRange(DataProcessorViewCreator.For(dataProcessorContainerType));
            }

            return creators.ToArray();
        }

        private void FillMenus_Legacy()
        {
            var dataProcessorInfos = DataProcessorsProvider.GetDataProcessorInfos();

            if (!dataProcessorInfos.Any())
                return;

            var dataProcessorGroups = dataProcessorInfos.ToLookup(pi => pi.DisplayGroup?.ToLower());

            foreach (var groupedDataProcessorInfos in dataProcessorGroups)
            {
                foreach (var dataProcessorInfo in groupedDataProcessorInfos)
                {
                    var menuItem = new ToolStripMenuItem(dataProcessorInfo.DisplayName)
                    {
                        ToolTipText = dataProcessorInfo.DisplayTooltip,
                    };

                    menuItem.Click += (sender, args) =>
                    {
                        var dataProcessor = dataProcessorInfo.CreateNewInstance();
                        Singleton.Get<DataProcessorRepository>().Add(dataProcessor);
                        dataProcessor.Initialize();
                        workspacePanel1.AddDataProcessor(dataProcessor);
                        
                        dataProcessor.OnImageFinalize += image =>
                        {
                            workspacePanel1.AddImageHandler(image);
                            var imageHandlerRepository = Singleton.Get<ImageHandlerRepository>();

                            imageHandlerRepository.Add(image);

                            if (dataProcessor is IImageHandler imageHandler)
                                imageHandlerRepository.Remove(imageHandler);

                            Singleton.Get<DataProcessorRepository>().Remove(dataProcessor);
                            workspacePanel1.Remove(dataProcessor);
                        };

                        dataProcessor.OnImageCreate += image =>
                        {
                            workspacePanel1.AddImageHandler(image);
                            var imageHandlerRepository = Singleton.Get<ImageHandlerRepository>();

                            imageHandlerRepository.Add(image);
                        };
                    };

                    if (dataProcessorInfo.DisplayMenuItem == "Input")
                        inputMenuItem.DropDownItems.Add(menuItem);
                    else
                        processingMenuItem.DropDownItems.Add(menuItem);
                }

                processingMenuItem.DropDownItems.Add(new ToolStripSeparator());
            }

            var lastSeparator = processingMenuItem.DropDownItems.OfType<ToolStripSeparator>().Last();
            processingMenuItem.DropDownItems.Remove(lastSeparator);
        }

        private void editOpenClProgramCode_Click(object sender, EventArgs e)
        {
            var editor = Application.OpenForms.OfType<OpenClProgramCodeEditor>().SingleOrDefault() ?? new OpenClProgramCodeEditor();

            editor.Show();
            editor.ReloadProgram();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (IDataProcessor dataProcessor in Singleton.Get<DataProcessorRepository>().GetAll())
            {
                dataProcessor.Dispose();
            }

            foreach (IImageHandler handler in Singleton.Get<ImageHandlerRepository>().GetAll())
            {
                handler.FreeComputingDevice();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // нужно перерисовывать без перерасчетов
            //dataEditor1.EditorView.Redraw();
        }
    }
}
