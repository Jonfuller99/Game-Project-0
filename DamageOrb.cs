using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Game_Project_0
{


    /// <summary>
    /// The direction enum 
    /// </summary>
    public enum Direction
    {
        Down = 0,
        Right = 1,
        Up =2 ,
        Left = 3,
    }
    


    /// <summary>
    /// A class representing a damage orb sprite sprite
    /// </summary>
    public class DamageOrbSprite
    {
        private Texture2D texture;
        private double directionTimer;
        private double animationTimer;
        private short animationFrame = 1;
        /// <summary>
        /// the direction of the bat
        /// </summary>
        public Direction Direction;

        /// <summary>
        /// The position of the bat
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("DamageOrb");
        }

        /// <summary>
        /// Updates the sprite's position based on user input
        /// </summary>
        /// <param name="gameTime">The GameTime</param>
        public void Update(GameTime gameTime)
        {
            directionTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if(directionTimer > 2.0)
            {
                switch (Direction)
                {
                    case Direction.Up:
                        Direction = Direction.Down;
                        break;
                    case Direction.Down:
                        Direction = Direction.Right;
                        break;
                    case Direction.Right:
                        Direction = Direction.Left;
                        break;
                    case Direction.Left:
                        Direction = Direction.Up;
                        break;                
                }
                directionTimer -= 2.0;
            }

            // Move damage orb
            switch(Direction)
            {
                    case Direction.Up:
                        Position += new Vector2(0, -1) * 100 * (float) gameTime.ElapsedGameTime.TotalSeconds;
                        break;
                    case Direction.Down:
                        Position += new Vector2(0, 1) * 100 * (float) gameTime.ElapsedGameTime.TotalSeconds;
                        break;
                    case Direction.Right:
                        Position += new Vector2(-1, 0) * 100 * (float) gameTime.ElapsedGameTime.TotalSeconds;
                        break;
                    case Direction.Left:
                        Position += new Vector2(1, 0) * 100 * (float) gameTime.ElapsedGameTime.TotalSeconds;
                        break;                
            }

            
        }            

        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {   
            //update animation timer
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            
            //update animation frame
            if (animationTimer > 0.3)
            {
                animationFrame++;
                if(animationFrame > 3) animationFrame = 1;
                animationTimer -= 0.3;
            }
            var source = new Rectangle(0, 0, 32, 32);
            spriteBatch.Draw(texture, Position, source, Color.White);
        }
    }
}