using Castle.Facilities.WcfIntegration;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WcfServiceHost.DAL;
using WcfServiceHost.Models;

namespace TestWeatherService
{
    class Program
    {
        
        static void Main(string[] args)
        {
            WeatherService.WeatherService weatherServiceClient = new WeatherService.WeatherService();
            var weatherResults = weatherServiceClient.ConvertCelciusToFahrenheit(30);

            WindsorContainer Container = new WindsorContainer();
            Container.AddFacility<WcfFacility>();

            Container.Register(
                Component.For<IRepository<Employee>>().ImplementedBy<Repository<Employee>>()
                );
            IEmployeeRepository _employeeRepository = Container.Resolve<EmployeeRepository>();

            WcfServiceHost.EmployeeService employeeServiceClient = new WcfServiceHost.EmployeeService(_employeeRepository);
            var employees = employeeServiceClient.GetEmployees();

            var emp = new Employee();
            emp.Name = "CiaranMary";
            emp.Gender = "Male";
            emp.DateOfBirth = Convert.ToDateTime("08/12/1982");
            employeeServiceClient.AddEmployee(emp);

           // var testEmployee = employeeServiceClient.GetEmployee(emp);
        }
    }
}
