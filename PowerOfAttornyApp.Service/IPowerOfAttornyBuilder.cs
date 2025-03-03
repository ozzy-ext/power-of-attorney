using PowerOfAttornyApp.Service.Entities;
using System;

namespace PowerOfAttornyApp.Service
{
	public interface IPowerOfAttornyBuilder
	{
		PowerOfAttorny Create(Person person, Address address, DateTime expirationDate);
	}
}
