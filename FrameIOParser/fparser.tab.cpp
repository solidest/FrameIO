/* A Bison parser, made by GNU Bison 3.1.  */

/* Bison implementation for Yacc-like parsers in C

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

/* C LALR(1) parser skeleton written by Richard Stallman, by
   simplifying the original so-called "semantic" parser.  */

/* All symbols defined below should begin with yy or YY, to avoid
   infringing on user name space.  This should be done even for local
   variables, as they might otherwise be expanded by user macros.
   There are some unavoidable exceptions within include files to
   define necessary library symbols; they are noted "INFRINGES ON
   USER NAME SPACE" below.  */

/* Identify Bison output.  */
#define YYBISON 1

/* Bison version.  */
#define YYBISON_VERSION "3.1"

/* Skeleton name.  */
#define YYSKELETON_NAME "yacc.c"

/* Pure parsers.  */
#define YYPURE 1

/* Push parsers.  */
#define YYPUSH 0

/* Pull parsers.  */
#define YYPULL 1




/* Copy the first part of user declarations.  */
#line 7 "fparser.y" /* yacc.c:339  */

#include <stdlib.h>
#include "stdafx.h" 
#include "fparser.h"
#include "fparser.tab.h"
#include <FlexLexer.h>
#include "FrameIOParserDb.h"

#undef yylex
#define yylex plexer->yylex
#define yyerror(pyylloc, plexer, db, msg) fyyerror(yylval, db, msg)

int fyyerror(YYSTYPE yylval, class FrameIOParserDb* db, const char* msg) 
{
	db->SaveError(ERROR_CODE_SYNTAX, yylval.symbol, yylval.symbol);
	return 1; 
}

#line 85 "fparser.tab.cpp" /* yacc.c:339  */

# ifndef YY_NULLPTR
#  if defined __cplusplus && 201103L <= __cplusplus
#   define YY_NULLPTR nullptr
#  else
#   define YY_NULLPTR 0
#  endif
# endif

/* Enabling verbose error messages.  */
#ifdef YYERROR_VERBOSE
# undef YYERROR_VERBOSE
# define YYERROR_VERBOSE 1
#else
# define YYERROR_VERBOSE 0
#endif

/* In a future release of Bison, this section will be replaced
   by #include "fparser.tab.h".  */
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
    T_DI = 290,
    T_DO = 291,
    T_DEVICEID = 292,
    T_BAUDRATE = 293,
    T_SIGNED = 294,
    T_BITCOUNT = 295,
    T_VALUE = 296,
    T_REPEATED = 297,
    T_BYTEORDER = 298,
    T_ENCODED = 299,
    T_ISDOUBLE = 300,
    T_TAIL = 301,
    T_ALIGNEDLEN = 302,
    T_TYPE = 303,
    T_BYTESIZE = 304,
    T_BYTESIZEOF = 305,
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
    T_NOTE = 356
  };
#endif

/* Value type.  */
#if ! defined YYSTYPE && ! defined YYSTYPE_IS_DECLARED

union YYSTYPE
{
#line 27 "fparser.y" /* yacc.c:355  */

	int symbol;
	int optionvalue;
	segpropertytype segproptype;
	segpropertyvaluetype segprovtype;
	segmenttype segtype;
	syspropertytype sysptype;
	channeloptiontype choptype;
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


#line 257 "fparser.tab.cpp" /* yacc.c:355  */
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

/* Copy the second part of user declarations.  */

#line 287 "fparser.tab.cpp" /* yacc.c:358  */

#ifdef short
# undef short
#endif

#ifdef YYTYPE_UINT8
typedef YYTYPE_UINT8 yytype_uint8;
#else
typedef unsigned char yytype_uint8;
#endif

#ifdef YYTYPE_INT8
typedef YYTYPE_INT8 yytype_int8;
#else
typedef signed char yytype_int8;
#endif

#ifdef YYTYPE_UINT16
typedef YYTYPE_UINT16 yytype_uint16;
#else
typedef unsigned short yytype_uint16;
#endif

#ifdef YYTYPE_INT16
typedef YYTYPE_INT16 yytype_int16;
#else
typedef short yytype_int16;
#endif

#ifndef YYSIZE_T
# ifdef __SIZE_TYPE__
#  define YYSIZE_T __SIZE_TYPE__
# elif defined size_t
#  define YYSIZE_T size_t
# elif ! defined YYSIZE_T
#  include <stddef.h> /* INFRINGES ON USER NAME SPACE */
#  define YYSIZE_T size_t
# else
#  define YYSIZE_T unsigned
# endif
#endif

#define YYSIZE_MAXIMUM ((YYSIZE_T) -1)

#ifndef YY_
# if defined YYENABLE_NLS && YYENABLE_NLS
#  if ENABLE_NLS
#   include <libintl.h> /* INFRINGES ON USER NAME SPACE */
#   define YY_(Msgid) dgettext ("bison-runtime", Msgid)
#  endif
# endif
# ifndef YY_
#  define YY_(Msgid) Msgid
# endif
#endif

#ifndef YY_ATTRIBUTE
# if (defined __GNUC__                                               \
      && (2 < __GNUC__ || (__GNUC__ == 2 && 96 <= __GNUC_MINOR__)))  \
     || defined __SUNPRO_C && 0x5110 <= __SUNPRO_C
#  define YY_ATTRIBUTE(Spec) __attribute__(Spec)
# else
#  define YY_ATTRIBUTE(Spec) /* empty */
# endif
#endif

#ifndef YY_ATTRIBUTE_PURE
# define YY_ATTRIBUTE_PURE   YY_ATTRIBUTE ((__pure__))
#endif

#ifndef YY_ATTRIBUTE_UNUSED
# define YY_ATTRIBUTE_UNUSED YY_ATTRIBUTE ((__unused__))
#endif

#if !defined _Noreturn \
     && (!defined __STDC_VERSION__ || __STDC_VERSION__ < 201112)
# if defined _MSC_VER && 1200 <= _MSC_VER
#  define _Noreturn __declspec (noreturn)
# else
#  define _Noreturn YY_ATTRIBUTE ((__noreturn__))
# endif
#endif

/* Suppress unused-variable warnings by "using" E.  */
#if ! defined lint || defined __GNUC__
# define YYUSE(E) ((void) (E))
#else
# define YYUSE(E) /* empty */
#endif

#if defined __GNUC__ && ! defined __ICC && 407 <= __GNUC__ * 100 + __GNUC_MINOR__
/* Suppress an incorrect diagnostic about yylval being uninitialized.  */
# define YY_IGNORE_MAYBE_UNINITIALIZED_BEGIN \
    _Pragma ("GCC diagnostic push") \
    _Pragma ("GCC diagnostic ignored \"-Wuninitialized\"")\
    _Pragma ("GCC diagnostic ignored \"-Wmaybe-uninitialized\"")
# define YY_IGNORE_MAYBE_UNINITIALIZED_END \
    _Pragma ("GCC diagnostic pop")
#else
# define YY_INITIAL_VALUE(Value) Value
#endif
#ifndef YY_IGNORE_MAYBE_UNINITIALIZED_BEGIN
# define YY_IGNORE_MAYBE_UNINITIALIZED_BEGIN
# define YY_IGNORE_MAYBE_UNINITIALIZED_END
#endif
#ifndef YY_INITIAL_VALUE
# define YY_INITIAL_VALUE(Value) /* Nothing. */
#endif


#if ! defined yyoverflow || YYERROR_VERBOSE

/* The parser invokes alloca or malloc; define the necessary symbols.  */

# ifdef YYSTACK_USE_ALLOCA
#  if YYSTACK_USE_ALLOCA
#   ifdef __GNUC__
#    define YYSTACK_ALLOC __builtin_alloca
#   elif defined __BUILTIN_VA_ARG_INCR
#    include <alloca.h> /* INFRINGES ON USER NAME SPACE */
#   elif defined _AIX
#    define YYSTACK_ALLOC __alloca
#   elif defined _MSC_VER
#    include <malloc.h> /* INFRINGES ON USER NAME SPACE */
#    define alloca _alloca
#   else
#    define YYSTACK_ALLOC alloca
#    if ! defined _ALLOCA_H && ! defined EXIT_SUCCESS
#     include <stdlib.h> /* INFRINGES ON USER NAME SPACE */
      /* Use EXIT_SUCCESS as a witness for stdlib.h.  */
#     ifndef EXIT_SUCCESS
#      define EXIT_SUCCESS 0
#     endif
#    endif
#   endif
#  endif
# endif

# ifdef YYSTACK_ALLOC
   /* Pacify GCC's 'empty if-body' warning.  */
#  define YYSTACK_FREE(Ptr) do { /* empty */; } while (0)
#  ifndef YYSTACK_ALLOC_MAXIMUM
    /* The OS might guarantee only one guard page at the bottom of the stack,
       and a page size can be as small as 4096 bytes.  So we cannot safely
       invoke alloca (N) if N exceeds 4096.  Use a slightly smaller number
       to allow for a few compiler-allocated temporary stack slots.  */
#   define YYSTACK_ALLOC_MAXIMUM 4032 /* reasonable circa 2006 */
#  endif
# else
#  define YYSTACK_ALLOC YYMALLOC
#  define YYSTACK_FREE YYFREE
#  ifndef YYSTACK_ALLOC_MAXIMUM
#   define YYSTACK_ALLOC_MAXIMUM YYSIZE_MAXIMUM
#  endif
#  if (defined __cplusplus && ! defined EXIT_SUCCESS \
       && ! ((defined YYMALLOC || defined malloc) \
             && (defined YYFREE || defined free)))
#   include <stdlib.h> /* INFRINGES ON USER NAME SPACE */
#   ifndef EXIT_SUCCESS
#    define EXIT_SUCCESS 0
#   endif
#  endif
#  ifndef YYMALLOC
#   define YYMALLOC malloc
#   if ! defined malloc && ! defined EXIT_SUCCESS
void *malloc (YYSIZE_T); /* INFRINGES ON USER NAME SPACE */
#   endif
#  endif
#  ifndef YYFREE
#   define YYFREE free
#   if ! defined free && ! defined EXIT_SUCCESS
void free (void *); /* INFRINGES ON USER NAME SPACE */
#   endif
#  endif
# endif
#endif /* ! defined yyoverflow || YYERROR_VERBOSE */


#if (! defined yyoverflow \
     && (! defined __cplusplus \
         || (defined YYLTYPE_IS_TRIVIAL && YYLTYPE_IS_TRIVIAL \
             && defined YYSTYPE_IS_TRIVIAL && YYSTYPE_IS_TRIVIAL)))

/* A type that is properly aligned for any stack member.  */
union yyalloc
{
  yytype_int16 yyss_alloc;
  YYSTYPE yyvs_alloc;
  YYLTYPE yyls_alloc;
};

/* The size of the maximum gap between one aligned stack and the next.  */
# define YYSTACK_GAP_MAXIMUM (sizeof (union yyalloc) - 1)

/* The size of an array large to enough to hold all stacks, each with
   N elements.  */
# define YYSTACK_BYTES(N) \
     ((N) * (sizeof (yytype_int16) + sizeof (YYSTYPE) + sizeof (YYLTYPE)) \
      + 2 * YYSTACK_GAP_MAXIMUM)

# define YYCOPY_NEEDED 1

