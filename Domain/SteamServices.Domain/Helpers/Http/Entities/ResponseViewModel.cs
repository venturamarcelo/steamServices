using SteamServices.Domain.Helpers.Http.Enums;

namespace SteamServices.Domain.Helpers.Http.Entities
{
    public class ResponseViewModel<T> 
    {
    public ResponseViewModel()
    {
        TypeReturn = ResponseTypeEnum.Success;
        Message = string.Empty;
        Obj = default(T);
    }

    public ResponseTypeEnum TypeReturn { get; set; }

    public string Message { get; set; }

    public T Obj { get; set; }
    }


   
}
