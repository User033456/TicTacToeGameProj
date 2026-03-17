namespace TicTacToeGameProj
{
    public partial class MainPage : ContentPage
    {
        private bool vsBot = true;
        private int BotDifficult;
        private bool _botThinking = false;
        private  TicTacToeGameProj.IBotPlayer _bot;
        private bool LastVsBot; // прошлый режим игры с ботом до открытия страницы настроек
        private int LastBotDifficult;
        private int size = 3;
        private Dictionary<string, string> _config = new();
        private UIManager _manager;
        private ScoreController scoreController;
        private WinCheck CheckWin = new WinCheck(); 
        private TimerManager timerManager;
        // false -ходит крестик true - нолик
        private bool FirstPlayerFlag = false;
        private bool StartGameFlag;
        /// <summary>
        /// Создание бота
        /// </summary>
        private void CreateBot()
        {
            // Получение сложности бота и создание нового бота
            switch (_config["botdifficulty"].ToLower())
            {
                case "easy":
                    BotDifficult = (((int)BotDifficulty.Easy));
                    _bot = new MinimaxBot(BotDifficulty.Easy);
                    break;
                case "normal":
                    BotDifficult = (((int)BotDifficulty.Normal));
                    _bot = new  MinimaxBot(BotDifficulty.Normal);
                    break;
                case "hard":
                    BotDifficult = (((int)BotDifficulty.Hard));
                    _bot = new MinimaxBot(BotDifficulty.Hard);
                    break;
            }
        }
        /// <summary>
        /// Загрузка фона
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // пересчитывание конфига
            _config = ConfigManager.LoadConfig();

            // тема
            ThemeManager.Initialize(_config);
            ThemeManager.ApplyBackgroundImage(ThemeBackgroundImage);

            // обновление локализации
            LanguageManager.Initialize(_config);
            await LocalizationService.Instance.SetLanguageAsync(LanguageManager.CurrentLanguage.Key);
            bool vsBotNow = _config.TryGetValue("vsbot", out var v)
                    && string.Equals(v, "ON", StringComparison.OrdinalIgnoreCase);
            BotDifficult = _config["botdifficulty"].ToLower() switch
            {
                "easy" => (((int)BotDifficulty.Easy)),
                "normal" => (((int)BotDifficulty.Normal)),
                "hard" => (((int)BotDifficulty.Hard))
            };
            // Если бота включили/ выключили - новая игра
            if (vsBotNow != LastVsBot)
            {
                LastVsBot = vsBotNow;
                vsBot = vsBotNow;
                NewGame(); // сразу новая партия
            }
            // Изменение сложности бота - новая игра
            if ((LastBotDifficult != BotDifficult) && vsBot)
            {
                LastBotDifficult = BotDifficult;
                NewGame();
                CreateBot();
            }
            ChangeActualColors();
        }
        /// <summary>
        /// Меняет цвета крестика и нолика на актуальные
        /// </summary>
        private void ChangeActualColors()
        {
            _config = ConfigManager.LoadConfig();
            _manager.XColor = Color.FromArgb(_config["xcolor"]);
            _manager.ZeroColor = Color.FromArgb(_config["zerocolor"]);
        }
        protected void Load(int n = 3)
        {
            InitializeComponent();
            scoreController = new ScoreController(XScoreLabel,OScoreLabel);
            timerManager = new TimerManager(TimerLabel);
            timerManager.Initial();
            Random r = new Random();
            // Старт случайного первого игрока
            if(r.Next(1,3) == 1)
            {
                // Старт нолика
                StartGameFlag = true;
                EllipseGlowEnable();
            }
            else
            {
                // Старт крестика
                StartGameFlag = false;
                RedEnableGlow();
            }
            FirstPlayerFlag = StartGameFlag;
            LoadUI(n);
            vsBot = (_config["vsbot"] == "ON") ? true : false;
           CreateBot();
           // _bot = new MinimaxBot(BotDifficulty.Hard);
            _ = BotTurnIfNeeded();
            LastVsBot = vsBot;
            LastBotDifficult = BotDifficult;

        }
        /// <summary>
        /// Конструктор n на n 
        /// </summary>
        /// <param name="s">размерность поля</param>
        public MainPage(int s)
        {
            _config = ConfigManager.LoadConfig();

            // 2. Язык
            LanguageManager.Initialize(_config);

            // 3. Локализация (текстовые файлы lang_xx.txt)
            LocalizationService.Instance
                .SetLanguageAsync(LanguageManager.CurrentLanguage.Key)
                .GetAwaiter()
                .GetResult();
            size = s;

            Load(s);
        }
        /// <summary>
        /// Конструктор по умолчанию (3на3)
        /// </summary>
        public MainPage()
        {
            _config = ConfigManager.LoadConfig();

            // 2. Язык
            LanguageManager.Initialize(_config);

            // 3. Локализация (текстовые файлы lang_xx.txt)
            LocalizationService.Instance
                .SetLanguageAsync(LanguageManager.CurrentLanguage.Key)
                .GetAwaiter()
                .GetResult();
            Load(3);
        }
        /// <summary>
        /// Загрузка элементов поля
        /// </summary>
        /// <param name="n">размерность поля</param>
        protected async void LoadUI(int n = 3)
        {
            _manager = new UIManager(this,Line1,Line2,n, EllipseGreen);
            ChangeActualColors();
             _manager.LoadX(CrossesLayer);
             _manager.Load0(CirclesLayer);
             _manager.LoadButtons(Button_OnClick, ButtonsLayer);
             _manager.LoadLines(LineLayer);
             _manager.LoadDiagonalLines(LineLayer);
             _manager.LoadBorderLines(BackgroundGrid);
        }
        
        /// <summary>
        /// Начало новой игры
        /// </summary>
        private void NewGame()
        {
            timerManager._sessionStartTime = DateTime.Now;
            _manager.ResetButtons();
            StartGameFlag = FirstPlayerFlag;
            // Ход передаётся тому, кто в прошлый раз не начинал партию
            if(!StartGameFlag)
            {
                StartGameFlag = false;
                RedEnableGlow();
                EllipseGlowDisable();
            }
            else
            {
                StartGameFlag = true;
                RedDisableGlow();
                EllipseGlowEnable();
            }
            _botThinking = false;
            FirstPlayerFlag = StartGameFlag;
            _ = BotTurnIfNeeded();
        }
        /// <summary>
        /// Создание линии при победе
        /// </summary>
        /// <returns></returns>
        private async Task CreateLine()
        {
            _manager.LoadDiagonalLines(LineLayer);
            switch (CheckWin.WinType)
            {
                case "IsDiagonalWin":
                    await _manager.AnimateScaleAppearance(_manager.MainD, FirstPlayerFlag);
                    _manager.MainD.IsVisible = false;
                    break;
                case "IsSecondDiagonalWin":
                    await _manager.AnimateScaleAppearance(_manager.SecD, FirstPlayerFlag);
                    _manager.SecD.IsVisible = false;
                    break;
                case "IsVerticalWin":
                    await _manager.AnimateScaleAppearance(_manager.boxViewsVertical[CheckWin.WinCoordinate], FirstPlayerFlag);
                    _manager.boxViewsVertical[CheckWin.WinCoordinate].IsVisible = false;
                    break;
                case "IsHorizontallWin":
                    await _manager.AnimateScaleAppearance(_manager.boxViewsHorizontal[CheckWin.WinCoordinate], FirstPlayerFlag);
                    _manager.boxViewsHorizontal[CheckWin.WinCoordinate].IsVisible = false;
                    break;
            }
        }
        /// <summary>
        /// Подсветка при ходе крестика
        /// </summary>
        public void RedEnableGlow()
        {
            Line1.Shadow.Opacity = 0.8F;
            Line2.Shadow.Opacity = 0.8F;
        }
        /// <summary>
        /// Выключение подсветки хода крестика
        /// </summary>
        public void RedDisableGlow()
        {
            Line1.Shadow.Opacity = 0;
            Line2.Shadow.Opacity = 0;
        }
        private void EllipseGlowEnable()
        {
            EllipseGreen.Shadow.Opacity = 0.8F;
        }
        private void EllipseGlowDisable()
        {
            EllipseGreen.Shadow.Opacity = 0;
        }
        /// <summary>
        /// Обработчик нажатия любой кнопки для хода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_OnClick(object sender, EventArgs e)
        {
            // Если бот включён и сейчас очередь нолика — человеку кликать нельзя
            if (vsBot && FirstPlayerFlag == true) return;

            this.IsEnabled = false;

            var button = sender as Button;
            int row = Grid.GetRow(button);
            int column = Grid.GetColumn(button);

            // Ходил крестик
            if (FirstPlayerFlag == false)
            {
                button.Text = "X";
                _manager.Xel[row][column].IsVisible = true;
                button.FontSize = 0;
                button.IsEnabled = false;

                FirstPlayerFlag = true; // теперь очередь нолика

                if (CheckWin.IsWin(_manager.Buttons, 'X') == true)
                {
                    if (!FirstPlayerFlag) scoreController.ZeroWindCount++;
                    else scoreController.XWindCount++;

                    scoreController.UpdateScore();
                    await CreateLine();
                    NewGame();
                    this.IsEnabled = true;
                    return;
                }

                RedDisableGlow();
                EllipseGlowEnable();
            }
            else
            {
                button.Text = "0";
                _manager.ZeroEl[row][column].IsVisible = true;
                button.FontSize = 0;
                button.IsEnabled = false;

                FirstPlayerFlag = false;

                if (CheckWin.IsWin(_manager.Buttons, '0') == true)
                {
                    if (!FirstPlayerFlag) scoreController.ZeroWindCount++;
                    else scoreController.XWindCount++;

                    scoreController.UpdateScore();
                    await CreateLine();
                    NewGame();
                    this.IsEnabled = true;
                    return;
                }

                RedEnableGlow();
                EllipseGlowDisable();
            }

            if (CheckWin.IsDraw(_manager.Buttons))
            {
                NewGame();
                this.IsEnabled = true;
                return;
            }

            this.IsEnabled = true;
            await BotTurnIfNeeded();
        }

        /// <summary>
        /// Обработчик начала новой игры
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewGameStartMenuItem_OnClick(object sender, EventArgs e)
        {
            if (FirstPlayerFlag)
            {
                FirstPlayerFlag = false;
            }
            else
            {
                FirstPlayerFlag = true;
            }
            _botThinking = false;
                NewGame();
        }
        private void ResetScoreButton_Clicked(object sender, EventArgs e) => scoreController.Reset();

        private void ContentPage_Loaded(object sender, EventArgs e) { }

        private async void SettingsButton_Clicked(object sender, EventArgs e)
        {
            this.IsEnabled = false;
            await Navigation.PushAsync(new SettingsPage());
            this.IsEnabled = true;
        }
        /// <summary>
        /// Получение состояния игрового поля (для бота)
        /// </summary>
        /// <returns></returns>
        private int[] GetBoard3x3()
        {
            var board = new int[9]; // 0 empty, 1 = X, -1 = 0
            for (int r = 0; r < 3; r++)
                for (int c = 0; c < 3; c++)
                {
                    var t = _manager.Buttons[r][c].Text;
                    int idx = r * 3 + c;
                    board[idx] = t == "X" ? 1 : (t == "0" ? -1 : 0);
                }
            return board;
        }
        /// <summary>
        /// Ход бота (бот играет за 0) — вызывается, когда очередь нолика.
        /// </summary>
        private async Task BotTurnIfNeeded()
        {
            // Бот выключен или уже думает
            if (!vsBot || _botThinking) return;
            // Бот ходит только когда очередь 0
            if (FirstPlayerFlag != true) return;
            _botThinking = true;
            this.IsEnabled = false;

            try
            {
                // Подсветка хода для нолика
                RedDisableGlow();
                EllipseGlowEnable();

                int[] board = GetBoard3x3();

                // считаем ход в фоне, чтобы UI не лагал
                int move = await Task.Run(() => _bot.ChooseMove(board, botPlayer: -1)); // -1 = 0

                if (move < 0) return;

                int row = move / 3;
                int col = move % 3;

                _manager.Buttons[row][col].Text = "0";
                _manager.ZeroEl[row][col].IsVisible = true;
                _manager.Buttons[row][col].FontSize = 0;
                _manager.Buttons[row][col].IsEnabled = false;

                // после хода 0 очередь X
                FirstPlayerFlag = false;

                if (CheckWin.IsWin(_manager.Buttons, '0') == true)
                {
                    
                    if (!FirstPlayerFlag) scoreController.ZeroWindCount++;
                    else scoreController.XWindCount++;

                    scoreController.UpdateScore();
                    await CreateLine();
                    NewGame();
                    return;
                }

                if (CheckWin.IsDraw(_manager.Buttons))
                {
                    NewGame();
                    return;
                }

                // подсветка хода крестика
                RedEnableGlow();
                EllipseGlowDisable();
            }
            finally
            {
                this.IsEnabled = true;
                _botThinking = false;
            }
        }
        
    }
}
