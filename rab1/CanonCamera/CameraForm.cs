using System;
using System.IO;
using System.IO.Ports;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

using EDSDKLib;

namespace rab1
{
    public partial class CameraForm : Form
    {
        #region Variables

        PhaseShiftDeviceController phaseShiftDeviceController = null;
        short phaseShiftStep = 0;
        short phaseShiftCount = 0;
        short currentPhaseShiftNumber = 0;
        short currentPhaseShiftValue = 0;
        short zeroPhaseShiftValue = 0x2000;
        int delayPhaseShift = 0;
        bool takeNextPhoto = false;
        bool executeNextShift = false;
        bool isMakePhaseShiftsProcess = false;
        bool isLiveViewHandled = true;
        ColorModeEnum colorMode = ColorModeEnum.Uknown;
        ImageSaveModeEnum imageSaveMode = ImageSaveModeEnum.Uknown;

        SDKHandler CameraHandler;
        List<int> AvList;
        List<int> TvList;
        List<int> ISOList;
        List<Camera> CamList;
        Bitmap Evf_Bmp;
        int LVBw, LVBh, w, h;
        float LVBratio, LVration;

        int ErrCount;
        object ErrLock = new object();

        #endregion


        #region Events

        public PictureTakenHandler PictureTaken;
        public LiveViewUpdatedHandler LiveViewUpdated;

        #endregion

        public CameraForm()
        {
            try
            {
                InitializeComponent();
                CameraHandler = new SDKHandler();
                CameraHandler.CameraAdded += new SDKHandler.CameraAddedHandler(SDK_CameraAdded);
                CameraHandler.LiveViewUpdated += new SDKHandler.StreamUpdate(SDK_LiveViewUpdated);
                CameraHandler.ProgressChanged += new SDKHandler.ProgressHandler(SDK_ProgressChanged);
                CameraHandler.ImageDownloaded += new SDKHandler.BitmapUpdate(SDK_ImageDownloaded);
                CameraHandler.ImageSavedToDirectory += new EventHandler(SDK_ImageSavedToDirectory);
                CameraHandler.CameraHasShutdown += SDK_CameraHasShutdown;
                SavePathTextBox.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "RemotePhoto");
                LVBw = LiveViewPicBox.Width;
                LVBh = LiveViewPicBox.Height;
                RefreshCamera();
            }
            catch (DllNotFoundException) { ReportError("Canon DLLs not found!", true); }
            catch (Exception ex) { ReportError(ex.Message, true); }

            try
            {
                InitSerialPorts();
            }
            catch(Exception ex)
            {
                ReportError(ex.Message, true);
            }

            InitColorModes();
            InitImageSaveModes();
            InitializeDefaultValues();
        }
        
        private void InitializeDefaultValues()
        {
            phaseShiftCountTextBox.Text = "4";
            phaseShiftStepTextBox.Text = "100";
            DelayPhaseShiftTextBox.Text = "500";
        }
        
        private void InitColorModes()
        {
            colorComboBox.ValueMember = "ColorModeValue";
            colorComboBox.DisplayMember = "ColorModeName";
            colorComboBox.SelectedIndexChanged += colorComboBox_SelectedIndexChanged;


            colorComboBox.Items.Add(new ColorItem() { ColorModeValue = ColorModeEnum.Gray, ColorModeName = "Gray" });
            colorComboBox.Items.Add(new ColorItem() { ColorModeValue = ColorModeEnum.Red, ColorModeName = "Red" });
            colorComboBox.Items.Add(new ColorItem() { ColorModeValue = ColorModeEnum.Green, ColorModeName = "Green" });
            colorComboBox.Items.Add(new ColorItem() { ColorModeValue = ColorModeEnum.Blue, ColorModeName = "Blue" });

            colorComboBox.SelectedIndex = 0;
        }
        
