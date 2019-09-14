using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace rab1.Forms
{
    public partial class FileMaker : Form
    {
        public FileMaker()
        {
            InitializeComponent();
        }

        private void SetDefaultSettings()
        {
            int startRowNumber = 50;
            int endRowNumber = 50;

            this.textBoxStartRowNumber.Text = startRowNumber.ToString();
            this.textBoxEndRowNumber.Text = endRowNumber.ToString();
        }

        private void MakeFile()
        {
            int filesCount = 8;

            int width;
            int height;

            int startRowNumber = int.Parse(this.textBoxStartRowNumber.Text);
            int endRowNumber = int.Parse(this.textBoxEndRowNumber.Text);

            int startRowIndex = GetRowIndex(startRowNumber);
            int endRowIndex = GetRowIndex(endRowNumber);

            int rowsCountPerFile = endRowNumber - startRowNumber + 1;
            height = rowsCountPerFile * filesCount * 2;

            string directory1Path = this.textBoxDirectory1.Text;
            string directory2Path = this.textBoxDirectory2.Text;

            string[] filePaths1 = Directory.GetFiles(directory1Path);
            string[] filePaths2 = Directory.GetFiles(directory2Path);

            IList<string> fileList1 = filePaths1.OrderBy(x => x).Take(filesCount).ToList();
            IList<string> fileList2 = filePaths2.OrderBy(x => x).Take(filesCount).ToList();

            string firstFile = fileList1.FirstOrDefault();

            Bitmap bitmap = new Bitmap(firstFile);
            width = bitmap.Width;

            Bitmap resBitmap = new Bitmap(width, height);

            int row = 0;

            for (int k = 0; k < fileList1.Count; k++)
            {
                string filePath = fileList1[k];
                Bitmap img = new Bitmap(filePath);
                for (int y = startRowIndex; y <= endRowIndex; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        Color pixel = img.GetPixel(x, y);
                        resBitmap.SetPixel(x, row, pixel);
                    }
                    row++;
                }
            }

            for (int k = 0; k < fileList2.Count; k++)
            {
                string filePath = fileList2[k];
                Bitmap img = new Bitmap(filePath);
                for (int y = startRowIndex; y <= endRowIndex; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        Color pixel = img.GetPixel(x, y);
                        resBitmap.SetPixel(x, row, pixel);
                    }
                    row++;
                }
            }

            string resFilePath = textBoxOutputFile.Text;

            resBitmap.Save(resFilePath);
        }


        private int GetRowIndex(int rowNumber)
        {
            return rowNumber - 1;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.MakeFile();
            this.Close();
        }

        private void buttonSelectDirectory1_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    textBoxDirectory1.Text = fbd.SelectedPath;
                }
            }
        }

        private void buttonSelectDirectory2_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    textBoxDirectory2.Text = fbd.SelectedPath;
                }
            }
        }

        private void buttonSelectOutputFile_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "(*.JPG)|*.JPG|(*.bmp)|*.bmp";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBoxOutputFile.Text = dialog.FileName;
            }
        }
    }
}
