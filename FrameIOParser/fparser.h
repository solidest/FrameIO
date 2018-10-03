#pragma once

/*

fparser.h

解析器使用到的函数及数据结构

*/

#define ERROR_CODE_SYMBOL			-1	//词法错误
#define ERROR_CODE_SYNTAX			-2	//语法错误

int get_utf8_length(char *str, int clen);

//注释
typedef struct note
{
	int notesyid;
	struct note * nextnote;
} NOTE;


//表达式类型
enum exptype
{
	EXP_INT = 1,
	EXP_REAL,
	EXP_ID,
	EXP_BYTESIZEOF,
	EXP_ADD,
	EXP_SUB,
	EXP_MUL,
	EXP_DIV
};

//字段属性
enum segpropertytype
{
	SEGP_SIGNED = 1,
	SEGP_BITCOUNT,
	SEGP_VALUE,
	SEGP_REPEATED,
	SEGP_BYTEORDER,
	SEGP_ENCODED,
	SEGP_ISDOUBLE,
	SEGP_TAIL,
	SEGP_ALIGNEDLEN,
	SEGP_TYPE,
	SEGP_BYTESIZE,
	SEGP_TOENUM,
	SEGP_MAX,
	SEGP_MIN,
	SEGP_CHECK,
	SEGP_CHECKRANGE_BEGIN,
	SEGP_CHECKRANGE_END
};

//字段属性值类型
enum segpropertyvaluetype
{
	SEGPV_INT = 1,
	SEGPV_REAL,
	SEGPV_STRING,
	SEGPV_TRUE,
	SEGPV_FALSE,
	SEGPV_ID,

	SEGPV_SMALL,
	SEGPV_BIG,
	SEGPV_PRIMITIVE,
	SEGPV_INVERSION,
	SEGPV_COMPLEMENT,

	SEGPV_SUM8,
	SEGPV_XOR8,
	SEGPV_SUM16,
	SEGPV_SUM16_FALSE,
	SEGPV_XOR16,
	SEGPV_XOR16_FALSE,
	SEGPV_SUM32,
	SEGPV_SUM32_FALSE,
	SEGPV_XOR32,
	SEGPV_XOR32_FALSE,
	SEGPV_CRC4_ITU,
	SEGPV_CRC5_EPC,
	SEGPV_CRC5_ITU,
	SEGPV_CRC5_USB,
	SEGPV_CRC6_ITU,
	SEGPV_CRC7_MMC,
	SEGPV_CRC8,
	SEGPV_CRC8_ITU,
	SEGPV_CRC8_ROHC,
	SEGPV_CRC8_MAXIM,
	SEGPV_CRC16_IBM,
	SEGPV_CRC16_MAXIM,
	SEGPV_CRC16_USB,
	SEGPV_CRC16_MODBUS,
	SEGPV_CRC16_CCITT,
	SEGPV_CRC16_CCITT_FALSE,
	SEGPV_CRC16_X25,
	SEGPV_CRC16_XMODEM,
	SEGPV_CRC16_DNP,
	SEGPV_CRC32,
	SEGPV_CRC32_MPEG_2,
	SEGPV_CRC64,
	SEGPV_CRC64_WE,

	SEGPV_NONAMEFRAME,
	SEGPV_ONEOF,
	SEGPV_EXP
};

//字段类型
enum segmenttype
{
	SEGT_INTEGER = 1,
	SEGT_REAL,
	SEGT_BLOCK,
	SEGT_TEXT
};

//字段属性
typedef struct segproperty
{
	segpropertytype pro;
	segpropertyvaluetype vtype;
	int iv;
	void * pv;
	struct segproperty * nextsegpro;
} SEGPROPERTY;

//表达式
typedef struct expvalue
{
	exptype valuetype;
	struct expvalue* lexp;
	struct expvalue* rexp;
	int valuesyid;
} EXPVALUE;

