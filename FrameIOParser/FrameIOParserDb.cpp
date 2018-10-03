#include "stdafx.h"
#include "FrameIOParserDb.h"

//初始化
FrameIOParserDb::FrameIOParserDb()
{
	sqlite3_config(SQLITE_CONFIG_URI, 1);
	//int res = sqlite3_open("file::memory:?cache=shared", &m_pDB);
	int res = sqlite3_open("file:C:/Kiyun/FrameIO/FrameIO/x64/Debug/frameio.db", &m_pDB);
	if (res != SQLITE_OK)
	{
		sqlite3_close(m_pDB);
		m_pDB = NULL;
		return;
	}

	res = sqlite3_prepare_v2(m_pDB, "SELECT code FROM fio_project WHERE rowid = :rowid;", -1, &m_fio_project_stmt, NULL);
	if (res != SQLITE_OK)
	{
		m_fio_project_stmt = NULL;
	}

	res = sqlite3_prepare_v2(m_pDB, "INSERT INTO fio_symbol (projectid, symbol, lineno, firstcolumn, lastcolumn) VALUES(:projectid, :symbol, :lineno, :firstcolumn, :lastcolumn);", -1, &m_fio_symbol_stmt, NULL);
	if (res != SQLITE_OK)
	{
		m_fio_symbol_stmt = NULL;
	}
	
	res = sqlite3_prepare_v2(m_pDB, "INSERT INTO fio_error (projectid, errorcode, firstsyid, lastsyid) VALUES(:projectid, :errorcode, :firstsyid, :lastsyid);", -1, &m_fio_error_stmt, NULL);
	if (res != SQLITE_OK)
	{
		m_fio_error_stmt = NULL;
	}
}

//析构函数 释放资源
FrameIOParserDb::~FrameIOParserDb()
{
	if (m_projectcode)
	{
		delete(m_projectcode);
		m_projectcode = NULL;
	}

	if (m_fio_symbol_stmt)
	{
		sqlite3_finalize(m_fio_symbol_stmt);
		m_fio_symbol_stmt = NULL;
	}

	if (m_fio_project_stmt)
	{
		sqlite3_finalize(m_fio_project_stmt);
		m_fio_project_stmt = NULL;
	}

	if (m_fio_error_stmt)
	{
		sqlite3_finalize(m_fio_error_stmt);
		m_fio_error_stmt = NULL;
	}

	if (m_pDB)
	{
		sqlite3_close(m_pDB);
		m_pDB = NULL;
	}
}

//保存错误信息
int FrameIOParserDb::SaveError(int errorcode, int firstsyid, int lastsyid)
{
	sqlite3_bind_int(m_fio_error_stmt, 1, m_projectid);
	sqlite3_bind_int(m_fio_error_stmt, 2, errorcode);
	sqlite3_bind_int(m_fio_error_stmt, 3, firstsyid);
	sqlite3_bind_int(m_fio_error_stmt, 4, lastsyid);
	int rc = sqlite3_step(m_fio_error_stmt);
	if ((rc != SQLITE_DONE) && (rc != SQLITE_ROW))
	{
		return -1;
	}
	sqlite3_reset(m_fio_error_stmt);
	return (int)sqlite3_last_insert_rowid(m_pDB);
}

//加载项目代码
bool FrameIOParserDb::LoadProject(int projectid)
{
	m_projectid = projectid;

	sqlite3_bind_int(m_fio_project_stmt, 1, m_projectid);
	int res = sqlite3_step(m_fio_project_stmt);
	if ((res != SQLITE_DONE) && (res != SQLITE_ROW))
	{
		m_codesize = -1;
		if (m_projectcode)
		{
			delete m_projectcode;
			m_projectcode = NULL;
		}
		return false;
	}

	const char* code = (const char*)sqlite3_column_text(m_fio_project_stmt, 0);
	if (!code) return false;
	int len = (int)strlen(code);
	m_codesize = len;
	if (m_projectcode) delete m_projectcode;
	m_projectcode = new char[len+1];
	strcpy_s(m_projectcode, len+1, code);
	
	sqlite3_reset(m_fio_project_stmt);
	return true;

}

//保存符号
int FrameIOParserDb::SaveSymbol(const char* symbol, int lineno, int firstcolumn, int lastcolumn)
{
	sqlite3_bind_int(m_fio_symbol_stmt, 1, m_projectid);
	sqlite3_bind_text(m_fio_symbol_stmt, 2, symbol, -1, SQLITE_STATIC);
	sqlite3_bind_int(m_fio_symbol_stmt, 3, lineno);
	sqlite3_bind_int(m_fio_symbol_stmt, 4, firstcolumn);
	sqlite3_bind_int(m_fio_symbol_stmt, 5, lastcolumn);
	int rc = sqlite3_step(m_fio_symbol_stmt);
	if ((rc != SQLITE_DONE) && (rc != SQLITE_ROW))
	{
		return -1;
	}
	sqlite3_reset(m_fio_symbol_stmt);
	return (int)sqlite3_last_insert_rowid(m_pDB);
}


//保存项目ast
void FrameIOParserDb::SaveProject(PROJECT* pj)
{
	auto _pj = pj;
	auto _name = pj->namesyid;
}

