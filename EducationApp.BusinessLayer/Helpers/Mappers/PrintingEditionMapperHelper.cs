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
        public static PrintingEditionModelItem MapToModel(this PrintingEditionDataModel source, Enums.Currency currency)
        {
            var instance = new PrintingEditionModelItem();

            instance.Id = source.Id;
            instance.Price = source.Price;
            instance.PrintingEditionType = source.PrintingEditionType;
            instance.Title = source.Title;
            instance.Currency = currency;
            instance.Description = source.Description;
            instance.Authors = source.Authors.Select(x => new AuthorModelItem { Name = x.Name }).ToList();
            
            return instance;
        }
        public static PrintingEditionDataModel MapToData(this PrintingEditionModelItem source)
        {
            var instance = new PrintingEditionDataModel();

            instance.Id = source.Id;
            instance.Price = source.Price;
            instance.PrintingEditionType = source.PrintingEditionType;
            instance.Title = source.Title;
            instance.Description = source.Description;

            instance.Authors = source.Authors.Select(x => new AuthorDataModel { Id = x.Id, Name = x.Name, PrintingEditionTitles = x.PrintingEditionTitles }).ToList();

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
