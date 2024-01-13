namespace SpaceShooter.Game;

/// <summary>
/// Resusable model, to avoid GC
/// </summary>
public interface IReusableSprite
{
    bool IsActive { get; set; }

    string Uid { get; }

    void ResetAnimationState();

    Task AnimateDisappearing();
}