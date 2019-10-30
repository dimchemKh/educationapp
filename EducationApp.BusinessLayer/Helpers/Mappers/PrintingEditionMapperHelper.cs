using System;
using System.Collections.Generic;
using System.Text;
using EducationApp.BusinessLayer.Models.PrintingEditions;
using EducationApp.DataAccessLayer.Models.PrintingEditions;
using EducationApp.BusinessLayer.Helpers.Mappers.Interfaces;
using System.Linq;
using EducationApp.BusinessLayer.Models.Authors;
using EducationApp.DataAccessLayer.Models.Authors;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Entities.Enums;

namespace EducationApp.BusinessLayer.Helpers.Mappers
{
    public static class PrintingEditionMapperHelper
    {
        public static PrintingEditionModelItem MapToModel(this PrintingEditionDataModel source, Enums.Currency currency = Enums.Currency.USD)
        {
            var instance = new PrintingEditionModelItem
            {
                Id = source.Id,
                Price = source.Price,
                PrintingEditionType = source.PrintingEditionType,
                Title = source.Title,
                Currency = currency,
                Description = source.Description
            };
            instance.Price = source.Price;
            instance.Authors = source.Authors.Select(x => new AuthorModelItem { Id = x.Id, Name = x.Name }).ToList();
            
            return instance;
        }
        public static PrintingEditionDataModel MapToData(this PrintingEditionModelItem source)
        {
            var instance = new PrintingEditionDataModel
            {
                Id = source.Id,
                Price = source.Price,
                PrintingEditionType = source.PrintingEditionType,
                Title = source.Title,
                Description = source.Description,

                Authors = source.Authors.Select(x => new AuthorDataModel { Id = x.Id, Name = x.Name, PrintingEditionTitles = x.PrintingEditionTitles }).ToList()
            };

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
