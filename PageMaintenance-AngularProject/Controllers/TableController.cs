using Microsoft.AspNetCore.Mvc;
using PageMaintenance_AngularProject.Services;

namespace PageMaintenance_AngularProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableController : Controller
    {
       
        public ITableInterface _tableInterface { get; }

        public TableController(ITableInterface tableInterface)
        {
            _tableInterface = tableInterface;
        }
        [HttpGet("tableNames")]
        public async Task<IActionResult> GetAllTableNames()
        {
            try
            {
                var tableNames = await _tableInterface.GetTableNames();
                if (tableNames == null)
                {
                    return NotFound();
                }
                return Ok(tableNames);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("table/{id}")]
        public async Task<IActionResult> GetTableById([FromRoute]Guid id)
        {
            try
            {   
                
                var table = await _tableInterface.GetTableById(id);
                if (table == null)
                {
                    return NotFound();
                }
                return Ok(table);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
