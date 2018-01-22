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
    }
}
