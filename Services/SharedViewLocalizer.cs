using Microsoft.Extensions.Localization;
using PromerceHelpDesk.Web.Resources;
using System.Reflection;

namespace PromerceHelpDesk.Web.Services
{
    public class SharedViewLocalizer : ISharedViewLocalizer
    {
        private readonly IStringLocalizer _localizer;

        public SharedViewLocalizer(IStringLocalizerFactory factory)
        {
            var type = typeof(SharedResources);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizer = factory.Create("SharedResources", assemblyName.Name);
        }

        public LocalizedString this[string key] => _localizer[key];

        public LocalizedString GetLocalizedString(string key)
        {
            return _localizer[key];
        }
    }
}
