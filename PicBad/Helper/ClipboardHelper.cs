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
                if (line !=null)
                ClipboardText += "\r\n" + Regex.Replace(line, "\\?Fname=[\\w\\W]{1,}", "");

            }

            return ClipboardText;
        }
        //![输入图片说明](https://gitee.com/uploads/images/2018/0122/210205_86dc40ef_370812.jpeg "TIM截图20180122170840.jpg")
        public static String MarkdownText(String[] ClipboardLine)
        {
            String ClipboardText = "";
            foreach (String line in ClipboardLine)
            {
                if (line != null)
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
        }
}
