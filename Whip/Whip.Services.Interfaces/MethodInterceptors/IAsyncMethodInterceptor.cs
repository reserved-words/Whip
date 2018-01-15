using System.Threading.Tasks;
using Whip.Common.Enums;

namespace Whip.Services.Interfaces
{
    public interface IAsyncMethodInterceptor
    {
        Task<T> TryMethod<T>(Task<T> task, T defaultReturnValue, WebServiceType type, string additionalErrorInfo = null);

        Task TryMethod(Task task, WebServiceType type, string additionalErrorInfo = null);
    }
}
