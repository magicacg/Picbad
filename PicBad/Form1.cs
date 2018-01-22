using PicBed.Helper;
using PicBed.PicWebHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicBad
{
    public partial class PicBedFrom : Form
    {
        public PicBedFrom()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UploadImgAsync();
        }
       
        private async Task UploadImgAsync()
        {
            List<String> FileList = new List<string>();
            FileList = FileHelper.SelectFiles();
            if (FileList.Count <= 50)
                foreach (String line in FileList)
                {
                 

                    String Json = await UploadToTourouAsync(line);
                    richTextBox1.AppendText("\r\nhttps://wx1.sinaimg.cn/large/" + Json + ".jpg?Fname=" + Path.GetFileNameWithoutExtension(line));

                }
            else {
                MessageBox.Show("每次最多上传50张!(太多图片很可能被封IP哦！请妥善使用！)");
            }
            InitDownLoadView();
        }

        private async Task<string> UploadToTourouAsync(string line)
        {/// await Tourou.UploadFileAsync(line)
            String Json = "";
            int count = 0;
            while (true)
            {
                if (count++ < 3)
                {
                   
                    Json = await Tourou.UploadFileAsync(line);
                    if (Json != "") break;
                    else
                        Console.WriteLine("文件{0}上传失败，重试{2}次。", Path.GetFileNameWithoutExtension(line), count);
                }
                else { Console.WriteLine("反复上传失败，直接放弃！"); break; }
            }
            return Json;
        }

        private void InitDownLoadView()
        {
            progressBar1.Value = 0;
        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText); // call default browser  
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(richTextBox1.Text, true);
        }
    }
}
