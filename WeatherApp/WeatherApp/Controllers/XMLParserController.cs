using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WeatherApp.ViewModels;

namespace WeatherApp.Controllers
{
    public class XMLParserController : Controller
    {
        // GET: XMLParser
        public ActionResult Index()
        {
            var mNestedParameters = new Dictionary<string, List<string>>();
            mNestedParameters.Add("Parent - One", new List<string>() { " Child - One", " Child - Two", " Child - Three" });

            XMLParser model = new XMLParser();
            model.NestedParameters = mNestedParameters;

            //string stringjsonData = @"{ 'FirstName': 'Jignesh', 'LastName': 'Trivedi' }";

            model.JsonData = JsonConvert.SerializeObject(@"[ { 'id': 'ajson1', 'parent': '#', 'text': 'Simple root node 123' },
                    { 'id': 'ajson2', 'parent': '#, 'text': 'Root node 2' },
                    { 'id': 'ajson3', 'parent': 'ajson2', 'text': 'Child 1' },
                    { 'id': 'ajson4', 'parent': 'ajson2', 'text': 'Child 2' } ]");
            return View(model);
        }

        public JsonResult GetJsonData()
        {
            return Json(JsonConvert.SerializeObject(@"[ { 'id': 'ajson1', 'parent': '#', 'text': 'Simple root node 123' },
                    { 'id': 'ajson2', 'parent': '#, 'text': 'Root node 2' },
                    { 'id': 'ajson3', 'parent': 'ajson2', 'text': 'Child 1' },
                    { 'id': 'ajson4', 'parent': 'ajson2', 'text': 'Child 2' } ]"), JsonRequestBehavior.AllowGet);
        }
    }
}