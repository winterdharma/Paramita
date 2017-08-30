using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.GameLogic;
using Paramita.UI.Input;
using System;

namespace Paramita.UI.Base.Game
{
    public class StatusPanel : Component
    {
        private int maxMessages;
        private string[] messages;
        private Point[] lineOrigins;
        private Color[] lineColors;
        private TimeSpan[] lineTimes;
        private TimeSpan lineDuration = new TimeSpan(0, 0, 15);
        private Color lineColor = new Color(Color.White, 1.0f);
        private Point origin;
        private SpriteFont font;
        private int areaHeight;



        public StatusPanel(InputResponder input, SpriteFont font, int maxMsgs, Point origin) : base(input)
        {
            maxMessages = maxMsgs;

            messages = new string[maxMessages];
            lineOrigins = new Point[maxMessages];
            lineColors = new Color[maxMessages];
            lineTimes = new TimeSpan[maxMessages];

            messages[0] = "Welcome back to Paramita. Come to try to get to the Other Side again?";
            lineTimes[0] = lineDuration;
            lineColors[0] = lineColor;

            this.origin = origin;
            this.font = font;

            CalcHeightOfMessageArea();
            lineOrigins = SetOriginsOfMessages();

            Dungeon.OnStatusMsgUINotification += HandleNewStatusMessage;
        }


        private void HandleNewStatusMessage(object sender, StatusMessageEventArgs e)
        {
            if (e.Message.Count == 0)
                return;

            if (e.Message.Count == 1)
            {
                AddMessage(e.Message[0]);
            }
            else
            {
                foreach(var msg in e.Message)
                {
                    AddMessage(msg);
                }
            }

        }

        // Sets the anchor points for drawing each message in the status area
        private Point[] SetOriginsOfMessages()
        {
            Vector2 size = font.MeasureString(messages[0]);
            int fontHeight = (int)size.Y;
            Point[] origins = new Point[messages.Length];
            int originsY = origin.Y - 30;
            origins[0] = new Point(10, originsY);
            for(int x = 1; x < messages.Length; x++)
            {
                originsY -= fontHeight + 5;
                origins[x] = new Point(10, originsY);
            }
            return origins;
        }


        // Calculates the Status Area's height in pixels given the font
        // and number of lines that can be displayed at one time.
        private void CalcHeightOfMessageArea()
        {
            Vector2 size = font.MeasureString(messages[0]);
            areaHeight = (int)(origin.Y + (maxMessages * size.Y) + 45);

        }


        // Adds a message to the Status area and initializes its duration and color
        private void AddMessage(string newMsg)
        {
            for(int x = messages.Length - 1; x > 0; x--)
            {
                messages[x] = messages[x - 1];
                lineTimes[x] = lineTimes[x - 1];
                lineColors[x] = lineColors[x - 1];
            }
            messages[0] = newMsg;
            lineTimes[0] = lineDuration;
            lineColors[0] = lineColor;
        }


        // Updates a message's color so that it gradually fades out
        // as its duration approaches zero. 
        // The fade-out effect is 2.5 seconds long
        public Color SetLineColor(TimeSpan timeLeft)
        {
            Color newColor = lineColor;
            if(timeLeft.TotalMilliseconds <= 2500f)
            {
                float alpha = (float)timeLeft.TotalMilliseconds / 2500.0f;
                newColor = new Color(1f,1f,1f) * alpha;
            }
            return newColor;
        }


        // Updates the messages elapsed time and colors
        public void Update(GameTime gameTime)
        {
            for(int x = 0; x < messages.Length; x++)
            {
                if(lineTimes[x] > TimeSpan.Zero)
                {
                    lineTimes[x] -= gameTime.ElapsedGameTime;
                    lineColors[x] = SetLineColor(lineTimes[x]);
                    if(lineTimes[x] <= TimeSpan.Zero)
                    {
                        lineTimes[x] = TimeSpan.Zero;
                        messages[x] = null;
                    }
                }
            }
        }


        // Draws the messages to the GameScene
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            for(int x = 0; x < messages.Length; x++)
            {
                if(messages[x] != null)
                {
                    spriteBatch.DrawString(font,
                    messages[x],
                    new Vector2(lineOrigins[x].X, lineOrigins[x].Y),
                    lineColors[x]);
                }
            }
            spriteBatch.End();
        }
    }
}
