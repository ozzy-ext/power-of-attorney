using PowerOfAttornyApp.Service.Entities;
using System;


namespace PowerOfAttornyApp.Service
{
	public interface IPowerOfAttornyDal
	{
		Address GetPersonRegistrationAddress(int personId);
		Person GetPerson(string snilsNumber);
		void AddPowerOfAttorny(PowerOfAttorny powerOfAttorny);
	}
}
