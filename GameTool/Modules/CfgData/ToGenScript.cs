using GameTool.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTool.Modules.CfgData
{
    public class ToGenScript
    {

        //public class StoryMgr : TplMgr
        //{
        //}

        //public class StoryData : BaseTpl
        //{
        //    /// <summary>
        //    /// sss
        //    /// </summary>
        //    public string Name { get; private set; }

        //    public override void Deserialization(DynamicBuffer buffer)
        //    {
        //        ID = buffer.ReadInt32();
        //    }
        //}


        public string StrTMPMgr = "\r\npublic class {0}Mgr : TplMgr __LK____RK__";
        public string StrTMPData = "public class {0}Data : BaseTpl\r\n__LK__\r\n{1}\r\npublic override void Deserialization(DynamicBuffer buffer)\r\n__LK__\r\n{2}__RK__\r\n__RK__";

        public string Gen(List<DataTable> dataTables)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("using ZCSharpLib.Features.NetWork;\r\nusing ZCSharpLib.Features.Tmplate;\r\n");

            for (int tableIndex = 0; tableIndex < dataTables.Count; tableIndex++)
            {
                DataTable oDataTable = dataTables[tableIndex];
                string oTableName = oDataTable.TableName;
                int rowCount = oDataTable.Rows.Count;
                int columnsCount = oDataTable.Columns.Count;

                // 获取首行数据类型
                string[] sTypes = new string[columnsCount];
                {
                    DataRow oDataRow = oDataTable.Rows[0];
                    for (int columnsIndex = 0; columnsIndex < sTypes.Length; columnsIndex++)
                    {
                        string strType = oDataRow[columnsIndex].ToString();
                        sTypes[columnsIndex] = strType;
                    }
                }
                // 获取第二行字段名称
                string[] sFiledNames = new string[columnsCount];
                {
                    DataRow oDataRow = oDataTable.Rows[1];
                    for (int columnsIndex = 0; columnsIndex < sFiledNames.Length; columnsIndex++)
                    {
                        string fileName = oDataRow[columnsIndex].ToString();
                        sFiledNames[columnsIndex] = fileName;
                    }
                }
                // 获取第三行注释
                string[] sComments = new string[columnsCount];
                {
                    DataRow oDataRow = oDataTable.Rows[2];
                    for (int columnsIndex = 0; columnsIndex < sComments.Length; columnsIndex++)
                    {
                        string comment = oDataRow[columnsIndex].ToString();
                        sComments[columnsIndex] = comment;
                    }
                }

                sb.Append(string.Format(StrTMPMgr, oTableName));
                sb.Append("\r\n");
                StringBuilder filedSB = new StringBuilder();
                StringBuilder methodSB = new StringBuilder();
                for (int i = 0; i < sTypes.Length; i++)
                {
                    string sType = sTypes[i];
                    string sFiledName = sFiledNames[i];
                    string sComment = sComments[i];

                    if (i > 0)
                    {
                        // 跳过ID字段
                        sComment = sComment.Replace("\r\n", "");
                        sComment = sComment.Replace("\n", "");
                        string combineComment = string.Format("/// <summary>\r\n/// {0}\r\n/// <summary>", sComment);
                        filedSB.Append(string.Format("{0}\r\npublic {1} {2} __LK__get; private set; __RK__", combineComment, sType, sFiledName));
                        filedSB.Append("\r\n");
                    }

                    methodSB.Append(string.Format("{0}=buffer.{1};", sFiledName, GetMethod(sType)));
                    methodSB.Append("\r\n");
                }
                sb.Append(string.Format(StrTMPData, oTableName, filedSB, methodSB));
            }
            string final = sb.ToString();
            final = final.Replace("__LK__", "{");
            final = final.Replace("__RK__", "}");
            return final;
        }

        private string GetMethod(string sType)
        {
            string sMethod = string.Empty;
            if (sType.Equals("int")) sMethod = "ReadInt32()";
            else if (sType.Equals("float")) sMethod = "ReadFloat()";
            else if (sType.Equals("string")) sMethod = "ReadUTF8()";
            return sMethod;
        }
    }
}
