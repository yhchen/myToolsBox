using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace miniDumpFilter
{
    class StackData
    {
        public string m_strModule;      //module
        public string m_strAddress;      //address
        public string m_strFuncName;    //function/name
        public string m_strFuncOffset;  //function/offset
        public string m_strFile;        //file
        public string m_strLineNum;     //line/number
        public string m_strLineOffset;  //line/offset
        public int m_nAddrOffset;
        public int m_nAddrAddress;
        public int m_nLineNum;
    }
}
