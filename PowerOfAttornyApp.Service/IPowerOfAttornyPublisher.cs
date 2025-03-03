using PowerOfAttornyApp.Service.Entities;

namespace PowerOfAttornyApp.Service
{
	public interface IPowerOfAttornyPublisher
	{
		void PublishToMoscowRegistry(PowerOfAttorny document);
		void PublishToNonMoscowRegistry(PowerOfAttorny document);
		void PublishToPensionFund(PowerOfAttorny document);
		void PublishToUniversityFund(PowerOfAttorny document);
	}
}
