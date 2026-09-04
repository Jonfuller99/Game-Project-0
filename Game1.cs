using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game_Project_0;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private JetSprite jet;

    private GemSprite [] gems;

    private DamageOrbSprite [] orbs;

    private int gemsLeft;

    private SpriteFont bangers;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        jet = new();

        orbs = new DamageOrbSprite[]
        {
            new DamageOrbSprite(){Position = new Vector2(100,100), Direction = Direction.Down},
            new DamageOrbSprite(){Position = new Vector2(400,400), Direction = Direction.Up},
            new DamageOrbSprite(){Position = new Vector2(200,500), Direction = Direction.Left}
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

        base.Initialize();
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
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        jet.Update(gameTime);

        jet.Color = Color.White;
        foreach(var gem in gems)
        {
            if (!gem.Collected && gem.Bounds.CollidesWith(jet.Bounds))
            {
                jet.Color = Color.Gold;
                gem.Collected = true;
                gemsLeft--;
            }
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.LightGray);
        // TODO: Add your drawing code here



        _spriteBatch.Begin();
        foreach (var gem in gems) gem.Draw(gameTime, _spriteBatch);
        foreach (var orb in orbs) orb.Draw(gameTime, _spriteBatch);
        jet.Draw(gameTime, _spriteBatch);
        _spriteBatch.DrawString(bangers, $"Gems left: {gemsLeft}", new Vector2(2,2), Color.Black);

    
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
