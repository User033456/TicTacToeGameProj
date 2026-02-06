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
        private List<List<Button>> buttons = new List<List<Button>>(); // Кнопки игрового поля
        private List<List<Grid>> Xelements = new List<List<Grid>>(); // Таблица крестиков
        private List<List<Grid>> Zeroelements = new List<List<Grid>>(); // Таблица ноликов
        private List<BoxView> boxViewsHorizontal= new List<BoxView>();
        private List<BoxView> boxViewsVertical = new List<BoxView>();
        private Microsoft.Maui.Controls.Shapes.Path MainD = new();
        private Microsoft.Maui.Controls.Shapes.Path SecD = new();
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
            _manager = new UIManager(this,n);
             _manager.LoadX(out Xelements,CrossesLayer);
            _manager.Load0(out Zeroelements, CirclesLayer);
            _manager.LoadButtons(out buttons, Button_OnClick, ButtonsLayer);
            _manager.LoadLines(out boxViewsHorizontal, out boxViewsVertical, LineLayer);
            _manager.LoadDiagonalLines(out MainD, out SecD, LineLayer);
            _manager.LoadBorderLines(BackgroundGrid);

        }
        /// <summary>
        /// Сброс значения кнопок
        /// </summary>
        private void ResetButtons()
        {
            // Сброс всех кнопок к начальному виду
            for (int i = 0; i < buttons.Count; i++)
            {
                for (int j = 0; j < buttons.Count; j++)
                {
                    buttons[i][j].Text = " ";
                    buttons[i][j].IsEnabled = true;
                    Xelements[i][j].IsVisible = false;
                    Zeroelements[i][j].IsVisible = false;
                }
            }
        }
        /// <summary>
        /// Начало новой игры
        /// </summary>
        private void NewGame()
        {
            timerManager._sessionStartTime = DateTime.Now;
            ResetButtons();
            // Ход передаётся тому, кто в прошлый раз не начинал партию
            if(StartGameFlag)
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
            FirstPlayerFlag = StartGameFlag;
            _ = BotTurnIfNeeded();
        }
        /// <summary>
        /// Создание линии при победе
        /// </summary>
        /// <returns></returns>
        private async Task CreateLine()
        {
            _manager.LoadDiagonalLines(out MainD, out SecD, LineLayer);
            switch (CheckWin.WinType)
            {
                case "IsDiagonalWin":
                    await _manager.AnimateScaleAppearance(MainD, FirstPlayerFlag);
                    MainD.IsVisible = false;
                    break;
                case "IsSecondDiagonalWin":
                    await _manager.AnimateScaleAppearance(SecD, FirstPlayerFlag);
                    SecD.IsVisible = false;
                    break;
                case "IsVerticalWin":
                    await _manager.AnimateScaleAppearance(boxViewsVertical[CheckWin.WinCoordinate], FirstPlayerFlag);
                    //await Task.Delay();
                    boxViewsVertical[CheckWin.WinCoordinate].IsVisible = false;
                    break;
                case "IsHorizontallWin":
                    await _manager.AnimateScaleAppearance(boxViewsHorizontal[CheckWin.WinCoordinate], FirstPlayerFlag);
                    boxViewsHorizontal[CheckWin.WinCoordinate].IsVisible = false;
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
                Xelements[row][column].IsVisible = true;
                button.FontSize = 0;
                button.IsEnabled = false;

                FirstPlayerFlag = true; // теперь очередь нолика

                if (CheckWin.IsWin(buttons, 'X') == true)
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
                Zeroelements[row][column].IsVisible = true;
                button.FontSize = 0;
                button.IsEnabled = false;

                FirstPlayerFlag = false;

                if (CheckWin.IsWin(buttons, '0') == true)
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

            if (CheckWin.IsDraw(buttons))
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
        private void NewGameStartMenuItem_OnClick(object sender, EventArgs e) => NewGame();
        private void ResetScoreButton_Clicked(object sender, EventArgs e) => scoreController.Reset();

        private void ContentPage_Loaded(object sender, EventArgs e) { }

        private async void SettingsButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
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
                    var t = buttons[r][c].Text;
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

                buttons[row][col].Text = "0";
                Zeroelements[row][col].IsVisible = true;
                buttons[row][col].FontSize = 0;
                buttons[row][col].IsEnabled = false;

                // после хода 0 очередь X
                FirstPlayerFlag = false;

                if (CheckWin.IsWin(buttons, '0') == true)
                {
                    
                    if (!FirstPlayerFlag) scoreController.ZeroWindCount++;
                    else scoreController.XWindCount++;

                    scoreController.UpdateScore();
                    await CreateLine();
                    NewGame();
                    return;
                }

                if (CheckWin.IsDraw(buttons))
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
