#pragma once
#define  _CRT_SECURE_NO_WARNINGS 

#define  FIOCHANNEL_COM 1
#define  FIOCHANNEL_CAN 2
#define  FIOCHANNEL_TCPSERVER 3
#define  FIOCHANNEL_TCPCLIENT 4
#define  FIOCHANNEL_UDP 5
#define  FIOCHANNEL_DIO 6


extern "C" __declspec(dllexport) void*  CreateFioChannel(int channelType);

extern "C" __declspec(dllexport) void __stdcall ReleaseFioChannel(void* channelHandle);

extern "C" __declspec(dllexport) void __stdcall InitialChannel(void* channelHandle);

extern "C" __declspec(dllexport) bool __stdcall OpenChannel(void* channelHandle);

extern "C" __declspec(dllexport) void __stdcall CloseChannel(void* channelHandle);

extern "C" __declspec(dllexport) bool __stdcall ChannelIsOpen(void* channelHandle);

extern "C" __declspec(dllexport) void __stdcall SetChannelOptionN(void* channelHandle, const char* optionname, int optionvalue);

extern "C" __declspec(dllexport) void __stdcall SetChannelOptionS(void* channelHandle, const char* optionname, const char* optionvalue);