%locations
%pure_parser 
%parse-param    { class yyFlexLexer* plexer }
%parse-param    { class FrameIOParserDb* db }
%lex-param {db}

%{
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
%}


%union {
	int symbol;

	PROJECT * project;
	SYS * syslist;
	FRAME * framelist;
	ENUMCFG * enumcfglist;
}

%token T_PROJECT T_SYSTEM T_FRAME T_CHANNEL T_ENUM T_ACTION T_NOTE T_ID
%token T_INTEGER T_REAL T_BLOCK T_TEXT
%token T_BOOL T_BYTE T_SBYTE T_USHORT T_SHORT T_UINT T_INT T_ULONG T_LONG T_FLOAT T_DOUBLE T_STRING
%token T_SEND T_ON T_RECV T_RECVLOOP
%token T_COM T_CAN T_TCPSERVER T_TCPCLIENT T_UDP T_DI T_DO
%token T_DEVICEID T_BAUDRATE
%token T_SIGNED T_BITCOUNT T_VALUE T_REPEATED T_BYTEORDER T_ENCODED T_REPEATED T_ISDOUBLE T_TAIL T_ALIGEDLEN T_TYPE T_BYTESIZE T_TOENUM T_ONEOF T_MAX T_MIN T_CHECK T_CHECKRANGE
%token T_TRUE T_FALSE T_SMALL T_BIG T_PRIMITIVE T_INVRSION T_COMPLEMENT
%token T_SUM8 T_XOR8 T_SUM16 T_SUM16_FALSE T_XOR16 T_XOR16_FALSE T_SUM32 T_SUM32_FALSE T_XOR32 T_XOR32_FALSE T_CRC4_ITU T_CRC5_EPC T_CRC5_ITU
%token T_CRC5_USB T_CRC6_ITU T_CRC7_MMC T_CRC8 T_CRC8_ITU T_CRC8_ROHC T_CRC8_MAXIM T_CRC16_IBM T_CRC16_MAXIM T_CRC16_USB T_CRC16_MODBUS
%token T_CRC16_CCITT T_CRC16_CCITT_FALSE T_CRC16_X25 T_CRC16_XMODEM T_CRC16_DNP T_CRC32 T_CRC32_MPEG_2 T_CRC64 T_CRC64_WE

%token VALUE_STRING VALUE_INT VALUE_REAL

%right '='
%left '+' '-'
%left '*' '/'

%start project

%%

project: notelist T_PROJECT T_ID '{' projectitemlist notelist '}' notelist
;

projectitemlist:
	| projectitemlist projectitem
;

projectitem:
	system
	| frame
	| enumcfg
;

system: notelist T_SYSTEM T_ID '{' systemitemlist notelist '}'
;

systemitemlist:
	| systemitemlist systemitem
;

systemitem:
	sysproperty
	| channel
	| action
;

sysproperty:
	notelist syspropertytype T_ID ';'
	| notelist syspropertytype '[' ']' T_ID ';'
;

syspropertytype:
	T_BOOL
	| T_BYTE
	| T_SBYTE
	| T_USHORT
	| T_SHORT
	| T_UINT
	| T_INT
	| T_ULONG
	| T_LONG
	| T_FLOAT
	| T_DOUBLE
;

channel: 
	notelist T_CHANNEL T_ID ':' channeltype '{' channeloptionlist notelist '}'
;

channeltype:
	T_COM
	| T_CAN
	| T_TCPSERVER
	| T_TCPCLIENT
	| T_UDP
	| T_DI
	| T_DO
;

channeloptionlist:
	channeloption
	|channeloptionlist channeloption
;

channeloption:
	notelist channeloptionname '=' channeloptionvalue ';'
;

channeloptionname:
	T_DEVICEID
	| T_BAUDRATE
;

channeloptionvalue:
	VALUE_INT;
	| VALUE_STRING
	| VALUE_REAL
;

action:
	notelist T_ACTION T_ID ':' actiontype T_ID T_ON T_ID '{' actionmaplist notelist'}'
