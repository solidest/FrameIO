// CRC.cpp : 定义 DLL 应用程序的导出函数。
//

#include "stdafx.h"
#include <stdint.h>

extern "C" __declspec(dllexport) uint32_t sum8(const unsigned char* data, const int data_len)
{
	if (data != NULL)
	{
		auto checkxorT = 0U;
		int i = 0;
		for (checkxorT = data[0], i = 1; i < data_len; i++)
		{
			checkxorT += data[i];
		}
		return checkxorT;// &0xff;
	}
	else
		return 0;
}
extern "C" __declspec(dllexport) uint32_t xor8(const unsigned char* data, const int data_len)
{
	if (data != NULL)
	{
		auto checkxorT = 0U;
		int i = 0;
		for (checkxorT = data[0], i = 1; i < data_len; i++)
		{
			checkxorT ^= data[i];
		}
		return checkxorT;//& 0xff;
	}
	else
		return 0;
}
extern "C" __declspec(dllexport) uint16_t sum16(const unsigned char* data, const int data_len)
{
	if (data != NULL && data_len >= 2)
	{
		auto checkxorT = 0U;
		int i = 0;
		checkxorT = data[0];
		checkxorT = checkxorT << 8;
		checkxorT |= data[1];
		unsigned short temp = 0;
		for (i = 2; i < data_len; i = i + 2)
		{
			temp = 0;
			temp = data[i];
			temp = temp << 8;
			temp |= data[i + 1];
			checkxorT += temp;
		}
		return checkxorT;
	}
	else
		return 0;
}
extern "C" __declspec(dllexport) uint16_t sum16_false(const unsigned char* data, const int data_len)
{
	if (data != NULL && data_len >= 2)
	{
		unsigned short checkxorT = 0U;
		unsigned short checkxor = 0U;
		int i = 0;
		checkxorT = data[0];
		checkxorT = checkxorT << 8;
		checkxorT |= data[1];
		unsigned short temp = 0;
		for (i = 2; i < data_len; i = i + 2)
		{
			temp = 0;
			temp = data[i];
			temp = temp << 8;
			temp |= data[i + 1];
			checkxorT += temp;
		}

		checkxor = checkxorT<<8;
		return checkxor | (checkxorT>>8);
	}
	else
		return 0;
}
extern "C" __declspec(dllexport) uint16_t xor16(const unsigned char* data, const int data_len)
{
	if (data != NULL && data_len >= 2)
	{
		auto checkxorT = 0U;
		int i = 0;
		checkxorT = data[0];
		checkxorT = checkxorT << 8;
		checkxorT |= data[1];
		unsigned short temp = 0;
		for (i = 2; i < data_len; i = i + 2)
		{
			temp = 0;
			temp = data[i];
			temp = temp << 8;
			temp |= data[i + 1];
			checkxorT ^= temp;
		}
		return checkxorT;
	}
	else
		return 0;
}
extern "C" __declspec(dllexport) uint16_t xor16_false(const unsigned char* data, const int data_len)
{
	if (data != NULL && data_len >= 2)
	{
		auto checkxorT = 0U;
		auto checkxor = 0U;
		int i = 0;
		checkxorT = data[0];
		checkxorT = checkxorT << 8;
		checkxorT |= data[1];
		unsigned short temp = 0;
		for (i = 2; i < data_len; i = i + 2)
		{
			temp = 0;
			temp = data[i];
			temp = temp << 8;
			temp |= data[i + 1];
			checkxorT ^= temp;
		}
		checkxor = checkxorT << 8;
		return checkxor | (checkxorT >> 8);
	}
	else
		return 0;
}
extern "C" __declspec(dllexport) uint32_t sum32(const unsigned char* data, const int data_len)
{
	if (data != NULL && data_len >= 4)
	{
		unsigned long checkxorT = 0U;
		int i = 0;
		checkxorT = data[0];
		checkxorT = checkxorT << 8;
		checkxorT |= data[1];
		checkxorT = checkxorT << 8;
		checkxorT |= data[2];
		checkxorT = checkxorT << 8;
		checkxorT |= data[3];
		unsigned long temp = 0;
		for (i = 4; i < data_len; i = i + 4)
		{

			temp = 0;

			temp = data[i];
			temp = temp << 8;
			temp |= data[i + 1];
			temp = temp << 8;
			temp |= data[i + 2];
			temp = temp << 8;
			temp |= data[i + 3];
			checkxorT += temp;
		}
		return checkxorT;
	}
	else
		return 0;
}
extern "C" __declspec(dllexport) uint32_t sum32_false(const unsigned char* data, const int data_len)
{
	if (data != NULL && data_len >= 4)
	{
		if (data != NULL && data_len >= 4)
		{
			unsigned long checkxorT = 0U;
			unsigned long a = 0U;
			int i = 0;
			checkxorT = data[0];
			checkxorT = checkxorT << 8;
			checkxorT |= data[1];
			checkxorT = checkxorT << 8;
			checkxorT |= data[2];
			checkxorT = checkxorT << 8;
			checkxorT |= data[3];
			unsigned long temp = 0;
			for (i = 4; i < data_len; i = i + 4)
			{
				temp = 0;

				temp = data[i];
				temp = temp << 8;
				temp |= data[i + 1];
				temp = temp << 8;
				temp |= data[i + 2];
				temp = temp << 8;
				temp |= data[i + 3];
				checkxorT += temp;
			}
			a = checkxorT;

			a = (a << 24) | ((a << 8) & 0xff0000) | ((a >> 8) & 0xff00) | (a >> 24);

			return a;
		}
		else
			return 0;
	}
	else
		return 0;
}

