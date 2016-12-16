using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramita.Components
{
    
        public enum MouseButtons
        {
            Left,
            Right,
            Middle
        }

    public class InputDevices : GameComponent
    {
        private static KeyboardState currentKeyboardState = Keyboard.GetState();
        private static KeyboardState previousKeyboardState = Keyboard.GetState();
        private static MouseState currentMouseState = Mouse.GetState();
        private static MouseState previousMouseState = Mouse.GetState();


        public static MouseState MouseState
        {
            get { return currentMouseState; }
        }

        public static KeyboardState KeyboardState
        {
            get { return currentKeyboardState; }
        }

        public static KeyboardState PreviousKeyboardState
        {
            get { return previousKeyboardState; }
        }

        public static MouseState PreviousMouseState
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


        public static void FlushInput()
        {
            currentMouseState = previousMouseState;
            currentKeyboardState = previousKeyboardState;
        }


        public static bool CheckKeyReleased(Keys key)
        {
            return currentKeyboardState.IsKeyUp(key) &&
            previousKeyboardState.IsKeyDown(key);
        }


        public static bool CheckMouseReleased(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.Left:
                    return (currentMouseState.LeftButton == ButtonState.Released) &&
                    (previousMouseState.LeftButton == ButtonState.Pressed);
                case MouseButtons.Right:
                    return (currentMouseState.RightButton == ButtonState.Released) &&
                    (previousMouseState.RightButton == ButtonState.Pressed);
                case MouseButtons.Middle:
                    return (currentMouseState.MiddleButton == ButtonState.Released) &&
                    (previousMouseState.MiddleButton == ButtonState.Pressed);
            }
            return false;
        }
    }
}
