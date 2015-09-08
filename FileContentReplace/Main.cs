using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SpaceKiller
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void btnSelectSourceFile_Click(object sender, EventArgs e)
        {
//             OpenFileDialog fileDialog = new OpenFileDialog();
//             fileDialog.Title = "选择源文件";
//             fileDialog.Filter = "文本文件 |*.xml;*.txt;*.csv|" +
//                                 "所有文件 |*.*";
// 
//             if (fileDialog.ShowDialog() == DialogResult.OK)
//             {
//                 txtSourceFile.Text = fileDialog.FileName;
//             }
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (Directory.Exists(txtSourcePath.Text))
            {
                dialog.SelectedPath = txtSourcePath.Text;
            }
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtSourcePath.Text = dialog.SelectedPath;
            }
        }

        char ConvertToFormat(char c)
        {
            switch (c)
            {
                case 'n':	return '\n';	//换行
                case 'r':	return '\r';	//回车
                case 't':	return '\t';	//制表符
                case 'f':	return '\f';	//formfeed
                case 'b':	return '\b';	//退格
                case 'a':	return '\a';	//响铃
//                 case 'e':	return '\e';	//escape（ASCII中的escape 字符）
                //	\007	任何八进制值（这里是，007=bell(响铃)）
                //	\x7f	任何十六进制值（这里是，007=bell）
                //	\cC	一个控制符（这里是，ctrl+c）
                case '\\':	return '\\';	//反斜线
                case '\"':	return '\"';	//双引号
//                 case 'l':	return '\l';	//下个字符小写
//                 case 'L':	return '\L';	//接着的字符均小写直到\E
//                 case 'u':	return '\u';	//下个字符大写
//                 case 'U':	return '\U';	//接着的字符均大写直到\E
//                 case 'Q':	return '\Q';	//在non-word字符前加上\(自动加转义符号)，直到\E
//                 case 'E':	return '\E';	//结束\L,\E和\Q
            }
            return '\0';
        }

        string FilterString(string input)
        {
            string output = "";
            for (int i = 0; i < input.Length;++i)
            {
                if (input[i] == '\\' && i < input.Length-1)
                {
                    char c = ConvertToFormat(input[i+1]);
                    if (c == '\0')
                    {
                        output += '\\';
                    }
                    else
                    {
                        output += c;
                        ++i;
                    }
                }
                else
                {
                    output += input[i];
                }
            }
            return output;
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            string strSourcePath = txtSourcePath.Text;
            string strRegText = txtRegex.Text;
            string strOriginReplaceText = txtReplaceString.Text;
            string strReplaceText = FilterString(strOriginReplaceText);//String.Format("{0:s}", strOriginReplaceText);
            string strFileFilter = txtFileFilter.Text.Trim();

            if (string.IsNullOrEmpty(strSourcePath))
            {
                MessageBox.Show("请先选择源文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSelectSourceFile_Click(null, null);
                return;
            }
            if (string.IsNullOrEmpty(strFileFilter))
            {
                MessageBox.Show("请先选择文件过滤条件,以\'|\'分割，\'*\'代表全部", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtFileFilter.Focus();
                return;
            }

            string[] strAllFiles = Directory.GetFiles(strSourcePath);
            string[] strFilters = strFileFilter.Split('|');
            int nTotalHandleFile = 0;
            foreach(string fileName in strAllFiles)
            {
                bool boPassFile = true;
                foreach (string filter in strFilters)
                {
                    if (filter == "*" || filter == fileName.Substring(Math.Max(0, fileName.Length - 1 - filter.Length)))
                    {
                        boPassFile = false;
                        break;
                    }
                }
                if (boPassFile)
                    continue;
                ++nTotalHandleFile;
                Encoding fileEncoding = TextFileUtil.GetFileEncodeType(fileName);
                string fileContent = File.ReadAllText(fileName, fileEncoding);
                string targetContent = Regex.Replace(fileContent, strRegText, strReplaceText);
                File.WriteAllText(fileName, targetContent, fileEncoding);
            }

            MessageBox.Show("处理完毕，总共处理 " + nTotalHandleFile + " 个文件", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
