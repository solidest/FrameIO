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
    T_NOTE = 264,
    T_ID = 265,
    T_INTEGER = 266,
    T_REAL = 267,
    T_BLOCK = 268,
    T_TEXT = 269,
    T_BOOL = 270,
    T_BYTE = 271,
    T_SBYTE = 272,
    T_USHORT = 273,
    T_SHORT = 274,
    T_UINT = 275,
    T_INT = 276,
    T_ULONG = 277,
    T_LONG = 278,
    T_FLOAT = 279,
    T_DOUBLE = 280,
    T_STRING = 281,
    T_SEND = 282,
    T_ON = 283,
    T_RECV = 284,
    T_RECVLOOP = 285,
    T_COM = 286,
    T_CAN = 287,
    T_TCPSERVER = 288,
    T_TCPCLIENT = 289,
    T_UDP = 290,
    T_DI = 291,
    T_DO = 292,
    T_DEVICEID = 293,
    T_BAUDRATE = 294,
    T_SIGNED = 295,
    T_BITCOUNT = 296,
    T_VALUE = 297,
    T_REPEATED = 298,
    T_BYTEORDER = 299,
    T_ENCODED = 300,
    T_ISDOUBLE = 301,
    T_TAIL = 302,
    T_ALIGEDLEN = 303,
    T_TYPE = 304,
    T_BYTESIZE = 305,
    T_TOENUM = 306,
    T_ONEOF = 307,
    T_MAX = 308,
    T_MIN = 309,
    T_CHECK = 310,
    T_CHECKRANGE = 311,
    T_TRUE = 312,
    T_FALSE = 313,
    T_SMALL = 314,
    T_BIG = 315,
    T_PRIMITIVE = 316,
    T_INVRSION = 317,
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
    VALUE_REAL = 354
  };
#endif

/* Value type.  */
#if ! defined YYSTYPE && ! defined YYSTYPE_IS_DECLARED

union YYSTYPE
{
#line 27 "fparser.y" /* yacc.c:1913  */

	int symbol;

	PROJECT * project;
	SYS * syslist;
	FRAME * framelist;
	ENUMCFG * enumcfglist;

#line 163 "fparser.tab.h" /* yacc.c:1913  */
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
