using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace EducationApp.BusinessLayer.Helpers
{
    public static class CustomIQueryable
    {
        public static Expression<Func<TEntity, object>> GetExpression<TEntity>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(TEntity), propertyName);
            Expression conversion = Expression.Convert(Expression.Property(parameter, propertyName), typeof(TEntity));
            return Expression.Lambda<Func<TEntity, object>>(conversion, parameter);
        }
        public static Func<TEntity, object> GetFunc<TEntity>(string propertyName)
        {
            return GetExpression<TEntity>(propertyName).Compile();
        }
        public static IOrderedQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> entities, string propertyName)
        {
            return entities.OrderBy(GetExpression<TEntity>(propertyName));
        }
    }
}
