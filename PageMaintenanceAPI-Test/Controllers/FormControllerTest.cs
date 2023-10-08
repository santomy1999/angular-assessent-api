using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using Moq;
using PageMaintenance_AngularProject.Controllers;
using PageMaintenance_AngularProject.Models;
using PageMaintenance_AngularProject.Services;

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
            _formInterface.Setup(f=>f.GetAllForms()).ReturnsAsync(forms);

            //Act
            var result = _formController.GetAllForms();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Task<IActionResult>>();
            result.Result.Should().BeAssignableTo<OkObjectResult>().Subject.Value.Should().Be(forms);
            _formInterface.Verify(f=>f.GetAllForms(), Times.Once());
        }
        [Fact]
        public async void GetAllForms_ShouldReturnNotFoundResult_WhenFormsNotFound()
        {
            //Arrange
            _formInterface.Setup(f=>f.GetAllForms()).ReturnsAsync(new List<Form>());

            //Act
            var result = _formController.GetAllForms();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Task<IActionResult>>();
            result.Result.Should().BeAssignableTo<NotFoundObjectResult>().Subject.Value.Should().Be("No Forms Found");
            _formInterface.Verify(f => f.GetAllForms(), Times.Once());
        }
        [Fact]
        public async void GetAllForms_ShouldReturnBadRequest_WhenErrorOccurs()
        {
            //Arrange
            _formInterface.Setup(f => f.GetAllForms()).Throws(new Exception("Error occured"));

            //Act
            var result = _formController.GetAllForms();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Task<IActionResult>>();
            result.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject.Value.Should().Be("Error occured");
            _formInterface.Verify(f => f.GetAllForms(), Times.Once());
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
            var result = _formController.GetFormById(formId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Task<IActionResult>>();
            result.Result.Should().BeAssignableTo<OkObjectResult>().Subject.Value.Should().Be(form); 
            _formInterface.Verify(t => t.GetFormById(formId), Times.Once());
        }
        [Fact]
        public async void GetFormById_ShouldReturnNotFoundResult_WhenFormNotFound()
        {
            //Arrange
            var formId = _fixture.Create<Guid>();
            _formInterface.Setup(t => t.GetFormById(formId)).ReturnsAsync((Form)null);

            //Act
            var result = _formController.GetFormById(formId);

            //Assert
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<NotFoundObjectResult>().Subject.Value.Should().Be("Form Not Found");
            _formInterface.Verify(t => t.GetFormById(formId), Times.Once());

        }
        [Fact]
        public async void GetFormById_ShouldReturnBadRequestResult_When_ErrorOccurs()
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
            //Arrange
            var formName = _fixture.Create<String>();
            var form = _fixture.Create<List<Form>>();
            _formInterface.Setup(t => t.GetFormByFormName(formName)).ReturnsAsync(form);

            //Action
            var result = _formController.GetFormByFormName(formName);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Task<IActionResult>>();
            result.Result.Should().BeAssignableTo<OkObjectResult>().Subject.Value.Should().Be(form);
            _formInterface.Verify(t => t.GetFormByFormName(formName), Times.Once());
        }
        [Fact]
        public async void GetFormByFormName_ShouldReturnNotFoundResult_WhenFormNotFound()
        {
            //Arrange
            var formName = _fixture.Create<String>();
            _formInterface.Setup(t => t.GetFormByFormName(formName)).Returns(Task.FromResult<List<Form>>(null));

            //Act
            var result = _formController.GetFormByFormName(formName);

            //Assert
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<NotFoundObjectResult>().Subject.Value.Should().Be("Form Not Found");
            _formInterface.Verify(t => t.GetFormByFormName(formName), Times.Once());

        }
        [Fact]
        public async void GetFormByFormName_ShouldReturnBadRequestResult_When_ErrorOccurs()
        {
            //Arrange
            var formName = _fixture.Create<String>();
            _formInterface.Setup(t => t.GetFormByFormName(formName)).Throws(new Exception("Error Occured"));

            //Act
            var result = _formController.GetFormByFormName(formName);

            //Assert
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<BadRequestObjectResult>("Error Occured");
            _formInterface.Verify(t => t.GetFormByFormName(formName), Times.Once());
        }
        [Fact]
        public async void GetFormByFormName_ShouldReturnBadRequestResult_When_FormNumber_IsNull()
        {
            //Arrange
            String formName = null;
            _formInterface.Setup(t => t.GetFormByFormName(formName));

            //Act
            var result = _formController.GetFormByFormName(formName);

            //Assert
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<BadRequestObjectResult>("Form Name is null or empty");
            _formInterface.Verify(t => t.GetFormByFormName(formName), Times.Never());
        }
        [Fact]
        public async void GetFormByFormName_ShouldReturnBadRequestResult_When_FormNumber_IsEmpty()
        {
            //Arrange
            String formName = "";
            _formInterface.Setup(t => t.GetFormByFormName(formName));

            //Act
            var result = _formController.GetFormByFormName(formName);

            //Assert
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<BadRequestObjectResult>("Form Name is null or empty");
            _formInterface.Verify(t => t.GetFormByFormNumber(formName), Times.Never());
        }
        //GetFormByFormNumber
        [Fact]
        public async void GetFormByFormNumber_ShouldReturnOkResult_WhenSuccess()
        {
            //Arrange
            var formNumber = _fixture.Create<String>();
            var form = _fixture.Create<List<Form>>();
            _formInterface.Setup(t => t.GetFormByFormNumber(formNumber)).ReturnsAsync(form);

            //Action
            var result = _formController.GetFormByFormNumber(formNumber);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Task<IActionResult>>();
            result.Result.Should().BeAssignableTo<OkObjectResult>().Subject.Value.Should().Be(form);
            _formInterface.Verify(t => t.GetFormByFormNumber(formNumber), Times.Once());
        }
        [Fact]
        public async void GetFormByFormNumber_ShouldReturnNotFoundResult_WhenFormNotFound()
        {
            //Arrange
            var formNumber = _fixture.Create<String>();
            _formInterface.Setup(t => t.GetFormByFormNumber(formNumber)).Returns(Task.FromResult<List<Form>>(null));

            //Act
            var result = _formController.GetFormByFormNumber(formNumber);

            //Assert
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<NotFoundObjectResult>().Subject.Value.Should().Be("Form Not Found");
            _formInterface.Verify(t => t.GetFormByFormNumber(formNumber), Times.Once());

        }
        [Fact]
        public async void GetFormByFormNumber_ShouldReturnBadRequestResult_When_ErrorOccurs()
        {
            //Arrange
            var formNumber = _fixture.Create<String>();
            _formInterface.Setup(t => t.GetFormByFormNumber(formNumber)).Throws(new Exception("Error Occured"));

            //Act
            var result = _formController.GetFormByFormNumber(formNumber);

            //Assert
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<BadRequestObjectResult>("Error Occured");
            _formInterface.Verify(t => t.GetFormByFormNumber(formNumber), Times.Once());
        }
        [Fact]
        public async void GetFormByFormNumber_ShouldReturnBadRequestResult_When_FormNumber_IsNull()
        {
            //Arrange
            String formNumber = null;
            _formInterface.Setup(t => t.GetFormByFormNumber(formNumber));

            //Act
            var result = _formController.GetFormByFormNumber(formNumber);

            //Assert
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<BadRequestObjectResult>("Form Number is null or empty");
            _formInterface.Verify(t => t.GetFormByFormNumber(formNumber), Times.Never());
        }
        [Fact]
        public async void GetFormByFormNumber_ShouldReturnBadRequestResult_When_FormNumber_IsEmpty()
        {
            //Arrange
            String formNumber = "";
            _formInterface.Setup(t => t.GetFormByFormNumber(formNumber));

            //Act
            var result = _formController.GetFormByFormNumber(formNumber);

            //Assert
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<BadRequestObjectResult>("Form Number is null or empty");
            _formInterface.Verify(t => t.GetFormByFormNumber(formNumber), Times.Never());
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
            var result = _formController.AddForm(form);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Task<IActionResult>>();
            result.Result.Should().BeAssignableTo<OkObjectResult>().Subject.Value.Should().Be(formReturn);
            _formInterface.Verify(f => f.AddForm(form), Times.Once());
        }
        [Fact]
        public async void AddForm_ShouldReturnBadRequest_WhenExceptionIsThrown()
        {
            //Arrange
            var form = _fixture.Create<Form>();
            _formInterface.Setup(f => f.AddForm(form)).Throws(new Exception("Error occured"));

            //Act
            var result = _formController.AddForm(form);

            //Assert
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject.Value.Should().Be("Error occured");
            _formInterface.Verify(t => t.AddForm(form), Times.Once());

        }
        [Fact]
        public async void AddForm_ShouldReturnBadRequestResult_When_ErrorOccurs()
        {
            //Arrange
            var form = _fixture.Create<Form>();
            Form formReturn = null;
            _formInterface.Setup(f => f.AddForm(form)).ReturnsAsync(formReturn);

            //Act
            var result = _formController.AddForm(form);

            //Assert
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<BadRequestObjectResult>("Error Occured");
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
            var result = _formController.EditFormById( existingform);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Task<IActionResult>>();
            result.Result.Should().BeAssignableTo<OkObjectResult>().Subject.Value.Should().Be(newform);
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
            var result = _formController.EditFormById(existingform);

            //Assert
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<NotFoundObjectResult>().Subject.Value.Should().Be("Form Not Found");
            _formInterface.Verify(t => t.EditFormById(existingform), Times.Once());

        }
        [Fact]
        public async void EditFormById_ShouldReturnBadRequestResult_When_ErrorOccurs()
        {
            //Arrange
            var formId = _fixture.Create<Guid>();
            var existingform = _fixture.Create<Form>();
            Form newform = null ;
            _formInterface.Setup(f => f.EditFormById(existingform)).Throws(new Exception("Error occured"));
            

            //Act
            var result = _formController.EditFormById(existingform);

            //Assert
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<BadRequestObjectResult>("Error Occured");
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
            var result = _formController.DeleteFormById(formId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Task<IActionResult>>();
            result.Result.Should().BeAssignableTo<OkObjectResult>().Subject.Value.Should().Be(form);
            _formInterface.Verify(t => t.DeleteFormById(formId), Times.Once());
        }
        [Fact]
        public async void DeleteFormById_ShouldReturnNotFoundResult_WhenFormNotFound()
        {
            //Arrange
            var formId = _fixture.Create<Guid>();
            _formInterface.Setup(t => t.DeleteFormById(formId)).ReturnsAsync((Form)null);

            //Act
            var result = _formController.DeleteFormById(formId);

            //Assert
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<NotFoundObjectResult>().Subject.Value.Should().Be("Form Not found");
            _formInterface.Verify(t => t.DeleteFormById(formId), Times.Once());

        }
        [Fact]
        public async void DeleteFormById_ShouldReturnBadRequestResult_When_ErrorOccurs()
        {
            //Arrange
            var formId = _fixture.Create<Guid>();
            _formInterface.Setup(t => t.DeleteFormById(formId)).Throws(new Exception("Error Occured"));

            //Act
            var result = _formController.DeleteFormById(formId);

            //Assert
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<BadRequestObjectResult>("Error Occured");
            _formInterface.Verify(t => t.DeleteFormById(formId), Times.Once());
        }
    }
}