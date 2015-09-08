using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace miniDumpFilter.xml
{
    class ModuleDataLoader
    {
        /// <summary>
        /// load data from xml Node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        static public ModuleData LoadFromXml(XmlNode node)
        {
            if (null == node)
                return null;
            ModuleData mData = new ModuleData();
            mData.m_strModuleName = XmlLoaderBase.getInnerTextByPath(node, "./name");
            mData.m_strModuleVersion = XmlLoaderBase.getInnerTextByPath(node, "./version");
            mData.m_strModuleBase = XmlLoaderBase.getInnerTextByPath(node, "./base");
            if (null == mData.m_strModuleBase || 0 == mData.m_strModuleBase.Length)
                return null;
            mData.m_nModuleBase = Convert.ToInt32(mData.m_strModuleBase, 16);
            mData.m_strModuleSize = XmlLoaderBase.getInnerTextByPath(node, "./size");
            if (null == mData.m_strModuleSize || 0 == mData.m_strModuleSize.Length)
                return null;
            mData.m_nModuleSize = Convert.ToInt32(mData.m_strModuleSize, 16);
            return mData;
        }
    }

    class ProcesssDataLoader
    {
        /// <summary>
        /// load Module data from XmlNode
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        static public ProcessData LoadFromXml(XmlNode node)
        {
            if (null == node)
                return null;
            ProcessData pData = new ProcessData();
            pData.m_strProcessName = XmlLoaderBase.getInnerTextByPath(node, "./name");
            pData.m_strProcessId = XmlLoaderBase.getInnerTextByPath(node, "./id");
            if (null == pData.m_strProcessId || 0 == pData.m_strProcessId.Length)
                return null;
            pData.m_nProcessId = Convert.ToInt32(pData.m_strProcessId);

            XmlNodeList mList = node.SelectNodes("./modules/module");
            if (null == mList)
                return null;
            pData.m_lstModules = new Dictionary<string, ModuleData>();
            foreach(XmlNode n in mList)
            {
                if (null == n)
                    continue;
                ModuleData mData = ModuleDataLoader.LoadFromXml(n);
                if (null == mData)
                    return null;
                pData.m_lstModules.Add(mData.m_strModuleName, mData);
            }
            return pData;
        }
    }
}
