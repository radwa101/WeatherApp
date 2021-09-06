using DemoDatabase;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WeatherApp.DAL;
using WeatherApp.Helpers;
using WeatherApp.Infrastructure;
using WeatherApp.Models;
using WeatherApp.ViewModels;

namespace WeatherApp.Controllers
{
    public class HomeController : Controller
    {
        private IRepository _repository;

        private string _privateKey;
        private string _publicKey;
        private UnicodeEncoding _encoder = new UnicodeEncoding();
        private IGridMvcHelper gridMvcHelper;

        public List<Person> Persons { get; set; }
        public List<Mainperson> mainPerson { get; set; }
        public static int cellValue = 0;
        public int collectioncount = 0;



        public HomeController(IRepository repository)
        {
            this.gridMvcHelper = new GridMvcHelper();
            _repository = repository;
        }

       
        public ActionResult Index()
        {
            //ExecutionEngine engine = new ExecutionEngine();
            //engine.Validate();

            WeatherInfoViewModel viewModel = new WeatherInfoViewModel();
            return View("Index", viewModel);
        }        public ActionResult Client()
        {
            Client client = new Client() { Id = 1, Name = "Ciaran", Email = "ciaran_reidy@msn.com", Active = true };
            return View("_UltraFileItem", client);
        }


        [HttpPost]
        public ActionResult FormSubmitted()
        {
            return View("Index");
        }

        [HttpPost]
        public ActionResult SaveFiles(HttpPostedFileBase file)
        {
            FileDetails model = new FileDetails();
            if (file != null && file.ContentLength > 0)
            {
                using (Stream fs = file.InputStream)
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        model.Image = br.ReadBytes((Int32)fs.Length);
                    }
                }
                try
                {
                    string path = Path.Combine(Server.MapPath("~/App_Data"), Path.GetFileName(file.FileName));
                    //file.SaveAs(path);
                    ViewBag.Message = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult SearchLocations(WeatherInfoViewModel viewModel)
        {
            var serviceLayerInteractions = new Infrastructure.ServiceLayerInteractions();
            viewModel = serviceLayerInteractions.GetWorldTemperatures(viewModel);

            serviceLayerInteractions.GetWorldTemperaturesWithMapper(viewModel);

            return View("Index", viewModel);
        }

        [Route("/excel")]
        public ActionResult Clients()
        {
            LoadData();
          
            string fileName = @"C:\TDM\MyfirstExcelDemo1.xlsx";
            CreatePackage(fileName);

            return View();
        }

        public void CreatePackage(string filePath)
        {
            //To create package
            using (SpreadsheetDocument package = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
            {
                CreateParts(package);
            }
        }

        private void CreateParts(SpreadsheetDocument document)
        {
            //To create the sheet
            WorkbookPart workbookPart1 = document.AddWorkbookPart();
            GenerateWorkbookPart1Content(workbookPart1);

            //To add the Cell Numbers
            WorksheetPart worksheetPart1 = workbookPart1.AddNewPart<WorksheetPart>("rId1");
            GenerateWorksheetPart1Content(worksheetPart1, mainPerson);

            //To add the Actual data 
            SharedStringTablePart sharedStringTablePart1 = workbookPart1.AddNewPart<SharedStringTablePart>("rId4");
            GenerateSharedStringTablePart1Content(sharedStringTablePart1, mainPerson);

            //To store the matadata
            SetPackageProperties(document);
        }

        private void SetPackageProperties(OpenXmlPackage document)
        {
            document.PackageProperties.Creator = "Author Name";
            document.PackageProperties.Created = DateTime.Now;
            document.PackageProperties.Modified = DateTime.Now;
            document.PackageProperties.LastModifiedBy = "Author Name";
        }


        private static int GetTotalRow(List<Mainperson> persons)
        {
            int count = 0;
            foreach (var item in persons)
            {
                foreach (var item1 in item.person)
                {
                    count = count + 1;
                }
                count = count + 1;
            }
            return count;
        }


        private void GenerateWorksheetPart1Content(WorksheetPart worksheetPart1, List<Mainperson> persons)
        {

            collectioncount = GetTotalRow(persons);

            //specifies the columns on which subtotal will apply
            List<string> subTotalColumnArray = new List<string>() { "A", "B", "C" };

            Worksheet worksheet1 = new Worksheet() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "x14ac" } };
            worksheet1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            worksheet1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            worksheet1.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");

            SheetFormatProperties sheetFormatProperties1 = new SheetFormatProperties() { DefaultRowHeight = 15D, OutlineLevelRow = 1, DyDescent = 0.25D };

            SheetProperties SheetProperties1 = new SheetProperties();
            OutlineProperties outlineProp = new OutlineProperties() { SummaryBelow = false };
            SheetProperties1.Append(outlineProp);

            SheetData sheetData1 = new SheetData();
            UInt32 rowIndex = (UInt32Value)1U;//Give Row Number 
            bool Outline = true;


            CreateRowAndAppendInSheetData(sheetData1, rowIndex, Outline, collectioncount);//To store the header values
            rowIndex = (UInt32Value)sheetData1.Elements<Row>().ToList<Row>().Last<Row>().RowIndex + 1;

            foreach (var item in persons)
            {
                CreateRowAndAppendInSheetData(sheetData1, rowIndex, Outline, collectioncount);
                rowIndex = (UInt32Value)sheetData1.Elements<Row>().ToList<Row>().Last<Row>().RowIndex + 1;

                foreach (var item1 in item.person)
                {
                    Outline = false;

                    CreateRowAndAppendInSheetData(sheetData1, rowIndex, Outline, collectioncount);
                    rowIndex = (UInt32Value)sheetData1.Elements<Row>().ToList<Row>().Last<Row>().RowIndex + 1;
                }
                Outline = true;
            }

            worksheet1.Append(SheetProperties1);
            worksheet1.Append(sheetFormatProperties1);
            worksheet1.Append(sheetData1);
            worksheetPart1.Worksheet = worksheet1;
        }

