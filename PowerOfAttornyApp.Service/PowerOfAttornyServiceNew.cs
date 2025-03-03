using PowerOfAttornyApp.Service.Entities;

namespace PowerOfAttornyApp.Service
{
    public class PowerOfAttornyServiceNew(
			IPowerOfAttornyDal _dal)
	{
		public PowerOfAttorny CreatePowerOfAttorny(string snilsNumber)
		{
		//	var person = _dal.GetPerson(snilsNumber);
		//	var address = _dal.GetPersonRegistrationAddress(person.Id);

		//	DateTime expirationDate = new DateTime(2030, 1, 1); 
		//	if (address.City == "Moscow")  
		//		expirationDate = expirationDate.AddYears(10);
		//	if (person.BirthYear > 2000)  
		//		expirationDate = expirationDate.AddYears(1);
			
		//	var document = new PowerOfAttorny(person.Id, address.Id, expirationDate);
		//	_dal.AddPowerOfAttorny(document);

		//	return document;
			
			return null;
		}
	}
}