extern "C" __declspec(dllexport) uint32_t xor32(const unsigned char* data, const int data_len)
{
	if (data != NULL && data_len >= 4)
	{
		unsigned long checkxorT = 0U;
		int i = 0;
		checkxorT = data[0];
		checkxorT = checkxorT << 8;
		checkxorT |= data[1];
		checkxorT = checkxorT << 8;
		checkxorT |= data[2];
		checkxorT = checkxorT << 8;
		checkxorT |= data[3];
		unsigned long temp = 0;
		for (i = 4; i < data_len; i = i + 4)
		{
			temp = 0;

			temp = data[i];
			temp = temp << 8;
			temp |= data[i + 1];
			temp = temp << 8;
			temp |= data[i + 2];
			temp = temp << 8;
			temp |= data[i + 3];
			checkxorT ^= temp;
		}

		return checkxorT;
	}
	else
		return 0;
}
extern "C" __declspec(dllexport) uint32_t xor32_false(const unsigned char* data, const int data_len)
{
	if (data != NULL && data_len >= 4)
	{
		unsigned long checkxorT = 0U;
		unsigned long a = 0U;

		int i = 0;
		checkxorT = data[0];
		checkxorT = checkxorT << 8;
		checkxorT |= data[1];
		checkxorT = checkxorT << 8;
		checkxorT |= data[2];
		checkxorT = checkxorT << 8;
		checkxorT |= data[3];
		unsigned long temp = 0;
		for (i = 4; i < data_len; i = i + 4)
		{
			temp = 0;

			temp = data[i];
			temp = temp << 8;
			temp |= data[i + 1];
			temp = temp << 8;
			temp |= data[i + 2];
			temp = temp << 8;
			temp |= data[i + 3];
			checkxorT ^= temp;
		}

		a = checkxorT;

		a = (a << 24) | ((a << 8) & 0xff0000) | ((a >> 8) & 0xff00) | (a >> 24);

		return a;
	}
	else
		return 0;
}

