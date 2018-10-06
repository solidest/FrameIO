#pragma once

/*

fparser.h

解析器使用到的函数及数据结构

*/

#define ERROR_CODE_SYMBOL			-1	//词法错误
#define ERROR_CODE_SYNTAX			-2	//语法错误
#define ERROR_CODE_0				0	//开始位置错误

int get_utf8_length(char *str, int clen);

//注释
struct NOTE
{
	int notesyid;
	struct NOTE * nextnote;
} ;


//项目成员类型
enum projectitemtype
{
	PI_SYSTEM = 1,
	PI_FRAME,
	PI_ENUMCFG
};

//系统成员类型
enum systitemtype
{
	SYSI_PROPERTY = 1,
	SYSI_CHANNEL,
	SYSI_ACTION
};

//系统属性类型
enum syspropertytype
{
	SYSPT_BOOL = 1,
	SYSPT_BYTE,
	SYSPT_SBYTE,
	SYSPT_USHORT,
	SYSPT_SHORT,
	SYSPT_UINT,
	SYSPT_INT,
	SYSPT_ULONG,
	SYSPT_LONG,
	SYSPT_FLOAT,
	SYSPT_DOUBLE
};

//通道类型
enum syschanneltype
{
	SCHT_COM = 1,
	SCHT_CAN,
	SCHT_TCPSERVER,
	SCHT_TCPCLIENT,
	SCHT_UDP,
	SCHT_DI,
	SCHT_DO
};

//通道选项
enum channeloptiontype
{
	CHOP_DEVICEID = 1,
	CHOP_BAUDRATE
};

//IO操作类型
enum actioniotype
{
	AIO_SEND = 1,
	AIO_RECV,
	AIO_RECVLOOP
};

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

	SEGPV_SMALL = 100,
	SEGPV_BIG,
	SEGPV_PRIMITIVE,
	SEGPV_INVERSION,
	SEGPV_COMPLEMENT,

	SEGPV_SUM8 = 200,
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

	SEGPV_NONAMEFRAME = 300,
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
struct SEGPROPERTY
{
	segpropertytype proname;
	segpropertyvaluetype vtype;
	int iv;
	void * pv;
	struct SEGPROPERTY * nextsegpro;
} ;

//表达式
struct EXPVALUE
{
	exptype valuetype;
	struct EXPVALUE* lexp;
	struct EXPVALUE* rexp;
	int valuesyid;
} ;

//oneoflist
struct ONEOFITEM
{
	int enumitemsyid;
	int framesyid;
	struct ONEOFITEM * nextitem;
} ;

//字段
struct SEGMENT
{
	segmenttype segtype;
	NOTE  * notes;
	int namesyid;
	SEGPROPERTY* segpropertylist;
	struct SEGMENT * nextsegment;
} ;

//数据帧
struct FRAME
{
	int namesyid;
	NOTE  * notes;
	SEGMENT *seglist;
	struct FRAME * nextframe;
} ;



//通道参数选项
struct CHANNELOPTION
{
	NOTE  * notes;
	channeloptiontype optiontype;
	int valuesyid;
	struct CHANNELOPTION * nextoption;
} ;

//通道
struct CHANNEL
{
	NOTE  * notes;
	int namesyid;
	syschanneltype channeltype;
	CHANNELOPTION * channeloption;
	struct CHANNEL * nextchannel;
} ;

//动作-映射
struct ACTIONMAP
{
	NOTE  * notes;
	int segsyid;
	int prosyid;
	struct ACTIONMAP * nextmap;
} ;

//动作
struct ACTION
{
	NOTE  * notes;
	int namesyid;
	actioniotype iotype;
	int framesyid;
	int channelsyid;
	ACTIONMAP * maplist;
	struct ACTION * nextaction;
} ;

//系统-属性
struct SYSPROPERTY
{
	NOTE  * notes;
	syspropertytype protype;
	int namesyid;
	bool isarray;
	struct SYSPROPERTY * nextsysproperty;
} ;

//系统内部成员
struct SYSITEM
{
	systitemtype itemtype;
	void* item;
} ;

//系统内部成员列表
struct SYSITEMLIST
{
	SYSPROPERTY* sysprolist;
	CHANNEL* channellist;
	ACTION* actionlist;
};

//系统
struct SYS
{
	int namesyid;
	NOTE  * notes;
	SYSPROPERTY * propertylist;
	CHANNEL * channellist;
	ACTION * actionlist;
	struct SYS * nextsys;
};



