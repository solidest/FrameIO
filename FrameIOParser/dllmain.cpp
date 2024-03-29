// dllmain.cpp : 定义 DLL 应用程序的入口点。
#include "stdafx.h"
#include "FrameIOParserDb.h"
#include "fparser.tab.h"
#include <FlexLexer.h>
#include <istream>
#include <streambuf>


struct membuf : std::streambuf
{
	membuf(char* begin, char* end) {
		this->setg(begin, begin, end);
	}
};


//启动projectid指定的项目解析任务，解析完成返回0，启动失败返回正数 错误编号
extern "C" int __declspec(dllexport) parse(int projectid)
{
	int ret = 0;
	FrameIOParserDb * db = new FrameIOParserDb();
	if (!db->LoadProject(projectid))
		ret = 2;
	else
	{
		membuf sbuf(db->getProjectCode(), (char*)(db->getProjectCode() + db->getCodeSize()));
		std::istream in(&sbuf);
		yyFlexLexer * pLex= new yyFlexLexer(&in);
		ret = yyparse(pLex, db);
		delete pLex;
	}
	delete db;
	return ret;
}

