namespace WeatherService.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class UserSettingsContext : DbContext
    {
        public UserSettingsContext()
            : base("name=UserSettingsContext")
        {
        }

        public virtual DbSet<Employee> Employees { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<UserSettingsContext>(null);
            modelBuilder.Entity<Employee>();
        }
    }
}
