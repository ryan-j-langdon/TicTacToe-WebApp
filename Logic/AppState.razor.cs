namespace TicTacToe.Logic;

// Contains logic for the state of the app.
public class AppState
{
    public event Action? OnViewChanged;
    
    // What component should be visible on the screen?
    public enum View {
        ModeSelect,
        DifficultySelect,
        GameBoard
    }

    private View _currentView;
    
    public View currentView
    {
        get { return _currentView; }
            
        set
        {
            _currentView = value;
            OnViewChanged?.Invoke();
        }
    }

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
}