using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WeatherService.DAL;
using WeatherService.Helpers;
using WeatherService.Models;

namespace WeatherService
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class EmployeeService : IEmployeeService
    {
        private readonly UserSettingsContext _context;
        private IRepository<Employee> _employeeRepository;

        public EmployeeService()
        {
            _context = new UserSettingsContext();
            _employeeRepository = new Repository<Employee>();
        }

        public EmployeeService(IRepository<Employee> repository, UserSettingsContext context)
        {
            _context = context;
            //_employeeRepository = new Repository<Employee>(context);
            _employeeRepository = repository;
        }

        public IEnumerable<Employee> GetEmployee(Employee employee)
        {
            var expr = PredicateBuilder.True<Employee>();
            if(!string.IsNullOrEmpty(employee.Name))
                expr = expr.And(e => e.Name == employee.Name);
            if(!string.IsNullOrEmpty(employee.Gender))
                expr = expr.And(e => e.Gender == employee.Gender);
            if (employee.DateOfBirth != null && employee.DateOfBirth != DateTime.MinValue)
                expr = expr.And(e => e.DateOfBirth == employee.DateOfBirth);
            return _employeeRepository.Find(expr);
        }

        public IEnumerable<Employee> GetEmployees()
        {
            return _employeeRepository.GetAllEntities();
        }

        public void UpdateEmployee(Employee employee)
        {
            _employeeRepository.UpdatEntity(employee);
        }

        public void AddEmployee(Employee employee)
        {
            _employeeRepository.AddEntity(employee);
        }
    }
}
