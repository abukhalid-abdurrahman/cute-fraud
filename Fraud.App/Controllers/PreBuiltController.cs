using System.Threading.Tasks;
using Fraud.Entities.DTOs;
using Fraud.Entities.DTOs.PreBuilt;
using Fraud.UseCase.PreBuilts;
using Microsoft.AspNetCore.Mvc;

namespace Fraud.App.Controllers
{
    [ApiController]
    [Route("api/[controller]/PreBuiltList")]
    public class PreBuiltController : ControllerBase
    {
        private readonly IGetPreBuiltEntitiesListUseCase _preBuiltListUseCase;

        public PreBuiltController(IGetPreBuiltEntitiesListUseCase preBuiltListUseCase)
        {
            _preBuiltListUseCase = preBuiltListUseCase;
        }
        
        [HttpGet]
        public async Task<Response<PreBuiltListDto>> GetPreBuiltList() =>
            Response<PreBuiltListDto>.FromReturnResult(await _preBuiltListUseCase.GetPreBuiltList());
    }
}