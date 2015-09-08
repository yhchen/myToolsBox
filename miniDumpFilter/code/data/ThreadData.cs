using System;
using System.Collections.Generic;
using System.Text;

namespace miniDumpFilter
{
    class ThreadData
    {
        public int m_nThreadId;                 //./thread/id
        public string m_strThreadStatus;        //./status
        public List<StackData> m_lstStackData;  //./stack
    }
}
