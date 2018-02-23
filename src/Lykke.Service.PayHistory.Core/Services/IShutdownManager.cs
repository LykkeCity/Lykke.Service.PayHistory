using System.Threading.Tasks;
using Common;

namespace Lykke.Service.PayHistory.Core.Services
{
    public interface IShutdownManager
    {
        Task StopAsync();

        void Register(IStopable stopable);
    }
}
