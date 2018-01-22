using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicBed.Helper
{
    class FileHelper
    {

        public static List<String> SelectFiles()
        {
            List<String> FileList = new List<string>();
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Multiselect = true;//该值确定是否可以选择多个文件
                dialog.Title = "请选择文件夹";
                dialog.Filter = "图片文件(*.jpg,*.gif,*.bmp;*.png;*.jpeg)|*.jpg;*.gif;*.bmp;*.png;*.jpeg";
                dialog.Multiselect = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (dialog.FileNames.Count() > 0)
                    {
                        FileList = dialog.FileNames.ToList<String>();
                    }
                }

            }
            return FileList;
        }
    }
}
