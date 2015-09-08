using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace miniDumpFilter
{
    class XmlLoaderBase
    {
        static public string getInnerTextByPath(XmlNode node, string strPath)
        {
            if (null == node)
                return "";
            XmlNode resNode = node.SelectSingleNode(strPath);
            if (null == resNode)
                return "";
            if (null == resNode.InnerText)
                return "";
            return resNode.InnerText;
        }
    }
}
