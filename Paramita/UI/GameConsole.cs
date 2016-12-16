using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MonoGame;
using Microsoft.Xna.Framework;

namespace Paramita.UI
{
    public class GameConsole
    {
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        internal Panel[,] panels;
        internal int[,] zBuffer;

        public GameConsole(int width, int height)
        {
            Resize(width, height);
        }

        /// <summary>
        /// Resizes the console.
        /// </summary>
        /// <param name="width">The new width of the console, in cells.</param>
        /// <param name="height">The new height of the console, in cells.</param>
        public void Resize(int width, int height)
        {
            if (width <= 0)
                throw new ArgumentOutOfRangeException("Width must be greater than zero.");
            if (height <= 0)
                throw new ArgumentOutOfRangeException("Height must be greater than zero.");

            Width = width;
            Height = height;
            this.panels = new Panel[width, height];
            this.zBuffer = new int[width, height];
        }

        /// <summary>
        /// Clears the console.
        /// </summary>
        public void Clear()
        {
            for (int iy = 0; iy < Height; iy++)
                for (int ix = 0; ix < Width; ix++)
                {
                    panels[ix, iy].backColor = Color.Black;
                    panels[ix, iy].color = Color.White;
                    panels[ix, iy].character = 0;
                    zBuffer[ix, iy] = int.MinValue;
                }
        }

        /// <summary>
        /// Clears the console.
        /// </summary>
        /// <param name="character">Character to set.</param>
        /// <param name="backColor">Background color to set.</param>
        /// <param name="color">Color to set.</param>
        public void Clear(int character, Color backColor, Color color)
        {
            Clear(character, backColor, color, int.MinValue);
        }

        /// <summary>
        /// Clears the console.
        /// </summary>
        /// <param name="character">Character to set.</param>
        /// <param name="backColor">Background color to set.</param>
        /// <param name="color">Color to set.</param>
        /// <param name="z">Z value to set.</param>
        public void Clear(int character, Color backColor, Color color, int z)
        {
            for (int iy = 0; iy < Height; iy++)
                for (int ix = 0; ix < Width; ix++)
                {
                    panels[ix, iy].backColor = backColor;
                    panels[ix, iy].color = color;
                    panels[ix, iy].character = character;
                    zBuffer[ix, iy] = z;
                }
        }

        /// <summary>
        /// Sets the character at the specified location.
        /// </summary>
        /// <param name="x">X position to set.</param>
        /// <param name="y">Y position to set.</param>
        /// <param name="character">Character to set.</param>
        public void SetChar(int x, int y, int character)
        {
            if (x >= 0 && y >= 0 && x < Width && y < Height)
            {
                panels[x, y].character = character;
            }
        }

        /// <summary>
        /// Sets the character at the specified location.
        /// </summary>
        /// <param name="x">X position to set.</param>
        /// <param name="y">Y position to set.</param>
        /// <param name="character">Character to set.</param>
        /// <param name="z">Z buffer value.</param>
        public void SetChar(int x, int y, int character, int z)
        {
            if (x >= 0 && y >= 0 && x < Width && y < Height && zBuffer[x, y] <= z)
            {
                zBuffer[x, y] = z;
                panels[x, y].character = character;
            }
        }

        /// <summary>
        /// Sets the character in the specified rectangle.
        /// </summary>
        /// <param name="x">X position to set.</param>
        /// <param name="y">Y position to set.</param>
        /// <param name="width">Width of the rectangle.</param>
        /// <param name="height">Height of the rectangle.</param>
        /// <param name="character">Character to set.</param>
        public void SetChar(int x, int y, int width, int height, int character)
        {
            if (width > 0 && height > 0)
            {
                for (int iy = y; iy < height + y; iy++)
                    for (int ix = x; ix < width + x; ix++)
                    {
                        SetChar(ix, iy, character);
                    }
            }
        }

        /// <summary>
        /// Sets the character in the specified rectangle.
        /// </summary>
        /// <param name="x">X position to set.</param>
        /// <param name="y">Y position to set.</param>
        /// <param name="width">Width of the rectangle.</param>
        /// <param name="height">Height of the rectangle.</param>
        /// <param name="character">Character to set.</param>
        /// <param name="z">Z buffer value.</param>
        public void SetChar(int x, int y, int width, int height, int character, int z)
        {
            if (width > 0 && height > 0)
            {
                for (int iy = y; iy < height + y; iy++)
                    for (int ix = x; ix < width + x; ix++)
                    {
                        SetChar(ix, iy, character, z);
                    }
            }
        }

