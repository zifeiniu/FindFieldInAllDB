using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindFieldInAllDB.Model
{
    public class SQLModel
    {
        public string TableName;

        public string ColumnName;

        /// <summary>
        /// 是否是空表
        /// </summary>
        public bool IsEmpty;


        /// <summary>
        /// 数据类型（只要 String 类型和 Text 类型）
        /// Text 类型不能用 = 查询
        /// </summary>
        public string DataType;

        public string GetSQL(string input) 
        {
            if (DataType == "Text")
            {
                //return $"select top 1 * from {TableName} where [{ColumnName}]  '{input}'";
            }
            return $"select top 1 * from {TableName} where [{ColumnName}] = '{input}'";
        }

    }
}
