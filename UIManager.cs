using Microsoft.Maui.Controls.Shapes;
namespace TicTacToeGameProj
{
    public class UIManager
    {
        private Page Pagee;

        private int n = 3;
        public UIManager(Page page,int N = 3)
        {
            Pagee = page;
            N = n;
        }
        /// <summary>
        /// Загрузка крестиков
        /// </summary>
        /// <param name="n"></param>
        public void LoadX(out List<List<Grid>> Xelements,Grid XGrid)
        {
            Xelements = new List<List<Grid>>();
            for (int i = 0; i < n; i++)
            {
                Xelements.Add(new List<Grid>());
                XGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                XGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                for (int j = 0; j < n; j++)
                {
                    // Создаем контейнер Grid для крестика
                    var crossGrid = new Grid
                    {
                        RowDefinitions = new RowDefinitionCollection
                    {
                        new RowDefinition { Height = GridLength.Star }
                    },
                        ColumnDefinitions = new ColumnDefinitionCollection
                    {
                        new ColumnDefinition { Width = GridLength.Star }
                    },
                        Opacity = 1,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        WidthRequest = 60,
                        HeightRequest = 60
                    };
                    crossGrid.IsVisible = false;
                    // Первая диагональная линия (из левого верхнего в правый нижний угол)
                    var line1 = new Line

                    {
                        Stroke = Colors.Red,
                        StrokeThickness = 4,
                        X1 = 0,
                        Y1 = 0,
                        X2 = 60,
                        Y2 = 60,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center
                    };

                    // Вторая диагональная линия (из правого верхнего в левый нижний угол)
                    var line2 = new Line
                    {
                        Stroke = Colors.Red,
                        StrokeThickness = 4,
                        X1 = 60,
                        Y1 = 0,
                        X2 = 0,
                        Y2 = 60,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center
                    };

                    // Добавляем линии в Grid
                    crossGrid.Children.Add(line1);
                    crossGrid.Children.Add(line2);

                    // Устанавливаем позицию в родительском Grid (если нужно)
                    Grid.SetRow(crossGrid, i); // Вторая строка (индекс 2)
                    Grid.SetColumn(crossGrid, j); // Первая колонка (индекс 0)
                    Xelements[i].Add(crossGrid);
                    XGrid.Children.Add(crossGrid);
                }
            }
        }
        /// <summary>
        /// Загрузка ноликов
        /// </summary>
        /// <param name="Zeroelements"></param>
        public void Load0(out List<List<Grid>> Zeroelements, Grid XGrid)
        {
            Zeroelements = new List<List<Grid>>();
            for (int i = 0; i < n; i++)
            {
                Zeroelements.Add(new List<Grid>());
                XGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                XGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                for (int j = 0; j < n; j++)
                {
                    // Создаем контейнер Grid для нолика
                    var circleGrid = new Grid
                    {
                        RowDefinitions = new RowDefinitionCollection
                        {
                            new RowDefinition { Height = GridLength.Star }
                        },
                        ColumnDefinitions = new ColumnDefinitionCollection
                        {
                            new ColumnDefinition { Width = GridLength.Star }
                        },
                        Opacity = 1,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        WidthRequest = 60,
                        HeightRequest = 60
                    };
                    circleGrid.IsVisible= false;
                    // Создаем сам нолик (Ellipse)
                    var circle = new Ellipse
                    {
                        Stroke = Colors.SpringGreen,
                        StrokeThickness = 4,
                        WidthRequest = 60,
                        HeightRequest = 60,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center
                    };

                    // Добавляем нолик в Grid
                    circleGrid.Children.Add(circle);

                    // Устанавливаем позицию в родительском Grid (CirclesLayer)
                    Grid.SetRow(circleGrid, 2); // Для ячейки (2,0)
                    Grid.SetColumn(circleGrid, 0);
                    // Устанавливаем позицию в родительском Grid (если нужно)
                    Grid.SetRow(circleGrid, i); // Вторая строка (индекс 2)
                    Grid.SetColumn(circleGrid, j); // Первая колонка (индекс 0)
                    Zeroelements[i].Add(circleGrid);
                    XGrid.Children.Add(circleGrid);
                }
            }
        }
        /// <summary>
        /// Создание игровых кнопок в формате n на n
        /// </summary>
        /// <param name="n">Размерность</param>
        public void LoadButtons(out List<List<Button>> buttons, EventHandler Button_OnClick, Grid ButtonsGrid )
        {
            buttons = new List<List<Button>>();
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
                    button.Text = " ";
                    button.Clicked += Button_OnClick;
                    button.BackgroundColor = Colors.Transparent;
                    button.FontSize = 0;
                    button.TextColor = Colors.Transparent;
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
        /// Создание столбцов
        /// </summary>
        /// <returns></returns>
        private BoxView CreateLineVertical()
        {
            return new BoxView
            {
                Color = Colors.Red,
                WidthRequest = 10,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.Center,
                IsVisible = false
            };
        }
        /// <summary>
        /// Создание столбцов
        /// </summary>
        /// <returns></returns>
        private BoxView CreateLineHorizontal()
        {
            return new BoxView
            {
                Color = Colors.Red,
                HeightRequest = 10,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                IsVisible = false
            };
        }
        /// <summary>
        /// Создание линий для анимаций
        /// </summary>
        public void LoadLines(out List<BoxView> boxViewsHorizontal, out List<BoxView> boxViewsVertical, Grid LinesGrid)
        {
            boxViewsHorizontal = new List<BoxView>();
            boxViewsVertical = new List<BoxView>();
            // Заполнение линиями
            for(int i = 0; i< n;i++)
            {
                LinesGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                LinesGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                // Заполнение строк
                var RedView = CreateLineHorizontal();
                Grid.SetRow(RedView, i);
                Grid.SetColumn(RedView, 0);
                Grid.SetColumnSpan(RedView, n);
                LinesGrid.Children.Add(RedView);
                boxViewsHorizontal.Add(RedView);
                // Заполнение столбцов
                var RedView2 = CreateLineVertical();
                Grid.SetRow(RedView2, 0);
                Grid.SetColumn(RedView2, i);
                Grid.SetRowSpan(RedView2, n);
                LinesGrid.Children.Add(RedView2);
                boxViewsVertical.Add(RedView2);
            }
        }
        public void LoadDiagonalLines(out Microsoft.Maui.Controls.Shapes.Path MainD,
            out Microsoft.Maui.Controls.Shapes.Path SecD, Grid LinesGrid)
        {
           
            MainD = new Microsoft.Maui.Controls.Shapes.Path
            {
                // Настройка внешнего вида
                Stroke = Colors.Red,
                StrokeThickness = 10,
                Data = new PathGeometry
                {
                    Figures = new PathFigureCollection
                    {
                        new PathFigure
                        {
                            StartPoint = new Point(0, 0), 
                            Segments = new PathSegmentCollection
                            {
                                new LineSegment
                                {
                                    Point = new Point(LinesGrid.Width,LinesGrid.Height) 
                                }
                            }
                        }
                    }
                }
            };
            Grid.SetRowSpan(MainD, LinesGrid.RowDefinitions.Count);
            Grid.SetColumnSpan(MainD, LinesGrid.ColumnDefinitions.Count);
            LinesGrid.Children.Add(MainD);
            SecD = new Microsoft.Maui.Controls.Shapes.Path
            {
                Data = new PathGeometry
                {
                    Figures = new PathFigureCollection
                    {
                        new PathFigure
                        {
                            StartPoint = new Point(0, LinesGrid.Height),
                            Segments = new PathSegmentCollection
                            {
                                new LineSegment
                                {
                                    Point = new Point(LinesGrid.Width, 0)
                                }
                            }
                        }
                    }
                },
                Stroke = Colors.Red,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                Aspect = Stretch.Uniform, 
                StrokeThickness = 10
            };
            SecD.IsVisible = false;
            MainD.IsVisible = false;
            Grid.SetRowSpan(SecD, LinesGrid.RowDefinitions.Count);
            Grid.SetColumnSpan(SecD, LinesGrid.ColumnDefinitions.Count);
            LinesGrid.Children.Add(SecD);
        }
    }
}
