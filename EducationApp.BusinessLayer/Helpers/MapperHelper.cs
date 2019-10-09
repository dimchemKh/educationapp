using EducationApp.BusinessLayer.Helpers.Interfaces;
using System;
using System.Linq;

namespace EducationApp.BusinessLayer.Helpers
{
    public class MapperHelper : IMapperHelper
    {
        public MapTo MapToModelItem<MapFrom, MapTo>(MapFrom source, MapTo destination)
        {
            var propertiesFrom = typeof(MapFrom).GetProperties();
            var propertiesTo = typeof(MapTo).GetProperties();

            var instance = (MapTo)Activator.CreateInstance(typeof(MapTo));
            
            for (int i = 0; i < propertiesTo.Length; i++)
            {
                var item = propertiesFrom.Where(x => x.Name == propertiesTo[i].Name && x.GetValue(source) != null).FirstOrDefault();
                if(item != null)
                {
                    propertiesTo[i].SetValue(instance, item.GetValue(source));
                }
            }

            return instance;
        }
        public MapTo MapToEntity<MapFrom, MapTo>(MapFrom source, MapTo destination)
        {
            var propertiesFrom = typeof(MapFrom).GetProperties();
            var propertiesTo = typeof(MapTo).GetProperties();

            for (int i = 0; i < propertiesTo.Length; i++)
            {
                var item = propertiesFrom.Where(x => x.Name == propertiesTo[i].Name && x.GetValue(source) != null).FirstOrDefault();
                if (item != null)
                {
                    propertiesTo[i].SetValue(destination, item.GetValue(source));
                }
            }

            return destination;
        }
        //public ICollection<MapTo> MapEntitiesToModel<MapFrom, MapTo>(IEnumerable<MapFrom> entities, ICollection<MapTo> itemsList)
        //{
        //    var listPropertiesFrom = typeof(MapFrom).GetProperties();
        //    var listPropertiesTo = typeof(MapTo).GetProperties(); 

        //    foreach (var entity in entities)
        //    {
        //        var instance = (MapTo)Activator.CreateInstance(typeof(MapTo));
                     
        //        foreach(var item in listPropertiesTo)
        //        {
        //            var property = listPropertiesFrom.Where(x => x.Name == item.Name).First();


        //        }
                

        //        //for (int i = 0; i < listPropertiesFrom.Length; i++)
        //        //{
        //        //    var value = listPropertiesTo[i].GetValue(entity);
        //        //    listPropertiesFrom[i].SetValue(instance, value);
        //        //}


        //        itemsList.Add(instance);
        //    }
        //    return itemsList;
        //}
    }
}
