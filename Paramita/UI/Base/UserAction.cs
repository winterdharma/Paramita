using Microsoft.Xna.Framework.Input;
using Paramita.UI.Input;
using System;

namespace Paramita.UI.Base
{
    public class UserAction
    {

        public UserAction(Action<Scene> action, Scene parent, InputSource source, EventType inputType)
        {
            Action = action;
            Parent = parent;
            InputSource = source;
            InputType = inputType;
            Parent.UserInputEvent += OnUserInput;
        }

        public Action<Scene> Action { get; private set; }
        public Scene Parent { get; private set; }
        public InputSource InputSource { get; private set; }
        public EventType InputType { get; private set; }

        public void ExecuteAction()
        {
            Action(Parent);
        }

        private void OnUserInput(object sender, UserInputEventArgs e)
        {
            if(e.EventSource is Keys && InputSource.Contains(e.EventSource) ||
                InputType == e.EventType && InputSource.Contains(e.EventSource))
            {
                ExecuteAction();
            }
        }
    }
}
