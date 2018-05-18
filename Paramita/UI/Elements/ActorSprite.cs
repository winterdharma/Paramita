using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.GameLogic.Mechanics;
using System.Collections.Generic;
using MonoGameUI.Elements;
using MonoGameUI.Base;
using System;

namespace Paramita.UI.Elements
{
    /// <summary>
    /// Sprite extends the Image element by allowing the Texture to be a spritesheet with 
    /// multiple frames that can be displayed dynamically. Currently, the class is designed
    /// for the Actors that have different images depending on their facing.
    /// </summary>
    public class ActorSprite : Sprite
    {
        #region Constructors
        public ActorSprite(string id, Component parent, Vector2 position, Texture2D spritesheet,
            Color unhighlighted, Color highlighted, int drawOrder) 
            : base(id, parent, position, spritesheet, unhighlighted, highlighted, drawOrder)
        {
            InitializeFacing();
        }

        public ActorSprite(string id, Component parent, Vector2 position, Texture2D spritesheet,
            int drawOrder)
            : base(id, parent, position, spritesheet, Color.White, Color.White, drawOrder)
        {
            InitializeFacing();
        }

        public ActorSprite(string id, Component parent, Point position, Texture2D spritesheet,
            Color unhighlighted, Color highlighted, int drawOrder)
            : base(id, parent, position, spritesheet, unhighlighted, highlighted, drawOrder)
        {
            InitializeFacing();
        }

        public ActorSprite(string id, Component parent, Point position, Texture2D spritesheet,
            int drawOrder)
            : base(id, parent, position, spritesheet, Color.White, Color.White, drawOrder)
        {
            InitializeFacing();
        }

        private void InitializeFacing()
        {
            Facing = Compass.East;
        }
        #endregion

        #region Properties
        public Compass Facing { get; set; }
        #endregion

        #region Public API
        public override void Update(GameTime gameTime)
        {
            Facing = SetCurrentFacing();
            CurrentFrame = SetCurrentFrame();
        }
        #endregion

        #region Helper Methods
        private Compass SetCurrentFacing()
        {
            if (Facing == Compass.North || Facing == Compass.Northeast
                || Facing == Compass.East || Facing == Compass.Southeast)
                return Compass.East;
            else
                return Compass.West;
        }

        /// <summary>
        /// Filters the possible Compass values to one of the two valid facings for an actor
        /// </summary>
        private int SetCurrentFrame()
        {
            if (Facing == Compass.East)
                return 0;
            else
                return 1;
        }
        #endregion
    }
}
