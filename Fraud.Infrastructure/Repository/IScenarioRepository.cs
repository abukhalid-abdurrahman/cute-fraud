using System;
using System.Threading.Tasks;
using Fraud.Entities.Models;

namespace Fraud.Infrastructure.Repository
{
    public interface IScenarioRepository : IDisposable
    {
        Task<int> CreateScenario(Scenario scenario);
        Task SetScenarioRule(int scenarioId, string scenarioRule);
        Task<string> GetScenarioRule(int scenarioId);
        Task DeleteScenario(int scenarioId);
    }
}