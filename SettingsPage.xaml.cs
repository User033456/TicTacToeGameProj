using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Maui.Views;
using TicTacToeGameProj.Services;

namespace TicTacToeGameProj
{
    public partial class SettingsPage : ContentPage
    {
        private Dictionary<string, string> _config = new();

        public SettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // грузим конфиг
            _config = ConfigManager.LoadConfig();

            // ---- ТЕМА ----
            ThemeManager.Initialize(_config);
            ThemeManager.ApplyBackgroundImage(ThemeBackgroundImage);
            ThemeName.Text = ThemeManager.CurrentTheme;
            ThemePreviewImage.Source = $"{ThemeManager.CurrentTheme}.png";

            // ---- ЯЗЫК ----
            LanguageManager.Initialize(_config);
            LanguageValueLabel.Text = LanguageManager.CurrentLanguage.DisplayName;

            // ---- МУЗЫКА ----
            ApplyConfigToUi();

            // язык мог нормализоваться, сохраним
            _config["language"] = LanguageManager.CurrentLanguage.DisplayName;
            ConfigManager.SaveConfig(_config);
        }

        private void ApplyConfigToUi()
        {
            bool musicOn = _config.TryGetValue("music", out var m)
                           && string.Equals(m, "ON", StringComparison.OrdinalIgnoreCase);

            MusicSwitch.IsToggled = musicOn;
            MusicValueLabel.Text = musicOn ? "ON" : "OFF";
        }

        // ==== смена темы стрелками ====

        private void OnPrevThemeTapped(object sender, EventArgs e)
        {
            ThemeManager.SwitchTheme(-1);

            ThemeManager.ApplyBackgroundImage(ThemeBackgroundImage);
            ThemePreviewImage.Source = $"{ThemeManager.CurrentTheme}.png";
            ThemeName.Text = ThemeManager.CurrentTheme;

            _config["theme"] = ThemeManager.CurrentTheme;
            ConfigManager.SaveConfig(_config);
        }

        private void OnNextThemeTapped(object sender, EventArgs e)
        {
            ThemeManager.SwitchTheme(+1);

            ThemeManager.ApplyBackgroundImage(ThemeBackgroundImage);
            ThemePreviewImage.Source = $"{ThemeManager.CurrentTheme}.png";
            ThemeName.Text = ThemeManager.CurrentTheme;

            _config["theme"] = ThemeManager.CurrentTheme;
            ConfigManager.SaveConfig(_config);
        }

        // ==== выбор языка через popup ====

        private async void OnLanguageTapped(object sender, EventArgs e)
        {
            var popup = new LanguagePopup();
            var result = await this.ShowPopupAsync(popup) as string; // "en", "ru" или null

            if (string.IsNullOrEmpty(result))
                return;

            var lang = LanguageManager.Languages.FirstOrDefault(l => l.Key == result);
            if (lang == null)
                return;

            LanguageManager.SetLanguage(lang);
            LanguageValueLabel.Text = lang.DisplayName;

            _config["language"] = lang.DisplayName;
            ConfigManager.SaveConfig(_config);
        }

        // ==== кнопка назад ====

        private async void OnBackButtonTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
