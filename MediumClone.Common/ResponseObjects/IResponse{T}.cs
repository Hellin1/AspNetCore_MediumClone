using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediumClone.Common.ResponseObjects
{
    public interface IResponse<T>
    {
        T Data { get; set; }

        List<CustomValidationError> ValidationErrors { get; set; }
    }
}
