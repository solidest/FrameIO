#pragma once
#include "FrameIORunX.h"

public class FioChannelX
{
private:
	void * _ch;

public:

	FioChannelX(int channelType)
	{
		_ch = FioChannelCreate(channelType);
	}

	~FioChannelX()
	{
		FioChannelRelease(_ch);
	}

	//设置参数
	void SetOption(const char* optionName, int optionValue)
	{
		FioChannelSetOptionN(_ch, optionName, optionValue);
	}

	void SetOption(const char* optionName, const char* optionValue)
	{
		FioChannelSetOptionS(_ch, optionName, optionValue);
	}

	//初始化通道
	void InitialChannel()
	{
		FioChannelInitial(_ch);
	}

	//打开通道
	bool OpenChannel()
	{
		return FioChannelOpen(_ch);
	}

	//通道是否打开
	bool IsOpen()
	{
		return FioChannelIsOpen(_ch);
	}

	//关闭通道
	void Close()
	{
		FioChannelClose(_ch);
	}

};