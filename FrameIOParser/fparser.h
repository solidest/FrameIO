#pragma once

#define ERROR_CODE_SYMBOL			-1	//�ʷ�����
#define ERROR_CODE_SYNTAX			-2	//�﷨����

int get_utf8_length(char *str, int clen);

//ע��
typedef struct note
{
	int notesyid;
	struct note * nextnote;
} NOTE;

//ѡ��
typedef struct option
{
	NOTE  * notes;
	int optionid;
	int optionvalue;
	struct option * nextoption;
} OPTION;

//ͨ��
typedef struct channel
{
	NOTE  * notes;
	int namesyid;
	int channeltype;
	OPTION * channeloption;
	struct channel * nextchannel;
} CHANNEL;

//����-ӳ��
typedef struct actionmap
{
	NOTE  * notes;
	int segmentid;
	int propertyid;
	struct actionmap * nextactionmap;
} ACTIONMAP;

//����
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

//ϵͳ-����
typedef struct sysproperty
{
	NOTE  * notes;
	int syspropertytype;
	int namesyid;
	int repetated;
	struct sysproperty * nextsysproperty;
} SYSPROPERTY;

//�ֶ�-����
typedef struct segproperty
{
	int segproperty;
	int propertyvalue;
	struct segproperty * nextsegproperty;
} SEGPROPERTY;

//�ֶ�
typedef struct segment
{
	NOTE  * notes;
	int namesyid;
	SEGPROPERTY segpropertylist;
	struct segment * nextsegment;
} SEGMENT;

//ö����
typedef struct enumitem
{
	NOTE  * notes;
	int itemsyid;
	int valueid;
	struct enumitem * nextitem;
} ENUMITEM;


//ö�ٶ���
typedef struct enumcfg
{
	int namesyid;
	NOTE  * notes;
	struct enumcfg * nextenumcfg;
} ENUMCFG;

//����֡
typedef struct frame
{
	int namesyid;
	NOTE  * notes;
	SEGMENT *seglist;
	struct frame * nextframe;
} FRAME;

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

//��Ŀ
typedef struct project
{
	int name_syid;
	struct NOTE * notes;
	struct SYS * syslist;
	struct FRAME * framelist;
	struct ENUMCFG * enumcfglist;
} PROJECT;