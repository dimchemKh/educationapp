using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models
{
    public class ResponseModel<T>
    {
        public bool IsSucceeded { get; set; }
        public T Data { get; set; }
        public IList<string> Errors = new List<string>();

        public ResponseModel()
        {
            IsSucceeded = false;
        }

        public ResponseModel(List<string> errors)
        {
            IsSucceeded = false;
            Errors = errors;
        }
    }
}
