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
	if (yylval.symbol==0)
		db->SaveError(ERROR_CODE_0, yylval.symbol, yylval.symbol);
	else
		db->SaveError(ERROR_CODE_SYNTAX, yylval.symbol, yylval.symbol);
	return 1; 
}
%}


%union {
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

}

%token T_PROJECT T_SYSTEM T_FRAME T_CHANNEL T_ENUM T_ACTION T_THIS
%token T_INTEGER T_REAL T_BLOCK T_TEXT
%token T_BOOL T_BYTE T_SBYTE T_USHORT T_SHORT T_UINT T_INT T_ULONG T_LONG T_FLOAT T_DOUBLE T_STRING
%token T_SEND T_ON T_RECV T_RECVLOOP
%token T_COM T_CAN T_TCPSERVER T_TCPCLIENT T_UDP T_DI T_DO
%token T_SIGNED T_BITCOUNT T_VALUE T_REPEATED T_BYTEORDER T_ENCODED T_REPEATED T_ISDOUBLE T_TAIL T_ALIGNEDLEN T_TYPE T_BYTESIZE 
%token T_BYTESIZEOF T_TOENUM T_ONEOF T_MAX T_MIN T_CHECK T_CHECKRANGE
%token T_TRUE T_FALSE T_SMALL T_BIG T_PRIMITIVE T_INVERSION T_COMPLEMENT
%token T_SUM8 T_XOR8 T_SUM16 T_SUM16_FALSE T_XOR16 T_XOR16_FALSE T_SUM32 T_SUM32_FALSE T_XOR32 T_XOR32_FALSE T_CRC4_ITU T_CRC5_EPC T_CRC5_ITU
%token T_CRC5_USB T_CRC6_ITU T_CRC7_MMC T_CRC8 T_CRC8_ITU T_CRC8_ROHC T_CRC8_MAXIM T_CRC16_IBM T_CRC16_MAXIM T_CRC16_USB T_CRC16_MODBUS
%token T_CRC16_CCITT T_CRC16_CCITT_FALSE T_CRC16_X25 T_CRC16_XMODEM T_CRC16_DNP T_CRC32 T_CRC32_MPEG_2 T_CRC64 T_CRC64_WE

%token <symbol> VALUE_STRING VALUE_INT VALUE_REAL T_ID T_NOTE T_UNION_ID

%type <project> project 
%type <pitem> projectitem frame enumcfg system
%type <pitemlist> projectitemlist

%type <sysitemlist> systemitemlist
%type <sysitem> systemitem sysproperty channel action
%type <sysptype> sysprotype
%type <choplist> channeloptionlist channeloption
%type <syschtype> channeltype
%type <optionvalue> channeloptionvalue
%type <amaplist> actionmaplist actionmap
%type <iotype> actiontype

%type <seglist> framesegmentlist framesegment
%type <segtype> framesegmenttype
%type <segprolist> framesegmentproperty framesegmentpropertylist framesegmentpropertytypevalue
%type <segproptype> framesegmentpropertyexp framesegmentpropertyconst framesegmentpropertyint framesegmentpropertybool
%type <segprovtype> framesegmentpropertyboolvalue framesegmentcheckvalue framesegmentpropertyorder framesegmentpropertyencoded 

%type <oneofitemlist> framesegmentoneoflist framesegmentoneofitem
%type <enumitemlist> enumitemlist enumitem
%type <valueexp> exp 
%type <notelist> notelist 

%destructor { free_projectitem($$); $$=NULL; } <pitem>
%destructor { free_projectitemlist($$); $$=NULL; } <pitemlist>
%destructor { free_sysitem($$); $$=NULL; } <sysitem>
%destructor { free_sysitemlist($$); $$=NULL; } <sysitemlist>
%destructor { free_channeloption($$); $$=NULL; } <choplist>
%destructor { free_actionmap($$); $$=NULL; } <amaplist>
%destructor { free_segment($$); $$=NULL; } <seglist>
%destructor { free_segproperty($$); $$=NULL; } <segprolist>
%destructor { free_expvalue($$); $$=NULL; } <valueexp>
%destructor { free_oneofitem($$); $$=NULL; } <oneofitemlist>
%destructor { free_enumitem($$); $$=NULL; } <enumitemlist>
%destructor { free_note($$); $$=NULL; } <notelist>

%right '='
%left '+' '-'
%left '*' '/'

%start project

%%

