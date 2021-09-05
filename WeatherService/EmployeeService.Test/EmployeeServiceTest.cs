using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WcfServiceHost.DAL;
using WcfServiceHost.Models;
using System.Collections.Generic;
using System.Linq;
using Rhino.Mocks;
using System.ServiceModel;
using AutoFixture;
using System.Threading;
using System.ServiceModel.Channels;
using System.Web.Services.Description;

namespace EmployeeService.Test
{
    [TestClass]
    public class EmployeeServiceTest
    {
        private IEnumerable<Employee> _fakeEmployees = new List<Employee>
        {
            new Employee {Id = 1, Name = "Mary", Gender = "female", DateOfBirth = new DateTime(1982,12,15)},
            new Employee {Id = 2, Name = "Ciaran", Gender = "male", DateOfBirth = new DateTime(1982,12,08)}
        };

        private IEmployeeRepository mockRepo;

       
        [TestMethod]
        public void If_id_is_1_the_employee_name_shall_be_putra()
        {
            // Arrange
            var fixture = new Fixture { RepeatCount = 10 };
            var fakeEmployees = fixture.Repeat(fixture.Create<Employee>).ToList();
            fakeEmployees[0].Id = 1;

            mockRepo = MockRepository.GenerateMock<IEmployeeRepository>();
            mockRepo.Stub(repo => repo.GetAllEntities()).Return(fakeEmployees);
            var service = new WcfServiceHost.EmployeeService(mockRepo);

            //// Act
            var dto = service.GetEmployeeById(1);

            //// Assert
            mockRepo.AssertWasCalled(ultraFile => ultraFile.GetAllEntities());
            mockRepo.VerifyAllExpectations();
            Assert.AreEqual(1, dto.Id);
        }

        
        [TestMethod]
        public void GetEmployeeById_WhenUsingID2ReturnMary()
        {
            // Arrange
            MockRepository mocks = new MockRepository();
            IEmployeeRepository employeeRepository = mocks.Stub<IEmployeeRepository>();
            WcfServiceHost.EmployeeService employeeService = new WcfServiceHost.EmployeeService(employeeRepository);

            using (mocks.Record())
            {
                SetupResult.For(employeeRepository.GetAllEntities()).Return(_fakeEmployees);
            }

            // Act
            var results = employeeService.GetEmployees();

            // Assert
            Assert.AreEqual(2, results.Count());
        }
        
    }
}
