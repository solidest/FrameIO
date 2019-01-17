#pragma once

#include "FioChannel.h"
#include "FioObjextX.h"
#include "FrameIORunX.h"

void SendFrame(FioChannelX& ch, FioObjextX& frame)
{
	FioSendFrame(ch.GetHandle(), frame.GetHandle());
}

void RecvFrame(const char * frameName, FioChannelX& ch, FioObjextX* frame)
{
	frame->LoadFioObjextX(ch.GetHandle(), frameName);
}
