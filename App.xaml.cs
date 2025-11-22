namespace TicTacToeGameProj
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            StartMusicFromConfig();
        }
        private async void StartMusicFromConfig()
        {
            Dictionary<string, string> cfg = ConfigManager.LoadConfig();

            if (cfg.TryGetValue("music", out var m) &&
                string.Equals(m, "ON", StringComparison.OrdinalIgnoreCase))
            {
                await MusicManager.PlayAsync();
            }
        }
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}