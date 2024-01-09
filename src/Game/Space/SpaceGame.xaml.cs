using AppoMobi.Maui.DrawnUi;
using AppoMobi.Maui.DrawnUi.Draw;
using AppoMobi.Maui.DrawnUi.Drawn.Animate;
using AppoMobi.Maui.DrawnUi.Drawn.Infrastructure.Interfaces;
using AppoMobi.Maui.DrawnUi.Infrastructure.Models;
using AppoMobi.Maui.Gestures;
using AppoMobi.Specials;
using SkiaSharp;

namespace SpaceShooter.Game;

public partial class SpaceGame : BaseGame
{

    /// <summary>
    /// changes player gestures-movement calculation
    /// </summary>
    bool preciseMovement = false;

    const int maxEnemies = 32;

    const int maxExplosions = 8;

    public const string SpritesPath = "Space/Sprites";

    /// <summary>
    /// for NON-precise movement system
    /// </summary>
    const float playerSpeed = 300;

    const float playerMoveSpeed = 1.25f; //points of movement per point of panning

    const float starsSpeed = 20; //stars parallax

    float pauseEnemySpawn = 3; // limit of enemy spawns

    int enemySpriteCounter; // int to help change enemy images

    float pauseEnemyCreation;

    //pools of reusable objects
    //to avoid lag spikes when creating or disposing or GC-ing items we simply reuse them

    protected Dictionary<string, EnemySprite> EnemiesPool = new(maxEnemies);

    protected Dictionary<string, ExplosionSprite> ExplosionsPool = new(maxExplosions);

    protected Dictionary<string, ExplosionCrashSprite> ExplosionsCrashPool = new(maxExplosions);

    #region RENDERING

    public SpaceGame()
    {
        InitializeComponent();

        BindingContext = this;
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        BindingContext = this; //insist in case parent view might set its own
    }

    protected override void OnLayoutReady()
    {
        base.OnLayoutReady();

        Task.Run(async () =>
        {
            while (Superview == null || !Superview.HasHandler)
            {
                await Task.Delay(30);
            }

            //we have some GPU cache used so we need the canvas to be fully created before we would start
            Initialize();  //game loop will be started inside

        }).ConfigureAwait(false);
    }

    void Initialize()
    {
        if (!Superview.HasHandler || _initialized)
            return;

        RndExtensions.RandomizeTime(); //amstrad cpc forever

        IgnoreChildrenInvalidations = true;

        // in case we implement key press for desktop
        Focus();

        //prebuilt reusable sprites pools

        for (int i = 0; i < maxEnemies; i++)
        {
            AddToPoolEnemySprite();
        }

        for (int i = 0; i < maxExplosions; i++)
        {
            AddToPoolExplosionSprite();
            AddToPoolExplosionCrashSprite();
        }

        PlayerShieldExplosion.GoToEnd();

        _needPrerender = true;

        _initialized = true;

        StartNewGame();
    }

    protected override void Draw(SkiaDrawingContext context, SKRect destination, float scale)
    {
        base.Draw(context, destination, scale);

        if (_needPrerender)
        {
            //prerender or precompile something like shaders etc
            // ...

            _needPrerender = false;
        }
    }

    /// <summary>
    /// Score can change several times per frame
    /// so we dont want bindings to update the score toooften.
    /// Instead we update the display manually once after the frame is finalized.
    /// </summary>
    void UpdateScore()
    {
        LabelScore.Text = ScoreLocalized;
        LabelHiScore.Text = HiScoreLocalized;
    }


    void AddToPoolExplosionCrashSprite()
    {
        var explosionCrash = ExplosionCrashSprite.Create();

        explosionCrash.Finished += (s, a) =>
        {
            RemoveReusable(explosionCrash);
        };

        ExplosionsCrashPool.Add(explosionCrash.Uid, explosionCrash);
    }

    void AddToPoolExplosionSprite()
    {
        var explosion = ExplosionSprite.Create();

        explosion.Finished += (s, a) =>
        {
            RemoveReusable(explosion);
        };

        ExplosionsPool.Add(explosion.Uid, explosion);
    }

    void AddToPoolEnemySprite()
    {
        var enemy = EnemySprite.Create();
        EnemiesPool.Add(enemy.Uid, enemy);
    }



    protected override void OnChildAdded(SkiaControl child)
    {
        if (_initialized)
            return; //do not care

        base.OnChildAdded(child);
    }

    protected override void OnChildRemoved(SkiaControl child)
    {
        if (_initialized)
            return; //do not care

        base.OnChildRemoved(child);
    }

    #endregion

    #region GAME LOOP

    List<SkiaControl> _spritesToBeRemoved = new(256);

    Queue<SkiaControl> _spritesToBeRemovedLater = new(256);

    List<SkiaControl> _spritesToBeAdded = new(256);

