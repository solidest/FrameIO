
#include "fparser.h"

#include <iostream>
#include <istream>
#include <streambuf>
#include <string>

NOTE* new_note(int notesyid)
{
	auto ret = new NOTE;
	ret->notesyid = notesyid;
	ret->nextnote = NULL;
	return (NOTE*)ret;
}

NOTE* append_note(NOTE* list, NOTE* lastitem)
{
	if (!list)
	{
		return lastitem;
	}
	NOTE* n = list;
	while (n->nextnote)
	{
		n = n->nextnote;
	}
	n->nextnote = lastitem;
	return list;
}

ENUMITEM* new_enumitem(int itemsyid, int valuesyid, NOTE  * notes)
{
	auto ret = new ENUMITEM;
	ret->itemsyid = itemsyid;
	ret->valuesyid = valuesyid;
	ret->notes = notes;
	ret->nextitem = NULL;
	return ret;
}

ENUMITEM* append_enumitem(ENUMITEM* enumitemlist, ENUMITEM* lastenumitem)
{
	if (!enumitemlist)
	{
		return lastenumitem;
	}
	auto n = enumitemlist;
	while (n->nextitem)
	{
		n = n->nextitem;
	}
	n->nextitem = lastenumitem;
	return enumitemlist;
}

ENUMCFG* new_enumcfg(int namesyid, ENUMITEM * enumitemlist, NOTE  * notes)
{
	auto ret = new ENUMCFG;
	ret->namesyid = namesyid;
	ret->enumitemlist = enumitemlist;
	ret->notes = notes;
	ret->nextenumcfg = NULL;
	return ret;
}

EXPVALUE* new_exp(exptype valuetype, EXPVALUE * lexp, EXPVALUE * rexp, int valuesyid)
{
	auto ret = new EXPVALUE;
	ret->valuetype = valuetype;
	ret->lexp = lexp;
	ret->rexp = rexp;
	ret->valuesyid = valuesyid;
	return ret;
}

SEGPROPERTY* new_segproperty(segpropertytype segprotype, segpropertyvaluetype segprovtype, int iv, void* pv)
{
	auto ret = new SEGPROPERTY;
	ret->pro = segprotype;
	ret->vtype = segprovtype;
	ret->iv = iv;
	ret->pv = pv;
	ret->nextsegpro = NULL;
	return ret;
}

SEGPROPERTY* append_segproperty(SEGPROPERTY* segprolist, SEGPROPERTY* lassegpro)
{
	if (!segprolist)
	{
		return lassegpro;
	}
	auto n = segprolist;
	while (n->nextsegpro)
	{
		n = n->nextsegpro;
	}
	n->nextsegpro = lassegpro;
	return segprolist;
}

ONEOFITEM* new_oneofitem(int enumitemsyid, int framesyid)
{
	auto ret = new ONEOFITEM;
	ret->enumitemsyid = enumitemsyid;
	ret->framesyid = framesyid;
	ret->nextitem = NULL;
	return ret;
}

ONEOFITEM* append_oneofitem(ONEOFITEM* list, ONEOFITEM* lastitem)
{
	if (!list)
	{
		return lastitem;
	}
	auto n = list;
	while (n->nextitem)
	{
		n = n->nextitem;
	}
	n->nextitem = lastitem;
	return list;
}

SEGMENT* new_segment(segmenttype segtype, int namesyid, SEGPROPERTY* prolist, NOTE* notes)
{
	auto ret = new SEGMENT;
	ret->namesyid = namesyid;
	ret->segtype = segtype;
	ret->segpropertylist = prolist;
	ret->notes = notes;
	ret->nextsegment = NULL;
	return ret;
}

SEGMENT* append_segment(SEGMENT* list, SEGMENT* lastitem)
{
	if (!list)
	{
		return lastitem;
	}
	auto n = list;
	while (n->nextsegment)
	{
		n = n->nextsegment;
	}
	n->nextsegment = lastitem;
	return list;
}

FRAME* new_frame(int namesyid, SEGMENT* seglist, NOTE* notes)
{
	auto ret = new FRAME;
	ret->namesyid = namesyid;
	ret->seglist = seglist;
	ret->notes = notes;
	ret->nextframe = NULL;
	return ret;
}




SYSPROPERTY* new_sysproperty(int namesyid, syspropertytype protype, bool isarray, NOTE* notes)
{
	auto ret = new SYSPROPERTY;
	ret->namesyid = namesyid;
	ret->protype = protype;
	ret->notes = notes;
	ret->nextsysproperty = NULL;
	return ret;
}