;

actiontype:
	T_SEND
	| T_RECV
	| T_RECVLOOP
;

actionmaplist:
	actionmap
	| actionmaplist actionmap
;

actionmap:
	notelist T_ID ':' T_ID ';'
;

frame: 
	notelist T_FRAME T_ID '{' framesegmentlist notelist '}'
;

framesegmentlist:
	| framesegmentlist framesegment
;

framesegment:
	notelist framesegmenttype T_ID framesegmentpropertylist ';'
;

framesegmenttype:
	T_INTEGER
	| T_REAL
	| T_BLOCK
	| T_TEXT
;

framesegmentpropertylist:
	| framesegmentpropertylist framesegmentproperty
;

framesegmentproperty:
	framesegmentpropertybool '=' framesegmentpropertyboolvalue
	| framesegmentpropertyint '=' VALUE_INT
	| framesegmentpropertyconst '=' framesegmentpropertyconstvalue
	| T_BYTEORDER '=' framesegmentpropertyorder
	| T_ENCODED '=' framesegmentpropertyencoded
	| T_CHECK '=' framesegmentchecktype
	| T_CHECKRANGE '=' '(' T_ID ',' T_ID ')'
	| T_TOENUM '=' T_ID
	| T_TAIL '=' VALUE_STRING
	| T_TYPE '=' framesegmentproperttype
	| framesegmentpropertyexp '=' exp
;


framesegmentpropertybool:
	T_ISDOUBLE	
	| T_SIGNED
;

framesegmentpropertyboolvalue:
	T_TRUE
	| T_FALSE
;

framesegmentpropertyint:
	T_BITCOUNT
	| T_ALIGEDLEN
;

framesegmentpropertyconst:
	T_MAX
	| T_MIN
;

framesegmentpropertyconstvalue:
	VALUE_INT
	| VALUE_REAL
;

framesegmentpropertyorder:
	T_SMALL
	| T_BIG
;

framesegmentpropertyencoded:
	T_PRIMITIVE
	| T_INVRSION
	| T_COMPLEMENT
;

framesegmentchecktype:
	T_SUM8
	| T_XOR8
	| T_SUM16
	| T_CRC5_EPC
	| T_CRC5_ITU
	| T_CRC5_USB
	| T_CRC6_ITU
	| T_CRC7_MMC
	| T_CRC8
	| T_CRC8_ITU
	| T_CRC8_ROHC
	| T_CRC8_MAXIM
	| T_CRC16_IBM
	| T_CRC16_MAXIM
	| T_CRC16_USB
	| T_CRC16_MODBUS
	| T_CRC16_CCITT
	| T_CRC16_CCITT_FALSE
	| T_CRC16_X25
	| T_CRC16_XMODEM
	| T_CRC16_DNP
	| T_CRC32
	| T_CRC32_MPEG_2
	| T_CRC64
	| T_CRC64_WE
;

framesegmentproperttype:
	T_ID
	| '{' framesegmentlist notelist '}'
	| T_ONEOF '(' T_ID ')' '{' framesegmentoneoflist notelist '}'
;

framesegmentoneoflist:
	framesegmentoneofitem
	|framesegmentoneoflist ',' framesegmentoneofitem
;

framesegmentoneofitem:
	T_ID ':' T_ID 
;

framesegmentpropertyexp:
	T_VALUE
	| T_REPEATED
	| T_BYTESIZE
;

exp:
	exp '+' exp
	| exp '-' exp
	| exp '*' exp
	| exp '/' exp
	| '(' exp ')'
	| T_ID	
	| VALUE_REAL
	| VALUE_INT
;

enumcfg: notelist T_ENUM T_ID '{' enumitemlist '}'
;

enumitemlist: 
	enumitem
	| enumitemlist ',' enumitem
;

enumitem:
	T_ID '=' VALUE_INT
	| T_ID
;

notelist:
	| notelist T_NOTE
;



%%

