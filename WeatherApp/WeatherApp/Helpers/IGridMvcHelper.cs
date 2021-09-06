using Grid.Mvc.Ajax.GridExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WeatherApp.Helpers
{
    public interface IGridMvcHelper
    {
        AjaxGrid<T> GetAjaxGrid<T>(IOrderedQueryable<T> items) where T : class;
        AjaxGrid<T> GetAjaxGrid<T>(IOrderedQueryable<T> items, int? page) where T : class;
        object GetGridJsonData<T>(AjaxGrid<T> grid, string gridPartialViewPath, Controller controller) where T : class;
    }
}
