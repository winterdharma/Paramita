using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Paramita.Scenes.Game;
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


        // Checks for an ItemSelection input and returns an integer
        public static int CheckIfItemSelected()
        {
            if (IsNumberOne())
                return 1;
            if (IsNumberTwo())
                return 2;
            if (IsNumberThree())
                return 3;
            if (IsNumberFour())
                return 4;
            if (IsNumberFive())
                return 5;
            if (IsNumberSix())
                return 6;
            if (IsNumberSeven())
                return 7;
            if (IsNumberEight())
                return 8;
            if (IsNumberNine())
                return 9;
            if (IsNumberZero())
                return 10;

            return 0;
        }


        // Checks for an InventoryAction input and returns the corresponding action
        public static InventoryActions CheckForInventoryAction()
        {
            if(IsDKeyPressed())
                return InventoryActions.Drop;
            if (IsEKeyPressed())
                return InventoryActions.Equip;
            if (IsUKeyPressed())
                return InventoryActions.Use;
            if (IsCKeyPressed())
                return InventoryActions.Cancel;

            return InventoryActions.None;
        }

        /*
         * These methods provide keymapping to movement directions on the keyboard.
         * When checking for player movement input, call these methods.
         */

        private static bool IsLeft()
        {
            return CurrentKeyboardState.IsKeyDown(Keys.NumPad4)
                && PreviousKeyboardState.IsKeyUp(Keys.NumPad4);
        }

        private static bool IsRight()
        {
            return CurrentKeyboardState.IsKeyDown(Keys.NumPad6)
                && PreviousKeyboardState.IsKeyUp(Keys.NumPad6);
        }

        private static bool IsDown()
        {
            return CurrentKeyboardState.IsKeyDown(Keys.NumPad2)
                && PreviousKeyboardState.IsKeyUp(Keys.NumPad2);
        }

        private static bool IsUp()
        {
            return CurrentKeyboardState.IsKeyDown(Keys.NumPad8)
                && PreviousKeyboardState.IsKeyUp(Keys.NumPad8);
        }

        private static bool IsUpperLeft()
        {
            return CurrentKeyboardState.IsKeyDown(Keys.NumPad7)
                && PreviousKeyboardState.IsKeyUp(Keys.NumPad7);
        }

        private static bool IsUpperRight()
        {
            return CurrentKeyboardState.IsKeyDown(Keys.NumPad9)
                && PreviousKeyboardState.IsKeyUp(Keys.NumPad9);
        }

        private static bool IsLowerLeft()
        {
            return CurrentKeyboardState.IsKeyDown(Keys.NumPad1)
                && PreviousKeyboardState.IsKeyUp(Keys.NumPad1);
        }

        private static bool IsLowerRight()
        {
            return CurrentKeyboardState.IsKeyDown(Keys.NumPad3)
                && PreviousKeyboardState.IsKeyUp(Keys.NumPad3);
        }


        /*
         * These methods check for a number (0 to 9) key being pressed
         */
        private static bool IsNumberOne()
        {
            return CurrentKeyboardState.IsKeyDown(Keys.D1)
                && PreviousKeyboardState.IsKeyUp(Keys.D1);
        }

        private static bool IsNumberTwo()
        {
            return CurrentKeyboardState.IsKeyDown(Keys.D2)
                && PreviousKeyboardState.IsKeyUp(Keys.D2);
        }

        private static bool IsNumberThree()
        {
            return CurrentKeyboardState.IsKeyDown(Keys.D3)
                && PreviousKeyboardState.IsKeyUp(Keys.D3);
        }

        private static bool IsNumberFour()
        {
            return CurrentKeyboardState.IsKeyDown(Keys.D4)
                && PreviousKeyboardState.IsKeyUp(Keys.D4);
        }

        private static bool IsNumberFive()
        {
            return CurrentKeyboardState.IsKeyDown(Keys.D5)
                && PreviousKeyboardState.IsKeyUp(Keys.D5);
        }

        private static bool IsNumberSix()
        {
            return CurrentKeyboardState.IsKeyDown(Keys.D6)
                && PreviousKeyboardState.IsKeyUp(Keys.D6);
        }

        private static bool IsNumberSeven()
        {
            return CurrentKeyboardState.IsKeyDown(Keys.D7)
                && PreviousKeyboardState.IsKeyUp(Keys.D7);
        }

        private static bool IsNumberEight()
        {
            return CurrentKeyboardState.IsKeyDown(Keys.D8)
                && PreviousKeyboardState.IsKeyUp(Keys.D8);
        }

        private static bool IsNumberNine()
        {
            return CurrentKeyboardState.IsKeyDown(Keys.D9)
                && PreviousKeyboardState.IsKeyUp(Keys.D9);
        }

        private static bool IsNumberZero()
        {
            return CurrentKeyboardState.IsKeyDown(Keys.D0)
                && PreviousKeyboardState.IsKeyUp(Keys.D0);
        }

        /*
         *  These methods check for key presses that correspond to InventoryActions
         */
        private static bool IsDKeyPressed()
        {
            return CurrentKeyboardState.IsKeyDown(Keys.D)
                && PreviousKeyboardState.IsKeyUp(Keys.D);
        }

        private static bool IsEKeyPressed()
        {
            return CurrentKeyboardState.IsKeyDown(Keys.E)
                && PreviousKeyboardState.IsKeyUp(Keys.E);
        }

        private static bool IsUKeyPressed()
        {
            return CurrentKeyboardState.IsKeyDown(Keys.U)
                && PreviousKeyboardState.IsKeyUp(Keys.U);
        }

        private static bool IsCKeyPressed()
        {
            return CurrentKeyboardState.IsKeyDown(Keys.C)
                && PreviousKeyboardState.IsKeyUp(Keys.C);
        }
    }
}
