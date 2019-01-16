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

	//���ò���
	void SetOption(const char* optionName, int optionValue)
	{
		FioChannelSetOptionN(_ch, optionName, optionValue);
	}

	void SetOption(const char* optionName, const char* optionValue)
	{
		FioChannelSetOptionS(_ch, optionName, optionValue);
	}

	//��ʼ��ͨ��
	void InitialChannel()
	{
		FioChannelInitial(_ch);
	}

	//��ͨ��
	bool OpenChannel()
	{
		return FioChannelOpen(_ch);
	}

	//ͨ���Ƿ��
	bool IsOpen()
	{
		return FioChannelIsOpen(_ch);
	}

	//�ر�ͨ��
	void Close()
	{
		FioChannelClose(_ch);
	}

};