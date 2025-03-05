using System.Linq.Expressions;
using AutoFixture;
using Moq;
using NUnit.Framework;
using PowerOfAttornyApp.Service.Entities;

namespace PowerOfAttornyApp.Service.Tests;

public partial class PowerOfAttornyServiceNewBehavior
{
    private PowerOfAttornyServiceNew _target;
    private Mock<IPowerOfAttornyDal> _dal;
    private Mock<IPowerOfAttornyPublisher> _publisher;
    private Fixture _fixture = new Fixture();

    [SetUp]
    public void Setup()
    {
        _dal = new Mock<IPowerOfAttornyDal>();
        _publisher = new Mock<IPowerOfAttornyPublisher>();
        _target = new PowerOfAttornyServiceNew(_dal.Object, _publisher.Object);
    }

    private void SetupDal(Person person, Address address, out string snilsNumber,
        Action<PowerOfAttorny>? caseCallback = null)
    {
        var innerSnilsNumber = _fixture.Create<string>();

        _dal.Setup(d => d.GetPerson(innerSnilsNumber))
            .Returns(person);

        _dal.Setup(d => d.GetPersonRegistrationAddress(person.Id))
            .Returns(address);

        if (caseCallback != null)
        {
            _dal.Setup(d => d.AddPowerOfAttorny(It.IsAny<PowerOfAttorny>()))
                .Callback(caseCallback);
        }

        snilsNumber = innerSnilsNumber;
    }

    public delegate Expression<Action<IPowerOfAttornyPublisher>> RegMethodSelectorProvider(PowerOfAttorny attorny);

    public static object[] RegistryByCityCases =
    {
        new object[] { "Moscow", (RegMethodSelectorProvider)(a => p => p.PublishToMoscowRegistry(a)) },
        new object[] { "Kiev", (RegMethodSelectorProvider)(a => p => p.PublishToNonMoscowRegistry(a)) }
    };

    public static object[] FoundByBirthYearCases =
    {
        new object[] { 2000, (RegMethodSelectorProvider)(a => p => p.PublishToPensionFund(a)) },
        new object[] { 2001, (RegMethodSelectorProvider)(a => p => p.PublishToUniversityFund(a)) }
    };
}