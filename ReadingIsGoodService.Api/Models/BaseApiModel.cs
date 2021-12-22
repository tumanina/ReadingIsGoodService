using System.Collections.Generic;
using System.Linq;

namespace ReadingIsGoodService.Api.Models
{
    public class BaseApiModel
    {
        public bool Success
        {
            get
            {
                return Errors == null || !Errors.Any();
            }
        }

        public List<string> Errors { get; set; } = new();
        public List<string> Warnings { get; set; } = new();
    }

    public class BaseApiDataModel<T> : BaseApiModel
    {
        public T Data { get; set; }
    }
}
