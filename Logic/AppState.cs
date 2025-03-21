namespace TicTacToe.Logic;

// Contains logic for the state of the app.
public class AppState
{
    // Event notification when state is changed.
    public event Action? OnViewChanged;

    // What component should be visible on the screen?
    public enum View {
        ModeSelect,
        DifficultySelect,
        TurnOrderSelect,
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
}