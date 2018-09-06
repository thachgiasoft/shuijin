using ZCSharpLib;
using ZCSharpLib.Http;
using ZCSharpLib.Tick;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Threading;
using ZCSharpLib.Json;
using ZCSharpLib.zip;
using System.IO;
using System.Windows;
using System.Threading;

namespace Upgrade
{
    public sealed class AppMain
    {
        private static string RootPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase.Replace("\\", "/");
        private static string LocalPath = RootPath.Substring(0, RootPath.Length - 1);
        private static string CfgPath = LocalPath + "/upgrade.cfg";
        private static string VLocalPath = LocalPath + "/release.txt";
        private static string Bootstrap = LocalPath + "/bootstrap.bat";
        private static string UpgradeInfo = LocalPath + "/upgrade.info";
        // 本地测试用
        //private static string ServerUpgradePackageUrl = RemotePath + "/update/";
        // 本地测试用
        //private static string ServerUpgradeConfigUrl = RemotePath + "/update/release.txt";
        private static string ServerUpgradePackageUrl;
        private static string ServerUpgradeConfigUrl;

        private static MainWindow MainWindow;
        private static Stopwatch Stopwatch;
        private static Version LocalVersion;
        private static VersionExt LocalVersionExt;
        private static VersionExt RemoteVersionExt;
        private static StringBuilder Sb;
        private static string LocalZip;
        //private static int TickID;

        public static void Initialize()
        {
            App.Setup(new Bootstarp());
            App.Register();

            Sb = new StringBuilder();
            Stopwatch = new Stopwatch();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Tick;
            timer.Start();
        }

        private static void Tick(object sender, EventArgs e)
        {
            Stopwatch.Stop();
            App.Make<Tick>().Update(Stopwatch.ElapsedMilliseconds * 0.001f);
            Stopwatch.Reset();
            Stopwatch.Start();
        }

        private static void OnTick(float obj)
        {

        }

        public static void Setup(MainWindow window)
        {
            MainWindow = window;
        }

        public static void Startup()
        {
            //TestRemoteWrite();
            //TestLocalWrite();
            //TestCfgWrite();

            // 获取本地版本号
            try
            {
                if (File.Exists(CfgPath) == false)
                {
                    Sb.Append("没有找到版本文件：" + VLocalPath);
                    MainWindow.TxtVersion.Content = "无法获取配置文件";
                }
                else
                {
                    string cfgJson = File.ReadAllText(CfgPath);
                    UpgradeCfg oCfg = JsonUtility.Decode<UpgradeCfg>(cfgJson);
                    ServerUpgradePackageUrl = oCfg.ServerUpgradePackageUrl;
                    ServerUpgradeConfigUrl = oCfg.ServerUpgradeConfigUrl;
                }

                if (File.Exists(VLocalPath) == false)
                {
                    Sb.Append("没有找到版本文件：" + VLocalPath);
                    MainWindow.TxtVersion.Content = "无法获取软件信息";
                }
                else
                {
                    string localVersion = File.ReadAllText(VLocalPath);

                    if (string.IsNullOrEmpty(localVersion) == true)
                    {
                        Sb.Append("版本信息为空：" + VLocalPath);
                        MainWindow.TxtVersion.Content = "无法获取软件信息";
                    }
                    else
                    {
                        LocalVersionExt = JsonUtility.Decode<VersionExt>(localVersion);

                        MainWindow.TxtVersion.Content = LocalVersionExt.Release.Version.Replace(".zip", "");

                        LocalVersion = new Version(LocalVersionExt.Release.Version);
                    }
                }
            }
            catch (System.Exception e)
            {
                Sb.Append("出现异常：" + e.ToString());
                MainWindow.TxtVersion.Content = "无法获取软件信息";
            }

            if (LocalVersionExt != null)
            {
                string content = string.Empty;

                try
                {
                    HttpHelper oHeper = new HttpHelper();
                    content = oHeper.HttpGet(ServerUpgradeConfigUrl, string.Empty);
                }
                catch (Exception e)
                {
                    // 这里进行日志输出
                    Sb.Append(e.ToString());
                }

                if (string.IsNullOrEmpty(content) == true)
                {
                    MainWindow.TxtUpdateInfo.Content = "无法连接到服务器！";
                    string localVersion = LocalVersionExt.Release.Name.Replace(".zip", "");
                    Process(LocalPath + "/" + localVersion);
                }
                else
                {
                    // 解析服务器版本号,获取最后的版本
                    VersionInfos versionInfos = JsonUtility.Decode<VersionInfos>(content);

                    //Version oCompareRemoteVersion = new Version(LocalVersion.ToString());
                    //for (int i = 0; i < versionInfos.Release.Length; i++)
                    //{
                    //    VersionInfo oTmpRemoteVersionInfo= versionInfos.Release[i];
                    //    Version oTmpRemoteVersion = new Version(oTmpRemoteVersionInfo.Version);
                    //    if (oTmpRemoteVersion.Compare(oCompareRemoteVersion.ToString()) == 1)
                    //    {
                    //        oCompareRemoteVersion = oTmpRemoteVersion;
                    //    }
                    //}

                    VersionInfo remoteVersionInfo = versionInfos.Release[versionInfos.Release.Length - 1];
                    RemoteVersionExt = new VersionExt() { Release = remoteVersionInfo };
                    try
                    {
                        Version localTMPV = new Version(LocalVersionExt.Release.Version);
                        Version remoteTMPV = new Version(RemoteVersionExt.Release.Version);
                        int versionCode = localTMPV.Compare(remoteTMPV.ToString());
                        if (versionCode == -1)
                        {
                            // 输入版本大于当前版本
                            MainWindow.TxtNextVersion.Content = remoteTMPV.ToString().Replace(".zip", "");
                            string appUrl = ServerUpgradePackageUrl + "/" + Path.GetFileNameWithoutExtension(remoteVersionInfo.Name) + ".zip";
                            LocalZip = LocalPath + "/" + Path.GetFileNameWithoutExtension(remoteVersionInfo.Name) + ".zip";
                            ZCSharpLib.App.Make<HttpLoader>().AddLoad(appUrl, LocalZip, OnFinishedHandler, OnProgressHandler);
                        }
                        else
                        {
                            string localVersion = LocalVersionExt.Release.Name.Replace(".zip", "");
                            Process(LocalPath + "/" + localVersion);
                        }
                    }
                    catch (System.Exception e)
                    {
                        Sb.Append("\r\n" + e.ToString());
                    }
                }

                File.WriteAllText(UpgradeInfo, Sb.ToString());
            }
        }

