#pragma once

/*

fparser.h

������ʹ�õ��ĺ��������ݽṹ

*/

#define ERROR_CODE_SYMBOL			-1	//�ʷ�����
#define ERROR_CODE_SYNTAX			-2	//�﷨����

int get_utf8_length(char *str, int clen);

//ע��
typedef struct note
{
	int notesyid;
	struct note * nextnote;
} NOTE;

//��Ŀ��Ա����
enum projectitemtype
{
	PI_SYSTEM = 1,
	PI_FRAME,
	PI_ENUMCFG
};

//ϵͳ��Ա����
enum systitemtype
{
	SYSI_PROPERTY = 1,
	SYSI_CHANNEL,
	SYSI_ACTION
};

//ϵͳ��������
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

//ͨ������
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

//ͨ��ѡ��
enum channeloptiontype
{
	CHOP_DEVICEID = 1,
	CHOP_BAUDRATE
};

//IO��������
enum actioniotype
{
	AIO_SEND = 1,
	AIO_RECV,
	AIO_RECVLOOP
};

//���ʽ����
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

//�ֶ�����
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

//�ֶ�����ֵ����
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

//�ֶ�����
enum segmenttype
{
	SEGT_INTEGER = 1,
	SEGT_REAL,
	SEGT_BLOCK,
	SEGT_TEXT
};

//�ֶ�����
typedef struct segproperty
{
	segpropertytype pro;
	segpropertyvaluetype vtype;
	int iv;
	void * pv;
	struct segproperty * nextsegpro;
} SEGPROPERTY;

//���ʽ
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

//�ֶ�
typedef struct segment
{
	segmenttype segtype;
	NOTE  * notes;
	int namesyid;
	SEGPROPERTY segpropertylist;
	struct segment * nextsegment;
} SEGMENT;

//����֡
typedef struct frame
{
	int namesyid;
	NOTE  * notes;
	SEGMENT *seglist;
	struct frame * nextframe;
} FRAME;




//ͨ������ѡ��
typedef struct channeloption
{
	NOTE  * notes;
	channeloptiontype optiontype;
	int valuesyid;
	struct option * nextoption;
} CHANNELOPTION;

//ͨ��
typedef struct channel
{
	NOTE  * notes;
	int namesyid;
	syschanneltype channeltype;
	CHANNELOPTION * channeloption;
	struct channel * nextchannel;
} CHANNEL;

//����-ӳ��
typedef struct actionmap
{
	NOTE  * notes;
	int segsyid;
	int prosyid;
	struct actionmap * nextmap;
} ACTIONMAP;

//����
typedef struct action
{
	NOTE  * notes;
	int namesyid;
	actioniotype iotype;
	int framesyid;
	int channelsyid;
	ACTIONMAP * maplist;
	struct action * nextaction;
} ACTION;



//ϵͳ-����
typedef struct sysproperty
{
	NOTE  * notes;
	syspropertytype protype;
	int namesyid;
	bool isarray;
	struct sysproperty * nextsysproperty;
} SYSPROPERTY;

//ϵͳ�ڲ���Ա
typedef struct sysitem
{
	systitemtype itemtype;
	void* item;
} SYSITEM;

//ϵͳ�ڲ���Ա�б�
typedef struct sysitemlist
{
	SYSITEM* sysprolist;
	SYSITEM* channellist;
	SYSITEM* actionlist;
}SYSITEMLIST;

//ϵͳ
typedef struct sys
{
	int namesyid;
	NOTE  * notes;
	SYSPROPERTY * propertylist;
	CHANNEL * channellist;
	ACTION * actionlist;
	struct sys * nextsys;
} SYS;





//ö����
typedef struct enumitem
{
	NOTE  * notes;
	int itemsyid;
	int valuesyid;
	struct enumitem * nextitem;
} ENUMITEM;


//ö�ٶ���
typedef struct enumcfg
{
	int namesyid;
	ENUMITEM * enumitemlist;
	NOTE  * notes;
	struct enumcfg * nextenumcfg;
} ENUMCFG;

//��Ŀ�ڲ���Ա
typedef struct projectitem
{
	projectitemtype itemtype;
	void* item;
} PROJECTITEM;

//��Ŀ�ڲ���Ա�б�
typedef struct projectitemlist
{
	PROJECTITEM* syslist;
	PROJECTITEM* framelist;
	PROJECTITEM* enumcfglist;
}PROJECTITEMLIST;

//��Ŀ
typedef struct project
{
	int namesyid;
	struct NOTE * notes;
	struct SYS * syslist;
	struct FRAME * framelist;
	struct ENUMCFG * enumcfglist;
} PROJECT;


NOTE* new_note(int notesyid);
NOTE* append_note(NOTE* notelist, int notesyid);\

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