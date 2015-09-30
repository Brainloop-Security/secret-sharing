#pragma once
#include "stdafx.h"
#include <vector>
#include "NTL\ZZ.h"
#include "LoadedPrimeNumber.h"
using namespace System;
using namespace std;
using namespace System::Collections::Generic;
using namespace NTL;
namespace SecretSharingCore
{
	public ref class PrimeGenerator
	{
	private:
		
	public:
		static List<LoadedPrimeNumber^>^ LoadedPrimes;
		static void SetLoadedPrimes(List<LoadedPrimeNumber^>^ loadedPrimes){
			PrimeGenerator::LoadedPrimes = loadedPrimes;
		}
		//bool PrimeGenerator::WhereFunc(LoadedPrimeNumber^ number, int size);
		PrimeGenerator();
		array<Byte>^ GenerateRandomPrime(int ByteLength);
		ZZ GenerateZZRandomPrime(int ByteLength);
		ZZ GenerateZZSecureRandomPrime(int ByteLength);
		vector<ZZ> FactorizeNumber(ZZ n);
		ZZ_p FindRandomGenerator(ZZ p);
		bool IsGeneratorOfP(ZZ p, ZZ_p g);
		array<Byte>^ PrimeGenerator::LoadOrGenerateRandomPrime(int ByteLength);
		ZZ PrimeGenerator::LoadOrGenerateZZRandomPrime(int ByteLength);
	};

}