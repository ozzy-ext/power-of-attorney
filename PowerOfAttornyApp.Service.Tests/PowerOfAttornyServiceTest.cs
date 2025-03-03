using NUnit.Framework;
using Moq;
using AutoFixture;
using PowerOfAttornyApp.Service.Entities;

namespace PowerOfAttornyApp.Service.Tests
{
	[TestFixture]
	public class PowerOfAttornyServiceTest
	{
		PowerOfAttornyService _target;
		
		Mock<IPowerOfAttornyDal> _dal;
		Mock<IPowerOfAttornyPublisher> _publisher;
		Mock<IPowerOfAttornyBuilder> _builder;

		Fixture _fixture = new Fixture();

		[SetUp]
		public void Setup()
		{
			_dal =  new Mock<IPowerOfAttornyDal>(MockBehavior.Strict);
			_publisher = new Mock<IPowerOfAttornyPublisher>(MockBehavior.Strict);
			_builder =  new Mock<IPowerOfAttornyBuilder>(MockBehavior.Strict);

			_target = new PowerOfAttornyService(_dal.Object, _publisher.Object, _builder.Object);
		}

		public void VerifyAllMocks()
		{
			_dal.VerifyAll();
			_builder.VerifyAll();
		}

		[Test]
		public void CreatePowerOfAttorny_2000_Kiev_CallsCorrect()
		{
			// arrange
			string snilsNumber = _fixture.Create<string>();

			int birthYear = 2000;
			string city = "Kiev";

			var person = _fixture.Build<Person>().With(p => p.BirthYear, birthYear).Create();
			_dal.Setup(d => d.GetPerson(snilsNumber)).Returns(person);

			var address = _fixture.Build<Address>().With(a => a.City, city).Create();
			_dal.Setup(d => d.GetPersonRegistrationAddress(person.Id)).Returns(address);

			var expirationDate = new DateTime(2030, 1, 1);						
			var document = _fixture.Create<PowerOfAttorny>();
			_builder.Setup(b => b.Create(person, address, expirationDate)).Returns(document);
				
			_publisher.Setup(d => d.PublishToNonMoscowRegistry(document)).Verifiable();
			_publisher.Setup(d => d.PublishToPensionFund(document)).Verifiable();
			_dal.Setup(d => d.AddPowerOfAttorny(document)).Verifiable();

			// act
			var actual = _target.CreatePowerOfAttorny(snilsNumber);

			// assert
			Assert.AreEqual(document, actual);
			VerifyAllMocks();
		}

		[Test]
		public void CreatePowerOfAttorny_2000_Moscow_CallsCorrect()
		{
			// arrange
			string snilsNumber = _fixture.Create<string>();

			int birthYear = 2000;
			string city = "Moscow";

			var person = _fixture.Build<Person>().With(p => p.BirthYear, birthYear).Create();
			_dal.Setup(d => d.GetPerson(snilsNumber)).Returns(person);

			var address = _fixture.Build<Address>().With(a => a.City, city).Create();
			_dal.Setup(d => d.GetPersonRegistrationAddress(person.Id)).Returns(address);

			var expirationDate = new DateTime(2040, 1, 1);						
			var document = _fixture.Create<PowerOfAttorny>();
			_builder.Setup(b => b.Create(person, address, expirationDate)).Returns(document);
				
			_publisher.Setup(d => d.PublishToMoscowRegistry(document)).Verifiable();
			_publisher.Setup(d => d.PublishToPensionFund(document)).Verifiable();
			_dal.Setup(d => d.AddPowerOfAttorny(document)).Verifiable();

			// act
			var actual = _target.CreatePowerOfAttorny(snilsNumber);

			// assert
			Assert.AreEqual(document, actual);
			VerifyAllMocks();
		}

		[Test]
		public void CreatePowerOfAttorny_2001_Kiev_CallsCorrect()
		{
			// arrange
			string snilsNumber = _fixture.Create<string>();

			int birthYear = 2001;
			string city = "Kiev";

			var person = _fixture.Build<Person>().With(p => p.BirthYear, birthYear).Create();
			_dal.Setup(d => d.GetPerson(snilsNumber)).Returns(person);

			var address = _fixture.Build<Address>().With(a => a.City, city).Create();
			_dal.Setup(d => d.GetPersonRegistrationAddress(person.Id)).Returns(address);

			var expirationDate = new DateTime(2031, 1, 1);					
			var document = _fixture.Create<PowerOfAttorny>();
			_builder.Setup(b => b.Create(person, address, expirationDate)).Returns(document);
				
			_publisher.Setup(d => d.PublishToNonMoscowRegistry(document)).Verifiable();
			_publisher.Setup(d => d.PublishToUniversityFund(document)).Verifiable();
			_dal.Setup(d => d.AddPowerOfAttorny(document)).Verifiable();

			// act
			var actual = _target.CreatePowerOfAttorny(snilsNumber);

			// assert
			Assert.AreEqual(document, actual);
			VerifyAllMocks();
		}

		[Test]
		public void CreatePowerOfAttorny_2001_Moscow_CallsCorrect()
		{
			// arrange
			string snilsNumber = _fixture.Create<string>();

			int birthYear = 2001;
			string city = "Moscow";

			var person = _fixture.Build<Person>().With(p => p.BirthYear, birthYear).Create();
			_dal.Setup(d => d.GetPerson(snilsNumber)).Returns(person);

			var address = _fixture.Build<Address>().With(a => a.City, city).Create();
			_dal.Setup(d => d.GetPersonRegistrationAddress(person.Id)).Returns(address);

			var expirationDate = new DateTime(2041, 1, 1);						
			var document = _fixture.Create<PowerOfAttorny>();
			_builder.Setup(b => b.Create(person, address, expirationDate)).Returns(document);
				
			_publisher.Setup(d => d.PublishToMoscowRegistry(document)).Verifiable();
			_publisher.Setup(d => d.PublishToUniversityFund(document)).Verifiable();
			_dal.Setup(d => d.AddPowerOfAttorny(document)).Verifiable();

			// act
			var actual = _target.CreatePowerOfAttorny(snilsNumber);

			// assert
			Assert.AreEqual(document, actual);
			VerifyAllMocks();
		}
	}
}