using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Shapes;
namespace TicTacToeGameProj.Generators
{
    /// <summary>
    /// Класс для создания элементов
    /// </summary>
    public class UIElementsGenerator

    {
        /// <summary>
        /// Создание крестика
        /// </summary>
        /// <param name="XColor">Цвет крестика</param>
        /// <returns></returns>
        public Grid CreateX(Color XColor)
        {
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
                HeightRequest = 60,
                Padding = 3,
                Margin = 3
            };
            crossGrid.IsVisible = false;
            // Первая диагональная линия (из левого верхнего в правый нижний угол)
            var line1 = new Line

            {
                Stroke = XColor,
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
                Stroke = XColor,
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
            return crossGrid;
        }
        /// <summary>
        /// Создание нолика
        /// </summary>
        /// <param name="ZeroColor">Цвет нолика</param>
        /// <returns></returns>
        public Grid CreateZero(Color ZeroColor)
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
                HeightRequest = 60,
                Padding = 3,
                Margin = 3,
            };
            circleGrid.IsVisible = false;
            // Создаем сам нолик (Ellipse)
            var circle = new Ellipse
            {
                Stroke = ZeroColor,
                StrokeThickness = 4,
                WidthRequest = 60,
                HeightRequest = 60,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            // Добавляем нолик в Grid
            circleGrid.Children.Add(circle);
            return circleGrid;
        }
        /// <summary>
        /// Создание кнопки для игрового поля
        /// </summary>
        /// <param name="Button_OnClick"></param>
        /// <returns></returns>
        public Button CreateGameButton(EventHandler Button_OnClick)
        {
            // Создание кнопки
            var button = new Button();
            button.Text = " ";
            button.Clicked += Button_OnClick;
            button.BackgroundColor = Colors.Transparent;
            button.FontSize = 0;
            button.TextColor = Colors.Transparent;
            //button.SetResourceReference(Button.StyleProperty, "CThemeButton");
            return button;
        }
        /// <summary>
        /// Создание победных вертикальных линий
        /// </summary>
        /// <returns></returns>
        public BoxView CreateLineVertical()
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
        /// Создание победных горизонтальных линий
        /// </summary>
        /// <returns></returns>
        public BoxView CreateLineHorizontal()
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
        /// Создание победной линии на главной диагонали
        /// </summary>
        /// <param name="LinesGrid"></param>
        /// <returns></returns>
        public Microsoft.Maui.Controls.Shapes.Path CreateMainDiagonalLine(Grid LinesGrid)
        {
            var MainD = new Microsoft.Maui.Controls.Shapes.Path
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
            MainD.IsVisible = false;
            return MainD;
        }
        /// <summary>
        /// Создание победной линии на побочной диагонали
        /// </summary>
        /// <param name="LinesGrid"></param>
        /// <returns></returns>
        public Microsoft.Maui.Controls.Shapes.Path CreateSecondDiagonalLine(Grid LinesGrid)
        {
            var SecD = new Microsoft.Maui.Controls.Shapes.Path
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
            return SecD;
        }
        /// <summary>
        /// Создание горизонтальных линий поля
        /// </summary>
        /// <returns></returns>
        public BoxView CreateHorizontalBorderLIne()
        {
            var HorizontalView = new BoxView
            {
                Color = Colors.Black,
                HeightRequest = 3,
                VerticalOptions = LayoutOptions.End
            };
            return HorizontalView;
        }
        /// <summary>
        /// Создание вертикальных линий поля
        /// </summary>
        /// <returns></returns>
        public BoxView CreateVerticallBorderLIne()
        {
            var VertView = new BoxView
            {
                Color = Colors.Black,
                WidthRequest = 3,
                HorizontalOptions = LayoutOptions.End
            };
            return VertView;
        }
    }
}
