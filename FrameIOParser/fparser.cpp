
#include "fparser.h"

#include <iostream>
#include <istream>
#include <streambuf>
#include <string>

NOTE* new_note(int notesyid)
{
	return NULL;
}

NOTE* append_note(NOTE* notelist, int notesyid)
{
	return NULL;
}

ENUMITEM* new_enumitem(int itemsyid, int valuesyid, NOTE  * notes)
{
	return NULL;

}

ENUMITEM* append_enumitem(ENUMITEM* enumitemlist, ENUMITEM* lastenumitem)
{
	return NULL;

}

ENUMCFG* new_enumcfg(int namesyid, ENUMITEM * enumitemlist, NOTE  * notes)
{
	return NULL;
}


EXPVALUE* new_exp(exptype valuetype, EXPVALUE * lexp, EXPVALUE * rexp, int valuesyid)
{
	return NULL;
}

SEGPROPERTY* new_segproperty(segpropertytype segprotype, segpropertyvaluetype segprovtype, int iv, void* pv)
{
	return NULL;
}

SEGPROPERTY* append_segproperty(SEGPROPERTY* segprolist, SEGPROPERTY* lassegpro)
{
	return NULL;
}

ONEOFITEM* new_oneofitem(int enumitemsyid, int framesyid)
{
	return NULL;
}

ONEOFITEM* append_oneofitem(ONEOFITEM* list, ONEOFITEM* lastitem)
{
	return NULL;
}

SEGMENT* new_segment(segmenttype segtype, int namesyid, SEGPROPERTY* prolist, NOTE* notes)
{
	return NULL;
}

SEGMENT* append_segment(SEGMENT* list, SEGMENT* lastitem)
{
	return NULL;
}

FRAME* new_frame(int namesyid, SEGMENT* seglist, NOTE* notes)
{
	return NULL;
}




SYSPROPERTY* new_sysproperty(int namesyid, syspropertytype protype, bool isarray, NOTE* notes)
{
	return NULL;
}

CHANNEL* new_syschannel(int namesyid, syschanneltype chtype, CHANNELOPTION* oplist, NOTE* notes)
{
	return NULL;
}

SYSITEM* new_sysitem(systitemtype itype, void* item)
{
	return NULL;
}

CHANNELOPTION* new_channeloption(channeloptiontype optype, int valuesyid, NOTE* notes)
{
	return NULL;
}

CHANNELOPTION* append_channeloption(CHANNELOPTION* list, CHANNELOPTION* lastitem)
{
	return NULL;
}

ACTIONMAP* new_actionmap(int segsyid, int prosyid, NOTE* notes)
{
	return NULL;
}

ACTIONMAP* append_actionmap(ACTIONMAP* list, ACTIONMAP* lastitem)
{
	return NULL;
}

ACTION* new_action(int namesyid, actioniotype iotype, int framesyid, int channelsyid, ACTIONMAP* maplist, NOTE* notes)
{
	return NULL;
}

SYSITEMLIST* add_sysitem(SYSITEMLIST* list, SYSITEM* item)
{
	return NULL;
}

SYS* new_sys(int namesyid, SYSITEMLIST* list, NOTE* notes)
{
	return NULL;
}



PROJECTITEM* new_projectitem(projectitemtype itype, void* item)
{
	return NULL;
}

PROJECTITEMLIST* add_projectitem(PROJECTITEMLIST* list, PROJECTITEM* item)
{
	return NULL;
}

PROJECT* new_project(int namesyid, PROJECTITEMLIST* itemlist, NOTE* notes)
{
	return NULL;
}

void free_project(PROJECT * project)
{

}

void free_projectitem(PROJECTITEM* pitem)
{

}

void free_projectitemlist(PROJECTITEMLIST* pitemlist)
{

}

void free_sysitem(SYSITEM* sysitem)
{

}

void free_sysitemlist(SYSITEMLIST* sysitemlist)
{

}

void free_channeloption(CHANNELOPTION* choplist)
{

}

void free_actionmap(ACTIONMAP* amaplist)
{

}

void free_segment(SEGMENT* seglist)
{

}

void free_segproperty(SEGPROPERTY* segprolist)
{

}

void free_expvalue(EXPVALUE* valueexp)
{

}

void free_oneofitem(ONEOFITEM* oneofitemlist)
{

}

void free_enumitem(ENUMITEM * enumitemlist)
{

}

void free_note(NOTE* notelist)
{

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


