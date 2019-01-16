#pragma once
#include "FrameIORunX.h"

struct  ValueArray
{
	int len;
	void* arr;
};

class FioObjextXArray
{
private:
	ValueArray* _h;
	FioObjextX** _oo;

public:
	FioObjextXArray(void* ovsHandle)
	{
		_h = (ValueArray*)ovsHandle;
		_oo = new FioObjextX*[_h->len];
		for (int i = 0; i < _h->len; i++)
			_oo[i] = new FioObjextX(((void**)(_h->arr))[i]);
	}

	~FioObjextXArray()
	{
		for (int i = 0; i < _h->len; i++)
			delete _oo[i];
		FioObjectReleaseArray(_h);
	}

	int GetLen()
	{
		return _h->len;
	}

	FioObjextX& GetFioObjectX(int index)
	{
		return *_oo[index];
	}
};

class FioObjextX
{

private:
	void * _o;


public:
	FioObjextX(const char* frameName)
	{
		_o = FioObjectCreate(frameName);
	}

	FioObjextX(void* ovHandle)
	{
		_o = ovHandle;
	}

	~FioObjextX()
	{
		FioObjectRelease(_o);
	}

public:
	void SetObjectX(const char* segname, FioObjextX& value)
	{
		FioObjectSetValue(_o, segname, value._o);
	}

	void SetObjectXArray(const char* segname, FioObjextX** values, int len)
	{
		auto arr = new void*[len];
		for (int i = 0; i < len; i++)
		{
			arr[i] = values[i]->GetHandle();
		}
		FioObjectSetArray(_o, segname, arr, len);
	}

	void* GetObjectXHandle(const char* segname)
	{
		return FioObjectGetValue(_o, segname);
	}

	void* GetObjectXArrayHandle(const char* segname)
	{
		return FioObjectGetArray(_o, segname);
	}

public:
	void * GetHandle()
	{
		return _o;
	}

public:
	void SetArray(const char* segname, bool* arr, int len)
	{
		FioSetBoolValues(_o, segname, arr, len);
	}

	void SetArray(const char* segname, unsigned char* arr, int len)
	{
		FioSetByteValues(_o, segname, arr, len);
	}

	void SetArray(const char* segname, signed char* arr, int len)
	{
		FioSetSByteValues(_o, segname, arr, len);
	}

	void SetArray(const char* segname, short* arr, int len)
	{
		FioSetShortValues(_o, segname, arr, len);
	}

	void SetArray(const char* segname,unsigned short* arr, int len)
	{
		FioSetUShortValues(_o, segname, arr, len);
	}

	void SetArray(const char* segname, int* arr, int len)
	{
		FioSetIntValues(_o, segname, arr, len);
	}

	void SetArray(const char* segname, unsigned int* arr, int len)
	{
		FioSetUIntValues(_o, segname, arr, len);
	}

	void SetArray(const char* segname, long long* arr, int len)
	{
		FioSetLongValues(_o, segname, arr, len);
	}

	void SetArray(const char* segname, unsigned long long* arr, int len)
	{
		FioSetULongValues(_o, segname, arr, len);
	}

	void SetArray(const char* segname, float* arr, int len)
	{
		FioSetFloatValues(_o, segname, arr, len);
	}

	void SetArray(const char* segname, double* arr, int len)
	{
		FioSetDoubleValues(_o, segname, arr, len);
	}

public:
	void GetArray(const char* segname, bool* arr)
	{
		auto o = (ValueArray*)FioGetBoolValues(_o, segname);
		for (int i = 0; i < o->len; i++)
			arr[i] = ((bool*)o->arr)[i];
		delete[] o->arr;
		delete o;
	}

	void GetArray(const char* segname, unsigned char* arr)
	{
		auto o = (ValueArray*)FioGetByteValues(_o, segname);
		for (int i = 0; i < o->len; i++)
			arr[i] = ((unsigned char*)o->arr)[i];
		delete[] o->arr;
		delete o;
	}

	void GetArray(const char* segname, signed char* arr)
	{
		auto o = (ValueArray*)FioGetSByteValues(_o, segname);
		for (int i = 0; i < o->len; i++)
			arr[i] = ((signed char*)o->arr)[i];
		delete[] o->arr;
		delete o;
	}

	void GetArray(const char* segname, short* arr)
	{
		auto o = (ValueArray*)FioGetShortValues(_o, segname);
		for (int i = 0; i < o->len; i++)
			arr[i] = ((short*)o->arr)[i];
		delete[] o->arr;
		delete o;
	}

