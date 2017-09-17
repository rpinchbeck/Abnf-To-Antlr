using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AbnfToAntlr.Common
{
    public interface INamedCharacterLookup
    {
        bool IsKnownCharacter(char character);
        NamedCharacter GetNamedCharacter(char character);
    }
}
