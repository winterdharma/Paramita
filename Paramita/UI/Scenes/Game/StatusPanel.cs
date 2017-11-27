using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameUI.Base;
using MonoGameUI.Elements;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Paramita.UI.Scenes
{
    public class StatusPanel : Component
    {
        #region Fields
        private const int MAX_MESSAGES = 10;
        private const string INTRO_MESSAGE = "Welcome back to Paramita. Come to try to get to the Other Side again?";
        private TimeSpan[] _lineTimes;
        private TimeSpan _lineDuration = new TimeSpan(0, 0, 15);
        private Color _lineColor = new Color(Color.White, 1.0f);
        private readonly Point ORIGIN = new Point(0, 720);
        private readonly SpriteFont FONT = ParamitaController.ArialBold;
        private int areaHeight;
        #endregion

        #region Constructors
        public StatusPanel(Scene parent, int drawOrder = 1) 
            : base(parent, drawOrder)
        {
            _lineTimes = new TimeSpan[MAX_MESSAGES];
            _lineTimes[0] = _lineDuration;

            CalcHeightOfMessageArea();
        }
        #endregion

        #region Properties
        #endregion

        #region Initialization
        protected override Dictionary<string, Element> InitializeElements()
        {
            var elements = new Dictionary<string, Element>();

            // 10 should actually be a parameter from the constructor maxMessages
            for(int i = 0; i < MAX_MESSAGES; i++)
            {
                string id = "line " + i;
                elements[id] = new LineOfText(id, this, GetLinePosition(i), "", FONT, Color.White, Color.White, 1);
            }

            var intro = (LineOfText)elements["line 0"];
            intro.Text = INTRO_MESSAGE;

            return elements;
        }

        protected override Rectangle UpdatePanelRectangle()
        {
            return new Rectangle();
        }
        #endregion

        #region Public API
        // Updates a message's color so that it gradually fades out
        // as its duration approaches zero. 
        // The fade-out effect is 2.5 seconds long
        public Color SetLineColor(TimeSpan timeLeft)
        {
            Color newColor = _lineColor;
            if(timeLeft.TotalMilliseconds <= 2500f)
            {
                float alpha = (float)timeLeft.TotalMilliseconds / 2500.0f;
                newColor = new Color(1f,1f,1f) * alpha;
            }
            return newColor;
        }

        // Updates the messages elapsed time and colors
        public override void Update(GameTime gameTime)
        {
            var sortedElements = Elements.OrderByDescending(element => element.Key);
            UpdateElements(gameTime, sortedElements);
        }

        // Draws the messages to the GameScene
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }

        // Adds a message to the Status area and initializes its duration and color
        public void AddMessage(string newMsg)
        {
            var sortedElements = Elements.OrderByDescending(element => element.Key);
            LineOfText lineOfText;
            foreach (KeyValuePair<string, Element> element in sortedElements)
            {
                string elementId = element.Key;
                int elementIndex = int.Parse(element.Key.Substring(5));
                lineOfText = (LineOfText)element.Value;
                LineOfText previousLine;

                if (elementIndex > 0)
                {
                    previousLine = (LineOfText)Elements["line " + (elementIndex - 1)];
                    lineOfText.Text = previousLine.Text;
                    lineOfText.Color = previousLine.Color;
                    _lineTimes[elementIndex] = _lineTimes[elementIndex - 1];
                }
            }

            lineOfText = (LineOfText)Elements["line 0"];
            lineOfText.Text = newMsg;
            lineOfText.Color = _lineColor;
            _lineTimes[0] = _lineDuration;
        }
        #endregion

        #region Helper Methods
        private Vector2 GetLinePosition(int index)
        {
            int fontHeight = (int)FONT.MeasureString("Test string").Y;
            int originY = ORIGIN.Y - 30;
            int offsetY = (fontHeight + 5) * index;

            int x = 10;
            int y = originY - offsetY;

            return new Vector2(x, y);
        }

        // Calculates the Status Area's height in pixels given the font
        // and number of lines that can be displayed at one time.
        private void CalcHeightOfMessageArea()
        {
            Vector2 size = FONT.MeasureString("Test string");
            areaHeight = (int)(ORIGIN.Y + (MAX_MESSAGES * size.Y) + 45);
        }

        private void UpdateElements(GameTime gameTime, 
            IOrderedEnumerable<KeyValuePair<string, Element>> sortedElements)
        {
            for (int x = 0; x < sortedElements.Count(); x++)
            {
                _lineTimes[x] -= gameTime.ElapsedGameTime;

                if (_lineTimes[x] <= TimeSpan.Zero)
                {
                    _lineTimes[x] = TimeSpan.Zero;
                    var line = (LineOfText)Elements["line " + x];
                    line.Text = "";
                }
                else
                {
                    Elements["line " + x].Color = SetLineColor(_lineTimes[x]);
                }
            }
        }
        #endregion
    }
}
