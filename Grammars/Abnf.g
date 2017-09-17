/*

    Copyright 2012-2013 Robert Pinchbeck
  
    This file is part of AbnfToAntlr.

    AbnfToAntlr is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    AbnfToAntlr is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with AbnfToAntlr.  If not, see <http://www.gnu.org/licenses/>.
  
// --------------------------------------------------------------------------------
// ANTLR grammar for parsing ABNF grammars
//
// Based on RFC 5234 (Augmented BNF for Syntax Specifications)...
// http://tools.ietf.org/html/rfc5234
// --------------------------------------------------------------------------------
*/

grammar Abnf;

options
{
	language=CSharp3;
}

tokens
{
	RULE_LIST_NODE;
	RULE_NODE;
	RULE_NAME_NODE;

	ALTERNATION_NODE;
	CONCATENATION_NODE;
	REPETITION_NODE;
	NUMBER_NODE;
	GROUP_NODE;
	OPTION_NODE;

	REPEAT_NODE;
	ONE_OCCURENCE;
	ZERO_OCCURENCES;
	ORMORE_OCCURENCES;
	EXACT_OCCURENCES;

	CHAR_VAL_NODE;
	BIN_VAL_NODE;
	DEC_VAL_NODE;
	HEX_VAL_NODE;
	PROSE_VAL_NODE;

	BIN_VAL_RANGE_NODE;
	BIN_VAL_CONCAT_NODE;
	BIN_VAL_NUMBER_NODE;

	DEC_VAL_RANGE_NODE;
	DEC_VAL_CONCAT_NODE;
	DEC_VAL_NUMBER_NODE;

	HEX_VAL_RANGE_NODE;
	HEX_VAL_CONCAT_NODE;
	HEX_VAL_NUMBER_NODE;

}

public start
	:
		rulelist
		;

rulelist
	:
		( rule | (c_wsp* (c_nl)=>c_nl) )+
		;

rule
	:
		rulename defined_as elements c_nl
			// continues if next line starts
			//  with white space
		;

rulename
	:
		rulechars
		;

rulechars
	:
		( HEX_ALPHA | OTHER_ALPHA ) ( HEX_ALPHA | OTHER_ALPHA | ZERO | ONE | OTHER_DIGIT | DASH )*
		;

defined_as
	:
		c_wsp* ( '=' | '=/' ) c_wsp*
		// basic rules definition and
		//  incremental alternatives
		;

elements
	:
		alternation ((c_wsp)=>c_wsp)*
		;

c_wsp
	:
		WSP | ( c_nl WSP )
		;

c_nl
	:
		comment | CRLF
	    // comment or newline
		;

comment
	:
		COMMENT
		;

alternation
	:
		concatenation ( c_wsp* '/' c_wsp* concatenation )*
		;

concatenation
	:
		repetition ( c_wsp+ repetition )*
		;

repetition
	:
		ASTERISK number element
		| min=number ASTERISK max=number element
		| number ASTERISK element
		| ASTERISK element
		| number element
		| element
		;

number
	:
		number_val
		;

number_val
	:
		( ZERO | ONE | OTHER_DIGIT )+
		// this extra rule allows easier rewriting in the number rule during AST contruction
		;

element
	:
		rulename
		| group
		| option
		| char_val
		| num_val
		| prose_val
		;

group
	:
		'(' c_wsp* alternation c_wsp* ')'
		;

option
	:
		'[' c_wsp* alternation c_wsp* ']'
		;

num_val
	:
		( bin_val | dec_val | hex_val )
		;

char_val
	:
		CHAR_VAL
        // quoted string of SP and VCHAR
        //  without DQUOTE
		;

bin_val
	:
		BIN_VAL_PREFIX min=bin_val_number DASH max=bin_val_number
		| BIN_VAL_PREFIX bin_val_number ('.' bin_val_number)+
		| BIN_VAL_PREFIX bin_val_number
        // series of concatenated bit values
        //  or single ONEOF range
		;

bin_val_number
	:
		bin_number
		;

bin_number
	:
		( ZERO | ONE )+
		// this extra rule allows easier rewriting in the bin_val_number rule during AST contruction
		;

dec_val
	:
		DEC_VAL_PREFIX min=dec_val_number DASH max=dec_val_number
		| DEC_VAL_PREFIX dec_val_number ('.' dec_val_number)+
		| DEC_VAL_PREFIX dec_val_number
		;

dec_val_number
	:
		dec_number
		;

dec_number
	:
		(ZERO | ONE | OTHER_DIGIT)+
		// this extra rule allows easier rewriting in the dec_val_number rule during AST contruction
		;

hex_val
	:
		HEX_VAL_PREFIX min=hex_val_number DASH max=hex_val_number
		| HEX_VAL_PREFIX hex_val_number ('.' hex_val_number)+
		| HEX_VAL_PREFIX hex_val_number
		;

hex_val_number
	:
		hex_number
		;

hex_number
	:
		(ZERO | ONE | OTHER_DIGIT | HEX_ALPHA)+
		// this extra rule allows easier rewriting in the hex_val_number rule during AST contruction
		;

prose_val
	:
		PROSE_VAL
		// bracketed string of SP and VCHAR
		//  without angles
		// prose description, to be used as
		//  last resort
		;

COMMENT
	:
		';' ( WSP | VCHAR )* CRLF
		;

CHAR_VAL
	:
		DQUOTE ( '\u0020'..'\u0021' | '\u0023'..'\u007E' )* DQUOTE
		;

BIN_VAL_PREFIX
	:
		'%b'
	 	;

DEC_VAL_PREFIX
	:
		'%d'
	 	;

HEX_VAL_PREFIX
	:
		'%x'
	 	;

PROSE_VAL
	:
		'<' ( '\u0020'..'\u003D' | '\u003F'..'\u007E' )* '>'
		;

HEX_ALPHA
	:
		'A'..'F'
	;

OTHER_ALPHA
	:
		'G'..'Z' | 'a'..'z'
		;

ASTERISK
	:
		'*'
		;

DASH
	:
		'-'
		;

fragment CR
	:
		'\u000D'
			// carriage return
		;

CRLF
	:
		CR LF
			// Internet standard newline
		;

ZERO
	:
		'0'
		;

ONE
	:
		'1'
		;

OTHER_DIGIT
	:
		'2'..'9'
		;

fragment DQUOTE
	:
		'\u0022'
			// " (double quote)
		;

fragment HTAB
	:
		'\u0009'
		// horizontal tab
		;

fragment LF
	:
		'\u000A'
		// linefeed
		;

fragment SP
	:
		'\u0020'
		// space
		;

fragment VCHAR
	:
		'\u0021'..'\u007E'
		// visible (printing) characters
		;

WSP
	:
		(SP | HTAB)+
		// white space
		;
