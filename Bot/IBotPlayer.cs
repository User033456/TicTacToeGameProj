namespace TicTacToeGameProj
{
    public interface IBotPlayer
    {
        // board: 0 empty, 1 = X, -1 = O
        // botPlayer: -1 for O, 1 for X
        int ChooseMove(int[] board, int botPlayer);
    }
}