//oneoflist
typedef struct segoneofitem
{
	int enumitemsyid;
	int framesyid;
	struct segoneofitem * nextitem;
} ONEOFITEM;


//字段
typedef struct segment
{
	segmenttype segtype;
	NOTE  * notes;
	int namesyid;
	SEGPROPERTY segpropertylist;
	struct segment * nextsegment;
} SEGMENT;


//数据帧
typedef struct frame
{
	int namesyid;
	NOTE  * notes;
	SEGMENT *seglist;
	struct frame * nextframe;
} FRAME;




//选项
typedef struct option
{
	NOTE  * notes;
	int optionid;
	int optionvalue;
	struct option * nextoption;
} OPTION;

//通道
typedef struct channel
{
	NOTE  * notes;
	int namesyid;
	int channeltype;
	OPTION * channeloption;
	struct channel * nextchannel;
} CHANNEL;

//动作-映射
typedef struct actionmap
{
	NOTE  * notes;
	int segmentid;
	int propertyid;
	struct actionmap * nextactionmap;
} ACTIONMAP;

//动作
typedef struct action
{
	NOTE  * notes;
	int namesyid;
	int actiontype;
	int frameid;
	int channelid;
	ACTIONMAP * segmap;
	struct action * nextaction;
} ACTION;

//系统-属性
typedef struct sysproperty
{
	NOTE  * notes;
	int syspropertytype;
	int namesyid;
	int repetated;
	struct sysproperty * nextsysproperty;
} SYSPROPERTY;


//枚举项
typedef struct enumitem
{
	NOTE  * notes;
	int itemsyid;
	int valuesyid;
	struct enumitem * nextitem;
} ENUMITEM;


//枚举定义
typedef struct enumcfg
{
	int namesyid;
	ENUMITEM * enumitemlist;
	NOTE  * notes;
	struct enumcfg * nextenumcfg;
} ENUMCFG;


//系统
typedef struct sys
{
	int namesyid;
	NOTE  * notes;
	SYSPROPERTY * propertylist;
	CHANNEL * channellist;
	ACTION * actionlist;
	struct sys * nextsys;
} SYS;

//项目
typedef struct project
{
	int name_syid;
	struct NOTE * notes;
	struct SYS * syslist;
	struct FRAME * framelist;
	struct ENUMCFG * enumcfglist;
} PROJECT;


NOTE* new_note(int notesyid);
NOTE* append_note(NOTE* notelist, int notesyid);
ENUMITEM* new_enumitem(int itemsyid, int valuesyid, NOTE  * notes);
ENUMITEM* append_enumitem(ENUMITEM* enumitemlist, ENUMITEM* lastenumitem);
ENUMCFG* new_enumcfg(int namesyid, ENUMITEM * enumitemlist, NOTE  * notes);
ENUMCFG* append_enumcfg(ENUMCFG* enumcfglist, ENUMCFG* lastenumcfg);
EXPVALUE* new_exp(exptype valuetype, EXPVALUE * lexp, EXPVALUE * rexp, int valuesyid=-1);
SEGPROPERTY* new_segproperty(segpropertytype segprotype, segpropertyvaluetype segprovtype, int iv=-1, void* pv=0);
SEGPROPERTY* append_segproperty(SEGPROPERTY* segprolist, SEGPROPERTY* lassegpro);
ONEOFITEM* new_oneofitem(int enumitemsyid, int framesyid);
ONEOFITEM* append_oneofitem(ONEOFITEM* list, ONEOFITEM* lastitem);
SEGMENT* new_segment(segmenttype segtype, int namesyid, SEGPROPERTY* prolist, NOTE* notes);
SEGMENT* append_segment(SEGMENT* list, SEGMENT* lastitem);
FRAME* new_frame(int namesyid, SEGMENT* seglist, NOTE* notes);
FRAME* append_frame(FRAME* list, FRAME* lastitem);