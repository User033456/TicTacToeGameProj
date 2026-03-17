using Microsoft.Maui.Controls.Shapes;
using TicTacToeGameProj.Generators;
namespace TicTacToeGameProj
{
    /// <summary>
    /// Класс для управления UI элементами
    /// </summary>
    public class UIManager
    {
        private Page Pagee;
        public List<List<Grid>> Xel; //Таблица крестиков
        public List<List<Grid>> ZeroEl; // Таблица ноликов
        public List<List<Button>> Buttons = new List<List<Button>>(); // Кнопки игрового поля
        private int n = 3;
        private UIElementsGenerator _generator = new UIElementsGenerator();
        public List<BoxView> boxViewsHorizontal = new List<BoxView>();
        public List<BoxView> boxViewsVertical = new List<BoxView>();
        public Microsoft.Maui.Controls.Shapes.Path MainD = new();
        public Microsoft.Maui.Controls.Shapes.Path SecD = new();
        private Line Line1, Line2; // линии крестика в таблице счёта
        // поле, хранящее цвет крестика
        private Color _xColor = Colors.Red;
        private Ellipse ScoreEllipse;
        // свойство, реагирующее на изменение цвета крестика
        public Color XColor
        {
            get => _xColor;
            set
            {
                if (_xColor == value)
                    return;

                _xColor = value;
                UpdateXColors();
            }
        }
        /// <summary>
        /// Изменение цвета всех крестиков
        /// </summary>
        private void UpdateXColors()
        {
            if (Xel == null)
                return;

            foreach (var row in Xel)
            {
                foreach (var crossGrid in row)
                {
                    foreach (var child in crossGrid.Children)
                    {
                        if (child is Line line)
                            line.Stroke = _xColor;
                    }
                }
            }
            if (Line1 != null) Line1.Stroke = _xColor;
            if (Line2 != null) Line2.Stroke = _xColor;
            
        }
        private Color _ZeroColor = Colors.SpringGreen;
        // свойство, реагирующее на изменение цвета крестика
        public Color ZeroColor
        {
            get => _ZeroColor;
            set
            {
                if (_ZeroColor == value)
                    return;

                _ZeroColor = value;
                UpdateZeroColors();
            }
        }
        /// <summary>
        /// Изменение цвета всех ноликов
        /// </summary>
        private void UpdateZeroColors()
        {
            if (ZeroEl == null)
                return;

            foreach (var row in ZeroEl)
            {
                foreach (var crossGrid in row)
                {
                    foreach (var child in crossGrid.Children)
                    {
                        if (child is Ellipse line)
                            line.Stroke = _ZeroColor;
                    }
                }
            }
            if (ScoreEllipse != null) ScoreEllipse.Stroke = _ZeroColor; 
        }
        public UIManager(Page page,Line line1 = null,Line line2 = null,int N = 3,Ellipse SEllipse = null)
        {
            Pagee = page;
            n = N;
            Line1 = line1;
            Line2 = line2;
            ScoreEllipse = SEllipse;
        }
        /// <summary>
        /// Загрузка крестиков
        /// </summary>
        /// <param name="n"></param>
        public async Task LoadX(Grid XGrid)
        {
            Xel = new List<List<Grid>>();
            for (int i = 0; i < n; i++)
            {
                Xel.Add(new List<Grid>());
                XGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                XGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                for (int j = 0; j < n; j++)
                {
                    // Создаем контейнер Grid для крестика
                    var crossGrid = _generator.CreateX(XColor);
                    // Устанавливаем позицию в родительском Grid (если нужно)
                    Grid.SetRow(crossGrid, i); // Вторая строка (индекс 2)
                    Grid.SetColumn(crossGrid, j); // Первая колонка (индекс 0)
                    Xel[i].Add(crossGrid);
                    XGrid.Children.Add(crossGrid);
                }
            }
        }
        /// <summary>
        /// Загрузка ноликов
        /// </summary>
        /// <param name="Zeroelements"></param>
        public async Task Load0(Grid XGrid)
        {
            ZeroEl = new List<List<Grid>>();
            for (int i = 0; i < n; i++)
            {
                ZeroEl.Add(new List<Grid>());
                XGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                XGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                for (int j = 0; j < n; j++)
                {
                    // создание нолика
                    var circleGrid = _generator.CreateZero(ZeroColor);
                    // Устанавливаем позицию в родительском Grid (CirclesLayer)
                    Grid.SetRow(circleGrid, 2); // Для ячейки (2,0)
                    Grid.SetColumn(circleGrid, 0);
                    // Устанавливаем позицию в родительском Grid (если нужно)
                    Grid.SetRow(circleGrid, i); // Вторая строка (индекс 2)
                    Grid.SetColumn(circleGrid, j); // Первая колонка (индекс 0)
                    ZeroEl[i].Add(circleGrid);
                    XGrid.Children.Add(circleGrid);
                }
            }
        }
        /// <summary>
        /// Создание игровых кнопок в формате n на n
        /// </summary>
        public async Task LoadButtons(EventHandler Button_OnClick, Grid ButtonsGrid )
        {
            Buttons = new List<List<Button>>();
            for (int i = 0; i < n; i++)
            {
                // Разделение грида на строки и столбцы
                Buttons.Add(new List<Button>());
                ButtonsGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                ButtonsGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                for (int j = 0; j < n; j++)
                {
                    var button = _generator.CreateGameButton(Button_OnClick);
                    // Добавление кнопки в грид и список
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    ButtonsGrid.Children.Add(button);
                    Buttons[i].Add(button);
                }
            }
        }
       
