using System.Text;
using System.Threading.Tasks;
using Fraud.Entities.DTOs.Order;
using Fraud.UseCase.Cards;
using Newtonsoft.Json;

namespace Fraud.Presentation.Services.MessageHandler
{
    public class MessageHandlerService : IMessageHandlerService
    {
        private readonly ICardAnalyzerUseCase _cardAnalyzerUseCase;

        public MessageHandlerService(ICardAnalyzerUseCase cardAnalyzerUseCase)
        {
            _cardAnalyzerUseCase = cardAnalyzerUseCase;
        }
        
        public async Task HandleMessage(byte[] messageBuffer)
        {
            var transactionContent = Encoding.UTF8.GetString(messageBuffer);
            var transactionObject = JsonConvert.DeserializeObject<OrderRequest>(transactionContent);
            await _cardAnalyzerUseCase.AnalyzeCard(transactionObject);
        }
    }
}