const uint64_t crc_tab64[256] = {
	0x0000000000000000ull,
	0x42F0E1EBA9EA3693ull,
	0x85E1C3D753D46D26ull,
	0xC711223CFA3E5BB5ull,
	0x493366450E42ECDFull,
	0x0BC387AEA7A8DA4Cull,
	0xCCD2A5925D9681F9ull,
	0x8E224479F47CB76Aull,
	0x9266CC8A1C85D9BEull,
	0xD0962D61B56FEF2Dull,
	0x17870F5D4F51B498ull,
	0x5577EEB6E6BB820Bull,
	0xDB55AACF12C73561ull,
	0x99A54B24BB2D03F2ull,
	0x5EB4691841135847ull,
	0x1C4488F3E8F96ED4ull,
	0x663D78FF90E185EFull,
	0x24CD9914390BB37Cull,
	0xE3DCBB28C335E8C9ull,
	0xA12C5AC36ADFDE5Aull,
	0x2F0E1EBA9EA36930ull,
	0x6DFEFF5137495FA3ull,
	0xAAEFDD6DCD770416ull,
	0xE81F3C86649D3285ull,
	0xF45BB4758C645C51ull,
	0xB6AB559E258E6AC2ull,
	0x71BA77A2DFB03177ull,
	0x334A9649765A07E4ull,
	0xBD68D2308226B08Eull,
	0xFF9833DB2BCC861Dull,
	0x388911E7D1F2DDA8ull,
	0x7A79F00C7818EB3Bull,
	0xCC7AF1FF21C30BDEull,
	0x8E8A101488293D4Dull,
	0x499B3228721766F8ull,
	0x0B6BD3C3DBFD506Bull,
	0x854997BA2F81E701ull,
	0xC7B97651866BD192ull,
	0x00A8546D7C558A27ull,
	0x4258B586D5BFBCB4ull,
	0x5E1C3D753D46D260ull,
	0x1CECDC9E94ACE4F3ull,
	0xDBFDFEA26E92BF46ull,
	0x990D1F49C77889D5ull,
	0x172F5B3033043EBFull,
	0x55DFBADB9AEE082Cull,
	0x92CE98E760D05399ull,
	0xD03E790CC93A650Aull,
	0xAA478900B1228E31ull,
	0xE8B768EB18C8B8A2ull,
	0x2FA64AD7E2F6E317ull,
	0x6D56AB3C4B1CD584ull,
	0xE374EF45BF6062EEull,
	0xA1840EAE168A547Dull,
	0x66952C92ECB40FC8ull,
	0x2465CD79455E395Bull,
	0x3821458AADA7578Full,
	0x7AD1A461044D611Cull,
	0xBDC0865DFE733AA9ull,
	0xFF3067B657990C3Aull,
	0x711223CFA3E5BB50ull,
	0x33E2C2240A0F8DC3ull,
	0xF4F3E018F031D676ull,
	0xB60301F359DBE0E5ull,
	0xDA050215EA6C212Full,
	0x98F5E3FE438617BCull,
	0x5FE4C1C2B9B84C09ull,
	0x1D14202910527A9Aull,
	0x93366450E42ECDF0ull,
	0xD1C685BB4DC4FB63ull,
	0x16D7A787B7FAA0D6ull,
	0x5427466C1E109645ull,
	0x4863CE9FF6E9F891ull,
	0x0A932F745F03CE02ull,
	0xCD820D48A53D95B7ull,
	0x8F72ECA30CD7A324ull,
	0x0150A8DAF8AB144Eull,
	0x43A04931514122DDull,
	0x84B16B0DAB7F7968ull,
	0xC6418AE602954FFBull,
	0xBC387AEA7A8DA4C0ull,
	0xFEC89B01D3679253ull,
	0x39D9B93D2959C9E6ull,
	0x7B2958D680B3FF75ull,
	0xF50B1CAF74CF481Full,
	0xB7FBFD44DD257E8Cull,
	0x70EADF78271B2539ull,
	0x321A3E938EF113AAull,
	0x2E5EB66066087D7Eull,
	0x6CAE578BCFE24BEDull,
	0xABBF75B735DC1058ull,
	0xE94F945C9C3626CBull,
	0x676DD025684A91A1ull,
	0x259D31CEC1A0A732ull,
	0xE28C13F23B9EFC87ull,
	0xA07CF2199274CA14ull,
	0x167FF3EACBAF2AF1ull,
	0x548F120162451C62ull,
	0x939E303D987B47D7ull,
	0xD16ED1D631917144ull,
	0x5F4C95AFC5EDC62Eull,
	0x1DBC74446C07F0BDull,
	0xDAAD56789639AB08ull,
	0x985DB7933FD39D9Bull,
	0x84193F60D72AF34Full,
	0xC6E9DE8B7EC0C5DCull,
	0x01F8FCB784FE9E69ull,
	0x43081D5C2D14A8FAull,
	0xCD2A5925D9681F90ull,
	0x8FDAB8CE70822903ull,
	0x48CB9AF28ABC72B6ull,
	0x0A3B7B1923564425ull,
	0x70428B155B4EAF1Eull,
	0x32B26AFEF2A4998Dull,
	0xF5A348C2089AC238ull,
	0xB753A929A170F4ABull,
	0x3971ED50550C43C1ull,
	0x7B810CBBFCE67552ull,
	0xBC902E8706D82EE7ull,
	0xFE60CF6CAF321874ull,
	0xE224479F47CB76A0ull,
	0xA0D4A674EE214033ull,
	0x67C58448141F1B86ull,
	0x253565A3BDF52D15ull,
	0xAB1721DA49899A7Full,
	0xE9E7C031E063ACECull,
	0x2EF6E20D1A5DF759ull,
	0x6C0603E6B3B7C1CAull,
	0xF6FAE5C07D3274CDull,
	0xB40A042BD4D8425Eull,
	0x731B26172EE619EBull,
	0x31EBC7FC870C2F78ull,
	0xBFC9838573709812ull,
	0xFD39626EDA9AAE81ull,
	0x3A28405220A4F534ull,
	0x78D8A1B9894EC3A7ull,
	0x649C294A61B7AD73ull,
	0x266CC8A1C85D9BE0ull,
	0xE17DEA9D3263C055ull,
	0xA38D0B769B89F6C6ull,
	0x2DAF4F0F6FF541ACull,
	0x6F5FAEE4C61F773Full,
	0xA84E8CD83C212C8Aull,
	0xEABE6D3395CB1A19ull,
	0x90C79D3FEDD3F122ull,
	0xD2377CD44439C7B1ull,
	0x15265EE8BE079C04ull,
	0x57D6BF0317EDAA97ull,
	0xD9F4FB7AE3911DFDull,
	0x9B041A914A7B2B6Eull,
	0x5C1538ADB04570DBull,
	0x1EE5D94619AF4648ull,
	0x02A151B5F156289Cull,
	0x4051B05E58BC1E0Full,
	0x87409262A28245BAull,
	0xC5B073890B687329ull,
	0x4B9237F0FF14C443ull,
	0x0962D61B56FEF2D0ull,
	0xCE73F427ACC0A965ull,
	0x8C8315CC052A9FF6ull,
	0x3A80143F5CF17F13ull,
	0x7870F5D4F51B4980ull,
	0xBF61D7E80F251235ull,
	0xFD913603A6CF24A6ull,
	0x73B3727A52B393CCull,
	0x31439391FB59A55Full,
	0xF652B1AD0167FEEAull,
	0xB4A25046A88DC879ull,
	0xA8E6D8B54074A6ADull,
	0xEA16395EE99E903Eull,
	0x2D071B6213A0CB8Bull,
	0x6FF7FA89BA4AFD18ull,
	0xE1D5BEF04E364A72ull,
	0xA3255F1BE7DC7CE1ull,
	0x64347D271DE22754ull,
	0x26C49CCCB40811C7ull,
	0x5CBD6CC0CC10FAFCull,
	0x1E4D8D2B65FACC6Full,
	0xD95CAF179FC497DAull,
	0x9BAC4EFC362EA149ull,
	0x158E0A85C2521623ull,
	0x577EEB6E6BB820B0ull,
	0x906FC95291867B05ull,
	0xD29F28B9386C4D96ull,
	0xCEDBA04AD0952342ull,
	0x8C2B41A1797F15D1ull,
	0x4B3A639D83414E64ull,
	0x09CA82762AAB78F7ull,
	0x87E8C60FDED7CF9Dull,
	0xC51827E4773DF90Eull,
	0x020905D88D03A2BBull,
	0x40F9E43324E99428ull,
	0x2CFFE7D5975E55E2ull,
	0x6E0F063E3EB46371ull,
	0xA91E2402C48A38C4ull,
	0xEBEEC5E96D600E57ull,
	0x65CC8190991CB93Dull,
	0x273C607B30F68FAEull,
	0xE02D4247CAC8D41Bull,
	0xA2DDA3AC6322E288ull,
	0xBE992B5F8BDB8C5Cull,
	0xFC69CAB42231BACFull,
	0x3B78E888D80FE17Aull,
	0x7988096371E5D7E9ull,
	0xF7AA4D1A85996083ull,
	0xB55AACF12C735610ull,
	0x724B8ECDD64D0DA5ull,
	0x30BB6F267FA73B36ull,
	0x4AC29F2A07BFD00Dull,
	0x08327EC1AE55E69Eull,
	0xCF235CFD546BBD2Bull,
	0x8DD3BD16FD818BB8ull,
	0x03F1F96F09FD3CD2ull,
	0x41011884A0170A41ull,
	0x86103AB85A2951F4ull,
	0xC4E0DB53F3C36767ull,
	0xD8A453A01B3A09B3ull,
	0x9A54B24BB2D03F20ull,
	0x5D45907748EE6495ull,
	0x1FB5719CE1045206ull,
	0x919735E51578E56Cull,
	0xD367D40EBC92D3FFull,
	0x1476F63246AC884Aull,
	0x568617D9EF46BED9ull,
	0xE085162AB69D5E3Cull,
	0xA275F7C11F7768AFull,
	0x6564D5FDE549331Aull,
	0x279434164CA30589ull,
	0xA9B6706FB8DFB2E3ull,
	0xEB46918411358470ull,
	0x2C57B3B8EB0BDFC5ull,
	0x6EA7525342E1E956ull,
	0x72E3DAA0AA188782ull,
	0x30133B4B03F2B111ull,
	0xF7021977F9CCEAA4ull,
	0xB5F2F89C5026DC37ull,
	0x3BD0BCE5A45A6B5Dull,
	0x79205D0E0DB05DCEull,
	0xBE317F32F78E067Bull,
	0xFCC19ED95E6430E8ull,
	0x86B86ED5267CDBD3ull,
	0xC4488F3E8F96ED40ull,
	0x0359AD0275A8B6F5ull,
	0x41A94CE9DC428066ull,
	0xCF8B0890283E370Cull,
	0x8D7BE97B81D4019Full,
	0x4A6ACB477BEA5A2Aull,
	0x089A2AACD2006CB9ull,
	0x14DEA25F3AF9026Dull,
	0x562E43B4931334FEull,
	0x913F6188692D6F4Bull,
	0xD3CF8063C0C759D8ull,
	0x5DEDC41A34BBEEB2ull,
	0x1F1D25F19D51D821ull,
	0xD80C07CD676F8394ull,
	0x9AFCE626CE85B507ull
};

