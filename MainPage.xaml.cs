
namespace TicTacToeGameProj
{
    public partial class MainPage : ContentPage
    {
        private int size = 3;
        private List<List<Button>> buttons = new List<List<Button>>();
        private bool isDarkTheme = true;
        // false - крестик true - нолик
        private bool FirstPlayerFlag = false;
        public MainPage(int s)
        {
            InitializeComponent();
            LoadButtons();
        }
        public MainPage()
        {
            InitializeComponent();
            LoadButtons();
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
                //if (IsWin(buttons, 'X') == true)
                {
                    //NewGame();
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
                //if (IsWin(buttons, '0') == true)
                {
                    //NewGame();
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
    }
}