        /// <summary>
        /// Sets the background color at the specified location.
        /// </summary>
        /// <param name="x">X position to set.</param>
        /// <param name="y">Y position to set.</param>
        /// <param name="color">Color to set.</param>
        public void SetBackColor(int x, int y, Color color)
        {
            if (x >= 0 && y >= 0 && x < Width && y < Height)
            {
                panels[x, y].backColor = color;
            }
        }

        /// <summary>
        /// Sets the background color at the specified location.
        /// </summary>
        /// <param name="x">X position to set.</param>
        /// <param name="y">Y position to set.</param>
        /// <param name="color">Color to set.</param>
        /// <param name="z">Z buffer value.</param>
        public void SetBackColor(int x, int y, Color color, int z)
        {
            if (x >= 0 && y >= 0 && x < Width && y < Height && zBuffer[x, y] <= z)
            {
                panels[x, y].backColor = color;
                zBuffer[x, y] = z;
            }
        }

        /// <summary>
        /// Sets the background color in the specified rectangle.
        /// </summary>
        /// <param name="x">X position to set.</param>
        /// <param name="y">Y position to set.</param>
        /// <param name="width">Width of the rectangle.</param>
        /// <param name="height">Height of the rectangle.</param>
        /// <param name="color">Color to set.</param>
        public void SetBackColor(int x, int y, int width, int height, Color color)
        {
            if (width > 0 && height > 0)
            {
                for (int iy = y; iy < height + y; iy++)
                    for (int ix = x; ix < width + x; ix++)
                    {
                        SetBackColor(ix, iy, color);
                    }
            }
        }

        /// <summary>
        /// Sets the background color in the specified rectangle.
        /// </summary>
        /// <param name="x">X position to set.</param>
        /// <param name="y">Y position to set.</param>
        /// <param name="width">Width of the rectangle.</param>
        /// <param name="height">Height of the rectangle.</param>
        /// <param name="color">Color to set.</param>
        /// <param name="z">Z buffer value.</param>
        public void SetBackColor(int x, int y, int width, int height, Color color, int z)
        {
            if (width > 0 && height > 0)
            {
                for (int iy = y; iy < height + y; iy++)
                    for (int ix = x; ix < width + x; ix++)
                    {
                        SetBackColor(ix, iy, color, z);
                    }
            }
        }

        /// <summary>
        /// Sets the color at the specified location.
        /// </summary>
        /// <param name="x">X position to set.</param>
        /// <param name="y">Y position to set.</param>
        /// <param name="color">Color to set.</param>
        public void SetColor(int x, int y, Color color)
        {
            if (x >= 0 && y >= 0 && x < Width && y < Height)
                panels[x, y].color = color;
        }

        /// <summary>
        /// Sets the color at the specified location.
        /// </summary>
        /// <param name="x">X position to set.</param>
        /// <param name="y">Y position to set.</param>
        /// <param name="color">Color to set.</param>
        public void SetColor(int x, int y, Color color, int z)
        {
            if (x >= 0 && y >= 0 && x < Width && y < Height && zBuffer[x, y] <= z)
            {
                zBuffer[x, y] = z;
                panels[x, y].color = color;
            }
        }

        /// <summary>
        /// Sets the color in the specified rectangle.
        /// </summary>
        /// <param name="x">X position to set.</param>
        /// <param name="y">Y position to set.</param>
        /// <param name="width">Width of the rectangle.</param>
        /// <param name="height">Height of the rectangle.</param>
        /// <param name="color">Color to set.</param>
        public void SetColor(int x, int y, int width, int height, Color color)
        {
            if (width > 0 && height > 0)
            {
                for (int iy = y; iy < height + y; iy++)
                    for (int ix = x; ix < width + x; ix++)
                    {
                        SetColor(ix, iy, color);
                    }
            }
        }

        /// <summary>
        /// Sets the color in the specified rectangle.
        /// </summary>
        /// <param name="x">X position to set.</param>
        /// <param name="y">Y position to set.</param>
        /// <param name="width">Width of the rectangle.</param>
        /// <param name="height">Height of the rectangle.</param>
        /// <param name="color">Color to set.</param>
        /// <param name="z">Z buffer value.</param>
        public void SetColor(int x, int y, int width, int height, Color color, int z)
        {
            if (width > 0 && height > 0)
            {
                for (int iy = y; iy < height + y; iy++)
                    for (int ix = x; ix < width + x; ix++)
                    {
                        SetColor(ix, iy, color, z);
                    }
            }
        }

