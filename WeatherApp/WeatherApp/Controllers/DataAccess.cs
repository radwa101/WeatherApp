using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace WeatherApp.Controllers
{
    public class DataAccess
    {
        private string ConnectionString;

        public dynamic GetConnection()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["FileProcessor"].ConnectionString;
            return new SqlConnection(ConnectionString);
        }

        public void InsertDynamic(dynamic connection, dynamic transaction, string tableName, int fileParseId, string distribution, List<Dictionary<string, object>> rows)
        {
            if (rows.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"INSERT INTO [dbo].[{tableName}]");
                sb.Append($"([FileParseId]");
                sb.Append($",[Distribution]");

                foreach (KeyValuePair<string, object> column in rows[0])
                {
                    sb.Append($",[{column.Key}]");
                }

                sb.Append($")VALUES");

                foreach (Dictionary<string, object> row in rows)
                {
                    sb.Append($"({fileParseId}");
                    sb.Append($",'{distribution.PadRight(2, ' ')}'");

                    foreach (KeyValuePair<string, object> column in row)
                    {
                        if (column.Value != null)
                        {
                            switch (Type.GetTypeCode(column.Value.GetType()))
                            {
                                case TypeCode.Decimal:
                                case TypeCode.Double:
                                case TypeCode.Int16:
                                case TypeCode.Int32:
                                case TypeCode.Int64:
                                case TypeCode.SByte:
                                case TypeCode.Single:
                                    {
                                        sb.Append($",{column.Value.ToString()}");
                                        break;
                                    }
                                case TypeCode.DateTime:
                                    {
                                        DateTime date = Convert.ToDateTime(column.Value);
                                        sb.Append($",'{date.Year.ToString().PadLeft(4, '0')}{date.Month.ToString().PadLeft(2, '0')}{date.Day.ToString().PadLeft(2, '0')}'");
                                        break;
                                    }
                                default:
                                    {
                                        sb.Append($",'{column.Value.ToString().Replace("'", "''")}'");
                                        break;
                                    }
                            }
                        }
                        else
                        {
                            sb.Append($",null");
                        }
                    }

                    sb.Append($"),");
                }

                //remove last comma
                string sqlStatement = sb.ToString();
                sqlStatement = sqlStatement.Substring(0, sqlStatement.Length - 1);

                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.CommandType = CommandType.Text;
                command.Transaction = transaction;
                command.ExecuteScalar();
            }
        }

        public void CreateTable(string tableName, List<Tuple<string, string, int, int?>> columns, bool xmlFile, string indexColumns)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"CREATE TABLE [dbo].[{tableName}]");
            sb.Append($"([RecordId] INT IDENTITY(1, 1) NOT NULL PRIMARY KEY");
            sb.Append($",[FileParseId] INT NOT NULL");
            sb.Append($",[Distribution] CHAR(2) NOT NULL");

            if (xmlFile)
            {
                foreach (Tuple<string, string, int, int?> column in columns)
                {
                    sb.Append(column.Item1);
                }
            }
            else
            {
                foreach (Tuple<string, string, int, int?> column in columns)
                {
                    string columnName = column.Item1;
                    string columnType = column.Item2;
                    int columnSize = column.Item3;
                    int? columnDecimals = column.Item4;
                    sb.Append(formatSQLtype(columnName, columnType, columnSize, columnDecimals));
                }
            }

            sb.Append($")");

            CreateNewDataTable(ConnectionString, sb.ToString());
            //CreateNewDataTable(ConnectionString, $"CREATE INDEX [NC_{tableName}_FileParseId] ON [dbo].[{tableName}] ({indexColumns})");
        }

        public void InsertDynamicWithColumns(dynamic connection, dynamic transaction, string tableName, int fileParseId, string distribution, List<Dictionary<string, object>> rows, List<string> cols = null)
        {
            if (rows.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                sb.Append($"INSERT INTO [dbo].[{tableName}]");
                sb.Append($"([FileParseId]");
                sb.Append($",[Distribution]");

                foreach (string col in cols)
                {
                    sb.Append($",[{col}]");
                }
                sb.Append($")VALUES");

                foreach (Dictionary<string, object> row in rows)
                {
                    sb.Append($"({fileParseId}");
                    sb.Append($",'{distribution.PadRight(2, ' ')}'");

                    foreach (string col in cols)
                    {
                        var columnVal = row.SingleOrDefault(r => r.Key == col).Value;

                        if (columnVal != null)
                        {
                            switch (Type.GetTypeCode(columnVal.GetType()))
                            {
                                case TypeCode.Decimal:
                                case TypeCode.Double:
                                case TypeCode.Int16:
                                case TypeCode.Int32:
                                case TypeCode.Int64:
                                case TypeCode.SByte:
                                case TypeCode.Single:
                                    {
                                        sb.Append($",{columnVal.ToString()}");
                                        break;
                                    }
                                case TypeCode.DateTime:
                                    {
                                        DateTime date = Convert.ToDateTime(columnVal);
                                        sb.Append($",'{date.Year.ToString().PadLeft(4, '0')}{date.Month.ToString().PadLeft(2, '0')}{date.Day.ToString().PadLeft(2, '0')}'");
                                        break;
                                    }
                                default:
                                    {
                                        sb.Append($",'{columnVal.ToString().Replace("'", "''")}'");
                                        break;
                                    }
                            }
                        }
                        else
                        {
                            sb.Append($",null");
                        }
                    }
                    sb.Append($"),");
                }

                //remove last comma
                string sqlStatement = sb.ToString();
                sqlStatement = sqlStatement.Substring(0, sqlStatement.Length - 1);

                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.CommandType = CommandType.Text;
                command.Transaction = transaction;
                command.ExecuteScalar();
            }
        }

        private string formatSQLtype(string name, string type, int size, int? decimals)
        {
            string result = string.Empty;

            switch (type.ToLower())
            {
                case "string":
                    {
                        result = $",[{name}] varchar({size})";
                        break;
                    }
                case "int":
                case "int16":
                case "int32":
                case "int64":
                    {
                        result = $",[{name}] int";
                        break;
                    }
                case "double":
                    {
                        result = $",[{name}] float";
                        break;
                    }
                case "datetime":
                case "date":
                    {
                        result = $",[{name}] date";
                        break;
                    }
                case "decimal":
                    {
                        int decimalPlaces = (decimals == null) ? 0 : (int)decimals;
                        result = $",[{name}] decimal({size},{decimalPlaces})";
                        break;
                    }
                case "xml":
                    {
                        result = $",[{name}] xml";
                        break;
                    }
            }

            return result;
        }

        public static void CreateNewDataTable(string connectionString, string commandString)
        {
            using (SqlConnection conn1 = new SqlConnection(connectionString))
            {
                using (SqlCommand comm1 = new SqlCommand())
                {
                    comm1.Connection = conn1;
                    comm1.CommandText = commandString;
                    conn1.Open();
                    comm1.ExecuteNonQuery();
                    conn1.Close();
                }
            }
        }
    }
}