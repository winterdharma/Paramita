using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramita.Mechanics
{
    
        public enum MouseButtons
        {
            Left,
            Right,
            Middle
        }

    public class InputDevices : GameComponent
    {
        private KeyboardState currentKeyboardState = Keyboard.GetState();
        private KeyboardState previousKeyboardState = Keyboard.GetState();
        private MouseState currentMouseState = Mouse.GetState();
        private MouseState previousMouseState = Mouse.GetState();


        public MouseState CurrentMouseState
        {
            get { return currentMouseState; }
        }

        public KeyboardState CurrentKeyboardState
        {
            get { return currentKeyboardState; }
        }

        public KeyboardState PreviousKeyboardState
        {
            get { return previousKeyboardState; }
        }

        public MouseState PreviousMouseState
        {
            get { return previousMouseState; }
        }



        public InputDevices(GameController game) : base(game)
        {
        }



        public override void Update(GameTime gameTime)
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            base.Update(gameTime);
        }


        public void FlushInput()
        {
            currentMouseState = previousMouseState;
            currentKeyboardState = previousKeyboardState;
        }


        public bool CheckKeyReleased(Keys key)
        {
            return currentKeyboardState.IsKeyUp(key) &&
            previousKeyboardState.IsKeyDown(key);
        }


        public bool CheckMouseReleased(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.Left:
                    return (CurrentMouseState.LeftButton == ButtonState.Released) &&
                    (PreviousMouseState.LeftButton == ButtonState.Pressed);
                case MouseButtons.Right:
                    return (CurrentMouseState.RightButton == ButtonState.Released) &&
                    (PreviousMouseState.RightButton == ButtonState.Pressed);
                case MouseButtons.Middle:
                    return (CurrentMouseState.MiddleButton == ButtonState.Released) &&
                    (PreviousMouseState.MiddleButton == ButtonState.Pressed);
            }
            return false;
        }

        /*
         * This method checks for player movement input and returns a point
         * representing the direction that was moved, if any, that can be
         * used to adjust the current position to find the tile moved to.
         */ 
        public Compass MovedTo()
        {
            if (IsUp())
                return Compass.North;
            else if (IsDown())
                return Compass.South;
            else if (IsLeft())
                return Compass.West;
            else if (IsRight())
                return Compass.East;
            else if (IsUpperLeft())
                return Compass.Northwest;
            else if (IsUpperRight())
                return Compass.Northeast;
            else if (IsLowerLeft())
                return Compass.Southwest;
            else if (IsLowerRight())
                return Compass.Southeast;

            return Compass.None; 
        }

        public bool DroppedItem()
        {
            if (IsDropItem())
                return true;

            return false;
        }

        /*
         * These methods provide keymapping to movement directions on the keyboard.
         * When checking for player movement input, call these methods.
         */ 

        public bool IsLeft()
        {
            return CurrentKeyboardState.IsKeyDown(Keys.A)
                && PreviousKeyboardState.IsKeyUp(Keys.A);
        }

        public bool IsRight()
        {
            return CurrentKeyboardState.IsKeyDown(Keys.D)
                && PreviousKeyboardState.IsKeyUp(Keys.D);
        }

        public bool IsDown()
        {
            return CurrentKeyboardState.IsKeyDown(Keys.S)
                && PreviousKeyboardState.IsKeyUp(Keys.S);
        }

        public bool IsUp()
        {
            return CurrentKeyboardState.IsKeyDown(Keys.W)
                && PreviousKeyboardState.IsKeyUp(Keys.W);
        }

        public bool IsUpperLeft()
        {
            return CurrentKeyboardState.IsKeyDown(Keys.Q)
                && PreviousKeyboardState.IsKeyUp(Keys.Q);
        }

        public bool IsUpperRight()
        {
            return CurrentKeyboardState.IsKeyDown(Keys.E)
                && PreviousKeyboardState.IsKeyUp(Keys.E);
        }

        public bool IsLowerLeft()
        {
            return CurrentKeyboardState.IsKeyDown(Keys.Z)
                && PreviousKeyboardState.IsKeyUp(Keys.Z);
        }

        public bool IsLowerRight()
        {
            return CurrentKeyboardState.IsKeyDown(Keys.C)
                && PreviousKeyboardState.IsKeyUp(Keys.C);
        }

        public bool IsDropItem()
        {
            return CurrentKeyboardState.IsKeyDown(Keys.Delete)
                && PreviousKeyboardState.IsKeyUp(Keys.Delete);
        }
    }
}
