using System.Collections.Generic;
using System.Threading.Tasks;
using Fraud.Entities.DTOs.Scenario;
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

            // TODO: Get user id, by current login user. From users manager.
            var userId = 0;
            
            // TODO: Create current state
            statesList.Add(createUserScenarioRequestDto.StateVertex);

            // TODO: Create states from array
            await _stateRepository.CreateState(statesList);
        }
    }
}