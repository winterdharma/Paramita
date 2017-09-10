using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input.InputListeners;
using Paramita.GameLogic.Utility;
using System;

namespace Paramita.UI.Input
{
    /*
     * This class evaluates raw input events from the InputListeners and broadcasts specific input events
     * to the UI. This class is not aware of the meaning of any given input event. Ideally, it should broadcast
     * all possible input events, but currently only broadcasts input that the UI currently subscribes to.
     * 
     * Example: A KeyboardListener.KeyPressed event is evaluated and broadcast as a DKeyPressed event which
     * has InventoryPanel as a subscriber for the DropItem action.
     */
    public class InputResponder
    {
        #region Fields
        private KeyboardListener _keyboard;
        private MouseListener _mouse;
        #endregion


        #region Events
        public event EventHandler<EventArgs> KeyPressed;
        public event EventHandler<EventArgs> EnterKeyPressed;
        public event EventHandler<EventArgs> LeftKeyPressed;
        public event EventHandler<EventArgs> RightKeyPressed;
        public event EventHandler<EventArgs> UpKeyPressed;
        public event EventHandler<EventArgs> DownKeyPressed;
        public event EventHandler<EventArgs> D0KeyPressed;
        public event EventHandler<EventArgs> D1KeyPressed;
        public event EventHandler<EventArgs> D2KeyPressed;
        public event EventHandler<EventArgs> D3KeyPressed;
        public event EventHandler<EventArgs> D4KeyPressed;
        public event EventHandler<EventArgs> D5KeyPressed;
        public event EventHandler<EventArgs> D6KeyPressed;
        public event EventHandler<EventArgs> D7KeyPressed;
        public event EventHandler<EventArgs> D8KeyPressed;
        public event EventHandler<EventArgs> D9KeyPressed;
        public event EventHandler<EventArgs> DKeyPressed;
        public event EventHandler<EventArgs> EKeyPressed;
        public event EventHandler<EventArgs> CKeyPressed;
        public event EventHandler<EventArgs> UKeyPressed;
        public event EventHandler<EventArgs> IKeyPressed;
        public event EventHandler<EventArgs> MouseClick;
        public event EventHandler<EventArgs> LeftMouseClick;
        public event EventHandler<EventArgs> DoubleLeftMouseClick;
        public event EventHandler<EventArgs> RightMouseClick;
        public event EventHandler<PointEventArgs> NewMousePosition;
        public event EventHandler<IntegerEventArgs> ScrollWheelMoved;
        #endregion


        public InputResponder(KeyboardListener keyboard, MouseListener mouse)
        {
            _keyboard = keyboard;
            _mouse = mouse;
            

            SubscribeToInputListenerEvents();
        }

        private void SubscribeToInputListenerEvents()
        {
            _keyboard.KeyPressed += OnKeyPressed;
            _mouse.MouseClicked += OnMouseClick;
            _mouse.MouseMoved += OnMouseMoved;
            _mouse.MouseDoubleClicked += OnMouseDoubleClick;
            _mouse.MouseWheelMoved += OnMouseScrollWheelMove;
        }

        private void OnMouseMoved(object sender, MouseEventArgs e)
        {
            NewMousePosition?.Invoke(this, new PointEventArgs(e.Position));
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            MouseClick?.Invoke(this, new EventArgs());
            if (e.Button == MouseButton.Left) LeftMouseClick?.Invoke(this, new EventArgs());
            else if (e.Button == MouseButton.Right) RightMouseClick?.Invoke(this, new EventArgs());
        }

        private void OnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButton.Left) DoubleLeftMouseClick?.Invoke(this, new EventArgs());
        }

        private void OnMouseScrollWheelMove(object sender, MouseEventArgs e)
        {
            ScrollWheelMoved?.Invoke(this, new IntegerEventArgs(e.ScrollWheelDelta));
        }

        private void OnKeyPressed(object sender, KeyboardEventArgs e)
        {
            KeyPressed?.Invoke(this, new EventArgs());

            switch (e.Key)
            {
                case Keys.Enter:
                    EnterKeyPressed?.Invoke(this, new EventArgs());
                    break;
                case Keys.Left:
                    LeftKeyPressed?.Invoke(this, new EventArgs());
                    break;
                case Keys.Right:
                    RightKeyPressed?.Invoke(this, new EventArgs());
                    break;
                case Keys.Up:
                    UpKeyPressed?.Invoke(this, new EventArgs());
                    break;
                case Keys.Down:
                    DownKeyPressed?.Invoke(this, new EventArgs());
                    break;
                case Keys.D0:
                    D0KeyPressed?.Invoke(this, new EventArgs());
                    break;
                case Keys.D1:
                    D1KeyPressed?.Invoke(this, new EventArgs());
                    break;
                case Keys.D2:
                    D2KeyPressed?.Invoke(this, new EventArgs());
                    break;
                case Keys.D3:
                    D3KeyPressed?.Invoke(this, new EventArgs());
                    break;
                case Keys.D4:
                    D4KeyPressed?.Invoke(this, new EventArgs());
                    break;
                case Keys.D5:
                    D5KeyPressed?.Invoke(this, new EventArgs());
                    break;
                case Keys.D6:
                    D6KeyPressed?.Invoke(this, new EventArgs());
                    break;
                case Keys.D7:
                    D7KeyPressed?.Invoke(this, new EventArgs());
                    break;
                case Keys.D8:
                    D8KeyPressed?.Invoke(this, new EventArgs());
                    break;
                case Keys.D9:
                    D9KeyPressed?.Invoke(this, new EventArgs());
                    break;
                case Keys.D:
                    DKeyPressed?.Invoke(this, new EventArgs());
                    break;
                case Keys.E:
                    EKeyPressed?.Invoke(this, new EventArgs());
                    break;
                case Keys.C:
                    CKeyPressed?.Invoke(this, new EventArgs());
                    break;
                case Keys.U:
                    UKeyPressed?.Invoke(this, new EventArgs());
                    break;
                case Keys.I:
                    IKeyPressed?.Invoke(this, new EventArgs());
                    break;
                default: return;
            }
        }
    }
}
