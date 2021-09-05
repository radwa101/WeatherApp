using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherService.Models
{
    [Table("tblEmployee")]
    public class Employee
    {
        public Employee()
        {
        }

        [Column("Id")]
        public int Id { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("Gender")]
        public string Gender { get; set; }

        [Column("DateOfBirth")]
        public DateTime DateOfBirth { get; set; }
    }
}
