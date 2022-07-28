using System.Threading.Tasks;
using Fraud.Concerns;
using Fraud.Entities.DTOs.Scenario;

namespace Fraud.UseCase.Scenario
{
    public interface IScenarioUseCase
    {
        Task<ReturnResult<bool>> CreateUserScenario(GraphScenarioDto graphScenarioDto);
        Task<ReturnResult<bool>> DeleteUserScenario();
        Task<ReturnResult<GraphScenarioDto>> GetUserScenario();
    }
}