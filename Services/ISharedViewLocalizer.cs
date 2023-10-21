using Microsoft.Extensions.Localization;

namespace PromerceHelpDesk.Web.Services
{
    public interface ISharedViewLocalizer
    {
        public LocalizedString this[string key]
        {
            get;
        }

        LocalizedString GetLocalizedString(string key);
    }
}
