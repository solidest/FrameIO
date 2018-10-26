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
    T_PROJECT = 258,
    T_SYSTEM = 259,
    T_FRAME = 260,
    T_CHANNEL = 261,
    T_ENUM = 262,
    T_ACTION = 263,
    T_THIS = 264,
    T_INTEGER = 265,
    T_REAL = 266,
    T_BLOCK = 267,
    T_TEXT = 268,
    T_BOOL = 269,
    T_BYTE = 270,
    T_SBYTE = 271,
    T_USHORT = 272,
    T_SHORT = 273,
    T_UINT = 274,
    T_INT = 275,
    T_ULONG = 276,
    T_LONG = 277,
    T_FLOAT = 278,
    T_DOUBLE = 279,
    T_STRING = 280,
    T_SEND = 281,
    T_ON = 282,
    T_RECV = 283,
    T_RECVLOOP = 284,
    T_COM = 285,
    T_CAN = 286,
    T_TCPSERVER = 287,
    T_TCPCLIENT = 288,
    T_UDP = 289,
    T_DIO = 290,
    T_SIGNED = 291,
    T_BITCOUNT = 292,
    T_VALUE = 293,
    T_REPEATED = 294,
    T_BYTEORDER = 295,
    T_ENCODED = 296,
    T_ISDOUBLE = 297,
    T_TAIL = 298,
    T_ALIGNEDLEN = 299,
    T_TYPE = 300,
    T_BYTESIZE = 301,
    T_BYTESIZEOF = 302,
    T_TOENUM = 303,
    T_ONEOF = 304,
    T_DEFAULT = 305,
    T_MAX = 306,
    T_MIN = 307,
    T_CHECK = 308,
    T_CHECKRANGE = 309,
    T_TRUE = 310,
    T_FALSE = 311,
    T_SMALL = 312,
    T_BIG = 313,
    T_PRIMITIVE = 314,
    T_INVERSION = 315,
    T_COMPLEMENT = 316,
    T_SUM8 = 317,
    T_XOR8 = 318,
    T_SUM16 = 319,
    T_SUM16_FALSE = 320,
    T_XOR16 = 321,
    T_XOR16_FALSE = 322,
    T_SUM32 = 323,
    T_SUM32_FALSE = 324,
    T_XOR32 = 325,
    T_XOR32_FALSE = 326,
    T_CRC4_ITU = 327,
    T_CRC5_EPC = 328,
    T_CRC5_ITU = 329,
    T_CRC5_USB = 330,
    T_CRC6_ITU = 331,
    T_CRC7_MMC = 332,
    T_CRC8 = 333,
    T_CRC8_ITU = 334,
    T_CRC8_ROHC = 335,
    T_CRC8_MAXIM = 336,
    T_CRC16_IBM = 337,
    T_CRC16_MAXIM = 338,
    T_CRC16_USB = 339,
    T_CRC16_MODBUS = 340,
    T_CRC16_CCITT = 341,
    T_CRC16_CCITT_FALSE = 342,
    T_CRC16_X25 = 343,
    T_CRC16_XMODEM = 344,
    T_CRC16_DNP = 345,
    T_CRC32 = 346,
    T_CRC32_MPEG_2 = 347,
    T_CRC64 = 348,
    T_CRC64_WE = 349,
    VALUE_STRING = 350,
    VALUE_INT = 351,
    VALUE_REAL = 352,
    T_ID = 353,
    T_NOTE = 354,
    T_UNION_ID = 355,
    T_AT_USER = 356
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
	syspropertytype sysptype;
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


#line 185 "fparser.tab.h" /* yacc.c:1913  */
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
