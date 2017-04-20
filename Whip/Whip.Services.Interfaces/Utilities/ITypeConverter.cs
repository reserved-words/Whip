
namespace Whip.Services.Interfaces
{
    public interface ITypeConverter<T1,T2> where T1 : class where T2 : class
    {
        void Convert(T2 source, T1 destination);

        void Convert(T1 source, T2 destination);
    }
}