	void GetArray(const char* segname, unsigned short* arr)
	{
		auto o = (ValueArray*)FioGetUShortValues(_o, segname);
		for (int i = 0; i < o->len; i++)
			arr[i] = ((unsigned short*)o->arr)[i];
		delete[] o->arr;
		delete o;
	}

	void GetArray(const char* segname, int* arr)
	{
		auto o = (ValueArray*)FioGetIntValues(_o, segname);
		for (int i = 0; i < o->len; i++)
			arr[i] = ((int*)o->arr)[i];
		delete[] o->arr;
		delete o;
	}

	void GetArray(const char* segname, unsigned int* arr)
	{
		auto o = (ValueArray*)FioGetUIntValues(_o, segname);
		for (int i = 0; i < o->len; i++)
			arr[i] = ((unsigned int*)o->arr)[i];
		delete[] o->arr;
		delete o;
	}

	void GetArray(const char* segname, long long* arr)
	{
		auto o = (ValueArray*)FioGetLongValues(_o, segname);
		for (int i = 0; i < o->len; i++)
			arr[i] = ((long long*)o->arr)[i];
		delete[] o->arr;
		delete o;
	}

	void GetArray(const char* segname, unsigned long long* arr)
	{
		auto o = (ValueArray*)FioGetULongValues(_o, segname);
		for (int i = 0; i < o->len; i++)
			arr[i] = ((unsigned long long*)o->arr)[i];
		delete[] o->arr;
		delete o;
	}

	void GetArray(const char* segname, double* arr)
	{
		auto o = (ValueArray*)FioGetDoubleValues(_o, segname);
		for (int i = 0; i < o->len; i++)
			arr[i] = ((double*)o->arr)[i];
		delete[] o->arr;
		delete o;
	}

	void GetArray(const char* segname, float* arr)
	{
		auto o = (ValueArray*)FioGetFloatValues(_o, segname);
		for (int i = 0; i < o->len; i++)
			arr[i] = ((float*)o->arr)[i];
		delete[] o->arr;
		delete o;
	}

public:

	void SetValue(const char*  segname, bool value)
	{
		FioSetBoolValue(_o, segname, value);
	}

	void SetValue(const char*  segname, unsigned char value)
	{
		FioSetByteValue(_o, segname, value);
	}

	void SetValue(const char*  segname, signed char value)
	{
		FioSetSByteValue(_o, segname, value);
	}

	void SetValue(const char*  segname, short value)
	{
		FioSetShortValue(_o, segname, value);
	}

	void SetValue(const char*  segname, unsigned short value)
	{
		FioSetUShortValue(_o, segname, value);
	}

	void SetValue(const char*  segname, int value)
	{
		FioSetIntValue(_o, segname, value);
	}

	void SetValue(const char*  segname, unsigned int value)
	{
		FioSetUIntValue(_o, segname, value);
	}

	void SetValue(const char*  segname, long value)
	{
		FioSetLongValue(_o, segname, value);
	}

	void SetValue(const char*  segname, unsigned long value)
	{
		FioSetULongValue(_o, segname, value);
	}

	void SetValue(const char*  segname, double value)
	{
		FioSetDoubleValue(_o, segname, value);
	}

	void SetValue(const char*  segname, float value)
	{
		FioSetFloatValue(_o, segname, value);
	}

	bool GetBoolValue(const char*  segname)
	{
		return FioGetBoolValue(_o, segname);
	}

	unsigned char GetByteValue(const char*  segname)
	{
		return FioGetByteValue(_o, segname);
	}

	signed char GetSByteValue(const char*  segname)
	{
		return FioGetSByteValue(_o, segname);
	}

	short GetShortValue(const char*  segname)
	{
		return FioGetShortValue(_o, segname);
	}
	
	unsigned short GetUShortValue(const char*  segname)
	{
		return FioGetUShortValue(_o, segname);
	}

	int GetIntValue(const char*  segname)
	{
		return FioGetIntValue(_o, segname);
	}

	unsigned int GetUIntValue(const char*  segname)
	{
		return FioGetUIntValue(_o, segname);
	}

	long long GetLongValue(const char*  segname)
	{
		return FioGetLongValue(_o, segname);
	}
	
	unsigned long long GetULongValue(const char*  segname)
	{
		return FioGetULongValue(_o, segname);
	}

	double GetDoubleValue(const char*  segname)
	{
		return FioGetDoubleValue(_o, segname);
	}

	float GetFloatValue(const char*  segname)
	{
		return FioGetFloatValue(_o, segname);
	}



};
