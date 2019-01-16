#pragma once
#pragma comment(lib,"C:\\Kiyun\\FrameIO\\FrameIO\\x64\\Debug\\FrameIORunX.lib")


#define  FIOCHANNEL_COM 1
#define  FIOCHANNEL_CAN 2
#define  FIOCHANNEL_TCPSERVER 3
#define  FIOCHANNEL_TCPCLIENT 4
#define  FIOCHANNEL_UDP 5
#define  FIOCHANNEL_DIO 6

extern "C" __declspec(dllimport) void FioInitial(const char* config);

#pragma region --Channel--

extern "C" __declspec(dllimport) void* FioChannelCreate(int channelType);

extern "C" __declspec(dllimport) void FioChannelRelease(void* channelHandle);

extern "C" __declspec(dllimport) void FioChannelInitial(void* channelHandle);

extern "C" __declspec(dllimport) bool FioChannelOpen(void* channelHandle);

extern "C" __declspec(dllimport) void FioChannelClose(void* channelHandle);

extern "C" __declspec(dllimport) bool FioChannelIsOpen(void* channelHandle);

extern "C" __declspec(dllimport) void FioChannelSetOptionN(void* channelHandle, const char* optionname, int optionvalue);

extern "C" __declspec(dllimport) void FioChannelSetOptionS(void* channelHandle, const char* optionname, const char* optionvalue);


#pragma endregion

#pragma region --Object--

extern "C" __declspec(dllimport) void* FioObjectCreate(const char * frameName);


extern "C" __declspec(dllimport) void* FioObjectGetValue(void * ovHandle, const char* segName);

extern "C" __declspec(dllimport) void FioObjectSetValue(void * ovHandle, const char* segName, void* oValueHandle);

extern "C" __declspec(dllimport) void FioObjectRelease(void * ovHandle);

extern "C" __declspec(dllimport) void* FioObjectGetArray(void * ovHandle, const char* segName);

extern "C" __declspec(dllimport) void FioObjectReleaseArray(void * ovsHandle);

extern "C" __declspec(dllimport) void FioObjectSetArray(void * ovHandle, const char* segName, void** osValueHandle, int len);


#pragma endregion

#pragma region --SetValue--

extern "C" __declspec(dllimport) void FioSetBoolValue(void * ovHandle, const char* segName, bool value);

extern "C" __declspec(dllimport) void FioSetByteValue(void * ovHandle, const char* segName, unsigned char value);

extern "C" __declspec(dllimport) void FioSetSByteValue(void * ovHandle, const char* segName, signed char value);

extern "C" __declspec(dllimport) void FioSetShortValue(void * ovHandle, const char* segName, signed short value);

extern "C" __declspec(dllimport) void FioSetUShortValue(void * ovHandle, const char* segName, unsigned short value);

extern "C" __declspec(dllimport) void FioSetIntValue(void * ovHandle, const char* segName, signed int value);

extern "C" __declspec(dllimport) void FioSetUIntValue(void * ovHandle, const char* segName, unsigned int value);

extern "C" __declspec(dllimport) void FioSetLongValue(void * ovHandle, const char* segName, long long value);

extern "C" __declspec(dllimport) void FioSetULongValue(void * ovHandle, const char* segName, unsigned long long value);

extern "C" __declspec(dllimport) void FioSetFloatValue(void * ovHandle, const char* segName, float value);

extern "C" __declspec(dllimport) void FioSetDoubleValue(void * ovHandle, const char* segName, double value);


#pragma endregion

#pragma region --GetValue--

extern "C" __declspec(dllimport) double FioGetDoubleValue(void * ovHandle, const char* segName);

extern "C" __declspec(dllimport) float FioGetFloatValue(void * ovHandle, const char* segName);

extern "C" __declspec(dllimport) long long FioGetLongValue(void * ovHandle, const char* segName);

extern "C" __declspec(dllimport) unsigned long long FioGetULongValue(void * ovHandle, const char* segName);

extern "C" __declspec(dllimport) int FioGetIntValue(void * ovHandle, const char* segName);

extern "C" __declspec(dllimport) unsigned int FioGetUIntValue(void * ovHandle, const char* segName);

extern "C" __declspec(dllimport) short FioGetShortValue(void * ovHandle, const char* segName);

extern "C" __declspec(dllimport) unsigned short FioGetUShortValue(void * ovHandle, const char* segName);

extern "C" __declspec(dllimport) unsigned char FioGetByteValue(void * ovHandle, const char* segName);

extern "C" __declspec(dllimport) signed char FioGetSByteValue(void * ovHandle, const char* segName);

extern "C" __declspec(dllimport) bool FioGetBoolValue(void * ovHandle, const char* segName);
#pragma endregion

#pragma region --Send && Recv--

extern "C" __declspec(dllimport) void FioSendFrame(void * chHandle, void * FrameHandle);

extern "C" __declspec(dllimport) void* FioRecvFrame(void * chHandle, const char* frameName);

#pragma endregion


#pragma region --SetArray--

struct  ValueArray
{
	int len;
	void* arr;
};

extern "C" __declspec(dllimport) void FioSetBoolValues(void * ovHandle, const char* segName, bool * values, int len);

extern "C" __declspec(dllimport) void FioSetByteValues(void * ovHandle, const char* segName, unsigned char * values, int len);

extern "C" __declspec(dllimport) void FioSetSByteValues(void * ovHandle, const char* segName, signed char * values, int len);

extern "C" __declspec(dllimport) void FioSetShortValues(void * ovHandle, const char* segName, short * values, int len);

extern "C" __declspec(dllimport) void FioSetUShortValues(void * ovHandle, const char* segName, unsigned short * values, int len);

extern "C" __declspec(dllimport) void FioSetIntValues(void * ovHandle, const char* segName, int * values, int len);

extern "C" __declspec(dllimport) void FioSetUIntValues(void * ovHandle, const char* segName, unsigned int * values, int len);

extern "C" __declspec(dllimport) void FioSetLongValues(void * ovHandle, const char* segName, long long * values, int len);

extern "C" __declspec(dllimport) void FioSetULongValues(void * ovHandle, const char* segName, unsigned long long * values, int len);

extern "C" __declspec(dllimport) void FioSetDoubleValues(void * ovHandle, const char* segName, double * values, int len);

extern "C" __declspec(dllimport) void FioSetFloatValues(void * ovHandle, const char* segName, float * values, int len);

#pragma endregion

#pragma region --GetArray--

extern "C" __declspec(dllimport) void* FioGetDoubleValues(void * ovHandle, const char* segName);

extern "C" __declspec(dllimport) void* FioGetFloatValues(void * ovHandle, const char* segName);

extern "C" __declspec(dllimport) void* FioGetLongValues(void * ovHandle, const char* segName);

extern "C" __declspec(dllimport) void* FioGetULongValues(void * ovHandle, const char* segName);

extern "C" __declspec(dllimport) void* FioGetIntValues(void * ovHandle, const char* segName);

extern "C" __declspec(dllimport) void* FioGetUIntValues(void * ovHandle, const char* segName);

extern "C" __declspec(dllimport) void* FioGetUShortValues(void * ovHandle, const char* segName);

extern "C" __declspec(dllimport) void* FioGetShortValues(void * ovHandle, const char* segName);

extern "C" __declspec(dllimport) void* FioGetByteValues(void * ovHandle, const char* segName);

extern "C" __declspec(dllimport) void* FioGetSByteValues(void * ovHandle, const char* segName);

extern "C" __declspec(dllimport) void* FioGetBoolValues(void * ovHandle, const char* segName);

#pragma endregion
