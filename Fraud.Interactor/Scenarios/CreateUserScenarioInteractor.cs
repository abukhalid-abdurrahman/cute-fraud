using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fraud.Entities.DTOs.Scenario;
using Fraud.Entities.DTOs.State;
using Fraud.Entities.Models;
using Fraud.Infrastructure.Repository;
using Fraud.UseCase.Scenario;

namespace Fraud.Interactor.Scenarios
{
    public class StateEdgeDto
    {
        public int FromStateId { get; set; }
        public int ToStateId { get; set; }
    }

    public class GraphScenarioDto
    {
        public List<StateRequestDto> StateVertices { get; set; }
        public List<StateEdgeDto> StateEdges { get; set; }

        public int StateVerticesCount => StateVertices.Count;
        public int StateEdgesCount => StateEdges.Count;
    }

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
            statesList.Add(createUserScenarioRequestDto.State);

            // TODO: Create states from array
            await _stateRepository.CreateState(statesList);
        }
    }
}