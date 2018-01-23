﻿using Newtonsoft.Json;
using PicBed.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PicBed.PicWebHelper
{/// <summary>
/// 辅助上传到iloli图床的工具
/// </summary>
    class ILoli
    {
        /// <summary>
        /// webclient，CookieAwareWebClient为自己实现能保持cookies的重写，可直接改成webclient
        /// </summary>
        private static    WebClient webClient = new CookieAwareWebClient();
        /// <summary>
        /// 上传文件并且使用自定义字段。注意，boundary在请求体中和header中差了“--”两个横线，如果不对应一定会上传失败！
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static async Task<string> UploadImgAsync(String line,UploadProgressChangedEventHandler uploadProgressChanged) {

            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            webClient.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:57.0) Gecko/20100101 Firefox/57.0");
            webClient.Headers.Add("Content-Type", "multipart/form-data; boundary=" + boundary);


            webClient.UploadProgressChanged += uploadProgressChanged;

            String Header = "--" + boundary + "\r\n" + "Content-Disposition: form-data; name=\"smfile\"; filename=\"" + Path.GetFileName(line) + "\"\r\nContent-Type: application/octet-stream\r\n\r\n";
            byte[] HeaderByte = Encoding.UTF8.GetBytes(Header);
            byte[] File = StreamToBytes(System.IO.File.OpenRead(line));//读取文件
            byte[] EndByte = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--");

            List<Byte[]> listcat = new List<byte[]>();
            listcat.Add(HeaderByte);
            listcat.Add(File);
            /*如需其他自定义字段，请自行定义然后添加到listcat中，下面的copybit会自动合并所有数据*/
            listcat.Add(EndByte);
            byte[] bytes = COPYBIT(listcat);




            String ImgUrl = "";
            for (int i = 0; i < 3; i++) {
                try
                {
             byte[] responseArray=  await webClient.UploadDataTaskAsync(new Uri("https://sm.ms/api/upload"),"POST", bytes);
              
                    String Shtml = Encoding.UTF8.GetString(responseArray);
                   ILoliJson.Root AllJson = JsonConvert.DeserializeObject<ILoliJson.Root>(Shtml);
                    ImgUrl = AllJson.data.url;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                if (ImgUrl != "")
                    break;
                else {
                    Console.WriteLine("上传{0}未成功，第{1}次重试",line,i);
                }
            }
         
            return ImgUrl;
        }


        /// 将 Stream 转成 byte[]

        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }
        /// <summary>
        /// 处理bit数据进行合并
        /// </summary>
        /// <param name="listcat"></param>
        /// <returns></returns>
        private static byte[] COPYBIT(List<byte[]> listcat)
        {
            int length = 0;
            int readLength = 0;

            //将List里的数组相加，累加起来看到底多少个。
            foreach (byte[] b in listcat)
            {
               // Console.WriteLine("当前数据大小:" + b.Length);
                length += b.Length;
            }
            //根据大小建立对应的数组
            byte[] bytes = new byte[length];
            //依次拷贝数据
            foreach (byte[] b in listcat)
            {
                b.CopyTo(bytes, readLength);
                readLength += b.Length;
            }

            return bytes;
        }
    }


    class ILoliJson {
        public class Data
        {
            /// <summary>
            /// 
            /// </summary>
            public int width { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int height { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string filename { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string storename { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int size { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string path { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string hash { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int timestamp { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ip { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string delete { get; set; }
        }

        public class Root
        {
            /// <summary>
            /// 
            /// </summary>
            public string code { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Data data { get; set; }
        }
    }
}
