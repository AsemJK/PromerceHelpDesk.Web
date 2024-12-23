using PromerceCRM.API.Models.ModelsViews;


namespace PromerceCRM.API.Services.Interfaces
{
    public interface IAccountGroupService
    {
        IEnumerable<AccountGroupDto> GetAll();
    }
}
