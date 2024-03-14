using Universities.DB.Models;

namespace Universities.DB.Mapping;

public sealed class UniversityMapper
{
    private static UniversityDto ToUniversityDto(University university)
    {
        return new UniversityDto
        {
            Country = university.Country,
            Name = university.Name,
            WebSites = university.WebSites
        };
    }

    public static IEnumerable<UniversityDto> ToUniversitiesDto(IEnumerable<University> universities)
    {
        return universities.Select(u => ToUniversityDto(u));
    }
    
    private static University ToUniversity(UniversityDto university)
    {
        return new University
        {
            Country = university.Country,
            Name = university.Name,
            WebSites = university.WebSites
        };
    }
    
    public static IEnumerable<University> ToUniversities(IEnumerable<UniversityDto> universities)
    {
        return universities.Select(u => ToUniversity(u));
    }
}