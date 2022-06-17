using System.Threading.Tasks;
using Fraud.Concerns;
using Fraud.Entities.DTOs.PreBuilt;

namespace Fraud.UseCase.PreBuilts
{
    public interface IGetPreBuiltEntitiesListUseCase
    {
        public Task<ReturnResult<PreBuiltListDto>> GetPreBuiltList();
    }
}