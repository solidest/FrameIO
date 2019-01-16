#include "stdafx.h"
#include "FrameIORunX.h"
#include <iostream>

using namespace System;

int main(array<System::String ^> ^args)
{
	auto ch = FioChannelCreate(FIOCHANNEL_TCPSERVER);
	FioChannelSetOptionS(ch, "serverip", "127.0.0.1");
	FioChannelSetOptionS(ch, "clientip", "127.0.0.1");
	FioChannelSetOptionN(ch, "port", 8007);
	FioChannelInitial(ch);
	bool res = FioChannelOpen(ch);
	std::cout << res;
    return 0;
}