/******************************************************************************
 * Name:    CRC-4/ITU           x4+x+1
 * Poly:    0x03
 * Init:    0x00
 * Refin:   True
 * Refout:  True
 * Xorout:  0x00
 * Note:
 *****************************************************************************/
extern "C" __declspec(dllexport)
uint8_t crc4_itu(uint8_t *data, uint32_t length)
{
	uint8_t i;
	uint8_t crc = 0;                // Initial value
	while (length--)
	{
		crc ^= *data++;                 // crc ^= *data; data++;
		for (i = 0; i < 8; ++i)
		{
			if (crc & 1)
				crc = (crc >> 1) ^ 0x0C;// 0x0C = (reverse 0x03)>>(8-4)
			else
				crc = (crc >> 1);
		}
	}
	return crc;
}

/******************************************************************************
 * Name:    CRC-5/EPC           x5+x3+1
 * Poly:    0x09
 * Init:    0x09
 * Refin:   False
 * Refout:  False
 * Xorout:  0x00
 * Note:
 *****************************************************************************/
extern "C" __declspec(dllexport)
uint8_t crc5_epc(uint8_t *data, uint32_t length)
{
	uint8_t i;
	uint8_t crc = 0x48;        // Initial value: 0x48 = 0x09<<(8-5)
	while (length--)
	{
		crc ^= *data++;        // crc ^= *data; data++;
		for (i = 0; i < 8; i++)
		{
			if (crc & 0x80)
				crc = (crc << 1) ^ 0x48;        // 0x48 = 0x09<<(8-5)
			else
				crc <<= 1;
		}
	}
	return crc >> 3;
}