//枚举项
struct ENUMITEM
{
	NOTE  * notes;
	int itemsyid;
	int valuesyid;
	struct ENUMITEM * nextitem;
} ;


//枚举定义
struct ENUMCFG
{
	int namesyid;
	ENUMITEM * enumitemlist;
	NOTE  * notes;
	struct ENUMCFG * nextenumcfg;
} ;

//项目内部成员
struct PROJECTITEM
{
	projectitemtype itemtype;
	void* item;
} ;

//项目内部成员列表
struct PROJECTITEMLIST
{
	SYS* syslist;
	FRAME* framelist;
	ENUMCFG* enumcfglist;
};

//项目
struct PROJECT
{
	int namesyid;
	struct NOTE * notes;
	struct SYS * syslist;
	struct FRAME * framelist;
	struct ENUMCFG * enumcfglist;
} ;


NOTE* new_note(int notesyid);
NOTE* append_note(NOTE* list, NOTE* lastitem);

ENUMITEM* new_enumitem(int itemsyid, int valuesyid, NOTE  * notes);
ENUMITEM* append_enumitem(ENUMITEM* enumitemlist, ENUMITEM* lastenumitem);
ENUMCFG* new_enumcfg(int namesyid, ENUMITEM * enumitemlist, NOTE  * notes);


EXPVALUE* new_exp(exptype valuetype, EXPVALUE * lexp, EXPVALUE * rexp, int valuesyid=-1);
SEGPROPERTY* new_segproperty(segpropertytype segprotype, segpropertyvaluetype segprovtype, int iv=-1, void* pv=0);
SEGPROPERTY* append_segproperty(SEGPROPERTY* segprolist, SEGPROPERTY* lassegpro);
ONEOFITEM* new_oneofitem(int enumitemsyid, int framesyid);
ONEOFITEM* append_oneofitem(ONEOFITEM* list, ONEOFITEM* lastitem);
SEGMENT* new_segment(segmenttype segtype, int namesyid, SEGPROPERTY* prolist, NOTE* notes);
SEGMENT* append_segment(SEGMENT* list, SEGMENT* lastitem);
FRAME* new_frame(int namesyid, SEGMENT* seglist, NOTE* notes);

SYSPROPERTY* new_sysproperty(int namesyid, syspropertytype protype, bool isarray, NOTE* notes);
CHANNEL* new_syschannel(int namesyid, syschanneltype chtype, CHANNELOPTION* oplist, NOTE* notes);
CHANNELOPTION* new_channeloption(channeloptiontype optype, int valuesyid, NOTE* notes);
CHANNELOPTION* append_channeloption(CHANNELOPTION* list, CHANNELOPTION* lastitem);
ACTIONMAP* new_actionmap(int segsyid, int prosyid, NOTE* notes);
ACTIONMAP* append_actionmap(ACTIONMAP* list, ACTIONMAP* lastitem);
ACTION* new_action(int namesyid, actioniotype iotype, int framesyid, int channelsyid, ACTIONMAP* maplist, NOTE* notes);
SYSITEM* new_sysitem(systitemtype itype, void* item);
SYSITEMLIST* add_sysitem(SYSITEMLIST* list, SYSITEM* item);
SYS* new_sys(int namesyid, SYSITEMLIST* list, NOTE* notes);

PROJECTITEM* new_projectitem(projectitemtype itype, void* item);
PROJECTITEMLIST* add_projectitem(PROJECTITEMLIST* list, PROJECTITEM* item);
PROJECT* new_project(int namesyid, PROJECTITEMLIST* itemlist, NOTE* notes);

void free_project(PROJECT * project);
void free_projectitem(PROJECTITEM* pitem);
void free_projectitemlist(PROJECTITEMLIST* pitemlist);
void free_sysitem(SYSITEM* sysitem);
void free_sysitemlist(SYSITEMLIST* sysitemlist);
void free_channeloption(CHANNELOPTION* choplist);
void free_actionmap(ACTIONMAP* amaplist);
void free_segment(SEGMENT* seglist);
void free_segproperty(SEGPROPERTY* segprolist);
void free_expvalue(EXPVALUE* valueexp);
void free_oneofitem(ONEOFITEM* oneofitemlist);
void free_enumitem(ENUMITEM * enumitemlist);
void free_note(NOTE* notelist);