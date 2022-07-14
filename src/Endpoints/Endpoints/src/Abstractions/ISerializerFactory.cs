using Quandt.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quandt.Endpoints
{
    internal interface ISerializerFactory
    {
        bool SupportsContentType(string contentType);
        bool SupportsContentTypes(out string contentType, params string[] contentTypes);
        ISerializer GetSerializer(string contentType);
    }
}
