using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeatherApp.ViewModels;

namespace WeatherApp.DAL
{
    public class Repository : IRepository
    {
        protected readonly UltraFileContext _context = new UltraFileContext();

        public int FileItemID { get; set; }

        public Repository()
        { }

        public Document GetFileItemById(int id, int id1)
        {
            var query = GetAllUltraFileItems().FirstOrDefault(x => x.Id == id);
            return query;
        }

        public IEnumerable<Document> GetAllUltraFileItems()
        {
            var ultraFileItems = new List<Document>();
            try
            {
                ultraFileItems = _context.UltraFileItems.ToList();
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
            }
            return ultraFileItems;
        }

    }
}