using SkiaSharp;

namespace SpaceShooter.Game;

public interface IWithHitBox
{
    SKRect GetHitBox();
}