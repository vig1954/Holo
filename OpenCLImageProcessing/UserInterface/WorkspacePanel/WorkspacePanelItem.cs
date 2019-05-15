using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Common;
using Infrastructure;
using UserInterface.Events;

namespace UserInterface.WorkspacePanel
{
    public partial class WorkspacePanelItem : UserControl
    {
        private Color RegularBackground = SystemColors.Control;
        private Color SelectedBackground = SystemColors.Highlight;

        public event Action OnOpenSettingsClicked;
        public event Action OnShowInEditorClicked;
        
        public bool  IsOpenSettingsButtonVisible
        {
            get => OpenSettings.Visible;
            set => OpenSettings.Visible = value;
        }

        public bool Selected { get; private set; }

        public bool IsShowInEditorButtonVisible
        {
            get => ShowInEditor.Visible;
            set => ShowInEditor.Visible = value;
        }

        public bool IsShowInEditorButtonEnabled
        {
            get => ShowInEditor.Enabled;
            set => ShowInEditor.Enabled = value;
        }

        public event Action<string> TitleChanged;

        public string Title => TitleLabel.Text;

        public WorkspacePanelItem()
        {
            InitializeComponent();
        }

        public void SetTitle(string title, Font font = null, Color? foreColor = null)
        {
            TitleLabel.Text = title;

            if (font != null)
                TitleLabel.Font = font;

            if (foreColor.HasValue)
                TitleLabel.ForeColor = foreColor.Value;
        }

        public void SetInfo(string info)
        {
            IconToolTip.SetToolTip(IconPictureBox, info);
            IconToolTip.ToolTipTitle = Title;
        }

        public void SetSelectionState(bool selected)
        {
            BackColor = selected ? SelectedBackground : RegularBackground;
            Selected = selected;
        }

        public void SetImage(Image image)
        {
            IconPictureBox.Image = image;
            IconPictureBox.Refresh();
        }

        private void IconPictureBox_DoubleClick(object sender, EventArgs e)
        {
            OnDoubleClick(e);
        }

        private void IconPictureBox_Click(object sender, EventArgs e)
        {
            OnClick(e);
        }

        private void TitleLabel_DoubleClick(object sender, EventArgs e)
        {
            TitleLabel.Hide();
            ChangeNameTextBox.Text = TitleLabel.Text;
            ChangeNameTextBox.Show();
            ChangeNameTextBox.Focus();
            ChangeNameTextBox.SelectAll();
        }

        private void ChangeNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !ChangeNameTextBox.Text.Trim().IsNullOrEmpty())
            {
                TitleLabel.Text = ChangeNameTextBox.Text;
                TitleChanged?.Invoke(TitleLabel.Text);
            }

            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
            {
                ChangeNameTextBox.Hide();
                TitleLabel.Show();
            }
        }

        private void OpenSettings_Click(object sender, EventArgs e)
        {
            OnOpenSettingsClicked?.Invoke();
        }

        private void ShowInEditor_Click(object sender, EventArgs e)
        {
            OnShowInEditorClicked?.Invoke();
        }
    }
}
