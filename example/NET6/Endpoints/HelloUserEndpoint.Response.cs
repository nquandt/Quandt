using Microsoft.AspNetCore.Mvc;
using System.Runtime.Serialization;

namespace WebProject1.Endpoints.HelloUserEndpointModels
{
    [DataContract]
    public class HelloUserEndpointResponse
    {
        [FromQuery]
        [DataMember(Name = "_user")]
        public string User { get; set; } = "";

        public string Password { get; set; } = "";

        [FromQuery]
        public string Setting { get; set; } = "";
    }
}
