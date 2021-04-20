using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoLibrary;
using MediaToolkit;

namespace YoutubeDownloader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
         
        private void Form1_Load(object sender, EventArgs e)
        {
            progressBar1.Visible = false;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Visible = false;
            using (FolderBrowserDialog fbd = new FolderBrowserDialog() { Description = "Set download location" })
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    progressBar1.Visible = true;
                    label5.Text = "Searching...";
                    try
                    {
                        var yt = YouTube.Default;
                        var video = await yt.GetVideoAsync(link.Text);

                        string name = "Downloading : " + video.FullName;
                        label5.Text = name;

                        File.WriteAllBytes(fbd.SelectedPath + @"\" + video.FullName, await video.GetBytesAsync());

                        var inputFile = new MediaToolkit.Model.MediaFile { Filename = fbd.SelectedPath + @"\" + video.FullName };
                        var outputFile = new MediaToolkit.Model.MediaFile { Filename = $"{fbd.SelectedPath + @"\" + video.FullName }.mp3" };

                        using (var enging = new Engine())
                        {
                            enging.GetMetadata(inputFile);
                            enging.Convert(inputFile, outputFile);
                        }

                        MessageBox.Show("Download succesfuly completed", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        progressBar1.Visible = false;
                        label5.Text = "";
                    }
                    catch 
                    { 
                        MessageBox.Show("Invalid link", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        progressBar1.Visible = false;
                        label5.Text = "";
                    }                                      
                }
                else
                {
                    MessageBox.Show("Select a folder location", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }
    }
}
