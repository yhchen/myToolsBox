using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace publishUpdateList.Common
{
    public class HttpPost
    {
        public void addPostValues(string key, string value, bool boUrlEncode)
        {
            if (boUrlEncode)
            {
                m_values.Set(key, UrlEncode(value));
            }
            else
            {
                m_values.Set(key, value);
            }
        }

        public string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = GlobalsConfig.defaultEncoder.GetBytes(str); //默认是System.Text.Encoding.Default.GetBytes(str)
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }

            return (sb.ToString());
        }

        public string PostWebRequest(string postUrl, Encoding dataEncode)
        {
            StringBuilder parameters = new StringBuilder();

            for (int i = 0; i < m_values.Count; i++)
            {
                EncodeAndAddItem(ref parameters, m_values.GetKey(i), m_values[i]);
            }

            string ret = string.Empty;
            byte[] byteArray = dataEncode.GetBytes(parameters.ToString()); //转化
            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
            webReq.Method = "POST";
            webReq.ContentType = "application/x-www-form-urlencoded";

            webReq.ContentLength = byteArray.Length;
            Stream newStream = webReq.GetRequestStream();
            newStream.Write(byteArray, 0, byteArray.Length);//写入参数
            newStream.Close();
            HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream(), GlobalsConfig.defaultEncoder);
            ret = sr.ReadToEnd();
            sr.Close();
            response.Close();
            newStream.Close();
            return ret;
        }

        private void EncodeAndAddItem(ref StringBuilder parameters, string key, string dataItem)
        {
            if (parameters.Length != 0)
            {
                parameters.Append("&");
            }
            parameters.Append(key);
            parameters.Append("=");
            parameters.Append(dataItem);
        }

        private NameValueCollection m_values = new NameValueCollection();
    }
}
