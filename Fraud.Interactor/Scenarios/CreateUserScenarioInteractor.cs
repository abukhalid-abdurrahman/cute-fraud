using System;
using System.Threading.Tasks;
using Fraud.Concerns;
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
        
        public async Task<ReturnResult<bool>> CreateUserScenario(GraphScenarioDto graphScenarioDto)
        {
            throw new ArgumentNullException();
        }
    }
}