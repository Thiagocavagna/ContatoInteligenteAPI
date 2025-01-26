using ContatoInteligenteAPI.Models;
using ContatoInteligenteAPI.Services.GitHub;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContatoInteligenteAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = "BasicAuth")]
    public class ContatoInteligenteController : ControllerBase
    {

        private readonly ILogger<ContatoInteligenteController> _logger;
        private readonly IGitHubApplicationService _gitHubApplicationService;

        public ContatoInteligenteController(ILogger<ContatoInteligenteController> logger, IGitHubApplicationService gitHubApplicationService)
        {
            _logger = logger;
            _gitHubApplicationService = gitHubApplicationService;
        }

        [HttpGet("repositories/{userName}")]
        public async Task<IActionResult> GetRepositoriesByUserAsync([FromRoute] string userName, [FromQuery] GitHubRepositoryQuery query)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);          

            try
            {
                var response = await _gitHubApplicationService.GetRepositoriesByUserAsync(userName, query);

                if(!response.result.Success)
                    return BadRequest(response.result);

                return Ok(response.data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar repositórios do usuário");

                return Problem(
                    title: "Erro Interno no Servidor",
                    detail: "Ocorreu um erro inesperado ao tentar buscar os repositórios do usuário.",
                    statusCode: 500);
            }
        }
    }
}
