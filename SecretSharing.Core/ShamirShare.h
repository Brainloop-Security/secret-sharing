#pragma once
#include "stdafx.h"
#include "NativePtr.h"
#include "NTL\ZZ.h"
#include "NTL\ZZ_p.h"
#include <vector>
#include "ISecretShare.h"
#include "NTLHelper.h"
using namespace SecretSharingCore::Common;
using namespace System::Text;
using namespace NTL;
using namespace System;
using namespace System::Runtime::Serialization;

namespace SecretSharingCore
{
	namespace Common
	{
		[Serializable]
		public ref class ShamirShare: public ISerializable , IShare 
		{
		private:
			int _x;
			CAutoNativePtr<ZZ_p> _zz;
			CAutoNativePtr<ZZ> _prime;
			ShamirShare(SerializationInfo^ info, StreamingContext^ context){
				this->_x = info->GetInt32("x");
				array<Byte>^ y =(array<Byte>^) info->GetValue("y",array<Byte>::typeid);
				array<Byte>^ p =(array<Byte>^) info->GetValue("p",array<Byte>::typeid);
				pin_ptr<unsigned char> unmanagedP = &p[0];
				ZZ _p = ZZFromBytes(unmanagedP, p->Length);
				this->_prime = new ZZ(_p);
				ZZ_p::init(_p);
				pin_ptr<unsigned char> unmanagedY = &y[0];
				ZZ _y = ZZFromBytes(unmanagedY, y->Length);
				this->_zz = new ZZ_p(to_ZZ_p(_y));
			}
		public:
			ShamirShare(int x, array<Byte>^ y, array<Byte>^ p){
				this->_x = x;
				pin_ptr<unsigned char> unmanagedP = &p[0];
				ZZ _p = ZZFromBytes(unmanagedP, p->Length);
				this->_prime = new ZZ(_p);
				ZZ_p::init(_p);
				pin_ptr<unsigned char> unmanagedY = &y[0];
				ZZ _y = ZZFromBytes(unmanagedY, y->Length);
				this->_zz = new ZZ_p(to_ZZ_p(_y));

				
			}
			ShamirShare(const ShamirShare% rhs){
				_x = rhs._x;
				//_prime =rhs._prime;
				//_zz = rhs._zz;
			}
			const ShamirShare operator=(const ShamirShare% rhs){
				_x = rhs._x;
				return *this;
			}

			ShamirShare::ShamirShare(int x, unsigned long y){
				_x = x;
			}
			ShamirShare::ShamirShare(int x, ZZ_p* y,ZZ* prime)
			{
				_x = x;
				_zz = y;
				_prime = prime;
			}

			virtual int GetX(){
				return _x;
			}

			virtual array<Byte>^ GetY(){
				ZZ_p::init(*this->_prime);
				return NTLHelper::ZZpToByte(*_zz);
			}
			int GetYSize(){
				ZZ share;
				conv(share, *_zz);
				return share.MaxAlloc();
			}
			virtual array<Byte>^ GetP(){
				return NTLHelper::ZZToByte(*_prime);
			}

			array<Byte>^ GetArrayOfZZ(ZZ number){
				return NTLHelper::ZZToByte(number);
			}

			virtual String^ ToString() override
			{
				array<Byte>^ y=GetY();
				String^ strPrime = Convert::ToBase64String(GetP());
				String^ strShare = Convert::ToBase64String(y);
				StringBuilder^ builder = gcnew StringBuilder();
				builder->AppendFormat("X:{0}\t y:{1}\t ySize:{2}\t p:{3}\t pSize:{4}", GetX(), strShare,y->Length,strPrime, NumBytes(*_prime));
				return builder->ToString();
			}

			ShamirShare::!ShamirShare(){
				///native pointer takes care of releasing pointers
			}
			// A method called when serializing a ShamirShare.
			[System::Security::Permissions::SecurityPermissionAttribute
				(System::Security::Permissions::SecurityAction::LinkDemand, 
				Flags=System::Security::Permissions::SecurityPermissionFlag::SerializationFormatter)]
			virtual void GetObjectData( SerializationInfo^ info, StreamingContext context )
			{
				// Instead of serializing this Object*, we will   
				// serialize a SingletonSerializationHelp instead.
				//info->SetType( ShamirShare::typeid );
				info->AddValue("x",this->GetX());
				info->AddValue("y",this->GetY());
				info->AddValue("p",this->GetP());
				// No other values need to be added.
			}

		internal:
			ZZ_p* GetZZ(){
				return _zz;
			}

			ZZ* GetPrime(){
				return _prime;
			}

		protected:
			ShamirShare::~ShamirShare(){
				this->!ShamirShare();
			}
		};
	}
}