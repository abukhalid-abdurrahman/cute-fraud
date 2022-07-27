using System.Threading.Tasks;
using Fraud.Concerns;
using Fraud.Entities.DTOs;
using Fraud.Entities.DTOs.Scenario;
using Fraud.UseCase.Scenario;

namespace Fraud.Interactor.Scenarios
{
    public partial class ScenarioInteractor : IScenarioUseCase
    {
        public async Task<ReturnResult<GraphScenarioDto>> GetUserScenario()
        {
            throw new System.NotImplementedException();
        }
    }
}