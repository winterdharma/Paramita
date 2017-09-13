using Microsoft.Xna.Framework;

namespace Paramita.UI.Base
{
    public interface IDrawable
    {
        int DrawOrder { get; }
        bool IsMouseOver { get; }
        Rectangle Rectangle { get; }
    }
}
