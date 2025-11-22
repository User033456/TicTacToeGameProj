namespace TicTacToeGameProj
{
    public  class ScoreController
    {
        private Label Xlabel;
        private Label Zerolabel;
        public int XWindCount = 0;
        public int ZeroWindCount = 0;
        public ScoreController(Label XLabel, Label ZeroLabel) 
        { 
            Xlabel = XLabel;
            Zerolabel = ZeroLabel;
        }
        /// <summary>
        /// Сброс счёта
        /// </summary>
        public void Reset()
        {
            XWindCount = 0;
            ZeroWindCount = 0;
            UpdateScore();
        }
        /// <summary>
        /// Обновление данных на табло для счёта
        /// </summary>
        public void UpdateScore()
        {
            Xlabel.Text = XWindCount.ToString();
            Zerolabel.Text = ZeroWindCount.ToString();
        }
    }
}
