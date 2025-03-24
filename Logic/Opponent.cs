namespace TicTacToe.Logic;

// Opponent's logic on different difficulties
public partial class Opponent
{
    private readonly GameRules rules;
    
    public Opponent(GameRules rules)
    {
        this.rules = rules;
    }
    
    // The difficulty level of the opponent AI
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        Impossible
    }

    public Difficulty currentDifficulty { get; set; }
    
    // Event handler needs to set difficulty with a function call
    // public void SetDifficulty(Difficulty diff)
    // {
    //     currentDifficulty = diff;
    // }

    // Returns the index the opponent wants to play on
    public int PlayMove(char[] board, char currentPlayer)
    {
        switch (currentDifficulty)
        {
            case Difficulty.Easy:
                return PlayEasyMove(board);
            case Difficulty.Medium:
                return PlayMediumMove(board, currentPlayer);
            case Difficulty.Hard:
                return PlayHardMove(board, currentPlayer);
            case Difficulty.Impossible:
                return PlayImpossibleMove(board, currentPlayer);
            default:
                throw new InvalidOperationException("Invalid difficulty level!");
        }
    }

    // Just selects a random tile to play on
    private int PlayEasyMove(char[] board)
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
    
    // Always blocks wins and claims victory when possible, but otherwise random
    private int PlayMediumMove(char[] board, char currentPlayer)
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
                if (board[rowStart] == currentPlayer)
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
                if (board[rowStart] == currentPlayer)
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
                if (board[rowStart + 1] == currentPlayer)
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
                if (board[i] == currentPlayer)
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
                if (board[i] == currentPlayer)
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
                if (board[i + 3] == currentPlayer)
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
            if (board[0] == currentPlayer)
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
            if (board[0] == currentPlayer)
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
            if (board[4] == currentPlayer)
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
            if (board[2] == currentPlayer)
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
            if (board[2] == currentPlayer)
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
            if (board[4] == currentPlayer)
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

    // Plays a mix of medium and impossible moves
    private int PlayHardMove(char[] board, char currentPlayer)
    {
        if (IsFirstMove(board))
        {
            Choice bestChoice = Minimax(board, currentPlayer, currentPlayer, 0, -1);
            return bestChoice.move;
        }
        
        Random random = new Random();
        int rand = random.Next(1,10);
        
        if (rand < 7)
        {
            return PlayMediumMove(board, currentPlayer);
        }
        else
        {
            return PlayHardMove(board, currentPlayer);
        }
    }

    // Plays optimally, impossible to beat
    private int PlayImpossibleMove(char[] board, char currentPlayer)
    {
        if (IsFirstMove(board))
        {
            return PlayEasyMove(board);
        }

        // Console.WriteLine("Playing impossible move!");
        Choice bestChoice = Minimax(board, currentPlayer, currentPlayer, 0, -1);
        return bestChoice.move;
    }
    
    // Minimax algorithm tries all possible gamestates and picks the optimal move
    // Wins are weighted positive, with earlier wins being weighted higher
    // Loses are weighted negative, with earlier loses being rated lower
    // Ties are weighted at 0
    private Choice Minimax(char[] gameState, char AIPlayer, char currentPlayer, int depth, int lastMove)
    {
        // Base cases is when the simulated game is over
        
        WinResult result = rules.CheckWinner(gameState);
        if (result.hasWinner)
        {
            if (result.winnerChar == AIPlayer)
            {
                // Console.WriteLine($"Found winning board with depth {depth} and weight {10 - depth}.");
                return new Choice(lastMove, 10 - depth, depth);
            }
            else
            {
                // Console.WriteLine($"Found losing board with depth {depth} and weight {-10 + depth}.");
                return new Choice(lastMove, -10 + depth, depth);
            } 
        }
        
        if (rules.CheckBoardFilled(gameState))
        {
            return new Choice(lastMove, 0, depth);
        }
        
        // Recursive case when there are still moves to analyze
        
        int maxWeight = -100;
        int maxPlay = -1;
        int minWeight = 100;
        int minPlay = -1;
        
        List<int> legalMoves = GetLegalMoves(gameState);
        foreach (int move in legalMoves)
        {
            char[] newGameState = (char[])gameState.Clone();
            newGameState[move] = currentPlayer;
            Choice choice = Minimax(newGameState, AIPlayer, OtherPlayer(currentPlayer), depth+1, move);
            if (choice.weight > maxWeight)
            {
                maxWeight = choice.weight;
                maxPlay = move;
            }
            if (choice.weight < minWeight)
            {
                minWeight = choice.weight;
                minPlay = move;
            }
        }
        
        // If this is our turn we should maximize
        if (AIPlayer == currentPlayer)
        {
            return new Choice(maxPlay, maxWeight, depth);
        }
        // If it's the opponent's turn we assume they minimize
        else {
            return new Choice(minPlay, minWeight, depth);
        }
    }
    
    // Returns a list of legal moves for a given game board
    private List<int> GetLegalMoves(char[] board)
    {
        List<int> validMoves = new List<int>();
        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] == '\0')
            {
                validMoves.Add(i);
            }
        }
        return validMoves;
    }
    
    // Returns the character of the other player
    public char OtherPlayer(char currentPlayer)
    {
        if (currentPlayer == 'X')
        {
            return 'O';
        }
        else
        {
            return 'X';
        }
    }
    
    public bool IsFirstMove(char[] board)
    {
        foreach (char c in board)
        {
            if (c != '\0') return false;
        }
        
        return true;
    }
}

// Represents a choice of move
public class Choice(int move, int weight, int depth)
{
    public int move = move;
    public int weight = weight;
    public int depth = depth;
}