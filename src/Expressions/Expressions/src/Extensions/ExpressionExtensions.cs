using Quandt.Expressions.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Quandt.Expressions.Extensions
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<TTo, bool>> Translate<TFrom, TTo>(this Expression<Func<TFrom, bool>> expression) where TTo : TFrom
        {
            if (!typeof(TFrom).IsAssignableFrom(typeof(TTo)))
                throw new Exception(string.Format("{0} is not assignable from {1}.", typeof(TFrom), typeof(TTo)));
            var param = Expression.Parameter(typeof(TTo), expression.Parameters[0].Name);
            var subst = new Dictionary<Expression, Expression> { { expression.Parameters[0], param } };
            var visitor = new TypeChangeVisitor(typeof(TFrom), typeof(TTo), subst);

            return Expression.Lambda<Func<TTo, bool>>(visitor.Visit(expression.Body), param);
        }        
    }    
}
