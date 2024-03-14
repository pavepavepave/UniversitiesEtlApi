using Universities.DB.Models;
using UniversityDto = Universities.DB.Models.UniversityDto;

namespace Universities.DB.Repository;

public interface IUniversityRepository
{
    Task<IEnumerable<UniversityDto>> GetAllAsync();
    Task<IEnumerable<UniversityDto>> GetByCountryAndNameAsync(string country, string universityName);
    Task AddRangeAsync(IEnumerable<UniversityDto> universities);
}