        private static void CreateRowAndAppendInSheetData(SheetData sheetData1, UInt32 rowIndex, bool Outline, int collectioncount)
        {
            Row row = null;
            int tempCellValue = cellValue;
            int val = 0;
            bool result = false;
            //specifies the columns on which you want to insert the data
            List<string> subTotalColumnArray = new List<string>() { "A", "B", "C" };

            //To create Row with Expander or Hidden row
            row = CreateRowWithFormateAndStyle(rowIndex, Outline, row);

            //To store the Cell values (if we have 5 records and 3 columns then)
            /*
             * 0    1   2(0,1,2 to store header you can use 3 4 as per the column and so on)
             * 3    8   13
             * 4    9   14   
             * 5    10  15
             * 6    11  16
             * 7    12  17
             * It will store like this 
             */
            StoreCellValues(rowIndex, Outline, collectioncount, row, ref tempCellValue, ref val, ref result, subTotalColumnArray);

            sheetData1.Append(row);
        }

        private static Row CreateRowWithFormateAndStyle(UInt32 rowIndex, bool Outline, Row row)
        {
            if (rowIndex == 1)
            {
                row = new Row() { RowIndex = (UInt32Value)rowIndex, Spans = new ListValue<StringValue>() { InnerText = "1:3" }, DyDescent = 0.25D };//Simple row 
            }
            else
            {
                if (Outline)
                {
                    row = new Row() { RowIndex = (UInt32Value)rowIndex, Spans = new ListValue<StringValue>() { InnerText = "1:3" }, DyDescent = 0.25D, Collapsed = true };//To give expander
                }
                else
                {
                    row = new Row() { RowIndex = (UInt32Value)rowIndex, Spans = new ListValue<StringValue>() { InnerText = "1:3" }, DyDescent = 0.25D, OutlineLevel = 1, Hidden = true, };//To hide the Row
                }
            }
            return row;
        }

        private static void CreateCellandAppendInRow(Row row1, string cellRef, int cellValue, Nullable<UInt32> styleIndex = null)
        {
            Cell cell1 = new Cell() { CellReference = cellRef, DataType = CellValues.SharedString };
            CellValue cellValue1 = new CellValue();
            cellValue1.Text = Convert.ToString(cellValue);

            if (styleIndex != null)
                cell1.StyleIndex = styleIndex;

            cell1.Append(cellValue1);
            row1.Append(cell1);

        }


