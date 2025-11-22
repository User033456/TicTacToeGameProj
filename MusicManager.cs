using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using Plugin.Maui.Audio;

namespace TicTacToeGameProj
{
    public static class MusicManager
    {
        private static IAudioPlayer? _player;
        private static bool _isInitialized;
        private static readonly object _lock = new();

        private const string MusicFilePath = "Resources/Raw/GameMusic.mp3"; 

        private static async Task EnsureInitializedAsync()
        {
            if (_isInitialized && _player != null)
                return;

            lock (_lock)
            {
                if (_isInitialized && _player != null)
                    return;

                _isInitialized = true;
            }

            var stream = await FileSystem.OpenAppPackageFileAsync(MusicFilePath);
            _player = AudioManager.Current.CreatePlayer(stream);

            // бесконечный цикл
            _player.Loop = true;
            _player.Volume = 0.5; // по желанию
        }

        public static async Task PlayAsync()
        {
            await EnsureInitializedAsync();

            if (_player != null && !_player.IsPlaying)
                _player.Play();
        }

        public static void Stop()
        {
            if (_player != null && _player.IsPlaying)
                _player.Stop();
        }

        public static bool IsPlaying => _player?.IsPlaying ?? false;
    }
}
