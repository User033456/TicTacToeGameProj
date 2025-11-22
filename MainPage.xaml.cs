namespace TicTacToeGameProj
{
    public partial class MainPage : ContentPage
    {
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
        /// Загрузка фона
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // перечитываем конфиг, если он мог поменяться
            _config = ConfigManager.LoadConfig();

            // тема
            ThemeManager.Initialize(_config);
            ThemeManager.ApplyBackgroundImage(ThemeBackgroundImage);

            // язык (обновляем локализацию, если вдруг изменился)
            LanguageManager.Initialize(_config);
            await LocalizationService.Instance.SetLanguageAsync(LanguageManager.CurrentLanguage.Key);

            // если у тебя есть логика таймера/счёта в OnAppearing — оставь её ниже
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
            Load();
        }
        /// <summary>
        /// Загрузка элементов поля
        /// </summary>
        /// <param name="n">размерность поля</param>
        protected void LoadUI(int n = 3)
        {
            _manager = new UIManager(this,n);
            _manager.LoadX(out Xelements,CrossesLayer);
            _manager.Load0(out Zeroelements, CirclesLayer);
            _manager.LoadButtons(out buttons, Button_OnClick, ButtonsLayer);
            _manager.LoadLines(out boxViewsHorizontal, out boxViewsVertical, LineLayer);
            _manager.LoadDiagonalLines(out MainD, out SecD, LineLayer);
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
        }
        /// <summary>
        /// Анимация появления линии
        /// </summary>
        /// <param name="myBoxView"></param>
        /// <returns></returns>
        async Task AnimateScaleAppearance(BoxView myBoxView)
        {
            myBoxView.IsVisible = true;
            if(!FirstPlayerFlag)
            {
                myBoxView.BackgroundColor = Colors.SpringGreen;
                myBoxView.Background = Colors.SpringGreen;
                myBoxView.Color = Colors.SpringGreen;
            }
            else
            {
                myBoxView.BackgroundColor = Colors.Red;
                myBoxView.BackgroundColor = Colors.Red;
                myBoxView.Color = Colors.Red;
            }
            // Анимация увеличения масштаба с эффектом "пружины"
            await myBoxView.ScaleTo(1, 1000, Easing.BounceOut);
            // Дополнительно: небольшая вибрация после появления
            await myBoxView.ScaleTo(1.05, 1000, Easing.BounceOut);
            await myBoxView.ScaleTo(1, 1000, Easing.BounceOut);
        }
        /// <summary>
        /// Отработка анимации победы
        /// </summary>
        /// <param name="myBoxView"></param>
        /// <returns></returns>
        async Task AnimateScaleAppearance(Microsoft.Maui.Controls.Shapes.Path myBoxView)
        {
            myBoxView.IsVisible = true;
            if (!FirstPlayerFlag)
            {
                myBoxView.BackgroundColor = Colors.SpringGreen;
                myBoxView.Background = Colors.SpringGreen;
                myBoxView.Stroke = Colors.SpringGreen;
            }
            else
            {
                myBoxView.BackgroundColor = Colors.Red;
                myBoxView.BackgroundColor = Colors.Red;
                myBoxView.Stroke = Colors.Red;
            }
            // Анимация увеличения масштаба с эффектом "пружины"
            await myBoxView.ScaleTo(1, 1000, Easing.BounceOut);
            // Дополнительно: небольшая вибрация после появления
            await myBoxView.ScaleTo(1.05, 1000, Easing.BounceOut);
            await myBoxView.ScaleTo(1, 1000, Easing.BounceOut);
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
                    await AnimateScaleAppearance(MainD);
                    MainD.IsVisible = false;
                    break;
                case "IsSecondDiagonalWin":
                    await AnimateScaleAppearance(SecD);
                    SecD.IsVisible = false;
                    break;
                case "IsVerticalWin":
                    await AnimateScaleAppearance(boxViewsVertical[CheckWin.WinCoordinate]);
                    //await Task.Delay();
                    boxViewsVertical[CheckWin.WinCoordinate].IsVisible = false;
                    break;
                case "IsHorizontallWin":
                    await AnimateScaleAppearance(boxViewsHorizontal[CheckWin.WinCoordinate]);
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
            this.IsEnabled = false;
            var button = sender as Button;
            int row = Grid.GetRow(button);
            // Получаем номер столбца
            int column = Grid.GetColumn(button);
            // Ходил крестик
            if (FirstPlayerFlag == false)
            {
                button.Text = "X";
                Xelements[row][column].IsVisible = true;
                button.FontSize = 0;
                // Блокировка нажатия кнопки, чтобы не допустить возможности повторного хода
                button.IsEnabled = false;
                FirstPlayerFlag = true;
                // Проверка на то, случилась ли победа после данного хода
                if (CheckWin.IsWin(buttons, 'X') == true)
                {
                    // Изменение счёта победителя
                    if (!FirstPlayerFlag)
                    {
                        scoreController.ZeroWindCount++;
                    }
                    else
                    {
                        scoreController.XWindCount++;
                    }
                    scoreController.UpdateScore();
                    await CreateLine();
                    NewGame();
                }
                RedDisableGlow();
                EllipseGlowEnable();
            }
            // Ходил нолик
            else
            {
                button.Text = "0";
                Zeroelements[row][column].IsVisible = true;
                button.FontSize = 0;
          
                // Блокировка нажатия кнопки, чтобы не допустить возможности повторного хода
                button.IsEnabled = false;
                FirstPlayerFlag = false;
                if (CheckWin.IsWin(buttons, '0') == true)
                {
                    // Изменение счёта победителя
                    if (!FirstPlayerFlag)
                    {
                        scoreController.ZeroWindCount++;
                    }
                    else
                    {
                        scoreController.XWindCount++;
                    }
                    scoreController.UpdateScore();
                    await CreateLine();
                    NewGame();
                }
                RedEnableGlow();
                EllipseGlowDisable();
            }
            // Если поле заполнено, но никто не победил
            if(CheckWin.IsDraw(buttons))
            {
                NewGame();
            }
            this.IsEnabled = true;
        }
        /// <summary>
        /// Обработчик начала новой игры
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewGameStartMenuItem_OnClick(object sender, EventArgs e) => NewGame();
        private void ResetScoreButton_Clicked(object sender, EventArgs e) => scoreController.Reset();

        private void ContentPage_Loaded(object sender, EventArgs e) { }

        private async void SettingsButton_Clicked(object sender, EventArgs e) =>  await Navigation.PushAsync(new SettingsPage());
    }
}