        /// <summary>
        /// Создание линий для анимаций
        /// </summary>
        public async Task LoadLines(Grid LinesGrid)
        {
            boxViewsHorizontal = new List<BoxView>();
            boxViewsVertical = new List<BoxView>();
            // Заполнение линиями
            for(int i = 0; i< n;i++)
            {
                LinesGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                LinesGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                // Заполнение строк
                var RedView = _generator.CreateLineHorizontal();
                Grid.SetRow(RedView, i);
                Grid.SetColumn(RedView, 0);
                Grid.SetColumnSpan(RedView, n);
                LinesGrid.Children.Add(RedView);
                boxViewsHorizontal.Add(RedView);
                // Заполнение столбцов
                var RedView2 = _generator.CreateLineVertical();
                Grid.SetRow(RedView2, 0);
                Grid.SetColumn(RedView2, i);
                Grid.SetRowSpan(RedView2, n);
                LinesGrid.Children.Add(RedView2);
                boxViewsVertical.Add(RedView2);
            }
        }
        public async Task LoadDiagonalLines(Grid LinesGrid)
        {

            MainD = _generator.CreateMainDiagonalLine(LinesGrid);
            Grid.SetRowSpan(MainD, LinesGrid.RowDefinitions.Count);
            Grid.SetColumnSpan(MainD, LinesGrid.ColumnDefinitions.Count);
            LinesGrid.Children.Add(MainD);
            SecD = _generator.CreateSecondDiagonalLine(LinesGrid);
            Grid.SetRowSpan(SecD, LinesGrid.RowDefinitions.Count);
            Grid.SetColumnSpan(SecD, LinesGrid.ColumnDefinitions.Count);
            LinesGrid.Children.Add(SecD);
        }
        /// <summary>
        /// Создание разметки поля
        /// </summary>
        /// <param name="BgGrid"></param>
        public async Task LoadBorderLines(Grid BgGrid)
        {
            for (int i = 0; i < n; i++)
            {
                BgGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                BgGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                if (i == n-1)
                {
                    break;
                }
                // Создание горизонтальной линии
                var HorizontalView = _generator.CreateHorizontalBorderLIne();
                Grid.SetRow(HorizontalView, i);
                Grid.SetColumn(HorizontalView, 0);
                Grid.SetColumnSpan(HorizontalView, n);
                BgGrid.Children.Add(HorizontalView);
                // Создание вертикальной линии
                var VertView = _generator.CreateVerticallBorderLIne();
                Grid.SetRow(VertView, 0);
                Grid.SetColumn(VertView, i);
                Grid.SetRowSpan(VertView, n);
                BgGrid.Children.Add(VertView);
            }
        }
        /// <summary>
        /// Отработка анимации победы
        /// </summary>
        /// <param name="myBoxView"></param>
        /// <returns></returns>
        public async Task AnimateScaleAppearance(Microsoft.Maui.Controls.Shapes.Path myBoxView, bool FirstPlayerFlag)
        {
            myBoxView.IsVisible = true;
            if (!FirstPlayerFlag)
            {
                myBoxView.BackgroundColor = ZeroColor;
                myBoxView.Background = ZeroColor;
                myBoxView.Stroke = ZeroColor;
            }
            else
            {
                myBoxView.BackgroundColor = XColor;
                myBoxView.BackgroundColor = XColor;
                myBoxView.Stroke = XColor;
            }
            // Анимация увеличения масштаба с эффектом "пружины"
            await myBoxView.ScaleTo(1, 1000, Easing.BounceOut);
            // Дополнительно: небольшая вибрация после появления
            await myBoxView.ScaleTo(1.05, 1000, Easing.BounceOut);
            await myBoxView.ScaleTo(1, 1000, Easing.BounceOut);
        }
        /// <summary>
        /// Анимация появления линии
        /// </summary>
        /// <param name="myBoxView"></param>
        /// <returns></returns>
        public async Task AnimateScaleAppearance(BoxView myBoxView, bool FirstPlayerFlag)
        {
            myBoxView.IsVisible = true;
            if (!FirstPlayerFlag)
            {
                myBoxView.BackgroundColor = ZeroColor;
                myBoxView.Background = ZeroColor;
                myBoxView.Color = ZeroColor;
            }
            else
            {
                myBoxView.BackgroundColor = ZeroColor;
                myBoxView.BackgroundColor = ZeroColor;
                myBoxView.Color = ZeroColor;
            }
            // Анимация увеличения масштаба с эффектом "пружины"
            await myBoxView.ScaleTo(1, 1000, Easing.BounceOut);
            // Дополнительно: небольшая вибрация после появления
            await myBoxView.ScaleTo(1.05, 1000, Easing.BounceOut);
            await myBoxView.ScaleTo(1, 1000, Easing.BounceOut);
        }
        /// <summary>
        /// Сброс значения кнопок
        /// </summary>
        public void ResetButtons()
        {
            // Сброс всех кнопок к начальному виду
            for (int i = 0; i < Buttons.Count; i++)
            {
                for (int j = 0; j < Buttons.Count; j++)
                {
                    Buttons[i][j].Text = " ";
                    Buttons[i][j].IsEnabled = true;
                    Xel[i][j].IsVisible = false;
                    ZeroEl[i][j].IsVisible = false;
                }
            }
        }
    }
}
