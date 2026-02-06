using System;

namespace TicTacToeGameProj
{
    public enum BotDifficulty
    {
        Easy,
        Normal,
        Hard
    }

    public sealed class MinimaxBot : IBotPlayer
    {
        private readonly BotDifficulty _difficulty;
        private readonly Random _random = new();

        public MinimaxBot(BotDifficulty difficulty)
        {
            _difficulty = difficulty;
        }
        public int ChooseMove(int[] board, int botPlayer)
        {
            // Список возможных ходов и их оценок
            List<(int move, int score)> moves = new();

            for (int i = 0; i < 9; i++)
            {
                if (board[i] != 0) continue;

                board[i] = botPlayer;
                int score = -Negamax(board, -botPlayer, int.MinValue + 1, int.MaxValue - 1);
                board[i] = 0;

                moves.Add((i, score));
            }

            return ChooseByDifficulty(moves);
        }
        private int ChooseByDifficulty(List<(int move, int score)> moves)
        {
            // Сортируем: лучший ход первый
            moves.Sort((a, b) => b.score.CompareTo(a.score));

            return _difficulty switch
            {
                BotDifficulty.Hard => moves[0].move,

                BotDifficulty.Normal => ChooseWithChance(moves, bestChance: 0.8),

                BotDifficulty.Easy => ChooseWithChance(moves, bestChance: 0.4),

                _ => moves[0].move
            };
        }
        private int ChooseWithChance(List<(int move, int score)> moves, double bestChance)
        {
            // С вероятностью bestChance — лучший ход
            if (_random.NextDouble() < bestChance)
                return moves[0].move;

            // Иначе — случайный из остальных
            int max = Math.Min(3, moves.Count); // не берём совсем плохие
            int index = _random.Next(1, max);

            return moves[index].move;
        }


        private static int Negamax(int[] b, int player, int alpha, int beta)
        {
            int w = Winner(b);
            if (w != 0) return w * player; // оценка с позиции текущего игрока
            if (IsFull(b)) return 0;

            int best = int.MinValue;

            for (int i = 0; i < 9; i++)
            {
                if (b[i] != 0) continue;

                b[i] = player;
                int score = -Negamax(b, -player, -beta, -alpha);
                b[i] = 0;

                if (score > best) best = score;
                if (score > alpha) alpha = score;
                if (alpha >= beta) break; // alpha-beta отсечение
            }

            return best;
        }

        private static bool IsFull(int[] b)
        {
            for (int i = 0; i < 9; i++)
                if (b[i] == 0) return false;
            return true;
        }

        // 1 если X выиграл, -1 если O выиграл, 0 иначе
        private static int Winner(int[] b)
        {
            // 8 линий для 3x3
            // (0,1,2) (3,4,5) (6,7,8)
            // (0,3,6) (1,4,7) (2,5,8)
            // (0,4,8) (2,4,6)
            int s;

            s = b[0] + b[1] + b[2]; if (s == 3) return 1; if (s == -3) return -1;
            s = b[3] + b[4] + b[5]; if (s == 3) return 1; if (s == -3) return -1;
            s = b[6] + b[7] + b[8]; if (s == 3) return 1; if (s == -3) return -1;

            s = b[0] + b[3] + b[6]; if (s == 3) return 1; if (s == -3) return -1;
            s = b[1] + b[4] + b[7]; if (s == 3) return 1; if (s == -3) return -1;
            s = b[2] + b[5] + b[8]; if (s == 3) return 1; if (s == -3) return -1;

            s = b[0] + b[4] + b[8]; if (s == 3) return 1; if (s == -3) return -1;
            s = b[2] + b[4] + b[6]; if (s == 3) return 1; if (s == -3) return -1;

            return 0;
        }
    }
}
