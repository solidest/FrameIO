#pragma once

#define ERROR_CODE_SYMBOL			-1	//词法错误
#define ERROR_CODE_SYNTAX			-2	//语法错误

int get_utf8_length(char *str, int clen);

//注释
typedef struct note
{
	int notesyid;
	struct note * nextnote;
} NOTE;

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

//字段-属性
typedef struct segproperty
{
	int segproperty;
	int propertyvalue;
	struct segproperty * nextsegproperty;
} SEGPROPERTY;

//字段
typedef struct segment
{
	NOTE  * notes;
	int namesyid;
	SEGPROPERTY segpropertylist;
	struct segment * nextsegment;
} SEGMENT;

//枚举项
typedef struct enumitem
{
	NOTE  * notes;
	int itemsyid;
	int valueid;
	struct enumitem * nextitem;
} ENUMITEM;


//枚举定义
typedef struct enumcfg
{
	int namesyid;
	NOTE  * notes;
	struct enumcfg * nextenumcfg;
} ENUMCFG;

//数据帧
typedef struct frame
{
	int namesyid;
	NOTE  * notes;
	SEGMENT *seglist;
	struct frame * nextframe;
} FRAME;

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