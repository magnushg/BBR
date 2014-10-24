using System.Linq;

namespace BouvetCodeCamp.Filters
{
    using System.Net;
    using System.Web.Http;
    using System.Web.Http.Description;

    using Swashbuckle.Swagger;

    public class AddAuthorizationRequiredResponseCodes : IOperationFilter
    {
        public void Apply(Operation operation, DataTypeRegistry dataTypeRegistry, ApiDescription apiDescription)
        {
            if (apiDescription.ActionDescriptor.ControllerDescriptor.GetFilters().OfType<AuthorizeAttribute>().Any())
            {
                operation.ResponseMessages.Add(new ResponseMessage
                {
                    Code = (int)HttpStatusCode.Unauthorized,
                    Message = "Authentication required"
                });
            }
        }
    }
}