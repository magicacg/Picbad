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
        WebClient webClient = new WebClient();
        private async Task UploadImgAsync()
        {
            List<String> FileList = new List<string>();
            FileList = FileHelper.SelectFiles();
            FileCount = FileList.Count;
            if (FileList.Count <= 50)
            {
                foreach (String line in FileList)
                {
                    //CATUP(line);
                  
                    Console.WriteLine(line);
              
                    string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
                  //  webClient.Headers.Add("Host", "sm.ms");
                  
                    webClient.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:57.0) Gecko/20100101 Firefox/57.0");
                    
                    webClient.Headers.Add("Content-Type", "multipart/form-data; boundary="+boundary);
                    String Header = "--"+ boundary + "\r\n" + "Content-Disposition: form-data; name=\"smfile\"; filename=\"" + Path.GetFileName(line) + "\"\r\nContent-Type: application/octet-stream\r\n\r\n";


                    byte[] HeaderByte = Encoding.UTF8.GetBytes(Header);
                    byte[] File = StreamToBytes(System.IO.File.OpenRead(line)) ;
                    byte[] EndByte = Encoding.UTF8.GetBytes("\r\n--" +boundary+"--");
                
                    List<Byte[]> listcat = new List<byte[]>();
                    listcat.Add(HeaderByte);
                    listcat.Add(File);
                    listcat.Add(EndByte);
                    byte[] bytes = COPYBIT(listcat);
                    Console.WriteLine(bytes.Length);
                    Byte[] responseArray = webClient.UploadData("https://sm.ms/api/upload", "POST", bytes);
                    Console.WriteLine(Encoding.UTF8.GetString(responseArray));
                 
                    // await UploadTourou(line);

                }
                if (FileCompletedCount < FileCount) {
                   
                  //  MessageBox.Show(String.Format("有{0}个上传失败", FileCount - FileCompletedCount));
                }
            }
            else
            {
                MessageBox.Show("每次最多上传50张!(太多图片很可能被封IP哦！请妥善使用！)");
            }
            InitDownLoadView();
        }

        private byte[] COPYBIT(List<byte[]> listcat)
        {
           int length = 0;
            int readLength = 0;
          
           
            foreach (byte[] b in listcat)
            {
                Console.WriteLine("b:"+b.Length);
                length += b.Length;
            }

            byte[] bytes = new byte[length];

            foreach (byte[] b in listcat)
            {
                b.CopyTo(bytes, readLength);
                readLength += b.Length;
            }

            return bytes;
        }

        private static void CATUP(string line)
        {
            var httpUpload = new HttpUpload();


            FileStream fspdf = new FileStream(line, FileMode.Open);
            byte[] fileBytepdf = new byte[fspdf.Length];
            fspdf.Read(fileBytepdf, 0, fileBytepdf.Length);
            fspdf.Close();
            var pdfName = line.Substring(line.LastIndexOf("\\") + 1);
            httpUpload.SetFieldValue("smfile", pdfName, "application/octet-stream", fileBytepdf);
            string responStr = "";
            bool responseArray = httpUpload.Upload("https://sm.ms/api/upload", out responStr);
            Console.WriteLine(responseArray);
        }

        /// 将 Stream 转成 byte[]

        public byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }
        private byte[] CopyToBig(byte[] bBig, byte[] bSmall)
        {
            byte[] tmp = new byte[bBig.Length + bSmall.Length];
            System.Buffer.BlockCopy(bBig, 0, tmp, 0, bBig.Length);
            System.Buffer.BlockCopy(bSmall, 0, tmp, bBig.Length, bSmall.Length);
            return tmp;
        }

        private async Task UploadTourou(string line)
        {
            String Json = await UploadToTourouAsync(line);
            richTextBox1.AppendText("https://wx1.sinaimg.cn/large/" + Json + ".jpg?Fname=" + Path.GetFileNameWithoutExtension(line) + "\r\n");
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
            FileCompletedCount = 0;
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

        private void button2_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(ClipboardHelper.HtmlText(richTextBox1.Lines,textBox1.Text,textBox2.Text,checkBox1.Checked,checkBox2.Checked), true);

        }

        private void button6_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }
    }
}
