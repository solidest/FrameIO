
%option c++
%option noyywrap
%option yylineno


EXP ([Ee][-+]?[0-9]+)
UCN (\\u[0-9a-fA-F]{4}|\\U[0-9a-fA-F]{8})
CHS (\xE9\xBE[\x80-\xA5]|(\xE4[\xB8-\xBF]|\xE9[\x80-\xBD]|[\xE5-\xE8][\x80-\xBF])[\x80-\xBF])

%{
#define YY_DECL int FrameLexer::yylex(YYSTYPE* yylval, YYLTYPE* yylloc)

#include "stdafx.h"
#include "fparser.h"
#include "fparser.tab.h"
#include "FrameLexer.h"


int yycolumn = 1;
int utf8len = 0;

#define YY_USER_ACTION utf8len = get_utf8_length(yytext, yyleng); yylloc->first_line = yylloc->last_line = yylineno; \
	yylloc->first_column = yycolumn; yylloc->last_column = yycolumn + utf8len - 1; \
	yycolumn += utf8len;
%}
%%


^("//"|([ \t]+"//")).*\n	{ yycolumn = 1; /*yylval.symbol = save_symbol(yytext, yylloc.first_line, yylloc.first_column, yylloc.last_column);*/ return T_NOTE; }
"//".*\n					{ yycolumn = 1; }


