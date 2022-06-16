using System;
using System.Threading.Tasks;
using Fraud.Concerns;
using Fraud.Entities.Models;

namespace Fraud.Infrastructure.Repository
{
    public interface IScenarioRepository : IDisposable
    {
        Task<ReturnResult<int>> CreateScenario(Scenario scenario);
        Task<ReturnResult<bool>> SetScenarioRule(int scenarioId, string scenarioRule);
        Task<ReturnResult<string>> GetScenarioRule(int scenarioId);
        Task<ReturnResult<bool>> DeleteScenario(int scenarioId);
    }
}