using PowerOfAttornyApp.Service.Entities;
using System;


namespace PowerOfAttornyApp.Service
{
	public class PowerOfAttornyBuilder : IPowerOfAttornyBuilder
	{
		public PowerOfAttorny Create(Person person, Address address, DateTime expirationDate)
		{
			return new PowerOfAttorny(
				person.Id, 
				person.Name, 
				person.LastName, 
				person.BirthYear,
				$"{person.LastName} {person.Name}",

				address.Id, 
				address.Country, 
				address.City, 
				address.Street, 
				address.House, 
				
				$"{address.Country}, {address.City}, {address.Street}, {address.House}",

				expirationDate);
		}
	}
}
