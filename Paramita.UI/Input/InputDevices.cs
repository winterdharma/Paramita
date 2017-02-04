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
        None,
        Select1,
        Select2,
        Select3,
        Select4,
        Select5,
        Select6,
        Select7,
        Select8,
        Select9,
        Select0,
        Drop,
        Use,
        Equip,
        Cancel,
        TogglePanel
        
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

        public static event EventHandler<EventArgs> OnLeftKeyWasPressed;
        public static event EventHandler<EventArgs> OnRightKeyWasPressed;
        public static event EventHandler<EventArgs> OnUpKeyWasPressed;
        public static event EventHandler<EventArgs> OnDownKeyWasPressed;
        public static event EventHandler<EventArgs> OnD0KeyWasPressed;
        public static event EventHandler<EventArgs> OnD1KeyWasPressed;
        public static event EventHandler<EventArgs> OnD2KeyWasPressed;
        public static event EventHandler<EventArgs> OnD3KeyWasPressed;
        public static event EventHandler<EventArgs> OnD4KeyWasPressed;
        public static event EventHandler<EventArgs> OnD5KeyWasPressed;
        public static event EventHandler<EventArgs> OnD6KeyWasPressed;
        public static event EventHandler<EventArgs> OnD7KeyWasPressed;
        public static event EventHandler<EventArgs> OnD8KeyWasPressed;
        public static event EventHandler<EventArgs> OnD9KeyWasPressed;
        public static event EventHandler<EventArgs> OnAKeyWasPressed;
        public static event EventHandler<EventArgs> OnBKeyWasPressed;
        public static event EventHandler<EventArgs> OnCKeyWasPressed;
        public static event EventHandler<EventArgs> OnDKeyWasPressed;
        public static event EventHandler<EventArgs> OnEKeyWasPressed;
        public static event EventHandler<EventArgs> OnFKeyWasPressed;
        public static event EventHandler<EventArgs> OnGKeyWasPressed;
        public static event EventHandler<EventArgs> OnHKeyWasPressed;
        public static event EventHandler<EventArgs> OnIKeyWasPressed;
        public static event EventHandler<EventArgs> OnJKeyWasPressed;
        public static event EventHandler<EventArgs> OnKKeyWasPressed;
        public static event EventHandler<EventArgs> OnLKeyWasPressed;
        public static event EventHandler<EventArgs> OnMKeyWasPressed;
        public static event EventHandler<EventArgs> OnNKeyWasPressed;
        public static event EventHandler<EventArgs> OnOKeyWasPressed;
        public static event EventHandler<EventArgs> OnPKeyWasPressed;
        public static event EventHandler<EventArgs> OnQKeyWasPressed;
        public static event EventHandler<EventArgs> OnRKeyWasPressed;
        public static event EventHandler<EventArgs> OnSKeyWasPressed;
        public static event EventHandler<EventArgs> OnTKeyWasPressed;
        public static event EventHandler<EventArgs> OnUKeyWasPressed;
        public static event EventHandler<EventArgs> OnVKeyWasPressed;
        public static event EventHandler<EventArgs> OnWKeyWasPressed;
        public static event EventHandler<EventArgs> OnXKeyWasPressed;
        public static event EventHandler<EventArgs> OnYKeyWasPressed;
        public static event EventHandler<EventArgs> OnZKeyWasPressed;
        public static event EventHandler<EventArgs> OnSpaceKeyWasPressed;
        public static event EventHandler<EventArgs> OnEscapeKeyWasPressed;
        public static event EventHandler<EventArgs> OnEnterKeyWasPressed;

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
            WasLeftKeyPressed();
            WasRightKeyPressed();
            WasUpKeyPressed();
            WasDownKeyPressed();
            WasD0KeyPressed();
            WasD1KeyPressed();
            WasD2KeyPressed();
            WasD3KeyPressed();
            WasD4KeyPressed();
            WasD5KeyPressed();
            WasD6KeyPressed();
            WasD7KeyPressed();
            WasD8KeyPressed();
            WasD9KeyPressed();
            WasAKeyPressed();
            WasBKeyPressed();
            WasCKeyPressed();
            WasDKeyPressed();
            WasEKeyPressed();
            WasFKeyPressed();
            WasGKeyPressed();
            WasHKeyPressed();
            WasIKeyPressed();
            WasJKeyPressed();
            WasKKeyPressed();
            WasLKeyPressed();
            WasMKeyPressed();
            WasNKeyPressed();
            WasOKeyPressed();
            WasPKeyPressed();
            WasQKeyPressed();
            WasRKeyPressed();
            WasSKeyPressed();
            WasTKeyPressed();
            WasUKeyPressed();
            WasVKeyPressed();
            WasWKeyPressed();
            WasXKeyPressed();
            WasYKeyPressed();
            WasZKeyPressed();
            WasSpaceKeyPressed();
            WasEscapeKeyPressed();
            WasEnterKeyPressed();
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

        private void WasLeftKeyPressed()
        {
            if(CurrentKeyboardState.IsKeyDown(Keys.Left)
                && PreviousKeyboardState.IsKeyUp(Keys.Left))
            {
                OnLeftKeyWasPressed(this, EventArgs.Empty);         
            }
        }

        private void WasRightKeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.Right)
                && PreviousKeyboardState.IsKeyUp(Keys.Right))
            {
                OnRightKeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasUpKeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.Up)
                && PreviousKeyboardState.IsKeyUp(Keys.Up))
            {
                OnUpKeyWasPressed(this, EventArgs.Empty);
            }
        }

        private void WasDownKeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.Down)
                && PreviousKeyboardState.IsKeyUp(Keys.Down))
            {
                OnDownKeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasD0KeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.D0)
                && PreviousKeyboardState.IsKeyUp(Keys.D0))
            {
                OnD0KeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasD1KeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.D1)
                && PreviousKeyboardState.IsKeyUp(Keys.D1))
            {
                OnD1KeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasD2KeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.D2)
                && PreviousKeyboardState.IsKeyUp(Keys.D2))
            {
                OnD2KeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasD3KeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.D3)
                && PreviousKeyboardState.IsKeyUp(Keys.D3))
            {
                OnD3KeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasD4KeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.D4)
                && PreviousKeyboardState.IsKeyUp(Keys.D4))
            {
                OnD4KeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasD5KeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.D5)
                && PreviousKeyboardState.IsKeyUp(Keys.D5))
            {
                OnD5KeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasD6KeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.D6)
                && PreviousKeyboardState.IsKeyUp(Keys.D6))
            {
                OnD6KeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasD7KeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.D7)
                && PreviousKeyboardState.IsKeyUp(Keys.D7))
            {
                OnD7KeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasD8KeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.D8)
                && PreviousKeyboardState.IsKeyUp(Keys.D8))
            {
                OnD8KeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasD9KeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.D9)
                && PreviousKeyboardState.IsKeyUp(Keys.D9))
            {
                OnD9KeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasAKeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.A)
                && PreviousKeyboardState.IsKeyUp(Keys.A))
            {
                OnAKeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasBKeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.B)
                && PreviousKeyboardState.IsKeyUp(Keys.B))
            {
                OnBKeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasCKeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.C)
                && PreviousKeyboardState.IsKeyUp(Keys.C))
            {
                OnCKeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasDKeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.D)
                && PreviousKeyboardState.IsKeyUp(Keys.D))
            {
                OnDKeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasEKeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.E)
                && PreviousKeyboardState.IsKeyUp(Keys.E))
            {
                OnEKeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasFKeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.F)
                && PreviousKeyboardState.IsKeyUp(Keys.F))
            {
                OnFKeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasGKeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.G)
                && PreviousKeyboardState.IsKeyUp(Keys.G))
            {
                OnGKeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasHKeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.H)
                && PreviousKeyboardState.IsKeyUp(Keys.H))
            {
                OnHKeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasIKeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.I)
                && PreviousKeyboardState.IsKeyUp(Keys.I))
            {
                OnIKeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasJKeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.J)
                && PreviousKeyboardState.IsKeyUp(Keys.J))
            {
                OnJKeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasKKeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.K)
                && PreviousKeyboardState.IsKeyUp(Keys.K))
            {
                OnKKeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasLKeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.L)
                && PreviousKeyboardState.IsKeyUp(Keys.L))
            {
                OnLKeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasMKeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.M)
                && PreviousKeyboardState.IsKeyUp(Keys.M))
            {
                OnMKeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasNKeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.N)
                && PreviousKeyboardState.IsKeyUp(Keys.N))
            {
                OnNKeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasOKeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.O)
                && PreviousKeyboardState.IsKeyUp(Keys.O))
            {
                OnOKeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasPKeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.P)
                && PreviousKeyboardState.IsKeyUp(Keys.P))
            {
                OnPKeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasQKeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.Q)
                && PreviousKeyboardState.IsKeyUp(Keys.Q))
            {
                OnQKeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasRKeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.R)
                && PreviousKeyboardState.IsKeyUp(Keys.R))
            {
                OnRKeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasSKeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.S)
                && PreviousKeyboardState.IsKeyUp(Keys.S))
            {
                OnSKeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasTKeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.T)
                && PreviousKeyboardState.IsKeyUp(Keys.T))
            {
                OnTKeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasUKeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.U)
                && PreviousKeyboardState.IsKeyUp(Keys.U))
            {
                OnUKeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasVKeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.V)
                && PreviousKeyboardState.IsKeyUp(Keys.V))
            {
                OnVKeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasWKeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.W)
                && PreviousKeyboardState.IsKeyUp(Keys.W))
            {
                OnWKeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasXKeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.X)
                && PreviousKeyboardState.IsKeyUp(Keys.X))
            {
                OnXKeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasYKeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.Y)
                && PreviousKeyboardState.IsKeyUp(Keys.Y))
            {
                OnYKeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasZKeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.Z)
                && PreviousKeyboardState.IsKeyUp(Keys.Z))
            {
                OnZKeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasSpaceKeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.Space)
                && PreviousKeyboardState.IsKeyUp(Keys.Space))
            {
                OnSpaceKeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasEscapeKeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.Escape)
                && PreviousKeyboardState.IsKeyUp(Keys.Escape))
            {
                OnEscapeKeyWasPressed(this, EventArgs.Empty);
            }
        }
            
        private void WasEnterKeyPressed()
        {
            if (CurrentKeyboardState.IsKeyDown(Keys.Enter)
                && PreviousKeyboardState.IsKeyUp(Keys.Enter))
            {
                OnEnterKeyWasPressed(this, EventArgs.Empty);
            }
        }
    }
}
