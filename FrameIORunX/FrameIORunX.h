#pragma once


#define  FIOCHANNEL_COM 1
#define  FIOCHANNEL_CAN 2
#define  FIOCHANNEL_TCPSERVER 3
#define  FIOCHANNEL_TCPCLIENT 4
#define  FIOCHANNEL_UDP 5
#define  FIOCHANNEL_DIO 6

extern "C" __declspec(dllexport) void* CreateFioChannel(int channelType);
extern "C" __declspec(dllexport) void ReleaseFioChannel(void* channelHandle);
