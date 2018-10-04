
/*

FrameIOParserDb.h

数据库读写操作类

*/

#pragma once
#include "sqlite3.h"
#include "fparser.h"

class FrameIOParserDb
{
private:
	int m_projectid;
	char* m_projectcode;
	int m_codesize;
	sqlite3 * m_pDB;

	sqlite3_stmt *m_fio_symbol_stmt;
	sqlite3_stmt *m_fio_project_openstmt;
	sqlite3_stmt *m_fio_project_updatestmt;
	sqlite3_stmt *m_fio_error_stmt;
	sqlite3_stmt *m_fio_notes_stmt;
	sqlite3_stmt *m_fio_enum_stmt;
	sqlite3_stmt *m_fio_enum_item_stmt;
	sqlite3_stmt *m_fio_frame_stmt;
	sqlite3_stmt *m_fio_frame_segment_stmt;
	sqlite3_stmt *m_fio_frame_segment_property_stmt;
	sqlite3_stmt *m_fio_frame_oneof_stmt;
	sqlite3_stmt *m_fio_frame_exp_stmt;
	sqlite3_stmt *m_fio_sys_stmt;
	sqlite3_stmt *m_fio_sys_action_stmt;
	sqlite3_stmt *m_fio_sys_action_map_stmt;
	sqlite3_stmt *m_fio_sys_channel_stmt;
	sqlite3_stmt *m_fio_sys_channel_option_stmt;
	sqlite3_stmt *m_fio_sys_property_stmt;

public:
	FrameIOParserDb();
	~FrameIOParserDb();


	int SaveSymbol(const char* symbol, int lineno, int firstcolumn, int lastcolumn);
	int SaveError(int errorcode, int firstsyid, int lastsyid);
	int SaveProject(PROJECT* pj);

	bool LoadProject(int projectid);

	int getProjectId() { return m_projectid; }
	char* getProjectCode() { return m_projectcode; }
	int getCodeSize() { return m_codesize; }

private:
	int SaveNotes(NOTE* notes, int forid);
	int SaveEnumcfg(ENUMCFG* ecfg);
	int SaveEnumcfgItems(ENUMITEM* ecfgitem, int ecfgid);
	int SaveFrame(FRAME* frm, int* frmid = NULL);
	int SaveFrameSegment(SEGMENT* seg, int frmid);
	int SaveFrameSegmentProperty(SEGPROPERTY* pro, int segid);
	int SaveFrameOneOf(ONEOFITEM* oit, int proid);
	int SaveFrameExp(EXPVALUE* exp, int proid, int* expid);
	int SaveSys(SYS* si, int* sysid = NULL);
	int SaveSysAction(ACTION* ac, int sysid, int* acid = NULL);
	int SaveSysActionMap(ACTIONMAP* mp, int acid);
	int SaveSysChannel(CHANNEL* ch, int sysid, int* chid = NULL);
	int SaveSysChannelOption(CHANNELOPTION* op, int chid);
	int SaveSysProperty(SYSPROPERTY* pt, int sysid);

};

