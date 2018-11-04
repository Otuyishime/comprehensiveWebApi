using System;
namespace testWebAPI.Infrastructure.Etags
{
    public interface IEtagHandlerFeature
    {
        bool NoneMatch(IEtaggable entity);
    }
}
