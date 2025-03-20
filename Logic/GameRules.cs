namespace TicTacToe.Logic;

public class GameRules
{
    // Returns winner information if there's a winner.
    public WinResult CheckWinner(char[] board)
    {
        // Check rows
        for (int i = 0; i < 3; i++)
        {
            int start = 3 * i;
            if (board[start] != '\0' && board[start] == board[start + 1] && board[start + 1] == board[start + 2])
            {
                return new WinResult(true, board[start], [start, start + 1, start + 2]);
            }
        }

        // Check columns
        for (int i = 0; i < 3; i++)
        {
            if (board[i] != '\0' && board[i] == board[i + 3] && board[i + 3] == board[i + 6])
            {
                return new WinResult(true, board[i], [i, i + 3, i + 6]);
            }
        }

        // Check diagonals
        if (board[0] != '\0' && board[0] == board[4] && board[4] == board[8])
        {
            return new WinResult(true, board[0], [0, 4, 8]);
        }
        if (board[2] != '\0' && board[2] == board[4] && board[4] == board[6])
        {
            return new WinResult(true, board[2], [2, 4, 6]);
        }

        return new WinResult(false);
    }

    // Returns true if the board is filled
    public bool CheckBoardFilled(char[] board)
    {
        foreach (char c in board)
        {
            if (c == '\0') return false;
        }
        return true;
    }
}