/* Relocate STACK from its old location to the new one.  The
   local variables YYSIZE and YYSTACKSIZE give the old and new number of
   elements in the stack, and YYPTR gives the new location of the
   stack.  Advance YYPTR to a properly aligned location for the next
   stack.  */
# define YYSTACK_RELOCATE(Stack_alloc, Stack)                           \
    do                                                                  \
      {                                                                 \
        YYSIZE_T yynewbytes;                                            \
        YYCOPY (&yyptr->Stack_alloc, Stack, yysize);                    \
        Stack = &yyptr->Stack_alloc;                                    \
        yynewbytes = yystacksize * sizeof (*Stack) + YYSTACK_GAP_MAXIMUM; \
        yyptr += yynewbytes / sizeof (*yyptr);                          \
      }                                                                 \
    while (0)

#endif

#if defined YYCOPY_NEEDED && YYCOPY_NEEDED
/* Copy COUNT objects from SRC to DST.  The source and destination do
   not overlap.  */
# ifndef YYCOPY
#  if defined __GNUC__ && 1 < __GNUC__
#   define YYCOPY(Dst, Src, Count) \
      __builtin_memcpy (Dst, Src, (Count) * sizeof (*(Src)))
#  else
#   define YYCOPY(Dst, Src, Count)              \
      do                                        \
        {                                       \
          YYSIZE_T yyi;                         \
          for (yyi = 0; yyi < (Count); yyi++)   \
            (Dst)[yyi] = (Src)[yyi];            \
        }                                       \
      while (0)
#  endif
# endif
#endif /* !YYCOPY_NEEDED */

/* YYFINAL -- State number of the termination state.  */
#define YYFINAL  3
/* YYLAST -- Last index in YYTABLE.  */
#define YYLAST   224

/* YYNTOKENS -- Number of terminals.  */
#define YYNTOKENS  116
/* YYNNTS -- Number of nonterminals.  */
#define YYNNTS  41
/* YYNRULES -- Number of rules.  */
#define YYNRULES  135
/* YYNSTATES -- Number of states.  */
#define YYNSTATES  235

/* YYTRANSLATE[YYX] -- Symbol number corresponding to YYX as returned
   by yylex, with out-of-bounds checking.  */
#define YYUNDEFTOK  2
#define YYMAXUTOK   356

#define YYTRANSLATE(YYX)                                                \
  ((unsigned) (YYX) <= YYMAXUTOK ? yytranslate[YYX] : YYUNDEFTOK)

/* YYTRANSLATE[TOKEN-NUM] -- Symbol number corresponding to TOKEN-NUM
   as returned by yylex, without out-of-bounds checking.  */
static const yytype_uint8 yytranslate[] =
{
       0,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     113,   115,   105,   103,   114,   104,     2,   106,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,   112,   109,
       2,   102,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,   110,     2,   111,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,   107,     2,   108,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     1,     2,     3,     4,
       5,     6,     7,     8,     9,    10,    11,    12,    13,    14,
      15,    16,    17,    18,    19,    20,    21,    22,    23,    24,
      25,    26,    27,    28,    29,    30,    31,    32,    33,    34,
      35,    36,    37,    38,    39,    40,    41,    42,    43,    44,
      45,    46,    47,    48,    49,    50,    51,    52,    53,    54,
      55,    56,    57,    58,    59,    60,    61,    62,    63,    64,
      65,    66,    67,    68,    69,    70,    71,    72,    73,    74,
      75,    76,    77,    78,    79,    80,    81,    82,    83,    84,
      85,    86,    87,    88,    89,    90,    91,    92,    93,    94,
      95,    96,    97,    98,    99,   100,   101
};

#if YYDEBUG
  /* YYRLINE[YYN] -- Source line where rule number YYN was defined.  */
static const yytype_uint16 yyrline[] =
{
       0,   107,   107,   110,   111,   115,   116,   117,   121,   124,
     125,   129,   130,   131,   135,   136,   140,   141,   142,   143,
     144,   145,   146,   147,   148,   149,   150,   154,   158,   159,
     160,   161,   162,   163,   164,   167,   168,   172,   176,   177,
     181,   182,   183,   187,   191,   192,   193,   196,   197,   201,
     205,   208,   209,   213,   217,   218,   219,   220,   223,   224,
     228,   229,   230,   231,   232,   233,   234,   235,   236,   237,
     238,   239,   244,   245,   249,   250,   254,   255,   259,   260,
     265,   266,   270,   271,   272,   276,   277,   278,   279,   280,
     281,   282,   283,   284,   285,   286,   287,   288,   289,   290,
     291,   292,   293,   294,   295,   296,   297,   298,   299,   300,
     304,   305,   306,   310,   311,   315,   319,   320,   321,   325,
     326,   327,   328,   329,   330,   331,   332,   333,   334,   338,
     342,   343,   347,   348,   351,   352
};
#endif

#if YYDEBUG || YYERROR_VERBOSE || 0
/* YYTNAME[SYMBOL-NUM] -- String name of the symbol SYMBOL-NUM.
   First, the terminals, then, starting at YYNTOKENS, nonterminals.  */
static const char *const yytname[] =
{
  "$end", "error", "$undefined", "T_PROJECT", "T_SYSTEM", "T_FRAME",
  "T_CHANNEL", "T_ENUM", "T_ACTION", "T_THIS", "T_INTEGER", "T_REAL",
  "T_BLOCK", "T_TEXT", "T_BOOL", "T_BYTE", "T_SBYTE", "T_USHORT",
  "T_SHORT", "T_UINT", "T_INT", "T_ULONG", "T_LONG", "T_FLOAT", "T_DOUBLE",
  "T_STRING", "T_SEND", "T_ON", "T_RECV", "T_RECVLOOP", "T_COM", "T_CAN",
  "T_TCPSERVER", "T_TCPCLIENT", "T_UDP", "T_DI", "T_DO", "T_DEVICEID",
  "T_BAUDRATE", "T_SIGNED", "T_BITCOUNT", "T_VALUE", "T_REPEATED",
  "T_BYTEORDER", "T_ENCODED", "T_ISDOUBLE", "T_TAIL", "T_ALIGNEDLEN",
  "T_TYPE", "T_BYTESIZE", "T_BYTESIZEOF", "T_TOENUM", "T_ONEOF", "T_MAX",
  "T_MIN", "T_CHECK", "T_CHECKRANGE", "T_TRUE", "T_FALSE", "T_SMALL",
  "T_BIG", "T_PRIMITIVE", "T_INVERSION", "T_COMPLEMENT", "T_SUM8",
  "T_XOR8", "T_SUM16", "T_SUM16_FALSE", "T_XOR16", "T_XOR16_FALSE",
  "T_SUM32", "T_SUM32_FALSE", "T_XOR32", "T_XOR32_FALSE", "T_CRC4_ITU",
  "T_CRC5_EPC", "T_CRC5_ITU", "T_CRC5_USB", "T_CRC6_ITU", "T_CRC7_MMC",
  "T_CRC8", "T_CRC8_ITU", "T_CRC8_ROHC", "T_CRC8_MAXIM", "T_CRC16_IBM",
  "T_CRC16_MAXIM", "T_CRC16_USB", "T_CRC16_MODBUS", "T_CRC16_CCITT",
  "T_CRC16_CCITT_FALSE", "T_CRC16_X25", "T_CRC16_XMODEM", "T_CRC16_DNP",
  "T_CRC32", "T_CRC32_MPEG_2", "T_CRC64", "T_CRC64_WE", "VALUE_STRING",
  "VALUE_INT", "VALUE_REAL", "T_ID", "T_NOTE", "'='", "'+'", "'-'", "'*'",
  "'/'", "'{'", "'}'", "';'", "'['", "']'", "':'", "'('", "','", "')'",
  "$accept", "project", "projectitemlist", "projectitem", "system",
  "systemitemlist", "systemitem", "sysproperty", "sysprotype", "channel",
  "channeltype", "channeloptionlist", "channeloption", "channeloptionname",
  "channeloptionvalue", "action", "actiontype", "actionmaplist",
  "actionmap", "frame", "framesegmentlist", "framesegment",
  "framesegmenttype", "framesegmentpropertylist", "framesegmentproperty",
  "framesegmentpropertybool", "framesegmentpropertyboolvalue",
  "framesegmentpropertyint", "framesegmentpropertyconst",
  "framesegmentpropertyorder", "framesegmentpropertyencoded",
  "framesegmentcheckvalue", "framesegmentpropertytypevalue",
  "framesegmentoneoflist", "framesegmentoneofitem",
  "framesegmentpropertyexp", "exp", "enumcfg", "enumitemlist", "enumitem",
  "notelist", YY_NULLPTR
};
#endif

# ifdef YYPRINT
/* YYTOKNUM[NUM] -- (External) token number corresponding to the
   (internal) symbol number NUM (which must be that of a token).  */
static const yytype_uint16 yytoknum[] =
{
       0,   256,   257,   258,   259,   260,   261,   262,   263,   264,
     265,   266,   267,   268,   269,   270,   271,   272,   273,   274,
     275,   276,   277,   278,   279,   280,   281,   282,   283,   284,
     285,   286,   287,   288,   289,   290,   291,   292,   293,   294,
     295,   296,   297,   298,   299,   300,   301,   302,   303,   304,
     305,   306,   307,   308,   309,   310,   311,   312,   313,   314,
     315,   316,   317,   318,   319,   320,   321,   322,   323,   324,
     325,   326,   327,   328,   329,   330,   331,   332,   333,   334,
     335,   336,   337,   338,   339,   340,   341,   342,   343,   344,
     345,   346,   347,   348,   349,   350,   351,   352,   353,   354,
     355,   356,    61,    43,    45,    42,    47,   123,   125,    59,
      91,    93,    58,    40,    44,    41
};
# endif

#define YYPACT_NINF -126

#define yypact_value_is_default(Yystate) \
  (!!((Yystate) == (-126)))

#define YYTABLE_NINF -1

#define yytable_value_is_error(Yytable_value) \
  0

  /* YYPACT[STATE-NUM] -- Index in YYTABLE of the portion describing
     STATE-NUM.  */
static const yytype_int16 yypact[] =
{
    -126,    19,     2,  -126,   -79,  -126,   -83,  -126,  -126,  -126,
    -126,  -126,  -126,    -3,   -61,   -28,   -14,  -126,   -10,    -8,
      16,    -9,  -126,  -126,  -126,  -126,  -126,     7,  -126,     8,
    -126,  -126,  -126,  -126,    14,  -126,    -1,  -126,   -48,     4,
      58,    59,  -126,  -126,  -126,  -126,  -126,  -126,  -126,  -126,
    -126,  -126,  -126,  -126,   -41,  -126,  -126,  -126,  -126,  -126,
      60,  -126,  -126,    63,    50,    53,    54,    55,  -126,  -126,
     119,    67,  -126,    64,     1,  -126,  -126,  -126,  -126,  -126,
    -126,  -126,    61,  -126,  -126,  -126,    69,    65,  -126,  -126,
    -126,  -126,    68,    70,  -126,    71,  -126,    73,  -126,    74,
    -126,  -126,    75,    76,  -126,  -126,    77,    78,    79,    81,
    -126,   140,  -126,    42,    21,    87,   -29,    85,    52,    80,
      62,    88,    27,   -32,  -126,    89,  -126,  -126,  -126,  -126,
    -126,  -126,  -126,  -126,    82,  -126,  -126,  -126,  -126,  -126,
    -126,  -126,  -126,  -126,  -126,  -126,  -126,  -126,  -126,  -126,
    -126,  -126,  -126,  -126,  -126,  -126,  -126,  -126,  -126,  -126,
    -126,  -126,  -126,  -126,  -126,    90,  -126,  -126,  -126,  -126,
    -126,  -126,    83,  -126,  -126,  -126,   -32,   -15,  -126,   -31,
      84,    92,  -126,    57,    -6,   -30,   -32,   -32,   -32,   -32,
    -126,  -126,  -126,    86,  -126,    72,     3,    94,    93,    95,
    -126,    51,    51,  -126,  -126,    15,  -126,    91,  -126,    96,
    -126,  -126,  -126,  -126,  -126,    98,  -126,   -21,    97,  -126,
    -126,   100,  -126,   101,   102,  -126,    99,   103,    97,   -43,
     105,  -126,  -126,  -126,  -126
};

  /* YYDEFACT[STATE-NUM] -- Default reduction number in state STATE-NUM.
     Performed when YYTABLE does not specify something else to do.  Zero
     means the default is an error.  */