CHANNEL* new_syschannel(int namesyid, syschanneltype chtype, CHANNELOPTION* oplist, NOTE* notes)
{
	auto ret = new CHANNEL;
	ret->namesyid = namesyid;
	ret->channeltype = chtype;
	ret->channeloption = oplist;
	ret->notes = notes;
	ret->nextchannel = NULL;
	return ret;
}

CHANNELOPTION* new_channeloption(channeloptiontype optype, int valuesyid, NOTE* notes)
{
	auto ret = new CHANNELOPTION;
	ret->nextoption = NULL;
	ret->notes = notes;
	ret->optiontype = optype;
	ret->valuesyid = valuesyid;
	return ret;
}

CHANNELOPTION* append_channeloption(CHANNELOPTION* list, CHANNELOPTION* lastitem)
{
	if (!list)
	{
		return lastitem;
	}
	auto n = list;
	while (n->nextoption)
	{
		n = n->nextoption;
	}
	n->nextoption = lastitem;
	return list;
}

ACTIONMAP* new_actionmap(int segsyid, int prosyid, NOTE* notes)
{
	auto ret = new ACTIONMAP;
	ret->nextmap = NULL;
	ret->segsyid = segsyid;
	ret->prosyid = prosyid;
	ret->notes = notes;
	return ret;
}

ACTIONMAP* append_actionmap(ACTIONMAP* list, ACTIONMAP* lastitem)
{
	if (!list)
	{
		return lastitem;
	}
	auto n = list;
	while (n->nextmap)
	{
		n = n->nextmap;
	}
	n->nextmap = lastitem;
	return list;
}

ACTION* new_action(int namesyid, actioniotype iotype, int framesyid, int channelsyid, ACTIONMAP* maplist, NOTE* notes)
{
	auto ret = new ACTION;
	ret->channelsyid = channelsyid;
	ret->framesyid = framesyid;
	ret->iotype = iotype;
	ret->maplist = maplist;
	ret->namesyid = namesyid;
	ret->nextaction = NULL;
	ret->notes = notes;
	return ret;
}

SYSITEM* new_sysitem(systitemtype itype, void* item)
{
	auto ret = new SYSITEM;
	ret->item = item;
	ret->itemtype = itype;
	return ret;
}

SYSPROPERTY* append_sysproperty(SYSPROPERTY* list, SYSPROPERTY* lastitem)
{
	if (!list)
	{
		return lastitem;
	}
	auto n = list;
	while (n->nextsysproperty)
	{
		n = n->nextsysproperty;
	}
	n->nextsysproperty = lastitem;
	return list;
}

CHANNEL* append_channel(CHANNEL* list, CHANNEL* lastitem)
{
	if (!list)
	{
		return lastitem;
	}
	auto n = list;
	while (n->nextchannel)
	{
		n = n->nextchannel;
	}
	n->nextchannel = lastitem;
	return list;
}

ACTION* append_action(ACTION* list, ACTION* lastitem)
{
	if (!list)
	{
		return lastitem;
	}
	auto n = list;
	while (n->nextaction)
	{
		n = n->nextaction;
	}
	n->nextaction = lastitem;
	return list;
}


SYSITEMLIST* add_sysitem(SYSITEMLIST* list, SYSITEM* item)
{
	if (!list)
	{
		list = new SYSITEMLIST;
		list->actionlist = NULL;
		list->channellist = NULL;
		list->sysprolist = NULL;
		switch (item->itemtype)
		{
		case SYSI_PROPERTY:
			list->sysprolist = (SYSPROPERTY*)(item->item);
			break;
		case SYSI_CHANNEL:
			list->channellist = (CHANNEL*)(item->item);
			break;
		case SYSI_ACTION:
			list->actionlist = (ACTION*)(item->item);
			break;
		}
	}
	else
	{
		switch (item->itemtype)
		{
		case SYSI_PROPERTY:
			list->sysprolist = append_sysproperty(list->sysprolist, (SYSPROPERTY*)(item->item));
			break;
		case SYSI_CHANNEL:
			list->channellist = append_channel(list->channellist, (CHANNEL*)(item->item));
			break;
		case SYSI_ACTION:
			list->actionlist = append_action(list->actionlist, (ACTION*)(item->item));
			break;
		}
	}
	delete item;
	return list;
}

SYS* new_sys(int namesyid, SYSITEMLIST* list, NOTE* notes)
{
	auto ret = new SYS;
	if (list)
	{
		ret->actionlist = list->actionlist;
		ret->channellist = list->channellist;
		ret->propertylist = list->sysprolist;
		delete list;
	}
	else
	{
		ret->actionlist = NULL;
		ret->channellist = NULL;
		ret->propertylist = NULL;
	}
	ret->namesyid = namesyid;
	ret->nextsys = NULL;
	ret->notes = notes;
	return ret;
}

