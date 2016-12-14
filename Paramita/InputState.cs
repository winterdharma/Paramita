using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Paramita
{
    public class InputState
    {
        public const int MaxInputs = 4;

        public readonly GamePadState[] CurrentGamePadStates;
        public readonly KeyboardState[] CurrentKeyboardStates;
        public readonly bool[] GamePadWasConnected;
        public readonly GamePadState[] LastGamePadStates;
        public readonly KeyboardState[] LastKeyboardStates;



      public InputState()
      {
         CurrentKeyboardStates = new KeyboardState[MaxInputs];
         CurrentGamePadStates = new GamePadState[MaxInputs];

         LastKeyboardStates = new KeyboardState[MaxInputs];
         LastGamePadStates = new GamePadState[MaxInputs];

         CurrentMouseState = new MouseState();
         LastMouseState = new MouseState();

         GamePadWasConnected = new bool[MaxInputs];
      }


      public MouseState CurrentMouseState { get; private set; }
      public MouseState LastMouseState { get; private set; }


      /// <summary>
      ///    Reads the latest state of the keyboard and gamepad.
      /// </summary>
      public void Update()
      {
         for ( int i = 0; i<MaxInputs; i++ )
         {
            LastKeyboardStates[i] = CurrentKeyboardStates[i];
            LastGamePadStates[i] = CurrentGamePadStates[i];

            CurrentKeyboardStates[i] = Keyboard.GetState( (PlayerIndex) i );
            CurrentGamePadStates[i] = GamePad.GetState( (PlayerIndex) i );

            // Keep track of whether a gamepad has ever been
            // connected, so we can detect if it is unplugged.
            if ( CurrentGamePadStates[i].IsConnected )
            {
               GamePadWasConnected[i] = true;
            }
         }

         LastMouseState = CurrentMouseState;
         CurrentMouseState = Mouse.GetState();
      }



      public bool IsNewLeftMouseClick(out MouseState mouseState)
      {
         mouseState = CurrentMouseState;
         return ( CurrentMouseState.LeftButton == ButtonState.Released && LastMouseState.LeftButton == ButtonState.Pressed );
      }



      public bool IsNewRightMouseClick(out MouseState mouseState)
      {
         mouseState = CurrentMouseState;
         return ( CurrentMouseState.RightButton == ButtonState.Released && LastMouseState.RightButton == ButtonState.Pressed );
      }



      public bool IsNewThirdMouseClick(out MouseState mouseState)
      {
         mouseState = CurrentMouseState;
         return ( CurrentMouseState.MiddleButton == ButtonState.Pressed && LastMouseState.MiddleButton == ButtonState.Released );
      }



      public bool IsNewMouseScrollUp(out MouseState mouseState)
      {
         mouseState = CurrentMouseState;
         return ( CurrentMouseState.ScrollWheelValue > LastMouseState.ScrollWheelValue );
      }



      public bool IsNewMouseScrollDown(out MouseState mouseState)
      {
         mouseState = CurrentMouseState;
         return ( CurrentMouseState.ScrollWheelValue<LastMouseState.ScrollWheelValue );
      }



      /// <summary>
      ///    Helper for checking if a key was newly pressed during this update. The
      ///    controllingPlayer parameter specifies which player to read input for.
      ///    If this is null, it will accept input from any player. When a keypress
      ///    is detected, the output playerIndex reports which player pressed it.
      /// </summary>

      public bool IsNewKeyPress(Keys key, PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
      {
         if ( controllingPlayer.HasValue )
         {
            // Read input from the specified player.
            playerIndex = controllingPlayer.Value;

            var i = (int)playerIndex;

            return ( CurrentKeyboardStates[i].IsKeyDown(key ) && LastKeyboardStates[i].IsKeyUp(key ) );
         }
         else
         {
            // Accept input from any player.
            return ( IsNewKeyPress(key, PlayerIndex.One, out playerIndex ) || IsNewKeyPress(key, PlayerIndex.Two, out playerIndex )
                     || IsNewKeyPress(key, PlayerIndex.Three, out playerIndex ) || IsNewKeyPress(key, PlayerIndex.Four, out playerIndex ) );
         }
      }



      /// <summary>
      ///    Helper for checking if a button was newly pressed during this update.
      ///    The controllingPlayer parameter specifies which player to read input for.
      ///    If this is null, it will accept input from any player. When a button press
      ///    is detected, the output playerIndex reports which player pressed it.
      /// </summary>
      public bool IsNewButtonPress(Buttons button, PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
      {
         if ( controllingPlayer.HasValue )
         {
            // Read input from the specified player.
            playerIndex = controllingPlayer.Value;

            var i = (int)playerIndex;

            return ( CurrentGamePadStates[i].IsButtonDown(button ) && LastGamePadStates[i].IsButtonUp(button ) );
         }
         else
         {
            // Accept input from any player.
            return ( IsNewButtonPress(button, PlayerIndex.One, out playerIndex ) || IsNewButtonPress(button, PlayerIndex.Two, out playerIndex )
                     || IsNewButtonPress(button, PlayerIndex.Three, out playerIndex ) || IsNewButtonPress(button, PlayerIndex.Four, out playerIndex ) );
         }
      }



      public bool IsKeyPressed(Keys key, PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
      {
         if ( controllingPlayer.HasValue )
         {
            // Read input from the specified player.
            playerIndex = controllingPlayer.Value;

            var i = (int)playerIndex;

            return ( CurrentKeyboardStates[i].IsKeyDown(key ) );
         }
         else
         {
            // Accept input from any player.
            return ( IsKeyPressed(key, PlayerIndex.One, out playerIndex ) || IsKeyPressed(key, PlayerIndex.Two, out playerIndex )
                     || IsKeyPressed(key, PlayerIndex.Three, out playerIndex ) || IsKeyPressed(key, PlayerIndex.Four, out playerIndex ) );
         }
      }



      public bool IsButtonPressed(Buttons button, PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
      {
         if ( controllingPlayer.HasValue )
         {
            // Read input from the specified player.
            playerIndex = controllingPlayer.Value;

            var i = (int)playerIndex;

            return ( CurrentGamePadStates[i].IsButtonDown(button ) );
         }
         else
         {
            // Accept input from any player.
            return ( IsButtonPressed(button, PlayerIndex.One, out playerIndex ) || IsButtonPressed(button, PlayerIndex.Two, out playerIndex )
                     || IsButtonPressed(button, PlayerIndex.Three, out playerIndex ) || IsButtonPressed(button, PlayerIndex.Four, out playerIndex ) );
         }
      }



      public bool IsExitGame(PlayerIndex? controllingPlayer)
      {
         PlayerIndex playerIndex;

         return IsNewKeyPress(Keys.Escape, controllingPlayer, out playerIndex ) || IsNewButtonPress(Buttons.Back, controllingPlayer, out playerIndex );
      }



      public bool IsLeft(PlayerIndex? controllingPlayer)
      {
         PlayerIndex playerIndex;
         return IsNewKeyPress(Keys.Left, controllingPlayer, out playerIndex ) || IsNewButtonPress(Buttons.DPadLeft, controllingPlayer, out playerIndex )
                || IsNewButtonPress(Buttons.LeftThumbstickLeft, controllingPlayer, out playerIndex );
      }


      public bool IsRight(PlayerIndex? controllingPlayer)
      {
         PlayerIndex playerIndex;

         return IsNewKeyPress(Keys.Right, controllingPlayer, out playerIndex ) || IsNewButtonPress(Buttons.DPadRight, controllingPlayer, out playerIndex )
                || IsNewButtonPress(Buttons.LeftThumbstickRight, controllingPlayer, out playerIndex );
      }


      public bool IsUp(PlayerIndex? controllingPlayer)
      {
         PlayerIndex playerIndex;

         return IsNewKeyPress(Keys.Up, controllingPlayer, out playerIndex ) || IsNewButtonPress(Buttons.DPadUp, controllingPlayer, out playerIndex )
                || IsNewButtonPress(Buttons.LeftThumbstickUp, controllingPlayer, out playerIndex );
      }


      public bool IsDown(PlayerIndex? controllingPlayer)
      {
         PlayerIndex playerIndex;

         return IsNewKeyPress(Keys.Down, controllingPlayer, out playerIndex ) || IsNewButtonPress(Buttons.DPadDown, controllingPlayer, out playerIndex )
                || IsNewButtonPress(Buttons.LeftThumbstickDown, controllingPlayer, out playerIndex );
      }

      public bool IsSpace(PlayerIndex? controllingPlayer)
      {
         PlayerIndex playerIndex;
         return IsNewKeyPress(Keys.Space, controllingPlayer, out playerIndex );
      }
   }
}
