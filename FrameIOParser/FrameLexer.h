#pragma once

#include <FlexLexer.h>
#include "fparser.tab.h"

class FrameLexer :public yyFlexLexer
{
public:
	virtual int yylex(YYSTYPE* yylval, YYLTYPE* yylloc);
};

class ParserContext
{
public:
	FrameLexer * m_pLexer;
};
