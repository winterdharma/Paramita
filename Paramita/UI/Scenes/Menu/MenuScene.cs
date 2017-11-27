using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameUI;
using MonoGameUI.Base;
using MonoGameUI.Components;
using MonoGameUI.Elements;
using MonoGameUI.Events;
using System;
using System.Collections.Generic;

namespace Paramita.UI.Scenes
{
    public class MenuScene : Scene
    {
        #region Fields
        private Texture2D _background;
        private SpriteFont _fontArialBold;
        private Texture2D _buttonTexture;
        private const string NEW_GAME_LABEL = "NEW GAME";
        private const string CONTINUE_LABEL = "CONTINUE";
        private const string OPTIONS_LABEL = "OPTIONS";
        private const string EXIT_LABEL = "EXIT";

        private List<string> _buttonIds = new List<string>()
        {
            "new_game_button", "continue_button", "options_button", "exit_button"
        };
        private int _selectedButtonId;
        #endregion

        #region Constructors
        public MenuScene(GameController game) : base(game)
        {
            Controller = (ParamitaController)game;
        }
        #endregion

        #region Properties
        public int SelectedButtonId
        {
            get => _selectedButtonId;
            set
            {
                string buttonId = _buttonIds[_selectedButtonId];
                Components[0].Elements[buttonId].Unhighlight();

                _selectedButtonId = value;

                buttonId = _buttonIds[_selectedButtonId];
                Components[0].Elements[buttonId].Highlight();
            }
        }
        public ParamitaController Controller { get; }
        #endregion

        #region Initialization
        public override void Initialize()
        {
            base.Initialize();
            var screen = new Screen(this, 0);
            screen.Elements = new Dictionary<string, Element>()
            {
                { "background", new Background("background", screen, ScreenRectangle.Location,
                    _background, Color.White, Color.White, ScreenRectangle.Size, 0)
                },
                { "new_game_button", new Button(_buttonIds[0], screen, 
                    new Point(1200 - _buttonTexture.Width, 90), _buttonTexture, 
                    new LineOfText("new_game_label", screen, new Vector2(0,0), NEW_GAME_LABEL, _fontArialBold,
                        Color.White, Color.Red, 2),
                    Color.White, Color.White, 1) },
                { "continue_button", new Button(_buttonIds[1], screen, 
                    new Point(1200 - _buttonTexture.Width, _buttonTexture.Height + 140), _buttonTexture,
                    new LineOfText("continue_label", screen, new Vector2(0,0), CONTINUE_LABEL, _fontArialBold,
                        Color.White, Color.Red, 2),
                    Color.White, Color.White, 1) },
                { "options_button", new Button(_buttonIds[2], screen, 
                    new Point(1200 - _buttonTexture.Width, (_buttonTexture.Height * 2) + 190), _buttonTexture,
                    new LineOfText("options_label", screen, new Vector2(0,0), OPTIONS_LABEL, _fontArialBold,
                        Color.White, Color.Red, 2),
                    Color.White, Color.White, 1) },
                { "exit_button", new Button(_buttonIds[3], screen, 
                    new Point(1200 - _buttonTexture.Width, (_buttonTexture.Height * 3) + 240), _buttonTexture,
                    new LineOfText("exit_label", screen, new Vector2(0,0), EXIT_LABEL, _fontArialBold,
                        Color.White, Color.Red, 2),
                    Color.White, Color.White, 1) }
            };
            Components = InitializeComponents(screen);
            UserActions = InitializeUserActions(Components);
        }

        protected override List<UserAction> InitializeUserActions(List<Component> components)
        {
            var actionsList = new List<UserAction>()
            {
                new UserAction(this, SelectButton, CanSelectButton),
                //new UserAction(this, DeselectButton, CanDeselectButton),
                new UserAction(this, SelectNewGame, CanSelectNewGame),
                //new UserAction(this, SelectContinueGame, CanSelectContinueGame),
                //new UserAction(this, SelectOptions, CanSelectOptions),
                new UserAction(this, ExitGame, CanExitGame)
            };
            return actionsList;
        }

        private bool CanSelectButton(Tuple<Scene, UserInputEventArgs> context)
        {
            var eventArgs = context.Item2;
            if (eventArgs.EventType != EventType.MouseOver && 
                eventArgs.EventType != EventType.Keyboard)
                return false;

            if (!(eventArgs.EventSource is Button) && !(eventArgs.EventSource is Keys))
                return false;

            if (eventArgs.EventSource is Keys key && (key != Keys.Up && key != Keys.Down))
                return false;

            return true;
        }

        private void SelectButton(Scene parent, UserInputEventArgs eventArgs)
        {
            if (eventArgs.EventSource is Button button)
                SelectedButtonId = _buttonIds.FindIndex(s => s.Equals(button.Id));
            else if (eventArgs.EventSource is Keys key)
                if (key == Keys.Up && SelectedButtonId > 0)
                    SelectedButtonId--;
                else if (key == Keys.Up && SelectedButtonId == 0)
                    SelectedButtonId = 3;
                else if (key == Keys.Down && SelectedButtonId < 3)
                    SelectedButtonId++;
                else if (key == Keys.Down && SelectedButtonId == 3)
                    SelectedButtonId = 0;
            else
                throw new ArgumentException("EventSource must be typeof Keys or Button");
        }

        private bool CanSelectNewGame(Tuple<Scene, UserInputEventArgs> context)
        {
            if (!_buttonIds[SelectedButtonId].Equals("new_game_button"))
                return false;

            if (context.Item2.EventType != EventType.Keyboard &&
                context.Item2.EventType != EventType.LeftClick)
                return false;

            if (context.Item2.EventSource is Keys key && key != Keys.Enter)
                return false;

            return true;
        }

        private void SelectNewGame(Scene parent, UserInputEventArgs eventArgs)
        {
            Controller.CurrentScene = Controller.GameScene;
        }

        private bool CanExitGame(Tuple<Scene, UserInputEventArgs> context)
        {
            if (!_buttonIds[SelectedButtonId].Equals("exit_button"))
                return false;

            if (context.Item2.EventType != EventType.Keyboard && 
                context.Item2.EventType != EventType.LeftClick)
                return false;

            if(context.Item2.EventSource is Keys key && key != Keys.Enter)
                return false;

            return true;
        }

        private void ExitGame(Scene parent, UserInputEventArgs eventArgs)
        {
            Game.Exit();
        }

        protected override void LoadContent()
        {
            _fontArialBold = ParamitaController.ArialBold;
            _background = _content.Load<Texture2D>("Images\\Scenes\\menuscreen");
            _buttonTexture = _content.Load<Texture2D>("Images\\Scenes\\wooden-button");
        }
        #endregion

        #region Public API
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
        #endregion
    }
}
