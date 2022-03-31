using System.Threading.Tasks;
using Fraud.Presentation.Enums;

namespace Fraud.Presentation.Services.MessageNotifier
{
    public interface IMessageNotifier
    {
        public Task Notify<TMessageObject>(TMessageObject messageObject, MessageType messageType);
    }
}