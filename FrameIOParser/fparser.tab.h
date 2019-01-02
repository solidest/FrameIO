/* A Bison parser, made by GNU Bison 3.1.  */

/* Bison interface for Yacc-like parsers in C

   Copyright (C) 1984, 1989-1990, 2000-2015, 2018 Free Software Foundation, Inc.

   This program is free software: you can redistribute it and/or modify
   it under the terms of the GNU General Public License as published by
   the Free Software Foundation, either version 3 of the License, or
   (at your option) any later version.

   This program is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.

   You should have received a copy of the GNU General Public License
   along with this program.  If not, see <http://www.gnu.org/licenses/>.  */

/* As a special exception, you may create a larger work that contains
   part or all of the Bison parser skeleton and distribute that work
   under terms of your choice, so long as that work isn't itself a
   parser generator using the skeleton or a modified version thereof
   as a parser skeleton.  Alternatively, if you modify or redistribute
   the parser skeleton itself, you may (at your option) remove this
   special exception, which will cause the skeleton and the resulting
   Bison output files to be licensed under the GNU General Public
   License without this special exception.

   This special exception was added by the Free Software Foundation in
   version 2.2 of Bison.  */

#ifndef YY_YY_FPARSER_TAB_H_INCLUDED
# define YY_YY_FPARSER_TAB_H_INCLUDED
/* Debug traces.  */
#ifndef YYDEBUG
# define YYDEBUG 0
#endif
#if YYDEBUG
extern int yydebug;
#endif

/* Token type.  */
#ifndef YYTOKENTYPE
# define YYTOKENTYPE
  enum yytokentype
  {
    T_MATCH = 258,
    T_SUBSYS = 259,
    T_PROJECT = 260,
    T_SYSTEM = 261,
    T_FRAME = 262,
    T_CHANNEL = 263,
    T_ENUM = 264,
    T_ACTION = 265,
    T_THIS = 266,
    T_INTEGER = 267,
    T_REAL = 268,
    T_BLOCK = 269,
    T_TEXT = 270,
    T_BOOL = 271,
    T_BYTE = 272,
    T_SBYTE = 273,
    T_USHORT = 274,
    T_SHORT = 275,
    T_UINT = 276,
    T_INT = 277,
    T_ULONG = 278,
    T_LONG = 279,
    T_FLOAT = 280,
    T_DOUBLE = 281,
    T_STRING = 282,
    T_SEND = 283,
    T_ON = 284,
    T_RECV = 285,
    T_RECVLOOP = 286,
    T_COM = 287,
    T_CAN = 288,
    T_TCPSERVER = 289,
    T_TCPCLIENT = 290,
    T_UDP = 291,
    T_DIO = 292,
    T_SIGNED = 293,
    T_BITCOUNT = 294,
    T_VALUE = 295,
    T_REPEATED = 296,
    T_BYTEORDER = 297,
    T_ENCODED = 298,
    T_ISDOUBLE = 299,
    T_TAIL = 300,
    T_ALIGNEDLEN = 301,
    T_TYPE = 302,
    T_BYTESIZE = 303,
    T_BYTESIZEOF = 304,
    T_TOENUM = 305,
    T_ONEOF = 306,
    T_DEFAULT = 307,
    T_MAX = 308,
    T_MIN = 309,
    T_CHECK = 310,
    T_CHECKRANGE = 311,
    T_TRUE = 312,
    T_FALSE = 313,
    T_SMALL = 314,
    T_BIG = 315,
    T_PRIMITIVE = 316,
    T_INVERSION = 317,
    T_COMPLEMENT = 318,
    T_SUM8 = 319,
    T_XOR8 = 320,
    T_SUM16 = 321,
    T_SUM16_FALSE = 322,
    T_XOR16 = 323,
    T_XOR16_FALSE = 324,
    T_SUM32 = 325,
    T_SUM32_FALSE = 326,
    T_XOR32 = 327,
    T_XOR32_FALSE = 328,
    T_CRC4_ITU = 329,
    T_CRC5_EPC = 330,
    T_CRC5_ITU = 331,
    T_CRC5_USB = 332,
    T_CRC6_ITU = 333,
    T_CRC7_MMC = 334,
    T_CRC8 = 335,
    T_CRC8_ITU = 336,
    T_CRC8_ROHC = 337,
    T_CRC8_MAXIM = 338,
    T_CRC16_IBM = 339,
    T_CRC16_MAXIM = 340,
    T_CRC16_USB = 341,
    T_CRC16_MODBUS = 342,
    T_CRC16_CCITT = 343,
    T_CRC16_CCITT_FALSE = 344,
    T_CRC16_X25 = 345,
    T_CRC16_XMODEM = 346,
    T_CRC16_DNP = 347,
    T_CRC32 = 348,
    T_CRC32_MPEG_2 = 349,
    T_CRC64 = 350,
    T_CRC64_WE = 351,
    VALUE_STRING = 352,
    VALUE_INT = 353,
    VALUE_REAL = 354,
    T_ID = 355,
    T_NOTE = 356,
    T_UNION_ID = 357,
    T_AT_USER = 358
  };
#endif

/* Value type.  */
#if ! defined YYSTYPE && ! defined YYSTYPE_IS_DECLARED

union YYSTYPE
{
#line 30 "fparser.y" /* yacc.c:1913  */

	int symbol;
	int optionvalue;
	segpropertytype segproptype;
	segpropertyvaluetype segprovtype;
	segmenttype segtype;
	syschanneltype syschtype;
	actioniotype iotype;

	PROJECT * project;
	PROJECTITEM* pitem;
	PROJECTITEMLIST* pitemlist;

	SYSITEM* sysitem;
	SYSITEMLIST* sysitemlist;
	CHANNELOPTION* choplist;
	ACTIONMAP* amaplist;
	
	SEGMENT* seglist;
	SEGPROPERTY* segprolist;
	EXPVALUE* valueexp;
	ONEOFITEM* oneofitemlist;

	ENUMITEM * enumitemlist;
	NOTE* notelist;


#line 186 "fparser.tab.h" /* yacc.c:1913  */
};

typedef union YYSTYPE YYSTYPE;
# define YYSTYPE_IS_TRIVIAL 1
# define YYSTYPE_IS_DECLARED 1
#endif

/* Location type.  */
#if ! defined YYLTYPE && ! defined YYLTYPE_IS_DECLARED
typedef struct YYLTYPE YYLTYPE;
struct YYLTYPE
{
  int first_line;
  int first_column;
  int last_line;
  int last_column;
};
# define YYLTYPE_IS_DECLARED 1
# define YYLTYPE_IS_TRIVIAL 1
#endif



int yyparse (class yyFlexLexer* plexer, class FrameIOParserDb* db);

#endif /* !YY_YY_FPARSER_TAB_H_INCLUDED  */
