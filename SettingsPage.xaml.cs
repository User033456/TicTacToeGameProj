using CommunityToolkit.Maui.Views;
namespace TicTacToeGameProj
{
    public partial class SettingsPage : ContentPage
    {
        private Dictionary<string, string> _config = new();

        public SettingsPage()
        {
            // 1. Загружаем конфиг
            _config = ConfigManager.LoadConfig();

            // 2. Инициализируем язык из конфига
            LanguageManager.Initialize(_config);

            // 3. СИНХРОННО грузим текстовый файл локализации,
            //    чтобы к моменту InitializeComponent словарь уже был заполнен
            LocalizationService.Instance
                .SetLanguageAsync(LanguageManager.CurrentLanguage.Key)
                .GetAwaiter()
                .GetResult();

            // 4. Только теперь создаём визуальное дерево и биндинги
            InitializeComponent();
        }
        private async void ChangeBotDifLevel()
        {
            BotDifficultyValueLabel.Text = _config["botdifficulty"] switch
            {
                "Easy" => LocalizationService.Instance["BotDifficultyEasy"],
                "Normal" => LocalizationService.Instance["BotDifficultyNormal"],
                "Hard" => LocalizationService.Instance["BotDifficultyHard"]
            };
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            _config = ConfigManager.LoadConfig();
            await Task.Run(() =>
            {
                LoadTheme();
            });
            await Task.Run(() =>
            {
                LoadMusic();
            });
            
            // --- язык ---
            LanguageManager.Initialize(_config);
            LanguageValueLabel.Text = LanguageManager.CurrentLanguage.DisplayName;
            await Task.Run(() =>
            {
               LocalizationService.Instance.SetLanguageAsync(LanguageManager.CurrentLanguage.Key);
            });
            bool VsBotOn = _config.TryGetValue("vsbot", out var mm)
                       && string.Equals(mm, "ON", StringComparison.OrdinalIgnoreCase);
            VsBotSwitch.Toggled -= VsBotSwitch_Toggled; // чтобы не ловить лишний вызов
            VsBotSwitch.IsToggled = VsBotOn;
            VsBotSwitch.Toggled += VsBotSwitch_Toggled;
            ChangeBotDifLevel();
          
        }
        private async void LoadTheme()
        {
            // --- тема ---
            ThemeManager.Initialize(_config);
            ThemeManager.ApplyBackgroundImage(ThemeBackgroundImage);
            ThemeName.Text = ThemeManager.CurrentTheme;
            ThemePreviewImage.Source = $"{ThemeManager.CurrentTheme}.png";
        }
        private async void LoadMusic()
        {
            // --- музыка (применяем состояние из конфига) ---
            bool musicOn = _config.TryGetValue("music", out var m)
                           && string.Equals(m, "ON", StringComparison.OrdinalIgnoreCase);
            MusicSwitch.Toggled -= MusicSwitch_Toggled; // чтобы не ловить лишний вызов
            MusicSwitch.IsToggled = musicOn;
            MusicSwitch.Toggled += MusicSwitch_Toggled;
            // синхронизируем фактическое состояние плеера
            if (musicOn && !MusicManager.IsPlaying)
                await MusicManager.PlayAsync();
            else if (!musicOn && MusicManager.IsPlaying)
                MusicManager.Stop();

        }
        private async void OnLanguageTapped(object sender, EventArgs e)
        {
            var popup = new LanguagePopup();
            var result = await this.ShowPopupAsync(popup) as string; 

            if (string.IsNullOrEmpty(result))
                return;

            var lang = LanguageManager.Languages.FirstOrDefault(l => l.Key == result);
            if (lang == null)
                return;

            // 1. Обновляем текущий язык в своём LanguageManager/конфиге
            LanguageManager.SetLanguage(lang);
            LanguageValueLabel.Text = lang.DisplayName;

            _config["language"] = lang.DisplayName;
            ConfigManager.SaveConfig(_config);

            // 2. ГЛАВНОЕ: говорим локализатору, что язык сменился
            await LocalizationService.Instance.SetLanguageAsync(lang.Key);
            ChangeBotDifLevel();
        }
        private async void OnBotDifficultyTapped(object sender, EventArgs e)
        {
            
            var popup = new BotDifficultyPopup(_config["botdifficulty"]);
            var result = await this.ShowPopupAsync(popup) as string;
            if (string.IsNullOrEmpty(result))
                return;
            _config["botdifficulty"] = result;
            ConfigManager.SaveConfig(_config);
            ChangeBotDifLevel();
        }

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

        private async void OnBackButtonTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        private async void MusicSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            var isOn = e.Value;

            // значение для конфига в том же формате, что уже используешь
            _config["music"] = isOn ? "ON" : "OFF";
            ConfigManager.SaveConfig(_config);
            if (isOn)
                await MusicManager.PlayAsync();
            else
                MusicManager.Stop();
        }
        private async void VsBotSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            var isOn = e.Value;

            // значение для конфига в том же формате, что уже используешь
            _config["vsbot"] = isOn ? "ON" : "OFF";
            ConfigManager.SaveConfig(_config);
        }
    }
}
