using System;

namespace PowerOfAttornyApp.Service.Entities
{
    public record PowerOfAttorny(
        int PersonId,          
        string PersonName,
        string PersonLastName,
        int PersonBirthYear,
        string PersonFullName,

        int AddressId,
	    string Country,
	    string City,
	    string Street,
	    string House,
        string FullAddress,
       
        DateTime ExpirationDate);

    //public class PowerOfAttorny
    //{
    //    public int PersonId { get; set; }
    //    public int AddressId { get; set; }
    //    public DateTime ExpirationDate { get; set; }
    //}
}
