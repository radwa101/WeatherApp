using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WeatherApp.ViewModels
{
    public class FileDetails
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter your name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter file type")]
        public string FileType { get; set; }

        [Required(ErrorMessage = "Please enter file type")]
        public bool FileType1 { get; set; }

        [Required(ErrorMessage = "Please enter a description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter some information")]
        public string Information { get; set; }

        public byte[] Image { get; set; }

        public List<string> Options = new List<string>() { "One", "Two", "Three" };

        public string SelectedOptions { get; set; }
    }
}