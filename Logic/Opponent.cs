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
        return 0;
    }
    
    // TODO Plays optimally, impossible to beat
    private static int PlayImpossibleMove(char[] board)
    {
        return 0;
    }


}
