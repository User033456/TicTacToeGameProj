
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Dispatching;
using System.Diagnostics;
using Windows.UI.Popups;
namespace TicTacToeGameProj
{
    public partial class MainPage : ContentPage
    {
        private int size = 3;
        private DateTime _sessionStartTime;
        private List<List<Button>> buttons = new List<List<Button>>();
        private IDispatcherTimer _updateTimer = null;
        // false - крестик true - нолик
        private bool FirstPlayerFlag = false;
        public MainPage(int s)
        {
            InitializeComponent();
            _sessionStartTime = DateTime.Now;
            _updateTimer = Application.Current.Dispatcher.CreateTimer();
            _updateTimer.Interval = TimeSpan.FromSeconds(1);
            _updateTimer.Tick += (s,e) => UpdateSessionTime();
            _updateTimer.Start();
            LoadButtons();

        }
        public MainPage()
        {
            InitializeComponent();
            LoadButtons();
            _sessionStartTime = DateTime.Now;
            _updateTimer = Application.Current.Dispatcher.CreateTimer();
            _updateTimer.Interval = TimeSpan.FromSeconds(1);
            _updateTimer.Tick += (s, e) => UpdateSessionTime();
            _updateTimer.Start();
        }
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
        private void UpdateSessionTime()
        {
            {
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
        private void LoadButtons(int n = 3)
        {
            buttons = new List<List<Button>>();
            var MaingGrid = this.FindByName<Grid>("MainGrid");
            var ButtonsGrid = MaingGrid.FindByName<Grid>("ButtonsGrid");
            for (int i = 0; i < n; i++)
            {
                // Разделение грида на строки и столбцы
                buttons.Add(new List<Button>());
                ButtonsGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                ButtonsGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                for (int j = 0; j < n; j++)
                {
                    // Создание кнопки
                    var button = new Button();
                    button.Margin = new Thickness(5);
                    //button.HorizontalOptions = HorizontalAlignment.Stretch;
                    // button.VerticalAlignment = VerticalAlignment.Stretch;
                    button.Text = " ";
                    button.Clicked += Button_OnClick;
                    //button.SetResourceReference(Button.StyleProperty, "CThemeButton");
                    // Добавление кнопки в грид и список
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    ButtonsGrid.Children.Add(button);
                    buttons[i].Add(button);
                }
            }
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
                }
            }
            // Ход передаётся крестику
            FirstPlayerFlag = false;
        }
        /// <summary>
        /// Обработчик нажатия любой кнопки для хода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_OnClick(object sender, EventArgs e)
        {
            var button = sender as Button;
            // Ходил крестик
            if (FirstPlayerFlag == false)
            {
                button.Text = "X";
                //button.BorderBrush = redBrush;
                // Блокировка нажатия кнопки, чтобы не допустить возможности повторного хода
                button.IsEnabled = false;
                FirstPlayerFlag = true;
                // Проверка на то, случилась ли победа после данного хода
                if (IsWin(buttons, 'X') == true)
                {
                    NewGame();
                }
            }
            // Ходил нолик
            else
            {
                button.Text = "0";
                //button.BorderBrush = greenBrush;
                // Блокировка нажатия кнопки, чтобы не допустить возможности повторного хода
                button.IsEnabled = false;
                FirstPlayerFlag = false;
                if (IsWin(buttons, '0') == true)
                {
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
