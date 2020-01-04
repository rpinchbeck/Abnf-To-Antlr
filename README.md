# Abnf-To-Antlr

This translator will convert any ABNF grammar into an ANTLR grammar.

The resulting ANTLR grammar should be syntactically correct; however, some ABNF grammars are inherently ambiguous and ANTLR may complain about them until the ambiguity is resolved.

The translator makes the following corrections automatically:

1. Dashes in ABNF rule names are replaced with underscores.
2. ABNF rule names which are also ANTLR keywords are given a numeric suffix (e.g., fragment_1).
3. Duplicate rule names are given a numeric suffix to make them unique (e.g., rule_1, rule_2)
4. All rule names are converted to lowercase (parser rules).
5. By default, all character literals are converted into lexer rules (resulting in a scannerless parser).

The provided translator binary can also be compiled from source code using Visual Studio or a compatible product like SharpDevelop.

Happy translating!
