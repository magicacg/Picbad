using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicBed.Helper
{
    class ByteHelper
    {
        /// <summary>
        /// 处理bit数据进行合并
        /// </summary>
        /// <param name="listcat"></param>
        /// <returns></returns>
        public static byte[] MergeByte(List<byte[]> listcat)
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
}
