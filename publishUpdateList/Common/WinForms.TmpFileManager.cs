using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace publishUpdateList.Common
{
    public static class TmpFileManager
    {
		public static string createTmpFile(byte[] array = null)
        {
			try
            {
                string tempFile = Path.GetTempFileName();
                FileStream fs = File.OpenWrite(tempFile);
                if (array != null)
                {
                    fs.Write(array, 0, array.Length);
                }
                fs.Flush();
                fs.Close();

                // 加入到临时列表中
                m_tmpFileList.Add(tempFile);
                return tempFile;
            }
			catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return "";
            }
        }

		public static bool releaseTmpFile(string tmpFilePath)
        {
			try
            {
				foreach(string filePath in m_tmpFileList)
                {
                    if (filePath != tmpFilePath)
                        continue;
                    if (File.Exists(tmpFilePath))
                    {
                        File.Delete(tmpFilePath);
                    }
                    m_tmpFileList.Remove(filePath);
                    return true;
                }
                return false;
            }
			catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public static void freeAllObjects()
        {
            try
            {
                // 退出时销毁所有文件
                foreach (string filePath in m_tmpFileList)
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
                m_tmpFileList.Clear();
            }
            finally
            {
            }
        }

        private static List<string> m_tmpFileList = new List<string>();
    }
}
