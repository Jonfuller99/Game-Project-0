using CollisionExample.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;


namespace Game_Project_0;


public enum GameState
{
    Start,
    Playing,
    Win,
    Lose
}

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private GameState state = GameState.Start;

    private JetSprite jet;

    private Texture2D background;
    private Texture2D startScreen;

    private GemSprite [] gems;

    private DamageOrbSprite [] orbs;

    private int gemsLeft;

    private SpriteFont bangers;

    public SoundEffect gemPickup;
    public SoundEffect hit;
    public SoundEffect victory;
    public SoundEffect lose;

    private Song backgroundMusic;

    private System.TimeSpan completionTime;


    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        jet = new(this);
        SetupGame();
        base.Initialize();
    }

    private void SetupGame()
    {

        orbs = new DamageOrbSprite[]
        {
            new DamageOrbSprite(new Vector2(100,100)){ Direction = Direction.Down},
            new DamageOrbSprite(new Vector2(450,400)){ Direction = Direction.Up},
            new DamageOrbSprite(new Vector2(200,500)){ Direction = Direction.Left},
            new DamageOrbSprite(new Vector2(700,100)){ Direction = Direction.Down},
            new DamageOrbSprite(new Vector2(300,50)){ Direction = Direction.Right},
        };

        System.Random rand = new System.Random();
        gems = new GemSprite[]
        {
          new GemSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),  
          new GemSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),  
          new GemSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),  
          new GemSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),  
          new GemSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),  
        };
        gemsLeft = gems.Length;
    }

    private void ResetGame()
    {
        SetupGame();
        jet.Reset(Content);
        foreach(var orb in orbs) orb.LoadContent(Content);
        foreach(var gem in gems)
        {
            gem.LoadContent(Content);
        }

        state = GameState.Playing;
        MediaPlayer.Play(backgroundMusic);
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        // TODO: use this.Content to load your game content here


        jet.LoadContent(Content);
        foreach(var orb in orbs) orb.LoadContent(Content);
        foreach(var gem in gems)
        {
            gem.LoadContent(Content);
        }
        bangers = Content.Load<SpriteFont>("bangers");
        background = Content.Load<Texture2D>("SunsetBackground");
        startScreen = Content.Load<Texture2D>("SkyDashStartScreen");

        gemPickup = Content.Load<SoundEffect>("gem_pickup");
        hit = Content.Load<SoundEffect>("hit");
        victory = Content.Load<SoundEffect>("victory");
        lose = Content.Load<SoundEffect>("lose");


        backgroundMusic = Content.Load<Song>("GameProject2 SoundTrack");
        MediaPlayer.IsRepeating = true;


    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        if (state == GameState.Playing)
        {    
            jet.Update(gameTime);

            jet.Color = Color.White;
            foreach(var gem in gems)
            {
                if (!gem.Collected && gem.Bounds.CollidesWith(jet.Bounds))
                {
                    jet.Color = Color.Gold;
                    gem.Collected = true;
                    gemsLeft--;
                    gemPickup.Play();
                    
                }
            }


            if (gemsLeft <= 0)
            {
                state = GameState.Win;
                completionTime = gameTime.TotalGameTime;
                MediaPlayer.Stop();
                victory.Play();
            }
            
            foreach(var orb in orbs)
            {
                orb.Update(gameTime);
                if (orb.Bounds.CollidesWith(jet.Bounds))
                {
                    state = GameState.Lose;
                    MediaPlayer.Stop();
                    hit.Play();
                    lose.Play();
                }
            }
        }
        else
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                ResetGame();
                gameTime.TotalGameTime = new System.TimeSpan(0);
                
            }
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.LightGray);
        // TODO: Add your drawing code here
        _spriteBatch.Begin();

        switch (state)
        {
            case GameState.Start:
                _spriteBatch.Draw(startScreen, new Vector2(0,0), Color.White);
                _spriteBatch.DrawString(bangers, $"Press ENTER to Start", new Vector2(200, 400 ), Color.Green);
                break;
            case GameState.Playing:
                _spriteBatch.Draw(background, new Vector2(0,0), Color.White);
                foreach (var gem in gems) gem.Draw(gameTime, _spriteBatch);
                foreach (var orb in orbs) orb.Draw(gameTime, _spriteBatch);
                jet.Draw(gameTime, _spriteBatch);
                _spriteBatch.DrawString(bangers, $"Gems left: {gemsLeft}", new Vector2(2,2), Color.Black);
                _spriteBatch.DrawString(bangers, $"Press ESC to quit", new Vector2(520,0), Color.Black);
                _spriteBatch.DrawString(bangers, $"Press SPACE to dash", new Vector2(520,30), Color.Black);
                break;
            case GameState.Win:
                GraphicsDevice.Clear(Color.LightGray);
                
                _spriteBatch.DrawString(bangers, $"You win!", new Vector2(200,150 ), Color.Green);
                _spriteBatch.DrawString(bangers, $"Your time: {completionTime:c}", new Vector2(200,200 ), Color.Black);
                _spriteBatch.DrawString(bangers, $"Press ENTER to reset", new Vector2(200, 300 ), Color.Black);
                break;
            case GameState.Lose:
                GraphicsDevice.Clear(Color.LightGray);
                
                _spriteBatch.DrawString(bangers, $"Game Over! :(", new Vector2(200,200 ), Color.Red);
                _spriteBatch.DrawString(bangers, $"Press ENTER to reset", new Vector2(200, 300 ), Color.Black);
                break;
        }


        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
