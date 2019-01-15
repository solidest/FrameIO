#pragma once

using namespace FrameIO::Run;
using namespace System;
ref class FrameObjectX
{
public:
	FrameObjectX();

	int NewFrameObject(String^ frameName);

	void SetValue(int oid, String^ segName, bool value);
	void SetValue(int oid, String^ segName, char value);
	void SetValue(int oid, String^ segName, signed char value);

	int GetSubObject(int oid, String^ frameName);

	bool GetBool(int oid, String^ segName);
	char GetByte(int oid, String^ segName);
	signed char GetSByte(int oid, String^ segName);

private:
	System::Collections::Generic::List<FrameObject^>^ _oo;
	int AddObject();
};

FrameObjectX::FrameObjectX()
{
	_oo = gcnew System::Collections::Generic::List<FrameObject^>();
}


inline int FrameObjectX::GetSubObject(int oid, String^ frameName)
{
	auto o = _oo[oid]->GetObject(frameName);
	_oo->Add(o);
	return _oo->Count;
}


inline int FrameObjectX::NewFrameObject(String^ frameName)
{
	_oo->Add(gcnew FrameObject);
	return _oo->Count;
}

inline void FrameObjectX::SetValue(int oid, String^ segName, bool value)
{
	_oo[oid]->SetValue(segName, value);
}

inline void FrameObjectX::SetValue(int oid, String^ segName, char value)
{
	_oo[oid]->SetValue(segName, value);
}

inline void FrameObjectX::SetValue(int oid, String^ segName, signed char value)
{
	_oo[oid]->SetValue(segName, value);
}


inline bool FrameObjectX::GetBool(int oid, String^ segName)
{
	return _oo[oid]->GetBool(segName);
}

inline char FrameObjectX::GetByte(int oid, String^ segName)
{
	return _oo[oid]->GetByte(segName);
}

inline signed char FrameObjectX::GetSByte(int oid, String^ segName)
{
	return _oo[oid]->GetSByte(segName);
}


