using System;

namespace PowerOfAttornyApp.Service.Entities
{
	public record Address(int Id, string Country, string City, string Street, string House);
	//public class Address
	//{
	//	public int Id { get; set; }
	//	public string Country { get; set; }
	//	public string City { get; set; }
	//	public string Street { get; set; }
	//	public string House { get; set; }
	//}
}
