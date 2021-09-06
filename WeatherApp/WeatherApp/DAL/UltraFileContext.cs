using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WeatherApp.ViewModels;

namespace WeatherApp.DAL
{
    public partial class UltraFileContext : DbContext
    {
        public UltraFileContext()
            : base("name=UltraFileContext")
        {

        }

        public virtual DbSet<Document> UltraFileItems { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<UltraFileContext>(null);
            modelBuilder.Entity<Document>().ToTable("dbo.UltraFileItem");
        }
    }
}