        private void InitImageSaveModes()
        {
            imageSaveComboBox.ValueMember = "ImageSaveModeValue";
            imageSaveComboBox.DisplayMember = "ImageSaveModeName";
            imageSaveComboBox.SelectedIndexChanged += imageSaveComboBox_SelectedIndexChanged;

            imageSaveComboBox.Items.Add(new ImageSaveItem() { ImageSaveModeValue = ImageSaveModeEnum.Memory, ImageSaveModeName = "Memory" });
            imageSaveComboBox.Items.Add(new ImageSaveItem() { ImageSaveModeValue = ImageSaveModeEnum.Directory, ImageSaveModeName = "Directory" });
            
            imageSaveComboBox.SelectedIndex = 0;
        }
        
        private void colorComboBox_SelectedIndexChanged(object sender, EventArgs args)
        {
            if (colorComboBox.SelectedItem != null)
            {
                colorMode = ((ColorItem)colorComboBox.SelectedItem).ColorModeValue;
            }
        }

        private void imageSaveComboBox_SelectedIndexChanged(object sender, EventArgs args)
        {
            if (imageSaveComboBox.SelectedItem != null)
            {
                imageSaveMode = ((ImageSaveItem)imageSaveComboBox.SelectedItem).ImageSaveModeValue;
            }
        }

        private void InitSerialPorts()
        {
            string[] portNames = SerialPort.GetPortNames();
            for (int index = 0; index < portNames.Length; index++)
            {
                string portName = portNames[index];
                phaseShiftSerialPortComboBox.Items.Add(portName);
            }
        }

