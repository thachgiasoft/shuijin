using GameTool.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AppMain.Startup();
        }

        #region Icon生成
        private void BtnIconGen_Click(object sender, RoutedEventArgs e)
        {
            string sPath = TxtPathIconSrcImg.Text;
            if (!string.IsNullOrEmpty(sPath))
            {
                SaveFileDialog oSaveFileDialog = new SaveFileDialog();
                oSaveFileDialog.DefaultExt = ".ico";
                oSaveFileDialog.Filter = "all file|*.ico";
                if (oSaveFileDialog.ShowDialog() ==  System.Windows.Forms.DialogResult.OK)
                {
                    string destPath = oSaveFileDialog.FileName;
                    Modules.IconGen.IconGenerate oIconGen = AppMain.Get<Modules.IconGen.IconGenerate>();
                    int size = int.Parse(PopListIcon.Text);
                    bool isSucess = oIconGen.ConvertToIcon(sPath, destPath, size);
                    string tip = isSucess ? "成功" : "失败";
                    System.Windows.Forms.MessageBox.Show("导出" + tip, "提示");
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("原始图像地址不能为空", "提示");
            }
        }

        private void BtnIconSelectSrcImg_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog oOpenFileDialog = new OpenFileDialog();
            oOpenFileDialog.DefaultExt = ".png";
            oOpenFileDialog.Filter = "all file|*.png;*.jpg;*.tga";
            if (oOpenFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TxtPathIconSrcImg.Text = oOpenFileDialog.FileName;
            }
        }

        #endregion

        #region 配置文件数据生成
        private void BtnCfgSelectSrcCfg_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog oFolderBrowserDialog = new FolderBrowserDialog();
            if (oFolderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TxtPathCfgSrcCfg.Text = oFolderBrowserDialog.SelectedPath;
            }
        }

        private void BtnCfgSelectDestScript_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog oFolderBrowserDialog = new FolderBrowserDialog();
            if (oFolderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TxtPathCfgDestScript.Text = oFolderBrowserDialog.SelectedPath + "\\TplScript.cs";
            }
        }

        private void BtnCfgSelectData_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog oFolderBrowserDialog = new FolderBrowserDialog();
            if (oFolderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TxtPathCfgData.Text = oFolderBrowserDialog.SelectedPath + "\\data.dat";
            }
        }

        private void BtnCfgGen_Click(object sender, RoutedEventArgs e)
        {
            string cfgPath = TxtPathCfgSrcCfg.Text;
            string destScript = TxtPathCfgDestScript.Text;
            string destData = TxtPathCfgData.Text;
            if (string.IsNullOrEmpty(cfgPath) || string.IsNullOrEmpty(destScript)
                || string.IsNullOrEmpty(destData))
            {
                System.Windows.Forms.MessageBox.Show("输入目录和输出目录不能为空", "提示");
            }
            else
            {
                ZLogger logger = new ZLogger();
                logger.Setup((str) => { TxtCfgConsole.Text = str; });
                Modules.CfgData.CfgDataGenerate oGen = new Modules.CfgData.CfgDataGenerate();
                oGen.SetLogger(logger);
                bool isSucess = oGen.ToGen(cfgPath, destScript, destData);
                if (isSucess)
                {
                    System.Windows.Forms.MessageBox.Show("生成成功!", "提示");
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("生成失败, 请查看控制台记录!", "提示");
                }
            }
        }
        #endregion


    }
}
