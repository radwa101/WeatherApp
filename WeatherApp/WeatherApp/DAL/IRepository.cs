using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeatherApp.ViewModels;

namespace WeatherApp.DAL
{
    public interface IRepository
    {
        int FileItemID { get; set; }
        Document GetFileItemById(int id, int id1);
        IEnumerable<Document> GetAllUltraFileItems();
    }
}