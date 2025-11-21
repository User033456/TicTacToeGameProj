using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
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
        /// <summary>
        /// Загрузка заднего фона
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            _config = ConfigManager.LoadConfig();

            ThemeManager.Initialize(_config);
            ThemeManager.ApplyBackgroundImage(ThemeBackgroundImage);

            // обновляем превью темы
            ThemePreviewImage.Source = $"{ThemeManager.CurrentTheme}.png";
            ThemeName.Text = ThemeManager.CurrentTheme;

            ApplyConfigToUi();
        }

        private void ApplyConfigToUi()
        {
            LanguageValueLabel.Text = _config.ContainsKey("language") ? _config["language"] : "English";

            bool musicOn = _config.TryGetValue("music", out var m) &&
                           string.Equals(m, "ON", StringComparison.OrdinalIgnoreCase);

            MusicSwitch.IsToggled = musicOn;
            MusicValueLabel.Text = musicOn ? "ON" : "OFF";
        }

        // левая стрелка
        private void OnPrevThemeTapped(object sender, EventArgs e)
        {
            ThemeManager.SwitchTheme(-1);
            ThemeManager.ApplyBackgroundImage(ThemeBackgroundImage);

            ThemePreviewImage.Source = $"{ThemeManager.CurrentTheme}.png";
            ThemeName.Text = ThemeManager.CurrentTheme;

            _config["theme"] = ThemeManager.CurrentTheme;
            ConfigManager.SaveConfig(_config);
        }

        // правая стрелка
        private void OnNextThemeTapped(object sender, EventArgs e)
        {
            ThemeManager.SwitchTheme(+1);
            ThemeManager.ApplyBackgroundImage(ThemeBackgroundImage);

            ThemePreviewImage.Source = $"{ThemeManager.CurrentTheme}.png";
            ThemeName.Text = ThemeManager.CurrentTheme;

            _config["theme"] = ThemeManager.CurrentTheme;
            ConfigManager.SaveConfig(_config);
        }

        private async void OnBackButtonTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
