using Dapper.Contrib.Extensions;

namespace EducationApp.DataAccessLayer.Entities
{
    [Table("AuthorInPrintingEditions")]
    public class AuthorInPrintingEdition
    {
        public long Id { get; set; }
        public long AuthorId { get; set; }
        [Write(false)]
        public Author Author { get; set; }
        public long PrintingEditionId { get; set; }
        [Write(false)]
        public PrintingEdition PrintingEdition { get; set; }
    }
}