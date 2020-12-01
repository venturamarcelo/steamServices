using System.Net.Http;

namespace SteamServices.Domain.Helpers.Http.Interfaces
{
    public interface IHttpMethod<in TParameters>
    {
        HttpResponseMessage Execute(TParameters parameters, string suffixComplement = null);
    }
}
