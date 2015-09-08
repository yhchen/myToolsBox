using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace miniDumpFilter.xml
{
    class StackDataLoader
    {
        static public StackData LoadFromXml(XmlNode node)
        {
            if (null == node)
                return null;
            StackData sData = new StackData();
            sData.m_strModule = XmlLoaderBase.getInnerTextByPath(node, "./frame/module");
            sData.m_strAddress = XmlLoaderBase.getInnerTextByPath(node, "./frame/address");
            sData.m_strFuncName = XmlLoaderBase.getInnerTextByPath(node, "./frame/function/name");
            sData.m_strFuncOffset = XmlLoaderBase.getInnerTextByPath(node, "./frame/function/offset");
            sData.m_strFile = XmlLoaderBase.getInnerTextByPath(node, "./frame/file");
            sData.m_strLineNum = XmlLoaderBase.getInnerTextByPath(node, "./frame/line/number");
            sData.m_nLineNum = (null == sData.m_strLineNum || sData.m_strLineNum.Length == 0) ? 0 : Convert.ToInt32(sData.m_strLineNum);
            sData.m_strLineOffset = XmlLoaderBase.getInnerTextByPath(node, "./frame/line/offset");
            if (null == sData.m_strAddress || "" == sData.m_strAddress)
                return null;
            int nTmpNum = sData.m_strAddress.IndexOf(':');
            if (0 > nTmpNum)
                return null;
            sData.m_nAddrOffset = Convert.ToInt32(sData.m_strAddress.Substring(0, nTmpNum), 16);
            sData.m_nAddrAddress = Convert.ToInt32(sData.m_strAddress.Substring(nTmpNum + 1, sData.m_strAddress.Length - nTmpNum - 1), 16);
            return sData;
        }

    }

    class ThreadDataLoader
    {
        static public ThreadData LoadFromXml(XmlNode node)
        {
            if (null == node)
                return null;
            ThreadData tData = new ThreadData();
            string strThreadId = XmlLoaderBase.getInnerTextByPath(node, "./thread/id");
            if (null != strThreadId && strThreadId != "")
            {
                tData.m_nThreadId = Convert.ToInt32(strThreadId, 16);
            }
            else
            {
                return null;
            }
            tData.m_strThreadStatus = XmlLoaderBase.getInnerTextByPath(node, "./thread/status");
            XmlNodeList lstNode = node.SelectNodes("./thread/stack");
            if (null == lstNode)
                return null;
            tData.m_lstStackData = new List<StackData>();
            foreach (XmlNode n in lstNode)
            {
                if (null == n)
                    continue;
                StackData sData = StackDataLoader.LoadFromXml(n);
                if (null == sData)
                    return null;
                tData.m_lstStackData.Add(sData);
            }
            return tData;
        }
    }
}
