using PowerOfAttornyApp.Service.Entities;

namespace PowerOfAttornyApp.Service
{
    public class PowerOfAttornyService(
			IPowerOfAttornyDal _dal,
			IPowerOfAttornyPublisher _powerOfAttornyPublisher,
			IPowerOfAttornyBuilder _builder)
	{
		public PowerOfAttorny CreatePowerOfAttorny(string snilsNumber)
		{
			var person = _dal.GetPerson(snilsNumber);
			var address = _dal.GetPersonRegistrationAddress(person.Id);

			Action<PowerOfAttorny> registryPublisher;
			Action<PowerOfAttorny> fundPublisher;
			DateTime expirationDate = new DateTime(2030, 1, 1); 

			if (address.City == "Moscow")
			{
				expirationDate = expirationDate.AddYears(10);
				registryPublisher = _powerOfAttornyPublisher.PublishToMoscowRegistry;
			}
			else
				registryPublisher = _powerOfAttornyPublisher.PublishToNonMoscowRegistry;

			if (person.BirthYear > 2000)
			{
				expirationDate = expirationDate.AddYears(1);
				fundPublisher = _powerOfAttornyPublisher.PublishToUniversityFund;
			}
			else
				fundPublisher = _powerOfAttornyPublisher.PublishToPensionFund;
			
			var document = _builder.Create(person, address, expirationDate);
			_dal.AddPowerOfAttorny(document);
			registryPublisher(document);
			fundPublisher(document);

			return document;
		}
	}
}
