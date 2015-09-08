using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SpaceKiller
{
    public static class TextFileUtil
    {
        public static Encoding GetFileEncodeType(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            BinaryReader br = new System.IO.BinaryReader(fs);
            Byte[] buffer = br.ReadBytes(2);
            br.Close();
            fs.Close();

            if (buffer[0] >= 0xEF)
            {
                if (buffer[0] == 0xEF && buffer[1] == 0xBB)
                {
                    return Encoding.UTF8;
                }
                else if (buffer[0] == 0xFE && buffer[1] == 0xFF)
                {
                    return Encoding.BigEndianUnicode;
                }
                else if (buffer[0] == 0xFF && buffer[1] == 0xFE)
                {
                    return Encoding.Unicode;
                }
                else
                {
                    return Encoding.Default;
                }
            }
            else
            {
                return Encoding.Default;
            }
        }
    }
}
