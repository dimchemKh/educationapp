using EducationApp.DataAccessLayer.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Common
{
    public class EquailtyComparerPrintingEdition : IEqualityComparer<PrintingEdition>
    {
        public bool Equals(PrintingEdition x, PrintingEdition y)
        {
            if(x == null || y == null)
            {
                return false;
            }
            if(x == null && y == null)
            {
                return true;
            }
            if(x.PrintingEditionType == y.PrintingEditionType && x.Title == y.Title && x.AuthorInPrintingEditions == y.AuthorInPrintingEditions)
            {
                return true;
            }
            return false;
        }
        public int GetHashCode(PrintingEdition obj)
        {
            long hashAuthors = 0;
            foreach (var item in obj.AuthorInPrintingEditions)
            {
                hashAuthors += item.AuthorId;
            }
            long hash = (long)obj.PrintingEditionType ^ Int32.Parse(obj.Title) ^ hashAuthors;
            return hash.GetHashCode();
        }
    }
}
