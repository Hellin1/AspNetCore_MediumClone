using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediumClone.Common.ResponseObjects
{
    public class Response<T> : Response, IResponse<T>
    {
        public T Data { get; set; }

        public List<CustomValidationError> ValidationErrors { get; set; }

        // data bulunamadı
        public Response(ResponseType responseType, string message) : base(responseType, message)
        {

        }

        public Response(ResponseType responseType, T data) : base(responseType)
        {
            Data = data;
        }

        // validation errors
        public Response(T data, List<CustomValidationError> errors) : base(ResponseType.ValidationError)
        {
            ValidationErrors = errors;
            Data = data;
        }
    }
}
