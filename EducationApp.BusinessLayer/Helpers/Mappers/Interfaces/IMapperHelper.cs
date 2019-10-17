namespace EducationApp.BusinessLayer.Helpers.Mappers.Interfaces
{
    public interface IMapperHelper
    {
        MapTo Map<MapFrom, MapTo>(MapFrom source) where MapTo : new();

    }
}