static const yytype_uint8 yydefact[] =
{
     134,     0,     0,     1,     0,   135,     0,     3,   134,     4,
       5,     6,     7,     0,     0,     0,     0,   134,     0,     0,
       0,     2,     9,    51,   134,   134,   134,   134,   130,     0,
      10,    11,    12,    13,     0,    52,     0,   134,     0,   133,
       0,     0,    16,    17,    18,    19,    20,    21,    22,    23,
      24,    25,    26,     8,     0,    54,    55,    56,    57,    50,
       0,   131,   129,     0,     0,     0,     0,     0,    58,   132,
       0,     0,    14,     0,     0,    28,    29,    30,    31,    32,
      33,    34,     0,    44,    45,    46,     0,     0,    73,    76,
     116,   117,     0,     0,    72,     0,    77,     0,   118,     0,
      78,    79,     0,     0,    53,    59,     0,     0,     0,     0,
      35,     0,    15,     0,     0,     0,     0,     0,     0,     0,
       0,     0,     0,     0,   134,     0,    80,    81,    64,    82,
      83,    84,    65,    69,     0,   110,    51,    70,    68,    85,
      86,    87,    88,    89,    90,    91,    92,    93,    94,    95,
      96,    97,    98,    99,   100,   101,   102,   103,   104,   105,
     106,   107,   108,   109,    66,     0,    74,    75,    60,    61,
      62,    63,     0,   126,   125,   124,     0,    71,    36,     0,
       0,     0,   134,     0,     0,     0,     0,     0,     0,     0,
      38,    39,    27,     0,    47,     0,     0,     0,     0,     0,
     123,   119,   120,   121,   122,     0,   134,     0,   111,     0,
     128,   127,    41,    40,    42,     0,    48,     0,     0,    67,
      37,     0,    43,     0,   134,   113,     0,     0,     0,     0,
       0,   115,   114,   112,    49
};

  /* YYPGOTO[NTERM-NUM].  */
static const yytype_int16 yypgoto[] =
{
    -126,  -126,  -126,  -126,  -126,  -126,  -126,  -126,  -126,  -126,
    -126,  -126,  -126,  -126,  -126,  -126,  -126,  -126,  -126,  -126,
      66,  -126,  -126,  -126,  -126,  -126,  -126,  -126,  -126,  -126,
    -126,  -126,  -126,  -126,   -27,  -126,  -125,  -126,  -126,   163,
       0
};

  /* YYDEFGOTO[NTERM-NUM].  */
static const yytype_int16 yydefgoto[] =
{
      -1,     1,     8,     9,    10,    25,    30,    31,    54,    32,
      82,   124,   178,   193,   215,    33,    86,   206,   216,    11,
      26,    35,    60,    74,   105,   106,   168,   107,   108,   128,
     132,   164,   137,   224,   225,   109,   177,    12,    27,    28,
      29
};

  /* YYTABLE[YYPACT[STATE-NUM]] -- What to do in state STATE-NUM.  If
     positive, shift that token.  If negative, reduce the rule whose
     number is the opposite.  If YYTABLE_NINF, syntax error.  */
static const yytype_uint8 yytable[] =
{
       2,    14,    15,   198,    16,     4,   190,   191,    13,    55,
      56,    57,    58,    55,    56,    57,    58,    21,   172,     3,
      40,     6,    41,   134,     7,    34,    36,    38,    42,    43,
      44,    45,    46,    47,    48,    49,    50,    51,    52,    18,
      88,    89,    90,    91,    92,    93,    94,    95,    96,    97,
      98,   185,    99,     5,   100,   101,   102,   103,     5,    66,
      62,   201,   202,   203,   204,   233,   173,   174,   175,    67,
       5,   135,    19,   186,   187,   188,   189,   192,   136,   221,
       5,   176,   129,   130,   131,   200,    20,   222,   186,   187,
     188,   189,     5,    83,   199,    84,    85,    22,     5,    23,
       5,   126,   127,     5,     5,    17,    63,    59,    39,     5,
     104,   208,   212,   213,   214,     5,   139,   140,   141,   166,
     167,    37,    53,    24,   179,   170,   171,   142,   143,   144,
     145,   146,   147,   148,   149,   150,   151,   152,   153,   154,
     155,   156,   157,   158,   159,   160,   161,   162,   163,    75,
      76,    77,    78,    79,    80,    81,   188,   189,    64,    65,
      68,    69,    70,    72,    87,    71,    73,   125,   110,   111,
     113,   197,   114,   115,   112,   116,   117,   118,   119,   120,
     121,   122,   196,   123,   133,   138,   169,   207,   205,   180,
     183,   194,   195,   165,   209,   181,   184,   223,   218,   230,
      61,   232,   182,   231,     0,     0,   217,   220,   210,     0,
     211,   219,   226,   227,   234,     0,   228,     0,     0,     0,
       0,     0,     0,     0,   229
};

static const yytype_int16 yycheck[] =
{
       0,     4,     5,     9,     7,     3,    37,    38,     8,    10,
      11,    12,    13,    10,    11,    12,    13,    17,    50,     0,
       6,   100,     8,    52,   107,    25,    26,    27,    14,    15,
      16,    17,    18,    19,    20,    21,    22,    23,    24,   100,
      39,    40,    41,    42,    43,    44,    45,    46,    47,    48,
      49,   176,    51,   101,    53,    54,    55,    56,   101,   100,
     108,   186,   187,   188,   189,   108,    98,    99,   100,   110,
     101,   100,   100,   103,   104,   105,   106,   108,   107,   100,
     101,   113,    61,    62,    63,   115,   100,   108,   103,   104,
     105,   106,   101,    26,   100,    28,    29,   107,   101,   107,
     101,    59,    60,   101,   101,   108,   102,   108,   100,   101,
     109,   108,    97,    98,    99,   101,    64,    65,    66,    57,
      58,   114,   108,   107,   124,    98,    99,    75,    76,    77,
      78,    79,    80,    81,    82,    83,    84,    85,    86,    87,
      88,    89,    90,    91,    92,    93,    94,    95,    96,    30,
      31,    32,    33,    34,    35,    36,   105,   106,   100,   100,
     100,    98,   112,   109,   100,   112,   111,    27,   107,   100,
     102,   114,   102,   102,   109,   102,   102,   102,   102,   102,
     102,   102,   182,   102,    97,   100,    98,   115,   102,   100,
     100,   107,   100,   113,   100,   113,   113,   100,   107,   100,
      37,   228,   136,   100,    -1,    -1,   206,   109,   115,    -1,
     115,   115,   112,   112,   109,    -1,   114,    -1,    -1,    -1,
      -1,    -1,    -1,    -1,   224
};

  /* YYSTOS[STATE-NUM] -- The (internal number of the) accessing
     symbol of state STATE-NUM.  */
static const yytype_uint8 yystos[] =
{
       0,   117,   156,     0,     3,   101,   100,   107,   118,   119,
     120,   135,   153,   156,     4,     5,     7,   108,   100,   100,
     100,   156,   107,   107,   107,   121,   136,   154,   155,   156,
     122,   123,   125,   131,   156,   137,   156,   114,   156,   100,
       6,     8,    14,    15,    16,    17,    18,    19,    20,    21,
      22,    23,    24,   108,   124,    10,    11,    12,    13,   108,
     138,   155,   108,   102,   100,   100,   100,   110,   100,    98,
     112,   112,   109,   111,   139,    30,    31,    32,    33,    34,
      35,    36,   126,    26,    28,    29,   132,   100,    39,    40,
      41,    42,    43,    44,    45,    46,    47,    48,    49,    51,
      53,    54,    55,    56,   109,   140,   141,   143,   144,   151,
     107,   100,   109,   102,   102,   102,   102,   102,   102,   102,
     102,   102,   102,   102,   127,    27,    59,    60,   145,    61,
      62,    63,   146,    97,    52,   100,   107,   148,   100,    64,
      65,    66,    75,    76,    77,    78,    79,    80,    81,    82,
      83,    84,    85,    86,    87,    88,    89,    90,    91,    92,
      93,    94,    95,    96,   147,   113,    57,    58,   142,    98,
      98,    99,    50,    98,    99,   100,   113,   152,   128,   156,
     100,   113,   136,   100,   113,   152,   103,   104,   105,   106,
      37,    38,   108,   129,   107,   100,   156,   114,     9,   100,
     115,   152,   152,   152,   152,   102,   133,   115,   108,   100,
     115,   115,    97,    98,    99,   130,   134,   156,   107,   115,
     109,   100,   108,   100,   149,   150,   112,   112,   114,   156,
     100,   100,   150,   108,   109
};

  /* YYR1[YYN] -- Symbol number of symbol that rule YYN derives.  */
static const yytype_uint8 yyr1[] =
{
       0,   116,   117,   118,   118,   119,   119,   119,   120,   121,
     121,   122,   122,   122,   123,   123,   124,   124,   124,   124,
     124,   124,   124,   124,   124,   124,   124,   125,   126,   126,
     126,   126,   126,   126,   126,   127,   127,   128,   129,   129,
     130,   130,   130,   131,   132,   132,   132,   133,   133,   134,
     135,   136,   136,   137,   138,   138,   138,   138,   139,   139,
     140,   140,   140,   140,   140,   140,   140,   140,   140,   140,
     140,   140,   141,   141,   142,   142,   143,   143,   144,   144,
     145,   145,   146,   146,   146,   147,   147,   147,   147,   147,
     147,   147,   147,   147,   147,   147,   147,   147,   147,   147,
     147,   147,   147,   147,   147,   147,   147,   147,   147,   147,
     148,   148,   148,   149,   149,   150,   151,   151,   151,   152,
     152,   152,   152,   152,   152,   152,   152,   152,   152,   153,
     154,   154,   155,   155,   156,   156
};

  /* YYR2[YYN] -- Number of symbols on the right hand side of rule YYN.  */