        private static void StoreCellValues(UInt32 rowIndex, bool Outline, int collectioncount, Row row, ref int tempCellValue, ref int val, ref bool result, List<string> subTotalColumnArray)
        {
            foreach (var item in subTotalColumnArray)
            {
                string cellRef = item + rowIndex;
                if (Outline == true && rowIndex > 1)
                {
                    CreateCellandAppendInRow(row, cellRef, tempCellValue);
                    tempCellValue = tempCellValue + collectioncount;
                    result = true;
                }
                else
                {
                    if (cellValue >= 3)//3 means number columns if column counts increse specify that count here
                    {
                        CreateCellandAppendInRow(row, cellRef, tempCellValue);
                        tempCellValue = tempCellValue + collectioncount;
                        result = true;
                    }
                    else
                    {
                        CreateCellandAppendInRow(row, cellRef, val);
                        cellValue = cellValue + 1;
                        val = cellValue;
                        result = false;
                    }
                }
            }

            if (result)
                cellValue = cellValue + 1;
        }


        private static void AddSheredString(SharedStringTable sharedStringTable1, string data)
        {
            SharedStringItem sharedStringItem = new SharedStringItem();
            Text text = new Text();
            text.Text = data;
            sharedStringItem.Append(text);

            sharedStringTable1.Append(sharedStringItem);
        }

        private void GenerateSharedStringTablePart1Content(SharedStringTablePart sharedStringTablePart1, List<Mainperson> persons)
        {
            SharedStringTable sharedStringTable1 = new SharedStringTable();

            //To give Header values
            AddSheredString(sharedStringTable1, "Date");
            AddSheredString(sharedStringTable1, "FirstName");
            AddSheredString(sharedStringTable1, "LastName");

            //TO store values Column Wise
            foreach (var item in persons)//First Column
            {
                AddSheredString(sharedStringTable1, item.Date);
                foreach (var item1 in item.person)
                {
                    AddSheredString(sharedStringTable1, "");
                }
            }

            foreach (var item in persons)//Second Column
            {
                AddSheredString(sharedStringTable1, "");
                foreach (var item1 in item.person)
                {
                    AddSheredString(sharedStringTable1, item1.FName);
                }
            }

            foreach (var item in persons)//Third Column
            {
                AddSheredString(sharedStringTable1, item.Total);
                foreach (var item1 in item.person)
                {
                    AddSheredString(sharedStringTable1, item1.LName);
                }
            }

            //Add the data into the sharedStringTable
            sharedStringTablePart1.SharedStringTable = sharedStringTable1;
        }


        private void GenerateWorkbookPart1Content(WorkbookPart workbookPart1)
        {
            Workbook workbook1 = new Workbook();
            workbook1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");

            Sheets sheets1 = new Sheets();
            Sheet sheet1 = new Sheet() { Name = "Sheet1", SheetId = (UInt32Value)1U, Id = "rId1" };

            sheets1.Append(sheet1);
            workbook1.Append(sheets1);

            workbookPart1.Workbook = workbook1;
        }

        private void LoadData()
        {
            Persons = new List<Person>();

            Persons.Add(new Person { Date = "19/5/2012", FName = "F1", LName = "L1" });
            Persons.Add(new Person { Date = "19/5/2012", FName = "F11", LName = "L11" });
            Persons.Add(new Person { Date = "19/5/2012", FName = "F111", LName = "L111" });
            Persons.Add(new Person { Date = "19/5/2012", FName = "F1111", LName = "L1111" });
            Persons.Add(new Person { Date = "20/5/2012", FName = "F2", LName = "L2" });
            Persons.Add(new Person { Date = "20/5/2012", FName = "F22", LName = "L22" });
            Persons.Add(new Person { Date = "20/5/2012", FName = "F222", LName = "L22" });
            Persons.Add(new Person { Date = "20/5/2012", FName = "F2222", LName = "L22" });
            Persons.Add(new Person { Date = "20/5/2012", FName = "F22222", LName = "L22" });
            Persons.Add(new Person { Date = "21/5/2012", FName = "F3", LName = "L3" });
            Persons.Add(new Person { Date = "21/5/2012", FName = "F33", LName = "L3" });
            Persons.Add(new Person { Date = "21/5/2012", FName = "F333", LName = "L3" });
            Persons.Add(new Person { Date = "21/5/2012", FName = "F3333", LName = "L3" });
            Persons.Add(new Person { Date = "21/5/2012", FName = "F33333", LName = "L3" });
            Persons.Add(new Person { Date = "22/5/2012", FName = "F4", LName = "L4" });
            Persons.Add(new Person { Date = "22/5/2012", FName = "F44", LName = "L4" });
            Persons.Add(new Person { Date = "22/5/2012", FName = "F444", LName = "L4" });
            Persons.Add(new Person { Date = "22/5/2012", FName = "F4444", LName = "L4" });
            Persons.Add(new Person { Date = "23/5/2012", FName = "F5", LName = "L5" });
            Persons.Add(new Person { Date = "23/5/2012", FName = "F5", LName = "L5" });
            Persons.Add(new Person { Date = "23/5/2012", FName = "F55", LName = "L5" });
            Persons.Add(new Person { Date = "23/5/2012", FName = "F555", LName = "L5" });

            mainPerson = new List<Mainperson>();

            mainPerson.Add(new Mainperson { Date = "19/5/2012", person = Persons.Where(c => c.Date.ToString() == "19/5/2012").ToList() });
            mainPerson.Add(new Mainperson { Date = "20/5/2012", person = Persons.Where(c => c.Date.ToString() == "20/5/2012").ToList() });
            mainPerson.Add(new Mainperson { Date = "21/5/2012", person = Persons.Where(c => c.Date.ToString() == "21/5/2012").ToList() });
            mainPerson.Add(new Mainperson { Date = "22/5/2012", person = Persons.Where(c => c.Date.ToString() == "22/5/2012").ToList() });
            mainPerson.Add(new Mainperson { Date = "23/5/2012", person = Persons.Where(c => c.Date.ToString() == "23/5/2012").ToList() });


        }