/******************************************************************************
 * Name:    CRC-5/ITU           x5+x4+x2+1
 * Poly:    0x15
 * Init:    0x00
 * Refin:   True
 * Refout:  True
 * Xorout:  0x00
 * Note:
 *****************************************************************************/
extern "C" __declspec(dllexport)
uint8_t crc5_itu(uint8_t *data, uint32_t length)
{
	uint8_t i;
	uint8_t crc = 0;                // Initial value
	while (length--)
	{
		crc ^= *data++;                 // crc ^= *data; data++;
		for (i = 0; i < 8; ++i)
		{
			if (crc & 1)
				crc = (crc >> 1) ^ 0x15;// 0x15 = (reverse 0x15)>>(8-5)
			else
				crc = (crc >> 1);
		}
	}
	return crc;
}

/******************************************************************************
 * Name:    CRC-5/USB           x5+x2+1
 * Poly:    0x05
 * Init:    0x1F
 * Refin:   True
 * Refout:  True
 * Xorout:  0x1F
 * Note:
 *****************************************************************************/
extern "C" __declspec(dllexport)
uint8_t crc5_usb(uint8_t *data, uint32_t length)
{
	uint8_t i;
	uint8_t crc = 0x1F;                // Initial value
	while (length--)
	{
		crc ^= *data++;                 // crc ^= *data; data++;
		for (i = 0; i < 8; ++i)
		{
			if (crc & 1)
				crc = (crc >> 1) ^ 0x14;// 0x14 = (reverse 0x05)>>(8-5)
			else
				crc = (crc >> 1);
		}
	}
	return crc ^ 0x1F;
}

/******************************************************************************
 * Name:    CRC-6/ITU           x6+x+1
 * Poly:    0x03
 * Init:    0x00
 * Refin:   True
 * Refout:  True
 * Xorout:  0x00
 * Note:
 *****************************************************************************/
extern "C" __declspec(dllexport)
uint8_t crc6_itu(uint8_t *data, uint32_t length)
{
	uint8_t i;
	uint8_t crc = 0;         // Initial value
	while (length--)
	{
		crc ^= *data++;        // crc ^= *data; data++;
		for (i = 0; i < 8; ++i)
		{
			if (crc & 1)
				crc = (crc >> 1) ^ 0x30;// 0x30 = (reverse 0x03)>>(8-6)
			else
				crc = (crc >> 1);
		}
	}
	return crc;
}

/******************************************************************************
 * Name:    CRC-7/MMC           x7+x3+1
 * Poly:    0x09
 * Init:    0x00
 * Refin:   False
 * Refout:  False
 * Xorout:  0x00
 * Use:     MultiMediaCard,SD,ect.
 *****************************************************************************/
extern "C" __declspec(dllexport)
uint8_t crc7_mmc(uint8_t *data, uint32_t length)
{
	uint8_t i;
	uint8_t crc = 0;        // Initial value
	while (length--)
	{
		crc ^= *data++;        // crc ^= *data; data++;
		for (i = 0; i < 8; i++)
		{
			if (crc & 0x80)
				crc = (crc << 1) ^ 0x12;        // 0x12 = 0x09<<(8-7)
			else
				crc <<= 1;
		}
	}
	return crc >> 1;
}

