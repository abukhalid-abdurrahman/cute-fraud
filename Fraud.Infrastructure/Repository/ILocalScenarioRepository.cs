using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Fraud.Entities.DTOs.Scenario;
using Fraud.Entities.Models;
using Newtonsoft.Json;

namespace Fraud.Infrastructure.Repository
{
    public interface ILocalScenarioRepository : IScenarioRepository
    {
        Task<GraphScenarioDto> GetScenarioGraph(int scenarioId);
    }

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
        
        public async Task<int> CreateScenario(Scenario scenario)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(InMemoryScenarioRepository));
            scenario.Id = await _scenarioRepository.CreateScenario(scenario);
            
            var scenarioGraph = JsonConvert.DeserializeObject<GraphScenarioDto>(scenario.Rule);
            if(scenarioGraph != null && !_scenarioGraphStorage.ContainsKey(scenario.Id))
                _scenarioGraphStorage.TryAdd(scenario.Id, scenarioGraph);
            else if (_scenarioGraphStorage.ContainsKey(scenario.Id))
                _scenarioGraphStorage[scenario.Id] = scenarioGraph;
            return scenario.Id;
        }

        public async Task SetScenarioRule(int scenarioId, string scenarioRule)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(InMemoryScenarioRepository));

            await _scenarioRepository.SetScenarioRule(scenarioId, scenarioRule);

            var scenarioGraph = JsonConvert.DeserializeObject<GraphScenarioDto>(scenarioRule);
            if (!_scenarioGraphStorage.ContainsKey(scenarioId))
                _scenarioGraphStorage.TryAdd(scenarioId, scenarioGraph);
            else
                _scenarioGraphStorage[scenarioId] = scenarioGraph;
        }

        public async Task<string> GetScenarioRule(int scenarioId)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(InMemoryScenarioRepository));

            if (_scenarioGraphStorage.ContainsKey(scenarioId))
                return _scenarioGraphStorage[scenarioId].ToString();

            var scenarioRuleStr = await _scenarioRepository.GetScenarioRule(scenarioId);
            if (string.IsNullOrEmpty(scenarioRuleStr))
                throw new Exception("Scenario rule is empty or null!");
            var scenarioGraph = JsonConvert.DeserializeObject<GraphScenarioDto>(scenarioRuleStr);
            _scenarioGraphStorage.TryAdd(scenarioId, scenarioGraph);

            return scenarioRuleStr;
        }

        public async Task DeleteScenario(int scenarioId)
        {            
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(InMemoryScenarioRepository));

            await _scenarioRepository.DeleteScenario(scenarioId);

            if (_scenarioGraphStorage.ContainsKey(scenarioId))
                _scenarioGraphStorage.TryRemove(scenarioId, out _);
        }

        public async Task<GraphScenarioDto> GetScenarioGraph(int scenarioId)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(InMemoryScenarioRepository));

            if (_scenarioGraphStorage.ContainsKey(scenarioId))
                return _scenarioGraphStorage[scenarioId];
            
            var scenarioRuleStr = await _scenarioRepository.GetScenarioRule(scenarioId);
            if (string.IsNullOrEmpty(scenarioRuleStr))
                throw new Exception("Scenario rule is empty or null!");
            var scenarioGraph = JsonConvert.DeserializeObject<GraphScenarioDto>(scenarioRuleStr);
            _scenarioGraphStorage.TryAdd(scenarioId, scenarioGraph);
            
            return scenarioGraph;
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