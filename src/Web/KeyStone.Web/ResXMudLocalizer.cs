using KeyStone.Web.Shared.Resources;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace KeyStone.Web
{
    internal class ResXMudLocalizer : MudLocalizer
    {
        private IStringLocalizer _localization;

        public ResXMudLocalizer(IStringLocalizer<LocalizationResource> localizer)
        {
            _localization = localizer;
        }
        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                string text = _localization[name, arguments];
                if (!string.IsNullOrEmpty(text))
                {
                    return new(name, text);
                }
                else
                {
                    Console.WriteLine($"Key not found: {name}");
                    return new(name, name, true);
                }
            }
        }
        public override LocalizedString this[string key]
        {
            get
            {

                string text = _localization[key];
                if (!string.IsNullOrEmpty(text))
                {
                    return new(key, text);
                }
                else
                {
                    Console.WriteLine($"Key not found: {key}");
                    return new(key, key, true);
                }
            }
        }
    }
}
