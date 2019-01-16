#include "stdafx.h"

#include "FrameIORunX.h"
#include <string>
#include <iostream>
#include <gcroot.h>


using namespace System::Runtime::InteropServices;
using namespace System;
using namespace FrameIO::Run;
using namespace FrameIO::Interface;

namespace FrameIORunX
{

	String^ ConvertToString(const char * str)
	{
		char * res = new char[100];
		strcpy(res, str);
		System::String^ ss = Marshal::PtrToStringAnsi(static_cast<System::IntPtr>(res));
		delete[] res;
		return ss;
	}

	public class FioChannelX
	{
	private:
		gcroot<ChannelOption^> _refOps;
		gcroot<FioChannel^> _refCh;
		int _chType;

	public:

		FioChannelX(int channelType)
		{
			_refOps = gcnew ChannelOption;
			_chType = channelType;
		}

		~FioChannelX()
		{
			delete _refOps;
			delete _refCh;
		}

		//设置参数
		void SetOption(const char* optionName, int optionValue)
		{
			_refOps->SetOption(ConvertToString(optionName), optionValue);

		}

		void SetOption(const char* optionName, const char* optionValue)
		{
		
		 
			_refOps->SetOption(ConvertToString(optionName), ConvertToString(optionValue));
		}

		//初始化通道
		void InitialChannel()
		{
			_refCh = IORunner::GetChannel((ChannelTypeEnum)_chType, _refOps);
		}

		//打开通道
		bool OpenChannel()
		{
			return _refCh->Open();
		}

		//通道是否打开
		bool IsOpen()
		{
			return _refCh->IsOpen;
		}

		//关闭通道
		void Close()
		{
			_refCh->Clear();
		}

	};


	#pragma region --Channel--

	extern "C" __declspec(dllexport) void* FioChannelCreate(int channelType)
	{
		return new FioChannelX(channelType);
	}

	extern "C" __declspec(dllexport) void FioChannelRelease(void* channelHandle)
	{
		delete (FioChannelX*)channelHandle;
	}

	extern "C" __declspec(dllexport) void FioChannelInitial(void* channelHandle)
	{
		try
		{
			((FioChannelX*)channelHandle)->InitialChannel();

		}
		catch (const std::exception& ex)
		{
			std::cout << ex.what();
		}

	}

	extern "C" __declspec(dllexport) bool FioChannelOpen(void* channelHandle)
	{
		try
		{
			return ((FioChannelX*)channelHandle)->OpenChannel();

		}
		catch (const std::exception& ex)
		{
			std::cout << ex.what();
		}
	}

	extern "C" __declspec(dllexport) void FioChannelClose(void* channelHandle)
	{
		((FioChannelX*)channelHandle)->Close();
	}

	extern "C" __declspec(dllexport) bool FioChannelIsOpen(void* channelHandle)
	{
		return ((FioChannelX*)channelHandle)->IsOpen();
	}

	extern "C" __declspec(dllexport) void FioChannelSetOptionN(void* channelHandle, const char* optionname, int optionvalue)
	{
		return ((FioChannelX*)channelHandle)->SetOption(optionname, optionvalue);
	}

	extern "C" __declspec(dllexport) void FioChannelSetOptionS(void* channelHandle, const char* optionname, const char* optionvalue)
	{
		return ((FioChannelX*)channelHandle)->SetOption(optionname, optionvalue);
	}


	#pragma endregion

	#pragma region --Object--

	extern "C" __declspec(dllexport) void* FioCreateObject(const char * frameName)
	{
		auto o = new gcroot<FrameObject^>();
		*o = IORunner::NewFrameObject(ConvertToString(frameName));
		return o;
	}


	extern "C" __declspec(dllexport) void* FioGetObjectValue(void * ovHandle, const char* segName)
	{
		auto o = new gcroot<FrameObject^>();
		*o = (*(gcroot<FrameObject^> *)ovHandle)->GetObject(ConvertToString(segName));
		return o;
	}

	extern "C" __declspec(dllexport) void FioSetObjectValue(void * ovHandle, const char* segName, void* oValueHandle)
	{
		(*(gcroot<FrameObject^> *)ovHandle)->SetObject(ConvertToString(segName), *(gcroot<FrameObject^> *)oValueHandle);
	}

	extern "C" __declspec(dllexport) void FioReleaseObject(void * ovHandle)
	{
		delete (gcroot<FrameObject^> *)ovHandle;
	}



	#pragma endregion

	#pragma region --SetValue--

	extern "C" __declspec(dllexport) void FioSetBoolValue(void * ovHandle, const char* segName, bool value)
	{
		(*((gcroot<FrameObject^> *)ovHandle))->SetValue(ConvertToString(segName), value);
	}

	extern "C" __declspec(dllexport) void FioSetByteValue(void * ovHandle, const char* segName, unsigned char value)
	{
		(*((gcroot<FrameObject^> *)ovHandle))->SetValue(ConvertToString(segName), value);
	}

