using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeatherApp.Controllers;

namespace WeatherApp.Controllers
{
    public class ExecutionEngine
    {
        public ExecutionEngine()
        {
        }

        public void Validate()
        {
            try
            {
                FileParserContext parser = new FileParserContext();
                parser.Parse();
            }
            catch (Exception ex)
            {

            }
        }
    }
}

