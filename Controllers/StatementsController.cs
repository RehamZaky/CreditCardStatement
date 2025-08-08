using CreditCardStatementApi.DTO;
using CreditCardStatementApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CreditCardStatementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            var response = await  _statementService.GetMonthStatement(date);
            return Ok(response);
        }

        [HttpGet("GetPeriodStatements")]
        public async Task<IActionResult> GetPeriodStatements([FromQuery] DatePeriodDTO periodDTO )
        {
            var response = await _statementService.GetPeriodStatements(periodDTO);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> PostStatement(StatementDTO statementDTO)
        {
            var response = await _statementService.AddStatement(statementDTO);
            return Ok(response);
        }
    }
}
