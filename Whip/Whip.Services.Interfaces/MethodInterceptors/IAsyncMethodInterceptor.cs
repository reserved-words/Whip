using System;
using System.Threading.Tasks;

namespace Whip.Services.Interfaces
{
    public interface IAsyncMethodInterceptor
    {
        Task<T> TryMethod<T>(Task<T> task, T defaultReturnValue, string additionalErrorInfo = null);

        Task TryMethod(Task task, string additionalErrorInfo = null);
    }
}