static const yytype_uint8 yyr2[] =
{
       0,     2,     8,     0,     2,     1,     1,     1,     7,     0,
       2,     1,     1,     1,     4,     6,     1,     1,     1,     1,
       1,     1,     1,     1,     1,     1,     1,     9,     1,     1,
       1,     1,     1,     1,     1,     0,     2,     5,     1,     1,
       1,     1,     1,    12,     1,     1,     1,     0,     2,     5,
       7,     0,     2,     5,     1,     1,     1,     1,     0,     2,
       3,     3,     3,     3,     3,     3,     3,     7,     3,     3,
       3,     3,     1,     1,     1,     1,     1,     1,     1,     1,
       1,     1,     1,     1,     1,     1,     1,     1,     1,     1,
       1,     1,     1,     1,     1,     1,     1,     1,     1,     1,
       1,     1,     1,     1,     1,     1,     1,     1,     1,     1,
       1,     4,     8,     1,     3,     3,     1,     1,     1,     3,
       3,     3,     3,     3,     1,     1,     1,     4,     4,     7,
       1,     3,     4,     2,     0,     2
};


#define yyerrok         (yyerrstatus = 0)
#define yyclearin       (yychar = YYEMPTY)
#define YYEMPTY         (-2)
#define YYEOF           0

#define YYACCEPT        goto yyacceptlab
#define YYABORT         goto yyabortlab
#define YYERROR         goto yyerrorlab


#define YYRECOVERING()  (!!yyerrstatus)

#define YYBACKUP(Token, Value)                                  \
do                                                              \
  if (yychar == YYEMPTY)                                        \
    {                                                           \
      yychar = (Token);                                         \
      yylval = (Value);                                         \
      YYPOPSTACK (yylen);                                       \
      yystate = *yyssp;                                         \
      goto yybackup;                                            \
    }                                                           \
  else                                                          \
    {                                                           \
      yyerror (&yylloc, plexer, db, YY_("syntax error: cannot back up")); \
      YYERROR;                                                  \
    }                                                           \
while (0)

/* Error token number */
#define YYTERROR        1
#define YYERRCODE       256


/* YYLLOC_DEFAULT -- Set CURRENT to span from RHS[1] to RHS[N].
   If N is 0, then set CURRENT to the empty location which ends
   the previous symbol: RHS[0] (always defined).  */

#ifndef YYLLOC_DEFAULT
# define YYLLOC_DEFAULT(Current, Rhs, N)                                \
    do                                                                  \
      if (N)                                                            \
        {                                                               \
          (Current).first_line   = YYRHSLOC (Rhs, 1).first_line;        \
          (Current).first_column = YYRHSLOC (Rhs, 1).first_column;      \
          (Current).last_line    = YYRHSLOC (Rhs, N).last_line;         \
          (Current).last_column  = YYRHSLOC (Rhs, N).last_column;       \
        }                                                               \
      else                                                              \
        {                                                               \
          (Current).first_line   = (Current).last_line   =              \
            YYRHSLOC (Rhs, 0).last_line;                                \
          (Current).first_column = (Current).last_column =              \
            YYRHSLOC (Rhs, 0).last_column;                              \
        }                                                               \
    while (0)
#endif

#define YYRHSLOC(Rhs, K) ((Rhs)[K])


/* Enable debugging if requested.  */
#if YYDEBUG

# ifndef YYFPRINTF
#  include <stdio.h> /* INFRINGES ON USER NAME SPACE */
#  define YYFPRINTF fprintf
# endif

# define YYDPRINTF(Args)                        \
do {                                            \
  if (yydebug)                                  \
    YYFPRINTF Args;                             \
} while (0)


/* YY_LOCATION_PRINT -- Print the location on the stream.
   This macro was not mandated originally: define only if we know
   we won't break user code: when these are the locations we know.  */

#ifndef YY_LOCATION_PRINT
# if defined YYLTYPE_IS_TRIVIAL && YYLTYPE_IS_TRIVIAL

/* Print *YYLOCP on YYO.  Private, do not rely on its existence. */

YY_ATTRIBUTE_UNUSED
static unsigned
yy_location_print_ (FILE *yyo, YYLTYPE const * const yylocp)
{
  unsigned res = 0;
  int end_col = 0 != yylocp->last_column ? yylocp->last_column - 1 : 0;
  if (0 <= yylocp->first_line)
    {
      res += YYFPRINTF (yyo, "%d", yylocp->first_line);
      if (0 <= yylocp->first_column)
        res += YYFPRINTF (yyo, ".%d", yylocp->first_column);
    }
  if (0 <= yylocp->last_line)
    {
      if (yylocp->first_line < yylocp->last_line)
        {
          res += YYFPRINTF (yyo, "-%d", yylocp->last_line);
          if (0 <= end_col)
            res += YYFPRINTF (yyo, ".%d", end_col);
        }
      else if (0 <= end_col && yylocp->first_column < end_col)
        res += YYFPRINTF (yyo, "-%d", end_col);
    }
  return res;
 }

#  define YY_LOCATION_PRINT(File, Loc)          \
  yy_location_print_ (File, &(Loc))

# else
#  define YY_LOCATION_PRINT(File, Loc) ((void) 0)
# endif
#endif


# define YY_SYMBOL_PRINT(Title, Type, Value, Location)                    \
do {                                                                      \
  if (yydebug)                                                            \
    {                                                                     \
      YYFPRINTF (stderr, "%s ", Title);                                   \
      yy_symbol_print (stderr,                                            \
                  Type, Value, Location, plexer, db); \
      YYFPRINTF (stderr, "\n");                                           \
    }                                                                     \
} while (0)


/*----------------------------------------.
| Print this symbol's value on YYOUTPUT.  |
`----------------------------------------*/

static void
yy_symbol_value_print (FILE *yyoutput, int yytype, YYSTYPE const * const yyvaluep, YYLTYPE const * const yylocationp, class yyFlexLexer* plexer, class FrameIOParserDb* db)
{
  FILE *yyo = yyoutput;
  YYUSE (yyo);
  YYUSE (yylocationp);
  YYUSE (plexer);
  YYUSE (db);
  if (!yyvaluep)
    return;
# ifdef YYPRINT
  if (yytype < YYNTOKENS)
    YYPRINT (yyoutput, yytoknum[yytype], *yyvaluep);
# endif
  YYUSE (yytype);
}


/*--------------------------------.
| Print this symbol on YYOUTPUT.  |
`--------------------------------*/

static void
yy_symbol_print (FILE *yyoutput, int yytype, YYSTYPE const * const yyvaluep, YYLTYPE const * const yylocationp, class yyFlexLexer* plexer, class FrameIOParserDb* db)
{
  YYFPRINTF (yyoutput, "%s %s (",
             yytype < YYNTOKENS ? "token" : "nterm", yytname[yytype]);

  YY_LOCATION_PRINT (yyoutput, *yylocationp);
  YYFPRINTF (yyoutput, ": ");
  yy_symbol_value_print (yyoutput, yytype, yyvaluep, yylocationp, plexer, db);
  YYFPRINTF (yyoutput, ")");
}

/*------------------------------------------------------------------.
| yy_stack_print -- Print the state stack from its BOTTOM up to its |
| TOP (included).                                                   |
`------------------------------------------------------------------*/

static void
yy_stack_print (yytype_int16 *yybottom, yytype_int16 *yytop)
{
  YYFPRINTF (stderr, "Stack now");
  for (; yybottom <= yytop; yybottom++)
    {
      int yybot = *yybottom;
      YYFPRINTF (stderr, " %d", yybot);
    }
  YYFPRINTF (stderr, "\n");
}

# define YY_STACK_PRINT(Bottom, Top)                            \
do {                                                            \
  if (yydebug)                                                  \
    yy_stack_print ((Bottom), (Top));                           \
} while (0)


/*------------------------------------------------.
| Report that the YYRULE is going to be reduced.  |
`------------------------------------------------*/

static void
yy_reduce_print (yytype_int16 *yyssp, YYSTYPE *yyvsp, YYLTYPE *yylsp, int yyrule, class yyFlexLexer* plexer, class FrameIOParserDb* db)
{
  unsigned long yylno = yyrline[yyrule];
  int yynrhs = yyr2[yyrule];
  int yyi;
  YYFPRINTF (stderr, "Reducing stack by rule %d (line %lu):\n",
             yyrule - 1, yylno);
  /* The symbols being reduced.  */
  for (yyi = 0; yyi < yynrhs; yyi++)
    {
      YYFPRINTF (stderr, "   $%d = ", yyi + 1);
      yy_symbol_print (stderr,
                       yystos[yyssp[yyi + 1 - yynrhs]],
                       &(yyvsp[(yyi + 1) - (yynrhs)])
                       , &(yylsp[(yyi + 1) - (yynrhs)])                       , plexer, db);
      YYFPRINTF (stderr, "\n");
    }
}

# define YY_REDUCE_PRINT(Rule)          \
do {                                    \
  if (yydebug)                          \
    yy_reduce_print (yyssp, yyvsp, yylsp, Rule, plexer, db); \
} while (0)

/* Nonzero means print parse trace.  It is left uninitialized so that
   multiple parsers can coexist.  */
int yydebug;
#else /* !YYDEBUG */
# define YYDPRINTF(Args)
# define YY_SYMBOL_PRINT(Title, Type, Value, Location)
# define YY_STACK_PRINT(Bottom, Top)
# define YY_REDUCE_PRINT(Rule)
#endif /* !YYDEBUG */


/* YYINITDEPTH -- initial size of the parser's stacks.  */
#ifndef YYINITDEPTH
# define YYINITDEPTH 200
#endif

/* YYMAXDEPTH -- maximum size the stacks can grow to (effective only
   if the built-in stack extension method is used).

   Do not make this value too large; the results are undefined if
   YYSTACK_ALLOC_MAXIMUM < YYSTACK_BYTES (YYMAXDEPTH)
   evaluated with infinite-precision integer arithmetic.  */

#ifndef YYMAXDEPTH
# define YYMAXDEPTH 10000
#endif


#if YYERROR_VERBOSE

# ifndef yystrlen
#  if defined __GLIBC__ && defined _STRING_H
#   define yystrlen strlen
#  else
/* Return the length of YYSTR.  */
static YYSIZE_T
yystrlen (const char *yystr)
{
  YYSIZE_T yylen;
  for (yylen = 0; yystr[yylen]; yylen++)
    continue;
  return yylen;
}
#  endif
# endif

# ifndef yystpcpy
#  if defined __GLIBC__ && defined _STRING_H && defined _GNU_SOURCE
#   define yystpcpy stpcpy
#  else
/* Copy YYSRC to YYDEST, returning the address of the terminating '\0' in
   YYDEST.  */
static char *
yystpcpy (char *yydest, const char *yysrc)
{
  char *yyd = yydest;
  const char *yys = yysrc;

  while ((*yyd++ = *yys++) != '\0')
    continue;

  return yyd - 1;
}
#  endif
# endif

# ifndef yytnamerr
/* Copy to YYRES the contents of YYSTR after stripping away unnecessary
   quotes and backslashes, so that it's suitable for yyerror.  The
   heuristic is that double-quoting is unnecessary unless the string
   contains an apostrophe, a comma, or backslash (other than
   backslash-backslash).  YYSTR is taken from yytname.  If YYRES is
   null, do not copy; instead, return the length of what the result
   would have been.  */
