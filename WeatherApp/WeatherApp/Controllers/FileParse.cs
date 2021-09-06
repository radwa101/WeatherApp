using DemoDatabase;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace WeatherApp.Controllers
{
    public class FileParse
    {
        public int Id { get; set; }
        public string AbsolutePath { get; set; }
        public int EncodingCodePage { get; set; }
        public string FileHash { get; set; }
        public DateTime ParseDate { get; set; }
        public int? Phase { get; set; }
        public DateTime? SystemDate { get; set; }
        public string TableName { get; set; }

        public FileParse()
        {
            
        }

        public int Parse(string absolutePath, FileParseWriterSQL fileParseWriter)
        {
            try
            {
                using (c227_FileProcessorEntities context1 = new c227_FileProcessorEntities())
                {
                    context1.Set<ft_transaction>().AsNoTracking();
                    var transactions = context1.ft_transaction.AsNoTracking().ToList();
                    ft_transaction transaction = transactions.SingleOrDefault(x => x.TransactionId == 123);
                    
                    
                    if (transaction != null)
                    {
                        writeTable(absolutePath, fileParseWriter, transaction);
                    }
                    else
                    {
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return this.Id;
        }

        private void writeTable(string absolutePath, FileParseWriterSQL fileParseWriter, ft_transaction transaction)
        {
            //write to the store
            if (!fileParseWriter.WriteToStore(absolutePath, transaction))
            {

            }
        }

        private string ToNullableString(object value)
        {
            return (value != null) ? value.ToString() : "null";
        }
    }
}