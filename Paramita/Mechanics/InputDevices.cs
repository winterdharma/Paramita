using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Paramita.UI;
using Paramita.UI.Input;

namespace Paramita.Mechanics
{

    public enum MouseButtonsApp
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


        public static MouseState CurrentMouseState
        {
            get { return currentMouseState; }
        }

        public static KeyboardState CurrentKeyboardState
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


        public static bool CheckMouseReleased(MouseButtonsApp button)
        {
            switch (button)
            {
                case MouseButtonsApp.Left:
                    return (CurrentMouseState.LeftButton == ButtonState.Released) &&
                    (PreviousMouseState.LeftButton == ButtonState.Pressed);
                case MouseButtonsApp.Right:
                    return (CurrentMouseState.RightButton == ButtonState.Released) &&
                    (PreviousMouseState.RightButton == ButtonState.Pressed);
                case MouseButtonsApp.Middle:
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
        public static Compass Moved()
        {
            if (IsKeyPressed(Keys.NumPad8))
                return Compass.North;
            else if (IsKeyPressed(Keys.NumPad2))
                return Compass.South;
            else if (IsKeyPressed(Keys.NumPad4))
                return Compass.West;
            else if (IsKeyPressed(Keys.NumPad6))
                return Compass.East;
            else if (IsKeyPressed(Keys.NumPad7))
                return Compass.Northwest;
            else if (IsKeyPressed(Keys.NumPad9))
                return Compass.Northeast;
            else if (IsKeyPressed(Keys.NumPad1))
                return Compass.Southwest;
            else if (IsKeyPressed(Keys.NumPad3))
                return Compass.Southeast;

            return Compass.None; 
        }


        // Checks for an ItemSelection input and returns an integer
        public static int CheckIfItemSelected()
        {
            if (IsKeyPressed(Keys.D1))
                return 1;
            if (IsKeyPressed(Keys.D2))
                return 2;
            if (IsKeyPressed(Keys.D3))
                return 3;
            if (IsKeyPressed(Keys.D4))
                return 4;
            if (IsKeyPressed(Keys.D5))
                return 5;
            if (IsKeyPressed(Keys.D6))
                return 6;
            if (IsKeyPressed(Keys.D7))
                return 7;
            if (IsKeyPressed(Keys.D8))
                return 8;
            if (IsKeyPressed(Keys.D9))
                return 9;
            if (IsKeyPressed(Keys.D0))
                return 10;

            return 0;
        }


        // Checks for an InventoryAction input and returns the corresponding action
        public static InventoryActions CheckForInventoryAction()
        {
            if(IsKeyPressed(Keys.D))
                return InventoryActions.Drop;
            if (IsKeyPressed(Keys.E))
                return InventoryActions.Equip;
            if (IsKeyPressed(Keys.U))
                return InventoryActions.Use;
            if (IsKeyPressed(Keys.C))
                return InventoryActions.Cancel;
            if (IsKeyPressed(Keys.I))
                return InventoryActions.TogglePanel;

            return InventoryActions.None;
        }

        
        private static bool IsKeyPressed(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key)
                && PreviousKeyboardState.IsKeyUp(key);
        }
    }
}