static YYSIZE_T
yytnamerr (char *yyres, const char *yystr)
{
  if (*yystr == '"')
    {
      YYSIZE_T yyn = 0;
      char const *yyp = yystr;

      for (;;)
        switch (*++yyp)
          {
          case '\'':
          case ',':
            goto do_not_strip_quotes;

          case '\\':
            if (*++yyp != '\\')
              goto do_not_strip_quotes;
            /* Fall through.  */
          default:
            if (yyres)
              yyres[yyn] = *yyp;
            yyn++;
            break;

          case '"':
            if (yyres)
              yyres[yyn] = '\0';
            return yyn;
          }
    do_not_strip_quotes: ;
    }

  if (! yyres)
    return yystrlen (yystr);

  return yystpcpy (yyres, yystr) - yyres;
}
# endif

/* Copy into *YYMSG, which is of size *YYMSG_ALLOC, an error message
   about the unexpected token YYTOKEN for the state stack whose top is
   YYSSP.

   Return 0 if *YYMSG was successfully written.  Return 1 if *YYMSG is
   not large enough to hold the message.  In that case, also set
   *YYMSG_ALLOC to the required number of bytes.  Return 2 if the
   required number of bytes is too large to store.  */
static int
yysyntax_error (YYSIZE_T *yymsg_alloc, char **yymsg,
                yytype_int16 *yyssp, int yytoken)
{
  YYSIZE_T yysize0 = yytnamerr (YY_NULLPTR, yytname[yytoken]);
  YYSIZE_T yysize = yysize0;
  enum { YYERROR_VERBOSE_ARGS_MAXIMUM = 5 };
  /* Internationalized format string. */
  const char *yyformat = YY_NULLPTR;
  /* Arguments of yyformat. */
  char const *yyarg[YYERROR_VERBOSE_ARGS_MAXIMUM];
  /* Number of reported tokens (one for the "unexpected", one per
     "expected"). */
  int yycount = 0;

  /* There are many possibilities here to consider:
     - If this state is a consistent state with a default action, then
       the only way this function was invoked is if the default action
       is an error action.  In that case, don't check for expected
       tokens because there are none.
     - The only way there can be no lookahead present (in yychar) is if
       this state is a consistent state with a default action.  Thus,
       detecting the absence of a lookahead is sufficient to determine
       that there is no unexpected or expected token to report.  In that
       case, just report a simple "syntax error".
     - Don't assume there isn't a lookahead just because this state is a
       consistent state with a default action.  There might have been a
       previous inconsistent state, consistent state with a non-default
       action, or user semantic action that manipulated yychar.
     - Of course, the expected token list depends on states to have
       correct lookahead information, and it depends on the parser not
       to perform extra reductions after fetching a lookahead from the
       scanner and before detecting a syntax error.  Thus, state merging
       (from LALR or IELR) and default reductions corrupt the expected
       token list.  However, the list is correct for canonical LR with
       one exception: it will still contain any token that will not be
       accepted due to an error action in a later state.
  */
  if (yytoken != YYEMPTY)
    {
      int yyn = yypact[*yyssp];
      yyarg[yycount++] = yytname[yytoken];
      if (!yypact_value_is_default (yyn))
        {
          /* Start YYX at -YYN if negative to avoid negative indexes in
             YYCHECK.  In other words, skip the first -YYN actions for
             this state because they are default actions.  */
          int yyxbegin = yyn < 0 ? -yyn : 0;
          /* Stay within bounds of both yycheck and yytname.  */
          int yychecklim = YYLAST - yyn + 1;
          int yyxend = yychecklim < YYNTOKENS ? yychecklim : YYNTOKENS;
          int yyx;

          for (yyx = yyxbegin; yyx < yyxend; ++yyx)
            if (yycheck[yyx + yyn] == yyx && yyx != YYTERROR
                && !yytable_value_is_error (yytable[yyx + yyn]))
              {
                if (yycount == YYERROR_VERBOSE_ARGS_MAXIMUM)
                  {
                    yycount = 1;
                    yysize = yysize0;
                    break;
                  }
                yyarg[yycount++] = yytname[yyx];
                {
                  YYSIZE_T yysize1 = yysize + yytnamerr (YY_NULLPTR, yytname[yyx]);
                  if (! (yysize <= yysize1
                         && yysize1 <= YYSTACK_ALLOC_MAXIMUM))
                    return 2;
                  yysize = yysize1;
                }
              }
        }
    }

  switch (yycount)
    {
# define YYCASE_(N, S)                      \
      case N:                               \
        yyformat = S;                       \
      break
    default: /* Avoid compiler warnings. */
      YYCASE_(0, YY_("syntax error"));
      YYCASE_(1, YY_("syntax error, unexpected %s"));
      YYCASE_(2, YY_("syntax error, unexpected %s, expecting %s"));
      YYCASE_(3, YY_("syntax error, unexpected %s, expecting %s or %s"));
      YYCASE_(4, YY_("syntax error, unexpected %s, expecting %s or %s or %s"));
      YYCASE_(5, YY_("syntax error, unexpected %s, expecting %s or %s or %s or %s"));
# undef YYCASE_
    }

  {
    YYSIZE_T yysize1 = yysize + yystrlen (yyformat);
    if (! (yysize <= yysize1 && yysize1 <= YYSTACK_ALLOC_MAXIMUM))
      return 2;
    yysize = yysize1;
  }

  if (*yymsg_alloc < yysize)
    {
      *yymsg_alloc = 2 * yysize;
      if (! (yysize <= *yymsg_alloc
             && *yymsg_alloc <= YYSTACK_ALLOC_MAXIMUM))
        *yymsg_alloc = YYSTACK_ALLOC_MAXIMUM;
      return 1;
    }

  /* Avoid sprintf, as that infringes on the user's name space.
     Don't have undefined behavior even if the translation
     produced a string with the wrong number of "%s"s.  */
  {
    char *yyp = *yymsg;
    int yyi = 0;
    while ((*yyp = *yyformat) != '\0')
      if (*yyp == '%' && yyformat[1] == 's' && yyi < yycount)
        {
          yyp += yytnamerr (yyp, yyarg[yyi++]);
          yyformat += 2;
        }
      else
        {
          yyp++;
          yyformat++;
        }
  }
  return 0;
}
#endif /* YYERROR_VERBOSE */

/*-----------------------------------------------.
| Release the memory associated to this symbol.  |
`-----------------------------------------------*/

static void
yydestruct (const char *yymsg, int yytype, YYSTYPE *yyvaluep, YYLTYPE *yylocationp, class yyFlexLexer* plexer, class FrameIOParserDb* db)
{
  YYUSE (yyvaluep);
  YYUSE (yylocationp);
  YYUSE (plexer);
  YYUSE (db);
  if (!yymsg)
    yymsg = "Deleting";
  YY_SYMBOL_PRINT (yymsg, yytype, yyvaluep, yylocationp);

  YY_IGNORE_MAYBE_UNINITIALIZED_BEGIN
  YYUSE (yytype);
  YY_IGNORE_MAYBE_UNINITIALIZED_END
}




/*----------.
| yyparse.  |
`----------*/