PROJECTITEM* new_projectitem(projectitemtype itype, void* item)
{
	auto ret = new PROJECTITEM;
	ret->item = item;
	ret->itemtype = itype;
	return ret;
}

SYS* append_sys(SYS* list, SYS* lastitem)
{
	if (!list)
	{
		return lastitem;
	}
	auto n = list;
	while (n->nextsys)
	{
		n = n->nextsys;
	}
	n->nextsys = lastitem;
	return list;
}

FRAME* append_frame(FRAME* list, FRAME* lastitem)
{
	if (!list)
	{
		return lastitem;
	}
	auto n = list;
	while (n->nextframe)
	{
		n = n->nextframe;
	}
	n->nextframe = lastitem;
	return list;
}

ENUMCFG* append_enum(ENUMCFG* list, ENUMCFG* lastitem)
{
	if (!list)
	{
		return lastitem;
	}
	auto n = list;
	while (n->nextenumcfg)
	{
		n = n->nextenumcfg;
	}
	n->nextenumcfg = lastitem;
	return list;
}

PROJECTITEMLIST* add_projectitem(PROJECTITEMLIST* list, PROJECTITEM* item)
{
	if (!list)
	{
		list = new PROJECTITEMLIST;
		list->enumcfglist = NULL;
		list->framelist = NULL;
		list->syslist = NULL;
		switch (item->itemtype)
		{
		case PI_SYSTEM:
			list->syslist = (SYS*)(item->item);
			break;
		case PI_FRAME:
			list->framelist = (FRAME*)(item->item);
			break;
		case PI_ENUMCFG:
			list->enumcfglist = (ENUMCFG*)(item->item);
			break;
		}
	}
	else
	{
		switch (item->itemtype)
		{
		case PI_SYSTEM:
			list->syslist = append_sys(list->syslist, (SYS*)(item->item));
			break;
		case PI_FRAME:
			list->framelist = append_frame(list->framelist, (FRAME*)(item->item));
			break;
		case PI_ENUMCFG:
			list->enumcfglist = append_enum(list->enumcfglist, (ENUMCFG*)(item->item));
			break;
		}
	}
	delete item;
	return list;
}

PROJECT* new_project(int namesyid, PROJECTITEMLIST* itemlist, NOTE* notes)
{
	auto ret = new PROJECT;

	if (itemlist)
	{
		ret->enumcfglist = itemlist->enumcfglist;
		ret->framelist = itemlist->framelist;
		ret->syslist = itemlist->syslist;
		delete itemlist;
	}
	else
	{
		ret->enumcfglist = NULL;
		ret->framelist = NULL;
		ret->syslist = NULL;
	}

	ret->namesyid = namesyid;
	ret->notes = notes;
	return ret;
}

void free_sysproperty(SYSPROPERTY* list)
{
	SYSPROPERTY* next = NULL;
	while (list)
	{
		free_note(list->notes);
		next = list->nextsysproperty;
		delete list;
		list = next;
	}
}

void free_channel(CHANNEL* list)
{
	CHANNEL* next = NULL;
	while (list)
	{
		free_note(list->notes);
		free_channeloption(list->channeloption);
		next = list->nextchannel;
		delete list;
		list = next;
	}
}

void free_action(ACTION* list)
{
	ACTION* next = NULL;
	while (list)
	{
		free_note(list->notes);
		free_actionmap(list->maplist);
		next = list->nextaction;
		delete list;
		list = next;
	}
}

void free_sys(SYS* list)
{
	SYS* next = NULL;
	while (list)
	{
		free_note(list->notes);
		free_action(list->actionlist);
		free_channel(list->channellist);
		free_sysproperty(list->propertylist);
		next = list->nextsys;
		delete list;
		list = next;
	}
}

void free_frame(FRAME* list)
{
	FRAME* next = NULL;
	while (list)
	{
		free_note(list->notes);
		free_segment(list->seglist);
		next = list->nextframe;
		delete list;
		list = next;
	}
}

void free_enum(ENUMCFG* list)
{
	ENUMCFG* next = NULL;
	while (list)
	{
		free_note(list->notes);
		free_enumitem(list->enumitemlist);
		next = list->nextenumcfg;
		delete list;
		list = next;
	}
}

void free_project(PROJECT * project)
{
	if (!project) return;
	free_note(project->notes);
	free_sys(project->syslist);
	free_frame(project->framelist);
	free_enum(project->enumcfglist);
	delete project;
}