        [HttpPost]
        public ActionResult Clients(Client objorder)
        {
            return View(objorder);
        }

        [ChildActionOnly]
        public ActionResult GetGrid()
        {
            IQueryable<Client> clients = new List<Client>()
            {
                new Client { Id = 1, Name = "Julio Avellaneda", Email = "julito_gtu@hotmail.com" },
                new Client { Id = 2, Name = "Juan Torres", Email = "jtorres@hotmail.com" },
                new Client { Id = 3, Name = "Oscar Camacho", Email = "oscar@hotmail.com" },
                new Client { Id = 4, Name = "Gina Urrego", Email = "ginna@hotmail.com" },
                new Client { Id = 5, Name = "Nathalia Ramirez", Email = "natha@hotmail.com" },
                new Client { Id = 6, Name = "Raul Rodriguez", Email = "rodriguez.raul@hotmail.com" },
                new Client { Id = 7, Name = "Johana Espitia", Email = "johana_espitia@hotmail.com" }
            }
            .AsQueryable();
            var grid = this.gridMvcHelper.GetAjaxGrid(clients.OrderBy(c => c.Id));

            return PartialView("~/Views/Home/_ClientGrid.cshtml", grid);
        }

        [HttpGet]
        public ActionResult GridPager(int? page)
        {
            IQueryable<Client> clients = new List<Client>()
            {
                new Client { Id = 1, Name = "Julio Avellaneda", Email = "julito_gtu@hotmail.com" },
                new Client { Id = 2, Name = "Juan Torres", Email = "jtorres@hotmail.com" },
                new Client { Id = 3, Name = "Oscar Camacho", Email = "oscar@hotmail.com" },
                new Client { Id = 4, Name = "Gina Urrego", Email = "ginna@hotmail.com" },
                new Client { Id = 5, Name = "Nathalia Ramirez", Email = "natha@hotmail.com" },
                new Client { Id = 6, Name = "Raul Rodriguez", Email = "rodriguez.raul@hotmail.com" },
                new Client { Id = 7, Name = "Johana Espitia", Email = "johana_espitia@hotmail.com" }
            }
            .AsQueryable();
            var grid = this.gridMvcHelper.GetAjaxGrid(clients.OrderBy(c => c.Id), page);
            object jsonData = this.gridMvcHelper.GetGridJsonData(grid, "~/Views/Home/_ClientGrid.cshtml", this);

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Register()
        {
            RegisterViewModel viewModel = new RegisterViewModel();
            viewModel.Email = string.Empty;
            return View("Register", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        public JsonResult ValidateUserName(string username, string email)
        {
            return Json(!username.Equals("duplicate"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetData()
        {
            System.Threading.Thread.Sleep(2000);
            var result = new { Success = true };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }

    public class Person
    {
        public string Date { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
    }

    public class Mainperson
    {
        public string Date { get; set; }
        public List<Person> person { get; set; }
        public string Total
        {
            get
            {
                if (person != null && person.Count != 0)
                    return person.Count.ToString();

                return "0";
            }
        }

    }
}