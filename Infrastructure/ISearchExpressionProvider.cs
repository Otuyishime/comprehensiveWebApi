using System;
using System.Linq.Expressions;

namespace testWebAPI.Infrastructure
{
    public interface ISearchExpressionProvider
    {
        ConstantExpression GetValue(string input);

        Expression GetComparison(MemberExpression left, string op, ConstantExpression right);
    }
}
