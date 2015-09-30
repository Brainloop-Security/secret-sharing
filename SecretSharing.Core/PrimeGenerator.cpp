#include "stdafx.h"
#include "PrimeGenerator.h"
#include "NTL\ZZ.h"
#include "NTL\ZZ_p.h"
#include "NTLHelper.h"
#include <time.h>
using namespace NTL;
using namespace System;
using namespace System::Linq;
namespace SecretSharingCore
{

	PrimeGenerator::PrimeGenerator()
	{
	}
	/*bool PrimeGenerator::WhereFunc(LoadedPrimeNumber^ number, int size){
		return number->PrimeSize == size;
	}*/
	array<Byte>^ PrimeGenerator::LoadOrGenerateRandomPrime(int ByteLength){
		// in case we have a loaded prime list we must read from the list rather than generating primes
		if (PrimeGenerator::LoadedPrimes != nullptr){
			//List<LoadedPrimeNumber^>^ filteredPrimes =  //Enumerable::ToList( Enumerable::Where
				//(PrimeGenerator::LoadedPrimes, gcnew Func<LoadedPrimeNumber^, int, bool>(&WhereFunc)));
			unsigned int time_ui = unsigned int(time(NULL));
			Random^ random = gcnew Random(time_ui);
			int randomIndex = random->Next(PrimeGenerator::LoadedPrimes->Count);
			if (PrimeGenerator::LoadedPrimes[randomIndex]->PrimeSize == ByteLength){
				return PrimeGenerator::LoadedPrimes[randomIndex]->PrimeNumber;
			}
		}
		return GenerateRandomPrime(ByteLength);
	}
	ZZ PrimeGenerator::LoadOrGenerateZZRandomPrime(int ByteLength){
		// in case we have a loaded prime list we must read from the list rather than generating primes
		if (PrimeGenerator::LoadedPrimes != nullptr){
			unsigned int time_ui = unsigned int(time(NULL));
			Random^ random = gcnew Random(time_ui);
			int randomIndex = random->Next(PrimeGenerator::LoadedPrimes->Count);
			if (PrimeGenerator::LoadedPrimes[randomIndex]->PrimeSize == ByteLength){
				return NTLHelper::ByteToZZ(PrimeGenerator::LoadedPrimes[randomIndex]->PrimeNumber);
			}
		}
		return GenerateZZRandomPrime(ByteLength);
	}
	array<Byte>^ PrimeGenerator::GenerateRandomPrime(int ByteLength){
		ZZ p = GenerateZZRandomPrime(ByteLength);
		return NTLHelper::ZZToByte(p);
	}
	ZZ PrimeGenerator::GenerateZZRandomPrime(int ByteLength){
		SetSeed(conv<ZZ>((long)time(0)));
		return GenGermainPrime_ZZ((ByteLength * 8));
	}


	vector<ZZ> PrimeGenerator::FactorizeNumber(ZZ n){
		vector<ZZ> factors;
		ZZ d = ZZ(2);
		while (n > 1)
		{
			while (n % d == 0)
			{
				factors.push_back(d);
				n /= d;

			}
			d = d + 1;
			if (d*d > n)
			{
				if (n > 1)
				{
					factors.push_back(n);
					break;
				}
			}
		}
		return factors;
	}
	ZZ PrimeGenerator::GenerateZZSecureRandomPrime(int ByteLength){

		// in case we have a loaded prime list we must read from the list rather than generating primes
		if (PrimeGenerator::LoadedPrimes != nullptr){
			unsigned int time_ui = unsigned int(time(NULL));
			Random^ random = gcnew Random(time_ui);
			int randomIndex = random->Next(PrimeGenerator::LoadedPrimes->Count);
			if (PrimeGenerator::LoadedPrimes[randomIndex]->PrimeSize == ByteLength){
				return NTLHelper::ByteToZZ(PrimeGenerator::LoadedPrimes[randomIndex]->PrimeNumber);
			}
		}

		SetSeed(conv<ZZ>((long)time(0)));
		return GenGermainPrime_ZZ((ByteLength * 8));
	}
	ZZ_p PrimeGenerator::FindRandomGenerator(ZZ p){
		//if p is a secure prime then p-1 = 2q where q is also a prime
		//given these assumptions we have:
		// http://blog.renzuocheng.com/2012/11/generate-cyclic-group-of-prime-order-and-one-of-its-generator/
		ZZ q = (p - 1) / 2;
		ZZ e2 = ZZ(2);
		ZZ_p::init(p);
		ZZ_p g = ZZ_p(1);
		//g must be greater than 1
		while (IsOne(g) || IsZero(g) || IsOne(power(g, e2)) || IsOne(power(g, q))){
			ZZ_p::init(p - 1);
			g = random_ZZ_p();
			ZZ_p::init(p);
		}
		return g;

	}

	bool PrimeGenerator::IsGeneratorOfP(ZZ p, ZZ_p g){
		ZZ_p::init(p);
		ZZ q = (p - 1) / 2;
		return !(IsOne(g) || IsZero(g) || IsOne(power(g, 2)) || IsOne(power(g, q)));
	}

}