using System.Threading.Tasks;
using Fraud.Entities.DTOs.Scenario;
using Fraud.Entities.Models;
using Fraud.Infrastructure.Repository;
using Fraud.UseCase.Scenario;
using Newtonsoft.Json;

namespace Fraud.Interactor.Scenarios
{
    public partial class ScenarioInteractor : IScenarioUseCase
    {
        private readonly IStateRepository _stateRepository;
        private readonly ILocalScenarioRepository _localScenarioRepository;

        public ScenarioInteractor(IStateRepository stateRepository, 
            ILocalScenarioRepository localScenarioRepository)
        {
            _stateRepository = stateRepository;
            _localScenarioRepository = localScenarioRepository;
        }
        
        public async Task CreateUserScenario(GraphScenarioDto graphScenarioDto)
        {
            foreach (var stateVertexDto in graphScenarioDto.StateVertices)
            {
                stateVertexDto.UserId = 0;
                stateVertexDto.StateId = await _stateRepository.CreateState(stateVertexDto);
                foreach (var stateAction in stateVertexDto.ActionTypes)
                {
                    await _stateRepository.SetStateAction(stateVertexDto.StateId.Value, stateAction.ActionId);
                }
            }
            
            var graphScenarioRuleContent = JsonConvert.SerializeObject(graphScenarioDto);
            var scenarioEntity = new Scenario()
            {
                UserId = 0,
                Rule = graphScenarioRuleContent
            };
            await _localScenarioRepository.CreateScenario(scenarioEntity);
        }
    }
}