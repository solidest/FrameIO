

#include "stdafx.h"
#include "zlog.h"


extern "C" __declspec(dllexport) int add(int a, int b) {
	int rc;
	rc = dzlog_init("zlog.conf", "pharser");

	if (rc) {

		printf("init failed\n");
		return -1;

	}

	dzlog_info("hello, zlog");
	dzlog_fatal("hello, zlog");
	dzlog_error("hello, zlog");
	dzlog_warn("hello, zlog");
	dzlog_notice("hello, zlog");
	dzlog_debug("hello, zlog");

	zlog_fini();

	return a+b;

}