/******************************************************************************
 * Name:    CRC-8               x8+x2+x+1
 * Poly:    0x07
 * Init:    0x00
 * Refin:   False
 * Refout:  False
 * Xorout:  0x00
 * Note:
 *****************************************************************************/
extern "C" __declspec(dllexport)
uint8_t crc8(uint8_t *data, uint32_t length)
{
	uint8_t i;
	uint8_t crc = 0;        // Initial value
	while (length--)
	{
		crc ^= *data++;        // crc ^= *data; data++;
		for (i = 0; i < 8; i++)
		{
			if (crc & 0x80)
				crc = (crc << 1) ^ 0x07;
			else
				crc <<= 1;
		}
	}
	return crc;
}

/******************************************************************************
 * Name:    CRC-8/ITU           x8+x2+x+1
 * Poly:    0x07
 * Init:    0x00
 * Refin:   False
 * Refout:  False
 * Xorout:  0x55
 * Alias:   CRC-8/ATM
 *****************************************************************************/
extern "C" __declspec(dllexport)
uint8_t crc8_itu(uint8_t *data, uint32_t length)
{
	uint8_t i;
	uint8_t crc = 0;        // Initial value
	while (length--)
	{
		crc ^= *data++;        // crc ^= *data; data++;
		for (i = 0; i < 8; i++)
		{
			if (crc & 0x80)
				crc = (crc << 1) ^ 0x07;
			else
				crc <<= 1;
		}
	}
	return crc ^ 0x55;
}

/******************************************************************************
 * Name:    CRC-8/ROHC          x8+x2+x+1
 * Poly:    0x07
 * Init:    0xFF
 * Refin:   True
 * Refout:  True
 * Xorout:  0x00
 * Note:
 *****************************************************************************/
extern "C" __declspec(dllexport)
uint8_t crc8_rohc(uint8_t *data, uint32_t length)
{
	uint8_t i;
	uint8_t crc = 0xFF;         // Initial value
	while (length--)
	{
		crc ^= *data++;            // crc ^= *data; data++;
		for (i = 0; i < 8; ++i)
		{
			if (crc & 1)
				crc = (crc >> 1) ^ 0xE0;        // 0xE0 = reverse 0x07
			else
				crc = (crc >> 1);
		}
	}
	return crc;
}

/******************************************************************************
 * Name:    CRC-8/MAXIM         x8+x5+x4+1
 * Poly:    0x31
 * Init:    0x00
 * Refin:   True
 * Refout:  True
 * Xorout:  0x00
 * Alias:   DOW-CRC,CRC-8/IBUTTON
 * Use:     Maxim(Dallas)'s some devices,e.g. DS18B20
 *****************************************************************************/
extern "C" __declspec(dllexport)
uint8_t crc8_maxim(uint8_t *data, uint32_t length)
{
	uint8_t i;
	uint8_t crc = 0;         // Initial value
	while (length--)
	{
		crc ^= *data++;        // crc ^= *data; data++;
		for (i = 0; i < 8; i++)
		{
			if (crc & 1)
				crc = (crc >> 1) ^ 0x8C;        // 0x8C = reverse 0x31
			else
				crc >>= 1;
		}
	}
	return crc;
}

/******************************************************************************
 * Name:    CRC-16/IBM          x16+x15+x2+1
 * Poly:    0x8005
 * Init:    0x0000
 * Refin:   True
 * Refout:  True
 * Xorout:  0x0000
 * Alias:   CRC-16,CRC-16/ARC,CRC-16/LHA
 *****************************************************************************/
extern "C" __declspec(dllexport)
uint16_t crc16_ibm(uint8_t *data, uint32_t length)
{
	uint8_t i;
	uint16_t crc = 0;        // Initial value
	while (length--)
	{
		crc ^= *data++;        // crc ^= *data; data++;
		for (i = 0; i < 8; ++i)
		{
			if (crc & 1)
				crc = (crc >> 1) ^ 0xA001;        // 0xA001 = reverse 0x8005
			else
				crc = (crc >> 1);
		}
	}
	return crc;
}

/******************************************************************************
 * Name:    CRC-16/MAXIM        x16+x15+x2+1
 * Poly:    0x8005
 * Init:    0x0000
 * Refin:   True
 * Refout:  True
 * Xorout:  0xFFFF
 * Note:
 *****************************************************************************/
extern "C" __declspec(dllexport)
uint16_t crc16_maxim(uint8_t *data, uint32_t length)
{
	uint8_t i;
	uint16_t crc = 0;        // Initial value
	while (length--)
	{
		crc ^= *data++;        // crc ^= *data; data++;
		for (i = 0; i < 8; ++i)
		{
			if (crc & 1)
				crc = (crc >> 1) ^ 0xA001;        // 0xA001 = reverse 0x8005
			else
				crc = (crc >> 1);
		}
	}
	return ~crc;    // crc^0xffff
}

