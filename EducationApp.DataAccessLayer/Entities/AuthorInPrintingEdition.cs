using Dapper.Contrib.Extensions;
using EducationApp.DataAccessLayer.Entities.Base;

namespace EducationApp.DataAccessLayer.Entities
{
    [Table("AuthorInPrintingEditions")]
    public class AuthorInPrintingEdition : BaseEntity
    {
        public long AuthorId { get; set; }
        [Write(false)]
        public Author Author { get; set; }
        public long PrintingEditionId { get; set; }
        [Write(false)]
        public PrintingEdition PrintingEdition { get; set; }
    }
}