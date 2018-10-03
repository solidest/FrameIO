
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

ENUMCFG* append_enumcfg(ENUMCFG* enumcfglist, ENUMCFG* lastenumcfg)
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

FRAME* append_frame(FRAME* list, FRAME* lastitem)
{
	return NULL;
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


