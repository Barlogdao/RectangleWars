
namespace RB.Services.ServiceLocator
{
    public interface IServiceLocator<T>
    {
        ST Register<ST> (ST newService) where ST:T;
        void Unregister<ST> (ST service) where ST:T;
        ST Get<ST> () where ST:T;
    }
}

