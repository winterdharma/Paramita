using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Paramita.UI.Input
{

    public enum MouseButtons
    {
        Left,
        Right,
        Middle
    }

    public enum InventoryActions
    {
        Drop,
        Use,
        Equip,
        Cancel,
        TogglePanel,
        None
    }

    public enum Compass
    {
        North = 0,
        Northeast,
        East,
        Southeast,
        South,
        Southwest,
        West,
        Northwest,
        None
    }

    public class InputDevices
    {
        private static KeyboardState _currentKeyboardState = Keyboard.GetState();
        private static KeyboardState _previousKeyboardState = Keyboard.GetState();
        private static MouseState _currentMouseState = Mouse.GetState();
        private static MouseState _previousMouseState = Mouse.GetState();

        public static event EventHandler<KeyWasPressedEventArgs> OnKeyWasPressed;

        public static MouseState CurrentMouseState
        {
            get { return _currentMouseState; }
        }

        public static MouseState PreviousMouseState
        {
            get { return _previousMouseState; }
        }

        public static KeyboardState CurrentKeyboardState
        {
            get { return _currentKeyboardState; }
        }

        public static KeyboardState PreviousKeyboardState
        {
            get { return _previousKeyboardState; }
        }

        



        public InputDevices()
        {
        }



        public void Update(GameTime gameTime)
        {
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
            CheckForKeyboardInput();
        }


        private void CheckForKeyboardInput()
        {
            CheckForPlayerMoveInput();
        }

        private void CheckForPlayerMoveInput()
        {
            WasKeyPressed(Keys.Left);
            WasKeyPressed(Keys.Right);
            WasKeyPressed(Keys.Up);
            WasKeyPressed(Keys.Down);
        }


        public static void FlushInput()
        {
            _currentMouseState = _previousMouseState;
            _currentKeyboardState = _previousKeyboardState;
        }


        public static bool CheckKeyReleased(Keys key)
        {
            return _currentKeyboardState.IsKeyUp(key) &&
            _previousKeyboardState.IsKeyDown(key);
        }


        public static bool CheckMouseReleased(MouseButtons button)
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

        private static void WasKeyPressed(Keys key)
        {
            if(CurrentKeyboardState.IsKeyDown(key)
                && PreviousKeyboardState.IsKeyUp(key) && OnKeyWasPressed != null)
            {
                OnKeyWasPressed(null, new KeyWasPressedEventArgs(key));
            }
        }
    }

    public class KeyWasPressedEventArgs : EventArgs
    {
        public Keys Key { get; set; }

        public KeyWasPressedEventArgs(Keys key)
        {
            Key = key;
        }
    }
}
