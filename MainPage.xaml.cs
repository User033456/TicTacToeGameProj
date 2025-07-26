using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Dispatching;
using System.Diagnostics;
//using Microsoft.UI.Xaml;
//using Microsoft.UI.Xaml.Controls;
//using Microsoft.UI.Xaml.Media;
//using Microsoft.UI.Xaml.Shapes;
using System;
using Microsoft.Maui.Controls.Shapes;
//using Uno.Toolkit.UI;
//using Windows.UI.Popups;
namespace TicTacToeGameProj
{
    public partial class MainPage : ContentPage
    {
        private int size = 3;
        private DateTime _sessionStartTime;
        private UIManager _manager;
        private List<List<Button>> buttons = new List<List<Button>>();
        private List<List<Grid>> Xelements = new List<List<Grid>>();
        private List<List<Grid>> Zeroelements = new List<List<Grid>>();
        private IDispatcherTimer _updateTimer = null;
        private int XWindCount = 0;
        private int ZeroWindCount = 0;
        // false -ходит крестик true - нолик
        private bool FirstPlayerFlag = false;
        private bool StartGameFlag;
        private void Load(int n = 3)
        {
            InitializeComponent();
            _sessionStartTime = DateTime.Now;
            _updateTimer = Application.Current.Dispatcher.CreateTimer();
            _updateTimer.Interval = TimeSpan.FromSeconds(1);
            //_updateTimer.Tick += (s, e) => UpdateSessionTime();
            //_updateTimer.Start();
            Random r = new Random();
            // Старт случайного первого игрока
            if(r.Next(0,1) == 1)
            {
                // Старт нолика
                StartGameFlag = true;
            }
            else
            {
                // Старт крестика
                StartGameFlag = false;
            }
            FirstPlayerFlag = StartGameFlag;
        }
        /// <summary>
        /// Конструктор n на n 
        /// </summary>
        /// <param name="s"></param>
        public MainPage(int s)
        {
            Load(s);
            LoadUI(s);
        }
        /// <summary>
        /// Конструктор по умолчанию (3на3)
        /// </summary>
        public MainPage()
        {
            Load();
            LoadUI();
        }
        private void LoadUI(int n = 3)
        {
            _manager = new UIManager(this,n);
            _manager.LoadX(out Xelements);
            _manager.Load0(out Zeroelements);
            _manager.LoadButtons(out buttons, Button_OnClick);
        }
        /// <summary>
        /// Форматирование времени для секундомера
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        private string FormatTime(TimeSpan timeSpan)
        {
            if (timeSpan.TotalDays >= 1)
            {
                return $"{(int)timeSpan.TotalDays}д {timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
            }
            else
            {
                return $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
            }
        }
        /// <summary>
        /// Обновление секундомера
        /// </summary>
        private void UpdateSessionTime()
        {
            {
                // Получение текстового поля
                var mGrid = this.FindByName<Grid>("MainGrid");
                var lay = mGrid.FindByName<StackLayout>("Layy");
                var lay2 = lay.
                    FindByName<StackLayout>("Statisticlay");
                var lay3 = lay2.FindByName<StackLayout>("TimerLay");
                var sessionTimeText = lay3.FindByName<Label>("LTimer");
                if (sessionTimeText != null)
                {
                    var sessionDuration = DateTime.Now - _sessionStartTime;
                    sessionTimeText.Text = FormatTime(sessionDuration);
                }
            }
        }
        /// <summary>
        /// Получение текстового поля крестика
        /// </summary>
        /// <returns></returns>
        private Label GetXLAbel()
        {
            // Получение текстового поля
            var mGrid = this.FindByName<Grid>("MainGrid");
            var lay = mGrid.FindByName<StackLayout>("Layy");
            var lay2 = lay.
                FindByName<StackLayout>("Statisticlay");
            return lay2.FindByName<Label>("ScoreX");
        }
        /// <summary>
        /// Получение текстового поля нолика
        /// </summary>
        /// <returns></returns>
        private Label Get0Label()
        {
            // Получение текстового поля
            var mGrid = this.FindByName<Grid>("MainGrid");
            var lay = mGrid.FindByName<StackLayout>("Layy");
            var lay2 = lay.
                FindByName<StackLayout>("Statisticlay");
            return lay2.FindByName<Label>("Score0");
        }
        /// <summary>
        /// Обновление счёта
        /// </summary>
        private void UpdateScore()
        {
            var XScore = this.GetXLAbel();
            var ZeroScore = this.Get0Label();
            XScore.Text = XWindCount.ToString();
            ZeroScore.Text = ZeroWindCount.ToString();
        }
        /// <summary>
        /// Начало новой игры
        /// </summary>
        private void NewGame()
        {
            _sessionStartTime = DateTime.Now;
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
            // Ход передаётся тому, кто в прошлый раз не начинал партию
            if(StartGameFlag)
            {
                StartGameFlag = false;
            }
            else
            {
                StartGameFlag = true;
            }
            FirstPlayerFlag = StartGameFlag;
        }
        /// <summary>
        /// Обработчик нажатия любой кнопки для хода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_OnClick(object sender, EventArgs e)
        {
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
                //button.BorderBrush = redBrush;
                // Блокировка нажатия кнопки, чтобы не допустить возможности повторного хода
                button.IsEnabled = false;
                FirstPlayerFlag = true;
                // Проверка на то, случилась ли победа после данного хода
                if (IsWin(buttons, 'X') == true)
                {
                    // Изменение счёта победителя
                    if (!FirstPlayerFlag)
                    {
                        ZeroWindCount++;
                    }
                    else
                    {
                        XWindCount++;
                    }
                   // UpdateScore();
                    NewGame();
                }
            }
            // Ходил нолик
            else
            {
                button.Text = "0";
                Zeroelements[row][column].IsVisible = true;
                button.FontSize = 0;
                //button.BorderBrush = greenBrush;
                // Блокировка нажатия кнопки, чтобы не допустить возможности повторного хода
                button.IsEnabled = false;
                FirstPlayerFlag = false;
                if (IsWin(buttons, '0') == true)
                {
                    // Изменение счёта победителя
                    if (!FirstPlayerFlag)
                    {
                        ZeroWindCount++;
                    }
                    else
                    {
                        XWindCount++;
                    }
                   // UpdateScore();
                    NewGame();
                }
            }
        }
        /// <summary>
        /// Обработчик начала новой игры
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewGameStartMenuItem_OnClick(object sender, EventArgs e)
        {
            NewGame();
        }
        /// <summary>
        /// Проверка на победу
        /// </summary>
        /// <param name="Buttons">Матрица с кнопками</param>
        /// <param name="symbol">Символ, последовательность из которого нужно искать</param>
        /// <returns></returns>
        private bool IsWin(List<List<Button>> Buttons, char symbol)
        {
            if (IsDiagonalWin(Buttons, symbol) == true || IsVerticalWin(Buttons, symbol) == true ||
                IsHorizontallWin(Buttons, symbol) == true || IsSecondDiagonalWin(Buttons, symbol) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Проверка победы по диагонали
        /// </summary>
        /// <param name="Buttons">Матрица с кнопками</param>
        /// <param name="symbol">Символ, последовательность из которого нужно искать</param>
        /// <returns></returns>
        private bool IsDiagonalWin(List<List<Button>> Buttons, char symbol)
        {
            bool result = true;
            for (int i = 0; i < Buttons.Count; i++)
            {
                if (Buttons[i][i].Text.ToString() != symbol.ToString())
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
        /// <summary>
        /// Проверка победы по побочной диагонали
        /// </summary>
        /// <param name="Buttons">Матрица с кнопками</param>
        /// <param name="symbol">Символ, последовательность из которого нужно искать</param>
        /// <returns></returns>
        private bool IsSecondDiagonalWin(List<List<Button>> Buttons, char symbol)
        {
            bool result = true;
            for (int i = Buttons.Count - 1, j = 0; i >= 0; i--, j++)
            {
                if (Buttons[i][j].Text.ToString() != symbol.ToString())
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
        /// <summary>
        /// Проверка победы по вертикали
        /// </summary>
        /// <param name="Buttons">Матрица с кнопками</param>
        /// <param name="symbol">Символ, последовательность из которого нужно искать</param>
        /// <returns></returns>
        private bool IsVerticalWin(List<List<Button>> Buttons, char symbol)
        {
            bool result = false;
            bool stopflag = false;
            for (int i = 0; i < Buttons.Count; i++)
            {
                for (int j = 0; j < Buttons.Count; j++)
                {
                    // Если в столбце хоть один символ не равен искомому, победы явно нет
                    if (Buttons[j][i].Text.ToString() != symbol.ToString())
                    {
                        stopflag = false;
                        break;
                    }
                    stopflag = true;
                }
                // Если после проверки всего столбца значение флага истинно, игрок победил
                if (stopflag == true)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
        /// <summary>
        /// Проверка победы по горизонтале
        /// </summary>
        /// <param name="Buttons">Матрица с кнопками</param>
        /// <param name="symbol">Символ, последовательность из которого нужно искать</param>
        /// <returns></returns>
        private bool IsHorizontallWin(List<List<Button>> Buttons, char symbol)
        {
            bool result = false;
            bool stopflag = false;
            for (int i = 0; i < Buttons.Count; i++)
            {
                for (int j = 0; j < Buttons.Count; j++)
                {
                    // Если в строке хоть один символ не равен искомому, победы явно нет
                    if (Buttons[i][j].Text.ToString() != symbol.ToString())
                    {
                        stopflag = false;
                        break;
                    }
                    stopflag = true;
                }
                // Если после проверки всей строки значение флага истинно, игрок победил
                if (stopflag == true)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}
