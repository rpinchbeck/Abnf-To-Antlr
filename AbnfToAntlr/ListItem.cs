using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AbnfToAntlr
{
    public class ListItem
    {
        string _text;
        string _value;

        public string Text { get { return _text; } }
        public string Value { get { return _value; } }

        public ListItem(string text, string value)
        {
            _text = text;
            _value = value;
        }
    }
}
