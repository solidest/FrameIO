#include "stdafx.h"
#include "FrameIORunX.h"
#include <iostream>

using namespace System;

int main(array<System::String ^> ^args)
{
	auto ch = CreateFioChannel(FIOCHANNEL_TCPSERVER);
	SetChannelOptionS(ch, "serverip", "127.0.0.1");
	SetChannelOptionS(ch, "clientip", "127.0.0.1");
	SetChannelOptionN(ch, "port", 8007);
	InitialChannel(ch);
	bool res = OpenChannel(ch);
	std::cout << res;
    return 0;
}