        private void CameraHandler_ImageDownloaded(Bitmap bmp)
        {
            throw new NotImplementedException();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try { if (CameraHandler != null) CameraHandler.Dispose(); }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        #region SDK Events

        private void SDK_ProgressChanged(int Progress)
        {
            try
            {
                if (Progress == 100) Progress = 0;
                this.Invoke((Action)delegate { MainProgressBar.Value = Progress; });
            }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void SDK_ImageDownloaded(Bitmap bitmap)
        {
            if (PictureTaken != null)
            {
                PictureTakenEventArgs eventArgs = new PictureTakenEventArgs()
                {
                    Image = bitmap,
                    PhaseShiftNumber = currentPhaseShiftNumber,
                    PhaseShiftValue = currentPhaseShiftValue,
                    ColorMode = colorMode
                };

                PictureTaken(eventArgs);
            }

            TryTakeNextPhoto();
        }

        private void SDK_ImageSavedToDirectory(object sender, EventArgs args)
        {
            if (PictureTaken != null)
            {
                PictureTaken(null);
            }

            TryTakeNextPhoto();
        }
        
        private void TryTakeNextPhoto()
        {
            if (takeNextPhoto)
            {
                currentPhaseShiftNumber++;
                currentPhaseShiftValue += phaseShiftStep;
                if (currentPhaseShiftNumber == phaseShiftCount)
                {
                    takeNextPhoto = false;
                }
                else
                {
                    takeNextPhoto = true;
                }

                if (currentPhaseShiftNumber <= phaseShiftCount)
                {
                    ExecutePhaseShiftAndTakePhoto();
                }
            }

        }
        
        private void SDK_LiveViewUpdated(Stream img)
        {
            if (!isLiveViewHandled) return;

            isLiveViewHandled = false;

            try
            {
                Evf_Bmp = new Bitmap(img);
                
                /*
                using (Graphics g = LiveViewPicBox.CreateGraphics())
                {
                    LVBratio = LVBw / (float)LVBh;
                    LVration = Evf_Bmp.Width / (float)Evf_Bmp.Height;
                    if (LVBratio < LVration)
                    {
                        w = LVBw;
                        h = (int)(LVBw / LVration);
                    }
                    else
                    {
                        w = (int)(LVBh * LVration);
                        h = LVBh;
                    }
                    //g.DrawImage(Evf_Bmp, 0, 0, w, h);
                }
                
                //Evf_Bmp.Dispose();
                */

                if (LiveViewUpdated != null && isMakePhaseShiftsProcess)
                {
                    LiveViewUpdatedEventArgs args = new LiveViewUpdatedEventArgs()
                    {
                        Image = Evf_Bmp,
                        PhaseShiftNumber = currentPhaseShiftNumber,
                        PhaseShiftValue = currentPhaseShiftValue,
                        ColorMode = colorMode
                    };

                    LiveViewUpdated(args);
    
                }
                else
                {
                    using (Graphics g = LiveViewPicBox.CreateGraphics())
                    {
                        LVBratio = LVBw / (float)LVBh;
                        LVration = Evf_Bmp.Width / (float)Evf_Bmp.Height;
                        if (LVBratio < LVration)
                        {
                            w = LVBw;
                            h = (int)(LVBw / LVration);
                        }
                        else
                        {
                            w = (int)(LVBh * LVration);
                            h = LVBh;
                        }
                        g.DrawImage(Evf_Bmp, 0, 0, w, h);
                    }
                }
                
                if (isMakePhaseShiftsProcess)
                {
                    if (executeNextShift)
                    {
                        currentPhaseShiftNumber++;
                        currentPhaseShiftValue += phaseShiftStep;
                        if (currentPhaseShiftNumber == phaseShiftCount)
                        {
                            executeNextShift = false;
                            isMakePhaseShiftsProcess = false;
                        }
                        else
                        {
                            executeNextShift = true;
                        }

                        if (currentPhaseShiftNumber <= phaseShiftCount)
                        {
                            ExecutePhaseShift();
                        }
                    }
                }

            }
            catch (Exception ex) { ReportError(ex.Message, false); }

            isLiveViewHandled = true;
        }

        private void SDK_CameraAdded()
        {
            try { RefreshCamera(); }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void SDK_CameraHasShutdown(object sender, EventArgs e)
        {
            try { CloseSession(); }
            catch (Exception ex) { ReportError(ex.Message, false); }

        }

        #endregion

        #region Session

        private void SessionButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (CameraHandler.CameraSessionOpen) CloseSession();
                else OpenSession();
            }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            try { RefreshCamera(); }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        #endregion

        #region Settings

        private void TakePhotoButton_Click(object sender, EventArgs e)
        {
            try
            {
                currentPhaseShiftNumber = 0;
                
                if ((string)TvCoBox.SelectedItem == "Bulb") CameraHandler.TakePhoto((uint)BulbUpDo.Value);
                else CameraHandler.TakePhoto();
            }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void RecordVideoButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CameraHandler.IsFilming)
                {
                    if (STComputerButton.Checked || STBothButton.Checked)
                    {
                        Directory.CreateDirectory(SavePathTextBox.Text);
                        CameraHandler.StartFilming(SavePathTextBox.Text);
                    }
                    else CameraHandler.StartFilming();
                    RecordVideoButton.Text = "Stop Video";
                }
                else
                {
                    CameraHandler.StopFilming();
                    RecordVideoButton.Text = "Record Video";
                }
            }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (Directory.Exists(SavePathTextBox.Text)) SaveFolderBrowser.SelectedPath = SavePathTextBox.Text;
                if (SaveFolderBrowser.ShowDialog() == DialogResult.OK)
                {
                    SavePathTextBox.Text = SaveFolderBrowser.SelectedPath;
                    CameraHandler.ImageSaveDirectory = SavePathTextBox.Text;
                }
            }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void AvCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try { CameraHandler.SetSetting(EDSDK.PropID_Av, CameraValues.AV((string)AvCoBox.SelectedItem)); }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void TvCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CameraHandler.SetSetting(EDSDK.PropID_Tv, CameraValues.TV((string)TvCoBox.SelectedItem));
                if ((string)TvCoBox.SelectedItem == "Bulb") BulbUpDo.Enabled = true;
                else BulbUpDo.Enabled = false;
            }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void ISOCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try { CameraHandler.SetSetting(EDSDK.PropID_ISOSpeed, CameraValues.ISO((string)ISOCoBox.SelectedItem)); }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void WBCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (WBCoBox.SelectedIndex)
                {
                    case 0: CameraHandler.SetSetting(EDSDK.PropID_WhiteBalance, EDSDK.WhiteBalance_Auto); break;
                    case 1: CameraHandler.SetSetting(EDSDK.PropID_WhiteBalance, EDSDK.WhiteBalance_Daylight); break;
                    case 2: CameraHandler.SetSetting(EDSDK.PropID_WhiteBalance, EDSDK.WhiteBalance_Cloudy); break;
                    case 3: CameraHandler.SetSetting(EDSDK.PropID_WhiteBalance, EDSDK.WhiteBalance_Tangsten); break;
                    case 4: CameraHandler.SetSetting(EDSDK.PropID_WhiteBalance, EDSDK.WhiteBalance_Fluorescent); break;
                    case 5: CameraHandler.SetSetting(EDSDK.PropID_WhiteBalance, EDSDK.WhiteBalance_Strobe); break;
                    case 6: CameraHandler.SetSetting(EDSDK.PropID_WhiteBalance, EDSDK.WhiteBalance_WhitePaper); break;
                    case 7: CameraHandler.SetSetting(EDSDK.PropID_WhiteBalance, EDSDK.WhiteBalance_Shade); break;
                }
            }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void SaveToButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (STCameraButton.Checked)
                {
                    CameraHandler.SetSetting(EDSDK.PropID_SaveTo, (uint)EDSDK.EdsSaveTo.Camera);
                    BrowseButton.Enabled = false;
                    SavePathTextBox.Enabled = false;
                }
                else
                {
                    if (STComputerButton.Checked) CameraHandler.SetSetting(EDSDK.PropID_SaveTo, (uint)EDSDK.EdsSaveTo.Host);
                    else if (STBothButton.Checked) CameraHandler.SetSetting(EDSDK.PropID_SaveTo, (uint)EDSDK.EdsSaveTo.Both);
                    CameraHandler.SetCapacity();
                    BrowseButton.Enabled = true;
                    SavePathTextBox.Enabled = true;
                    Directory.CreateDirectory(SavePathTextBox.Text);
                    CameraHandler.ImageSaveDirectory = SavePathTextBox.Text;
                }
            }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        
        #endregion

        #region Live view

        private void LiveViewButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CameraHandler.IsLiveViewOn) { CameraHandler.StartLiveView(); LiveViewButton.Text = "Stop LV"; }
                else { CameraHandler.StopLiveView(); LiveViewButton.Text = "Start LV"; }
            }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void LiveViewPicBox_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (CameraHandler.IsLiveViewOn && CameraHandler.IsCoordSystemSet)
                {
                    ushort x = (ushort)((e.X / (double)LiveViewPicBox.Width) * CameraHandler.Evf_CoordinateSystem.width);
                    ushort y = (ushort)((e.Y / (double)LiveViewPicBox.Height) * CameraHandler.Evf_CoordinateSystem.height);
                    CameraHandler.SetManualWBEvf(x, y);
                }
            }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void LiveViewPicBox_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                LVBw = LiveViewPicBox.Width;
                LVBh = LiveViewPicBox.Height;
            }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void FocusNear3Button_Click(object sender, EventArgs e)
        {
            try { CameraHandler.SetFocus(EDSDK.EvfDriveLens_Near3); }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void FocusNear2Button_Click(object sender, EventArgs e)
        {
            try { CameraHandler.SetFocus(EDSDK.EvfDriveLens_Near2); }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void FocusNear1Button_Click(object sender, EventArgs e)
        {
            try { CameraHandler.SetFocus(EDSDK.EvfDriveLens_Near1); }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void FocusFar1Button_Click(object sender, EventArgs e)
        {
            try { CameraHandler.SetFocus(EDSDK.EvfDriveLens_Far1); }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void FocusFar2Button_Click(object sender, EventArgs e)
        {
            try { CameraHandler.SetFocus(EDSDK.EvfDriveLens_Far2); }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void FocusFar3Button_Click(object sender, EventArgs e)
        {
            try { CameraHandler.SetFocus(EDSDK.EvfDriveLens_Far3); }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        #endregion

        #region Subroutines

        private void CloseSession()
        {
            CameraHandler.CloseSession();
            AvCoBox.Items.Clear();
            TvCoBox.Items.Clear();
            ISOCoBox.Items.Clear();
            SettingsGroupBox.Enabled = false;
            LiveViewGroupBox.Enabled = false;
            SessionButton.Text = "Open Session";
            SessionLabel.Text = "No open session";
            RefreshCamera();//Closing the session invalidates the current camera pointer
        }

        private void RefreshCamera()
        {
            CameraListBox.Items.Clear();
            CamList = CameraHandler.GetCameraList();
            foreach (Camera cam in CamList) CameraListBox.Items.Add(cam.Info.szDeviceDescription);
            if (CameraHandler.CameraSessionOpen) CameraListBox.SelectedIndex = CamList.FindIndex(t => t.Ref == CameraHandler.MainCamera.Ref);
            else if (CamList.Count > 0) CameraListBox.SelectedIndex = 0;
        }

        private void OpenSession()
        {
            if (CameraListBox.SelectedIndex >= 0)
            {
                CameraHandler.OpenSession(CamList[CameraListBox.SelectedIndex]);
                SessionButton.Text = "Close Session";
                string cameraname = CameraHandler.MainCamera.Info.szDeviceDescription;
                SessionLabel.Text = cameraname;
                if (CameraHandler.GetSetting(EDSDK.PropID_AEMode) != EDSDK.AEMode_Manual) MessageBox.Show("Camera is not in manual mode. Some features might not work!");
                AvList = CameraHandler.GetSettingsList((uint)EDSDK.PropID_Av);
                TvList = CameraHandler.GetSettingsList((uint)EDSDK.PropID_Tv);
                ISOList = CameraHandler.GetSettingsList((uint)EDSDK.PropID_ISOSpeed);
                foreach (int Av in AvList) AvCoBox.Items.Add(CameraValues.AV((uint)Av));
                foreach (int Tv in TvList) TvCoBox.Items.Add(CameraValues.TV((uint)Tv));
                foreach (int ISO in ISOList) ISOCoBox.Items.Add(CameraValues.ISO((uint)ISO));
                AvCoBox.SelectedIndex = AvCoBox.Items.IndexOf(CameraValues.AV((uint)CameraHandler.GetSetting((uint)EDSDK.PropID_Av)));
                TvCoBox.SelectedIndex = TvCoBox.Items.IndexOf(CameraValues.TV((uint)CameraHandler.GetSetting((uint)EDSDK.PropID_Tv)));
                ISOCoBox.SelectedIndex = ISOCoBox.Items.IndexOf(CameraValues.ISO((uint)CameraHandler.GetSetting((uint)EDSDK.PropID_ISOSpeed)));
                int wbidx = (int)CameraHandler.GetSetting((uint)EDSDK.PropID_WhiteBalance);
                switch (wbidx)
                {
                    case EDSDK.WhiteBalance_Auto: WBCoBox.SelectedIndex = 0; break;
                    case EDSDK.WhiteBalance_Daylight: WBCoBox.SelectedIndex = 1; break;
                    case EDSDK.WhiteBalance_Cloudy: WBCoBox.SelectedIndex = 2; break;
                    case EDSDK.WhiteBalance_Tangsten: WBCoBox.SelectedIndex = 3; break;
                    case EDSDK.WhiteBalance_Fluorescent: WBCoBox.SelectedIndex = 4; break;
                    case EDSDK.WhiteBalance_Strobe: WBCoBox.SelectedIndex = 5; break;
                    case EDSDK.WhiteBalance_WhitePaper: WBCoBox.SelectedIndex = 6; break;
                    case EDSDK.WhiteBalance_Shade: WBCoBox.SelectedIndex = 7; break;
                    default: WBCoBox.SelectedIndex = -1; break;
                }
                SettingsGroupBox.Enabled = true;
                LiveViewGroupBox.Enabled = true;
            }
        }

        private void ReportError(string message, bool lockdown)
        {
            int errc;
            lock (ErrLock) { errc = ++ErrCount; }

            if (lockdown) EnableUI(false);

            if (errc < 4) MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (errc == 4) MessageBox.Show("Many errors happened!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            lock (ErrLock) { ErrCount--; }
        }

        private void EnableUI(bool enable)
        {
            if (InvokeRequired) Invoke((Action)delegate { EnableUI(enable); });
            else
            {
                SettingsGroupBox.Enabled = enable;
                InitGroupBox.Enabled = enable;
                LiveViewGroupBox.Enabled = enable;
            }
        }

        #endregion

        private void takeSeriesPhotoButton_Click(object sender, EventArgs e)
        {
            TakeSeriesPhoto();
        }

        private void initSerialPortButton_Click(object sender, EventArgs e)
        {
            InitPhaseShiftDeviceController();
            initSerialPortButton.Enabled = false;
            closephaseShiftSerialPortButton.Enabled = true;
        }
        
        private void InitPhaseShiftDeviceController()
        {
            if (phaseShiftSerialPortComboBox != null && phaseShiftSerialPortComboBox.SelectedItem != null)
            {
                string serialPort = phaseShiftSerialPortComboBox.SelectedItem.ToString();

                phaseShiftDeviceController = new PhaseShiftDeviceController(serialPort);
                phaseShiftDeviceController.Initialize();
            }
        }
        
        private void DisposePhaseShiftDeviceController()
        {
            if (phaseShiftDeviceController != null)
            {
                phaseShiftDeviceController.Dispose();
                phaseShiftDeviceController = null;
            }
        }
        
        private void TakeSeriesPhoto()
        {
            InitPhaseShiftParameters();
            takeNextPhoto = true;
            ExecutePhaseShiftAndTakePhoto();
        }

        private void ExecutePhaseShiftAndTakePhoto()
        {
            ExecutePhaseShift();
            TakePhoto();
        }

        private void ExecutePhaseShift()
        {
            if (phaseShiftDeviceController != null)
            {
                phaseShiftDeviceController.SetShift(currentPhaseShiftValue);
                //currentPhaseShiftLabel.Text = currentPhaseShiftValue.ToString();
            }

            MakeDelay();
        }

        private void TakePhoto()
        {
            CameraHandler.TakePhoto();
        }

        private void ClosePhaseShiftSerialPort()
        {
            DisposePhaseShiftDeviceController();
            initSerialPortButton.Enabled = true;
            closephaseShiftSerialPortButton.Enabled = false;
        }
        
        private void closephaseShiftSerialPortButton_Click(object sender, EventArgs e)
        {
            ClosePhaseShiftSerialPort();    
        }

        private void MakePhaseShiftsButton_Click(object sender, EventArgs e)
        {
            MakePhaseShifts();
        }
    
        private void MakePhaseShifts()
        {
            isMakePhaseShiftsProcess = true;
            InitPhaseShiftParameters();
            executeNextShift = true;
            ExecutePhaseShift();
        }
 
        private void MakeDelay()
        {
            if (delayPhaseShift > 0)
            {
                Thread.Sleep(delayPhaseShift);
            }
        }
        
        private void InitPhaseShiftParameters()
        {
            currentPhaseShiftNumber = 1;
            currentPhaseShiftValue = zeroPhaseShiftValue;

            phaseShiftStep = short.Parse(phaseShiftStepTextBox.Text, NumberStyles.HexNumber);
            phaseShiftCount = short.Parse(phaseShiftCountTextBox.Text);
            delayPhaseShift = int.Parse(DelayPhaseShiftTextBox.Text);
        }

        private void CameraForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (phaseShiftDeviceController != null)
            {
                phaseShiftDeviceController.Dispose();
            }
        }
    }
}
