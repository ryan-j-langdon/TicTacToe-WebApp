namespace TicTacToe.Logic;

public class WinResult
{
    public bool hasWinner = false;
    public char winnerChar = '\0';
    public int[]? winningCells = [];
    
    public WinResult(bool hasWinner, char winnerChar = '\0', int[]? winningCells = null)
    {
        this.hasWinner = hasWinner;
        this.winnerChar = winnerChar;
        this.winningCells = winningCells;
    }
}

// Contains information about the state of the game
public class GameState
{
    private readonly Opponent opponent;
    private readonly GameRules rules;

    public GameState(Opponent opponent, GameRules rules)
    {
        this.opponent = opponent;
        this.rules = rules;
    }
    
    // Event notification when state is changed
    public event Action? OnBoardChanged;
    
    // Represents the tic tac toe board.
    // Indexes 0-2 are the first row, 3-5 second row, 6-8 third row
    // '\0' is an empty space and 'X' and 'Y' are occupied spaces
    public char[] board = new char[9];
    
    // Which pieces is the human playing in an AI game, X or O?
    public char humanPlayer { get; set; } = '\0';
    
    // The current player's turn, X or O.
    public char currentPlayer { get; set; } = 'X';
    
    // Whether or not the first player should be randomized
    public bool randomizePieces = false;

    // Should the player be able to interact with the board?
    public bool interactable = true;
    
    // Is the game over?
    public bool gameOver = false;
    
    // Who is the winner? '\0' if n/a
    public char winner = '\0';
    
    // Which cells caused the win?
    public int[]? winningCells = null;
    
    // Was the game a draw?
    public bool tie = false;
    

    // Whether the game is against another person or against AI
    public enum Gamemode
    {
        Multiplayer,
        AI_Opponent
    }

    public Gamemode currentGamemode { get; set; }

    public void SetGamemode(Gamemode gm)
    {
        currentGamemode = gm;
    }
    
    // Notifies other components when the state of the game has changed
    private void OnChange()
    {
        OnBoardChanged?.Invoke();
    }

    // Handles when a cell is clicked on
    public void PlayMove(int cell_index)
    {
        // Console.WriteLine($"Cell {cell_index} clicked.");
        if (cell_index >= board.Length || cell_index < 0) return;

        if (gameOver || !interactable) return;

        // Can't play on occupied cells
        if (board[cell_index] != '\0') return;

        board[cell_index] = currentPlayer;
        WinResult result = rules.CheckWinner(board);
        // Console.WriteLine($"hasWinner = {result.hasWinner}");
        if (result.hasWinner)
        {
            gameOver = true;
            winner = result.winnerChar;
            winningCells = result.winningCells;
        }
        else if (rules.CheckBoardFilled(board))
        {
            gameOver = true;
            tie = true;
        }
        else
        {
            SwitchTurns();
        }
        OnChange();
        
        // If playing against AI, they take their turn
        if (!gameOver && currentGamemode == Gamemode.AI_Opponent)
        {
            Task.Run(OpponentTurn);
        }
    }

    // Opponent AI takes their turn
    public async Task OpponentTurn()
    {
        Console.WriteLine("AI taking turn!");
        int move = opponent.PlayMove(board, currentPlayer);
        interactable = false;
        OnChange();
        await Task.Delay(500);
        board[move] = currentPlayer;
        WinResult result = rules.CheckWinner(board);
        if (result.hasWinner)
        {
            gameOver = true;
            winner = result.winnerChar;
            winningCells = result.winningCells;
        }
        else if (rules.CheckBoardFilled(board))
        {
            gameOver = true;
            tie = true;
        }
        else
        {
            interactable = true;
            SwitchTurns();
        }
        OnChange();
    }

    // Switches the active player
    private void SwitchTurns()
    {
        if (currentPlayer == 'X')
        {
            currentPlayer = 'O';
        }
        else
        {
            currentPlayer = 'X';
        }
        // Console.WriteLine($"It is now {currentPlayer}'s turn.");
        OnChange();
    }

    // Restarts the game
    public void Restart()
    {
        // Console.WriteLine($"Restart clicked!");
        Array.Clear(board);
        currentPlayer = 'X';
        interactable = true;
        gameOver = false;
        winner = '\0';
        winningCells = null;
        tie = false;
        
        if (randomizePieces)
        {
            Random random = new Random();
            humanPlayer = random.Next(2) == 0 ? 'X' : 'O';
        }
        
        if (humanPlayer == 'O')
        {
            Task.Run(OpponentTurn);
        }
        
        OnChange();
    }
    
    // Resets the state of the game board
    public void Reset()
    {
        Array.Clear(board);
        humanPlayer = '\0';
        currentPlayer = 'X';
        randomizePieces = false;
        interactable = true;
        gameOver = false;
        winner = '\0';
        winningCells = null;
        tie = false;
        OnChange();
    }
}