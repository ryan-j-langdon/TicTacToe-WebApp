namespace TicTacToe.Logic;

public class WinResult
{
    public bool hasWinner = false;
    public char winner = '\0';
    public int[]? winningCells = [];
    
    public WinResult(bool hasWinner, char winner = '\0', int[]? winningCells = null)
    {
        this.hasWinner = hasWinner;
        this.winner = winner;
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

    // The current player's turn, X or Y. X plays first
    public char currentPlayer { get; set; } = 'X';

    // Should the player be able to interact with the board?
    public bool interactable = true;
    
    // Is the game over?
    public bool gameOver = false;
    
    // Who is the winner? '\0' if n/a
    public char winner = '\0';
    
    // Was the game a draw?
    public bool tie = false;
    
    // Which cells caused the win?
    public int[]? winningCells = null;

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
        if (cell_index >= board.Length || cell_index < 0)
        {
            Console.WriteLine("HandleClick called with out of bounds value!");
            return;
        }

        if (gameOver || !interactable) return;

        // Can't play on occupied cells
        if (board[cell_index] != '\0') return;

        board[cell_index] = currentPlayer;
        WinResult result = rules.CheckWinner(board);
        if (result.hasWinner)
        {
            gameOver = true;
            winner = result.winner;
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

        // If playing against AI, they take their turn
        if (!gameOver && currentGamemode == Gamemode.AI_Opponent)
        {
            Task.Run(async () => await OpponentTurn());
        }
    }

    // Opponent AI takes their turn
    private async Task OpponentTurn()
    {
        int move = opponent.PlayMove(board);
        interactable = false;
        OnChange();
        await Task.Delay(500);
        board[move] = currentPlayer;
        WinResult result = rules.CheckWinner(board);
        if (result.hasWinner)
        {
            gameOver = true;
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

    // Resets the state of the game
    public void Restart()
    {
        // Console.WriteLine($"Restart clicked!");
        Array.Clear(board);
        currentPlayer = 'X';
        winner = '\0';
        gameOver = false;
        winningCells = null;
        tie = false;
        interactable = true;
        OnChange();
    }
}