    List<SkiaControl> _startAnimations = new(maxExplosions);

    public override void GameLoop(float deltaMs)
    {
        base.GameLoop(deltaMs);

        if (State == GameState.Playing)
        {
            //update stars parallax
            ParallaxLayer.TileOffsetY -= starsSpeed * deltaMs;

            // get the player hit box
            var playerPosition = player.GetPositionOnCanvasInPoints();

            _playerHitBox = new SKRect(playerPosition.X, playerPosition.Y,
                (float)(playerPosition.X + player.Width), (float)(playerPosition.Y + player.Height));

            // Process collision of bullets and enemies etc in parallel
            Parallel.ForEach(Views, x =>
            {
                if (x is BulletSprite bulletSprite && bulletSprite.IsActive)
                {
                    var bullet = bulletSprite.GetHitBox();
                    if (bulletSprite.TranslationY < -Height)
                    {
                        RemoveBullet(bulletSprite);
                    }

                    Parallel.ForEach(Views, y =>
                    {
                        if (y is EnemySprite enemySprite2 && enemySprite2.IsActive)
                        {
                            var enemy = enemySprite2.GetHitBox();
                            if (bullet.IntersectsWith(enemy))
                            {
                                CollideBulletAndEnemy(enemySprite2, bulletSprite);
                            }
                        }
                    });

                    if (bulletSprite.IsActive)
                    {
                        bulletSprite.UpdatePosition(deltaMs);
                    }
                }
                else if (x is EnemySprite enemySprite && enemySprite.IsActive)
                {
                    var enemy = enemySprite.GetHitBox();
                    bool enemyAlive = true;

                    if (enemySprite.TranslationY > this.Height)
                    {
                        enemyAlive = false;
                        CollideEnemyAndEarth(enemySprite);
                    }

                    if (_playerHitBox.IntersectsWith(enemy))
                    {
                        enemyAlive = false;
                        CollidePlayerAndEnemy(enemySprite);
                    }

                    if (enemyAlive)
                    {
                        enemySprite.UpdatePosition(deltaMs);
                    }
                }
            });

            // reduce time we wait between enemy creations
            pauseEnemyCreation -= 1 * deltaMs;

            // our logic for calculating time between enemy spans according to difficulty and current score
            if (pauseEnemyCreation < 0)
            {
                AddEnemy(); // run the make enemies function

                if (Score > 300)
                {
                    pauseEnemyCreation = pauseEnemySpawn - 2.0f;
                }
                else
                if (Score > 200)
                {
                    pauseEnemyCreation = pauseEnemySpawn - 1.5f;
                }
                else
                if (Score > 100)
                {
                    pauseEnemyCreation = pauseEnemySpawn - 1.0f;
                }
                else
                if (Score > 1)
                {
                    pauseEnemyCreation = pauseEnemySpawn - 0.5f;
                }
                else
                {
                    pauseEnemyCreation = pauseEnemySpawn;
                }
            }

            //we have 2 gestures modes as optional: fun and precise
            if (preciseMovement)
            {
                while (MoveCommands.Count > 0)
                {
                    var command = MoveCommands.Pop();

                    if (command.Direction == MoveDirection.Left || command.Direction == MoveDirection.Right)
                    {
                        var movePlayer = playerSpeed * command.Distance;
                        UpdatePlayerPosition(player.TranslationX + movePlayer);
                    }
                }
            }
            else
            {
                // player movement begins
                if (moveLeft)
                {
                    // if move left is true AND player is inside the boundary then move player to the left
                    UpdatePlayerPosition(player.TranslationX - playerSpeed * deltaMs);
                }

                if (moveRight)
                {
                    // if move right is true AND player left + 90 is less than the width of the form
                    // then move the player to the right
                    UpdatePlayerPosition(player.TranslationX + playerSpeed * deltaMs);
                }
            }


            // if the damage integer is greater than 99
            if (Health < 1)
            {
                EndGameLost();
            }

        }

        // removing sprites
        ProcessSpritesToBeRemoved();

        if (_spritesToBeAdded.Count > 0)
        {
            foreach (var add in _spritesToBeAdded)
            {
                AddSubView(add);
            }
            _spritesToBeAdded.Clear();
        }

        if (_startAnimations.Count > 0)
        {
            foreach (var animation in _startAnimations)
            {
                // remove them permanently from the canvas
                if (animation is SkiaLottie lottie)
                {
                    AddSubView(lottie);
                    lottie.Start();
                }
            }
            _startAnimations.Clear();
        }

        UpdateScore();

    }


    #endregion

    #region METHODS

    void StartNewGame()
    {
        _spritesToBeRemoved.Clear();

        foreach (var control in Views)
        {
            if (control is EnemySprite || control is BulletSprite)
            {
                _spritesToBeRemoved.Add(control);
            }
        }

        ProcessSpritesToBeRemoved();

        pauseEnemyCreation = pauseEnemySpawn;
        Score = 0;
        Health = 100;
        GameOver.IsVisible = false;
        State = GameState.Playing;

        UpdateScore();

        StartLoop();

    }

