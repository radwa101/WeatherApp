using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WeatherApp.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool? Active { get; set; }

        public bool DisplayActive
        {
            get
            {
                return (Active.HasValue && Active.Value == true) ? Active.Value : false;
            }
        }
    }
}