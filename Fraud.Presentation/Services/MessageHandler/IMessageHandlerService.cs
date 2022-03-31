using System.Threading.Tasks;

namespace Fraud.Presentation.Services.MessageHandler
{
    public interface IMessageHandlerService
    {
        public Task HandleMessage(byte[] messageBuffer);
    }
}