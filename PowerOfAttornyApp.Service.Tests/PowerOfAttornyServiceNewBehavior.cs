using System.Linq.Expressions;
using NUnit.Framework;
using Moq;
using AutoFixture;
using PowerOfAttornyApp.Service.Entities;

namespace PowerOfAttornyApp.Service.Tests
{
	[TestFixture]
	public partial class PowerOfAttornyServiceNewBehavior
	{
        [TestCase(2000, "Kiev", "2030.01.01")]
		[TestCase(2000, "Moscow", "2040.01.01")]
		[TestCase(2001, "Kiev", "2031.01.01")]
		[TestCase(2001, "Moscow", "2041.01.01")]
		public void ShouldCalcExpirationDate(int birthYear, string city, string expectedExpirationDateStr)
		{
			// arrange
			var person = _fixture
                .Build<Person>()
                .With(p => p.BirthYear, birthYear)
                .Create();

            var address = _fixture
                .Build<Address>()
                .With(a => a.City, city)
                .Create();

            SetupDal(person, address, out var snilsNumber);

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
            var person = _fixture.Build<Person>().Create();
            var address = _fixture.Build<Address>().Create();
            PowerOfAttorny? savedPoa = null;

            SetupDal(person, address, out var snilsNumber, poa => savedPoa = poa);

            // act
            _target.CreatePowerOfAttorny(snilsNumber);

            // assert
            Assert.NotNull(savedPoa);
            Assert.AreEqual(address.Id, savedPoa!.AddressId);
            Assert.AreEqual(person.Id, savedPoa.PersonId);
            Assert.AreNotEqual(DateTime.MinValue, savedPoa.ExpirationDate);
        }

        [Test]
        public void ShouldCreateAttorney()
        {
            // arrange
            var person = _fixture.Create<Person>();
            var address = _fixture.Create<Address>();
            DateTime expirationDate = _fixture.Create<DateTime>();

            var expected = new PowerOfAttorny(
                person.Id,
                person.Name,
                person.LastName,
                person.BirthYear,
                PowerOfAttornyServiceNew.BuildPersonFullName(person),

                address.Id,
                address.Country,
                address.City,
                address.Street,
                address.House,
                PowerOfAttornyServiceNew.BuildFullAddress(address),

                expirationDate);

            PowerOfAttorny? savedPoa = null;

            SetupDal(person, address, out var snilsNumber, poa => savedPoa = poa);

            // act
            _target.CreatePowerOfAttorny(snilsNumber);

            // assert
            Assert.NotNull(savedPoa);
            Assert.AreEqual(expected with { ExpirationDate = savedPoa!.ExpirationDate }, savedPoa);
        }

        [Test]
        public void ShouldBuildPersonFullname()
        {
            // arrange
            var person = _fixture.Create<Person>();
            var expectedFullName = $"{person.LastName} {person.Name}";

            // act
            var actualFullName = PowerOfAttornyServiceNew.BuildPersonFullName(person);

            // assert
            Assert.AreEqual(expectedFullName, actualFullName);
        }

        [Test]
        public void ShouldBuildFullAddress()
        {
            // arrange
            var address = _fixture.Create<Address>();
            var expectedFullAddress = $"{address.Country}, {address.City}, {address.Street}, {address.House}";

            // act
            var actualFullAddress = PowerOfAttornyServiceNew.BuildFullAddress(address);

            // assert
            Assert.AreEqual(expectedFullAddress, actualFullAddress);
        }

        [TestCaseSource(nameof(RegistryByCityCases))]
        public void ShouldUseRegistryDueToCity(string city, RegMethodSelectorProvider regMethodSelectorProvider)
        {
            // arrange
            PowerOfAttorny? savedPoa = null;

            var person = _fixture.Build<Person>().Create();

            var address = _fixture.Build<Address>()
                .With(a => a.City, city)
                .Create();

            SetupDal(person, address, out var snilsNumber, poa => savedPoa = poa);

            // act
            _target.CreatePowerOfAttorny(snilsNumber);

            // assert
            Assert.NotNull(savedPoa);
            _publisher.Verify(regMethodSelectorProvider(savedPoa!));
            _publisher.Verify(r => r.PublishToPensionFund(It.IsAny<PowerOfAttorny>()), Times.AtMost(1));
            _publisher.Verify(r => r.PublishToUniversityFund(It.IsAny<PowerOfAttorny>()), Times.AtMost(1));
            _publisher.VerifyNoOtherCalls();
        }

        [TestCaseSource(nameof(FoundByBirthYearCases))]
        public void ShouldUseFoundDueToBirthYear(int birthYear, RegMethodSelectorProvider regMethodSelectorProvider)
        {
            // arrange
            PowerOfAttorny? savedPoa = null;

            var person = _fixture.Build<Person>()
                .With(p => p.BirthYear, birthYear)
                .Create();

            var address = _fixture.Build<Address>().Create();

            SetupDal(person, address, out var snilsNumber, poa => savedPoa = poa);

            // act
            _target.CreatePowerOfAttorny(snilsNumber);

            // assert
            Assert.NotNull(savedPoa);
            _publisher.Verify(regMethodSelectorProvider(savedPoa!));
            _publisher.Verify(r => r.PublishToMoscowRegistry(It.IsAny<PowerOfAttorny>()), Times.AtMost(1));
            _publisher.Verify(r => r.PublishToNonMoscowRegistry(It.IsAny<PowerOfAttorny>()), Times.AtMost(1));
            _publisher.VerifyNoOtherCalls();
        }
    }
}