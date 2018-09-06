using GameTool.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCSharpLib.Features.NetWork;

namespace GameTool.Modules.CfgData
{
    public class ToGenData
    {
        public byte[] Gen(List<DataTable> dataTables)
        {
            DynamicBuffer buffer = new DynamicBuffer();
            for (int tableIndex = 0; tableIndex < dataTables.Count; tableIndex++)
            {
                DataTable oDataTable = dataTables[tableIndex];
                int rowCount = oDataTable.Rows.Count;
                int realyRowCount = Math.Max((rowCount - 3), 0);

                // 写入行数
                buffer.WriteInt32(realyRowCount); 

                // 如果真实数据是零行则略过
                if (realyRowCount == 0) continue;

                int columnsCount = oDataTable.Columns.Count;
                // 记录首行数据
                string[] strTypes = new string[columnsCount];
                {
                    DataRow oDataRow = oDataTable.Rows[0];
                    for (int columnsIndex = 0; columnsIndex < strTypes.Length; columnsIndex++)
                    {
                        string strType = oDataRow[columnsIndex].ToString();
                        strTypes[columnsIndex] = strType;
                    }
                }
                // 写入实际数据
                for (int rowIndex = 3; rowIndex < rowCount; rowIndex++)
                {
                    DataRow oDataRow = oDataTable.Rows[rowIndex];
                    for (int columnIndex = 0; columnIndex < columnsCount; columnIndex++)
                    {
                        string value = string.Empty;
                        object obj = oDataRow[columnIndex];
                        if (obj != null) value = obj.ToString();

                        string strType = strTypes[columnIndex];
                        if (strType.Equals("int"))
                        {
                            buffer.WriteInt32(int.Parse(value));
                        }
                        else if (strType.Equals("float"))
                        {
                            buffer.WriteFloat(float.Parse(value));
                        }
                        else if (strType.Equals("string"))
                        {
                            buffer.WriteUTF8(value);
                        }
                    }
                }
            }
            return buffer.Buffer;
        }
    }
}
