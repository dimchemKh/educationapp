using EducationApp.BusinessLayer.Helpers.Interfaces;
using EducationApp.DataAccessLayer.Common.Constants;
using EducationApp.DataAccessLayer.Entities.Enums;
using System.Collections.Generic;
using System.Linq;

namespace EducationApp.BusinessLayer.Helpers
{
    public class SorterHelper<T> : ISorterHelper<T>
    {
        public IQueryable<T> Sorting(Enums.SortType sortType, IQueryable<T> entitesList)
        {
            IQueryable<T> responseList = null;
            var filterList = new Dictionary<Enums.SortType, string>()
            {
                { Enums.SortType.Id, Constants.SortProperties.Id },
                { Enums.SortType.Price, Constants.SortProperties.Price },
                { Enums.SortType.Type, Constants.SortProperties.Category },
            };
            foreach (var type in filterList)
            {
                if(type.Key == sortType)
                {
                    responseList = entitesList.OrderBy(type.Value);
                }
            }
            return responseList;
        }        
    }
}
