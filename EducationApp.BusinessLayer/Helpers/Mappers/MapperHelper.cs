using EducationApp.BusinessLogic.Helpers.Mappers.Interfaces;
using System;
using System.Linq;

namespace EducationApp.BusinessLogic.Helpers.Mappers
{
    public class MapperHelper : IMapperHelper
    {
        public MapTo Map<MapFrom, MapTo>(MapFrom source) where MapTo : new()
        {
            var propertiesFrom = typeof(MapFrom).GetProperties();

            var propertiesTo = typeof(MapTo).GetProperties();

            var instance = (MapTo)Activator.CreateInstance(typeof(MapTo));

            for (int i = 0; i < propertiesTo.Length; i++)
            {
                var item = propertiesFrom.Where(x => x.Name.Equals(propertiesTo[i].Name) && x.GetValue(source) != null).FirstOrDefault();

                if(item != null)
                {
                    propertiesTo[i].SetValue(instance, item.GetValue(source));
                }
            }

            return instance;
        }
    }
}
