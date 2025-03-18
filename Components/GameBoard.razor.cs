namespace TicTacToe.Components;
using TicTacToe.Logic;

public partial class GameBoard
{
    // Represents the tic tac toe board.
    // Indexes 0-2 are the first row, 3-5 second row, 6-8 third row
    // '\0' is an empty space and 'X' and 'Y' are occupied spaces
    char[] board = new char[9];
    // The current player's turn, X or Y. X plays first
    public char currentPlayer { get; private set; } = 'X';
    // Should the player be able to interact with the cell?
    bool interactable = true;
    // Is the game over?
    bool gameOver = false;
    // Who is the winner? '\0' if n/a
    char winner = '\0';
    // Was the game a draw?
    bool tie = false;
    // Which cells caused the win?
    int[]? winningCells = null;
    
    // Handles when a cell is clicked on
    void HandleClick(int cell_index)
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
        
        if (CheckWinner())
        {
            gameOver = true;
        }
        else if (CheckTie())
        {
            gameOver = true;
            tie = true;
        }
        else
        {
            SwitchTurns();
        }

        // If playing against AI, they take their turn
        if (!gameOver && app.currentGamemode == AppState.Gamemode.AI_Opponent) 
        {
            Task.Run(async () => await OpponentTurn());
        }
    }
    
    // Opponent AI takes their turn
    async Task OpponentTurn()
    {
        int move = Opponent.PlayMove(board);
        interactable = false;
        StateHasChanged();
        await Task.Delay(500);
        board[move] = currentPlayer;
        if (CheckWinner())
        {
            gameOver = true;
        }
        else if (CheckTie())
        {
            gameOver = true;
            tie = true;
        }
        else
        {
            interactable = true;
            SwitchTurns();
        }
        StateHasChanged();
    }

    // Switches the active player
    void SwitchTurns()
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
        StateHasChanged();
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
        StateHasChanged();
    }

    // Returns true if there's a winner, else false.
    // Sets `winner` and `winningCells` to indicate the winner.
    bool CheckWinner()
    {
        // Check rows
        for (int i = 0; i < 3; i++)
        {
            int start = 3*i;
            if (board[start] != '\0' && board[start] == board[start+1] && board[start+1] == board[start+2])
            {
                winner = board[start];
                winningCells = [start, start+1, start+2];
                return true;
            }
        }

        // Check columns
        for (int i = 0; i < 3; i++)
        {
            if (board[i] != '\0' && board[i] == board[i+3] && board[i+3] == board[i+6])
            {
                winner = board[i];
                winningCells = [i, i+3, i+6];
                return true;
            }
        }

        // Check diagonals
        if (board[0] != '\0' && board[0] == board[4] && board[4] == board[8])
        {
            winner = board[0];
            winningCells = [0, 4, 8];
            return true;
        }
        if (board[2] != '\0' && board[2] == board[4] && board[4] == board[6])
        {
            winner = board[2];
            winningCells = [2, 4, 6];
            return true;
        }

        return false;
    }
    
    // Returns true if tie
    bool CheckTie()
    {
        if (winner != '\0')
        {
            return false;
        }
        
        foreach (char c in board)
        {
            if (c == '\0') return false;
        }
        return true;
    }
}
