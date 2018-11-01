using System;
namespace testWebAPI.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SearchableAttribute : Attribute
    {
        public ISearchExpressionProvider ExpressionProvider { get; set; }
            = new DefaultSearchExpressionProvider();
    }
}
