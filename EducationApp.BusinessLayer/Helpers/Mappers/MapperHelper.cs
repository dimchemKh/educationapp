using EducationApp.BusinessLayer.Helpers.Mappers.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EducationApp.BusinessLayer.Helpers.Mappers
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
            // try create generic list properties mapper
            var queryFrom = propertiesFrom.Where(x => x.PropertyType.IsAbstract && x.PropertyType.IsGenericType).FirstOrDefault();
            var queryTo = propertiesTo.Where(x => x.PropertyType.IsAbstract && x.PropertyType.IsGenericType).FirstOrDefault();
            var arrayTypesFrom = queryFrom.PropertyType.GetGenericArguments();
            var arrayTypesTo = queryTo.PropertyType.GetGenericArguments();

            foreach (var item in arrayTypesFrom)
            {
                var props = item.GetType().GetProperties();
            }


            return instance;
        }
    }
}