	extern "C" __declspec(dllexport) void FioSetSByteValue(void * ovHandle, const char* segName, signed char value)
	{
		(*((gcroot<FrameObject^> *)ovHandle))->SetValue(ConvertToString(segName), value);
	}

	extern "C" __declspec(dllexport) void FioSetShortValue(void * ovHandle, const char* segName, signed short value)
	{
		(*((gcroot<FrameObject^> *)ovHandle))->SetValue(ConvertToString(segName), value);
	}

	extern "C" __declspec(dllexport) void FioSetUShortValue(void * ovHandle, const char* segName, unsigned short value)
	{
		(*((gcroot<FrameObject^> *)ovHandle))->SetValue(ConvertToString(segName), value);
	}

	extern "C" __declspec(dllexport) void FioSetIntValue(void * ovHandle, const char* segName, signed int value)
	{
		(*((gcroot<FrameObject^> *)ovHandle))->SetValue(ConvertToString(segName), value);
	}

	extern "C" __declspec(dllexport) void FioSetUIntValue(void * ovHandle, const char* segName, unsigned int value)
	{
		(*((gcroot<FrameObject^> *)ovHandle))->SetValue(ConvertToString(segName), value);
	}

	extern "C" __declspec(dllexport) void FioSetLongValue(void * ovHandle, const char* segName, long long value)
	{
		(*((gcroot<FrameObject^> *)ovHandle))->SetValue(ConvertToString(segName), value);
	}

	extern "C" __declspec(dllexport) void FioSetULongValue(void * ovHandle, const char* segName, unsigned long long value)
	{
		(*((gcroot<FrameObject^> *)ovHandle))->SetValue(ConvertToString(segName), value);
	}

	extern "C" __declspec(dllexport) void FioSetFloatValue(void * ovHandle, const char* segName, float value)
	{
		(*((gcroot<FrameObject^> *)ovHandle))->SetValue(ConvertToString(segName), value);
	}

	extern "C" __declspec(dllexport) void FioSetDoubleValue(void * ovHandle, const char* segName, double value)
	{
		(*((gcroot<FrameObject^> *)ovHandle))->SetValue(ConvertToString(segName), value);
	}


#pragma endregion

	#pragma region --GetValue--

	extern "C" __declspec(dllexport) double FioGetDoubleValue(void * ovHandle, const char* segName)
	{
		return (*((gcroot<FrameObject^> *)ovHandle))->GetDouble(ConvertToString(segName));
	}

	extern "C" __declspec(dllexport) float FioGetFloatValue(void * ovHandle, const char* segName)
	{
		return (*((gcroot<FrameObject^> *)ovHandle))->GetFloat(ConvertToString(segName));
	}

	extern "C" __declspec(dllexport) long long FioGetLongValue(void * ovHandle, const char* segName)
	{
		return (*((gcroot<FrameObject^> *)ovHandle))->GetLong(ConvertToString(segName));
	}

	extern "C" __declspec(dllexport) unsigned long long FioGetULongValue(void * ovHandle, const char* segName)
	{
		return (*((gcroot<FrameObject^> *)ovHandle))->GetULong(ConvertToString(segName));
	}

	extern "C" __declspec(dllexport) int FioGetIntValue(void * ovHandle, const char* segName)
	{
		return (*((gcroot<FrameObject^> *)ovHandle))->GetInt(ConvertToString(segName));
	}

	extern "C" __declspec(dllexport) unsigned int FioGetUIntValue(void * ovHandle, const char* segName)
	{
		return (*((gcroot<FrameObject^> *)ovHandle))->GetUInt(ConvertToString(segName));
	}

	extern "C" __declspec(dllexport) short FioGetShortValue(void * ovHandle, const char* segName)
	{
		return (*((gcroot<FrameObject^> *)ovHandle))->GetShort(ConvertToString(segName));
	}

	extern "C" __declspec(dllexport) unsigned short FioGetUShortValue(void * ovHandle, const char* segName)
	{
		return (*((gcroot<FrameObject^> *)ovHandle))->GetUShort(ConvertToString(segName));
	}

	extern "C" __declspec(dllexport) unsigned char FioGetByteValue(void * ovHandle, const char* segName)
	{
		return (*((gcroot<FrameObject^> *)ovHandle))->GetByte(ConvertToString(segName));
	}

	extern "C" __declspec(dllexport) signed char FioGetSByteValue(void * ovHandle, const char* segName)
	{
		return (*((gcroot<FrameObject^> *)ovHandle))->GetSByte(ConvertToString(segName));
	}
#pragma endregion

#pragma region --Send && Recv--
	extern "C" __declspec(dllexport) double SendFrame(void * chHandle, void * FrameHandle)
	{

	}

#pragma endregion


}
