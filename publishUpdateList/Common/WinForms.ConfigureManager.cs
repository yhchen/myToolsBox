using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace publishUpdateList.Common
{
    public static class ConfigureManager
    {

        // get & set update url
        public static string getUpdateUrl() { return XMLProcess.Read(m_strConfigFileName, m_strConfigKey_UpdateURL); }
        public static void setUpdateUrl(string val) { setValue(m_strConfigKey_UpdateURL, val); }
        // get & set last select file
        public static string getLastSelcFile() { return XMLProcess.Read(m_strConfigFileName, m_strConfigKey_LastSelecFile); }
        public static void setLastSelecFile(string val) { setValue(m_strConfigKey_LastSelecFile, val); }
        // get & set check url
        public static string getCheckUrl() { return XMLProcess.Read(m_strConfigFileName, m_strConfigKey_CheckUrl); }
        public static void setCheckUrl(string val) { setValue(m_strConfigKey_CheckUrl, val); }

        private static string getValue(string strKey)
        {

            return XMLProcess.Read(m_strConfigFileName, strKey + "[@Attribute='Name']");
        }
        private static void setValue(string strKey, string strValue)
        {
            try
            {
                XMLProcess.Update(m_strConfigFileName, strKey, "Name", strValue);
            }
            catch
            {
                int lastIndex = strKey.Length;
                while (lastIndex > 0)
                {
                    if (strKey[--lastIndex] == '/')
                        break;
                }
                XMLProcess.Insert(m_strConfigFileName, strKey, "", "Name", strValue);
            }
        }

        private static string m_strConfigFileName = AppDomain.CurrentDomain.BaseDirectory.ToString() + GlobalsConfig.configurationFileName;

        private static string m_strConfigKey_UpdateURL = "/root/Config/Setting/updateUrl";
        private static string m_strConfigKey_LastSelecFile = "/root/Config/Setting/lastSelecFile";
        private static string m_strConfigKey_CheckUrl = "/root/Config/Setting/checkUrl";
    }
}
