using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Camera;
using Common;
using Infrastructure;
using Processing;
using Processing.Computing;
using UserInterface.DataEditors;
using UserInterface.DataEditors.InterfaceBinding;
using UserInterface.DataEditors.Renderers;
using UserInterface.DataProcessorViews;
using UserInterface.Events;
using UserInterface.ImageSeries;
using UserInterface.Utility;
using UserInterface.WorkspacePanel;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace HolographicInterferometryVNext
{
    public partial class MainForm : Form, IEventHandler<ShowInEditorEvent>
    {
        public DataProcessorViewRepository DataProcessorViewRepository { get; set; }
        public ImageHandlerRepository ImageHandlerRepository { get; set; }
        public DataEditorManager DataEditorManager { get; set; }

        public MainForm()
        {
            InitializeComponent();

            DataProcessorViewRepository = Singleton.Get<DataProcessorViewRepository>();
            ImageHandlerRepository = Singleton.Get<ImageHandlerRepository>();

            this.Closing += OnClosing;

            Singleton.Get<EventManager>().Subscribe(this);
        }
        
        private void MainForm_Load(object sender, EventArgs e)
        {
            AppDomain.CurrentDomain.Load(new AssemblyName("Camera"));

            DataEditorManager = new DataEditorManager(dataEditorView1);
            Singleton.Register(DataEditorManager);
            DataEditorManager.SetActive(dataEditorView1);

            InitializeWorkspacePanel();

            FillMenus();


            var lastSessionValuesJson = Properties.Settings.Default.SessionValues;

            if (!lastSessionValuesJson.IsNullOrEmpty())
                Singleton.Get<SessionValues>().FromJson(lastSessionValuesJson);

            timer1.Start();

            ValueBindingSynchronizer.ValueUpdateStarted += UpdateManager.Lock;
            ValueBindingSynchronizer.ValueUpdateFinished += UpdateManager.Unlock;
        }

        private void InitializeWorkspacePanel()
        {
            Singleton.Get<ImageSeriesRepository>().ItemAdded += imageSeries =>
            {
                var psi4Processor = DataProcessorViewCreator.For(typeof(Psi),nameof(Psi.Psi4)).Create();
                imageSeries.AddDataProcessor(psi4Processor);

                var freshnelProcessor = DataProcessorViewCreator.For(typeof(Freshnel),nameof(Freshnel.Transform)).Create();
                imageSeries.AddDataProcessor(freshnelProcessor);

                DataProcessorViewRepository.Add(psi4Processor);
                DataProcessorViewRepository.Add(freshnelProcessor);

                workspacePanel1.AddSeries(imageSeries);
            };

            workspacePanel1.OnOpenCameraClick += ShowCameraForm;

            workspacePanel1.OnItemAdded += item =>
            {
                var activeEditor = DataEditorManager.GetActive();

                if (activeEditor != null && !activeEditor.HasData)
                {
                    activeEditor.SetData(item.Data);
                    // workspacePanel1.MarkItemSelected(item.View);
                }
            };

            workspacePanel1.OnItemDoubleClick += (item, me) =>
            {
                if (!item.View.Selected && me.Button == MouseButtons.Left)
                {
                    DataEditorManager.GetActive()?.SetData(item.Data);
                    // workspacePanel1.MarkItemSelected(item.View);
                }
            };
            
            workspacePanel1.OnItemClick += (item, me) =>
            {
                var activeEditor = DataEditorManager.GetActive();
                // TODO: move to settings
                if (ModifierKeys == Keys.Control && me.Button == MouseButtons.Left && activeEditor != null && activeEditor.HasData && activeEditor.Data is IDataProcessorView)
                {
                    activeEditor.SetFirstEmptyDataPropertyIfExist(item.Data);
                }
            };

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
                DisplayCondition = item => item.Data is IImageHandler,
                Text = "Дублировать",
                Action = item =>
                {
                    var copy = (item.Data as IImageHandler).Duplicate();
                    ImageHandlerRepository.Add(copy);
                    workspacePanel1.AddItem(copy);
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
                        dataProcessorView.AutoCompute = true;
                        workspacePanel1.AddItem(dataProcessorView);

                        dataProcessorView.OnImageCreate += ImageCreate;
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

            var cameraInputMenuItem = new ToolStripMenuItem("Camera");
            cameraInputMenuItem.Click += (sender, args) => { ShowCameraForm(); };

            inputMenuItem.DropDownItems.Add(cameraInputMenuItem);
        }

        private void ShowCameraForm()
        {
            if (Process.GetProcessesByName("EOS Utility").Any())
            {
                MessageBox.Show("Обнаружен процесс EOS Utility, который вызывает проблемы при работе с камерой. Для продолжения работы необходимо завершить этот процесс.",
                    "Обнаружен EOS Utility.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                return;
            }

            CameraInputViewForm cameraInputViewForm;
            var cameraInputViewForms = Application.OpenForms.OfType<CameraInputViewForm>().ToArray();

            if (cameraInputViewForms.Any())
                cameraInputViewForm = cameraInputViewForms.Single();
            else
            {
                cameraInputViewForm = new CameraInputViewForm();
//                    cameraInputViewForm.ImageCreate += ImageCreate;
//                    cameraInputViewForm.SeriesStarted += UpdateManager.Lock;
//                    cameraInputViewForm.SeriesComplete += UpdateManager.Unlock;
            }


            cameraInputViewForm.Show();
        }

        private void ImageCreate(IImageHandler image)
        {
            workspacePanel1.AddItem(image);
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

        private void editOpenClProgramCode_Click(object sender, EventArgs e)
        {
            var editor = Application.OpenForms.OfType<OpenClProgramCodeEditor>().SingleOrDefault() ?? new OpenClProgramCodeEditor();

            editor.Show();
            editor.ReloadProgram();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (var dataProcessorView in DataProcessorViewRepository.GetAll())
            {
                dataProcessorView.Dispose();
            }

            foreach (IImageHandler handler in Singleton.Get<ImageHandlerRepository>().GetAll())
            {
                handler.FreeComputingDevice();
            }

            Properties.Settings.Default.SessionValues = Singleton.Get<SessionValues>().ToJson();
            Properties.Settings.Default.Save();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // нужно перерисовывать без перерасчетов
            //dataEditor1.EditorView.Redraw();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            Singleton.DisposeAll();
        }

        public void Handle(ShowInEditorEvent @event)
        {
            DataEditorManager.GetActive()?.SetData(@event.ImageHandler);
        }
    }
}