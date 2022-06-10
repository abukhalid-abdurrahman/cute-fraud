using System.Collections.Generic;
using System.Threading.Tasks;
using Fraud.Entities.DTOs.Scenario;
using Fraud.Entities.DTOs.State;
using Fraud.Entities.Models;
using Fraud.Infrastructure.Repository;
using Fraud.UseCase.Scenario;

namespace Fraud.Interactor.Scenarios
{
    public partial class ScenarioInteractor : IScenarioUseCase
    {
        private readonly IStateRepository _stateRepository;
        private readonly IScenarioRepository _scenarioRepository;

        public ScenarioInteractor(IStateRepository stateRepository, 
            IScenarioRepository scenarioRepository)
        {
            _stateRepository = stateRepository;
            _scenarioRepository = scenarioRepository;
        }
        
        public async Task CreateUserScenario(CreateUserScenarioRequestDto createUserScenarioRequestDto)
        {
            var statesList = new List<State>();

            StateRequestDto currentStateObj = null;
            int iter = 0;
            int objCount = createUserScenarioRequestDto.State.NextStates.Length;
            do
            {
                statesList.Add(new State());
                currentStateObj = createUserScenarioRequestDto.State.NextStates[iter];
                iter++;
                objCount--;
            } while (currentStateObj == null || objCount > 0);

            
            await _stateRepository.CreateState(statesList);
        }
    }
}