    void EndGameLost()
    {
        EndGameInternal();
        GameOver.IsVisible = true;
    }

    void EndGameWin()
    {
        EndGameInternal();
    }

    void EndGameInternal()
    {
        State = GameState.Ended;

        StopLoop();
    }

    void AddDamage(int damage)
    {
        Health -= damage / 5f;
    }

    void CollidePlayerAndEnemy(EnemySprite enemySprite)
    {
        AddDamage(5);
        Score += 1;

        RemoveReusable(enemySprite);

        PlayerShieldExplosion.Start();
    }

    void CollideBulletAndEnemy(EnemySprite enemySprite, BulletSprite bulletSprite)
    {
        Score += 10;

        RemoveBullet(bulletSprite);
        RemoveReusable(enemySprite);

        AddExplosion(enemySprite.TranslationX + enemySprite.Width / 2f, enemySprite.TranslationY + enemySprite.Height / 2f);
    }

    private void CollideEnemyAndEarth(EnemySprite enemySprite)
    {
        AddDamage(10);

        RemoveReusable(enemySprite);

        AddExplosionCrash(enemySprite.TranslationX + enemySprite.Width / 2f, enemySprite.Height / 2f);
    }

    void UpdatePlayerPosition(double x)
    {
        var leftLimit = -Width / 2f + player.Width / 2f;
        var rightLimit = Width / 2f - player.Width / 2f;
        var clampedX = Math.Clamp(x, leftLimit, rightLimit);

        if (clampedX != player.TranslationX)
        {
            player.TranslationX = clampedX;
            PlayerShield.TranslationX = clampedX;
            PlayerShieldExplosion.TranslationX = clampedX;
            HealthBar.TranslationX = clampedX;
        }
    }

    private void AddEnemy()
    {
        if (EnemiesPool.Count > 0)
        {
            var enemyIndex = RndExtensions.CreateRandom(0, EnemiesPool.Count - 1);
            var enemy = EnemiesPool.Values.ElementAt(enemyIndex);
            if (enemy != null)
            {
                if (EnemiesPool.Remove(enemy.Uid))
                {
                    enemy.IsActive = true;
                    enemy.TranslationX = RndExtensions.CreateRandom(0, (int)(Width - enemy.WidthRequest));
                    enemy.TranslationY = -50;

                    enemy.ResetAnimationState();

                    _spritesToBeAdded.Add(enemy); ;
                }
            }
        }
    }

    private void AddBullet()
    {
        var newBullet = BulletSprite.Create();

        // place the bullet on top of the player location
        newBullet.TranslationX = player.TranslationX;
        newBullet.TranslationY = player.TranslationY - newBullet.HeightRequest - player.Height;
        newBullet.IsActive = true;

        _spritesToBeAdded.Add(newBullet);
    }

    private void AddExplosion(double x, double y)
    {

        var explosion = ExplosionsPool.Values.FirstOrDefault();
        if (explosion != null)
        {
            if (ExplosionsPool.Remove(explosion.Uid))
            {
                explosion.IsActive = true;
                explosion.TranslationX = x - explosion.WidthRequest / 2f;
                explosion.TranslationY = y - explosion.WidthRequest / 2f;

                explosion.ResetAnimationState();

                _startAnimations.Add(explosion); ;
            }
        }
    }

    private void AddExplosionCrash(double x, double y)
    {

        var explosion = ExplosionsCrashPool.Values.FirstOrDefault();
        if (explosion != null)
        {
            if (ExplosionsCrashPool.Remove(explosion.Uid))
            {
                explosion.IsActive = true;
                explosion.TranslationX = x - explosion.WidthRequest / 2f;
                explosion.TranslationY = y;

                explosion.ResetAnimationState();

                _startAnimations.Add(explosion);
            }
        }
    }

    private void RemoveReusable(IReusableSprite sprite)
    {
        sprite.IsActive = false;
        sprite.AnimateDisappearing().ContinueWith(async (s) =>
        {
            _spritesToBeRemovedLater.Enqueue(sprite as SkiaControl);
        }).ConfigureAwait(false);
    }

    private void RemoveBullet(BulletSprite bulletSprite)
    {
        bulletSprite.IsActive = false;
        _spritesToBeRemoved.Add(bulletSprite);
    }

