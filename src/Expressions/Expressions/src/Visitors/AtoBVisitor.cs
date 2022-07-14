// https://stackoverflow.com/a/10643755
//possible TODO
//Going to Hold onto this for the _mappings idea, and maybe PORT to the TypeChangeVisitor


//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Quandt.Expressions.Visitors
//{
//    sealed class AToBConverter<TA, TB> : ExpressionVisitor
//    {
//        private readonly Dictionary<ParameterExpression, ParameterExpression> _parameters = new Dictionary<ParameterExpression, ParameterExpression>();
//        //private readonly Dictionary<MemberInfo, LambdaExpression> _mappings;

//        protected override Expression VisitParameter(ParameterExpression node)
//        {
//            if (node.Type == typeof(TA))
//            {
//                ParameterExpression parameter;
//                if (!this._parameters.TryGetValue(node, out parameter))
//                {
//                    this._parameters.Add(node, parameter = Expression.Parameter(typeof(TB), node.Name));
//                }
//                return parameter;
//            }
//            return node;
//        }

//        protected override Expression VisitMember(MemberExpression node)
//        {
//            if (node.Expression == null || node.Expression.Type != typeof(TA))
//            {
//                return base.VisitMember(node);
//            }
//            Expression expression = this.Visit(node.Expression);
//            if (expression.Type != typeof(TB))
//            {
//                throw new Exception("Whoops");
//            }
//            //LambdaExpression lambdaExpression;
//            //if (this._mappings.TryGetValue(node.Member, out lambdaExpression))
//            //{
//            //    return new SimpleExpressionReplacer(lambdaExpression.Parameters.Single(), expression).Visit(lambdaExpression.Body);
//            //}
//            return Expression.Property(
//                expression,
//                node.Member.Name
//            );
//        }

//        protected override Expression VisitLambda<T>(Expression<T> node)
//        {
//            return Expression.Lambda(
//                this.Visit(node.Body),
//                node.Parameters.Select(this.Visit).Cast<ParameterExpression>()
//            );
//        }

//        public AToBConverter()
//        {

//        }

//        //public AToBConverter(Dictionary<MemberInfo, LambdaExpression> mappings)
//        //{
//        //    this._mappings = mappings;
//        //}
//    }

//    sealed class SimpleExpressionReplacer : ExpressionVisitor
//    {
//        private readonly Expression _replacement;
//        private readonly Expression _toFind;

//        public override Expression Visit(Expression node)
//        {
//            return node == this._toFind ? this._replacement : base.Visit(node);
//        }

//        public SimpleExpressionReplacer(Expression toFind, Expression replacement)
//        {
//            this._toFind = toFind;
//            this._replacement = replacement;
//        }
//    }
//}
