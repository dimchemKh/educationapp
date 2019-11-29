using EducationApp.BusinessLogic.Models.PrintingEditions;
using EducationApp.DataAccessLayer.Models.PrintingEditions;
using System.Linq;
using EducationApp.BusinessLogic.Models.Authors;
using EducationApp.DataAccessLayer.Models.Authors;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Entities.Enums;

namespace EducationApp.BusinessLogic.Helpers.Mappers
{
    public static class PrintingEditionMapperHelper
    {
        public static PrintingEditionModelItem MapToModel(this PrintingEditionDataModel source, Enums.Currency currency = Enums.Currency.USD)
        {
            var instance = new PrintingEditionModelItem();
            instance.Id = source.Id;
            instance.Price = source.Price;
            instance.PrintingEditionType = source.PrintingEditionType;
            instance.Title = source.Title;
            instance.Currency = currency;
            instance.Description = source.Description;
            instance.Price = source.Price;

            instance.Authors = source.Authors.Select(x => new AuthorModelItem
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
            
            return instance;
        }
        public static PrintingEdition MapToEntity(this PrintingEditionModelItem source, PrintingEdition instance)
        {
            instance.Id = source.Id;
            instance.Price = source.Price;
            instance.PrintingEditionType = source.PrintingEditionType;
            instance.Title = source.Title;
            instance.Description = source.Description;

            return instance;
        }
    }
}
