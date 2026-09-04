﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using CollisionExample.Collisions;

namespace Game_Project_0
{
    /// <summary>
    /// A class representing a jet
    /// </summary>
    public class JetSprite
    {
        private GamePadState gamePadState;

        private KeyboardState keyboardState;

        private Texture2D texture;

        private Texture2D hitbox;

        private Vector2 position = new Vector2(200, 200);

        private bool flipped;
        

        private BoudningRectangle bounds = new BoudningRectangle(new Vector2(200-32, 200-32), 64, 64);

        /// <summary>
        /// The boudning volume of the sprite
        /// </summary>
        public BoudningRectangle Bounds => bounds;

        /// <summary>
        /// The color to blend with the ghost
        /// </summary>
        public Color Color {get; set;} = Color.White;

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Jet");
            hitbox = content.Load<Texture2D>("64x64HitBox");
        }

        /// <summary>
        /// Updates the sprite's position based on user input
        /// </summary>
        /// <param name="gameTime">The GameTime</param>
        public void Update(GameTime gameTime)
        {
            gamePadState = GamePad.GetState(0);
            keyboardState = Keyboard.GetState();

            // Apply the gamepad movement with inverted Y axis
            position += gamePadState.ThumbSticks.Left * new Vector2(1, -1);
            if (gamePadState.ThumbSticks.Left.X < 0) flipped = true;
            if (gamePadState.ThumbSticks.Left.X > 0) flipped = false;

            // Apply keyboard movement
            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W)) position += new Vector2(0, -5);
            if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S)) position += new Vector2(0, 5);
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            { 
                position += new Vector2(-5, 0);
                flipped = true;
            }
            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                position += new Vector2(5, 0);
                flipped = false;
            }

            // update the bounding box center
            bounds.X = position.X - 32;
            bounds.Y = position.Y - 32;
        }

        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            SpriteEffects spriteEffects = (flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            
            spriteBatch.Draw(texture, position, null, Color, 0, new Vector2(32, 32), 1f, spriteEffects, 0);
            Rectangle rect = new ((int)bounds.X, (int)bounds.Y, (int)bounds.Width, (int)bounds.Height);
            spriteBatch.Draw(hitbox, rect, Color.White);
        }
    }
}