/******************************************************************************
 * Name:    CRC-16/USB          x16+x15+x2+1
 * Poly:    0x8005
 * Init:    0xFFFF
 * Refin:   True
 * Refout:  True
 * Xorout:  0xFFFF
 * Note:
 *****************************************************************************/
extern "C" __declspec(dllexport)
uint16_t crc16_usb(uint8_t *data, uint32_t length)
{
	uint8_t i;
	uint16_t crc = 0xffff;        // Initial value
	while (length--)
	{
		crc ^= *data++;            // crc ^= *data; data++;
		for (i = 0; i < 8; ++i)
		{
			if (crc & 1)
				crc = (crc >> 1) ^ 0xA001;        // 0xA001 = reverse 0x8005
			else
				crc = (crc >> 1);
		}
	}
	return ~crc;    // crc^0xffff
}

/******************************************************************************
 * Name:    CRC-16/MODBUS       x16+x15+x2+1
 * Poly:    0x8005
 * Init:    0xFFFF
 * Refin:   True
 * Refout:  True
 * Xorout:  0x0000
 * Note:
 *****************************************************************************/
extern "C" __declspec(dllexport)
uint16_t crc16_modbus(uint8_t *data, uint32_t length)
{
	uint8_t i;
	uint16_t crc = 0xffff;        // Initial value
	while (length--)
	{
		crc ^= *data++;            // crc ^= *data; data++;
		for (i = 0; i < 8; ++i)
		{
			if (crc & 1)
				crc = (crc >> 1) ^ 0xA001;        // 0xA001 = reverse 0x8005
			else
				crc = (crc >> 1);
		}
	}
	return crc;
}

/******************************************************************************
 * Name:    CRC-16/CCITT        x16+x12+x5+1
 * Poly:    0x1021
 * Init:    0x0000
 * Refin:   True
 * Refout:  True
 * Xorout:  0x0000
 * Alias:   CRC-CCITT,CRC-16/CCITT-TRUE,CRC-16/KERMIT
 *****************************************************************************/
extern "C" __declspec(dllexport)
uint16_t crc16_ccitt(uint8_t *data, uint32_t length)
{
	uint8_t i;
	uint16_t crc = 0;        // Initial value
	while (length--)
	{
		crc ^= *data++;        // crc ^= *data; data++;
		for (i = 0; i < 8; ++i)
		{
			if (crc & 1)
				crc = (crc >> 1) ^ 0x8408;        // 0x8408 = reverse 0x1021
			else
				crc = (crc >> 1);
		}
	}
	return crc;
}

/******************************************************************************
 * Name:    CRC-16/CCITT-FALSE   x16+x12+x5+1
 * Poly:    0x1021
 * Init:    0xFFFF
 * Refin:   False
 * Refout:  False
 * Xorout:  0x0000
 * Note:
 *****************************************************************************/
extern "C" __declspec(dllexport)
uint16_t crc16_ccitt_false(uint8_t *data, uint32_t length)
{
	uint8_t i;
	uint16_t crc = 0xffff;        //Initial value
	while (length--)
	{
		crc ^= (uint16_t)(*data++) << 8; // crc ^= (uint6_t)(*data)<<8; data++;
		for (i = 0; i < 8; ++i)
		{
			if (crc & 0x8000)
				crc = (crc << 1) ^ 0x1021;
			else
				crc <<= 1;
		}
	}
	return crc;
}

/******************************************************************************
 * Name:    CRC-16/X25          x16+x12+x5+1
 * Poly:    0x1021
 * Init:    0xFFFF
 * Refin:   True
 * Refout:  True
 * Xorout:  0XFFFF
 * Note:
 *****************************************************************************/
extern "C" __declspec(dllexport)
uint16_t crc16_x25(uint8_t *data, uint32_t length)
{
	uint8_t i;
	uint16_t crc = 0xffff;        // Initial value
	while (length--)
	{
		crc ^= *data++;            // crc ^= *data; data++;
		for (i = 0; i < 8; ++i)
		{
			if (crc & 1)
				crc = (crc >> 1) ^ 0x8408;        // 0x8408 = reverse 0x1021
			else
				crc = (crc >> 1);
		}
	}
	return ~crc;                // crc^Xorout
}

/******************************************************************************
 * Name:    CRC-16/XMODEM       x16+x12+x5+1
 * Poly:    0x1021
 * Init:    0x0000
 * Refin:   False
 * Refout:  False
 * Xorout:  0x0000
 * Alias:   CRC-16/ZMODEM,CRC-16/ACORN
 *****************************************************************************/