    void RemoveSprite(SkiaControl sprite)
    {
        if (sprite is SkiaLottie lottie)
        {
            lottie.Stop(); //just in case to avoid empty animators running
        }
        // remove from the canvas
        if (sprite is EnemySprite enemy)
        {
            EnemiesPool.TryAdd(enemy.Uid, enemy);
        }
        else
        if (sprite is ExplosionSprite explosion)
        {
            ExplosionsPool.TryAdd(explosion.Uid, explosion);
        }
        else
        if (sprite is ExplosionCrashSprite explosionCrash)
        {
            ExplosionsCrashPool.TryAdd(explosionCrash.Uid, explosionCrash);
        }
        RemoveSubView(sprite);
    }

    void ProcessSpritesToBeRemoved()
    {
        SkiaControl sprite;
        while (_spritesToBeRemovedLater.Count > 0)
        {
            if (_spritesToBeRemovedLater.TryDequeue(out sprite))
            {
                _spritesToBeRemoved.Add(sprite);
            }
        }

        if (_spritesToBeRemoved.Count > 0)
        {
            foreach (var remove in _spritesToBeRemoved)
            {
                RemoveSprite(remove);
            }
            _spritesToBeRemoved.Clear();
        }
    }

    #endregion

    #region GESTURES

    // move left and move right boolean decleration
    volatile bool moveLeft, moveRight;

    bool _wasPanning;

    public override ISkiaGestureListener ProcessGestures(TouchActionType type, TouchActionEventArgs args, TouchActionResult touchAction,
        SKPoint childOffset, SKPoint childOffsetDirect, ISkiaGestureListener alreadyConsumed)
    {

        if (touchAction == TouchActionResult.Tapped)
        {
            if (State == GameState.Ended)
            {
                StartNewGame();
            }
            return this;
        }

        if (State == GameState.Playing)
        {
            var velocityX = (float)(args.Distance.Velocity.X / RenderingScale);
            //var velocityY = (float)(args.Distance.Velocity.Y / RenderingScale);

            if (touchAction == TouchActionResult.Down)
            {
                _wasPanning = false;
                _lastDownX = args.Location.X / RenderingScale;
            }

            if (touchAction == TouchActionResult.Up)
            {
                moveLeft = false;
                moveRight = false;
                if (!_wasPanning)
                {
                    //custom tapped
                    AddBullet();
                }
            }

            if (touchAction == TouchActionResult.Panning)
            {
                _wasPanning = true;
                if (velocityX < 0)
                {
                    moveLeft = true;
                    moveRight = false;
                }
                else
                if (velocityX > 0)
                {
                    moveRight = true;
                    moveLeft = false;
                }

                if (preciseMovement)
                {

                    var lastPanX = args.Location.X / RenderingScale;
                    var distance = lastPanX - _lastDownX;
                    _lastDownX = lastPanX;

                    if (distance < 0)
                    {
                        MoveCommands.Push(new MoveCommand()
                        {
                            Distance = distance,
                            Direction = MoveDirection.Left
                        });

                    }
                    else
                    if (distance > 0)
                    {
                        MoveCommands.Push(new MoveCommand()
                        {
                            Distance = distance,
                            Direction = MoveDirection.Right
                        });

                    }
                }

            }
        }

        return base.ProcessGestures(type, args, touchAction, childOffset, childOffsetDirect, alreadyConsumed);
    }

    public enum MoveDirection
    {
        None,
        Left,
        Right
    }

    public struct MoveCommand
    {
        public MoveDirection Direction { get; set; }
        public float Distance { get; set; }
    }

    LimitedQueue<MoveCommand> MoveCommands { get; } = new(32);


    #endregion

    #region HUD

    private GameState _gameState;
    public GameState State
    {
        get
        {
            return _gameState;
        }
        set
        {
            if (_gameState != value)
            {
                _gameState = value;
                OnPropertyChanged();
            }
        }
    }

    private double _health = 100;
    public double Health
    {
        get
        {
            return _health;
        }
        set
        {
            if (_health != value)
            {
                _health = value;
                OnPropertyChanged();
            }
        }
    }

    public string ScoreLocalized
    {
        get
        {
            return $"SCORE: {Score:0}";
        }
    }

    public string HiScoreLocalized
    {
        get
        {
            return $"HI: {HiScore:0}";
        }
    }

    private int _Score;
    public int Score
    {
        get
        {
            return _Score;
        }
        set
        {
            if (_Score != value)
            {
                _Score = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ScoreLocalized));

                if (Score > HiScore)
                {
                    HiScore = Score;
                }
            }
        }
    }

    private int _HiScore = 500;
    public int HiScore
    {
        get
        {
            return _HiScore;
        }
        set
        {
            if (_HiScore != value)
            {
                _HiScore = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HiScoreLocalized));
            }
        }
    }

    #endregion


    private SKRect _playerHitBox = new();
    private bool _needPrerender;
    private bool _initialized;
    private float _lastDownX;

    public enum GameState
    {
        Unknown,
        Playing,
        Paused,
        Ended
    }

    public interface IWithHitBox
    {
        SKRect GetHitBox();
    }

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
}