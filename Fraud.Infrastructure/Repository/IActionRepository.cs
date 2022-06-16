using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fraud.Concerns;
using Action = Fraud.Entities.Models.Action;

namespace Fraud.Infrastructure.Repository
{
    public interface IActionRepository : IDisposable
    {
        Task<ReturnResult<IEnumerable<Action>>> GetAllActions();
        Task<ReturnResult<IEnumerable<Action>>> GetActionById(int actionId);
    }
}