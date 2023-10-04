using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

using PageMaintenance_AngularProject.Controllers;
using PageMaintenance_AngularProject.Models;
using PageMaintenance_AngularProject.Services;

namespace PageMaintenanceAPI_Test.Controllers
{
    public class TableControllerTest
    {
        private readonly IFixture _fixture;
        private readonly TableController _tableController;
        private readonly Mock<ITableInterface> _tableInterface;

        public TableControllerTest()
        {
            _fixture = new Fixture();
            _tableInterface = _fixture.Freeze<Mock<ITableInterface>>();
            _tableController = new TableController(_tableInterface.Object);
        }
        //GetTableNames Test
        [Fact]
        public void GetTableNames_ReturnsOk_When_Success()
        {
            //Arrange
            var tableNames = _fixture.Create<List<TableNames>>();
            _tableInterface.Setup(t=>t.GetTableNames()).ReturnsAsync(tableNames);

            //Act
            var result = _tableController.GetAllTableNames();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Task<IActionResult>>();
            result.Result.Should().BeAssignableTo<OkObjectResult>().Subject.Value.Should().Be(tableNames);
            _tableInterface.Verify(t=>t.GetTableNames(), Times.Once());
        }
        [Fact]
        public async void GetTableNames_ShouldReturn_NotFound_When_DataNotFound()
        {
            //Arrange
           _tableInterface.Setup(t=>t.GetTableNames()).Returns(Task.FromResult<List<TableNames>>(null));

            //Act
            var result = _tableController.GetAllTableNames();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Task<IActionResult>>();
            result.Result.Should().BeAssignableTo<NotFoundResult>();
            _tableInterface.Verify(t => t.GetTableNames(), Times.Once());
        }
        [Fact]
        public async void GetTableNames_ShouldReturn_BadRequest_When_ErrorOccured()
        {
            //Arrange
            var tableNames = _fixture.Create<List<TableNames>>();
            _tableInterface.Setup(t => t.GetTableNames()).Throws(new Exception("Error Occured"));

            //Act
            var result = _tableController.GetAllTableNames();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Task<IActionResult>>();
            result.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject.Value.Should().Be("Error Occured");
            _tableInterface.Verify(t => t.GetTableNames(), Times.Once());
        }
        //GetTableById Tests

        [Fact]
        public async void GetTableById_ShouldReturn_OkResult_WhenSuccess()
        {
            //Arrange
            var tableId = _fixture.Create<Guid>();
            var table = _fixture.Create<Aotable>();
            _tableInterface.Setup(t => t.GetTableById(tableId)).ReturnsAsync(table);

            //Action
            var result = _tableController.GetTableById(tableId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Task<IActionResult>>();
            result.Result.Should().BeAssignableTo<OkObjectResult>();
            _tableInterface.Verify(t=>t.GetTableById(tableId), Times.Once());
        }
        [Fact]
        public async void GetTableById_ShouldReturn_NotFoundResult_WhenTableNotFound()
        {
            //Arrange
            var tableId = _fixture.Create<Guid>();
            _tableInterface.Setup(t => t.GetTableById(tableId)).ReturnsAsync((Aotable)null);

            //Act
            var result = _tableController.GetTableById(tableId);

            //Assert
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<NotFoundResult>();
            _tableInterface.Verify(t=>t.GetTableById(tableId), Times.Once() );

        }
        [Fact]
        public async void GetTableById_ShouldReturn_BadRequestResult_When_ErrorOccurs()
        {
            //Arrange
            var tableId = _fixture.Create<Guid>();
            _tableInterface.Setup(t => t.GetTableById(tableId)).Throws(new Exception("Error Occured"));

            //Act
            var result = _tableController.GetTableById(tableId);

            //Assert
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<BadRequestObjectResult>("Error Occured");
            _tableInterface.Verify(t => t.GetTableById(tableId), Times.Once());
        }
    }
}