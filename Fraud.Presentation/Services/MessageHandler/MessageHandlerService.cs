using System.Threading.Tasks;

namespace Fraud.Presentation.Services.MessageHandler
{
    public class MessageHandlerService : IMessageHandlerService
    {

        public MessageHandlerService()
        {
        }
        
        public async Task HandleMessage(byte[] messageBuffer)
        {
            throw new System.NotImplementedException();
        }
    }
}