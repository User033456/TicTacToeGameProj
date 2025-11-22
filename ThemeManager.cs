namespace TicTacToeGameProj
{
    /// <summary>
    /// Класс для управления задним фоном
    /// </summary>
    public static class ThemeManager
    {
        // !! Тут перечисли реальные имена файлов тем без .png
        // например, если есть Resources/Images/BASIC.png и DARK.png:
        private static readonly List<string> _allThemes = new()
        {
            "BASIC",
            "bricks",
            "games",
            "space",
            "paper"
        };

        public static IReadOnlyList<string> ThemeList => _allThemes;
        public static string CurrentTheme { get; private set; } = "BASIC";

        public static void Initialize(Dictionary<string, string> config)
        {
            if (_allThemes.Count == 0)
                throw new InvalidOperationException("Theme list is empty");

            if (config.TryGetValue("theme", out var theme) &&
                _allThemes.Contains(theme))
                CurrentTheme = theme;
            else
                CurrentTheme = _allThemes[0];
        }

        public static string SwitchTheme(int direction)
        {
            if (_allThemes.Count == 0)
                return CurrentTheme;

            int index = _allThemes.IndexOf(CurrentTheme);
            if (index < 0) index = 0;

            int newIndex = (index + direction + _allThemes.Count) % _allThemes.Count;
            CurrentTheme = _allThemes[newIndex];

            return CurrentTheme;
        }

        /// <summary>
        /// Ставит нужную картинку в Image. 
        /// Имя файла = CurrentTheme + ".png", лежит в Resources/Images.
        /// </summary>
        public static void ApplyBackgroundImage(Image image)
        {
            if (CurrentTheme == "BASIC")
            {
                image.Source = ImageSource.FromFile(null);
                return;
            }
            string fileName = $"{CurrentTheme}.png"; // напр. BASIC.png
            image.Source = ImageSource.FromFile(fileName);
        }
    }
}
