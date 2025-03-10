using AutoFixture;
using NUnit.Framework;
using PowerOfAttornyApp.Service.Entities;
using System;

namespace PowerOfAttornyApp.Service.Tests
{
	[TestFixture]
	public class PowerOfAttornyBuilderTest
	{
		PowerOfAttornyBuilder _target;

		Fixture _fixture = new Fixture();

		[SetUp]
		public void Setup()
		{
			_target = new PowerOfAttornyBuilder();
		}

		[Test]
		public void Create_Random_ReturnsCorrect()
		{
			// arrange
			var person = _fixture.Create<Person>();			
			var address = _fixture.Create<Address>();
			DateTime expirationDate =  _fixture.Create<DateTime>();

			var expected = new PowerOfAttorny(
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

			// act
			var actual = _target.Create(person, address, expirationDate);

			// assert
			Assert.AreEqual(expected, actual);
		}

		[TestCase(123, "Vasily", "Ivanov", 1999, "Ivanov Vasily", 456, "Russia", "Omsk", "Lenina", "22a", "2100.02.03", $"Russia, Omsk, Lenina, 22a")]
		[TestCase(234, "Ivan", "Petrov", 2001, "Petrov Ivan", 567, "Ukraine", "Donetsk", "Bandery", "33b", "1456.04.05", $"Ukraine, Donetsk, Bandery, 33b")]
		public void Create_SomeNotRandom_ReturnsCorrect(int personId, string personName, string personLastName, int personBirthYear, string expectedFullName,
			int addressId, string addressCountry, string addressCity, string addressStreet, string addressHouse, string expirationDateStr, string expectedFullAddress)
		{
			// arrange
			var person = new Person(personId, personName, personLastName, 1999);
			var address = new Address(addressId, addressCountry, addressCity, addressStreet, addressHouse);
			DateTime expirationDate = DateTime.ParseExact(expirationDateStr, "yyyy.MM.dd", null);

			var expected = new PowerOfAttorny(
				personId, 
				personName, 
				personLastName, 
				personBirthYear,
				expectedFullName,

				addressId, 
				addressCountry, 
				addressCity, 
				addressStreet, 
				addressHouse, 
				expectedFullAddress,

				expirationDate);

			// act
			var actual = _target.Create(person, address, expirationDate);

			// assert
			Assert.AreEqual(expected, actual);
		}
	}
}
