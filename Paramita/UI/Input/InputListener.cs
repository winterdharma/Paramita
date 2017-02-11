using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Paramita.UI.Input
{
    public class MouseEventArgs : EventArgs
    {
        public Point Position { get; private set; }
        public int ScrollWheelValue { get; private set; }

        public MouseEventArgs(Point position, int scrollWheelValue)
        {
            Position = position;
            ScrollWheelValue = scrollWheelValue;
        }
    }


    public static class InputListener
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
        public static event EventHandler<EventArgs> OnAnyKeyWasPressed;


        public static event EventHandler<EventArgs> OnLeftMouseButtonClicked;
        public static event EventHandler<EventArgs> OnMiddleMouseButtonClicked;
        public static event EventHandler<EventArgs> OnRightMouseButtonClicked;
        public static event EventHandler<MouseEventArgs> OnMousePositionChanged;
        public static event EventHandler<MouseEventArgs> OnMouseScrollWheelMoved;

        

        //public InputListener()
        //{
        //}



        public static void Update()
        {
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            CheckForKeyboardInput();
            CheckForMouseInput();
        }


        private static void CheckForKeyboardInput()
        {
            WasAnyKeyPressed();
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

        public static void CheckForMouseInput()
        {
            WasLeftMouseButtonClicked();
            WasMiddleMouseButtonClicked();
            WasRightMouseButtonClicked();
            WasMouseScrollWheelMoved();
            WasMousePositionChanged();
        }


        private static void WasLeftMouseButtonClicked()
        {
            if (_currentMouseState.LeftButton == ButtonState.Released
                && _previousMouseState.LeftButton == ButtonState.Pressed)
            {
                OnLeftMouseButtonClicked?.Invoke(null, EventArgs.Empty);
            }
        }

        private static void WasMiddleMouseButtonClicked()
        {
            if (_currentMouseState.MiddleButton == ButtonState.Released
                && _previousMouseState.MiddleButton == ButtonState.Pressed)
            {
                OnMiddleMouseButtonClicked?.Invoke(null, EventArgs.Empty);
            }
        }

        private static void WasRightMouseButtonClicked()
        {
            if (_currentMouseState.RightButton == ButtonState.Released
                && _previousMouseState.RightButton == ButtonState.Pressed)
            {
                OnRightMouseButtonClicked?.Invoke(null, EventArgs.Empty);
            }
        }

        private static void WasMouseScrollWheelMoved()
        {
            if (_currentMouseState.ScrollWheelValue != _previousMouseState.ScrollWheelValue)
            {
                OnMouseScrollWheelMoved?.Invoke(null, 
                    new MouseEventArgs(_currentMouseState.Position, _currentMouseState.ScrollWheelValue));
            }
        }

        private static void WasMousePositionChanged()
        {
            if (_currentMouseState.Position != _previousMouseState.Position)
            {
                OnMousePositionChanged?.Invoke(null, 
                    new MouseEventArgs(_currentMouseState.Position, _currentMouseState.ScrollWheelValue));
            }
        }


        private static void WasLeftKeyPressed()
        {
            if(_currentKeyboardState.IsKeyDown(Keys.Left)
                && _previousKeyboardState.IsKeyUp(Keys.Left))
            {
                OnLeftKeyWasPressed?.Invoke(null, EventArgs.Empty);         
            }
        }

        private static void WasRightKeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.Right)
                && _previousKeyboardState.IsKeyUp(Keys.Right))
            {
                OnRightKeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasUpKeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.Up)
                && _previousKeyboardState.IsKeyUp(Keys.Up))
            {
                OnUpKeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }

        private static void WasDownKeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.Down)
                && _previousKeyboardState.IsKeyUp(Keys.Down))
            {
                OnDownKeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasD0KeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.D0)
                && _previousKeyboardState.IsKeyUp(Keys.D0))
            {
                OnD0KeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasD1KeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.D1)
                && _previousKeyboardState.IsKeyUp(Keys.D1))
            {
                OnD1KeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasD2KeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.D2)
                && _previousKeyboardState.IsKeyUp(Keys.D2))
            {
                OnD2KeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasD3KeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.D3)
                && _previousKeyboardState.IsKeyUp(Keys.D3))
            {
                OnD3KeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasD4KeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.D4)
                && _previousKeyboardState.IsKeyUp(Keys.D4))
            {
                OnD4KeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasD5KeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.D5)
                && _previousKeyboardState.IsKeyUp(Keys.D5))
            {
                OnD5KeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasD6KeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.D6)
                && _previousKeyboardState.IsKeyUp(Keys.D6))
            {
                OnD6KeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasD7KeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.D7)
                && _previousKeyboardState.IsKeyUp(Keys.D7))
            {
                OnD7KeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasD8KeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.D8)
                && _previousKeyboardState.IsKeyUp(Keys.D8))
            {
                OnD8KeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasD9KeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.D9)
                && _previousKeyboardState.IsKeyUp(Keys.D9))
            {
                OnD9KeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasAKeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.A)
                && _previousKeyboardState.IsKeyUp(Keys.A))
            {
                OnAKeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasBKeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.B)
                && _previousKeyboardState.IsKeyUp(Keys.B))
            {
                OnBKeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasCKeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.C)
                && _previousKeyboardState.IsKeyUp(Keys.C))
            {
                OnCKeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasDKeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.D)
                && _previousKeyboardState.IsKeyUp(Keys.D))
            {
                OnDKeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasEKeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.E)
                && _previousKeyboardState.IsKeyUp(Keys.E))
            {
                OnEKeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasFKeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.F)
                && _previousKeyboardState.IsKeyUp(Keys.F))
            {
                OnFKeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasGKeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.G)
                && _previousKeyboardState.IsKeyUp(Keys.G))
            {
                OnGKeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasHKeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.H)
                && _previousKeyboardState.IsKeyUp(Keys.H))
            {
                OnHKeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasIKeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.I)
                && _previousKeyboardState.IsKeyUp(Keys.I))
            {
                OnIKeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasJKeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.J)
                && _previousKeyboardState.IsKeyUp(Keys.J))
            {
                OnJKeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasKKeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.K)
                && _previousKeyboardState.IsKeyUp(Keys.K))
            {
                OnKKeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasLKeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.L)
                && _previousKeyboardState.IsKeyUp(Keys.L))
            {
                OnLKeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasMKeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.M)
                && _previousKeyboardState.IsKeyUp(Keys.M))
            {
                OnMKeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasNKeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.N)
                && _previousKeyboardState.IsKeyUp(Keys.N))
            {
                OnNKeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasOKeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.O)
                && _previousKeyboardState.IsKeyUp(Keys.O))
            {
                OnOKeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasPKeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.P)
                && _previousKeyboardState.IsKeyUp(Keys.P))
            {
                OnPKeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasQKeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.Q)
                && _previousKeyboardState.IsKeyUp(Keys.Q))
            {
                OnQKeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasRKeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.R)
                && _previousKeyboardState.IsKeyUp(Keys.R))
            {
                OnRKeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasSKeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.S)
                && _previousKeyboardState.IsKeyUp(Keys.S))
            {
                OnSKeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasTKeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.T)
                && _previousKeyboardState.IsKeyUp(Keys.T))
            {
                OnTKeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasUKeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.U)
                && _previousKeyboardState.IsKeyUp(Keys.U))
            {
                OnUKeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasVKeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.V)
                && _previousKeyboardState.IsKeyUp(Keys.V))
            {
                OnVKeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasWKeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.W)
                && _previousKeyboardState.IsKeyUp(Keys.W))
            {
                OnWKeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasXKeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.X)
                && _previousKeyboardState.IsKeyUp(Keys.X))
            {
                OnXKeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasYKeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.Y)
                && _previousKeyboardState.IsKeyUp(Keys.Y))
            {
                OnYKeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasZKeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.Z)
                && _previousKeyboardState.IsKeyUp(Keys.Z))
            {
                OnZKeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasSpaceKeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.Space)
                && _previousKeyboardState.IsKeyUp(Keys.Space))
            {
                OnSpaceKeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasEscapeKeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.Escape)
                && _previousKeyboardState.IsKeyUp(Keys.Escape))
            {
                OnEscapeKeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
            
        private static void WasEnterKeyPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.Enter)
                && _previousKeyboardState.IsKeyUp(Keys.Enter))
            {
                OnEnterKeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }

        private static void WasAnyKeyPressed()
        {
            if(_currentKeyboardState != _previousKeyboardState)
            {
                OnAnyKeyWasPressed?.Invoke(null, EventArgs.Empty);
            }
        }
    }
}
