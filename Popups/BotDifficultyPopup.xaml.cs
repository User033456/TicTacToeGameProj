using CommunityToolkit.Maui.Views;

namespace TicTacToeGameProj
{
    public partial class BotDifficultyPopup : Popup
    {
        private readonly string _currentDifficulty;

        /// <param name="currentDifficulty">
        /// Текущая сложность, например: "Easy", "Normal", "Hard"
        /// </param>
        public BotDifficultyPopup(string currentDifficulty)
        {
            InitializeComponent();
            _currentDifficulty = currentDifficulty;
            BuildDifficultyButtons();
        }

        private void BuildDifficultyButtons()
        {
            DifficultyStack.Children.Clear();

            AddButton("Easy", LocalizationService.Instance["BotDifficultyEasy"]);
            AddButton("Normal", LocalizationService.Instance["BotDifficultyNormal"]);
            AddButton("Hard", LocalizationService.Instance["BotDifficultyHard"]);

        }

        private void AddButton(string value, string text)
        {
            var btn = new Button
            {
                Text = text,
                FontSize = 14,
                CornerRadius = 18,
                BackgroundColor = string.Equals(value, _currentDifficulty, StringComparison.OrdinalIgnoreCase)
                    ? Color.FromArgb("#B3EBED")   // выбранная
                    : Color.FromArgb("#D9D9D9"), // обычная
                BorderColor = Color.FromArgb("#020136"),
                BorderWidth = 1,
                TextColor = Color.FromArgb("#020136"),
                Padding = new Thickness(16, 4)
            };
            btn.Clicked += (_, _) => Close(value);

            DifficultyStack.Children.Add(btn);
        }

        private void OnCloseClicked(object sender, EventArgs e)
        {
            Close(null);
        }
    }
}
