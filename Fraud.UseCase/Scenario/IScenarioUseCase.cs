using System.Threading.Tasks;
using Fraud.Entities.DTOs;
using Fraud.Entities.DTOs.Scenario;

namespace Fraud.UseCase.Scenario
{
    public interface IScenarioUseCase
    {
        Task CreateUserScenario(GraphScenarioDto graphScenarioDto);
        Task<Response<GetUserScenarioResponseDto>> GetUserScenario(int userId);
    }
}