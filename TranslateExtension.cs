using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace TicTacToeGameProj
{
    [ContentProperty(nameof(Key))]
    public class TranslateExtension : IMarkupExtension<BindingBase>
    {
        public string Key { get; set; }

        public BindingBase ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrEmpty(Key))
                return null;

            return new Binding(
                path: $"[{Key}]",
                source: LocalizationService.Instance);
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
            => ProvideValue(serviceProvider);
    }
}
