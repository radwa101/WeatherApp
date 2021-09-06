using DemoDatabase;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace WeatherApp.Controllers
{
    public class FileParseWriterSQL
    {
        private DataAccess dataAccess;
        

        public FileParseWriterSQL()
        {
            dataAccess = new DataAccess();
        }

        public bool WriteToStore(string absolutePath, ft_transaction transaction)
        {
                //multi record file
                return WriteMultiRecordType(absolutePath, Encoding.UTF8, string.Empty, transaction);
        }

        private bool WriteMltRecordType(string absolutePath, string distribution, ft_transaction transaction)
        {
            bool result = false;

            string tableName = transaction.Description;
            List<frd_record_definition> columns = new List<frd_record_definition>();

            List<ftd_transaction_definition> transactionDefinitions = transaction.ftd_transaction_definition.OrderBy(t => t.Sequence).ToList();
            foreach (ftd_transaction_definition transactionDefinition in transactionDefinitions)
            {
                List<frd_record_definition> recordDefinitions = transactionDefinition.fr_record.frd_record_definition.OrderBy(r => r.Sequence).ToList();
                recordDefinitions.ForEach(r => columns.Add(r));
            }

            //version and/or create target table if required
            //validateTargetTable(tableName, false, DateTime.Now, columns);

            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            //start SQL Transaction
            SqlConnection connection = dataAccess.GetConnection();
            connection.Open();
            SqlTransaction tran = connection.BeginTransaction();

            //blank or non-numeric distribution implies '01'
            if (distribution == string.Empty | !int.TryParse(distribution, out int d))
            {
                distribution = "01";
            }

            //distribution is defined as char(2)
            distribution = distribution.PadRight(2, ' ');

            //get all identifiers for this transaction
            List<string> identifiers = new List<string>();

            foreach (ftd_transaction_definition transactionDefinition in transactionDefinitions)
            {
                identifiers.Add(transactionDefinition.fr_record.Identifier);
            }

            List<Tuple<string, string, int, int?>> colDefs = new List<Tuple<string, string, int, int?>>();
            foreach (frd_record_definition column in columns)
            {
                colDefs.Add(new Tuple<string, string, int, int?>(column.Name, column.ed_element_definition.DataType, column.Length, column.DecimalPlaces));
            }
            dataAccess.CreateTable(tableName, colDefs, false, string.Empty);

            try
            {
                int size = (int)transaction.ftd_transaction_definition.FirstOrDefault().fr_record.RecordLength;
                using (StreamReader sr = new StreamReader(File.OpenRead(absolutePath), Encoding.UTF8, false, size))
                {
                    //calculate number of records to read
                    //int recordCount = 0;
                    //long fileLength = sr.BaseStream.Length;
                    //int totalHeaderSize = (transaction.HeaderLength ?? 0) * (transaction.HeaderCount ?? 1);
                    //int totalTrailerSize = (transaction.TrailerLength ?? 0) * (transaction.TrailerCount ?? 1);

                    //long recordBytes = fileLength - (totalHeaderSize + totalTrailerSize);
                    //if (recordBytes > 0)
                    //{
                    //    recordCount = (int)Math.Ceiling((double)recordBytes / size);
                    //}

                    //bool hasTrailer = totalTrailerSize != 0;
                    int recordLine = 1;
                    string rowIdentifier = string.Empty;

                    //skip header
                    //readNext(sr, totalHeaderSize);

                    //for (int x = 0; x < recordCount; x++)
                    //{
                    string line = string.Empty;
                    while ((line = sr.ReadLine()) != null)
                    {
                        //size = (x == recordCount-1) ? size-2 : size; 

                        //string recordString = readNext(sr, size);
                        //if (recordString.Length != size)
                        //{
                        //    break;
                        //}

                        //string lineIdentifier = GetMltIdentifier(recordString, identifiers);
                        string lineIdentifier = GetMltIdentifier(line, identifiers);
                        if (string.IsNullOrEmpty(lineIdentifier))
                        {
                            recordLine++;
                        }
                        else
                        {
                            rowIdentifier = lineIdentifier;
                            recordLine = 1;
                        }

                        ftd_transaction_definition transDef = transactionDefinitions.Where(t => t.fr_record.Identifier == rowIdentifier).FirstOrDefault();
                        if (transDef != null)
                        {
                            fr_record record = transDef.fr_record;

                            //check for new transaction
                            if (rowIdentifier == identifiers[0] && row.Keys.Count > 0)
                            {
                                rows.Add(row);
                                row = new Dictionary<string, object>();
                            }

                            //Dictionary<string, object> rowValues = GetMltRowValues(recordString, record, recordLine);
                            Dictionary<string, object> rowValues = GetMltRowValues(line, record, recordLine);
                            foreach (KeyValuePair<string, object> rowValue in rowValues)
                            {
                                row.Add(rowValue.Key, rowValue.Value);
                            }
                        }

                        if (rows.Count == 20)
                        {
                            dataAccess.InsertDynamicWithColumns(connection, tran, tableName, 0, distribution, rows, columns.Select(c => c.Name).ToList());
                            rows.Clear();
                        }
                    }
                }

                if (row.Keys.Count > 0)
                {
                    rows.Add(row);
                }
                if (rows.Count > 0)
                {
                    dataAccess.InsertDynamicWithColumns(connection, tran, tableName, 0, distribution, rows, columns.Select(c => c.Name).ToList());
                }

                tran.Commit();
                result = true;
            }

            catch (Exception ex)
            {
                tran.Rollback();
            }
            finally
            {
                connection.Close();
            }

            return result;
        }

        private string GetColumnSeparator(fr_record record)
        {
            string columnSeparator = null;

            if (record != null)
            {
                switch (record.Template.ToLower())
                {
                    case "template":
                        {
                            columnSeparator = null;
                            break;
                        }
                    case "f_csv":
                    case "csv":
                        {
                            columnSeparator = ",";
                            break;
                        }
                    case "ssv": // semi-colon separated
                        {
                            columnSeparator = ";";
                            break;
                        }
                    default:
                        {
                            columnSeparator = null;
                            break;
                        }
                }
            }
            return columnSeparator;
        }

        //private void validateTargetTable(string tableName, bool xmlFile, DateTime fileDefLastUpdated, List<frd_record_definition> columns)
        //{
        //    //version table if file def changed since the table was created
        //    if (dataAccess.TableExists(tableName))
        //    {
        //        //get table created date and compare to definition changed date
        //        DateTime tableCreatedDate = dataAccess.GetTableCreatedDate(tableName);
        //        Logger.GetLogger().WriteDebug($"Table:'{tableName}' created:{tableCreatedDate.ToString("dd/MM/yyyy hh:mm")} definition last changed:{fileDefLastUpdated.ToString("dd/MM/yyyy hh:mm")}");

        //        if (tableCreatedDate <= fileDefLastUpdated)
        //        {
        //            dataAccess.VersionTable(tableName, xmlFile);
        //        }
        //    }

        //    //re-check because table may have been versioned - create table if does not exist
        //    if (!dataAccess.TableExists(tableName))
        //    {
        //        createTable(tableName, xmlFile, columns, dataAccess);
        //    }
        //}


        private bool WriteMultiRecordType(string absolutePath, Encoding encoding, string distribution, ft_transaction transaction)
        {
            bool result = false;

            //handle multi line tagged file
            if (string.Equals(transaction.ftd_transaction_definition.FirstOrDefault().fr_record.Template, "mlt", StringComparison.OrdinalIgnoreCase))
            {
                result = WriteMltRecordType(absolutePath, string.Empty, transaction);
            }
            else
            {
                throw new NotImplementedException("Multi-transactioned based files not implemented yet.");
            }
            return result;
        }

        private void createTable(string tableName, bool xmlFile, List<frd_record_definition> columns)
        {
            List<Tuple<string, string, int, int?>> colDefs = new List<Tuple<string, string, int, int?>>();

            string indexColumns = "[FileParseId]";
            
            foreach (frd_record_definition column in columns)
            {
                colDefs.Add(new Tuple<string, string, int, int?>(column.Name, column.ed_element_definition.DataType, column.Length, column.DecimalPlaces));
            }

            //dataAccess.CreateTable(tableName, colDefs, xmlFile, indexColumns);
        }

        private string getIdentifier(string recordString, List<Tuple<string, string, string, int, int, int?>> columnPositions)

        {

            string identifier = null;



            // identifier column can be either 'Identifier' or 'RecordType'

            Tuple<string, string, string, int, int, int?> column = columnPositions.Where(c => c.Item1 == "Identifier").FirstOrDefault();

            if (column == null)

            {

                column = columnPositions.Where(c => c.Item1 == "RecordType").FirstOrDefault();

            }



            if (column != null && recordString.Length >= column.Item4 + column.Item5)

            {

                identifier = recordString.Substring(column.Item4, column.Item5).Trim();

            }



            return identifier;

        }

        private string GetMltIdentifier(string recordString, List<string> identifiers)
        {
            string identifier = null;

            foreach (string recIdentifier in identifiers)
            {
                if (recordString.Substring(29, 1).Contains(recIdentifier))
                {
                    identifier = recIdentifier;
                    break;
                }
            }
            return identifier;
        }

        private Dictionary<string, object> getRowValues(string recordString, List<Tuple<string, string, string, int, int, int?>> columnPositions)
        {
            Dictionary<string, object> colValues = new Dictionary<string, object>();
            foreach (Tuple<string, string, string, int, int, int?> column in columnPositions)
            {
                string columnValue = recordString.Substring(column.Item4, column.Item5).Trim();
                colValues.Add(column.Item1, createObject(columnValue, column.Item2, column.Item3, column.Item6));
            }

            return colValues;
        }

        private Dictionary<string, object> GetMltRowValues(string recordString, fr_record record, int recordLine)
        {
            List<frd_record_definition> recordDefinitions = record.frd_record_definition.Where(r => r.Line == recordLine).OrderBy(r => r.Sequence).ToList();

            int pos = 0;
            Dictionary<string, object> colValues = new Dictionary<string, object>();
            foreach (frd_record_definition recordDefinition in recordDefinitions)
            {
                if (pos + recordDefinition.Length <= recordString.Length)
                {
                    string columnValue = recordString.Substring(pos, recordDefinition.Length).Trim();
                    colValues.Add(recordDefinition.Name, createObject(columnValue, recordDefinition.ed_element_definition.DataType, recordDefinition.ed_element_definition.SourceFormatString, recordDefinition.DecimalPlaces));
                }

                pos += recordDefinition.Length;
            }
            return colValues;
        }

        private object createObject(string value, string type, string formatString, int? decimals)
        {
            switch (type.ToLower())
            {
                case "date":
                    {
                        if (string.IsNullOrEmpty(formatString))
                        {
                            formatString = "yyyyMMdd";
                        }

                        if (!string.IsNullOrEmpty(value) && value != new string('0', formatString.Length) && !string.Equals(value, "00/00/0000")) // return null for all zeros
                        {
                            DateTime date = DateTime.MinValue;
                            if (DateTime.TryParseExact(value, formatString, new System.Globalization.CultureInfo("en-US"), System.Globalization.DateTimeStyles.None, out date))
                            {
                                return date;
                            }
                            else
                            {
                            }
                        }
                        return null;
                    }
                case "decimal":
                    {
                        if (decimals != null && decimals > 0)
                        {
                            //pad string with leading zeros if required
                            if (value.Length <= (int)decimals)
                            {
                                value = value.PadLeft((int)decimals + 1, '0');
                            }

                            //insert decimal place if it is not already there
                            if (value.Substring(value.Length - (int)decimals - 1, 1) != ".")
                            {
                                value = value.Insert(value.Length - (int)decimals, ".");
                            }
                        }

                        value = value.Trim();
                        if (value != string.Empty)
                        {
                            if (decimal.TryParse(value, out decimal result))
                            {
                                return result;
                            }
                            else
                            {
                            }
                        }
                        return 0;
                    }
                case "int":
                case "int32":
                    {
                        value = value.Trim();
                        if (value != string.Empty)
                        {
                            if (Int32.TryParse(value, out Int32 result))
                            {
                                return result;
                            }
                            else
                            {
                            }
                        }
                        return 0;
                    }
                case "int16":
                    {
                        value = value.Trim();
                        if (value != string.Empty)
                        {
                            if (Int16.TryParse(value, out Int16 result))
                            {
                                return result;
                            }
                            else
                            {
                            }
                        }
                        return 0;
                    }
                case "int64":
                    {
                        value = value.Trim();
                        if (value != string.Empty)
                        {
                            if (Int64.TryParse(value, out Int64 result))
                            {
                                return result;
                            }
                            else
                            {
                            }
                        }
                        return 0;
                    }
                case "double":
                    {
                        value = value.Trim();
                        if (value != string.Empty)
                        {
                            if (double.TryParse(value, out double result))
                            {
                                return result;
                            }
                            else
                            {
                            }
                        }
                        return 0;
                    }
                case "sbyte":
                    {
                        value = value.Trim();
                        if (value != string.Empty)
                        {
                            if (sbyte.TryParse(value, out sbyte result))
                            {
                                return result;
                            }
                            else
                            {
                            }
                        }
                        return 0;
                    }
                case "single":
                    {
                        value = value.Trim();
                        if (value != string.Empty)
                        {
                            if (Single.TryParse(value, out Single result))
                            {
                                return result;
                            }
                            else
                            {
                            }
                        }
                        return 0;
                    }
                default:
                    {
                        return value;
                    }
            }
        }

        private string readNext(StreamReader sr, int size)
        {
            string result = string.Empty;

            char[] characters = new char[size];
            if (sr.Read(characters, 0, size) == size)
            {
                result = new string(characters);
            }
            return result;
        }

        private bool lastRow(StreamReader sr)
        {
            return sr.Peek() == -1;
        }
    }
}