
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
	sqlite3_stmt *m_fio_project_stmt;
	sqlite3_stmt *m_fio_error_stmt;

public:
	FrameIOParserDb();
	~FrameIOParserDb();


	int SaveSymbol(const char* symbol, int lineno, int firstcolumn, int lastcolumn);
	int SaveError(int errorcode, int firstsyid, int lastsyid);
	void SaveProject(PROJECT* pj);

	bool LoadProject(int projectid);

	int getProjectId() { return m_projectid; }
	char* getProjectCode() { return m_projectcode; }
	int getCodeSize() { return m_codesize; }

};