int
yyparse (class yyFlexLexer* plexer, class FrameIOParserDb* db)
{
/* The lookahead symbol.  */
int yychar;


/* The semantic value of the lookahead symbol.  */
/* Default value used for initialization, for pacifying older GCCs
   or non-GCC compilers.  */
YY_INITIAL_VALUE (static YYSTYPE yyval_default;)
YYSTYPE yylval YY_INITIAL_VALUE (= yyval_default);

/* Location data for the lookahead symbol.  */
static YYLTYPE yyloc_default
# if defined YYLTYPE_IS_TRIVIAL && YYLTYPE_IS_TRIVIAL
  = { 1, 1, 1, 1 }
# endif
;
YYLTYPE yylloc = yyloc_default;

    /* Number of syntax errors so far.  */
    int yynerrs;

    int yystate;
    /* Number of tokens to shift before error messages enabled.  */
    int yyerrstatus;

    /* The stacks and their tools:
       'yyss': related to states.
       'yyvs': related to semantic values.
       'yyls': related to locations.

       Refer to the stacks through separate pointers, to allow yyoverflow
       to reallocate them elsewhere.  */

    /* The state stack.  */
    yytype_int16 yyssa[YYINITDEPTH];
    yytype_int16 *yyss;
    yytype_int16 *yyssp;

    /* The semantic value stack.  */
    YYSTYPE yyvsa[YYINITDEPTH];
    YYSTYPE *yyvs;
    YYSTYPE *yyvsp;

    /* The location stack.  */
    YYLTYPE yylsa[YYINITDEPTH];
    YYLTYPE *yyls;
    YYLTYPE *yylsp;

    /* The locations where the error started and ended.  */
    YYLTYPE yyerror_range[3];

    YYSIZE_T yystacksize;

  int yyn;
  int yyresult;
  /* Lookahead token as an internal (translated) token number.  */
  int yytoken = 0;
  /* The variables used to return semantic value and location from the
     action routines.  */
  YYSTYPE yyval;
  YYLTYPE yyloc;

#if YYERROR_VERBOSE
  /* Buffer for error messages, and its allocated size.  */
  char yymsgbuf[128];
  char *yymsg = yymsgbuf;
  YYSIZE_T yymsg_alloc = sizeof yymsgbuf;
#endif

#define YYPOPSTACK(N)   (yyvsp -= (N), yyssp -= (N), yylsp -= (N))

  /* The number of symbols on the RHS of the reduced rule.
     Keep to zero when no symbol should be popped.  */
  int yylen = 0;

  yyssp = yyss = yyssa;
  yyvsp = yyvs = yyvsa;
  yylsp = yyls = yylsa;
  yystacksize = YYINITDEPTH;

  YYDPRINTF ((stderr, "Starting parse\n"));

  yystate = 0;
  yyerrstatus = 0;
  yynerrs = 0;
  yychar = YYEMPTY; /* Cause a token to be read.  */
  yylsp[0] = yylloc;
  goto yysetstate;

/*------------------------------------------------------------.
| yynewstate -- Push a new state, which is found in yystate.  |
`------------------------------------------------------------*/
 yynewstate:
  /* In all cases, when you get here, the value and location stacks
     have just been pushed.  So pushing a state here evens the stacks.  */
  yyssp++;

 yysetstate:
  *yyssp = yystate;

  if (yyss + yystacksize - 1 <= yyssp)
    {
      /* Get the current used size of the three stacks, in elements.  */
      YYSIZE_T yysize = yyssp - yyss + 1;

#ifdef yyoverflow
      {
        /* Give user a chance to reallocate the stack.  Use copies of
           these so that the &'s don't force the real ones into
           memory.  */
        YYSTYPE *yyvs1 = yyvs;
        yytype_int16 *yyss1 = yyss;
        YYLTYPE *yyls1 = yyls;

        /* Each stack pointer address is followed by the size of the
           data in use in that stack, in bytes.  This used to be a
           conditional around just the two extra args, but that might
           be undefined if yyoverflow is a macro.  */
        yyoverflow (YY_("memory exhausted"),
                    &yyss1, yysize * sizeof (*yyssp),
                    &yyvs1, yysize * sizeof (*yyvsp),
                    &yyls1, yysize * sizeof (*yylsp),
                    &yystacksize);

        yyls = yyls1;
        yyss = yyss1;
        yyvs = yyvs1;
      }
#else /* no yyoverflow */
# ifndef YYSTACK_RELOCATE
      goto yyexhaustedlab;
# else
      /* Extend the stack our own way.  */
      if (YYMAXDEPTH <= yystacksize)
        goto yyexhaustedlab;
      yystacksize *= 2;
      if (YYMAXDEPTH < yystacksize)
        yystacksize = YYMAXDEPTH;

      {
        yytype_int16 *yyss1 = yyss;
        union yyalloc *yyptr =
          (union yyalloc *) YYSTACK_ALLOC (YYSTACK_BYTES (yystacksize));
        if (! yyptr)
          goto yyexhaustedlab;
        YYSTACK_RELOCATE (yyss_alloc, yyss);
        YYSTACK_RELOCATE (yyvs_alloc, yyvs);
        YYSTACK_RELOCATE (yyls_alloc, yyls);
#  undef YYSTACK_RELOCATE
        if (yyss1 != yyssa)
          YYSTACK_FREE (yyss1);
      }
# endif
#endif /* no yyoverflow */

      yyssp = yyss + yysize - 1;
      yyvsp = yyvs + yysize - 1;
      yylsp = yyls + yysize - 1;

      YYDPRINTF ((stderr, "Stack size increased to %lu\n",
                  (unsigned long) yystacksize));

      if (yyss + yystacksize - 1 <= yyssp)
        YYABORT;
    }

  YYDPRINTF ((stderr, "Entering state %d\n", yystate));

  if (yystate == YYFINAL)
    YYACCEPT;

  goto yybackup;

/*-----------.
| yybackup.  |
`-----------*/
yybackup:

  /* Do appropriate processing given the current state.  Read a
     lookahead token if we need one and don't already have one.  */

  /* First try to decide what to do without reference to lookahead token.  */
  yyn = yypact[yystate];
  if (yypact_value_is_default (yyn))
    goto yydefault;

  /* Not known => get a lookahead token if don't already have one.  */

  /* YYCHAR is either YYEMPTY or YYEOF or a valid lookahead symbol.  */
  if (yychar == YYEMPTY)
    {
      YYDPRINTF ((stderr, "Reading a token: "));
      yychar = yylex (&yylval, &yylloc, db);
    }

  if (yychar <= YYEOF)
    {
      yychar = yytoken = YYEOF;
      YYDPRINTF ((stderr, "Now at end of input.\n"));
    }
  else
    {
      yytoken = YYTRANSLATE (yychar);
      YY_SYMBOL_PRINT ("Next token is", yytoken, &yylval, &yylloc);
    }

  /* If the proper action on seeing token YYTOKEN is to reduce or to
     detect an error, take that action.  */
  yyn += yytoken;
  if (yyn < 0 || YYLAST < yyn || yycheck[yyn] != yytoken)
    goto yydefault;
  yyn = yytable[yyn];
  if (yyn <= 0)
    {
      if (yytable_value_is_error (yyn))
        goto yyerrlab;
      yyn = -yyn;
      goto yyreduce;
    }

  /* Count tokens shifted since error; after three, turn off error
     status.  */
  if (yyerrstatus)
    yyerrstatus--;

  /* Shift the lookahead token.  */
  YY_SYMBOL_PRINT ("Shifting", yytoken, &yylval, &yylloc);

  /* Discard the shifted token.  */
  yychar = YYEMPTY;

  yystate = yyn;
  YY_IGNORE_MAYBE_UNINITIALIZED_BEGIN
  *++yyvsp = yylval;
  YY_IGNORE_MAYBE_UNINITIALIZED_END
  *++yylsp = yylloc;
  goto yynewstate;


/*-----------------------------------------------------------.
| yydefault -- do the default action for the current state.  |
`-----------------------------------------------------------*/
yydefault:
  yyn = yydefact[yystate];
  if (yyn == 0)
    goto yyerrlab;
  goto yyreduce;


/*-----------------------------.
| yyreduce -- Do a reduction.  |
`-----------------------------*/
yyreduce:
  /* yyn is the number of a rule to reduce with.  */
  yylen = yyr2[yyn];

  /* If YYLEN is nonzero, implement the default value of the action:
     '$$ = $1'.

     Otherwise, the following line sets YYVAL to garbage.
     This behavior is undocumented and Bison
     users should not rely upon it.  Assigning to YYVAL
     unconditionally makes the parser a bit smaller, and it avoids a
     GCC warning that YYVAL may be used uninitialized.  */
  yyval = yyvsp[1-yylen];

  /* Default location. */
  YYLLOC_DEFAULT (yyloc, (yylsp - yylen), yylen);
  yyerror_range[1] = yyloc;
  YY_REDUCE_PRINT (yyn);
  switch (yyn)
    {
        case 2:
#line 107 "fparser.y" /* yacc.c:1651  */
    { (yyval.project) = new_project((yyvsp[-5].symbol), (yyvsp[-3].pitemlist), (yyvsp[-7].notelist)); }
#line 1671 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 3:
#line 110 "fparser.y" /* yacc.c:1651  */
    { (yyval.pitemlist) = NULL; }
#line 1677 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 4:
#line 111 "fparser.y" /* yacc.c:1651  */
    { (yyval.pitemlist) = add_projectitem((yyvsp[-1].pitemlist), (yyvsp[0].pitem)); }
#line 1683 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 5:
#line 115 "fparser.y" /* yacc.c:1651  */
    { (yyval.pitem) = (yyvsp[0].pitem); }
#line 1689 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 6:
#line 116 "fparser.y" /* yacc.c:1651  */
    { (yyval.pitem) = (yyvsp[0].pitem); }
#line 1695 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 7:
#line 117 "fparser.y" /* yacc.c:1651  */
    { (yyval.pitem) = (yyvsp[0].pitem); }
#line 1701 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 8:
#line 121 "fparser.y" /* yacc.c:1651  */
    { (yyval.pitem) = new_projectitem(PI_SYSTEM, new_sys((yyvsp[-4].symbol), (yyvsp[-2].sysitemlist), (yyvsp[-6].notelist))); }
#line 1707 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 9:
#line 124 "fparser.y" /* yacc.c:1651  */
    { (yyval.sysitemlist) = NULL; }
#line 1713 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 10:
#line 125 "fparser.y" /* yacc.c:1651  */
    { (yyval.sysitemlist) = add_sysitem((yyvsp[-1].sysitemlist), (yyvsp[0].sysitem)); }
#line 1719 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 11:
#line 129 "fparser.y" /* yacc.c:1651  */
    { (yyval.sysitem) = (yyvsp[0].sysitem); }
#line 1725 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 12:
#line 130 "fparser.y" /* yacc.c:1651  */
    { (yyval.sysitem) = (yyvsp[0].sysitem); }
#line 1731 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 13:
#line 131 "fparser.y" /* yacc.c:1651  */
    { (yyval.sysitem) = (yyvsp[0].sysitem); }
#line 1737 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 14:
#line 135 "fparser.y" /* yacc.c:1651  */
    { (yyval.sysitem) = new_sysitem(SYSI_PROPERTY, new_sysproperty((yyvsp[-1].symbol), (yyvsp[-2].sysptype), FALSE, (yyvsp[-3].notelist))); }
#line 1743 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 15:
#line 136 "fparser.y" /* yacc.c:1651  */
    { (yyval.sysitem) = new_sysitem(SYSI_PROPERTY, new_sysproperty((yyvsp[-1].symbol), (yyvsp[-4].sysptype), TRUE, (yyvsp[-5].notelist))); }
#line 1749 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 16:
#line 140 "fparser.y" /* yacc.c:1651  */
    { (yyval.sysptype) = SYSPT_BOOL; }
#line 1755 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 17:
#line 141 "fparser.y" /* yacc.c:1651  */
    { (yyval.sysptype) = SYSPT_BYTE; }
#line 1761 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 18:
#line 142 "fparser.y" /* yacc.c:1651  */
    { (yyval.sysptype) = SYSPT_SBYTE; }
#line 1767 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 19:
#line 143 "fparser.y" /* yacc.c:1651  */
    { (yyval.sysptype) = SYSPT_USHORT; }
#line 1773 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 20:
#line 144 "fparser.y" /* yacc.c:1651  */
    { (yyval.sysptype) = SYSPT_SHORT; }
#line 1779 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 21:
#line 145 "fparser.y" /* yacc.c:1651  */
    { (yyval.sysptype) = SYSPT_UINT; }
#line 1785 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 22:
#line 146 "fparser.y" /* yacc.c:1651  */
    { (yyval.sysptype) = SYSPT_INT; }
#line 1791 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 23:
#line 147 "fparser.y" /* yacc.c:1651  */
    { (yyval.sysptype) = SYSPT_ULONG; }
#line 1797 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 24:
#line 148 "fparser.y" /* yacc.c:1651  */
    { (yyval.sysptype) = SYSPT_LONG; }
#line 1803 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 25:
#line 149 "fparser.y" /* yacc.c:1651  */
    { (yyval.sysptype) = SYSPT_FLOAT; }
#line 1809 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 26:
#line 150 "fparser.y" /* yacc.c:1651  */
    { (yyval.sysptype) = SYSPT_DOUBLE; }
#line 1815 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 27:
#line 154 "fparser.y" /* yacc.c:1651  */
    { (yyval.sysitem) = new_sysitem(SYSI_CHANNEL, new_syschannel((yyvsp[-6].symbol), (yyvsp[-4].syschtype), (yyvsp[-2].choplist), (yyvsp[-8].notelist))); }
#line 1821 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 28:
#line 158 "fparser.y" /* yacc.c:1651  */
    { (yyval.syschtype) = SCHT_COM; }
#line 1827 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 29:
#line 159 "fparser.y" /* yacc.c:1651  */
    { (yyval.syschtype) = SCHT_CAN; }
#line 1833 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 30:
#line 160 "fparser.y" /* yacc.c:1651  */
    { (yyval.syschtype) = SCHT_TCPSERVER; }
#line 1839 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 31:
#line 161 "fparser.y" /* yacc.c:1651  */
    { (yyval.syschtype) = SCHT_TCPCLIENT; }
#line 1845 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 32:
#line 162 "fparser.y" /* yacc.c:1651  */
    { (yyval.syschtype) = SCHT_UDP; }
#line 1851 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 33:
#line 163 "fparser.y" /* yacc.c:1651  */
    { (yyval.syschtype) = SCHT_DI; }
#line 1857 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 34:
#line 164 "fparser.y" /* yacc.c:1651  */
    { (yyval.syschtype) = SCHT_DO; }
#line 1863 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 35:
#line 167 "fparser.y" /* yacc.c:1651  */
    { (yyval.choplist) = NULL; }
#line 1869 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 36:
#line 168 "fparser.y" /* yacc.c:1651  */
    { (yyval.choplist) = append_channeloption((yyvsp[-1].choplist), (yyvsp[0].choplist)); }
#line 1875 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 37:
#line 172 "fparser.y" /* yacc.c:1651  */
    { (yyval.choplist) = new_channeloption((yyvsp[-3].choptype), (yyvsp[-1].optionvalue), (yyvsp[-4].notelist)); }
#line 1881 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 38:
#line 176 "fparser.y" /* yacc.c:1651  */
    { (yyval.choptype) = CHOP_DEVICEID; }
#line 1887 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 39:
#line 177 "fparser.y" /* yacc.c:1651  */
    { (yyval.choptype) = CHOP_BAUDRATE; }
#line 1893 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 40:
#line 181 "fparser.y" /* yacc.c:1651  */
    { (yyval.optionvalue) = (yyvsp[0].symbol); }
#line 1899 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 41:
#line 182 "fparser.y" /* yacc.c:1651  */
    { (yyval.optionvalue) = (yyvsp[0].symbol); }
#line 1905 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 42:
#line 183 "fparser.y" /* yacc.c:1651  */
    { (yyval.optionvalue) = (yyvsp[0].symbol); }
#line 1911 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 43:
#line 187 "fparser.y" /* yacc.c:1651  */
    { (yyval.sysitem) = new_sysitem(SYSI_ACTION, new_action((yyvsp[-9].symbol), (yyvsp[-7].iotype), (yyvsp[-6].symbol), (yyvsp[-4].symbol), (yyvsp[-2].amaplist), (yyvsp[-11].notelist))); }
#line 1917 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 44:
#line 191 "fparser.y" /* yacc.c:1651  */
    { (yyval.iotype) = AIO_SEND; }
#line 1923 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 45:
#line 192 "fparser.y" /* yacc.c:1651  */
    { (yyval.iotype) = AIO_RECV; }
#line 1929 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 46:
#line 193 "fparser.y" /* yacc.c:1651  */
    { (yyval.iotype) = AIO_RECVLOOP; }
#line 1935 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 47:
#line 196 "fparser.y" /* yacc.c:1651  */
    { (yyval.amaplist) = NULL; }
#line 1941 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 48:
#line 197 "fparser.y" /* yacc.c:1651  */
    { (yyval.amaplist) = append_actionmap((yyvsp[-1].amaplist), (yyvsp[0].amaplist)); }
#line 1947 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 49:
#line 201 "fparser.y" /* yacc.c:1651  */
    { (yyval.amaplist) = new_actionmap((yyvsp[-3].symbol), (yyvsp[-1].symbol), (yyvsp[-4].notelist)); }
#line 1953 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 50:
#line 205 "fparser.y" /* yacc.c:1651  */
    { (yyval.pitem) = new_projectitem(PI_FRAME, new_frame((yyvsp[-4].symbol), (yyvsp[-2].seglist), (yyvsp[-6].notelist))); }
#line 1959 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 51:
#line 208 "fparser.y" /* yacc.c:1651  */
    { (yyval.seglist) = NULL; }
#line 1965 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 52:
#line 209 "fparser.y" /* yacc.c:1651  */
    { (yyval.seglist) = append_segment((yyvsp[-1].seglist), (yyvsp[0].seglist)); }
#line 1971 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 53:
#line 213 "fparser.y" /* yacc.c:1651  */
    { (yyval.seglist) = new_segment((yyvsp[-3].segtype), (yyvsp[-2].symbol), (yyvsp[-1].segprolist), (yyvsp[-4].notelist)); }
#line 1977 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 54:
#line 217 "fparser.y" /* yacc.c:1651  */
    { (yyval.segtype) = SEGT_INTEGER; }
#line 1983 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 55:
#line 218 "fparser.y" /* yacc.c:1651  */
    { (yyval.segtype) = SEGT_REAL; }
#line 1989 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 56:
#line 219 "fparser.y" /* yacc.c:1651  */
    { (yyval.segtype) = SEGT_BLOCK; }
#line 1995 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 57:
#line 220 "fparser.y" /* yacc.c:1651  */
    { (yyval.segtype) = SEGT_TEXT; }
#line 2001 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 58:
#line 223 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprolist) = NULL; }
#line 2007 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 59:
#line 224 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprolist) = append_segproperty((yyvsp[-1].segprolist), (yyvsp[0].segprolist)); }
#line 2013 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 60:
#line 228 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprolist) = new_segproperty((yyvsp[-2].segproptype), (yyvsp[0].segprovtype)); }
#line 2019 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 61:
#line 229 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprolist) = new_segproperty((yyvsp[-2].segproptype), SEGPV_INT, (yyvsp[0].symbol)); }
#line 2025 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 62:
#line 230 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprolist) = new_segproperty((yyvsp[-2].segproptype), SEGPV_INT, (yyvsp[0].symbol)); }
#line 2031 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 63:
#line 231 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprolist) = new_segproperty((yyvsp[-2].segproptype), SEGPV_REAL, (yyvsp[0].symbol)); }
#line 2037 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 64:
#line 232 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprolist) = new_segproperty(SEGP_BYTEORDER, (yyvsp[0].segprovtype)); }
#line 2043 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 65:
#line 233 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprolist) = new_segproperty(SEGP_ENCODED, (yyvsp[0].segprovtype)); }
#line 2049 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 66:
#line 234 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprolist) = new_segproperty(SEGP_ENCODED, (yyvsp[0].segprovtype)); }
#line 2055 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 67:
#line 235 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprolist) = append_segproperty(new_segproperty(SEGP_CHECKRANGE_BEGIN, SEGPV_ID, (yyvsp[-3].symbol)), new_segproperty(SEGP_CHECKRANGE_END, SEGPV_ID, (yyvsp[-1].symbol))); }
#line 2061 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 68:
#line 236 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprolist) = new_segproperty(SEGP_TOENUM, SEGPV_STRING, (yyvsp[0].symbol)); }
#line 2067 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 69:
#line 237 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprolist) = new_segproperty(SEGP_TAIL, SEGPV_STRING, (yyvsp[0].symbol)); }
#line 2073 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 70:
#line 238 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprolist) = (yyvsp[0].segprolist); }
#line 2079 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 71:
#line 239 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprolist) = new_segproperty((yyvsp[-2].segproptype), SEGPV_EXP, -1, (yyvsp[0].valueexp)); }
#line 2085 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 72:
#line 244 "fparser.y" /* yacc.c:1651  */
    { (yyval.segproptype) = SEGP_ISDOUBLE; }
#line 2091 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 73:
#line 245 "fparser.y" /* yacc.c:1651  */
    { (yyval.segproptype) = SEGP_SIGNED; }
#line 2097 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 74:
#line 249 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprovtype) = SEGPV_TRUE; }
#line 2103 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 75:
#line 250 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprovtype) = SEGPV_FALSE; }
#line 2109 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 76:
#line 254 "fparser.y" /* yacc.c:1651  */
    { (yyval.segproptype) = SEGP_BITCOUNT; }
#line 2115 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 77:
#line 255 "fparser.y" /* yacc.c:1651  */
    { (yyval.segproptype) = SEGP_ALIGNEDLEN; }
#line 2121 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 78:
#line 259 "fparser.y" /* yacc.c:1651  */
    { (yyval.segproptype) = SEGP_MAX; }
#line 2127 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 79:
#line 260 "fparser.y" /* yacc.c:1651  */
    { (yyval.segproptype) = SEGP_MIN; }
#line 2133 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 80:
#line 265 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprovtype) = SEGPV_SMALL; }
#line 2139 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 81:
#line 266 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprovtype) = SEGPV_BIG; }
#line 2145 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 82:
#line 270 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprovtype) = SEGPV_PRIMITIVE; }
#line 2151 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 83:
#line 271 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprovtype) = SEGPV_INVERSION; }
#line 2157 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 84:
#line 272 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprovtype) = SEGPV_COMPLEMENT; }
#line 2163 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 85:
#line 276 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprovtype) = SEGPV_SUM8; }
#line 2169 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 86:
#line 277 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprovtype) = SEGPV_XOR8; }
#line 2175 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 87:
#line 278 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprovtype) = SEGPV_SUM16; }
#line 2181 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 88:
#line 279 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprovtype) = SEGPV_CRC5_EPC; }
#line 2187 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 89:
#line 280 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprovtype) = SEGPV_CRC5_ITU; }
#line 2193 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 90:
#line 281 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprovtype) = SEGPV_CRC5_USB; }
#line 2199 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 91:
#line 282 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprovtype) = SEGPV_CRC6_ITU; }
#line 2205 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 92:
#line 283 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprovtype) = SEGPV_CRC7_MMC; }
#line 2211 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 93:
#line 284 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprovtype) = SEGPV_CRC8; }
#line 2217 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 94:
#line 285 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprovtype) = SEGPV_CRC8_ITU; }
#line 2223 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 95:
#line 286 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprovtype) = SEGPV_CRC8_ROHC; }
#line 2229 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 96:
#line 287 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprovtype) = SEGPV_CRC8_MAXIM; }
#line 2235 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 97:
#line 288 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprovtype) = SEGPV_CRC16_IBM; }
#line 2241 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 98:
#line 289 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprovtype) = SEGPV_CRC16_MAXIM; }
#line 2247 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 99:
#line 290 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprovtype) = SEGPV_CRC16_USB; }
#line 2253 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 100:
#line 291 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprovtype) = SEGPV_CRC16_MODBUS; }
#line 2259 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 101:
#line 292 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprovtype) = SEGPV_CRC16_CCITT; }
#line 2265 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 102:
#line 293 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprovtype) = SEGPV_CRC16_CCITT_FALSE; }
#line 2271 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 103:
#line 294 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprovtype) = SEGPV_CRC16_X25; }
#line 2277 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 104:
#line 295 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprovtype) = SEGPV_CRC16_XMODEM; }
#line 2283 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 105:
#line 296 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprovtype) = SEGPV_CRC16_DNP; }
#line 2289 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 106:
#line 297 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprovtype) = SEGPV_CRC32; }
#line 2295 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 107:
#line 298 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprovtype) = SEGPV_CRC32_MPEG_2; }
#line 2301 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 108:
#line 299 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprovtype) = SEGPV_CRC64; }
#line 2307 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 109:
#line 300 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprovtype) = SEGPV_CRC64_WE; }
#line 2313 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 110:
#line 304 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprolist) = new_segproperty(SEGP_TYPE, SEGPV_ID, (yyvsp[0].symbol)); }
#line 2319 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 111:
#line 305 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprolist) = new_segproperty(SEGP_TYPE, SEGPV_NONAMEFRAME, -1, (yyvsp[-2].seglist)); }
#line 2325 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 112:
#line 306 "fparser.y" /* yacc.c:1651  */
    { (yyval.segprolist) = new_segproperty(SEGP_TYPE, SEGPV_ONEOF, (yyvsp[-5].symbol), (yyvsp[-2].oneofitemlist)); }
#line 2331 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 113:
#line 310 "fparser.y" /* yacc.c:1651  */
    { (yyval.oneofitemlist) = (yyvsp[0].oneofitemlist); }
#line 2337 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 114:
#line 311 "fparser.y" /* yacc.c:1651  */
    { (yyval.oneofitemlist) = append_oneofitem((yyvsp[-2].oneofitemlist), (yyvsp[0].oneofitemlist)); }
#line 2343 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 115:
#line 315 "fparser.y" /* yacc.c:1651  */
    { (yyval.oneofitemlist) = new_oneofitem((yyvsp[-2].symbol), (yyvsp[0].symbol)); }
#line 2349 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 116:
#line 319 "fparser.y" /* yacc.c:1651  */
    { (yyval.segproptype) = SEGP_VALUE; }
#line 2355 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 117:
#line 320 "fparser.y" /* yacc.c:1651  */
    { (yyval.segproptype) = SEGP_REPEATED; }
#line 2361 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 118:
#line 321 "fparser.y" /* yacc.c:1651  */
    { (yyval.segproptype) = SEGP_BYTESIZE; }
#line 2367 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 119:
#line 325 "fparser.y" /* yacc.c:1651  */
    { (yyval.valueexp) = new_exp(EXP_ADD, (yyvsp[-2].valueexp), (yyvsp[0].valueexp)); }
#line 2373 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 120:
#line 326 "fparser.y" /* yacc.c:1651  */
    { (yyval.valueexp) = new_exp(EXP_SUB, (yyvsp[-2].valueexp), (yyvsp[0].valueexp)); }
#line 2379 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 121:
#line 327 "fparser.y" /* yacc.c:1651  */
    { (yyval.valueexp) = new_exp(EXP_MUL, (yyvsp[-2].valueexp), (yyvsp[0].valueexp)); }
#line 2385 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 122:
#line 328 "fparser.y" /* yacc.c:1651  */
    { (yyval.valueexp) = new_exp(EXP_DIV, (yyvsp[-2].valueexp), (yyvsp[0].valueexp)); }
#line 2391 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 123:
#line 329 "fparser.y" /* yacc.c:1651  */
    { (yyval.valueexp) = (yyvsp[-1].valueexp); }
#line 2397 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 124:
#line 330 "fparser.y" /* yacc.c:1651  */
    { (yyval.valueexp) = new_exp(EXP_ID, NULL, NULL, (yyvsp[0].symbol)); }
#line 2403 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 125:
#line 331 "fparser.y" /* yacc.c:1651  */
    { (yyval.valueexp) = new_exp(EXP_REAL, NULL, NULL, (yyvsp[0].symbol)); }
#line 2409 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 126:
#line 332 "fparser.y" /* yacc.c:1651  */
    { (yyval.valueexp) = new_exp(EXP_INT, NULL, NULL, (yyvsp[0].symbol)); }
#line 2415 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 127:
#line 333 "fparser.y" /* yacc.c:1651  */
    { (yyval.valueexp) = new_exp(EXP_BYTESIZEOF, NULL, NULL, (yyvsp[-1].symbol)); }
#line 2421 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 128:
#line 334 "fparser.y" /* yacc.c:1651  */
    { (yyval.valueexp) = new_exp(EXP_BYTESIZEOF, NULL, NULL, -1); }
#line 2427 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 129:
#line 338 "fparser.y" /* yacc.c:1651  */
    { (yyval.pitem) = new_projectitem(PI_ENUMCFG, new_enumcfg((yyvsp[-4].symbol), (yyvsp[-2].enumitemlist), (yyvsp[-6].notelist))); }
#line 2433 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 130:
#line 342 "fparser.y" /* yacc.c:1651  */
    { (yyval.enumitemlist) = (yyvsp[0].enumitemlist); }
#line 2439 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 131:
#line 343 "fparser.y" /* yacc.c:1651  */
    { (yyval.enumitemlist) = append_enumitem((yyvsp[-2].enumitemlist), (yyvsp[0].enumitemlist)); }
#line 2445 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 132:
#line 347 "fparser.y" /* yacc.c:1651  */
    { (yyval.enumitemlist) = new_enumitem((yyvsp[-2].symbol), (yyvsp[0].symbol), (yyvsp[-3].notelist)); }
#line 2451 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 133:
#line 348 "fparser.y" /* yacc.c:1651  */
    { (yyval.enumitemlist) = new_enumitem((yyvsp[0].symbol), -1, (yyvsp[-1].notelist)); }
#line 2457 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 134:
#line 351 "fparser.y" /* yacc.c:1651  */
    { (yyval.notelist) = NULL; }
#line 2463 "fparser.tab.cpp" /* yacc.c:1651  */
    break;

  case 135:
#line 352 "fparser.y" /* yacc.c:1651  */
    { (yyval.notelist) = append_note((yyvsp[-1].notelist), (yyvsp[0].symbol)); }
#line 2469 "fparser.tab.cpp" /* yacc.c:1651  */
    break;


#line 2473 "fparser.tab.cpp" /* yacc.c:1651  */
      default: break;
    }
  /* User semantic actions sometimes alter yychar, and that requires
     that yytoken be updated with the new translation.  We take the
     approach of translating immediately before every use of yytoken.
     One alternative is translating here after every semantic action,
     but that translation would be missed if the semantic action invokes
     YYABORT, YYACCEPT, or YYERROR immediately after altering yychar or
     if it invokes YYBACKUP.  In the case of YYABORT or YYACCEPT, an
     incorrect destructor might then be invoked immediately.  In the
     case of YYERROR or YYBACKUP, subsequent parser actions might lead
     to an incorrect destructor call or verbose syntax error message
     before the lookahead is translated.  */
  YY_SYMBOL_PRINT ("-> $$ =", yyr1[yyn], &yyval, &yyloc);

  YYPOPSTACK (yylen);
  yylen = 0;
  YY_STACK_PRINT (yyss, yyssp);

  *++yyvsp = yyval;
  *++yylsp = yyloc;

  /* Now 'shift' the result of the reduction.  Determine what state
     that goes to, based on the state we popped back to and the rule
     number reduced by.  */

  yyn = yyr1[yyn];

  yystate = yypgoto[yyn - YYNTOKENS] + *yyssp;
  if (0 <= yystate && yystate <= YYLAST && yycheck[yystate] == *yyssp)
    yystate = yytable[yystate];
  else
    yystate = yydefgoto[yyn - YYNTOKENS];

  goto yynewstate;


/*--------------------------------------.
| yyerrlab -- here on detecting error.  |
`--------------------------------------*/
yyerrlab:
  /* Make sure we have latest lookahead translation.  See comments at
     user semantic actions for why this is necessary.  */
  yytoken = yychar == YYEMPTY ? YYEMPTY : YYTRANSLATE (yychar);

  /* If not already recovering from an error, report this error.  */
  if (!yyerrstatus)
    {
      ++yynerrs;
#if ! YYERROR_VERBOSE
      yyerror (&yylloc, plexer, db, YY_("syntax error"));
#else
# define YYSYNTAX_ERROR yysyntax_error (&yymsg_alloc, &yymsg, \
                                        yyssp, yytoken)
      {
        char const *yymsgp = YY_("syntax error");
        int yysyntax_error_status;
        yysyntax_error_status = YYSYNTAX_ERROR;
        if (yysyntax_error_status == 0)
          yymsgp = yymsg;
        else if (yysyntax_error_status == 1)
          {
            if (yymsg != yymsgbuf)
              YYSTACK_FREE (yymsg);
            yymsg = (char *) YYSTACK_ALLOC (yymsg_alloc);
            if (!yymsg)
              {
                yymsg = yymsgbuf;
                yymsg_alloc = sizeof yymsgbuf;
                yysyntax_error_status = 2;
              }
            else
              {
                yysyntax_error_status = YYSYNTAX_ERROR;
                yymsgp = yymsg;
              }
          }
        yyerror (&yylloc, plexer, db, yymsgp);
        if (yysyntax_error_status == 2)
          goto yyexhaustedlab;
      }
# undef YYSYNTAX_ERROR
#endif
    }

  yyerror_range[1] = yylloc;

  if (yyerrstatus == 3)
    {
      /* If just tried and failed to reuse lookahead token after an
         error, discard it.  */

      if (yychar <= YYEOF)
        {
          /* Return failure if at end of input.  */
          if (yychar == YYEOF)
            YYABORT;
        }
      else
        {
          yydestruct ("Error: discarding",
                      yytoken, &yylval, &yylloc, plexer, db);
          yychar = YYEMPTY;
        }
    }

  /* Else will try to reuse lookahead token after shifting the error
     token.  */
  goto yyerrlab1;


