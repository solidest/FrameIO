#include "stdafx.h"
#include "FrameIOParserDb.h"

//初始化
FrameIOParserDb::FrameIOParserDb()
{
	sqlite3_config(SQLITE_CONFIG_URI, 1);
	int res = sqlite3_open("file::memory:?cache=shared", &m_pDB);
	//int res = sqlite3_open("file:C:/Kiyun/FrameIO/FrameIO/x64/Debug/frameio.db", &m_pDB);
	if (res != SQLITE_OK)
	{
		sqlite3_close(m_pDB);
		m_pDB = NULL;
		return;
	}

	res = sqlite3_prepare_v2(m_pDB, "SELECT code FROM fio_project WHERE rowid = :rowid;", -1, &m_fio_project_openstmt, NULL);
	if (res != SQLITE_OK)
	{
		m_fio_project_openstmt = NULL;
	}

	res = sqlite3_prepare_v2(m_pDB, "UPDATE fio_project SET namesyid = :namesyid WHERE rowid = :rowid;", -1, &m_fio_project_updatestmt, NULL);
	if (res != SQLITE_OK)
	{
		m_fio_project_updatestmt = NULL;
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

	res = sqlite3_prepare_v2(m_pDB, "INSERT INTO fio_notes (projectid, notesyid, aftersyid) VALUES(:projectid, :notesyid, :aftersyid);", -1, &m_fio_notes_stmt, NULL);
	if (res != SQLITE_OK)
	{
		m_fio_notes_stmt = NULL;
	}

	res = sqlite3_prepare_v2(m_pDB, "INSERT INTO fio_enum (projectid, namesyid) VALUES(:projectid, :namesyid);", -1, &m_fio_enum_stmt, NULL);
	if (res != SQLITE_OK)
	{
		m_fio_enum_stmt = NULL;
	}

	res = sqlite3_prepare_v2(m_pDB, "INSERT INTO fio_enum_item (projectid, enumid, namesyid, integervid) VALUES(:projectid, :enumid, :namesyid, :integervid);", -1, &m_fio_enum_item_stmt, NULL);
	if (res != SQLITE_OK)
	{
		m_fio_enum_item_stmt = NULL;
	}

	res = sqlite3_prepare_v2(m_pDB, "INSERT INTO fio_frame (projectid, namesyid) VALUES(:projectid, :namesyid);", -1, &m_fio_frame_stmt, NULL);
	if (res != SQLITE_OK)
	{
		m_fio_frame_stmt = NULL;
	}

	res = sqlite3_prepare_v2(m_pDB, "INSERT INTO fio_frame_segment (projectid, frameid, namesyid, segmenttype) VALUES(:projectid, :frameid, :namesyid, :segmenttype);", -1, &m_fio_frame_segment_stmt, NULL);
	if (res != SQLITE_OK)
	{
		m_fio_frame_segment_stmt = NULL;
	}

	res = sqlite3_prepare_v2(m_pDB, "INSERT INTO fio_frame_segment_property (projectid, segmentid, proname, protype, ivalue) VALUES(:projectid, :segmentid, :proname, :protype, :ivalue);", -1, &m_fio_frame_segment_property_stmt, NULL);
	if (res != SQLITE_OK)
	{
		m_fio_frame_segment_property_stmt = NULL;
	}

	res = sqlite3_prepare_v2(m_pDB, "INSERT INTO fio_frame_oneof (projectid, proertyid, itemsyid, framesyid) VALUES(:projectid, :proertyid, :itemsyid, :framesyid);", -1, &m_fio_frame_oneof_stmt, NULL);
	if (res != SQLITE_OK)
	{
		m_fio_frame_oneof_stmt = NULL;
	}

	res = sqlite3_prepare_v2(m_pDB, "INSERT INTO fio_frame_exp (projectid, propertyid, op, leftsyid, rightsyid, valuesyid) VALUES(:projectid, :propertyid, :op, :leftsyid, :rightsyid, :valuesyid);", -1, &m_fio_frame_exp_stmt, NULL);
	if (res != SQLITE_OK)
	{
		m_fio_frame_exp_stmt = NULL;
	}

	res = sqlite3_prepare_v2(m_pDB, "INSERT INTO fio_sys (projectid, namesyid) VALUES(:projectid, :namesyid);", -1, &m_fio_sys_stmt, NULL);
	if (res != SQLITE_OK)
	{
		m_fio_sys_stmt = NULL;
	}

	res = sqlite3_prepare_v2(m_pDB, "INSERT INTO fio_sys_action (projectid, sysid, namesyid, actiontype, frameid, channelid) VALUES(:projectid, :sysid, :namesyid, :actiontype, :frameid, :channelid);", -1, &m_fio_sys_action_stmt, NULL);
	if (res != SQLITE_OK)
	{
		m_fio_sys_action_stmt = NULL;
	}

	res = sqlite3_prepare_v2(m_pDB, "INSERT INTO fio_sys_action_map (projectid, actionid, segsyid, sysprosyid) VALUES(:projectid, :actionid, :segsyid, :sysprosyid);", -1, &m_fio_sys_action_map_stmt, NULL);
	if (res != SQLITE_OK)
	{
		m_fio_sys_action_map_stmt = NULL;
	}

	res = sqlite3_prepare_v2(m_pDB, "INSERT INTO fio_sys_channel (projectid, sysid, namesyid, channeltype) VALUES(:projectid, :sysid, :namesyid, :channeltype);", -1, &m_fio_sys_channel_stmt, NULL);
	if (res != SQLITE_OK)
	{
		m_fio_sys_channel_stmt = NULL;
	}

	res = sqlite3_prepare_v2(m_pDB, "INSERT INTO fio_sys_channel_option (projectid, channelid, nameid, valuesyid) VALUES(:projectid, :channelid, :nameid, :valuesyid);", -1, &m_fio_sys_channel_option_stmt, NULL);
	if (res != SQLITE_OK)
	{
		m_fio_sys_channel_option_stmt = NULL;
	}

	res = sqlite3_prepare_v2(m_pDB, "INSERT INTO fio_sys_property (projectid, sysid, namesyid, propertytype, isarray) VALUES(:projectid, :sysid, :namesyid, :propertytype, :isarray);", -1, &m_fio_sys_property_stmt, NULL);
	if (res != SQLITE_OK)
	{
		m_fio_sys_property_stmt = NULL;
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

	if (m_fio_project_openstmt)
	{
		sqlite3_finalize(m_fio_project_openstmt);
		m_fio_project_openstmt = NULL;
	}

	if (m_fio_project_updatestmt)
	{
		sqlite3_finalize(m_fio_project_updatestmt);
		m_fio_project_updatestmt = NULL;
	}

	if (m_fio_error_stmt)
	{
		sqlite3_finalize(m_fio_error_stmt);
		m_fio_error_stmt = NULL;
	}

	if (m_fio_notes_stmt)
	{
		sqlite3_finalize(m_fio_notes_stmt);
		m_fio_notes_stmt = NULL;
	}

	if (m_fio_enum_stmt)
	{
		sqlite3_finalize(m_fio_enum_stmt);
		m_fio_enum_stmt = NULL;
	}

	if (m_fio_enum_item_stmt)
	{
		sqlite3_finalize(m_fio_enum_item_stmt);
		m_fio_enum_item_stmt = NULL;
	}

	if (m_fio_frame_stmt)
	{
		sqlite3_finalize(m_fio_frame_stmt);
		m_fio_frame_stmt = NULL;
	}

	if (m_fio_frame_segment_stmt)
	{
		sqlite3_finalize(m_fio_frame_segment_stmt);
		m_fio_frame_segment_stmt = NULL;
	}

	if (m_fio_frame_segment_property_stmt)
	{
		sqlite3_finalize(m_fio_frame_segment_property_stmt);
		m_fio_frame_segment_property_stmt = NULL;
	}

	if (m_fio_frame_oneof_stmt)
	{
		sqlite3_finalize(m_fio_frame_oneof_stmt);
		m_fio_frame_oneof_stmt = NULL;
	}

	if (m_fio_frame_exp_stmt)
	{
		sqlite3_finalize(m_fio_frame_exp_stmt);
		m_fio_frame_exp_stmt = NULL;
	}

	if (m_fio_sys_stmt)
	{
		sqlite3_finalize(m_fio_sys_stmt);
		m_fio_sys_stmt = NULL;
	}

	if (m_fio_sys_action_stmt)
	{
		sqlite3_finalize(m_fio_sys_action_stmt);
		m_fio_sys_action_stmt = NULL;
	}

	if (m_fio_sys_action_map_stmt)
	{
		sqlite3_finalize(m_fio_sys_action_map_stmt);
		m_fio_sys_action_map_stmt = NULL;
	}

	if (m_fio_sys_channel_stmt)
	{
		sqlite3_finalize(m_fio_sys_channel_stmt);
		m_fio_sys_channel_stmt = NULL;
	}

	if (m_fio_sys_channel_option_stmt)
	{
		sqlite3_finalize(m_fio_sys_channel_option_stmt);
		m_fio_sys_channel_option_stmt = NULL;
	}

	if (m_fio_sys_property_stmt)
	{
		sqlite3_finalize(m_fio_sys_property_stmt);
		m_fio_sys_property_stmt = NULL;
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

	sqlite3_bind_int(m_fio_project_openstmt, 1, m_projectid);
	int res = sqlite3_step(m_fio_project_openstmt);
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

	const char* code = (const char*)sqlite3_column_text(m_fio_project_openstmt, 0);
	if (!code) return false;
	int len = (int)strlen(code);
	m_codesize = len;
	if (m_projectcode) delete m_projectcode;
	m_projectcode = new char[len+1];
	strcpy_s(m_projectcode, len+1, code);
	
	sqlite3_reset(m_fio_project_openstmt);
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

////分析语义错误
//int FrameIOParserDb::Semantics()
//{
//	//字段属性重复设置
//	const char* sql = "INSERT INTO fio_error (projectid, errorcode, firstsyid, lastsyid) SELECT projectid, 1, segnamesyid, segnamesyid \
//		FROM(SELECT pt.projectid, count(*) repet, seg.namesyid segnamesyid FROM fio_frame_segment_property pt LEFT JOIN fio_frame_segment seg ON pt.segmentid = seg.rowid \
//			WHERE pt.projectid = ?1 GROUP BY pt.segmentid, pt.proname) et WHERE et.repet > 1";
//	if (RunSqlWithProjectId(sql) != 0) return -1;
//
//}

//int  FrameIOParserDb::RunSqlWithProjectId(const char* sql)
//{
//	int res = sqlite3_prepare_v2(m_pDB, sql, -1, &m_semantics_stmt, NULL);
//	if (res != SQLITE_OK)
//	{
//		m_semantics_stmt = NULL;
//	}
//	sqlite3_bind_int(m_semantics_stmt, 1, m_projectid);
//	int rc = sqlite3_step(m_semantics_stmt);
//	if ((rc != SQLITE_DONE) && (rc != SQLITE_ROW))
//	{
//		return -1;
//	}
//	if (m_semantics_stmt)
//	{
//		sqlite3_finalize(m_semantics_stmt);
//		m_semantics_stmt = NULL;
//	}
//	return 0;
//}


//保存项目ast
int FrameIOParserDb::SaveProject(PROJECT* pj)
{
	sqlite3_bind_int(m_fio_project_updatestmt, 1, pj->namesyid);
	sqlite3_bind_int(m_fio_project_updatestmt, 2, m_projectid);
	int res = sqlite3_step(m_fio_project_updatestmt);
	if ((res != SQLITE_DONE) && (res != SQLITE_ROW))
	{
		return -1;
	}
	sqlite3_reset(m_fio_project_updatestmt);

	if (SaveNotes(pj->notes, pj->namesyid)!=0) return -1;
	if (SaveEnumcfg(pj->enumcfglist) != 0) return -1;
	if (SaveFrame(pj->framelist) != 0) return -1;
	if (SaveSys(pj->syslist) != 0) return -1;
	//if (Semantics() != 0) return -1;
	return 0;
}


//保存分系统
int FrameIOParserDb::SaveSys(SYS* si, int* sysid)
{
	while (si)
	{
		sqlite3_bind_int(m_fio_sys_stmt, 1, m_projectid);
		sqlite3_bind_int(m_fio_sys_stmt, 2, si->namesyid);
		int rc = sqlite3_step(m_fio_sys_stmt);
		if ((rc != SQLITE_DONE) && (rc != SQLITE_ROW)) return -1;
		sqlite3_reset(m_fio_sys_stmt);

		auto sid = (int)sqlite3_last_insert_rowid(m_pDB);
		if (SaveSysAction(si->actionlist, sid) != 0) return -1;
		if (SaveSysChannel(si->channellist, sid) != 0) return -1;
		if (SaveSysProperty(si->propertylist, sid) != 0) return -1;
		if (SaveNotes(si->notes, si->namesyid) != 0) return -1;

		if (sysid != NULL) *sysid = sid;
		si = si->nextsys;
	}
	return 0;
}

//保存动作
int FrameIOParserDb::SaveSysAction(ACTION* ac, int sysid, int* acid)
{
	while (ac)
	{
		sqlite3_bind_int(m_fio_sys_action_stmt, 1, m_projectid);
		sqlite3_bind_int(m_fio_sys_action_stmt, 2, sysid);
		sqlite3_bind_int(m_fio_sys_action_stmt, 3, ac->namesyid);
		sqlite3_bind_int(m_fio_sys_action_stmt, 4, ac->iotype);
		sqlite3_bind_int(m_fio_sys_action_stmt, 5, ac->framesyid);
		sqlite3_bind_int(m_fio_sys_action_stmt, 6, ac->channelsyid);
		int rc = sqlite3_step(m_fio_sys_action_stmt);
		if ((rc != SQLITE_DONE) && (rc != SQLITE_ROW)) return -1;
		sqlite3_reset(m_fio_sys_action_stmt);

		auto aid = (int)sqlite3_last_insert_rowid(m_pDB);
		if (SaveSysActionMap(ac->maplist, aid) != NULL) return -1;
		if (SaveNotes(ac->notes, ac->namesyid) != 0) return -1;
		if (acid != NULL) *acid = aid;
		ac = ac->nextaction;
	}
	return 0;
}

//保存动作映射
int FrameIOParserDb::SaveSysActionMap(ACTIONMAP* mp, int acid)
{
	while (mp)
	{
		sqlite3_bind_int(m_fio_sys_action_map_stmt, 1, m_projectid);
		sqlite3_bind_int(m_fio_sys_action_map_stmt, 2, acid);
		sqlite3_bind_int(m_fio_sys_action_map_stmt, 3, mp->segsyid);
		sqlite3_bind_int(m_fio_sys_action_map_stmt, 4, mp->prosyid);
		int rc = sqlite3_step(m_fio_sys_action_map_stmt);
		if ((rc != SQLITE_DONE) && (rc != SQLITE_ROW)) return -1;
		sqlite3_reset(m_fio_sys_action_map_stmt);

		if (SaveNotes(mp->notes, mp->segsyid) != 0) return -1;

		mp = mp->nextmap;
	}
	return 0;
}

//保存通道
int FrameIOParserDb::SaveSysChannel(CHANNEL* ch, int sysid, int* chid)
{
	while (ch)
	{
		sqlite3_bind_int(m_fio_sys_channel_stmt, 1, m_projectid);
		sqlite3_bind_int(m_fio_sys_channel_stmt, 2, sysid);
		sqlite3_bind_int(m_fio_sys_channel_stmt, 3, ch->namesyid);
		sqlite3_bind_int(m_fio_sys_channel_stmt, 4, ch->channeltype);
		int rc = sqlite3_step(m_fio_sys_channel_stmt);
		if ((rc != SQLITE_DONE) && (rc != SQLITE_ROW)) return -1;
		sqlite3_reset(m_fio_sys_channel_stmt);

		auto cid = (int)sqlite3_last_insert_rowid(m_pDB);
		if (SaveSysChannelOption(ch->channeloption, cid) != 0) return -1;
		if (SaveNotes(ch->notes,ch->namesyid) != 0) return -1;
		if (chid != NULL) *chid = cid;
		ch = ch->nextchannel;
	}
	return 0;
}

//保存通道选项
int FrameIOParserDb::SaveSysChannelOption(CHANNELOPTION* op, int chid)
{
	while (op)
	{
		sqlite3_bind_int(m_fio_sys_channel_option_stmt, 1, m_projectid);
		sqlite3_bind_int(m_fio_sys_channel_option_stmt, 2, chid);
		sqlite3_bind_int(m_fio_sys_channel_option_stmt, 3, op->optiontype);
		sqlite3_bind_int(m_fio_sys_channel_option_stmt, 4, op->valuesyid);
		int rc = sqlite3_step(m_fio_sys_channel_option_stmt);
		if ((rc != SQLITE_DONE) && (rc != SQLITE_ROW)) return -1;
		sqlite3_reset(m_fio_sys_channel_option_stmt);

		if (SaveNotes(op->notes, op->valuesyid) != 0) return -1;
		op = op->nextoption;
	}
	return 0;
}

//保存系统属性
int FrameIOParserDb::SaveSysProperty(SYSPROPERTY* pt, int sysid)
{
	while (pt)
	{
		sqlite3_bind_int(m_fio_sys_property_stmt, 1, m_projectid);
		sqlite3_bind_int(m_fio_sys_property_stmt, 2, sysid);
		sqlite3_bind_int(m_fio_sys_property_stmt, 3, pt->namesyid);
		sqlite3_bind_int(m_fio_sys_property_stmt, 4, pt->protype);
		sqlite3_bind_int(m_fio_sys_property_stmt, 5, pt->isarray? 1:0);
		int rc = sqlite3_step(m_fio_sys_property_stmt);
		if ((rc != SQLITE_DONE) && (rc != SQLITE_ROW)) return -1;
		sqlite3_reset(m_fio_sys_property_stmt);

		if (SaveNotes(pt->notes, pt->namesyid) != 0) return -1;
		pt = pt->nextsysproperty;
	}
	return 0;
}

//保存数据帧
int FrameIOParserDb::SaveFrame(FRAME* frm, int* frmid)
{
	while (frm)
	{
		sqlite3_bind_int(m_fio_frame_stmt, 1, m_projectid);
		sqlite3_bind_int(m_fio_frame_stmt, 2, frm->namesyid);
		int rc = sqlite3_step(m_fio_frame_stmt);
		if ((rc != SQLITE_DONE) && (rc != SQLITE_ROW))
		{
			return -1;
		}
		sqlite3_reset(m_fio_frame_stmt);

		auto _frmid = (int)sqlite3_last_insert_rowid(m_pDB);
		if (frmid != NULL) *frmid = _frmid;
		if (SaveNotes(frm->notes, frm->namesyid) != 0) return -1;
		if (SaveFrameSegment(frm->seglist, _frmid) != 0) return -1;

		frm = frm->nextframe;
	}

	return 0;
}

//保存数据帧字段
int FrameIOParserDb::SaveFrameSegment(SEGMENT* seglist, int frmid)
{
	while (seglist)
	{
		sqlite3_bind_int(m_fio_frame_segment_stmt, 1, m_projectid);
		sqlite3_bind_int(m_fio_frame_segment_stmt, 2, frmid);
		sqlite3_bind_int(m_fio_frame_segment_stmt, 3, seglist->namesyid);
		sqlite3_bind_int(m_fio_frame_segment_stmt, 4, seglist->segtype);
		int rc = sqlite3_step(m_fio_frame_segment_stmt);
		if ((rc != SQLITE_DONE) && (rc != SQLITE_ROW))
		{
			return -1;
		}
		sqlite3_reset(m_fio_frame_segment_stmt);

		auto segid = (int)sqlite3_last_insert_rowid(m_pDB);
		if (SaveNotes(seglist->notes, seglist->namesyid) != 0) return -1;
		if (SaveFrameSegmentProperty(seglist->segpropertylist, segid) != 0) return -1;

		seglist = seglist->nextsegment;
	}

	return 0;
}

//保存数据帧字段的属性
int FrameIOParserDb::SaveFrameSegmentProperty(SEGPROPERTY* pro, int segid)
{
	while (pro)
	{
		sqlite3_bind_int(m_fio_frame_segment_property_stmt, 1, m_projectid);
		sqlite3_bind_int(m_fio_frame_segment_property_stmt, 2, segid);
		sqlite3_bind_int(m_fio_frame_segment_property_stmt, 3, pro->pro);
		sqlite3_bind_int(m_fio_frame_segment_property_stmt, 4, pro->vtype);
		if (pro->vtype == SEGPV_NONAMEFRAME)
		{
			int id = -1;
			if (SaveFrame((FRAME*)pro->pv, &id) != 0) return -1;
			sqlite3_bind_int(m_fio_frame_segment_property_stmt, 5, id);
		}
		else
			sqlite3_bind_int(m_fio_frame_segment_property_stmt, 5, pro->iv);
			
		int rc = sqlite3_step(m_fio_frame_segment_property_stmt);
		if ((rc != SQLITE_DONE) && (rc != SQLITE_ROW)) return -1;
		sqlite3_reset(m_fio_frame_segment_property_stmt);

		auto proid = (int)sqlite3_last_insert_rowid(m_pDB);
		if (pro->vtype == SEGPV_ONEOF)
		{
			if (SaveFrameOneOf((ONEOFITEM*)pro->pv, proid) != 0) return -1;
		}
		else if (pro->vtype == SEGPV_EXP)
		{
			if (SaveFrameExp((EXPVALUE*)pro->pv, proid, NULL) != 0) return -1;
		}

		pro = pro->nextsegpro;
	}
	return 0;
}

//保存oneof
int FrameIOParserDb::SaveFrameOneOf(ONEOFITEM* oit, int proid)
{
	while (oit)
	{
		sqlite3_bind_int(m_fio_frame_oneof_stmt, 1, m_projectid);
		sqlite3_bind_int(m_fio_frame_oneof_stmt, 2, proid);
		sqlite3_bind_int(m_fio_frame_oneof_stmt, 3, oit->enumitemsyid);
		sqlite3_bind_int(m_fio_frame_oneof_stmt, 4, oit->framesyid);
		int rc = sqlite3_step(m_fio_frame_oneof_stmt);
		if ((rc != SQLITE_DONE) && (rc != SQLITE_ROW)) return -1;

		sqlite3_reset(m_fio_frame_oneof_stmt);

		oit = oit->nextitem;
	}
	return 0;
}

//保存表达式
int FrameIOParserDb::SaveFrameExp(EXPVALUE* exp, int proid, int* expid)
{
	if (exp == NULL) return 0;

	int li = -1, ri = -1;
	if (SaveFrameExp(exp->lexp, proid, &li) != 0) return -1;
	if (SaveFrameExp(exp->rexp, proid, &ri) != 0) return -1;

	sqlite3_bind_int(m_fio_frame_exp_stmt, 1, m_projectid);
	sqlite3_bind_int(m_fio_frame_exp_stmt, 2, proid);
	sqlite3_bind_int(m_fio_frame_exp_stmt, 3, exp->valuetype);
	sqlite3_bind_int(m_fio_frame_exp_stmt, 4, li);
	sqlite3_bind_int(m_fio_frame_exp_stmt, 5, ri);
	sqlite3_bind_int(m_fio_frame_exp_stmt, 6, exp->valuesyid);
	int rc = sqlite3_step(m_fio_frame_exp_stmt);
	if ((rc != SQLITE_DONE) && (rc != SQLITE_ROW)) return -1;
	sqlite3_reset(m_fio_frame_exp_stmt);

	if(expid !=NULL) *expid = (int)sqlite3_last_insert_rowid(m_pDB);

	return 0;
}




//保存注释信息
int FrameIOParserDb::SaveNotes(NOTE* notes, int forid)
{
	while (notes)
	{
		sqlite3_bind_int(m_fio_notes_stmt, 1, m_projectid);
		sqlite3_bind_int(m_fio_notes_stmt, 2, notes->notesyid);
		sqlite3_bind_int(m_fio_notes_stmt, 3, forid);
		int rc = sqlite3_step(m_fio_notes_stmt);
		if ((rc != SQLITE_DONE) && (rc != SQLITE_ROW))
		{
			return -1;
		}
		sqlite3_reset(m_fio_notes_stmt);
		notes = notes->nextnote;
	}
	return 0;
}

//保存枚举
int FrameIOParserDb::SaveEnumcfg(ENUMCFG* ecfg)
{
	while (ecfg)
	{
		sqlite3_bind_int(m_fio_enum_stmt, 1, m_projectid);
		sqlite3_bind_int(m_fio_enum_stmt, 2, ecfg->namesyid);
		int rc = sqlite3_step(m_fio_enum_stmt);
		if ((rc != SQLITE_DONE) && (rc != SQLITE_ROW))
		{
			return -1;
		}
		sqlite3_reset(m_fio_enum_stmt);
		auto ecfgid = (int)sqlite3_last_insert_rowid(m_pDB);

		if (SaveNotes(ecfg->notes, ecfg->namesyid) != 0) return -1;
		if (SaveEnumcfgItems(ecfg->enumitemlist, ecfgid) != 0) return -1;
		ecfg = ecfg->nextenumcfg;
	}
	return 0;
}

//保存枚举列表
int FrameIOParserDb::SaveEnumcfgItems(ENUMITEM* ecfgitem, int ecfgid)
{
	while (ecfgitem)
	{
		sqlite3_bind_int(m_fio_enum_item_stmt, 1, m_projectid);
		sqlite3_bind_int(m_fio_enum_item_stmt, 2, ecfgid);
		sqlite3_bind_int(m_fio_enum_item_stmt, 3, ecfgitem->itemsyid);
		sqlite3_bind_int(m_fio_enum_item_stmt, 4, ecfgitem->valuesyid);
		int rc = sqlite3_step(m_fio_enum_item_stmt);
		if ((rc != SQLITE_DONE) && (rc != SQLITE_ROW))
		{
			return -1;
		}
		sqlite3_reset(m_fio_enum_item_stmt);

		if (SaveNotes(ecfgitem->notes, ecfgitem->itemsyid) != 0) return -1;
		ecfgitem = ecfgitem->nextitem;
	}

	return 0;
}


