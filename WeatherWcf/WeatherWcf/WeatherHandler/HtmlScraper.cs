using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using WeatherWcf.Models;

namespace WeatherWcf.WeatherHandler
{
    public class HtmlScraper : IScraper
    {
        public List<Location> GetCities(string htmlContent)
        {
            List<Location> cities = new List<Location>();
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlContent);
            htmlDoc.DocumentNode.Descendants().Where(n => n.Name == "script").ToList().ForEach(n => n.Remove());
            var tableNode = htmlDoc.DocumentNode.SelectNodes("//table")[0];
            foreach (var trNode in tableNode.SelectNodes("tr"))
            {
                if (!string.IsNullOrEmpty(trNode.InnerHtml))
                {
                    if (!string.IsNullOrEmpty(trNode.SelectNodes("td")[0].InnerHtml))
                    {
                        int temperature;
                        int.TryParse(trNode.SelectNodes("td")[3].InnerHtml.Replace("&nbsp;Â°C", ""), out temperature);
                        string city = HttpUtility.HtmlDecode(trNode.SelectNodes("td")[0].InnerText);
                        string country = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(trNode.SelectNodes("td")[0].FirstChild.Attributes["href"].Value.Split('/')[2]);
                        cities.Add(new Location()
                        {
                            City = city,
                            Country = country,
                            LocationDisplayed = city + " (" + country + ")",
                            DateTime = trNode.SelectNodes("td")[1].InnerHtml,
                            Icon = trNode.SelectNodes("td")[2].FirstChild.Attributes["src"].Value,
                            Temperature = temperature,
                            TemperatureDisplay = trNode.SelectNodes("td")[3].InnerHtml.Replace("&nbsp;Â°C", "") + " °C",
                            ForecastDetailsUrl = trNode.SelectNodes("td")[0].FirstChild.Attributes["href"].Value
                    });
                    }
                    if (!string.IsNullOrEmpty(trNode.SelectNodes("td")[4].InnerHtml))
                    {
                        int temperature;
                        int.TryParse(trNode.SelectNodes("td")[7].InnerHtml.Replace("&nbsp;Â°C", ""), out temperature);
                        string city = HttpUtility.HtmlDecode(trNode.SelectNodes("td")[4].InnerText);
                        string country = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(trNode.SelectNodes("td")[4].FirstChild.Attributes["href"].Value.Split('/')[2]);
                        cities.Add(new Location()
                        {
                            City = city,
                            Country = country,
                            LocationDisplayed = city + " (" + country + ")",
                            DateTime = trNode.SelectNodes("td")[5].InnerHtml,
                            Icon = trNode.SelectNodes("td")[6].FirstChild.Attributes["src"].Value,
                            Temperature = temperature,
                            TemperatureDisplay = trNode.SelectNodes("td")[7].InnerHtml.Replace("&nbsp;Â°C", "") + " °C",
                            ForecastDetailsUrl = trNode.SelectNodes("td")[4].FirstChild.Attributes["href"].Value
                        });
                    }
                    if (trNode.SelectNodes("td").Count > 8)
                    {
                        if (!string.IsNullOrEmpty(trNode.SelectNodes("td")[8].InnerHtml))
                        {
                            int temperature;
                            int.TryParse(trNode.SelectNodes("td")[11].InnerHtml.Replace("&nbsp;Â°C", ""), out temperature);
                            string city = HttpUtility.HtmlDecode(trNode.SelectNodes("td")[8].InnerText);
                            string country = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(trNode.SelectNodes("td")[8].FirstChild.Attributes["href"].Value.Split('/')[2]);
                            cities.Add(new Location()
                            {
                                City = city,
                                Country = country,
                                LocationDisplayed = city + " (" + country + ")",
                                DateTime = trNode.SelectNodes("td")[9].InnerHtml,
                                Icon = trNode.SelectNodes("td")[10].FirstChild.Attributes["src"].Value,
                                Temperature = temperature,
                                TemperatureDisplay = trNode.SelectNodes("td")[11].InnerHtml.Replace("&nbsp;Â°C", "") + " °C",
                                ForecastDetailsUrl = trNode.SelectNodes("td")[8].FirstChild.Attributes["href"].Value
                            });
                        }
                    }
                }
            }
            return cities.OrderByDescending(c => c.Temperature).ToList();
        }

        public string GetForecastData(string htmlContent, string filter)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlContent);
            htmlDoc.DocumentNode.Descendants().Where(n => n.Name == "script").ToList().ForEach(n => n.Remove());
            var tableNode = htmlDoc.DocumentNode.SelectNodes(".//table[@id='" + filter + "']")[0];
            return tableNode.OuterHtml;
        }
    }
}