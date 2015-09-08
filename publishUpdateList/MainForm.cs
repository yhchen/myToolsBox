using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using NDiffDiff;
using publishUpdateList.Common;

namespace publishUpdateList
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            tbSelecFile.Text = ConfigureManager.getLastSelcFile();
            tbURL.Text = ConfigureManager.getUpdateUrl();
            loadContentFromFile();

            m_boInitilized = true;
        }

        private void btn_Compare_Click(object sender, EventArgs e)
        {
            try
            {
                HttpPost post = new HttpPost();
                post.addPostValues("content", "GET", false);
                string strFeedBack = post.PostWebRequest(tbURL.Text, GlobalsConfig.defaultEncoder);
                if (strFeedBack.Length <= 0)
                {
                    MessageBox.Show("返回数据为空，请检查URL或请求列表[-1001]");
                    return;
                }
                if (strFeedBack[0] != '{')
                {
                    MessageBox.Show(strFeedBack);
                    return;
                }

                //将文件1和文件2存至临时文件中
                string sFileNew = TmpFileManager.createTmpFile(GlobalsConfig.defaultEncoder.GetBytes(tbContent.Text));
                string sFileOld = TmpFileManager.createTmpFile(GlobalsConfig.defaultEncoder.GetBytes(strFeedBack));

                CompareForm cmpForm = new CompareForm(sFileOld, sFileNew, tbURL.Text);
                cmpForm.ShowDialog();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tbContent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
                tbContent.SelectAll();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            TmpFileManager.freeAllObjects();
        }

        private void loadContentFromFile()
        {
            string strFile = tbSelecFile.Text;
            if (File.Exists(strFile))
            {
                tbContent.Clear();
                byte[] array = File.ReadAllBytes(strFile);
                tbContent.Text = GlobalsConfig.defaultEncoder.GetString(array);
            }
        }

        private void tbSelecFile_TextChanged(object sender, EventArgs e)
        {
            if (m_boInitilized)
            {
                ConfigureManager.setLastSelecFile(tbSelecFile.Text);
            }
        }

        private void tbURL_TextChanged(object sender, EventArgs e)
        {
            if (m_boInitilized)
            {
                ConfigureManager.setUpdateUrl(tbURL.Text);
            }
        }

        private void btn_OpenFile_Click(object sender, EventArgs e)
        {
            loadContentFromFile();
        }

        private void tbSelecFile_DragDrop(object sender, DragEventArgs e)
        {
            //从拖放的事件中取得需要的数据,注意要转换成字符串数组
            //然后再从字符串数组中取值
            string[] dropfiles = (string[])e.Data.GetData(DataFormats.FileDrop);
            tbSelecFile.Text = dropfiles[0];
            loadContentFromFile();
        }

        private void tbSelecFile_DragEnter(object sender, DragEventArgs e)
        {
            // 确定拖放的是文档
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                // 允许拖放动作继续,此时鼠标会显示为+
                e.Effect = DragDropEffects.All;
            }
        }

        private bool m_boInitilized = false;

        private void btnSelecFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string strFile = dlg.FileName;
                tbSelecFile.Text = strFile;
                loadContentFromFile();
            }
        }
    }
}