project:
	notelist T_PROJECT T_ID '{' projectitemlist notelist '}' notelist		{ $$ = new_project($3, $5, $1); db->SaveProject($$); free_project($$); $$=NULL; }
;

projectitemlist:															{ $$ = NULL; }
	| projectitemlist projectitem											{ $$ = add_projectitem($1, $2); }														
;

projectitem:
	system																	{ $$ = $1; }
	| frame																	{ $$ = $1; }
	| enumcfg																{ $$ = $1; }
;

system: 
	notelist T_SYSTEM T_ID '{' systemitemlist notelist '}'					{ $$ = new_projectitem(PI_SYSTEM, new_sys($3, $5, $1)); }
;

systemitemlist:																{ $$ = NULL; }
	| systemitemlist systemitem												{ $$ = add_sysitem($1, $2); }
;

systemitem:
	sysproperty																{ $$ = $1; }
	| channel																{ $$ = $1; }
	| action																{ $$ = $1; }
;

sysproperty:
	notelist sysprotype T_ID ';'										{ $$ = new_sysitem(SYSI_PROPERTY, new_sysproperty($3, $2, FALSE, $1)); }
	| notelist sysprotype '[' ']' T_ID ';'								{ $$ = new_sysitem(SYSI_PROPERTY, new_sysproperty($5, $2, TRUE, $1)); }
;

sysprotype:
	T_BOOL																	{ $$ = SYSPT_BOOL; }
	| T_BYTE																{ $$ = SYSPT_BYTE; }
	| T_SBYTE																{ $$ = SYSPT_SBYTE; }
	| T_USHORT																{ $$ = SYSPT_USHORT; }
	| T_SHORT																{ $$ = SYSPT_SHORT; }
	| T_UINT																{ $$ = SYSPT_UINT; }
	| T_INT																	{ $$ = SYSPT_INT; }
	| T_ULONG																{ $$ = SYSPT_ULONG; }
	| T_LONG																{ $$ = SYSPT_LONG; }
	| T_FLOAT																{ $$ = SYSPT_FLOAT; }
	| T_DOUBLE																{ $$ = SYSPT_DOUBLE; }
;

channel: 
	notelist T_CHANNEL T_ID ':' channeltype '{' channeloptionlist notelist '}'	{ $$ = new_sysitem(SYSI_CHANNEL, new_syschannel($3, $5, $7, $1)); }
;

channeltype:
	T_COM																		{ $$ = SCHT_COM; }
	| T_CAN																		{ $$ = SCHT_CAN; }
	| T_TCPSERVER																{ $$ = SCHT_TCPSERVER; }
	| T_TCPCLIENT																{ $$ = SCHT_TCPCLIENT; }
	| T_UDP																		{ $$ = SCHT_UDP; }
	| T_DI																		{ $$ = SCHT_DI; }
	| T_DO																		{ $$ = SCHT_DO; }
;

channeloptionlist:																{ $$ = NULL; }
	| channeloptionlist channeloption											{ $$ = append_channeloption($1, $2); }
;

channeloption:
	notelist T_ID '=' channeloptionvalue ';'						{ $$ = new_channeloption($2, $4, $1); }
;


channeloptionvalue:
	VALUE_INT																	{ $$ = $1; }
	| VALUE_STRING																{ $$ = $1; }
	| VALUE_REAL																{ $$ = $1; }
;

action:
	notelist T_ACTION T_ID ':' actiontype T_ID T_ON T_ID '{' actionmaplist notelist'}'	{ $$ = new_sysitem(SYSI_ACTION, new_action($3, $5, $6, $8, $10, $1)); }
;

actiontype:
	T_SEND																				{ $$ = AIO_SEND; }
	| T_RECV																			{ $$ = AIO_RECV; }
	| T_RECVLOOP																		{ $$ = AIO_RECVLOOP; }
;

actionmaplist:																			{ $$ = NULL; }
	| actionmaplist actionmap															{ $$ = append_actionmap($1, $2); }
;

actionmap:
	notelist T_ID ':' T_ID ';'															{ $$ = new_actionmap($2, $4, $1); }
	| notelist T_UNION_ID ':' T_ID ';'													{ $$ = new_actionmap($2, $4, $1); }
;


frame: 
	notelist T_FRAME T_ID '{' framesegmentlist notelist '}'								{ $$ = new_projectitem(PI_FRAME, new_frame($3, $5, $1)); }
;

framesegmentlist:																		{ $$ = NULL; }
	| framesegmentlist framesegment														{ $$ = append_segment($1, $2); }
