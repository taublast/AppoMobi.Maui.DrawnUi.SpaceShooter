using AppoMobi.Maui.DrawnUi.Draw;
using AppoMobi.Maui.DrawnUi.Drawn.Animate;

namespace SpaceShooter.Game;

public class MauiGame : SkiaLayout
{
    public static event EventHandler<MauiKey> KeyDown;
    public static event EventHandler<MauiKey> KeyUp;

    public static void KeyboardPressed(MauiKey key)
    {
        KeyDown?.Invoke(null, key);
    }

    public static void KeyboardReleased(MauiKey key)
    {
        KeyUp?.Invoke(null, key);
    }

    public MauiGame()
    {
        KeyDown += OnKeyboardDownEvent;
        KeyUp += OnKeyboardUpEvent;
    }

    ~MauiGame()
    {
        KeyUp -= OnKeyboardUpEvent;
        KeyDown -= OnKeyboardDownEvent;
    }

    private void OnKeyboardDownEvent(object sender, MauiKey key)
    {
        OnKeyDown(key);
    }

    private void OnKeyboardUpEvent(object sender, MauiKey key)
    {
        OnKeyUp(key);
    }

    public void StartLoop(int delayMs = 0)
    {
        if (_appLoop == null)
        {
            _appLoop = new(this, GameTick);
        }
        _appLoop.Start(delayMs);
    }

    public virtual void OnKeyDown(MauiKey key)
    {
    }

    public virtual void OnKeyUp(MauiKey key)
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