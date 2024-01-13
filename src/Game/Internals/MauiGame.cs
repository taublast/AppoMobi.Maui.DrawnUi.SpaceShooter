using AppoMobi.Maui.DrawnUi.Draw;
using AppoMobi.Maui.DrawnUi.Drawn.Animate;

namespace SpaceShooter.Game;

public class MauiGame : SkiaLayout
{
    public static event EventHandler<GameKey> OnKeyDown;
    public static event EventHandler<GameKey> OnKeyUp;

    public static void KeyboardPressed(GameKey key)
    {
        OnKeyDown?.Invoke(null, key);
    }

    public static void KeyboardReleased(GameKey key)
    {
        OnKeyUp?.Invoke(null, key);
    }

    public MauiGame()
    {
        OnKeyDown += OnKeyboardDownEvent;
        OnKeyUp += OnKeyboardUpEvent;
    }

    ~MauiGame()
    {
        OnKeyUp -= OnKeyboardUpEvent;
        OnKeyDown -= OnKeyboardDownEvent;
    }

    private void OnKeyboardDownEvent(object sender, GameKey key)
    {
        GameKeyDown(key);
    }

    private void OnKeyboardUpEvent(object sender, GameKey key)
    {
        GameKeyUp(key);
    }

    public void StartLoop(int delayMs = 0)
    {
        if (_appLoop == null)
        {
            _appLoop = new(this, GameTick);
        }
        _appLoop.Start(delayMs);
    }

    public virtual void GameKeyDown(GameKey key)
    {
    }

    public virtual void GameKeyUp(GameKey key)
    {
    }

    private ActionOnTickAnimator _appLoop;

    protected long LastFrameTimeNanos;

    protected virtual void GameTick(long frameTime)
    {
        // Incoming frameTime is in nanoseconds
        // Calculate delta time in seconds for later use
        float deltaTime = (frameTime - LastFrameTimeNanos) / 1_000_000_000.0f;
        LastFrameTimeNanos = frameTime;

        GameLoop(deltaTime);
    }

    public virtual void StopLoop()
    {
        _appLoop.Stop();
    }

    /// <summary>
    /// Override this for your game. `deltaMs` is time elapsed between the previous frame and this one 
    /// </summary>
    /// <param name="deltaMs"></param>
    public virtual void GameLoop(float deltaMs)
    {

    }
}