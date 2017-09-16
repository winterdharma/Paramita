using Paramita.UI.Input;
using System;

namespace Paramita.UI.Base
{
    public enum EventType
    {
        LeftClick,
        RightClick,
        DoublClick,
        MouseOver,
        MouseGone,
        ScrollWheel,
        Keyboard
    }

    public class UserInputEventArgs : EventArgs
    {
        public InputSource Source { get; }
        public EventType Type { get; }

        public UserInputEventArgs(InputSource eventSource, EventType eventType)
        {
            Source = eventSource;
            Type = eventType;
        }
    }
}