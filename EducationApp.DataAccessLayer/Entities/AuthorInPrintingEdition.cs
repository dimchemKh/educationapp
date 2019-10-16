using EducationApp.DataAccessLayer.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EducationApp.DataAccessLayer.Entities
{
    public class AuthorInPrintingEdition : BaseEntity
    {
        public long AuthorId { get; set; }
        public Author Author { get; set; }
        public long PrintingEditionId { get; set; }
        public PrintingEdition PrintingEdition { get; set; }
    }
}