;

framesegment:
	notelist framesegmenttype T_ID framesegmentpropertylist ';'							{ $$ = new_segment($2, $3, $4, $1); }
;

framesegmenttype:
	T_INTEGER																			{ $$ = SEGT_INTEGER; }
	| T_REAL																			{ $$ = SEGT_REAL; }
	| T_BLOCK																			{ $$ = SEGT_BLOCK; }
	| T_TEXT																			{ $$ = SEGT_TEXT; }
;

framesegmentpropertylist:																{ $$ = NULL; }
	| framesegmentpropertylist framesegmentproperty										{ $$ = append_segproperty($1, $2); }
;

framesegmentproperty:
	framesegmentpropertybool '=' framesegmentpropertyboolvalue							{ $$ = new_segproperty($1, $3); }
	| framesegmentpropertyint '=' VALUE_INT												{ $$ = new_segproperty($1, SEGPV_INT, $3); }
	| framesegmentpropertyconst '=' VALUE_INT											{ $$ = new_segproperty($1, SEGPV_INT, $3); }
	| framesegmentpropertyconst '=' VALUE_REAL											{ $$ = new_segproperty($1, SEGPV_REAL, $3); }
	| T_BYTEORDER '=' framesegmentpropertyorder											{ $$ = new_segproperty(SEGP_BYTEORDER, $3); }
	| T_ENCODED '=' framesegmentpropertyencoded											{ $$ = new_segproperty(SEGP_ENCODED, $3); }
	| T_CHECK '=' framesegmentcheckvalue												{ $$ = new_segproperty(SEGP_CHECK, $3); }
	| T_CHECKRANGE '=' '(' T_ID ',' T_ID ')'											{ $$ = append_segproperty(new_segproperty(SEGP_CHECKRANGE_BEGIN, SEGPV_ID, $4), new_segproperty(SEGP_CHECKRANGE_END, SEGPV_ID, $6)); }
	| T_TOENUM '=' T_ID																	{ $$ = new_segproperty(SEGP_TOENUM, SEGPV_STRING, $3); }
	| T_TAIL '=' VALUE_STRING															{ $$ = new_segproperty(SEGP_TAIL, SEGPV_STRING, $3); }
	| T_TYPE '=' framesegmentpropertytypevalue											{ $$ = $3; }
	| framesegmentpropertyexp '=' exp													{ $$ = new_segproperty($1, SEGPV_EXP, -1, $3); }
;


framesegmentpropertybool:
	T_ISDOUBLE																			{ $$ = SEGP_ISDOUBLE; }
	| T_SIGNED																			{ $$ = SEGP_SIGNED; }
;

framesegmentpropertyboolvalue:
	T_TRUE																				{ $$ = SEGPV_TRUE; }
	| T_FALSE																			{ $$ = SEGPV_FALSE; }
;

framesegmentpropertyint:
	T_BITCOUNT																			{ $$ = SEGP_BITCOUNT; }
	| T_ALIGNEDLEN																		{ $$ = SEGP_ALIGNEDLEN; }
;

framesegmentpropertyconst:
	T_MAX																				{ $$ = SEGP_MAX; }
	| T_MIN																				{ $$ = SEGP_MIN; }
;


framesegmentpropertyorder:
	T_SMALL																				{ $$ = SEGPV_SMALL; }
	| T_BIG																				{ $$ = SEGPV_BIG; }
;

framesegmentpropertyencoded:
	T_PRIMITIVE																			{ $$ = SEGPV_PRIMITIVE; }
	| T_INVERSION																		{ $$ = SEGPV_INVERSION; }
	| T_COMPLEMENT																		{ $$ = SEGPV_COMPLEMENT; }
;

