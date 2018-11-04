using System;
namespace testWebAPI.Infrastructure.Etags
{
    public interface IEtaggable
    {
        string GetEtag();
    }
}
