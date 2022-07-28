using System.Threading.Tasks;
using Fraud.Concerns;
using Fraud.Entities.DTOs.Scenario;

namespace Fraud.Infrastructure.Repository
{
    public interface ILocalScenarioRepository : IScenarioRepository
    {
        Task<ReturnResult<GraphScenarioDto>> GetScenarioGraph(int scenarioId);
        Task<ReturnResult<GraphScenarioDto>> GetScenarioGraphByUserId(int userId);
    }
}