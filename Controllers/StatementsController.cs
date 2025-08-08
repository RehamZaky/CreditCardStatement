using CreditCardStatementApi.DTO;
using CreditCardStatementApi.Services.Statements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CreditCardStatementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
    public class StatementsController : ControllerBase
    {
        private readonly IStatementService _statementService;
        public StatementsController(IStatementService statementService)
        {
            _statementService = statementService;
        }

        [HttpPost("GetStatement")]
        public async Task<IActionResult> GetStatement([FromBody] DateDTO date)
        {
            try { 
            var response = await  _statementService.GetMonthStatement(date);
                if (response == null)
                {
                    return Ok(new List<StatementDTO>());
                }
            return Ok(response);
            }
            
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetPeriodStatements")]
        public async Task<IActionResult> GetPeriodStatements([FromQuery] DatePeriodDTO periodDTO )
        {
            try
            {
                var response = await _statementService.GetPeriodStatements(periodDTO);
                return Ok(response);
            }
            
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
    }
}

        [HttpPost]
        public async Task<IActionResult> PostStatement(StatementDTO statementDTO)
        {
            try
            {
                var response = await _statementService.AddStatement(statementDTO);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