/*---------------------------------------------------.
| yyerrorlab -- error raised explicitly by YYERROR.  |
`---------------------------------------------------*/
yyerrorlab:

  /* Pacify compilers like GCC when the user code never invokes
     YYERROR and the label yyerrorlab therefore never appears in user
     code.  */
  if (/*CONSTCOND*/ 0)
     goto yyerrorlab;

  /* Do not reclaim the symbols of the rule whose action triggered
     this YYERROR.  */
  YYPOPSTACK (yylen);
  yylen = 0;
  YY_STACK_PRINT (yyss, yyssp);
  yystate = *yyssp;
  goto yyerrlab1;


/*-------------------------------------------------------------.
| yyerrlab1 -- common code for both syntax error and YYERROR.  |
`-------------------------------------------------------------*/
yyerrlab1:
  yyerrstatus = 3;      /* Each real token shifted decrements this.  */

  for (;;)
    {
      yyn = yypact[yystate];
      if (!yypact_value_is_default (yyn))
        {
          yyn += YYTERROR;
          if (0 <= yyn && yyn <= YYLAST && yycheck[yyn] == YYTERROR)
            {
              yyn = yytable[yyn];
              if (0 < yyn)
                break;
            }
        }

      /* Pop the current state because it cannot handle the error token.  */
      if (yyssp == yyss)
        YYABORT;

      yyerror_range[1] = *yylsp;
      yydestruct ("Error: popping",
                  yystos[yystate], yyvsp, yylsp, plexer, db);
      YYPOPSTACK (1);
      yystate = *yyssp;
      YY_STACK_PRINT (yyss, yyssp);
    }

  YY_IGNORE_MAYBE_UNINITIALIZED_BEGIN
  *++yyvsp = yylval;
  YY_IGNORE_MAYBE_UNINITIALIZED_END

  yyerror_range[2] = yylloc;
  /* Using YYLLOC is tempting, but would change the location of
     the lookahead.  YYLOC is available though.  */
  YYLLOC_DEFAULT (yyloc, yyerror_range, 2);
  *++yylsp = yyloc;

  /* Shift the error token.  */
  YY_SYMBOL_PRINT ("Shifting", yystos[yyn], yyvsp, yylsp);

  yystate = yyn;
  goto yynewstate;


