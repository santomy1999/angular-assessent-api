using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
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
        public async void GetTableNames_ReturnsOk_WhenSuccess()
        {
            //Arrange
            var tableNames = _fixture.Create<List<TableNames>>();
            _tableInterface.Setup(t=>t.GetTableNames()).ReturnsAsync(tableNames);

            //Act
            var result = await _tableController.GetAllTableNames();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            var okObjectResult = result.As<OkObjectResult>();
            okObjectResult.Value.Should().BeEquivalentTo(tableNames);
            _tableInterface.Verify(t=>t.GetTableNames(), Times.Once());
        }
        [Fact]
        public async void GetTableNames_ShouldReturnNotFound_WhenDataNotFound()
        {
            //Arrange
           _tableInterface.Setup(t=>t.GetTableNames()).Returns(Task.FromResult<List<TableNames>>(null));

            //Act
            var result =  await _tableController.GetAllTableNames();

            //Assert
             result.Should().NotBeNull();
            var notFoundObjectResult = Assert.IsType<NotFoundResult>(result);
            _tableInterface.Verify(t => t.GetTableNames(), Times.Once());
        }
        [Fact]
        public async void GetTableNames_ShouldReturnBadRequest_WhenErrorOccured()
        {
            //Arrange
            var tableNames = _fixture.Create<List<TableNames>>();
            _tableInterface.Setup(t => t.GetTableNames()).Throws(new Exception("Error Occured"));

            //Act
            var result =await _tableController.GetAllTableNames();

            //Assert
            result.Should().NotBeNull();
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error Occured", badRequestResult.Value);
            _tableInterface.Verify(t => t.GetTableNames(), Times.Once());
        }
        //GetTableById Tests

        [Fact]
        public async void GetTableById_ShouldReturnOkResult_WhenSuccess()
        {
            //Arrange
            var tableId = _fixture.Create<Guid>();
            var table = _fixture.Create<Aotable>();
            _tableInterface.Setup(t => t.GetTableById(tableId)).ReturnsAsync(table);

            //Action
            var result = await _tableController.GetTableById(tableId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            var okObjectResult = result.As<OkObjectResult>();
            okObjectResult.Value.Should().BeEquivalentTo(table);
            _tableInterface.Verify(t=>t.GetTableById(tableId), Times.Once());
        }
        [Fact]
        public async void GetTableById_ShouldReturnNotFoundResult_WhenTableNotFound()
        {
            //Arrange
            var tableId = _fixture.Create<Guid>();
            _tableInterface.Setup(t => t.GetTableById(tableId)).ReturnsAsync((Aotable)null);

            //Act
            var result = await _tableController.GetTableById(tableId);

            //Assert
            result.Should().NotBeNull();
            var notFoundObjectResult = Assert.IsType<NotFoundResult>(result);
            _tableInterface.Verify(t=>t.GetTableById(tableId), Times.Once() );

        }
        [Fact]
        public async void GetTableById_ShouldReturnBadRequestResult_WhenErrorOccurs()
        {
            //Arrange
            var tableId = _fixture.Create<Guid>();
            _tableInterface.Setup(t => t.GetTableById(tableId)).Throws(new Exception("Error Occured"));

            //Act
            var result = await _tableController.GetTableById(tableId);

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error Occured", badRequestResult.Value);
            _tableInterface.Verify(t => t.GetTableById(tableId), Times.Once());
        }
    }
}