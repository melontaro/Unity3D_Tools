using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ToolBox4Unity
{
    /// <summary>
    /// SetLink.xaml 的交互逻辑
    /// </summary>
    public partial class SetLink : Window
    {
        /// <summary>
        /// 创建符号链接
        /// </summary>
        /// <param name="SymbolicFileName">符号链接名字</param>
        /// <param name="TargetFileName">目标名字</param>
        /// <param name="Flags">0表示创建文件符号链接；1表示创建目录符号链接</param>
        /// <returns>是否成功</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.I1)]
        static extern bool CreateSymbolicLink(string SymbolicFileName, string TargetFileName, UInt32 Flags);

        const UInt32 SymbolicLinkFlagFile = 0;
        const UInt32 SymbolicLinkFlagDirectory = 1;
        public SetLink()
        {
            InitializeComponent();
        }

        private void SelectTargetFile(object sender, MouseButtonEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog() { Filter = "" };
            var result = openFileDialog.ShowDialog(); if (result == true) { this.txtTargetFile.Text = openFileDialog.FileName; }
        }

        private void SelectTargetFolder(object sender, MouseButtonEventArgs e)
        {
            FolderBrowserDialog m_Dialog = new FolderBrowserDialog(); DialogResult result = m_Dialog.ShowDialog(); if (result == System.Windows.Forms.DialogResult.Cancel) { return; }
            string m_Dir = m_Dialog.SelectedPath.Trim(); this.txtTargetFolder.Text = m_Dir;
        }

        private void SelectSourceFolderClick(object sender, MouseButtonEventArgs e)
        {
            FolderBrowserDialog m_Dialog = new FolderBrowserDialog(); DialogResult result = m_Dialog.ShowDialog(); if (result == System.Windows.Forms.DialogResult.Cancel) { return; }
            string m_Dir = m_Dialog.SelectedPath.Trim(); this.txtSourceFolder.Text = m_Dir;
        }
        // 创建链接
        void SetSymbolicLink(string target,string link,uint isFolder)
        {
  
            bool succ = CreateSymbolicLink(link, target, isFolder);

        }

        private void OnSetLinkFolderClick(object sender, RoutedEventArgs e)
        {
            string target = @txtTargetFolder.Text;
            string source = @txtSourceFolder.Text;
            SetSymbolicLink(target, source, SymbolicLinkFlagDirectory);
        }
    }
}
