using NUnit.Framework;
using Moq;
using AutoFixture;
using PowerOfAttornyApp.Service.Entities;
using System.Linq.Expressions;

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

		[TestCaseSource(nameof(Cases))]
		public void CreatePowerOfAttorny_2000_Kiev_CallsCorrect(string city, int birthYear, DateTime expectedExpirationDate,
			 PublishMethodProvider regMethodProvider, PublishMethodProvider fundMethodProvider)
		{
			// arrange
			string snilsNumber = _fixture.Create<string>();

			var person = _fixture.Build<Person>().With(p => p.BirthYear, birthYear).Create();
			_dal.Setup(d => d.GetPerson(snilsNumber)).Returns(person);

			var address = _fixture.Build<Address>().With(a => a.City, city).Create();
			_dal.Setup(d => d.GetPersonRegistrationAddress(person.Id)).Returns(address);
					
			var document = _fixture.Create<PowerOfAttorny>();
			_builder.Setup(b => b.Create(person, address, expectedExpirationDate)).Returns(document);
				
			_publisher.Setup(regMethodProvider(document)).Verifiable();
			_publisher.Setup(fundMethodProvider(document)).Verifiable();
			_dal.Setup(d => d.AddPowerOfAttorny(document)).Verifiable();

			// act
			var actual = _target.CreatePowerOfAttorny(snilsNumber);

			// assert
			Assert.AreEqual(document, actual);
			VerifyAllMocks();
		}

		public delegate Expression<Action<IPowerOfAttornyPublisher>> PublishMethodProvider(PowerOfAttorny document);
		public static object[] Cases =
		{
			new object[] { "Kiev", 2000, new DateTime(2030, 1, 1), 
				(PublishMethodProvider)(d => p => p.PublishToNonMoscowRegistry(d)), (PublishMethodProvider)(d => p => p.PublishToPensionFund(d)) },
			new object[] { "Moscow", 2000, new DateTime(2040, 1, 1), 
				(PublishMethodProvider)(d => p => p.PublishToMoscowRegistry(d)), (PublishMethodProvider)(d => p => p.PublishToPensionFund(d)) },
			new object[] { "Kiev", 2001, new DateTime(2031, 1, 1),
				(PublishMethodProvider)(d => p => p.PublishToNonMoscowRegistry(d)), (PublishMethodProvider)(d => p => p.PublishToUniversityFund(d)) },
			new object[] { "Moscow", 2001, new DateTime(2041, 1, 1),
				(PublishMethodProvider)(d => p => p.PublishToMoscowRegistry(d)), (PublishMethodProvider)(d => p => p.PublishToUniversityFund(d)) },
		};
	}
}