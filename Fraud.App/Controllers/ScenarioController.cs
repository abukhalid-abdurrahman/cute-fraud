using System.Threading.Tasks;
using Fraud.Entities.DTOs;
using Fraud.Entities.DTOs.Scenario;
using Fraud.UseCase.Scenario;
using Microsoft.AspNetCore.Mvc;

namespace Fraud.App.Controllers
{
    [ApiController]
    [Route("api/[controller]/Scenario")]
    public class ScenarioController : ControllerBase
    {
        private readonly IScenarioUseCase _scenarioUseCase;

        public ScenarioController(IScenarioUseCase scenarioUseCase)
        {
            _scenarioUseCase = scenarioUseCase;
        }

        [HttpGet]
        public async Task<Response<GraphScenarioDto>> GetScenario() =>
            Response<GraphScenarioDto>.FromReturnResult(await _scenarioUseCase.GetUserScenario());

        [HttpPost]
        public async Task<Response<bool>> SaveScenario(GraphScenarioDto graphScenarioDto) =>
            Response<bool>.FromReturnResult(await _scenarioUseCase.CreateUserScenario(graphScenarioDto));

        [HttpDelete]
        public async Task<Response<bool>> DeleteScenario() =>
            Response<bool>.FromReturnResult(await _scenarioUseCase.DeleteUserScenario());
    }
}