/*-------------------------------------.
| yyacceptlab -- YYACCEPT comes here.  |
`-------------------------------------*/
yyacceptlab:
  yyresult = 0;
  goto yyreturn;

/*-----------------------------------.
| yyabortlab -- YYABORT comes here.  |
`-----------------------------------*/
yyabortlab:
  yyresult = 1;
  goto yyreturn;

#if !defined yyoverflow || YYERROR_VERBOSE
/*-------------------------------------------------.
| yyexhaustedlab -- memory exhaustion comes here.  |
`-------------------------------------------------*/
yyexhaustedlab:
  yyerror (&yylloc, plexer, db, YY_("memory exhausted"));
  yyresult = 2;
  /* Fall through.  */
#endif

yyreturn:
  if (yychar != YYEMPTY)
    {
      /* Make sure we have latest lookahead translation.  See comments at
         user semantic actions for why this is necessary.  */
      yytoken = YYTRANSLATE (yychar);
      yydestruct ("Cleanup: discarding lookahead",
                  yytoken, &yylval, &yylloc, plexer, db);
    }
  /* Do not reclaim the symbols of the rule whose action triggered
     this YYABORT or YYACCEPT.  */
  YYPOPSTACK (yylen);
  YY_STACK_PRINT (yyss, yyssp);
  while (yyssp != yyss)
    {
      yydestruct ("Cleanup: popping",
                  yystos[*yyssp], yyvsp, yylsp, plexer, db);
      YYPOPSTACK (1);
    }
#ifndef yyoverflow
  if (yyss != yyssa)
    YYSTACK_FREE (yyss);
#endif
#if YYERROR_VERBOSE
  if (yymsg != yymsgbuf)
    YYSTACK_FREE (yymsg);
#endif
  return yyresult;
}
#line 357 "fparser.y" /* yacc.c:1910  */


