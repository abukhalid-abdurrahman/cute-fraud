using System.Threading.Tasks;
using Fraud.Entities.DTOs;
using Fraud.Entities.DTOs.Scenario;
using Fraud.UseCase.Scenario;

namespace Fraud.Interactor.Scenarios
{
    public partial class ScenarioInteractor : IScenarioUseCase
    {
        public async Task<Response<GetUserScenarioResponseDto>> GetUserScenario(int userId)
        {
            throw new System.NotImplementedException();
        }
    }
}