using EducationApp.BusinessLayer.Models.Filters.Base;
using EducationApp.DataAccessLayer.Repository.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Helpers.Interfaces
{
    public interface IMapperHelper
    {
        MapTo Map<MapFrom, MapTo>(MapFrom source, MapTo destination);
        ICollection<MapTo> MapEntitiesToModel<MapFrom, MapTo>(IEnumerable<MapFrom> entities, ICollection<MapTo> itemsList);
    }
}
