

#include "stdafx.h"
#include "zlog.h"


extern "C" __declspec(dllexport) int add(int a, int b) {
	int rc;
	zlog_category_t *c;

	rc = zlog_init("/etc/zlog.conf");
	if (rc) {
		printf("init failed\n");
		return -1;
	}

	c = zlog_get_category("my_cat");
	if (!c) {
		printf("get cat fail\n");
		zlog_fini();
		return -2;
	}

	zlog_info(c, "hello, zlog");

	zlog_fini();

	return 0;
	return a + b;

}