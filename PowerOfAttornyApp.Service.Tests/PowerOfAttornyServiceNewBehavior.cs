using NUnit.Framework;
using Moq;
using AutoFixture;
using PowerOfAttornyApp.Service.Entities;

namespace PowerOfAttornyApp.Service.Tests
{
	[TestFixture]
	public class PowerOfAttornyServiceNewBehavior
	{
        PowerOfAttornyServiceNew _target;
		
		Mock<IPowerOfAttornyDal> _dal;

		Fixture _fixture = new Fixture();

		[SetUp]
		public void Setup()
		{
			_dal =  new Mock<IPowerOfAttornyDal>();

			_target = new PowerOfAttornyServiceNew(_dal.Object);
		}

		[TestCase(2000, "Kiev", "2030.01.01")]
		[TestCase(2000, "Moscow", "2040.01.01")]
		[TestCase(2001, "Kiev", "2031.01.01")]
		[TestCase(2001, "Moscow", "2041.01.01")]
		public void ShouldCalcExpirationDate(int birthYear, string city, string expectedExpirationDateStr)
		{
			// arrange
			var snilsNumber = _fixture.Create<string>();

			var person = _fixture
                .Build<Person>()
                .With(p => p.BirthYear, birthYear)
                .Create();

            var address = _fixture
                .Build<Address>()
                .With(a => a.City, city)
                .Create();

            _dal.Setup(d => d.GetPerson(snilsNumber))
                .Returns(person);

			_dal.Setup(d => d.GetPersonRegistrationAddress(person.Id))
                .Returns(address);

			var expectedExpirationDate = DateTime.ParseExact(expectedExpirationDateStr, "yyyy.MM.dd", null);						
				
			// act
			var actual = _target.CreatePowerOfAttorny(snilsNumber);

			// assert
			Assert.AreEqual(expectedExpirationDate, actual.ExpirationDate);
		}

		[Test]
        public void ShouldSaveCreatedAttorney()
        {
            // arrange
            var snilsNumber = _fixture.Create<string>();
            var person = _fixture.Build<Person>().Create();
            var address = _fixture.Build<Address>().Create();
            PowerOfAttorny? savedPoa = null;

            _dal.Setup(d => d.GetPerson(snilsNumber))
                .Returns(person);

            _dal.Setup(d => d.GetPersonRegistrationAddress(person.Id))
                .Returns(address);

            _dal.Setup(d => d.AddPowerOfAttorny(It.IsAny<PowerOfAttorny>()))
                .Callback<PowerOfAttorny>(a => savedPoa = a);

            // act
            _target.CreatePowerOfAttorny(snilsNumber);

            // assert
            Assert.NotNull(savedPoa);
            Assert.AreEqual(address.Id, savedPoa!.AddressId);
            Assert.AreEqual(person.Id, savedPoa.PersonId);
            Assert.AreNotEqual(DateTime.MinValue, savedPoa.ExpirationDate);
        }
    }
}