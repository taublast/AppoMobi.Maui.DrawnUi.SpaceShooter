using AppoMobi.Maui.DrawnUi.Draw;
using AppoMobi.Maui.DrawnUi.Enums;
using AppoMobi.Specials;
using SkiaSharp;

namespace SpaceShooter.Game;

public partial class SpaceGame
{
    public class EnemySprite : SkiaImage, IWithHitBox, IReusableSprite
    {
        public static float Speed = 50f;

        public SKRect GetHitBox()
        {
            var position = GetPositionOnCanvasInPoints();
            var hitBox = new SKRect(position.X, position.Y,
                (float)(position.X + Width), (float)(position.Y + Height));

            return hitBox;
        }

        public bool IsActive { get; set; }

        public float SpeedRatio { get; set; }

        public static EnemySprite Create()
        {
            var enemySpriteCounter = RndExtensions.CreateRandom(1, 5);

            var newEnemy = new EnemySprite()
            {
                ZIndex = 4,
                UseCache = SkiaCacheType.GPU,
                Source = $"{SpritesPath}/{enemySpriteCounter}.png",
                Tag = "enemy",
                WidthRequest = 50,
                HeightRequest = 44,
                Effect = SkiaImageEffect.Tint,
                EffectBlendMode = SKBlendMode.SrcATop,
                ColorTint = Color.Parse("#22110022"), //tinted for our game
                SpeedRatio = 0.9f + enemySpriteCounter * 2 / 10f
            };

            newEnemy.ResetAnimationState();

            return newEnemy;
        }

        public void ResetAnimationState()
        {
            Opacity = 1;
            Scale = 1;
        }

        public void UpdatePosition(float deltaTime)
        {
            TranslationY += SpeedRatio * Speed * deltaTime; // move the enemy downwards
        }

        public async Task AnimateDisappearing()
        {
            await FadeToAsync(0, 150);
        }

    }
}