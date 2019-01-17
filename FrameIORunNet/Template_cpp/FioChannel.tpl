#pragma once
#include "FrameIORunX.h"

class FioChannelX
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

	void * GetHandle()
	{
		return _ch;
	}

	void SetOption(const char* optionName, int optionValue)
	{
		FioChannelSetOptionN(_ch, optionName, optionValue);
	}

	void SetOption(const char* optionName, const char* optionValue)
	{
		FioChannelSetOptionS(_ch, optionName, optionValue);
	}

	void InitialChannel()
	{
		FioChannelInitial(_ch);
	}

	bool OpenChannel()
	{
		return FioChannelOpen(_ch);
	}

	bool IsOpen()
	{
		return FioChannelIsOpen(_ch);
	}

	void Close()
	{
		FioChannelClose(_ch);
	}

};