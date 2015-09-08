using System;
using System.Collections.Generic;
using System.Text;

namespace miniDumpFilter
{
    class ProcessData
    {
        public string m_strProcessName;                //./name
        public string m_strProcessId;                  //./id
        public int m_nProcessId;
        public Dictionary<string, ModuleData> m_lstModules;

        /// <summary>
        /// get Module Base Address By Name
        /// </summary>
        /// <param name="strModuleName"></param>
        /// <returns></returns>
        public int getModuleBaseByModuleName(string strModuleName)
        {
            if (m_lstModules.ContainsKey(strModuleName))
            {
                ModuleData mData = m_lstModules[strModuleName];
                if (null != mData)
                {
                    return mData.m_nModuleBase;
                }
            }
            return 0;
        }
    }
}
