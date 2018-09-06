using GameTool.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTool.Modules.CfgData
{
    public class CfgDataGenerate : IApplication
    {
        private ZLogger Logger { get; set; }

        public void Initialize()
        {
        }

        public void Uninitialize()
        {
        }

        public void SetLogger(ZLogger logger)
        {
            Logger = logger;
        }

        public bool ToGen(string srcPath, string scriptPath, string dataPath)
        {
            bool isSucess = true;
            try
            {
                // 开始计时
                Stopwatch oStopWatch = new Stopwatch();

                string[] files = Directory.GetFiles(srcPath, "*.xlsx");
                ParseExcel oParseExcel = new ParseExcel();
                oParseExcel.SetLogger(Logger);

                Logger.Info("=======================数据表处理开始===========================");
                oStopWatch.Start();
                isSucess = isSucess & oParseExcel.Parse(files);
                isSucess = isSucess & oParseExcel.Check();
                oStopWatch.Stop();
                Logger.Info("耗时：" + oStopWatch.Elapsed.Milliseconds + "毫秒");
                Logger.Info("=======================数据表处理结束==========================");
                if (isSucess)
                {
                    List<DataTable> dataTables = oParseExcel.DataTables;

                    {
                        // 生成脚本
                        Logger.Info("=======================脚本生成开始===========================");
                        oStopWatch.Start();
                        ToGenScript oGenScript = new ToGenScript();
                        string strScript = oGenScript.Gen(dataTables);
                        using (FileStream fs = new FileStream(scriptPath, FileMode.Create, FileAccess.Write))
                        {
                            byte[] bytes = Encoding.UTF8.GetBytes(strScript);
                            fs.Write(bytes, 0, bytes.Length);
                        }
                        oStopWatch.Stop();
                        Logger.Info("耗时：" + oStopWatch.Elapsed.Milliseconds + "毫秒");
                        Logger.Info("=======================脚本生成结束===========================");
                    }

                    {
                        // 生成二进制数据
                        Logger.Info("=======================表数据生成开始===========================");
                        oStopWatch.Start();
                        ToGenData oGenData = new ToGenData();
                        byte[] bytes = oGenData.Gen(dataTables);
                        using (FileStream fs = new FileStream(dataPath, FileMode.Create, FileAccess.Write))
                        {
                            fs.Write(bytes, 0, bytes.Length);
                        }
                        oStopWatch.Stop();
                        Logger.Info("耗时：" + oStopWatch.Elapsed.Milliseconds + "毫秒");
                        Logger.Info("=======================表数据生成结束===========================");
                    }
                }

            }
            catch (System.Exception e)
            {
                isSucess = false;
                Logger.Error(e.Message + "\n" + e.StackTrace);
            }
            return isSucess;
        }
    }
}
