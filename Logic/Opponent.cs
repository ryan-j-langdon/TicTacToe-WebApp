namespace TicTacToe.Logic;

// Opponent's logic on different difficulties
public static partial class Opponent
{
    // The difficulty level of the opponent AI
    public enum Difficulty
    {
        Easy,
        Medium,
        Impossible
    }

    public static Difficulty currentDifficulty { get; set; }
    
    // Event handler needs to set difficulty with a function call
    public static void SetDifficulty(Difficulty diff)
    {
        currentDifficulty = diff;
    }

    // Returns the index the opponent wants to play on
    public static int PlayMove(char[] board)
    {
        switch (currentDifficulty)
        {
            case Difficulty.Easy:
                return PlayEasyMove(board);
            case Difficulty.Medium:
                return PlayMediumMove(board);
            case Difficulty.Impossible:
                return PlayImpossibleMove(board);
            default:
                throw new InvalidOperationException("Invalid difficulty level!");
        }
    }
    
    // Just selects a random tile to play on
    private static int PlayEasyMove(char[] board)
    {
        List<int> possibleMoves = new List<int>();
        Random random = new Random();
        for (int i = 0; i<board.Length; i++)
        {
            if (board[i] == '\0')
            {
                possibleMoves.Add(i);
            }
        }
        return possibleMoves[random.Next(possibleMoves.Count)];
    }
    
    // TODO Always blocks wins and claims victory when possible, but otherwise random
    private static int PlayMediumMove(char[] board)
    {
        int? winningMove = null;
        int? blockingMove = null;

        // Find potential winning or blocking moves
        
        // Check rows
        for (int i = 0; i < 3; i++)
        {
            int rowStart = 3 * i;
            // Rightmost cell open
            if (board[rowStart] != '\0' && board[rowStart] == board[rowStart + 1] && board[rowStart + 2] == '\0')
            {
                if (board[rowStart] == 'O')
                {
                    winningMove = rowStart + 2;
                }
                else
                {
                    blockingMove = rowStart + 2;
                }
            }
            // Middle cell open
            if (board[rowStart] != '\0' && board[rowStart] == board[rowStart + 2] && board[rowStart + 1] == '\0')
            {
                if (board[rowStart] == 'O')
                {
                    winningMove = rowStart + 1;
                }
                else
                {
                    blockingMove = rowStart + 1;
                }
            }
            // Leftmost cell open
            if (board[rowStart + 1] != '\0' && board[rowStart + 1] == board[rowStart + 2] && board[rowStart] == '\0')
            {
                if (board[rowStart + 1] == 'O')
                {
                    winningMove = rowStart;
                }
                else
                {
                    blockingMove = rowStart;
                }
            }
        }

        // Check columns
        for (int i = 0; i < 3; i++)
        {
            // Bottom cell open
            if (board[i] != '\0' && board[i] == board[i + 3] && board[i + 6] == '\0')
            {
                if (board[i] == 'O')
                {
                    winningMove = i + 6;
                }
                else
                {
                    blockingMove = i + 6;
                }
            }
            // Middle cell open
            if (board[i] != '\0' && board[i] == board[i + 6] && board[i + 3] == '\0')
            {
                if (board[i] == 'O')
                {
                    winningMove = i + 3;
                }
                else
                {
                    blockingMove = i + 3;
                }
            }
            // Top cell open
            if (board[i + 3] != '\0' && board[i + 3] == board[i + 6] && board[i] == '\0')
            {
                if (board[i + 3] == 'O')
                {
                    winningMove = i;
                }
                else
                {
                    blockingMove = i;
                }
            }
        }

        // Check diagonals
        
        // Bottom right is open
        if (board[0] != '\0' && board[0] == board[4] && board[8] == '\0')
        {
            if (board[0] == 'O')
            {
                winningMove = 8;
            }
            else
            {
                blockingMove = 8;
            }
        }
        // Center is open
        if (board[0] != '\0' && board[0] == board[8] && board[4] == '\0')
        {
            if (board[0] == 'O')
            {
                winningMove = 4;
            }
            else
            {
                blockingMove = 4;
            }
        }
        // Top left is open
        if (board[4] != '\0' && board[4] == board[8] && board[0] == '\0')
        {
            if (board[4] == 'O')
            {
                winningMove = 0;
            }
            else
            {
                blockingMove = 0;
            }
        }
        
        // Bottom left is open
        if (board[2] != '\0' && board[2] == board[4] && board[6] == '\0')
        {
            if (board[2] == 'O')
            {
                winningMove = 6;
            }
            else
            {
                blockingMove = 6;
            }
        }
        // Center is open
        if (board[2] != '\0' && board[2] == board[6] && board[4] == '\0')
        {
            if (board[2] == 'O')
            {
                winningMove = 4;
            }
            else
            {
                blockingMove = 4;
            }
        }
        // Top right is open
        if (board[4] != '\0' && board[4] == board[6] && board[2] == '\0')
        {
            if (board[4] == 'O')
            {
                winningMove = 2;
            }
            else
            {
                blockingMove = 2;
            }
        }
        
        // Play a winning move if there is one
        if (winningMove != null)
        {
            return (int)winningMove;
        }
        
        // Else play a blocking move if there is one
        else if (blockingMove != null)
        {
            return (int)blockingMove;
        }
        
        // Else play randomly
        else 
        {
            return PlayEasyMove(board);
        }
        
    }
    
    // TODO Plays optimally, impossible to beat
    private static int PlayImpossibleMove(char[] board)
    {
        return 0;
    }


}
