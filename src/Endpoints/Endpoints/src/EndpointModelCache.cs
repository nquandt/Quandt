using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;
#if NET6_0
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
#elif NETSTANDARD2_0
using Microsoft.AspNetCore.Http;
#endif
using System.Runtime.Serialization;

namespace Quandt.Endpoints
{
    internal class EndpointModelCache<T>
    {
#if NET6_0
        private Dictionary<string, Action<T, string>>? _routeCache;
        private Dictionary<string, Action<T, string>>? _queryCache;
#elif NETSTANDARD2_0
        private Dictionary<string, PropertyInfo> _routeCache;
        private Dictionary<string, PropertyInfo> _queryCache;
#endif
        public EndpointModelCache()
        {
            var props = typeof(T).GetProperties();

            var route = props.Where(x => x.GetCustomAttributes().Any(y => typeof(FromRouteAttribute).IsAssignableFrom(y.GetType())));
            var query = props.Where(x => x.GetCustomAttributes().Any(y => typeof(FromQueryAttribute).IsAssignableFrom(y.GetType())));

            _isRouted = route.Any();
            _isQuerys = query.Any();

            if (_isRouted) {
                _routeCache = route.ToDictionary(x => x.Name, x => CreateSetter<T,string>(x));
            }

            if (_isQuerys)
            {
                _queryCache = query.ToDictionary(x => x.Name.ToUpper(), x => CreateSetter<T, string>(x));
                var dataMembers = query.Where(x => x.GetCustomAttributes().Any(y => typeof(DataMemberAttribute).IsAssignableFrom(y.GetType())));
                if (dataMembers.Any())
                {
                    foreach(var member in dataMembers)
                    {
                        var attObj = member.GetCustomAttribute(typeof(DataMemberAttribute), true);
                        if (attObj != null && attObj is DataMemberAttribute att)
                        {                            
                            if (att.Name != null)
                            {
                                _queryCache[att.Name.ToUpper()] = CreateSetter<T, string>(member);
                            }
                        }
                    }
                }
            }
        }

        public static Action<TEntity, TProperty> CreateSetter<TEntity, TProperty>(PropertyInfo propertyInfo)
        {
            ParameterExpression instance = Expression.Parameter(typeof(TEntity), "instance");
            ParameterExpression parameter = Expression.Parameter(typeof(TProperty), "param");

            var body = Expression.Call(instance, propertyInfo.GetSetMethod(), parameter);
            var parameters = new ParameterExpression[] { instance, parameter };

            var lambda = Expression.Lambda<Action<TEntity, TProperty>>(body, parameters);
            
            return lambda.Compile();
        }

        private readonly bool _isRouted = false;
        private readonly bool _isQuerys = false;

        public void SetProperties(HttpContext context, T model)
        {
            SetPropertiesFromRouteValues(context, model);
            SetPropertiesFromQueryParams(context, model);
        }

        private void SetPropertiesFromRouteValues(HttpContext context, T model)
        {
            if (_isRouted && _routeCache != null)
            {
                var dict = context.Request.RouteValues;
                var keys = dict.Keys;
                foreach (var key in keys)
                {
                    if (_routeCache.TryGetValue(key, out var setProperty))
                    {
                        setProperty(model, (string)dict[key]);                        
                    }
                }
            }
        }

        private void SetPropertiesFromQueryParams(HttpContext context, T model)
        {
            if (_isQuerys && _queryCache != null)
            {
                var query = context.Request.Query;

                var keys = query.Keys;
                foreach (var key in keys)
                {
                    if (_queryCache.TryGetValue(key.ToUpper(), out var setProperty))
                    {
                        setProperty(model, query[key].ToString());
                    }
                }
            }
        }
    }
}
