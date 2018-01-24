using Newtonsoft.Json;
using PicBed.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PicBed.PicWebHelper
{
    class Pixhost
    {


        /// <summary>
        /// webclient，CookieAwareWebClient为自己实现能保持cookies的重写，可直接改成webclient
        /// </summary>
        private static WebClient webClient = new CookieAwareWebClient();
        /// <summary>
        /// 上传文件并且使用自定义字段。注意，boundary在请求体中和header中差了“--”两个横线，如果不对应一定会上传失败！
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static async Task<string> UploadImgAsync(String line, UploadProgressChangedEventHandler uploadProgressChanged)
        {

            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            webClient.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:57.0) Gecko/20100101 Firefox/57.0");
            webClient.Headers.Add("Content-Type", "multipart/form-data;charset=utf-8;boundary=" + boundary);
            //   webClient.Headers.Add("Referer", "https://pixhost.org/");
            webClient.Headers.Add("Accept", "application/json");

            webClient.UploadProgressChanged += uploadProgressChanged;


            byte[] HeaderByte = Encoding.UTF8.GetBytes(ByteHelper. CreateHeadInfo(boundary, "img", "") + string.Format(";filename=\"{0}\"\r\nContent-Type: image/jpeg\r\n\r\n", Path.GetFileName(line)));
            byte[] File = ByteHelper.StreamToBytes(System.IO.File.OpenRead(line));//读取文件
            byte[] EndByte = Encoding.UTF8.GetBytes("\r\n" +ByteHelper. CreateHeadInfo(boundary, "content_type", "\r\n\r\n0") + "\r\n--" + boundary + "--");

            List<Byte[]> listcat = new List<byte[]>();
            listcat.Add(HeaderByte);
            listcat.Add(File);
            /*如需其他自定义字段，请自行定义然后添加到listcat中，下面的copybit会自动合并所有数据*/
            listcat.Add(EndByte);
            byte[] bytes = ByteHelper.MergeByte(listcat);

       

            String ImgUrl = "";
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    byte[] responseArray = await webClient.UploadDataTaskAsync(new Uri("https://api.pixhost.org/images"), "POST", bytes);

                    String Shtml = Encoding.UTF8.GetString(responseArray);

                    PixHostJson.Root  AllJson = JsonConvert.DeserializeObject<PixHostJson.Root>(Shtml);

                    ImgUrl  = AllJson.th_url.Replace("/thumbs/", "/images/").Replace("https://t", "https://img");
                    Console.WriteLine(ImgUrl);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                if (ImgUrl != "")
                    break;
                else
                {
                    Console.WriteLine("上传{0}未成功，第{1}次重试", line, i);
                }
            }

            return ImgUrl;
        }
    }
    class PixHostJson {
        public class Root
        {
            /// <summary>
            /// 
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string show_url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string th_url { get; set; }
        }
    }
}
