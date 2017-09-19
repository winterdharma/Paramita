using System;

namespace Paramita.UI.Base
{
    public class UserAction
    {
        public UserAction(Scene parent, Action<Scene> action, Predicate<Tuple<Scene, UserInputEventArgs>> predicate)
        {
            Action = action;
            Parent = parent;
            Predicate = predicate;
            Parent.UserInputEvent += OnUserInput;
        }

        public Action<Scene> Action { get; private set; }
        public Predicate<Tuple<Scene, UserInputEventArgs>> Predicate { get; private set; }
        public Scene Parent { get; private set; }

        public void ExecuteAction()
        {
            Action(Parent);
        }

        private void OnUserInput(object sender, UserInputEventArgs e)
        {
            var context = new Tuple<Scene, UserInputEventArgs>(Parent, e);
            if(Predicate(context))
            {
                ExecuteAction();
            }
        }
    }
}
