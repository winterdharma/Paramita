using Microsoft.Xna.Framework.Input;
using Paramita.UI.Base;
using System.Collections.Generic;

namespace Paramita.UI.Input
{
    public class InputSource
    {
        #region Fields
        private List<Element> _sourceElements = new List<Element>();
        private List<Keys> _sourceKeys = new List<Keys>();
        #endregion

        #region Constructors
        public InputSource(Keys key)
        {
            SourceKeys.Add(key);
        }

        public InputSource(Element element)
        {
            SourceElements.Add(element);
        }

        public InputSource(Element element, Keys key)
        {
            SourceElements.Add(element);
            SourceKeys.Add(key);
        }

        public InputSource(params Element[] elements)
        {
            SourceElements.AddRange(elements);
        }

        public InputSource(params Keys[] keys)
        {
            SourceKeys.AddRange(keys);
        }

        public InputSource(List<Element> elements, List<Keys> keys)
        {
            SourceElements.AddRange(elements);
            SourceKeys.AddRange(keys);
        }
        #endregion

        #region Properties
        public List<Element> SourceElements { get; set; }
        public List<Keys> SourceKeys { get; set; }
        #endregion

        #region Public API
        public bool Contains(Element element)
        {
            return SourceElements.Contains(element);
        }

        public bool Contains(Keys key)
        {
            return SourceKeys.Contains(key);
        }
        #endregion
    }
}
