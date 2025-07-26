using Microsoft.Maui.Controls.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public void LoadX(out List<List<Grid>> Xelements)
        {
            var MaingGrid = Pagee.FindByName<Grid>("MainGrid");
            var f = MaingGrid.FindByName<Frame>("GameFrame");
            var GameBoardGrid = f.FindByName<Grid>("GameBoard");
            var XGrid = GameBoardGrid.FindByName<Grid>("CrossesLayer");
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
        public void Load0(out List<List<Grid>> Zeroelements)
        {
            Zeroelements = new List<List<Grid>>();
            var MaingGrid = Pagee.FindByName<Grid>("MainGrid");
            var f = MaingGrid.FindByName<Frame>("GameFrame");
            var GameBoardGrid = f.FindByName<Grid>("GameBoard");
            var XGrid = GameBoardGrid.FindByName<Grid>("CirclesLayer");
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
        public void LoadButtons(out List<List<Button>> buttons, EventHandler Button_OnClick )
        {
            buttons = new List<List<Button>>();
            var MaingGrid = Pagee.FindByName<Grid>("MainGrid");
            var f = MaingGrid.FindByName<Frame>("GameFrame");
            var GameBoardGrid = f.FindByName<Grid>("GameBoard");
            var ButtonsGrid = GameBoardGrid.FindByName<Grid>("ButtonsLayer");
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
                    //button.Margin = new Thickness(5);
                    //button.HorizontalOptions = HorizontalAlignment.Stretch;
                    // button.VerticalAlignment = VerticalAlignment.Stretch;
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
    }
}
