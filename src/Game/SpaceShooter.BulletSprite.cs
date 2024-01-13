// NOTE: Parts of the code below are based on
// https://www.mooict.com/wpf-c-tutorial-create-a-space-battle-shooter-game-in-visual-studio/7/

using AppoMobi.Maui.DrawnUi.Draw;
using AppoMobi.Maui.DrawnUi.Enums;
using SkiaSharp;

namespace SpaceShooter.Game;

public partial class SpaceShooter
{
    public class BulletSprite : SkiaShape, IWithHitBox
    {
        public static float Speed = 500f;

        public bool IsActive { get; set; }

        public SKRect GetHitBox()
        {

            var position = GetPositionOnCanvasInPoints();
            var hitBox = new SKRect(position.X, position.Y,
                (float)(position.X + Width), (float)(position.Y + Height));

            return hitBox;
        }

        public float SpeedRatio { get; set; }

        public static BulletSprite Create()
        {
            var newBullet = new BulletSprite()
            {
                Tag = "bullet",
                HeightRequest = 16,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End,
                CornerRadius = 6,
                WidthRequest = 5,
                StrokeWidth = 1,
                StrokeCap = SKStrokeCap.Round,
                BackgroundColor = Color.Parse("#f0ff3333"),
                StrokeColor = Color.Parse("#eeff0000"),
                UseCache = SkiaCacheType.Operations,
                SpeedRatio = 1
            };
            return newBullet;
        }

        public void UpdatePosition(float deltaTime)
        {
            TranslationY -= SpeedRatio * Speed * deltaTime;
        }
    }
}