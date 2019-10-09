namespace EducationApp.BusinessLayer.Helpers.Interfaces
{
    public interface IMapperHelper
    {
        MapTo MapToModelItem<MapFrom, MapTo>(MapFrom source, MapTo destination);
        MapTo MapToEntity<MapFrom, MapTo>(MapFrom source, MapTo destination);
        //ICollection<MapTo> MapEntitiesToModel<MapFrom, MapTo>(IEnumerable<MapFrom> entities, ICollection<MapTo> itemsList);
    }
}