void free_projectitem(PROJECTITEM* pitem)
{
	if (!pitem) return;
	switch(pitem->itemtype)
	{
		case PI_SYSTEM:
			free_sys((SYS*)(pitem->item));
			break;
		case PI_FRAME:
			free_frame((FRAME*)(pitem->item));
			break;
		case PI_ENUMCFG:
			free_enum((ENUMCFG*)(pitem->item));
			break;
	}
	delete pitem;
}

void free_projectitemlist(PROJECTITEMLIST* pitemlist)
{
	if (!pitemlist) return;
	free_sys(pitemlist->syslist);
	free_enum(pitemlist->enumcfglist);
	free_frame(pitemlist->framelist);
	delete pitemlist;
}

void free_sysitem(SYSITEM* sysitem)
{
	if (!sysitem) return;
	switch (sysitem->itemtype)
	{
	case SYSI_PROPERTY:
		free_sysproperty((SYSPROPERTY*)(sysitem->item));
		break;
	case SYSI_CHANNEL:
		free_channel((CHANNEL*)(sysitem->item));
		break;
	case SYSI_ACTION:
		free_action((ACTION*)(sysitem->item));
		break;
	}
	delete sysitem;
}

void free_sysitemlist(SYSITEMLIST* sysitemlist)
{
	if (!sysitemlist) return;
	free_sysproperty(sysitemlist->sysprolist);
	free_channel(sysitemlist->channellist);
	free_action(sysitemlist->actionlist);
	delete sysitemlist;
}

void free_channeloption(CHANNELOPTION* choplist)
{
	CHANNELOPTION* next = NULL;
	while (choplist)
	{
		free_note(choplist->notes);
		next = choplist->nextoption;
		delete choplist;
		choplist = next;
	}
}

void free_actionmap(ACTIONMAP* amaplist)
{
	ACTIONMAP* next = NULL;
	while (amaplist)
	{
		free_note(amaplist->notes);
		next = amaplist->nextmap;
		delete amaplist;
		amaplist = next;
	}
}

void free_segment(SEGMENT* seglist)
{
	SEGMENT* next = NULL;
	while (seglist)
	{
		free_note(seglist->notes);
		free_segproperty(seglist->segpropertylist);
		next = seglist->nextsegment;
		delete seglist;
		seglist = next;
	}
}

void free_segproperty(SEGPROPERTY* segprolist)
{
	SEGPROPERTY* next = NULL;
	while (segprolist)
	{
		if (segprolist->pv)
		{
			switch (segprolist->vtype)
			{
			case SEGPV_NONAMEFRAME:
				free_segment((SEGMENT*)segprolist->pv);
				break;
			case SEGPV_ONEOF:
				free_oneofitem((ONEOFITEM*)segprolist->pv);
				break;
			case SEGPV_EXP:
				free_expvalue((EXPVALUE*)segprolist->pv);
				break;
			}
		}
		next = segprolist->nextsegpro;
		delete segprolist;
		segprolist = next;
	}
}

void free_expvalue(EXPVALUE* valueexp)
{
	if (!valueexp) return;
	free_expvalue(valueexp->lexp);
	free_expvalue(valueexp->rexp);
	delete valueexp;
}

void free_oneofitem(ONEOFITEM* oneofitemlist)
{
	ONEOFITEM* next = NULL;
	while (oneofitemlist)
	{
		next = oneofitemlist->nextitem;
		delete oneofitemlist;
		oneofitemlist = next;
	}
}

void free_enumitem(ENUMITEM * enumitemlist)
{
	ENUMITEM* next = NULL;
	while (enumitemlist)
	{
		free_note(enumitemlist->notes);
		next = enumitemlist->nextitem;
		delete enumitemlist;
		enumitemlist = next;
	}
}

void free_note(NOTE* notelist)
{
	NOTE* next = NULL;
	while (notelist)
	{
		next = notelist->nextnote;
		delete notelist;
		notelist = next;
	}
}

unsigned char utf8_look_for_table[] =
{
	1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
	1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
	1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
	1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
	1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
	1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
	1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
	1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
	1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
	1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
	1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
	1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
	2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
	2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
	3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
	4, 4, 4, 4, 4, 4, 4, 4, 5, 5, 5, 5, 6, 6, 1, 1
};


#define UTFLEN(x)  utf8_look_for_table[(x)]


//¼ÆËãstr×Ö·ûÊýÄ¿
int get_utf8_length(char *str, int clen)
{
	int len = 0;
	for (char *ptr = str;
		*ptr != 0 && len < clen;
		len++, ptr += UTFLEN((unsigned char)*ptr));
	return len;
}


