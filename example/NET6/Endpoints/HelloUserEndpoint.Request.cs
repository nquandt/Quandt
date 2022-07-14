using Microsoft.AspNetCore.Mvc;
using System.Runtime.Serialization;

namespace WebProject1.Endpoints.HelloUserEndpointModels
{
    [DataContract]
    public class HelloUserEndpointRequest
    {
        [FromRoute]
        [DataMember(Name = "_user")]
        public string User { get; set; } = "";

        [FromRoute]
        public string Action { get; set; } = "";

        public string Password { get; set; } = "";

        [FromQuery]
        public string Setting { get; set; } = "";
    }
}
