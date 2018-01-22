using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PicBed.Helper
{
    class ClipboardHelper
    {/// <summary>
    /// 获取纯净的文本信息给剪贴板使用。
    /// </summary>
    /// <param name="ClipboardLine"></param>
    /// <returns></returns>
        public static String PureText(String[] ClipboardLine) {
            String ClipboardText = "";
            foreach(String line in ClipboardLine)
            {
                if (line !="")
                ClipboardText += "\r\n" + Regex.Replace(line, "\\?Fname=[\\w\\W]{1,}", "");

            }

            return ClipboardText;
        }
        /// <summary>
        /// 处理文本到MarkdownText
        /// </summary>
        /// <param name="ClipboardLine"></param>
        /// <returns></returns>
        public static String MarkdownText(String[] ClipboardLine)
        {
            String ClipboardText = "";
            foreach (String line in ClipboardLine)
            {
                if (line != "")
                {
                    Console.WriteLine(line);
                    String[] FileInfo = Regex.Split(line, "\\?Fname=");
                    if (FileInfo.Count() == 2)
                    {
                        Console.WriteLine(FileInfo[0]);
                        Console.WriteLine(FileInfo[1]);
                        ClipboardText += "\r\n" + String.Format("![{0}]({1} \"{2}\")", FileInfo[1], FileInfo[0], FileInfo[1]);
                    }
                    else {
                        ClipboardText += "\r\n" + String.Format("![]({0} \"\")",line);

                    }
                }

            }
            return ClipboardText;
        }
        /// <summary>
        /// 处理文本到HTML
        /// </summary>
        /// <param name="ClipboardLine"></param>
        /// <returns></returns>

        public static String HtmlText(String[] ClipboardLine,String ImgClass="",String ImgId="", bool JumpToImg=false ,bool Lightbox=false)
        {
            String ClipboardText = "";
            foreach (String line in ClipboardLine)
            {
                String TempString = "";
                String ImgUrl = line;
                if (line != "")
                {
                    Console.WriteLine(line);
                    String[] FileInfo = Regex.Split(ImgUrl, "\\?Fname=");
                    if (FileInfo.Count() == 2)
                    {
                        ImgUrl = FileInfo[0];
                        Console.WriteLine(FileInfo[0]);
                        Console.WriteLine(FileInfo[1]);
                        TempString = "\r\n" + String.Format("<img src=\"{0}\" alt=\"{1}\" class=\"{2}\" id=\"{3}\">", FileInfo[0], FileInfo[1],ImgClass,ImgId );
                    }
                    else
                    {
                        TempString = "\r\n" + String.Format("<img src=\"{0}\" class=\"{1}\" id=\"{2}\">", ImgUrl, ImgClass, ImgId);
                    }
                    if (JumpToImg) {
                        TempString = String.Format("<a  href=\"{0}\">{1}</a>", ImgUrl,TempString);
                      
                    }
                    if (Lightbox) {
                        TempString = TempString.Replace("<a  href=", "<a class=\"example-image-link\" data-lightbox=\"image-1\" data-title=\"\"  href=\"");
                    }
                }
                ClipboardText += TempString;
            }
            return ClipboardText;
        }
    }
}