        /// <summary>
        /// Sets the color, background color, and character at the specified location.
        /// </summary>
        /// <param name="x">X position to set.</param>
        /// <param name="y">Y position to set.</param>
        /// <param name="color">Color to set.</param>
        /// <param name="backColor">Background color to set.</param>
        /// <param name="character">Character to set.</param>
        public void Set(int x, int y, Color? color, Color? backColor, int? character)
        {
            if (x >= 0 && y >= 0 && x < Width && y < Height)
            {
                if (color.HasValue) panels[x, y].color = color.Value;
                if (backColor.HasValue) panels[x, y].backColor = backColor.Value;
                if (character.HasValue) panels[x, y].character = character.Value;
            }
        }

        /// <summary>
        /// Sets the color, background color, and character at the specified location.
        /// </summary>
        /// <param name="x">X position to set.</param>
        /// <param name="y">Y position to set.</param>
        /// <param name="color">Color to set.</param>
        /// <param name="backColor">Background color to set.</param>
        /// <param name="character">Character to set.</param>
        /// <param name="z">Z buffer value.</param>
        public void Set(int x, int y, Color? color, Color? backColor, int? character, int z)
        {
            if (x >= 0 && y >= 0 && x < Width && y < Height && zBuffer[x, y] <= z)
            {
                zBuffer[x, y] = z;
                if (color.HasValue) panels[x, y].color = color.Value;
                if (backColor.HasValue) panels[x, y].backColor = backColor.Value;
                if (character.HasValue) panels[x, y].character = character.Value;
            }
        }

        /// <summary>
        /// Sets the color, background color, and character in the specified rectangle.
        /// </summary>
        /// <param name="x">X position to set.</param>
        /// <param name="y">Y position to set.</param>
        /// <param name="width">Width of the rectangle.</param>
        /// <param name="height">Height of the rectangle.</param>
        /// <param name="color">Color to set.</param>
        public void Set(int x, int y, int width, int height, Color? color, Color? backColor, int? character)
        {
            if (width > 0 && height > 0)
            {
                for (int iy = y; iy < height + y; iy++)
                    for (int ix = x; ix < width + x; ix++)
                    {
                        Set(ix, iy, color, backColor, character);
                    }
            }
        }

        /// <summary>
        /// Sets the color, background color, and character in the specified rectangle.
        /// </summary>
        /// <param name="x">X position to set.</param>
        /// <param name="y">Y position to set.</param>
        /// <param name="width">Width of the rectangle.</param>
        /// <param name="height">Height of the rectangle.</param>
        /// <param name="color">Color to set.</param>
        /// <param name="z">Z buffer value.</param>
        public void Set(int x, int y, int width, int height, Color? color, Color? backColor, int? character, int z)
        {
            if (width > 0 && height > 0)
            {
                for (int iy = y; iy < height + y; iy++)
                    for (int ix = x; ix < width + x; ix++)
                    {
                        Set(ix, iy, color, backColor, character, z);
                    }
            }
        }

        /// <summary>
        /// Sets color, background color, and character at the specified location.
        /// Ignores the Z buffer, if you want to check the Z buffer before setting the cell, use SetZCheck
        /// </summary>
        /// <param name="x">X position to set.</param>
        /// <param name="y">Y position to set.</param>
        /// <param name="cell">Cell to set.</param>
        public void Set(int x, int y, Panel panel)
        {
            if (x >= 0 && y >= 0 && x < Width && y < Height)
            {
                panels[x, y] = panel;
            }
        }

        /// <summary>
        /// Sets color, background color, and character at the specified location.
        /// </summary>
        /// <param name="x">X position to set.</param>
        /// <param name="y">Y position to set.</param>
        /// <param name="cell">Cell to set.</param>
        /// <param name="z">Z buffer value.</param>
        public void Set(int x, int y, Panel panel, int z)
        {
            if (x >= 0 && y >= 0 && x < Width && y < Height && zBuffer[x, y] <= z)
            {
                zBuffer[x, y] = z;
                panels[x, y] = panel;
            }
        }

