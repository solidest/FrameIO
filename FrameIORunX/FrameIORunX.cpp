#include "stdafx.h"

#include "FrameIORunX.h"


using namespace System::Runtime::InteropServices;
using namespace System;




extern "C" __declspec(dllexport) void* CreateFioChannel(int channelType)
{

}


extern "C" __declspec(dllexport) void ReleaseFioChannel(void* channelHandle)
{

}