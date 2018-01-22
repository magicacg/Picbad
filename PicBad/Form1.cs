using PicBed.Helper;
using PicBed.PicWebHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicBad
{
    public partial class PicBedFrom : Form
    {
        public PicBedFrom()
        {
            InitializeComponent();
        }/// <summary>
        /// 用于记录缓存用户选了多少文件
        /// </summary>
        int FileCount = 0;
        /// <summary>
        /// 用于记录上传成功多少个
        /// </summary>
        int FileCompletedCount = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            UploadImgAsync();
        }
       
        private async Task UploadImgAsync()
        {
            List<String> FileList = new List<string>();
            FileList = FileHelper.SelectFiles();
            FileCount = FileList.Count;
            if (FileList.Count <= 50)
            {
                foreach (String line in FileList)
                {
                    String Json = await UploadToTourouAsync(line);
                    richTextBox1.AppendText("https://wx1.sinaimg.cn/large/" + Json + ".jpg?Fname=" + Path.GetFileNameWithoutExtension(line)+"\r\n");

                }
                if (FileCompletedCount < FileCount) {
                    Console.WriteLine();
                    MessageBox.Show(String.Format("有{0}个上传失败", FileCount - FileCompletedCount));
                }
            }
            else
            {
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
                   
                    Json = await Tourou.UploadFileAsync(line,uploadFileCompleted,uploadProgress);
                    if (Json != "") break;
                    else
                        Console.WriteLine("文件{0}上传失败，重试{2}次。", Path.GetFileNameWithoutExtension(line), count);
                }
                else { Console.WriteLine("反复上传失败，直接放弃！"); break; }
            }
            return Json;
        }

        private void uploadProgress(object sender, UploadProgressChangedEventArgs e)
        {
            progressBar2.Value = e.ProgressPercentage;
            Console.WriteLine(e.ProgressPercentage);
        }

        private void uploadFileCompleted(object sender, UploadFileCompletedEventArgs e)
        {
            FileCompletedCount++;
            progressBar1.Value = (int)((float)FileCompletedCount / FileCount * 100);
            Console.WriteLine("一个完成");
        }

        private void InitDownLoadView()
        {
            FileCount = 0;
            progressBar1.Value = 0;
            progressBar2.Value = 0;
        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText); // call default browser  
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(ClipboardHelper.PureText(richTextBox1.Lines), true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            String[] FileInfo = Regex.Split("https://wx1.sinaimg.cn/large/a15b4afegy1fnpoyjdm9wj20u01900xz.jpg?Fname=donshofertumblr_p1svxfcr3H1rjk2kao2_1280", "\\?Fname=");
            foreach (String line in FileInfo) {

                Console.WriteLine(line);
            }
            Console.WriteLine(ClipboardHelper.MarkdownText(new string[] { "https://wx1.sinaimg.cn/large/a15b4afegy1fnpoyjdm9wj20u01900xz.jpg?Fname=donshofertumblr_p1svxfcr3H1rjk2kao2_1280" }));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(ClipboardHelper.MarkdownText(richTextBox1.Lines), true);

        }
    }
}
