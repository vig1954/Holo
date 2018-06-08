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
using Processing.DataProcessors;
using UserInterface.Utility;

namespace HolographicInterferometryVNext
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
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
                    && dataEditor1.EditorView.HasData && dataEditor1.EditorView.Data is IDataProcessor)
                {
                    dataEditor1.EditorView.SetFirstEmptyDataPropertyIfExist(item.Data);
                }
            };

            FillMenus();
        }

        private void FillMenus()
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
    }
}
