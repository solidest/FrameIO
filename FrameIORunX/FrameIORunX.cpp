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

	struct  ValueArray
	{
		int len;
		void* arr;
	};

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

	extern "C" __declspec(dllexport) void FioInitial(const char* config)
	{
		char * res = new char[1024*256];
		strcpy(res, res);
		System::String^ ss = Marshal::PtrToStringAnsi(static_cast<System::IntPtr>(res));

		delete[] res;
	}

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
		return ((FioChannelX*)channelHandle)->OpenChannel();
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

	extern "C" __declspec(dllexport) void* FioObjectCreate(const char * frameName)
	{
		auto o = new gcroot<FrameObject^>();
		*o = IORunner::NewFrameObject(ConvertToString(frameName));
		return o;
	}


	extern "C" __declspec(dllexport) void* FioObjectGetValue(void * ovHandle, const char* segName)
	{
		auto o = new gcroot<FrameObject^>();
		*o = (*(gcroot<FrameObject^> *)ovHandle)->GetObject(ConvertToString(segName));
		return o;
	}

	extern "C" __declspec(dllexport) void FioObjectSetValue(void * ovHandle, const char* segName, void* oValueHandle)
	{
		(*(gcroot<FrameObject^> *)ovHandle)->SetObject(ConvertToString(segName), *(gcroot<FrameObject^> *)oValueHandle);
	}

	extern "C" __declspec(dllexport) void FioObjectSetArray(void * ovHandle, const char* segName, void** osValueHandle, int len)
	{
		auto arr = gcnew array<FrameObject^>(len);

		for (int i = 0; i < len; i++)
		{
			arr[i] = *(((gcroot<FrameObject^>**)(osValueHandle))[i]);
		}
		(*(gcroot<FrameObject^> *)ovHandle)->SetObjectArray(ConvertToString(segName), arr);
	}

	extern "C" __declspec(dllexport) void FioObjectRelease(void * ovHandle)
	{
		delete (gcroot<FrameObject^> *)ovHandle;
	}


	extern "C" __declspec(dllexport) void* FioObjectGetArray(void * ovHandle, const char* segName)
	{
		auto oo = (*(gcroot<FrameObject^> *)ovHandle)->GetObjectArray(ConvertToString(segName));
		auto ret = new ValueArray;
		ret->len = 0;
		for each (FrameObject^ o in oo) ret->len++;
		if (ret->len == 0) return NULL;
		ret->arr = new void * [ret->len];

		for (int i = 0; i < ret->len; i++)
			((gcroot<FrameObject^>**)(ret->arr))[i] = new gcroot<FrameObject^>();
		return ret;
	}

	extern "C" __declspec(dllexport) void FioObjectReleaseArray(void * ovsHandle)
	{
		for (int i = 0; i < ((ValueArray*)ovsHandle)->len; i++)
			delete (((gcroot<FrameObject^>**)(((ValueArray*)ovsHandle)->arr))[i]);
		delete[]((gcroot<FrameObject^>**)(((ValueArray*)ovsHandle)->arr));
		delete (ValueArray*)ovsHandle;
	}


	#pragma endregion

	#pragma region --SetArray--


	extern "C" __declspec(dllexport) void FioSetBoolValues(void * ovHandle, const char* segName, bool * values, int len)
	{
		auto arr = gcnew array<bool>(len);
		for (int i = 0; i < len; i++)
			arr[i] = values[i];
		(*((gcroot<FrameObject^> *)ovHandle))->SetValueArray(ConvertToString(segName), arr);
	}


	extern "C" __declspec(dllexport) void FioSetByteValues(void * ovHandle, const char* segName, unsigned char * values, int len)
	{
		auto arr = gcnew array<unsigned char>(len);
		for (int i = 0; i < len; i++)
			arr[i] = values[i];
		(*((gcroot<FrameObject^> *)ovHandle))->SetValueArray(ConvertToString(segName), arr);
	}

	extern "C" __declspec(dllexport) void FioSetSByteValues(void * ovHandle, const char* segName, signed char * values, int len)
	{
		auto arr = gcnew array<signed char>(len);
		for (int i = 0; i < len; i++)
			arr[i] = values[i];
		(*((gcroot<FrameObject^> *)ovHandle))->SetValueArray(ConvertToString(segName), arr);
	}


	extern "C" __declspec(dllexport) void FioSetShortValues(void * ovHandle, const char* segName, short * values, int len)
	{
		auto arr = gcnew array<short>(len);
		for (int i = 0; i < len; i++)
			arr[i] = values[i];
		(*((gcroot<FrameObject^> *)ovHandle))->SetValueArray(ConvertToString(segName), arr);
	}

	extern "C" __declspec(dllexport) void FioSetUShortValues(void * ovHandle, const char* segName, unsigned short * values, int len)
	{
		auto arr = gcnew array<unsigned short>(len);
		for (int i = 0; i < len; i++)
			arr[i] = values[i];
		(*((gcroot<FrameObject^> *)ovHandle))->SetValueArray(ConvertToString(segName), arr);
	}

	extern "C" __declspec(dllexport) void FioSetIntValues(void * ovHandle, const char* segName, int * values, int len)
	{
		auto arr = gcnew array<int>(len);
		for (int i = 0; i < len; i++)
			arr[i] = values[i];
		(*((gcroot<FrameObject^> *)ovHandle))->SetValueArray(ConvertToString(segName), arr);
	}

	extern "C" __declspec(dllexport) void FioSetUIntValues(void * ovHandle, const char* segName, unsigned int * values, int len)
	{
		auto arr = gcnew array<unsigned int>(len);
		for (int i = 0; i < len; i++)
			arr[i] = values[i];
		(*((gcroot<FrameObject^> *)ovHandle))->SetValueArray(ConvertToString(segName), arr);
	}

	extern "C" __declspec(dllexport) void FioSetLongValues(void * ovHandle, const char* segName, long long * values, int len)
	{
		auto arr = gcnew array<long long>(len);
		for (int i = 0; i < len; i++)
			arr[i] = values[i];
		(*((gcroot<FrameObject^> *)ovHandle))->SetValueArray(ConvertToString(segName), arr);
	}

	extern "C" __declspec(dllexport) void FioSetULongValues(void * ovHandle, const char* segName, unsigned long long * values, int len)
	{
		auto arr = gcnew array<unsigned long long>(len);
		for (int i = 0; i < len; i++)
			arr[i] = values[i];
		(*((gcroot<FrameObject^> *)ovHandle))->SetValueArray(ConvertToString(segName), arr);
	}

	extern "C" __declspec(dllexport) void FioSetDoubleValues(void * ovHandle, const char* segName, double * values, int len)
	{
		auto arr = gcnew array<double>(len);
		for (int i = 0; i < len; i++)
		{
			arr[i] = values[i];
		}
		(*((gcroot<FrameObject^> *)ovHandle))->SetValueArray(ConvertToString(segName), arr);
	}

	extern "C" __declspec(dllexport) void FioSetFloatValues(void * ovHandle, const char* segName, float * values, int len)
	{
		auto arr = gcnew array<float>(len);
		for (int i = 0; i < len; i++)
		{
			arr[i] = values[i];
		}
		(*((gcroot<FrameObject^> *)ovHandle))->SetValueArray(ConvertToString(segName), arr);
	}



	#pragma endregion

	#pragma region --GetArray--

		extern "C" __declspec(dllexport) void* FioGetDoubleValues(void * ovHandle, const char* segName)
		{
			auto arr = (*((gcroot<FrameObject^> *)ovHandle))->GetDoubleArray(ConvertToString(segName));
			auto ret = new ValueArray;
			ret->len = 0;
			for each (double v in arr)
				ret->len++;
			if (ret->len == 0) return NULL;

			ret->arr = new double[ret->len];
			int i = 0;
			for each (double v in arr)
				((double*)ret->arr)[i++] = v;
			return ret;
		}

		extern "C" __declspec(dllexport) void* FioGetFloatValues(void * ovHandle, const char* segName)
		{
			auto arr = (*((gcroot<FrameObject^> *)ovHandle))->GetDoubleArray(ConvertToString(segName));
			auto ret = new ValueArray;
			ret->len = 0;
			for each (float v in arr)
				ret->len++;
			if (ret->len == 0) return NULL;

			ret->arr = new float[ret->len];
			int i = 0;
			for each (float v in arr)
				((float*)ret->arr)[i++] = v;
			return ret;
		}

		extern "C" __declspec(dllexport) void* FioGetLongValues(void * ovHandle, const char* segName)
		{
			auto arr = (*((gcroot<FrameObject^> *)ovHandle))->GetDoubleArray(ConvertToString(segName));
			auto ret = new ValueArray;
			ret->len = 0;
			for each (long long v in arr)
				ret->len++;
			if (ret->len == 0) return NULL;

			ret->arr = new long long[ret->len];
			int i = 0;
			for each (long long v in arr)
				((long long*)ret->arr)[i++] = v;
			return ret;
		}

		extern "C" __declspec(dllexport) void* FioGetULongValues(void * ovHandle, const char* segName)
		{
			auto arr = (*((gcroot<FrameObject^> *)ovHandle))->GetDoubleArray(ConvertToString(segName));
			auto ret = new ValueArray;
			ret->len = 0;
			for each (unsigned long long v in arr)
				ret->len++;
			if (ret->len == 0) return NULL;

			ret->arr = new unsigned long long[ret->len];
			int i = 0;
			for each (unsigned long long v in arr)
				((unsigned long long*)ret->arr)[i++] = v;
			return ret;
		}

		extern "C" __declspec(dllexport) void* FioGetIntValues(void * ovHandle, const char* segName)
		{
			auto arr = (*((gcroot<FrameObject^> *)ovHandle))->GetDoubleArray(ConvertToString(segName));
			auto ret = new ValueArray;
			ret->len = 0;
			for each (int v in arr)
				ret->len++;
			if (ret->len == 0) return NULL;

			ret->arr = new int[ret->len];
			int i = 0;
			for each (int v in arr)
				((int*)ret->arr)[i++] = v;
			return ret;
		}

		extern "C" __declspec(dllexport) void* FioGetUIntValues(void * ovHandle, const char* segName)
		{
			auto arr = (*((gcroot<FrameObject^> *)ovHandle))->GetDoubleArray(ConvertToString(segName));
			auto ret = new ValueArray;
			ret->len = 0;
			for each (unsigned int v in arr)
				ret->len++;
			if (ret->len == 0) return NULL;

			ret->arr = new unsigned int[ret->len];
			int i = 0;
			for each (unsigned int v in arr)
				((unsigned int*)ret->arr)[i++] = v;
			return ret;
		}

		extern "C" __declspec(dllexport) void* FioGetUShortValues(void * ovHandle, const char* segName)
		{
			auto arr = (*((gcroot<FrameObject^> *)ovHandle))->GetDoubleArray(ConvertToString(segName));
			auto ret = new ValueArray;
			ret->len = 0;
			for each (unsigned short v in arr)
				ret->len++;
			if (ret->len == 0) return NULL;

			ret->arr = new unsigned short[ret->len];
			int i = 0;
			for each (unsigned short v in arr)
				((unsigned short*)ret->arr)[i++] = v;
			return ret;
		}

		extern "C" __declspec(dllexport) void* FioGetShortValues(void * ovHandle, const char* segName)
		{
			auto arr = (*((gcroot<FrameObject^> *)ovHandle))->GetDoubleArray(ConvertToString(segName));
			auto ret = new ValueArray;
			ret->len = 0;
			for each (short v in arr)
				ret->len++;
			if (ret->len == 0) return NULL;

			ret->arr = new short[ret->len];
			int i = 0;
			for each (short v in arr)
				((short*)ret->arr)[i++] = v;
			return ret;
		}

		extern "C" __declspec(dllexport) void* FioGetByteValues(void * ovHandle, const char* segName)
		{
			auto arr = (*((gcroot<FrameObject^> *)ovHandle))->GetDoubleArray(ConvertToString(segName));
			auto ret = new ValueArray;
			ret->len = 0;
			for each (unsigned char v in arr)
				ret->len++;
			if (ret->len == 0) return NULL;

			ret->arr = new unsigned char[ret->len];
			int i = 0;
			for each (unsigned char v in arr)
				((unsigned char*)ret->arr)[i++] = v;
			return ret;
		}


		extern "C" __declspec(dllexport) void* FioGetSByteValues(void * ovHandle, const char* segName)
		{
			auto arr = (*((gcroot<FrameObject^> *)ovHandle))->GetDoubleArray(ConvertToString(segName));
			auto ret = new ValueArray;
			ret->len = 0;
			for each (signed char v in arr)
				ret->len++;
			if (ret->len == 0) return NULL;

			ret->arr = new signed char[ret->len];
			int i = 0;
			for each (signed char v in arr)
				((unsigned char*)ret->arr)[i++] = v;
			return ret;
		}

		extern "C" __declspec(dllexport) void* FioGetBoolValues(void * ovHandle, const char* segName)
		{
			auto arr = (*((gcroot<FrameObject^> *)ovHandle))->GetDoubleArray(ConvertToString(segName));
			auto ret = new ValueArray;
			ret->len = 0;
			for each (bool v in arr)
				ret->len++;
			if (ret->len == 0) return NULL;

			ret->arr = new bool[ret->len];
			int i = 0;
			for each (bool v in arr)
				((bool*)ret->arr)[i++] = v;
			return ret;
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

	extern "C" __declspec(dllexport) bool FioGetBoolValue(void * ovHandle, const char* segName)
	{
		return (*((gcroot<FrameObject^> *)ovHandle))->GetBool(ConvertToString(segName));
	}
#pragma endregion

	#pragma region --Send && Recv--

	extern "C" __declspec(dllexport) void FioSendFrame(void * chHandle, void * FrameHandle)
	{
		IORunner::SendFrame(*(gcroot<FrameObject^> *)FrameHandle, *(gcroot<FioChannel^> *)chHandle);
	}

	extern "C" __declspec(dllexport) void* FioRecvFrame(void * chHandle, const char* frameName)
	{
		auto o = new gcroot<FrameObject^>();
		*o = IORunner::RecvFrame(ConvertToString(frameName), *(gcroot<FioChannel^> *)chHandle);
		return o;
	}

	#pragma endregion


}
