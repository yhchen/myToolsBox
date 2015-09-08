using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace miniDumpFilter.xml
{
    class XmlDataLoader
    {
        public XmlDataLoader()
        {
        }

        public bool loadFromFile(string strFilePath)
        {
            try
            {
                m_XmlDocument = new XmlDocument();
                m_XmlDocument.Load(strFilePath);
            }
            catch
            {
                return false;
            }
            m_strFilePath = strFilePath;
            m_strPlatform = m_XmlDocument.SelectSingleNode("/report/platform").InnerText;
            m_strApplication = m_XmlDocument.SelectSingleNode("/report/application").InnerText;
            m_strComputer = m_XmlDocument.SelectSingleNode("/report/computer").InnerText;
            m_strIp = m_XmlDocument.SelectSingleNode("/report/ips/ip").InnerText;
            m_strUser = m_XmlDocument.SelectSingleNode("/report/user").InnerText;
            m_strTimestamp = m_XmlDocument.SelectSingleNode("/report/timestamp").InnerText;
            m_llTimeStamp = Convert.ToInt64(m_strTimestamp);
            // FIXME : 无法获取系统时间
            //m_strTimeFormat = new DateTime(m_llTimeStamp / 10).ToString();

            m_strWhat = m_XmlDocument.SelectSingleNode("/report/error/what").InnerText;
            m_strProcessName = m_XmlDocument.SelectSingleNode("/report/error/process/name").InnerText;
            m_strProcessId = m_XmlDocument.SelectSingleNode("/report/error/process/id").InnerText;
            m_strModule = m_XmlDocument.SelectSingleNode("/report/error/module").InnerText;
            m_strAddress = m_XmlDocument.SelectSingleNode("/report/error/address").InnerText;

            XmlNodeList stackDataList = m_XmlDocument.SelectNodes("/report/threads");
            if (null == stackDataList)
                return false;
            m_lstThreas = new List<ThreadData>();
            foreach(XmlNode n in stackDataList)
            {
                if (null == n)
                    continue;
                ThreadData sdList = ThreadDataLoader.LoadFromXml(n);
                if (null == sdList)
                    return false;
                m_lstThreas.Add(sdList);
            }
            m_strIp = m_XmlDocument.SelectSingleNode("/report/ips").InnerText;
            XmlNodeList moduleDataList = m_XmlDocument.SelectNodes("/report/processes/process");
            if (null == moduleDataList)
                return false;
            m_lstProcesses = new Dictionary<string, ProcessData>();
            foreach(XmlNode n in moduleDataList)
            {
                if (null == n)
                    continue;
                ProcessData mdList = ProcesssDataLoader.LoadFromXml(n);
                if (null == mdList)
                    return false;
                m_lstProcesses.Add(mdList.m_strProcessName, mdList);
            }
            return true;
        }

        //
        public string m_strFilePath;
        public string m_strPlatform;
        public string m_strApplication;
        public string m_strComputer;
        public string m_strIp;
        public string m_strUser;
        public string m_strTimestamp;
        public long m_llTimeStamp;
        /// <summary>
        /// error
        /// </summary>
        public string m_strWhat;
        public string m_strProcessName;
        public string m_strProcessId;
        public string m_strModule;
        public string m_strAddress;
        /// <summary>
        /// module
        /// </summary>
        Dictionary<string, ProcessData> m_lstProcesses;
        /// <summary>
        /// stack
        /// </summary>
        List<ThreadData> m_lstThreas;
        ////////////////////////////////////////////////////////////////////////////////
        ///xml
        /// Points to document root.
        protected XmlDocument m_XmlDocument;
        ////////////////////////////////////////////////////////////////////////////////
        ///get data file
        protected string m_strAddressHashCode;
        public string getAddressHash(int deepLevel)
        {
            if (null != m_strAddressHashCode && 0 != m_strAddressHashCode.Length)
            {
                return m_strAddressHashCode;
            }
            m_strAddressHashCode = "";
            ThreadData threadData = null;
            foreach(ThreadData td in m_lstThreas)
            {
                if (null == td)
                    continue;
                if (td.m_strThreadStatus.Equals("interrupted"))
                {
                    threadData = td;
                }
            }
            if (null == threadData)
                return m_strAddressHashCode;
            ProcessData processData = m_lstProcesses[m_strProcessName];
            if (null == processData)
                return m_strAddressHashCode;
            int currDeep = 0;
            foreach (StackData sd in threadData.m_lstStackData)
            {
                if (deepLevel > 0 && deepLevel <= currDeep)
                    break;
                if (null == sd)
                    continue;
                if (!sd.m_strModule.Contains(m_strProcessName))
                    continue;
                // FIXME : 有可能
//                 if (!m_strModule.Equals(sd.m_strModule))
//                     continue;
                m_strAddressHashCode += sd.m_nAddrAddress - processData.getModuleBaseByModuleName(sd.m_strModule);
                ++currDeep;
            }
            return m_strAddressHashCode;
        }
    }

}
