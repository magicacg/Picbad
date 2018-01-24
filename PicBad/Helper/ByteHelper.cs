using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicBed.Helper
{
    class ByteHelper
    {



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
