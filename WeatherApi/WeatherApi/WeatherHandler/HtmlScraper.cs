using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WeatherApi.WeatherHandler
{
    public class HtmlScraper : IScraper
    {
        public List<Location> GetCities(string htmlContent)
        {
            List<Location> cities = new List<Location>();
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlContent);
            htmlDoc.DocumentNode.Descendants().Where(n => n.Name == "script").ToList().ForEach(n => n.Remove());
            var tableNode = htmlDoc.DocumentNode.SelectNodes("//table/tbody")[0];
            var timeTempRecordedHtml = htmlDoc.DocumentNode.Descendants("div")
                            .Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("hp_title_wrapper")).LastOrDefault().InnerText;
            foreach (var trNode in tableNode.SelectNodes("tr"))
            {
                if (!string.IsNullOrEmpty(trNode.InnerHtml))
                {
                    if (!string.IsNullOrEmpty(trNode.SelectNodes("th")[0].InnerHtml))
                    {
                        int temperature;
                        int.TryParse(trNode.SelectNodes("td")[4].InnerText, out temperature);
                        cities.Add(new Location()
                        {
                            City = HttpUtility.HtmlDecode(trNode.SelectNodes("th")[0].InnerHtml),
                            WindSpeed = new Regex(@"\((\d+)\)").Match(trNode.SelectNodes("td")[1].InnerText).Groups[1].Value + " Km/h",
                            WeatherDescription = trNode.SelectNodes("td")[3].InnerText,
                            Temperature = temperature,
                            DateTime = new Regex(@"FOR(.*)").Match(timeTempRecordedHtml).Groups[1].Value,
                            Humidity = trNode.SelectNodes("td")[5].InnerText + "%",
                            TemperatureDisplay = temperature.ToString() + " °C",
                            Pressure = trNode.SelectNodes("td")[7].InnerText
                        });
                    }
                }
            }
            return cities.OrderByDescending(c => c.Temperature).ToList();
        }

        List<Location> IScraper.GetExtremes(string htmlContent, string filter, bool isTemp)
        {
            int tableIndex = (isTemp) ? 0 : 1;
            List<Location> cities = new List<Location>();
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlContent);
            htmlDoc.DocumentNode.Descendants().Where(n => n.Name == "script").ToList().ForEach(n => n.Remove());
            var rows = htmlDoc.DocumentNode.SelectNodes(".//table[@style=" + filter + "]")[tableIndex]
                            .Descendants("tr")
                            .Where(n => n.Attributes["bgcolor"] != null);
            foreach (var trNode in rows)
            {
                cities.Add(new Location()
                {
                    City = trNode.SelectNodes("td")[1].SelectSingleNode("a").InnerHtml,
                    TemperatureDisplay = HttpUtility.HtmlDecode(trNode.SelectNodes("td")[3].Descendants("strong").FirstOrDefault().InnerText)
                });
            }

            return cities.OrderByDescending(c => c.Temperature).ToList();
        }
    }
}
