using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using Moq;
using PageMaintenance_AngularProject.Controllers;
using PageMaintenance_AngularProject.Models;
using PageMaintenance_AngularProject.Services;
using System.Drawing.Printing;

namespace PageMaintenanceAPI_Test
{
    public class FormControllerTest
    {
        private readonly IFixture _fixture;
        private readonly FormController _formController;
        private readonly Mock<IFormInterface> _formInterface;

        public FormControllerTest()
        {
            _fixture = new Fixture();
            _formInterface= _fixture.Freeze<Mock<IFormInterface>>();
            _formController = new FormController(_formInterface.Object);
        }

        //GetAllForms Test
        [Fact]
        public async void GetAllForms_ShouldReturnOkResult_WhenSeccess()
        {
            //Arrange
            var forms = _fixture.Create<List<Form>>();
            int pageNumber = _fixture.Create<int>();
            int pageSize = _fixture.Create<int>();

            _formInterface.Setup(f=>f.GetAllForms(pageNumber,pageSize)).ReturnsAsync(forms);

            //Act
            var result = await _formController.GetAllForms(pageNumber, pageSize);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            var okObjectResult = result.As<OkObjectResult>();
            okObjectResult.Value.Should().BeEquivalentTo(forms);
            _formInterface.Verify(f=>f.GetAllForms(pageNumber, pageSize), Times.Once());
        }
        [Fact]
        public async void GetAllForms_ShouldReturnNotFoundResult_WhenFormsNotFound()
        {

            //Arrange
            int pageNumber = _fixture.Create<int>();
            int pageSize = _fixture.Create<int>();
            _formInterface.Setup(f=>f.GetAllForms(pageNumber, pageSize)).ReturnsAsync(new List<Form>());

            //Act
            var result = await _formController.GetAllForms(pageNumber, pageSize);

            //Assert
            result.Should().NotBeNull();
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No Forms Found", notFoundObjectResult.Value);
            _formInterface.Verify(f => f.GetAllForms(pageNumber, pageSize), Times.Once());
        }
        [Fact]
        public async void GetAllForms_ShouldReturnBadRequest_WhenErrorOccurs()
        {
            //Arrange
            int pageNumber = _fixture.Create<int>();
            int pageSize = _fixture.Create<int>();
            _formInterface.Setup(f => f.GetAllForms(pageNumber,pageSize)).Throws(new Exception("Error occured"));

            //Act
            var result = await _formController.GetAllForms(pageNumber, pageSize);

            //Assert
            result.Should().NotBeNull();
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error occured", badRequestResult.Value);
            _formInterface.Verify(f => f.GetAllForms(pageNumber, pageSize), Times.Once());
        }
        //GetFormById test
        [Fact]
        public async void GetFormById_ShouldReturnOkResult_WhenSuccess()
        {
            //Arrange
            var formId = _fixture.Create<Guid>();
            var form = _fixture.Create<Form>();
            _formInterface.Setup(t => t.GetFormById(formId)).ReturnsAsync(form);

            //Action
            var result = await _formController.GetFormById(formId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            var okObjectResult = result.As<OkObjectResult>();
            okObjectResult.Value.Should().BeEquivalentTo(form);
            _formInterface.Verify(t => t.GetFormById(formId), Times.Once());
        }
        [Fact]
        public async void GetFormById_ShouldReturnNotFoundResult_WhenFormNotFound()
        {
            //Arrange
            var formId = _fixture.Create<Guid>();
            _formInterface.Setup(t => t.GetFormById(formId)).ReturnsAsync((Form)null);

            //Act
            var result = await _formController.GetFormById(formId);

            //Assert
            result.Should().NotBeNull();
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Form Not Found", notFoundObjectResult.Value); 
            _formInterface.Verify(t => t.GetFormById(formId), Times.Once());

        }
        [Fact]
        public async void GetFormById_ShouldReturnBadRequestResult_WhenErrorOccurs()
        {
            //Arrange
            var formId = _fixture.Create<Guid>();
            _formInterface.Setup(t => t.GetFormById(formId)).Throws(new Exception("Error Occured"));

            //Act
            var result = _formController.GetFormById(formId);

            //Assert
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<BadRequestObjectResult>("Error Occured");
            _formInterface.Verify(t => t.GetFormById(formId), Times.Once());
        }
        //GetFormByFormName
        [Fact]
        public async void GetFormByFormName_ShouldReturnOkResult_WhenSuccess()
        {
            //Arrange int pageNumber = _fixture.Create<int>();
            
            var formName = _fixture.Create<String>();
            var form = _fixture.Create<List<Form>>();
            int pageNumber = _fixture.Create<int>();
            int pageSize = _fixture.Create<int>();
            _formInterface.Setup(t => t.GetFormByFormName(formName, pageNumber, pageSize)).ReturnsAsync(form);

            //Action
            var result = await _formController.GetFormByFormName(formName, pageNumber, pageSize);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            var okObjectResult = result.As<OkObjectResult>();
            okObjectResult.Value.Should().BeEquivalentTo(form);
            _formInterface.Verify(t => t.GetFormByFormName(formName, pageNumber, pageSize), Times.Once());
        }
        [Fact]
        public async void GetFormByFormName_ShouldReturnNotFoundResult_WhenFormNotFound()
        {
            //Arrange
            int pageNumber = _fixture.Create<int>();
            int pageSize = _fixture.Create<int>();
            var formName = _fixture.Create<String>();
            _formInterface.Setup(t => t.GetFormByFormName(formName, pageNumber, pageSize)).Returns(Task.FromResult<List<Form>>(null));

            //Act
            var result =  await _formController.GetFormByFormName(formName, pageNumber, pageSize);

            //Assert
            result.Should().NotBeNull();
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Form Not Found", notFoundObjectResult.Value); 
            _formInterface.Verify(t => t.GetFormByFormName(formName, pageNumber, pageSize), Times.Once());

        }
        [Fact]
        public async void GetFormByFormName_ShouldReturnBadRequestResultWhenErrorOccurs()
        {
            //Arrange
            int pageNumber = _fixture.Create<int>();
            int pageSize = _fixture.Create<int>();
            var formName = _fixture.Create<String>();
            _formInterface.Setup(t => t.GetFormByFormName(formName, pageNumber, pageSize)).Throws(new Exception("Error Occured"));

            //Act
            var result = await _formController.GetFormByFormName(formName, pageNumber, pageSize);

            //Assert
            result.Should().NotBeNull();
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error Occured", badRequestResult.Value);
            _formInterface.Verify(t => t.GetFormByFormName(formName, pageNumber, pageSize), Times.Once());
        }
        [Fact]
        public async void GetFormByFormName_ShouldReturnBadRequestResult_WhenFormNumberIsNull()
        {
            //Arrange
            int pageNumber = _fixture.Create<int>();
            int pageSize = _fixture.Create<int>();
            String formName = null;
            _formInterface.Setup(t => t.GetFormByFormName(formName , pageNumber, pageSize));

            //Act
            var result = await _formController.GetFormByFormName(formName, pageNumber, pageSize);

            //Assert
            result.Should().NotBeNull();
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Form Name is null or empty", badRequestResult.Value);
            _formInterface.Verify(t => t.GetFormByFormName(formName, pageNumber, pageSize), Times.Never());
        }
        [Fact]
        public async void GetFormByFormName_ShouldReturnBadRequestResult_WhenFormNumberIsEmpty()
        {
            //Arrange
            int pageNumber = _fixture.Create<int>();
            int pageSize = _fixture.Create<int>();
            String formName = "";
            _formInterface.Setup(t => t.GetFormByFormName(formName, pageNumber, pageSize));

            //Act
            var result = await _formController.GetFormByFormName(formName, pageNumber, pageSize);

            //Assert
            result.Should().NotBeNull();
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Form Name is null or empty", badRequestResult.Value); 
            _formInterface.Verify(t => t.GetFormByFormNumber(formName, pageNumber, pageSize), Times.Never());
        }
        //GetFormByFormNumber
        [Fact]
        public async void GetFormByFormNumber_ShouldReturnOkResult_WhenSuccess()
        {
            //Arrange
            int pageNumber = _fixture.Create<int>();
            int pageSize = _fixture.Create<int>();
            var formNumber = _fixture.Create<String>();
            var form = _fixture.Create<List<Form>>();
            _formInterface.Setup(t => t.GetFormByFormNumber(formNumber,pageNumber, pageSize)).ReturnsAsync(form);

            //Action
            var result = await _formController.GetFormByFormNumber(formNumber, pageNumber, pageSize);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            var okObjectResult = result.As<OkObjectResult>();
            okObjectResult.Value.Should().BeEquivalentTo(form);
            _formInterface.Verify(t => t.GetFormByFormNumber(formNumber, pageNumber, pageSize), Times.Once());
        }
        [Fact]
        public async void GetFormByFormNumber_ShouldReturnNotFoundResult_WhenFormNotFound()
        {
            //Arrange
            int pageNumber = _fixture.Create<int>();
            int pageSize = _fixture.Create<int>();
            var formNumber = _fixture.Create<String>();
            _formInterface.Setup(t => t.GetFormByFormNumber(formNumber, pageNumber, pageSize)).Returns(Task.FromResult<List<Form>>(null));

            //Act
            var result = await _formController.GetFormByFormNumber(formNumber, pageNumber, pageSize);

            //Assert
            result.Should().NotBeNull();
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Form Not Found", notFoundObjectResult.Value);
            _formInterface.Verify(t => t.GetFormByFormNumber(formNumber, pageNumber, pageSize), Times.Once());

        }
        [Fact]
        public async void GetFormByFormNumber_ShouldReturnBadRequestResult_WhenErrorOccurs()
        {
            //Arrange
            int pageNumber = _fixture.Create<int>();
            int pageSize = _fixture.Create<int>();
            var formNumber = _fixture.Create<String>();
            _formInterface.Setup(t => t.GetFormByFormNumber(formNumber, pageNumber, pageSize)).Throws(new Exception("Error Occured"));

            //Act
            var result = await _formController.GetFormByFormNumber(formNumber, pageNumber, pageSize);

            //Assert
            result.Should().NotBeNull();
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error Occured", badRequestResult.Value);
            _formInterface.Verify(t => t.GetFormByFormNumber(formNumber, pageNumber, pageSize), Times.Once());
        }
        [Fact]
        public async void GetFormByFormNumber_ShouldReturnBadRequestResult_WhenFormNumberIsNull()
        {
            //Arrange
            int pageNumber = _fixture.Create<int>();
            int pageSize = _fixture.Create<int>();
            String formNumber = null;
            _formInterface.Setup(t => t.GetFormByFormNumber(formNumber, pageNumber, pageSize));

            //Act
            var result = await _formController.GetFormByFormNumber(formNumber, pageNumber, pageSize);

            //Assert
            result.Should().NotBeNull();
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Form Number is null or empty", badRequestResult.Value);
            _formInterface.Verify(t => t.GetFormByFormNumber(formNumber, pageNumber, pageSize), Times.Never());
        }
        [Fact]
        public async void GetFormByFormNumber_ShouldReturnBadRequestResult_WhenFormNumberIsEmpty()
        {
            //Arrange
            int pageNumber = _fixture.Create<int>();
            int pageSize = _fixture.Create<int>();
            String formNumber = "";
            _formInterface.Setup(t => t.GetFormByFormNumber(formNumber, pageNumber, pageSize));

            //Act
            var result = await _formController.GetFormByFormNumber(formNumber, pageNumber, pageSize);

            //Assert
            result.Should().NotBeNull();
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Form Number is null or empty", badRequestResult.Value);
            _formInterface.Verify(t => t.GetFormByFormNumber(formNumber, pageNumber, pageSize), Times.Never());
        }
        //AddForm Test
        [Fact]
        public async void AddForm_ShouldReturnOkResult_WhenSuccess()
        {
            //Arrange
            var form = _fixture.Create<Form>();
            var formReturn = _fixture.Create<Form>();
            _formInterface.Setup(f => f.AddForm(form)).ReturnsAsync(formReturn);

            //Action
            var result = await _formController.AddForm(form);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            var okObjectResult = result.As<OkObjectResult>();
            okObjectResult.Value.Should().BeEquivalentTo(formReturn);
            _formInterface.Verify(f => f.AddForm(form), Times.Once());
        }
        [Fact]
        public async void AddForm_ShouldReturnBadRequest_WhenExceptionIsThrown()
        {
            //Arrange
            var form = _fixture.Create<Form>();
            _formInterface.Setup(f => f.AddForm(form)).Throws(new Exception("Error occured"));

            //Act
            var result = await _formController.AddForm(form);

            //Assert
            result.Should().NotBeNull();
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error occured", badRequestResult.Value);
            _formInterface.Verify(t => t.AddForm(form), Times.Once());

        }
        [Fact]
        public async void AddForm_ShouldReturnBadRequestResult_WhenErrorOccurs()
        {
            //Arrange
            var form = _fixture.Create<Form>();
            Form formReturn = null;
            _formInterface.Setup(f => f.AddForm(form)).ReturnsAsync(formReturn);

            //Act
            var result = await _formController.AddForm(form);

            //Assert
            result.Should().NotBeNull();
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error occured", badRequestResult.Value);
            _formInterface.Verify(f => f.AddForm(form), Times.Once());
        }
        //Update Form
        [Fact]
        public async void EditFormById_ShouldReturnOkResult_WhenSuccess()
        {
            //Arrange
            var formId = _fixture.Create<Guid>();
            var existingform = _fixture.Create<Form>();
            var newform = _fixture.Create<Form>();
            _formInterface.Setup(f => f.EditFormById(existingform)).ReturnsAsync(newform);

            //Action
            var result = await _formController.EditFormById( existingform);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            var okObjectResult = result.As<OkObjectResult>();
            okObjectResult.Value.Should().BeEquivalentTo(newform);
            _formInterface.Verify(f => f.EditFormById(existingform), Times.Once());
        }
        [Fact]
        public async void EditFormById_ShouldReturnNotFound_WhenFormNotFound()
        {
            //Arrange
            var formId = _fixture.Create<Guid>();
            Form existingform = null;
            var newform = _fixture.Create<Form>();
            _formInterface.Setup(f => f.EditFormById(existingform)).ReturnsAsync(existingform);

            //Act
            var result = await _formController.EditFormById(existingform);

            //Assert
            result.Should().NotBeNull();
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Form Not Found", notFoundObjectResult.Value);
            _formInterface.Verify(t => t.EditFormById(existingform), Times.Once());

        }
        [Fact]
        public async void EditFormById_ShouldReturnBadRequestResult_WhenErrorOccurs()
        {
            //Arrange
            var formId = _fixture.Create<Guid>();
            var existingform = _fixture.Create<Form>();
            Form newform = null ;
            _formInterface.Setup(f => f.EditFormById(existingform)).Throws(new Exception("Error occured"));
            

            //Act
            var result =await _formController.EditFormById(existingform);

            //Assert
            result.Should().NotBeNull();
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error occured", badRequestResult.Value);
            _formInterface.Verify(f => f.EditFormById(existingform), Times.Once());
        }

        //DeleteFormByid
        [Fact]
        public async void DeleteFormById_ShouldReturnOkResult_WhenSuccess()
        {
            //Arrange
            var formId = _fixture.Create<Guid>();
            var form = _fixture.Create<Form>();
            _formInterface.Setup(t => t.DeleteFormById(formId)).ReturnsAsync(form);

            //Action
            var result = await _formController.DeleteFormById(formId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            var okObjectResult = result.As<OkObjectResult>();
            okObjectResult.Value.Should().BeEquivalentTo(form);
            _formInterface.Verify(t => t.DeleteFormById(formId), Times.Once());
        }
        [Fact]
        public async void DeleteFormById_ShouldReturnNotFoundResult_WhenFormNotFound()
        {
            //Arrange
            var formId = _fixture.Create<Guid>();
            _formInterface.Setup(t => t.DeleteFormById(formId)).ReturnsAsync((Form)null);

            //Act
            var result = await _formController.DeleteFormById(formId);

            //Assert
            result.Should().NotBeNull();
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Form Not found", notFoundObjectResult.Value);
            _formInterface.Verify(t => t.DeleteFormById(formId), Times.Once());

        }
        [Fact]
        public async void DeleteFormById_ShouldReturnBadRequestResult_When_ErrorOccurs()
        {
            //Arrange
            var formId = _fixture.Create<Guid>();
            _formInterface.Setup(t => t.DeleteFormById(formId)).Throws(new Exception("Error Occured"));

            //Act
            var result = await _formController.DeleteFormById(formId);

            //Assert
            result.Should().NotBeNull();
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error Occured", badRequestResult.Value);
            _formInterface.Verify(t => t.DeleteFormById(formId), Times.Once());
        }
    }
}