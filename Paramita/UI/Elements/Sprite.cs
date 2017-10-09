using Paramita.UI.Base;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.GameLogic.Mechanics;
using System.Collections.Generic;

namespace Paramita.UI.Elements
{
    /// <summary>
    /// Sprite extends the Image element by allowing the Texture to be a spritesheet with 
    /// multiple frames that can be displayed dynamically. Currently, the class is designed
    /// for the Actors that have different images depending on their facing.
    /// </summary>
    public class Sprite : Image
    {
        #region Fields
        private Compass _facing;
        private readonly Rectangle _frame = new Rectangle(0, 0, 32, 32);
        #endregion

        #region Constructors
        public Sprite(string id, Component parent, Vector2 position, Texture2D spritesheet,
            Color unhighlighted, Color highlighted, int drawOrder) 
            : base(id, parent, position, spritesheet, unhighlighted, highlighted, drawOrder)
        {
            SetSpriteProperties();
        }

        public Sprite(string id, Component parent, Vector2 position, Texture2D spritesheet,
            int drawOrder)
            : base(id, parent, position, spritesheet, Color.White, Color.White, drawOrder)
        {
            SetSpriteProperties();
        }

        public Sprite(string id, Component parent, Point position, Texture2D spritesheet,
            Color unhighlighted, Color highlighted, int drawOrder)
            : base(id, parent, position, spritesheet, unhighlighted, highlighted, drawOrder)
        {
            SetSpriteProperties();
        }

        public Sprite(string id, Component parent, Point position, Texture2D spritesheet,
            int drawOrder)
            : base(id, parent, position, spritesheet, Color.White, Color.White, drawOrder)
        {
            SetSpriteProperties();
        }

        private void SetSpriteProperties()
        {
            Frames = CreateFrames();
            Facing = Compass.East;
        }
        #endregion

        #region Properties
        public Compass Facing
        {
            get => _facing;
            set
            {
                _facing = value;
                SetCurrentFacing();
            }
        }
        public Dictionary<Compass, Rectangle> Frames { get; set; }
        #endregion

        #region Initialization
        protected override Rectangle CreateRectangle()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, _frame.Height, _frame.Width);
        }

        private Dictionary<Compass, Rectangle> CreateFrames()
        {
            var dictionary = new Dictionary<Compass, Rectangle>
            {
                { Compass.East, new Rectangle(0, 0, _frame.Width, _frame.Height) },
                { Compass.West, new Rectangle(32, 0, _frame.Width, _frame.Height) }
            };

            return dictionary;
        }
        #endregion

        #region Public API
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(Visible)
                spriteBatch.Draw(Texture, Position, Frames[Facing], Color.White);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position)
        {
            if (Visible)
                spriteBatch.Draw(Texture, position, Frames[Facing], Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            if (Enabled)
                SetCurrentFacing();
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Filters the possible Compass values to one of the two valid facings for an actor
        /// </summary>
        private void SetCurrentFacing()
        {
            if (Facing == Compass.North || Facing == Compass.Northeast
                || Facing == Compass.East || Facing == Compass.Southeast)
                _facing = Compass.East;
            else if (Facing == Compass.South || Facing == Compass.Northwest
                || Facing == Compass.West || Facing == Compass.Southwest)
                _facing = Compass.West;
        }
        #endregion
    }
}
