using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using miniDumpFilter.xml;

namespace miniDumpFilter
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        protected void getPathAllFiles(string strPath, string strFilter, List<FileInfo> lstFiles)
        {
            DirectoryInfo dir = new DirectoryInfo(strPath);
            FileInfo[] allFile = dir.GetFiles();
            foreach (FileInfo fi in allFile)
            {
                if (null != strFilter && 0 != strFilter.Length && !fi.Extension.Equals(strFilter))
                    continue;
                lstFiles.Add(fi);
            }
            DirectoryInfo[] allDirection = dir.GetDirectories();
            foreach (DirectoryInfo di in allDirection)
            {
                getPathAllFiles(di.FullName, strFilter, lstFiles);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (null == tbSearchPath.Text || 0 == tbSearchPath.Text.Length)
            {
                MessageBox.Show("请先选择搜索目录");
                return;
            }
            List<FileInfo> lstFiles = new List<FileInfo>();
            getPathAllFiles(tbSearchPath.Text, ".xml", lstFiles);
            foreach (FileInfo fi in lstFiles)
            {
                XmlAnalysis.addXmlFileData(fi.FullName);
            }
            List<SortResult> doResult = XmlAnalysis.DoAnalysis(0);
            tbResult.Clear();
            tbResult.AppendText("++++++++++++++++++++++++++++++++++++++++\n");
            tbResult.AppendText("total search " + ".xml" + " file " + lstFiles.Count + "\n");
            tbResult.AppendText("total different dump type " + doResult.Count + "\n");
            tbResult.AppendText("++++++++++++++++++++++++++++++++++++++++\n");
            foreach (var sameGrp in doResult)
            {
                tbResult.AppendText("= Total Found [" + sameGrp.fileList.Count + "] Times.================\n");
                foreach (string strFile in sameGrp.fileList)
                {
                    tbResult.AppendText(strFile + "\n");
                }
                tbResult.AppendText("\n\n\n");
            }
        }
    }
}
