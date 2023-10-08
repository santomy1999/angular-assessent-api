using Microsoft.AspNetCore.Mvc;
using PageMaintenance_AngularProject.Models;
using PageMaintenance_AngularProject.Services;

namespace PageMaintenance_AngularProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormController : Controller
    {
        private readonly IFormInterface _formInterface;

        public FormController(IFormInterface formInterface)
        {
            _formInterface = formInterface;
        }
        //[HttpGet("formsAll")]
        //public async Task<IActionResult> GetAllForms()
        //{
        //    try
        //    {
        //        var forms = await _formInterface.GetAllForms();
        //        if (forms.Any())
        //        {
        //            return Ok(forms);
        //        }
        //        return NotFound("No Forms Found");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        [HttpGet("formsAll")]
        public async Task<IActionResult> GetAllForms([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 30)
        {
            try
            {
                var forms = await _formInterface.GetAllForms(pageNumber, pageSize);
                if (forms.Any())
                {
                    return Ok(forms);
                }
                return NotFound("No Forms Found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("formId:{id}")]
        public async Task<IActionResult> GetFormById([FromRoute] Guid id)
        {
            try
            {
                var form = await _formInterface.GetFormById(id);
                if (form == null)
                {
                    return NotFound("Form Not Found");
                }
                return Ok(form);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("formName:{formName}")]
        public async Task<IActionResult> GetFormByFormName([FromRoute] string formName, [FromQuery] int pageNumber=1, [FromQuery] int pageSize = 30)
        {
            try
            {
                if (formName == null || formName == "")
                {
                    return BadRequest("Form Name is null or empty");
                }
                var forms = await _formInterface.GetFormByFormName(formName ,pageNumber , pageSize);
                if (forms != null && forms.Any())
                {
                    return Ok(forms);
                }
                return NotFound("Form Not Found");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("formNumber:{formNumber}")]
        public async Task<IActionResult> GetFormByFormNumber([FromRoute] string formNumber, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 30)
        {
            try
            {
                if(formNumber == null|| formNumber =="")
                {
                    return BadRequest("Form Number is null or empty");
                }
                var forms = await _formInterface.GetFormByFormNumber(formNumber ,pageNumber, pageSize);
                if (forms != null && forms.Any())
                {
                    return Ok(forms);
                }
                return NotFound("Form Not Found");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //Post Functions
        [HttpPost("form")]
        public async Task<IActionResult> AddForm([FromBody] Form form)
        {
            try
            {
                form.Id = Guid.NewGuid();
                var result = await _formInterface.AddForm(form);
                if (result!=null)
                {
                    return Ok(result);
                }
                return BadRequest("Error occured");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Update functions

        [HttpPatch("form")]
        public async Task<IActionResult> EditFormById([FromBody] Form newForm)
        {
            try
            {
                var form = await _formInterface.EditFormById(newForm);
                if(form != null)
                {
                    return Ok(form);
                }
                return NotFound("Form Not Found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //Delete Functions
        [HttpDelete("formNumber:{formId}")]
        public async Task<IActionResult> DeleteFormById([FromRoute] Guid formId)
        {
            try
            {
                var form = await _formInterface.DeleteFormById(formId);
                if( form == null)
                {
                    return NotFound("Form Not found");
                } 
                return Ok(form);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
