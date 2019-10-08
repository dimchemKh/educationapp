using EducationApp.BusinessLayer.Helpers.Interfaces;
using EducationApp.BusinessLayer.Models.Filters.Base;
using EducationApp.DataAccessLayer.Repository.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;

namespace EducationApp.BusinessLayer.Helpers
{
    public class MapperHelper/*<MapFrom, MapTo>*/ : IMapperHelper
    {
        public MapTo Map<MapFrom, MapTo>(MapFrom source, MapTo destination)
        {
            var propertiesFrom = typeof(MapFrom).GetProperties();
            var propertiesTo = typeof(MapTo).GetProperties();

            for (int i = 0; i < propertiesTo.Length; i++)
            {
                var item = propertiesFrom.Where(x => x.Name == propertiesTo[i].Name).First();
                propertiesTo[i].SetValue(destination, item.GetValue(source));
            }
            return destination;
        }
        public ICollection<MapTo> MapEntitiesToModel<MapFrom, MapTo>(IEnumerable<MapFrom> entities, ICollection<MapTo> itemsList)
        {
            var listPropertiesFrom = typeof(MapFrom).GetProperties();
            var listPropertiesTo = typeof(MapTo).GetProperties(); 

            foreach (var entity in entities)
            {
                var instance = (MapTo)Activator.CreateInstance(typeof(MapTo));

                foreach(var item in listPropertiesTo)
                {
                    var property = listPropertiesFrom.Where(x => x.Name == item.Name).First();


                }
                

                //for (int i = 0; i < listPropertiesFrom.Length; i++)
                //{
                //    var value = listPropertiesTo[i].GetValue(entity);
                //    listPropertiesFrom[i].SetValue(instance, value);
                //}


                itemsList.Add(instance);
            }
            return itemsList;
        }
    }
}
