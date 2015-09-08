using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace listenFileToRename
{
    public partial class FormMain : Form
    {
        /// <summary>
        /// variable
        /// </summary>
        private int m_nCurrShowSpot = 0;
        private String m_strStartListen = "开始监听(&S)";
        private String m_strEndListen = "取消监听(&S)";

        /// <summary>
        /// methods
        /// </summary>
        public FormMain()
        {
            InitializeComponent();
            btnStart.Text = m_strStartListen;
            label3.Text = "";
            m_nCurrShowSpot = 0;
            textBox_source.Text = Properties.Settings.Default.Setting_str_Source;
            textBox_replace.Text = Properties.Settings.Default.Setting_str_replace;
            this.Text = "文件监听程序 - " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            //btnStart.UseMnemonic = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // update spot
            ++m_nCurrShowSpot;
            int nSpotCnt = m_nCurrShowSpot / 10;
            String strSpot = "正在监听";
            for (int i = 0; i < nSpotCnt; ++i)
                strSpot += ".";
            label3.Text = strSpot;
            if (m_nCurrShowSpot >= 39)
                m_nCurrShowSpot = 0;

            // copy file
            String strCurDir = System.Environment.CurrentDirectory + @"\";
            String strTargetFile = strCurDir + textBox_replace.Text;
            String strCopyFile = strCurDir + textBox_source.Text;
            if (File.Exists(strTargetFile))
            {
                File.Delete(strTargetFile);
                File.Copy(strCopyFile, strTargetFile);
                btnStart.Text = m_strStartListen;
                label3.Text = "已替换完成";
                timer1.Enabled = false;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            onSwitchListenStatus(!timer1.Enabled);
        }

        private void textBox_copy_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Setting_str_Source = textBox_source.Text;
            Properties.Settings.Default.Save();
        }

        private void textBox_target_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Setting_str_replace = textBox_replace.Text;
            Properties.Settings.Default.Save();
        }

        private void onStartListener(string strClearPath, string sourceFileFullName)
        {
            //MessageBox.Show("移除目录[" + strClearPath + "]");
            DirectoryInfo dirInfo = new DirectoryInfo(strClearPath);
            FileInfo[] fileList = dirInfo.GetFiles();
            String strBackupPath = strClearPath + @"\" + System.DateTime.Now.ToString(@"yyyy_MM_dd_HH_mm_ss_ffffff");
            Directory.CreateDirectory(strBackupPath);
            bool boBackupFile = false;
            foreach (FileInfo fileInfo in fileList)
            {
                try
                {
                    // ignore current run file
                    if (fileInfo.FullName.ToLower() == Application.ExecutablePath.ToLower())
                        continue;
                    // ignore source file
                    if (fileInfo.FullName.ToLower() == sourceFileFullName.ToLower())
                        continue;
                    // move file to backup file
                    File.Move(fileInfo.FullName, strBackupPath + @"\" + fileInfo.Name);
                    boBackupFile = true;
                }
                catch{ }
            }
            if (!boBackupFile)
            {
                Directory.Delete(strBackupPath);
            }
        }

        private void onSwitchListenStatus(bool boStatus)
        {
            if (boStatus == timer1.Enabled)
                return; //状态已经切换
            // check can listening
            if (boStatus)
            {
                // check textBox_copy exists
                if (!File.Exists(textBox_source.Text))
                {
                    MessageBox.Show("找不到目标文件[" + textBox_source.Text + "]");
                    return;
                }
            }
            // switch listener status
            timer1.Enabled = boStatus;
            m_nCurrShowSpot = 0;
            if (boStatus)
            {
                btnStart.Text = m_strEndListen;
                try
                {
                    onStartListener(new FileInfo(textBox_replace.Text).DirectoryName, new FileInfo(textBox_source.Text).FullName);
                }
                catch { }
            }
            else
            {
                btnStart.Text = m_strStartListen;
            }
            label3.Text = "";
            m_nCurrShowSpot = 0;
        }

        private void FormMain_Activated(object sender, EventArgs e)
        {
            //注册热键Shift+S，Id号为100。HotKey.KeyModifiers.Shift也可以直接使用数字4来表示。
            HotKey.RegisterHotKey(Handle, 100, HotKey.KeyModifiers.Ctrl, Keys.S);
        }

        private void FormMain_Leave(object sender, EventArgs e)
        {
            //注销Id号为100的热键设定
            HotKey.UnregisterHotKey(Handle, 100);
        }

        /// 
        /// 监视Windows消息
        /// 重载WndProc方法，用于实现热键响应
        /// 
        /// 
        protected override void WndProc(ref Message m)
        {

            const int WM_HOTKEY = 0x0312;
            //按快捷键 
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    switch (m.WParam.ToInt32())
                    {
                        case 100:    //按下的是Ctrl+S
                            //onSwitchListenStatus(!timer1.Enabled);
                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        }// end switch
    }
}