        private static void OnProgressHandler(string url, bool isDone, float progress)
        {
            MainWindow.LoadProgress.Value = progress * 100;
            MainWindow.TxtUpdateInfo.Content = string.Format("下载进度：{0}%", (progress * 100).ToString("0.00"));
        }

        private static void OnFinishedHandler(bool isSucess, string error)
        {
            if (isSucess)
            {
                MainWindow.TxtUpdateInfo.Content = "正在解压,请稍候!";
                ZipMgr helper = new ZipMgr();
                helper.Decompress(LocalZip, Path.GetDirectoryName(LocalZip), OnFinished);
            }
            else
            {
                // TODO
                MainWindow.TxtUpdateInfo.Content = "最新版本客户端下载失败!";
                string localVersion = LocalVersionExt.Release.Name.Replace(".zip", "");
                Process(LocalPath + "/" + localVersion);
            }
        }

        private static void OnFinished(bool isSucess)
        {
            if (isSucess == true)
            {
                string localVersion = LocalVersionExt.Release.Name.Replace(".zip", "");

                string localDirectory = LocalPath + "/" + localVersion;
                if (Directory.Exists(localDirectory))
                {
                    Directory.Delete(localDirectory, true);
                }

                string localLastZip = LocalPath + "/" + localVersion + ".zip";
                if (File.Exists(localLastZip) == true)
                {
                    File.Delete(localLastZip);
                }

                if (File.Exists(VLocalPath) == true)
                {
                    File.Delete(VLocalPath);
                }

                VersionExt infos = RemoteVersionExt;
                string json = JsonUtility.Encode(infos);
                File.WriteAllText(VLocalPath, json);

                MainWindow.TxtUpdateInfo.Content = "解压成功,程序即将运行!";

                // TODO：启动客户端
                string remoteVersion = RemoteVersionExt.Release.Name.Replace(".zip", "");
                Process(LocalPath + "/" + remoteVersion);
            }
            else
            {
                string localVersion = LocalVersionExt.Release.Name.Replace(".zip", "");
                Process(LocalPath + "/" + localVersion);
            }
        }

        private static void Process(string param)
        {
            try
            {
                System.Diagnostics.Process.Start(Bootstrap, param);
            }
            catch (System.Exception e)
            {
                Sb.Append("\r\n" + e.ToString());
            }

            File.WriteAllText(UpgradeInfo, Sb.ToString());

            Application.Current.Shutdown();
        }

        #region 用于测试

        private static void TestRemoteWrite()
        {
            VersionInfos infos = new VersionInfos();
            infos.Release = new VersionInfo[1]
            {
                new VersionInfo(){ Name = "xuyouji_1001", Version="0.1.0" , MD5="ceshi"}
            };
            string json = JsonUtility.Encode(infos);
            File.WriteAllText("X:/ProjectAssets/xuyouji/release.txt", json);
        }

        private static void TestLocalWrite()
        {
            VersionExt infos = new VersionExt();
            infos.Release = new VersionInfo { Name = "xuyouji_1001", Version = "0.1.0", MD5 = "ceshi" };
            string json = JsonUtility.Encode(infos);
            File.WriteAllText(VLocalPath, json);
        }

        private static void TestCfgWrite()
        {
            UpgradeCfg oCfg = new UpgradeCfg();
            oCfg.ServerUpgradePackageUrl = "http://vr.4008289828.com/Uploads/system_version";
            oCfg.ServerUpgradeConfigUrl = "http://vr.4008289828.com/Uploads/system_version/release.txt";
            string json = JsonUtility.Encode(oCfg);
            File.WriteAllText(CfgPath, json);
        }

        #endregion
    }
}
