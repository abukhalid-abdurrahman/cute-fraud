using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Fraud.Concerns;
using Fraud.Concerns.FaultHandling;
using Fraud.Entities.DTOs.Scenario;
using Fraud.Entities.Models;
using Fraud.Infrastructure.Repository;
using Newtonsoft.Json;

namespace Fraud.Infrastructure.Implementation.PostgreSqlRepository
{
    public class InMemoryScenarioRepository : ILocalScenarioRepository
    {
        private readonly IScenarioRepository _scenarioRepository;
        private ConcurrentDictionary<int, GraphScenarioDto> _scenarioGraphStorage;

        private bool _isDisposed = false;

        public InMemoryScenarioRepository(IScenarioRepository scenarioRepository)
        {
            _scenarioRepository = scenarioRepository;
            _scenarioGraphStorage = new ConcurrentDictionary<int, GraphScenarioDto>();
        }
        
        public async Task<ReturnResult<int>> CreateScenario(Scenario scenario)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(InMemoryScenarioRepository));

            if (scenario == null)
                throw new ArgumentNullException(nameof(scenario));

            var result = new ReturnResult<int>();
            var errorMessageTemplate = "Scenario creation failed in method InMemoryScenarioRepository.CreateScenario! Reason: {0}";

            var scenarioCreationResult = await _scenarioRepository.CreateScenario(scenario);
            if (!scenarioCreationResult.IsSuccessfully)
            {
                FaultHandler.HandleError(ref result, 
                    string.Format(errorMessageTemplate, scenarioCreationResult.Message));
                return result;
            }
            
            var scenarioGraph = JsonConvert.DeserializeObject<GraphScenarioDto>(scenario.Rule);
            if (scenarioGraph == null)
            {
                FaultHandler.HandleError(ref result, 
                    string.Format(errorMessageTemplate, $"{scenario.Rule} failed to be deserialized into object of type {nameof(GraphScenarioDto)}"));
                return result;
            }

            scenario.Id = scenarioCreationResult.Result;
            if(!_scenarioGraphStorage.ContainsKey(scenario.Id))
                _scenarioGraphStorage.TryAdd(scenario.Id, scenarioGraph);
            else
                _scenarioGraphStorage[scenario.Id] = scenarioGraph;

