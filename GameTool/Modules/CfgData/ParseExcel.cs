using GameTool.Core;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTool.Modules.CfgData
{
    public class ParseExcel
    {
        public List<DataTable> DataTables { get; private set; }

        private ZLogger Logger { get; set; }

        public void SetLogger(ZLogger logger)
        {
            Logger = logger;
        }

        public ParseExcel()
        {
            DataTables = new List<DataTable>();
        }

        public bool Parse(string[] files)
        {
            if (files.Length == 0) return false;
            string folderPath = Path.GetDirectoryName(files[0]);
            List<string> fileNames = new List<string>();
            for (int i = 0; i < files.Length; i++)
            {
                string fileName = Path.GetFileNameWithoutExtension(files[i]);
                string fileExt = Path.GetExtension(files[i]);
                fileNames.Add(fileName);
            }
            fileNames.Sort((a, b) => { return a.CompareTo(b); });

            for (int i = 0; i < fileNames.Count; i++)
            {
                string file = folderPath + "/" + fileNames[i] + ".xlsx";
                file = file.Replace("\\", "/");
                ExcelHelper helper = new ExcelHelper(file);
                helper.SetLogger(Logger);
                DataTable oDataTable = helper.ExcelToDataTable(null, true);
                oDataTable.TableName = fileNames[i]; // 设置表名
                if (oDataTable != null)
                {
                    DataTables.Add(oDataTable);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public bool Check()
        {
            bool isPass = true;
            for (int tableIndex = 0; tableIndex < DataTables.Count; tableIndex++)
            {
                DataTable oDataTable = DataTables[tableIndex];
                int rowCount = oDataTable.Rows.Count;
                if (rowCount < 3) 
                {
                    Logger.Error("{0} 表没有元配置, 请添加元配置!", oDataTable.TableName);
                    isPass = false;
                    continue; // 如果没有元数据则不再对当前表进行检查
                }
                int columnsCount = oDataTable.Columns.Count;

                {
                    // 首行数据检查
                    DataRow oDataRow = oDataTable.Rows[0];
                    for (int columnsIndex = 0; columnsIndex < columnsCount; columnsIndex++)
                    {
                        if (oDataRow.IsNull(columnsIndex))
                        {
                            Logger.Error("{0} 表第{1}行, 第{2}列数据为空", oDataTable.TableName, 1, columnsIndex + 1);
                            isPass = false;
                        }
                        else
                        {
                            string strType = oDataRow[columnsIndex].ToString();
                            bool isType = false;
                            if (strType.Equals("int")) isType = true;
                            else if (strType.Equals("float")) isType = true;
                            else if (strType.Equals("string")) isType = true;
                            if (!isType)
                            {
                                Logger.Error("{0} 表第{1}行，第{2}列数据数据异常，元数据只能为int;float;string这几种类型!", oDataTable.TableName, 1, columnsIndex + 1);
                                isPass = false;
                            }
                        }
                    }
                    if (!isPass) continue; // 如果首行有数据异常则不再对这张表进行检查 
                }

                {
                    // 第二行数据检查
                    List<string> oFiledNames = new List<string>();
                    DataRow oDataRow = oDataTable.Rows[1];
                    for (int columnsIndex = 0; columnsIndex < columnsCount; columnsIndex++)
                    {
                        if (oDataRow.IsNull(columnsIndex))
                        {
                            Logger.Error("{0} 表第{1}行, 第{2}列数据为空", oDataTable.TableName, 2, columnsIndex + 1);
                            isPass = false;
                        }
                        else
                        {
                            string strType = oDataRow[columnsIndex].ToString();
                            if (oFiledNames.IndexOf(strType) != -1)
                            {
                                Logger.Error("{0} 表第{1}行，第{2}列与已有的字段名重复。 注意：元数据中的第二行数据字段名不能重复!", oDataTable.TableName, 2, columnsIndex + 1);
                                isPass = false;
                            }
                            oFiledNames.Add(strType);
                        }
                    }
                    if (!isPass) continue; // 如果第二行有数据异常则不再对这张表进行检查 
                }

                {
                    // 第三行数据略过
                }

                {
                    // 从第四行数据开始检查
                    string[] strTypes = new string[columnsCount];
                    {
                        DataRow oDataRow = oDataTable.Rows[0];
                        for (int columnsIndex = 0; columnsIndex < columnsCount; columnsIndex++)
                        {
                            string strType = oDataRow[columnsIndex].ToString();
                            strTypes[columnsIndex] = strType;
                        }
                    }
                    for (int rowIndex = 3; rowIndex < rowCount; rowIndex++)
                    {
                        DataRow oDataRow = oDataTable.Rows[rowIndex];
                        for (int columnsIndex = 0; columnsIndex < columnsCount; columnsIndex++)
                        {
                            if (oDataRow.IsNull(columnsIndex) && (strTypes[columnsIndex].Equals("int") || strTypes[columnsIndex].Equals("float")))
                            {
                                Logger.Error("{0} 表第{1}行, 第{2}列数据为空。注意：int或float 字段下的数据不能为空", oDataTable.TableName, rowIndex + 1, columnsIndex + 1);
                            }
                        }
                    }
                }
            }
            return isPass;
        }
    }
}
