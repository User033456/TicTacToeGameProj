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
        private UIManager _manager;
        private ScoreController scoreController;
        private WinCheck CheckWin = new WinCheck();
        private TimerManager timerManager;
        private List<List<Button>> buttons = new List<List<Button>>();
        private List<List<Grid>> Xelements = new List<List<Grid>>();
        private List<List<Grid>> Zeroelements = new List<List<Grid>>();
        private List<BoxView> boxViewsHorizontal= new List<BoxView>();
        private List<BoxView> boxViewsVertical = new List<BoxView>();
        private Microsoft.Maui.Controls.Shapes.Path MainD = new();
        private Microsoft.Maui.Controls.Shapes.Path SecD = new();
        // false -ходит крестик true - нолик
        private bool FirstPlayerFlag = false;
        private bool StartGameFlag;
        private void Load(int n = 3)
        {
            InitializeComponent();
            scoreController = new ScoreController(GetXLAbel(),Get0Label());
            timerManager = new TimerManager(GetTimerLabel());
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
        /// <param name="s"></param>
        public MainPage(int s)
        {
            size = s;
            Load(s);
        }
        /// <summary>
        /// Конструктор по умолчанию (3на3)
        /// </summary>
        public MainPage()
        {
            Load();
        }
        private void LoadUI(int n = 3)
        {
            _manager = new UIManager(this,n);
            _manager.LoadX(out Xelements);
            _manager.Load0(out Zeroelements);
            _manager.LoadButtons(out buttons, Button_OnClick);
            _manager.LoadLines(out boxViewsHorizontal, out boxViewsVertical);
        }
        private Label GetTimerLabel()
        {
            // Получение текстового поля
            var mGrid = this.FindByName<Grid>("MainGrid");
            var StatGrid = mGrid.FindByName<Grid>("StatisticGrid");
            var TFrame = StatGrid.FindByName<Frame>("TimerFrame");
            var TGrid = TFrame.FindByName<Grid>("TimerGrid");
            var TFrame2 = TGrid.FindByName<Frame>("FrameWithTimeLabel");
            var sessionTimeText = TFrame2.FindByName<Label>("TimerLabel");
            return sessionTimeText;
        }
        /// <summary>
        /// Получение текстового поля крестика
        /// </summary>
        /// <returns></returns>
        private Label GetXLAbel()
        {
            // Получение текстового поля
            var mGrid = this.FindByName<Grid>("MainGrid");
            var StatGrid = mGrid.FindByName<Grid>("StatisticGrid");
            var TFrame = StatGrid.FindByName<Frame>("ScoreFrame");
            var TGrid = TFrame.FindByName<Grid>("ScoreGrid");
            var TFrame2 = TGrid.FindByName<Frame>("FrameWithScoreStatistic");
            var Grid2 = TFrame2.FindByName<Grid>("GridWithScoreStatistic");
            return Grid2.FindByName<Label>("XScoreLabel");
        }
        /// <summary>
        /// Получение текстового поля нолика
        /// </summary>
        /// <returns></returns>
        private Label Get0Label()
        {
            // Получение текстового поля
            var mGrid = this.FindByName<Grid>("MainGrid");
            var StatGrid = mGrid.FindByName<Grid>("StatisticGrid");
            var TFrame = StatGrid.FindByName<Frame>("ScoreFrame");
            var TGrid = TFrame.FindByName<Grid>("ScoreGrid");
            var TFrame2 = TGrid.FindByName<Frame>("FrameWithScoreStatistic");
            var Grid2 = TFrame2.FindByName<Grid>("GridWithScoreStatistic");
            return Grid2.FindByName<Label>("OScoreLabel");
        }
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
            await myBoxView.ScaleTo(1, 1500, Easing.CubicIn);
            // Дополнительно: небольшая вибрация после появления
            await myBoxView.ScaleTo(1.05, 100, Easing.Linear);
            await myBoxView.ScaleTo(1, 100, Easing.Linear);
        }
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
            await myBoxView.ScaleTo(1, 1500, Easing.CubicIn);
            // Дополнительно: небольшая вибрация после появления
            await myBoxView.ScaleTo(1.05, 100, Easing.Linear);
            await myBoxView.ScaleTo(1, 100, Easing.Linear);
        }
        private async Task CreateLine()
        {
            _manager.LoadDiagonalLines(out MainD, out SecD);
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

        public void RedEnableGlow()
        {
            // Получение линий
            var mGrid = this.FindByName<Grid>("MainGrid");
            var StatGrid = mGrid.FindByName<Grid>("StatisticGrid");
            var TFrame = StatGrid.FindByName<Frame>("ScoreFrame");
            var TGrid = TFrame.FindByName<Grid>("ScoreGrid");
            var TFrame2 = TGrid.FindByName<Frame>("FrameWithScoreStatistic");
            var Grid2 = TFrame2.FindByName<Grid>("GridWithScoreStatistic");
            var view = Grid2.FindByName<ContentView>("RedLineOneCV");
            var view2 = Grid2.FindByName<ContentView>("RedLineTwoCV");
            var line1 = view.FindByName<Line>("Line1");
            var line2 = view2.FindByName<Line>("Line2");
            line1.Shadow.Opacity = 0.8F;
            line2.Shadow.Opacity = 0.8F;
        }
        public void RedDisableGlow()
        {
            var mGrid = this.FindByName<Grid>("MainGrid");
            var StatGrid = mGrid.FindByName<Grid>("StatisticGrid");
            var TFrame = StatGrid.FindByName<Frame>("ScoreFrame");
            var TGrid = TFrame.FindByName<Grid>("ScoreGrid");
            var TFrame2 = TGrid.FindByName<Frame>("FrameWithScoreStatistic");
            var Grid2 = TFrame2.FindByName<Grid>("GridWithScoreStatistic");
            var view = Grid2.FindByName<ContentView>("RedLineOneCV");
            var view2 = Grid2.FindByName<ContentView>("RedLineTwoCV");
            var line1 = view.FindByName<Line>("Line1");
            var line2 = view2.FindByName<Line>("Line2");
            line1.Shadow.Opacity = 0;
            line2.Shadow.Opacity = 0;
        }
        private void EllipseGlowEnable()
        {
            var mGrid = this.FindByName<Grid>("MainGrid");
            var StatGrid = mGrid.FindByName<Grid>("StatisticGrid");
            var TFrame = StatGrid.FindByName<Frame>("ScoreFrame");
            var TGrid = TFrame.FindByName<Grid>("ScoreGrid");
            var TFrame2 = TGrid.FindByName<Frame>("FrameWithScoreStatistic");
            var Grid2 = TFrame2.FindByName<Grid>("GridWithScoreStatistic");
            var el = Grid2.FindByName<Ellipse>("EllipseGreen");
            el.Shadow.Opacity = 0.8F;
        }
        private void EllipseGlowDisable()
        {
            var mGrid = this.FindByName<Grid>("MainGrid");
            var StatGrid = mGrid.FindByName<Grid>("StatisticGrid");
            var TFrame = StatGrid.FindByName<Frame>("ScoreFrame");
            var TGrid = TFrame.FindByName<Grid>("ScoreGrid");
            var TFrame2 = TGrid.FindByName<Frame>("FrameWithScoreStatistic");
            var Grid2 = TFrame2.FindByName<Grid>("GridWithScoreStatistic");
            var el = Grid2.FindByName<Ellipse>("EllipseGreen");
            el.Shadow.Opacity = 0;
        }
        /// <summary>
        /// Обработчик нажатия любой кнопки для хода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_OnClick(object sender, EventArgs e)
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
        private void ResetScoreButton_Clicked(object sender, EventArgs e)
        {
            scoreController.Reset();
        }

        private void ContentPage_Loaded(object sender, EventArgs e)
        {

        }
    }
}