            return ReturnResult<int>.SuccessResult(scenario.Id);
        }

        public async Task<ReturnResult<bool>> SetScenarioRule(int scenarioId, string scenarioRule)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(InMemoryScenarioRepository));

            var result = new ReturnResult<bool>();
            var errorMessageTemplate = "Scenario updating failed in method InMemoryScenarioRepository.SetScenarioRule! Reason: {0}";
            
            var scenarioRuleUpdatingResult = await _scenarioRepository.SetScenarioRule(scenarioId, scenarioRule);
            if (!scenarioRuleUpdatingResult.IsSuccessfully)
            {
                FaultHandler.HandleError(ref result, 
                    string.Format(errorMessageTemplate, scenarioRuleUpdatingResult.Message));
                return result;
            }
            
            var scenarioGraph = JsonConvert.DeserializeObject<GraphScenarioDto>(scenarioRule);
            if (scenarioGraph == null)
            {
                FaultHandler.HandleError(ref result, 
                    string.Format(errorMessageTemplate, $"{scenarioRule} failed to be deserialized into object of type {nameof(GraphScenarioDto)}"));
                return result;
            }
            
            if (!_scenarioGraphStorage.ContainsKey(scenarioId))
                _scenarioGraphStorage.TryAdd(scenarioId, scenarioGraph);
            else
                _scenarioGraphStorage[scenarioId] = scenarioGraph;
            
            return ReturnResult<bool>.SuccessResult(true);
        }

        public async Task<ReturnResult<string>> GetScenarioRule(int scenarioId)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(InMemoryScenarioRepository));

            var result = new ReturnResult<string>();
            var errorMessageTemplate = "Scenario updating failed in method InMemoryScenarioRepository.SetScenarioRule! Reason: {0}";

            if (_scenarioGraphStorage.ContainsKey(scenarioId))
                return ReturnResult<string>.SuccessResult(_scenarioGraphStorage[scenarioId].ToString());

            var scenarioRuleResult = await _scenarioRepository.GetScenarioRule(scenarioId);
            if (!scenarioRuleResult.IsSuccessfully)
            {
                FaultHandler.HandleError(ref result, 
                    string.Format(errorMessageTemplate, scenarioRuleResult.Message));
                return result;
            }
            
            var scenarioGraph = JsonConvert.DeserializeObject<GraphScenarioDto>(scenarioRuleResult.Result);
            if (scenarioGraph == null)
            {
                FaultHandler.HandleError(ref result, 
                    string.Format(errorMessageTemplate, $"{scenarioRuleResult.Result} failed to be deserialized into object of type {nameof(GraphScenarioDto)}"));
                return result;
            }
            
            _scenarioGraphStorage.TryAdd(scenarioId, scenarioGraph);
            return scenarioRuleResult;
        }

        public async Task<ReturnResult<bool>> DeleteScenario(int scenarioId)
        {            
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(InMemoryScenarioRepository));

            var returnResult = new ReturnResult<bool>();
            var errorMessageTemplate = "Scenario deleting failed in method InMemoryScenarioRepository.DeleteScenario! Reason: {0}";
            
            var deletionScenarioResult = await _scenarioRepository.DeleteScenario(scenarioId);
            if (!deletionScenarioResult.IsSuccessfully)
            {
                FaultHandler.HandleError(ref returnResult, 
                    string.Format(errorMessageTemplate, deletionScenarioResult.Message));
                return returnResult;
            }

            if (_scenarioGraphStorage.ContainsKey(scenarioId))
            {
                _scenarioGraphStorage.TryRemove(scenarioId, out _);
                returnResult.Result = true;
            }
            else
                FaultHandler.HandleWarning(ref returnResult, 
                    "Scenario removing failed!", "Scenario not exist in InMemoryScenario storage!");
            
            return returnResult;
        }

        public async Task<ReturnResult<GraphScenarioDto>> GetScenarioGraph(int scenarioId)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(InMemoryScenarioRepository));

            var returnResult = new ReturnResult<GraphScenarioDto>();
            var errorMessageTemplate = "Scenario fetching failed in method InMemoryScenarioRepository.GetScenarioGraph! Reason: {0}";

            if (_scenarioGraphStorage.ContainsKey(scenarioId))
                return ReturnResult<GraphScenarioDto>.SuccessResult(_scenarioGraphStorage[scenarioId]);

            var scenarioRuleResult = await _scenarioRepository.GetScenarioRule(scenarioId);
            if (!scenarioRuleResult.IsSuccessfully)
            {
                FaultHandler.HandleError(ref returnResult, 
                    string.Format(errorMessageTemplate, scenarioRuleResult.Message));
                return returnResult;
            }
            
            var scenarioGraph = JsonConvert.DeserializeObject<GraphScenarioDto>(scenarioRuleResult.Result);
            if (scenarioGraph == null)
            {
                FaultHandler.HandleError(ref returnResult, 
                    string.Format(errorMessageTemplate, $"{scenarioRuleResult.Result} failed to be deserialized into object of type {nameof(GraphScenarioDto)}"));
                return returnResult;
            }
            _scenarioGraphStorage.TryAdd(scenarioId, scenarioGraph);
            
            return ReturnResult<GraphScenarioDto>.SuccessResult(scenarioGraph);
        }

        private void ReleaseUnmanagedResources()
        {
            if(_isDisposed) return;
            
            _scenarioGraphStorage.Clear();
            _scenarioGraphStorage = null;
            
            _scenarioRepository.Dispose();

            _isDisposed = true;
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~InMemoryScenarioRepository()
        {
            ReleaseUnmanagedResources();
        }
    }
}