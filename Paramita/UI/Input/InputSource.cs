using Microsoft.Xna.Framework.Input;
using Paramita.UI.Base;

namespace Paramita.UI.Input
{
    public class InputSource
    {
        public InputSource(Element element = null, Keys key = Keys.None)
        {
            SourceElement = element;
            SourceKey = key;
        }

        public Element SourceElement { get; set; }
        public Keys SourceKey { get; set; }
    }
}