framesegmentcheckvalue:
	T_SUM8																		{ $$ = SEGPV_SUM8; }
	| T_XOR8																	{ $$ = SEGPV_XOR8; }
	| T_SUM16																	{ $$ = SEGPV_SUM16; }
	| T_CRC5_EPC																{ $$ = SEGPV_CRC5_EPC; }
	| T_CRC5_ITU																{ $$ = SEGPV_CRC5_ITU; }
	| T_CRC5_USB																{ $$ = SEGPV_CRC5_USB; }
	| T_CRC6_ITU																{ $$ = SEGPV_CRC6_ITU; }
	| T_CRC7_MMC																{ $$ = SEGPV_CRC7_MMC; }
	| T_CRC8																	{ $$ = SEGPV_CRC8; }
	| T_CRC8_ITU																{ $$ = SEGPV_CRC8_ITU; }
	| T_CRC8_ROHC																{ $$ = SEGPV_CRC8_ROHC; }
	| T_CRC8_MAXIM																{ $$ = SEGPV_CRC8_MAXIM; }
	| T_CRC16_IBM																{ $$ = SEGPV_CRC16_IBM; }
	| T_CRC16_MAXIM																{ $$ = SEGPV_CRC16_MAXIM; }
	| T_CRC16_USB																{ $$ = SEGPV_CRC16_USB; }
	| T_CRC16_MODBUS															{ $$ = SEGPV_CRC16_MODBUS; }
	| T_CRC16_CCITT																{ $$ = SEGPV_CRC16_CCITT; }
	| T_CRC16_CCITT_FALSE														{ $$ = SEGPV_CRC16_CCITT_FALSE; }
	| T_CRC16_X25																{ $$ = SEGPV_CRC16_X25; }
	| T_CRC16_XMODEM															{ $$ = SEGPV_CRC16_XMODEM; }
	| T_CRC16_DNP																{ $$ = SEGPV_CRC16_DNP; }
	| T_CRC32																	{ $$ = SEGPV_CRC32; }
	| T_CRC32_MPEG_2															{ $$ = SEGPV_CRC32_MPEG_2; }
	| T_CRC64																	{ $$ = SEGPV_CRC64; }
	| T_CRC64_WE																{ $$ = SEGPV_CRC64_WE; }
;

framesegmentpropertytypevalue:
	T_ID																		{ $$ = new_segproperty(SEGP_TYPE, SEGPV_ID, $1); }
	| '{' framesegmentlist notelist '}'											{ $$ = new_segproperty(SEGP_TYPE, SEGPV_NONAMEFRAME, -1, $2); }
	| T_ONEOF '(' T_ID ')' '{' framesegmentoneoflist notelist '}'				{ $$ = new_segproperty(SEGP_TYPE, SEGPV_ONEOF, $3, $6); }
;

framesegmentoneoflist:
	framesegmentoneofitem														{ $$ = $1; }
	|framesegmentoneoflist ',' framesegmentoneofitem							{ $$ = append_oneofitem($1, $3); }
;

framesegmentoneofitem:
	T_ID ':' T_ID																{ $$ = new_oneofitem($1, $3); }
;

framesegmentpropertyexp:
	T_VALUE																		{ $$ = SEGP_VALUE; }
	| T_REPEATED																{ $$ = SEGP_REPEATED; }
	| T_BYTESIZE																{ $$ = SEGP_BYTESIZE; }
;

exp:
	exp '+' exp																	{ $$ = new_exp(EXP_ADD, $1, $3); }
	| exp '-' exp																{ $$ = new_exp(EXP_SUB, $1, $3); }
	| exp '*' exp																{ $$ = new_exp(EXP_MUL, $1, $3); }
	| exp '/' exp																{ $$ = new_exp(EXP_DIV, $1, $3); }
	| '(' exp ')'																{ $$ = $2; }
	| T_ID																		{ $$ = new_exp(EXP_ID, NULL, NULL, $1); }
	| VALUE_REAL																{ $$ = new_exp(EXP_REAL, NULL, NULL, $1); }
	| VALUE_INT																	{ $$ = new_exp(EXP_INT, NULL, NULL, $1); }
	| T_BYTESIZEOF '(' T_ID ')'													{ $$ = new_exp(EXP_BYTESIZEOF, NULL, NULL, $3); }
	| T_BYTESIZEOF '(' T_THIS ')'												{ $$ = new_exp(EXP_BYTESIZEOF, NULL, NULL, -1); }
;

enumcfg: 
	notelist T_ENUM T_ID '{' enumitemlist notelist '}'							{ $$ = new_projectitem(PI_ENUMCFG, new_enumcfg($3, $5, $1)); }
;

enumitemlist: 
	enumitem																	{ $$ = $1; }
	| enumitemlist ',' enumitem													{ $$ = append_enumitem($1, $3); }
;

enumitem:
	notelist T_ID '=' VALUE_INT													{ $$ = new_enumitem($2, $4, $1); }
	| notelist T_ID																{ $$ = new_enumitem($2, -1, $1); }
;

notelist:																		{ $$ = NULL; }
	| notelist T_NOTE															{ $$ = append_note($1, new_note($2)); }
;



%%

