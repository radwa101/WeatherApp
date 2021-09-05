using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WeatherApi.WeatherHandler;

namespace WeatherApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExtremesController : ControllerBase
    {
        private IConfiguration _configuration { get; set; }
        private IWeatherReader _weatherReader { get; set; }
        private IScraper _htmlScraper { get; set; }

        public ExtremesController(IConfiguration configuration, IWeatherReader weatherReader, IScraper htmlScraper)
        {
            _configuration = configuration;
            _weatherReader = weatherReader;
            _htmlScraper = htmlScraper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Location>> Get(string region = "World", string filter = "margin-top:5px; width: 800px;", bool isTemp = true)
        {
            string htmlContents = _weatherReader.Download(_configuration["WeatherEndpoints:HighestLowestUrl"] + region.Replace("+", "%2B"));
            return _htmlScraper.GetExtremes(htmlContents, filter, isTemp);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
