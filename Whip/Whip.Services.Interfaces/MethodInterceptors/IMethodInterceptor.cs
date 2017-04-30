using System;

namespace Whip.Services.Interfaces
{
    public interface IMethodInterceptor
    {
        T TryMethod<T>(Func<T> method, T defaultReturnValue, string additionalErrorInfo = null) where T : class;

        void TryMethod(Action method, string additionalErrorInfo = null);
    }
}
