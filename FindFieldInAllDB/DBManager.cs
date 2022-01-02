using FindFieldInAllDB.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindFieldInAllDB
{
    internal class DBManager
    {
 
        public static List<string> AllTableName = new List<string>();
          
        static string FileAllQuerySQL = "数据库表结构.json";

        public static List<string> GetAllTable()
        {
            //强森排除 xyz
            DataTable table = SQLHelper.GetTable("SELECT name   FROM SYSOBJECTS WHERE TYPE='U' and name not like 'xt_%' and name not like 'zt_%' and name not like 'yt_%'");
            AllTableName = table.AsEnumerable().Select(K => K[0].ToString()).ToList();
            //AllTableName = AllTableName.Where(K => !K.StartsWith("XT_", StringComparison.CurrentCultureIgnoreCase) && !K.StartsWith("XT_", StringComparison.CurrentCultureIgnoreCase) && !K.StartsWith("XT_", StringComparison.CurrentCultureIgnoreCase)).ToList();
            return AllTableName;
        }

        //public static bool IsEmptyTable(string tableName)
        //{
        //    string sql = $" exec sp_spaceused   " + tableName;
        //    DataTable table = SQLHelper.GetTable(sql);
        //    string rows = table.Rows[0]["rows"].ToString().Trim();
        //    return rows == "0";
        //}

        public static List<SQLModel> GetQuerySQL() 
        {
            if (File.Exists(FileAllQuerySQL))
            {
                string json=  File.ReadAllText(FileAllQuerySQL);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<SQLModel>>(json);
            }

            List<SQLModel> AllQuerySql = new List<SQLModel>();

            List<string> allTable = GetAllTable();
            for (int i = 0; i < allTable.Count; i++)
            {
                AllQuerySql.AddRange(GetAllFilterSQL(allTable[i]));
            }

            File.WriteAllText(FileAllQuerySQL, Newtonsoft.Json.JsonConvert.SerializeObject(AllQuerySql));

            return AllQuerySql; 
        }
         
        public static List<SQLModel> GetAllFilterSQL(string tableName)
        { 
            List<SQLModel> Result = new List<SQLModel>();
            string sql = "select top 1 * from " + tableName;
            DataTable table =  SQLHelper.GetTable(sql);
            if (table.Rows.Count == 0)
            {
                return Result;
            }

            //35 和 99 都是text 类型，不能用 sql的等号查询
           DataTable colDT = SQLHelper.GetTable("Select name from syscolumns Where ID=OBJECT_ID('" + tableName + "') and (xtype=35 or xtype=99)");
           string[] allText= colDT.AsEnumerable().Select(K => K[0].ToString()).ToArray();
            for (int i = 0; i < table.Columns.Count; i++)
            {
                string ColumnName = table.Columns[i].ColumnName; 
                string types = table.Columns[i].DataType.Name;
                switch (types)
                {
                    case "DateTime":
                    case "Int32":
                    case "Decimal":
                    case "Double":
                    case "Int16":
                    case "Int64":
                    case "Byte":
                    case "Byte[]":
                    case "Boolean":
                        continue;
                    case "String":
                        break;
                    default:
                        Console.WriteLine(types);
                        break;
                }

                if (allText.Contains(ColumnName))
                {
                    types = "text";
                }

                Result.Add(new SQLModel() { TableName = tableName, ColumnName = ColumnName, DataType = types });


                
                  
            }
            return Result; 
        } 
    }



}
