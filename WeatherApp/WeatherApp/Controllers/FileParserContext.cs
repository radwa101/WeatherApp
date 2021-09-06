using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherApp.Controllers
{
    public class FileParserContext : IDisposable
    {
        public FileParse Parser { get; private set; }
        public string FileParserConnectionString { get; set; }
        public string FileProcessorConnectionString { get; set; }

        bool disposed = false;

        public FileParserContext()
        {
            Parser = new FileParse();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    disposed = true;
                }
            }
        }

        //State Engine to pass in 'parseTables' list
        public void Parse()
        {
            try
            {
                int parseId = this.Parser.Parse(@"C:\TDM\IVAS1.dat", new FileParseWriterSQL());
            }
            catch (Exception ex)
            {
            }
        }
    }
}