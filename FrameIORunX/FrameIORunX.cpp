#include "stdafx.h"

#include "FrameIORunX.h"

using namespace System::Runtime::InteropServices;
using namespace System;

extern "C" __declspec(dllexport) int NewFrameObject(char* frameName)
{
	String^ fraeName = Marshal::PtrToStringAnsi(static_cast<System::IntPtr>(frameName));

	String^ A_path = "CSharp ascall string";
	char* Ipath = (char*)(void*)Marshal::StringToHGlobalAnsi(A_path);
	return Ipath;
}

extern "C" __declspec(dllexport) void release(void* b)
{
	Marshal::FreeHGlobal(static_cast<System::IntPtr>(b));
}