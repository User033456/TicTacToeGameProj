namespace TicTacToeGameProj
{
    internal class TimerManager
    {
        private IDispatcherTimer _updateTimer = null;
        private Label TimerLabel;
        public DateTime _sessionStartTime;
        public TimerManager(Label tl) 
        {
            TimerLabel = tl;
        }
        public void Initial()
        {
            _sessionStartTime = DateTime.Now;
            _updateTimer = Application.Current.Dispatcher.CreateTimer();
            _updateTimer.Interval = TimeSpan.FromSeconds(1);
            _updateTimer.Tick += (s, e) => UpdateSessionTime();
            _updateTimer.Start();
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
                var sessionDuration = DateTime.Now - _sessionStartTime;
                TimerLabel.Text = FormatTime(sessionDuration);
            }
        }
    }
}