\"([^"\\]|\\['"?\\abfnrtv]|\\[0-7]{1,3}|\\[xX][0-9a-fA-F]+|{UCN}|{CHS})*\"	{ /*yylval.symbol = save_symbol(yytext, yylloc.first_line, yylloc.first_column, yylloc.last_column);*/ return VALUE_STRING; }

"+" |
"-" |
"*" |
"/" |
"=" |
"("	|					
")" |
"{" |
"}" |
"[" |
"]" |
":" |
";"						{ return yytext[0]; }


"project"				{ return T_PROJECT; }
"system"				{ return T_SYSTEM; }
"frame"					{ return T_FRAME; }
"channel"				{ return T_CHANNEL; }
"enum"					{ return T_ENUM; }
"action"				{ return T_ACTION; }

"integer"				{ return T_INTEGER; }
"real"					{ return T_REAL; }
"block"					{ return T_BLOCK; }
"text"					{ return T_TEXT; }

"bool"					{ return T_BOOL; }
"byte"					{ return T_BYTE; }
"sbyte"					{ return T_SBYTE; }
"ushort"				{ return T_USHORT; }
"short"					{ return T_SHORT; }
"uint"					{ return T_UINT; }
"int"					{ return T_INT; }
"ulong"					{ return T_ULONG; }
"long"					{ return T_LONG; }
"float"					{ return T_FLOAT; }
"double"				{ return T_DOUBLE; }
"string"				{ return T_STRING; }

"send"					{ return T_SEND; }
"on"					{ return T_ON; }
"recv"					{ return T_RECV; }
"recvloop"				{ return T_RECVLOOP; }

"com"					{ return T_COM; }
"can"					{ return T_CAN; }
"tcpserver"				{ return T_TCPSERVER; }
"tcpclient"				{ return T_TCPCLIENT; }
"upd"					{ return T_UDP; }
"di"					{ return T_DI; }
"do"					{ return T_DO; }

"deviceid"				{ return T_DEVICEID; }
"baudrate"				{ return T_BAUDRATE; }
      
"signed"				{ return T_SIGNED; }
"bitcount"				{ return T_BITCOUNT; }
"value"					{ return T_VALUE; }
"repeated"				{ return T_REPEATED; }
"byteorder"				{ return T_BYTEORDER; }
"encoded"				{ return T_ENCODED; }
"repeated"				{ return T_REPEATED; }
"isdouble"				{ return T_ISDOUBLE; }
"tail"					{ return T_TAIL; }
"alignedlen"			{ return T_ALIGEDLEN; }
"type"					{ return T_TYPE; }
"bytesize"				{ return T_BYTESIZE; }
"toenum"				{ return T_TOENUM; }
"oneof"					{ return T_ONEOF; }
"max"					{ return T_MAX; }
"min"					{ return T_MIN; }
"check"					{ return T_CHECK; }
"checkrange"			{ return T_CHECKRANGE;}

"true"					{ return T_TRUE; }
"false"					{ return T_FALSE; }
"small"					{ return T_SMALL; }
"big"					{ return T_BIG; }
"primitive"				{ return T_PRIMITIVE; }
"inversion"				{ return T_INVRSION; }
"complement"			{ return T_COMPLEMENT; }

"sum8"					{ return T_SUM8; }
"xor8"					{ return T_XOR8; }
"sum16"					{ return T_SUM16; }
"sum16_false"			{ return T_SUM16_FALSE; }
"xor16"					{ return T_XOR16; }
"xor16_false"			{ return T_XOR16_FALSE; }
"sum32"					{ return T_SUM32; }
"sum32_false"			{ return T_SUM32_FALSE; }
"xor32"					{ return T_XOR32; }
"xor32_false"			{ return T_XOR32_FALSE; }
"crc4_itu"				{ return T_CRC4_ITU; }
"crc5_epc"				{ return T_CRC5_EPC; }
"crc5_itu"				{ return T_CRC5_ITU; }
"crc5_usb"				{ return T_CRC5_USB; }
"crc6_itu"				{ return T_CRC6_ITU; }
"crc7_mmc"				{ return T_CRC7_MMC; }
"crc8"					{ return T_CRC8; }
"crc8_itu"				{ return T_CRC8_ITU; }
"crc8_rohc"				{ return T_CRC8_ROHC; }
"crc8_maxim"			{ return T_CRC8_MAXIM; }
"crc16_ibm"				{ return T_CRC16_IBM; }
"crc16_maxim"			{ return T_CRC16_MAXIM; }
"crc16_usb"				{ return T_CRC16_USB; }
"crc16_modbus"			{ return T_CRC16_MODBUS; }
"crc16_ccitt"			{ return T_CRC16_CCITT; }
"crc16_ccitt_false"		{ return T_CRC16_CCITT_FALSE; }
"crc16_x25"				{ return T_CRC16_X25; }
"crc16_xmodem"			{ return T_CRC16_XMODEM; }
"crc16_dnp"				{ return T_CRC16_DNP; }
"crc32"					{ return T_CRC32; }
"crc32_mpeg_2"			{ return T_CRC32_MPEG_2; }
"crc64"					{ return T_CRC64; }
"crc64_we"				{ return T_CRC64_WE; }


0[0-7]* |
[1-9][0-9]* |
0[Xx][0-9a-fA-F]+		{ /*yylval.symbol = save_symbol(yytext, yylloc.first_line, yylloc.first_column, yylloc.last_column);*/ return VALUE_INT; }

-?([0-9]*\.[0-9]+|[0-9]+\.){EXP}? |
[0-9]+{EXP}				{ /*yylval.symbol = save_symbol(yytext, yylloc.first_line, yylloc.first_column, yylloc.last_column);*/ return VALUE_REAL; }

[a-zA-Z_][a-zA-Z0-9_]*	{ /*yylval.symbol = save_symbol(yytext, yylloc.first_line, yylloc.first_column, yylloc.last_column);*/ return T_ID; }

\r\n					{ yycolumn=1; }
\n						{ yycolumn=1; }
[ \t]					{ ; }

[\xEF\xBB\xBF]			{ yycolumn=1; }
<<EOF>>					{ return 0; }
.						{ /*yylval.symbol = save_symbol(yytext, yylloc.first_line, yylloc.first_column, yylloc.last_column);out_error(ERROR_CODE_SYMBOL, yylval.symbol, yylval.symbol);*/  }
%%
