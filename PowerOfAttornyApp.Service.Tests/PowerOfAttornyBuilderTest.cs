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

		[Test]
		public void Create_SomeNotRandom_ReturnsCorrect()
		{
			// arrange
			var person = new Person(123, "Vasily", "Ivanov", 1999);
			var address = new Address(456, "Russia", "Omsk", "Lenina", "22a");
			DateTime expirationDate = new DateTime(2100, 2, 3);

			var expected = new PowerOfAttorny(
				123, 
				"Vasily", 
				"Ivanov", 
				1999,
				"Ivanov Vasily",

				456, 
				"Russia", 
				"Omsk", 
				"Lenina", 
				"22a", 
				
				$"Russia, Omsk, Lenina, 22a",

				expirationDate);

			// act
			var actual = _target.Create(person, address, expirationDate);

			// assert
			Assert.AreEqual(expected, actual);
		}
	}
}