        /// <summary>
        /// Sets the color, background color, and character in the specified rectangle.
        /// Ignores the Z buffer, if you want to check the Z buffer before setting the cell, use SetZCheck
        /// </summary>
        /// <param name="x">X position to set.</param>
        /// <param name="y">Y position to set.</param>
        /// <param name="width">Width of the rectangle.</param>
        /// <param name="height">Height of the rectangle.</param>
        /// <param name="color">Color to set.</param>
        public void Set(int x, int y, int width, int height, Panel panel)
        {
            if (width > 0 && height > 0)
            {
                for (int iy = y; iy < height + y; iy++)
                    for (int ix = x; ix < width + x; ix++)
                    {
                        Set(ix, iy, panel);
                    }
            }
        }

        /// <summary>
        /// Sets the color, background color, and character in the specified rectangle.
        /// </summary>
        /// <param name="x">X position to set.</param>
        /// <param name="y">Y position to set.</param>
        /// <param name="width">Width of the rectangle.</param>
        /// <param name="height">Height of the rectangle.</param>
        /// <param name="color">Color to set.</param>
        /// <param name="z">Z buffer value.</param>
        public void Set(int x, int y, int width, int height, Panel panel, int z)
        {
            if (width > 0 && height > 0)
            {
                for (int iy = y; iy < height + y; iy++)
                    for (int ix = x; ix < width + x; ix++)
                    {
                        Set(ix, iy, panel, z);
                    }
            }
        }

        /// <summary>
        /// Prints the text to the console.
        /// </summary>
        /// <param name="x">Starting X position to print.</param>
        /// <param name="y">Starting Y position to print.</param>
        /// <param name="text">Text to be printed.</param>
        /// <param name="color">Color to be printed.</param>
        /// <param name="backColor">BackColor to be printed.</param>
        public void Print(int x, int y, string text, Color color, Color? backColor = null)
        {
            if (text == null) return;
            for (int i = 0; i < text.Length; i++)
            {
                SetColor(x + i, y, color);
                SetChar(x + i, y, text[i]);
                if (backColor.HasValue) SetBackColor(x + i, y, backColor.Value);
            }
        }

        /// <summary>
        /// Prints the text to the console.
        /// </summary>
        /// <param name="x">Starting X position to print.</param>
        /// <param name="y">Starting Y position to print.</param>
        /// <param name="text">Text to be printed.</param>
        /// <param name="color">Color to be printed.</param>
        /// <param name="z">Z buffer value.</param>
        /// <param name="backColor">BackColor to be printed.</param>
        public void Print(int x, int y, string text, Color color, int z, Color? backColor = null)
        {
            if (text == null) return;
            for (int i = 0; i < text.Length; i++)
            {
                SetColor(x + i, y, color, z);
                SetChar(x + i, y, text[i], z);
                if (backColor.HasValue) SetBackColor(x + i, y, backColor.Value, z);
            }
        }

        /// <summary>
        /// Prints the text to the console.
        /// </summary>
        /// <param name="x">Starting X position to print.</param>
        /// <param name="y">Starting Y position to print.</param>
        /// <param name="text">Text to be printed.</param>
        /// <param name="color">Color to be printed.</param>
        /// <param name="backColor">BackColor to be printed.</param>
        /// <param name="wrap">Characters to print before wrapping to the next line.</param>
        /// <returns>Number of lines printed.</returns>
        public int Print(int x, int y, string text, Color color, Color? backColor, int wrap)
        {
            return Print(x, y, -1, text, color, backColor, wrap);
        }

        /// <summary>
        /// Prints the text to the console.
        /// </summary>
        /// <param name="x">Starting X position to print.</param>
        /// <param name="y">Starting Y position to print.</param>
        /// <param name="text">Text to be printed.</param>
        /// <param name="color">Color to be printed.</param>
        /// <param name="backColor">BackColor to be printed.</param>
        /// <param name="wrap">Characters to print before wrapping to the next line.</param>
        /// <param name="z">Z buffer value.</param>
        /// <returns>Number of lines printed.</returns>
        public int Print(int x, int y, string text, Color color, Color? backColor, int wrap, int z)
        {
            return Print(x, y, -1, text, color, backColor, wrap, z);
        }

