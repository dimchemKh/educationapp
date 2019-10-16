namespace EducationApp.BusinessLayer.Helpers.Interfaces
{
    public interface IMapperHelper
    {
        MapTo MapToModelItem<MapFrom, MapTo>(MapFrom source) where MapTo : new();
    }
}
