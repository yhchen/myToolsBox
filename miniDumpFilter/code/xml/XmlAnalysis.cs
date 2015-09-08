using System;
using System.Collections.Generic;
using System.Text;

namespace miniDumpFilter.xml
{
    public class SortResult : IComparable
    {
        public string strHash;
        public List<string> fileList;  //文件列表
        public int nValue; //排序用
        public int CompareTo(object obj)
        {
            SortResult other = obj as SortResult;
            if (nValue > other.nValue)
                return -1;
            else if (nValue < other.nValue)
                return 1;
            return strHash.CompareTo(other.strHash);
        }
    }

    class XmlAnalysis
    {
        /// <summary>
        /// Xml Data List
        /// </summary>
        public static Dictionary<string, XmlDataLoader> m_lstXmlData;

        public static XmlDataLoader addXmlFileData(string strFileName)
        {
            // 初始化
            if (null == m_lstXmlData)
            {
                m_lstXmlData = new Dictionary<string, XmlDataLoader>();
            }

            //已包含文件
            if (m_lstXmlData.ContainsKey(strFileName))
                return null;
            try
            {
                XmlDataLoader loader = new XmlDataLoader();
                bool boResult = loader.loadFromFile(strFileName);
                if (boResult)
                {
                    m_lstXmlData.Add(strFileName, loader);
                    return loader;
                }
                return null;
            }
            catch { }
            return null;
        }

        public static List<SortResult> DoAnalysis(int deepLevel)
        {
            Dictionary<string, List<string>> dicGather = new Dictionary<string,List<string>>();
            foreach (var xmlData in m_lstXmlData)
            {
                string strKey = xmlData.Key;
                XmlDataLoader dataValue = xmlData.Value;
                if (null == dataValue)
                    continue;
                string strAddressHash = dataValue.getAddressHash(deepLevel);
                if (dicGather.ContainsKey(strAddressHash))
                {
                    dicGather[strAddressHash].Add(dataValue.m_strFilePath);
                }
                else
                {
                    List<string> lstString = new List<string>();
                    lstString.Add(dataValue.m_strFilePath);
                    dicGather.Add(strAddressHash, lstString);
                }
            }

            List<SortResult> result = new List<SortResult>();
            foreach (var sameGrp in dicGather)
            {
                SortResult res = new SortResult();
                res.nValue = sameGrp.Value.Count;
                res.strHash = sameGrp.Key;
                res.fileList = sameGrp.Value;
                result.Add(res);
            }
            result.Sort();
            return result;
        }
    }
}
