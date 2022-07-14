using Microsoft.Extensions.DependencyInjection;
using Quandt.Abstractions;
using Quandt.Endpoints.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Quandt.Endpoints
{
    internal class DefaultSerializerFactory : ISerializerFactory
    {
        private Dictionary<string, ISerializer> _serializers;
        public DefaultSerializerFactory(IServiceProvider serviceProvider)
        {
            var serializers = serviceProvider.GetServices<ISerializer>();

            _serializers = new Dictionary<string, ISerializer>();

            foreach(ISerializer serializer in serializers)
            {
                var type = serializer.GetType();
                var att = (SupportsAdditionalContentTypesAttribute)type.GetCustomAttributes(typeof(SupportsAdditionalContentTypesAttribute), true).Single();

                _serializers.Add(serializer.ContentType, serializer);

                foreach(var contentType in att.ContentTypes)
                {
                    if (!_serializers.ContainsKey(contentType))
                    {
                        _serializers.Add(contentType, serializer);
                    }
                }

                var att2 = (SupportsQueryFormatsAttribute)type.GetCustomAttributes(typeof(SupportsQueryFormatsAttribute), true).Single();

                foreach (var formatType in att2.FormatTypes)
                {
                    if (!_serializers.ContainsKey(formatType))
                    {
                        _serializers.Add(formatType, serializer);
                    }
                }                               
            }
            _serializers.Add("__default", serializers.First());
        }

        public ISerializer GetSerializer(string contentType)
        {
            if (_serializers.ContainsKey(contentType))
            {
                return _serializers[contentType];
            }

            return _serializers["__default"];
        }

        public bool SupportsContentType(string contentType)
        {
            return _serializers.ContainsKey(contentType);
        }

        public bool SupportsContentTypes(out string contentType, params string[] contentTypes)
        {            
            foreach(var ct in contentTypes)
            {
                if (_serializers.ContainsKey(ct))
                {
                    contentType = ct;
                    return true;
                }
            }

            contentType = "";
            return false;
        }
    }
}
