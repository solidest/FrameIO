#pragma once

#include "FrameIORunX.h"

void FioInitial()
{
	const char* config =
		<%framesconfig%>;

	FioInitial(config);
}
