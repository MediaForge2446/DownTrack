using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace DownTrack.Infrastructure.Localization;

[MarkupExtensionReturnType(typeof(object))]
public sealed class LocExtension : MarkupExtension
{
    public string Key { get; set; } = string.Empty;

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return new Binding($"[{Key}]")
        {
            Source = LocalizationService.Instance,
            Mode = BindingMode.OneWay
        }.ProvideValue(serviceProvider);
    }
}
