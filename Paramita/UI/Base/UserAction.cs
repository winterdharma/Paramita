using System;

namespace Paramita.UI.Base
{
    /// <summary>
    /// UserAction encapsulates the conditions for a user action to take place and the logic that
    /// executes that action. It evaluates UserInputEvents with the CanExecute Predicate and then
    /// performs the Action if the Predicate returns true. UserAction objects are held by the Scene
    /// class and collectively define the User's ability to interact with the active Scene.
    /// </summary>
    public class UserAction
    {
        public UserAction(Scene parent, Action<Scene, UserInputEventArgs> action, Predicate<Tuple<Scene, UserInputEventArgs>> predicate)
        {
            Action = action;
            Parent = parent;
            CanExecute = predicate;
            Parent.UserInputEvent += OnUserInput;
        }

        public Action<Scene, UserInputEventArgs> Action { get; private set; }
        public Predicate<Tuple<Scene, UserInputEventArgs>> CanExecute { get; private set; }
        public Scene Parent { get; private set; }

        public void ExecuteAction(UserInputEventArgs e)
        {
            Action(Parent, e);
        }

        private void OnUserInput(object sender, UserInputEventArgs e)
        {
            var context = new Tuple<Scene, UserInputEventArgs>(Parent, e);
            if(CanExecute(context))
            {
                ExecuteAction(e);
            }
        }
    }
}