        /// <summary>
        /// Prints the text to the console.
        /// </summary>
        /// <param name="x">Starting X position to print.</param>
        /// <param name="y">Starting Y position to print.</param>
        /// <param name="maxLines">Maximum number of lines, -1 for no limit.</param>
        /// <param name="text">Text to be printed.</param>
        /// <param name="color">Color to be printed.</param>
        /// <param name="backColor">BackColor to be printed.</param>
        /// <param name="wrap">Characters to print before wrapping to the next line.</param>
        /// <returns>Number of lines printed.</returns>
        public int Print(int x, int y, int maxLines, string text, Color color, Color? backColor, int wrap)
        {
            if (text == null) return 0;
            StringBuilder sb = new StringBuilder(wrap);
            string[] words = text.Split(' ');
            int i = 0;
            int lines = 0;

            while (i < words.Length && (maxLines == -1 || lines <= maxLines))
            {
                while (i < words.Length && sb.Length + words[i].Length + 1 < wrap)
                {
                    sb.Append(words[i++] + " ");
                }

                string line = sb.ToString();
                sb.Clear();

                for (int j = 0; j < line.Length; j++)
                {
                    SetColor(x + j, y + lines, color);
                    SetChar(x + j, y + lines, line[j]);
                    if (backColor.HasValue) SetBackColor(x + i, y, backColor.Value);
                }
                lines++;
            }
            return lines;
        }

        /// </summary>
        /// <param name="x">Starting X position to print.</param>
        /// <param name="y">Starting Y position to print.</param>
        /// <param name="maxLines">Maximum number of lines, -1 for no limit.</param>
        /// <param name="text">Text to be printed.</param>
        /// <param name="color">Color to be printed.</param>
        /// <param name="backColor">BackColor to be printed.</param>
        /// <param name="wrap">Characters to print before wrapping to the next line.</param>
        /// <param name="z">Z buffer value.</param>
        /// <returns>Number of lines printed.</returns>
        public int Print(int x, int y, int maxLines, string text, Color color, Color? backColor, int wrap, int z)
        {
            if (text == null) return 0;
            StringBuilder sb = new StringBuilder(wrap);
            string[] words = text.Split(' ');
            int i = 0;
            int lines = 0;

            while (i < words.Length && (maxLines == -1 || lines <= maxLines))
            {
                while (i < words.Length && sb.Length + words[i].Length + 1 < wrap)
                {
                    sb.Append(words[i++] + " ");
                }

                string line = sb.ToString();
                sb.Clear();

                for (int j = 0; j < line.Length; j++)
                {
                    SetColor(x + j, y + lines, color, z);
                    SetChar(x + j, y + lines, line[j], z);
                    if (backColor.HasValue) SetBackColor(x + i, y, backColor.Value, z);
                }
                lines++;
            }
            return lines;
        }

        /// <summary>
        /// Copies contents of one console to another
        /// </summary>
        /// <param name="srcConsole">Source console to copy from.</param>
        /// <param name="srcX">Starting X position of the copy.</param>
        /// <param name="srcY">Starting Y position of the copy.</param>
        /// <param name="srcWidth">Width of the copy.</param>
        /// <param name="srcHeight">Height of the copy.</param>
        /// <param name="destConsole">Destination console to copy to.</param>
        /// <param name="destX">Desitination X position to copy to.</param>
        /// <param name="destY">Desitination Y position to copy to.</param>
        public static void Blit(GameConsole srcConsole, int srcX, int srcY, int srcWidth, int srcHeight, GameConsole destConsole, int destX, int destY)
        {
            if (srcConsole == null)
                throw new ArgumentNullException("srcConsole");
            if (destConsole == null)
                throw new ArgumentNullException("destConsole");
            if (srcWidth <= 0)
                throw new ArgumentOutOfRangeException("srcWidth must be greater than zero.");
            if (srcHeight <= 0)
                throw new ArgumentOutOfRangeException("srcHeight must be greater than zero.");

            for (int iy = 0; iy < srcHeight; iy++)
            {
                for (int ix = 0; ix < srcWidth; ix++)
                {
                    if (ix + destX >= 0 && iy + destY >= 0 && ix + destX < destConsole.Width && iy + destY < destConsole.Height
                        && ix + srcX >= 0 && iy + srcY >= 0 && ix + srcX < srcConsole.Width && iy + srcY < srcConsole.Height)
                        destConsole.panels[ix + destX, iy + destY] = srcConsole.panels[ix + srcX, iy + srcY];
                }
            }
        }
    }
}