extern "C" __declspec(dllexport)
uint16_t crc16_xmodem(uint8_t *data, uint32_t length)
{
	uint8_t i;
	uint16_t crc = 0;            // Initial value
	while (length--)
	{
		crc ^= (uint16_t)(*data++) << 8; // crc ^= (uint16_t)(*data)<<8; data++;
		for (i = 0; i < 8; ++i)
		{
			if (crc & 0x8000)
				crc = (crc << 1) ^ 0x1021;
			else
				crc <<= 1;
		}
	}
	return crc;
}

/******************************************************************************
 * Name:    CRC-16/DNP          x16+x13+x12+x11+x10+x8+x6+x5+x2+1
 * Poly:    0x3D65
 * Init:    0x0000
 * Refin:   True
 * Refout:  True
 * Xorout:  0xFFFF
 * Use:     M-Bus,ect.
 *****************************************************************************/
extern "C" __declspec(dllexport)
uint16_t crc16_dnp(uint8_t *data, uint32_t length)
{
	uint8_t i;
	uint16_t crc = 0;            // Initial value
	while (length--)
	{
		crc ^= *data++;            // crc ^= *data; data++;
		for (i = 0; i < 8; ++i)
		{
			if (crc & 1)
				crc = (crc >> 1) ^ 0xA6BC;        // 0xA6BC = reverse 0x3D65
			else
				crc = (crc >> 1);
		}
	}
	return ~crc;                // crc^Xorout
}

/******************************************************************************
 * Name:    CRC-32  x32+x26+x23+x22+x16+x12+x11+x10+x8+x7+x5+x4+x2+x+1
 * Poly:    0x4C11DB7
 * Init:    0xFFFFFFF
 * Refin:   True
 * Refout:  True
 * Xorout:  0xFFFFFFF
 * Alias:   CRC_32/ADCCP
 * Use:     WinRAR,ect.
 *****************************************************************************/
extern "C" __declspec(dllexport)
uint32_t crc32(uint8_t *data, uint32_t length)
{
	uint8_t i;
	uint32_t crc = 0xffffffff;        // Initial value
	while (length--)
	{
		crc ^= *data++;                // crc ^= *data; data++;
		for (i = 0; i < 8; ++i)
		{
			if (crc & 1)
				crc = (crc >> 1) ^ 0xEDB88320;// 0xEDB88320= reverse 0x04C11DB7
			else
				crc = (crc >> 1);
		}
	}
	return ~crc;
}

/******************************************************************************
 * Name:    CRC-32/MPEG-2  x32+x26+x23+x22+x16+x12+x11+x10+x8+x7+x5+x4+x2+x+1
 * Poly:    0x4C11DB7
 * Init:    0xFFFFFFF
 * Refin:   False
 * Refout:  False
 * Xorout:  0x0000000
 * Note:
 *****************************************************************************/
extern "C" __declspec(dllexport)
uint32_t crc32_mpeg_2(uint8_t *data, uint32_t length)
{
	uint8_t i;
	uint32_t crc = 0xffffffff;  // Initial value
	while (length--)
	{
		crc ^= (uint32_t)(*data++) << 24;// crc ^=(uint32_t)(*data)<<24; data++;
		for (i = 0; i < 8; ++i)
		{
			if (crc & 0x80000000)
				crc = (crc << 1) ^ 0x04C11DB7;
			else
				crc <<= 1;
		}
	}
	return crc;
}

/******************************************************************************
 * Name:    CRC-64-WE
 * Poly:    0x42F0E1EBA9EA3693
 * Init:	0xFFFFFFFFFFFFFFFF
 * Refin:	false
 * Refout:	false
 * Xorout:  0xFFFFFFFFFFFFFFFF
 * Note:
 *****************************************************************************/
extern "C" __declspec(dllexport)
uint64_t crc64_we(uint8_t *data, uint32_t length) {

	uint64_t crc;
	size_t a;

	crc = 0xFFFFFFFFFFFFFFFFull;

	if (data != NULL) for (a = 0; a < length; a++) {

		crc = (crc << 8) ^ crc_tab64[((crc >> 56) ^ (uint64_t)*data++) & 0x00000000000000FFull];
	}

	return crc ^ 0xFFFFFFFFFFFFFFFFull;

}

/******************************************************************************
 * Name:    CRC-64
 * Poly:    0x42F0E1EBA9EA3693
 * Init:    0x0000000000000000
 * Refin:   false
 * Refout:  false
 * Xorout:  0x0000000000000000
 * Note:
 *****************************************************************************/
extern "C" __declspec(dllexport)
uint64_t crc64(unsigned char *data, uint32_t length) {

	uint64_t crc;
	size_t a;

	crc = 0x0000000000000000ull;

	if (data != NULL) for (a = 0; a < length; a++) {

		crc = (crc << 8) ^ crc_tab64[((crc >> 56) ^ (uint64_t)*data++) & 0x00000000000000FFull];
	}

	return crc;

}  /* crc_64_ecma */
