using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.IO.Compression;
using System.Windows.Input;
using AutoUpdaterDotNET;
using System.Reflection;

namespace IRO补丁管理工具
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        string rootDirectory;
        string datagrf;

        public MainWindow()
        {
            IniFile ini = new IniFile(@".\set.ini");
            //读取根目录路径
            rootDirectory = ini.IniReadValue("路径", "根目录");
            //读取data.grf文件路径
            datagrf = ini.IniReadValue("路径", "data文件");
            AutoUpdater.Start("https://www.jiangxue.fun:5001/share.cgi/AutoUpdateText.xml?ssid=920144d4fc474ec5878b09f1a65385ba&openfolder=normal&ep=&_dc=1686143389890&fid=920144d4fc474ec5878b09f1a65385ba");
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Button_Readfolder_Click(object sender, RoutedEventArgs e)
        {
            SelectRootFolder();
            string batPath = @".\预读.bat";
            //如果没有预读脚本，写出脚本
            if (!File.Exists(batPath))
            {
                StringBuilder argsString = new StringBuilder();
                argsString.AppendLine(@"@echo off");
                argsString.AppendLine("set /p \"merge=输入1启用预读，输入2停用预读 \"");
                argsString.AppendLine("");
                argsString.AppendLine("if %merge% == 1 goto :edits");
                argsString.AppendLine("if %merge% == 2 goto :backups");
                argsString.AppendLine("");
                argsString.AppendLine(":edits");
                argsString.AppendLine($"\".\\GrfCL.exe\" -m \"{datagrf}\" \".\\readfolder.grf\"");
                argsString.AppendLine("");
                argsString.AppendLine("goto :pause");
                argsString.AppendLine("");
                argsString.AppendLine(":backups");
                argsString.AppendLine($"\".\\GrfCL.exe\" -m \"{datagrf}\" \".\\readfolderbackup.grf\"");
                argsString.AppendLine("");
                argsString.AppendLine(":pause");
                argsString.AppendLine("pause");
                //File.WriteAllText(fileName,argsString.ToString(), Encoding.GetEncoding("gb2312"));
                File.WriteAllText(batPath, argsString.ToString(), CodePagesEncodingProvider.Instance.GetEncoding("gb2312"));
            }

            //执行bat脚本
            Process proc = new Process();
            proc.StartInfo.FileName = @".\预读.bat";
            //proc.StartInfo.Arguments = string.Format("10");//this is argument
            proc.StartInfo.UseShellExecute = false;//运行时隐藏dos窗口
            proc.StartInfo.CreateNoWindow = false;//运行时隐藏dos窗口
            proc.StartInfo.Verb = "runas";//设置该启动动作，会以管理员权限运行进程
            proc.Start();
            proc.WaitForExit();
            File.Delete(batPath);
            MessageBox.Show("已完成预读设置", "完成");
        }

        private void kpfwCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            kpfyCheckBox.IsChecked = false;
        }

        private void kpfyCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            kpfwCheckBox.IsChecked = false;
        }

        private void Button_Lazy_Click(object sender, RoutedEventArgs e)
        {
            jcbCheckBox.IsChecked = true;
            kpfyCheckBox.IsChecked = true;
            k3sCheckBox.IsChecked = true;
            sysCheckBox.IsChecked = true;
            hywCheckBox.IsChecked = true;
            mjdCheckBox.IsChecked = true;
            xtwCheckBox.IsChecked = true;
            xsfCheckBox.IsChecked = true;
            xdtCheckBox.IsChecked = true;
            zszCheckBox.IsChecked = true;
            sjjCheckBox.IsChecked = true;
        }

        private void Button_Backup_Click(object sender, RoutedEventArgs e)
        {
            SelectRootFolder();
            BackUp();
            MessageBox.Show("已卸载全部补丁", "完成");
        }

        private void Button_Patch_Click(object sender, RoutedEventArgs e)
        {
            SelectRootFolder();
            Patch();
            MessageBox.Show("已应用补丁，请重启游戏确认", "完成");
        }

        private void Button_Help_Click(object sender, RoutedEventArgs e)
        {
            var window = new HelpWindow();
            window.Show();
        }
        private void BackUp()
        {

            string FolderPath = rootDirectory + @"\data";
            string zipPath = @".\补丁\复原data\data.zip";
            if (Directory.Exists(FolderPath))
            {
                Directory.Delete(FolderPath, true);
            }
            ZipFile.ExtractToDirectory(zipPath, FolderPath, CodePagesEncodingProvider.Instance.GetEncoding("gb2312"), true);
        }

        private void Patch()
        {
            string FolderPath = rootDirectory + @"\data";
            string zipPath;
            if (bakeupCheckBox.IsChecked == true)
            {
                BackUp();
            }
            if (jcbCheckBox.IsChecked == true)
            {
                zipPath = @".\补丁\基础补丁包200518\data.zip";
                ZipFile.ExtractToDirectory(zipPath, FolderPath, CodePagesEncodingProvider.Instance.GetEncoding("gb2312"), true);
            }
            if (kpfwCheckBox.IsChecked == true)
            {
                zipPath = @".\补丁\卡片放大\群友卡无字版\data.zip";
                ZipFile.ExtractToDirectory(zipPath, FolderPath, CodePagesEncodingProvider.Instance.GetEncoding("gb2312"), true);
            }
            if (kpfyCheckBox.IsChecked == true)
            {
                zipPath = @".\补丁\卡片放大\群友卡有字版\data.zip";
                ZipFile.ExtractToDirectory(zipPath, FolderPath, CodePagesEncodingProvider.Instance.GetEncoding("gb2312"), true);
            }
            if (k3sCheckBox.IsChecked == true)
            {
                zipPath = @".\补丁\矿3神力矿石放大补丁\data.zip";
                ZipFile.ExtractToDirectory(zipPath, FolderPath, CodePagesEncodingProvider.Instance.GetEncoding("gb2312"), true);
            }
            if (sysCheckBox.IsChecked == true)
            {
                zipPath = @".\补丁\所有树去树冠史芬克斯去横梁\data.zip";
                ZipFile.ExtractToDirectory(zipPath, FolderPath, CodePagesEncodingProvider.Instance.GetEncoding("gb2312"), true);
            }
            if (hywCheckBox.IsChecked == true)
            {
                zipPath = @".\补丁\幻影乌龟绿油油\data.zip";
                ZipFile.ExtractToDirectory(zipPath, FolderPath, CodePagesEncodingProvider.Instance.GetEncoding("gb2312"), true);
            }
            if (mjdCheckBox.IsChecked == true)
            {
                zipPath = @".\补丁\通用免鉴定补丁\System.zip";
                ZipFile.ExtractToDirectory(zipPath, FolderPath.Replace("data", "System"), CodePagesEncodingProvider.Instance.GetEncoding("gb2312"), true);
                System.Diagnostics.Process.Start(FolderPath.Replace("data", "System") + @"\免鉴定补丁.cmd");
            }
            if (xtwCheckBox.IsChecked == true)
            {
                zipPath = @".\补丁\系统文件缺失修复补丁\data.zip";
                ZipFile.ExtractToDirectory(zipPath, FolderPath, CodePagesEncodingProvider.Instance.GetEncoding("gb2312"), true);
            }
            if (xsfCheckBox.IsChecked == true)
            {
                zipPath = @".\补丁\显示附魔卡片前缀补丁\data.zip";
                ZipFile.ExtractToDirectory(zipPath, FolderPath, CodePagesEncodingProvider.Instance.GetEncoding("gb2312"), true);
            }
            if (xdtCheckBox.IsChecked == true)
            {
                zipPath = @".\补丁\小地图补全补丁\data.zip";
                ZipFile.ExtractToDirectory(zipPath, FolderPath, CodePagesEncodingProvider.Instance.GetEncoding("gb2312"), true);
            }
            if (zszCheckBox.IsChecked == true)
            {
                zipPath = @".\补丁\战死者之墓亮化补丁\data.zip";
                ZipFile.ExtractToDirectory(zipPath, FolderPath, CodePagesEncodingProvider.Instance.GetEncoding("gb2312"), true);
            }
            if (sjjCheckBox.IsChecked == true)
            {
                zipPath = @".\补丁\视角解锁补丁\data.zip";
                ZipFile.ExtractToDirectory(zipPath, FolderPath, CodePagesEncodingProvider.Instance.GetEncoding("gb2312"), true);
            }
        }

        private void SelectRootFolder()
        {
            //定位RO游戏根目录
            if (!File.Exists(datagrf) || !datagrf.Contains("\\data.grf"))
            {
                // Configure open file dialog box
                var dialog = new Microsoft.Win32.OpenFileDialog();
                dialog.FileName = "data.grf"; // Default file name
                dialog.DefaultExt = ".grf"; // Default file extension
                dialog.Filter = "数据库文件(*.grf)|*.grf"; // Filter files by extension
                dialog.Title = "请选择IRO目录下的data.grf文件";
                // Show open file dialog box
                bool? result = dialog.ShowDialog();
                // Process open file dialog box results
                if (result == true && dialog.FileNames[0].Contains("\\data.grf"))
                {
                    // Open document
                    datagrf = dialog.FileName;
                    IniFile ini = new IniFile(@".\set.ini");
                    //写data.grf文件路径
                    ini.IniWriteValue("路径", "data文件", datagrf);
                    //写根目录路径
                    rootDirectory = datagrf.Replace(@"\data.grf", "");
                    ini.IniWriteValue("路径", "根目录", rootDirectory);
                }

                else
                {
                    MessageBox.Show("无法定位data.grf文件位置，请正确选择data.grf", "错误");
                }
            }
        }

        public static void CopyDirectory(string sourceDir, string targetDir)
        {
            // 创建目标目录
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            // 获取源目录中的所有文件和子目录
            string[] files = Directory.GetFiles(sourceDir);
            string[] subDirs = Directory.GetDirectories(sourceDir);

            // 复制所有文件
            foreach (string file in files)
            {
                string targetFile = System.IO.Path.Combine(targetDir, System.IO.Path.GetFileName(file));
                File.Copy(file, targetFile, true);
            }

            // 递归复制所有子目录
            foreach (string subDir in subDirs)
            {
                string targetSubDir = System.IO.Path.Combine(targetDir, System.IO.Path.GetFileName(subDir));
                CopyDirectory(subDir, targetSubDir);
            }
        }

    }
}
