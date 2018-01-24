using Newtonsoft.Json;
using PicBed.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PicBed.PicWebHelper
{
    class Tourou
    {


        /// <summary>
        /// 上传图片到偷揉图库，暂定是这里。
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static async Task<string> UploadFileAsync(String FileName, UploadFileCompletedEventHandler uploadFileCompleted = null, UploadProgressChangedEventHandler uploadProgress = null)
        {
            String Html = "";
            CookieAwareWebClient ImgUpLoad = new CookieAwareWebClient();
            ImgUpLoad.Headers.Add("Referer", "x.mouto.org");
            ImgUpLoad.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:57.0) Gecko/20100101 Firefox/57.0");

            try
            {
                if (uploadFileCompleted != null)
                    ImgUpLoad.UploadFileCompleted += uploadFileCompleted;
                if(uploadProgress!=null)
                ImgUpLoad.UploadProgressChanged += uploadProgress;


                byte[] responseArray = await ImgUpLoad.UploadFileTaskAsync(new Uri("https://x.mouto.org/wb/x.php?up&_r="+new Random().NextDouble()), FileName);

        

                String JsonText = Encoding.UTF8.GetString(responseArray);

                Root AllJson = JsonConvert.DeserializeObject<Root>(JsonText);
                Html = AllJson.pid;
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
            }
            return Html;
        }
    }

    public class Root
    {
        /// <summary>
        /// a15b4afegy1fnpl5w0prhj20m80xcgq2
        /// </summary>
        public string pid { get; set; }
        /// <summary>
        /// jpeg
        /// </summary>
        public string type { get; set; }
    }
}
