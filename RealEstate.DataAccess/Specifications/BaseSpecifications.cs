using Microsoft.EntityFrameworkCore;
using RealEstate.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RealEstate.Specifications
{
    // Generic Specifications , can be easily used for filter expressions
    // For additional expressions, class needs to be derived.
    public class BaseSpecifications<T> where T:class
    {
        private readonly List<Expression<Func<T, object>>> _includeCollection = new List<Expression<Func<T, object>>>();

        public BaseSpecifications()
        {
        }

        public BaseSpecifications(Expression<Func<T, bool>> filterCondition)
        {
            this.FilterCondition = filterCondition;
        }

        public bool IsSatisfiedBy(T entity)
        {
            Func<T, bool> predicate = this.FilterCondition.Compile();
            return predicate(entity);
        }

        public BaseSpecifications<T> And(BaseSpecifications<T> specification)
        {
            return new AndSpecification<T>(this, specification);
        }

        public BaseSpecifications<T> Or(BaseSpecifications<T> specification)
        {
            return new OrSpecification<T>(this, specification);
        }

        public virtual Expression<Func<T, bool>> FilterCondition { get; private set; }
        public Expression<Func<T, object>> OrderBy { get; private set; }
        public Expression<Func<T, object>> OrderByDescending { get; private set; }
        public List<Expression<Func<T, object>>> Includes
        {
            get
            {
                return _includeCollection;
            }
        }

        public Expression<Func<T, object>> GroupBy { get; private set; }

        public void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        public void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        public void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
        }

        public void SetFilterCondition(Expression<Func<T, bool>> filterExpression)
        {
            FilterCondition = filterExpression;
        }

        public void ApplyGroupBy(Expression<Func<T, object>> groupByExpression)
        {
            GroupBy = groupByExpression;
        }
      public  int page = 0;
      public  int pageSize = 10;

        /// <inheritdoc/>
        public bool isPagingEnabled  = false;
    }
}
public static class QuerySpecificationExtensions
{
    public static T SpecifyFirstOrDefault<T>(this IQueryable<T> query, BaseSpecifications<T> spec) where T : class 
    {
        // fetch a Queryable that includes all expression-based includes
        var queryableResultWithIncludes = spec.Includes
            .Aggregate(query,
                (current, include) => current.Include(include));

        // modify the IQueryable to include any string-based include statements
      
        // return the result of the query using the specification's criteria expression
        return queryableResultWithIncludes.Where(spec.FilterCondition).FirstOrDefault();
    }
    public static IQueryable<T> Specify<T>(this IQueryable<T> query, BaseSpecifications<T> spec) where T : class
    {
        // fetch a Queryable that includes all expression-based includes
        var queryableResultWithIncludes = spec.Includes
            .Aggregate(query,
                (current, include) => current.Include(include));

        // modify the IQueryable to include any string-based include statements
        var secondaryResult = spec.Includes
            .Aggregate(queryableResultWithIncludes,
                (current, include) => current.Include(include));

        // return the result of the query using the specification's criteria expression
        return secondaryResult.Where(spec.FilterCondition);
    }
    public static async Task<IQueryable<T>>  Pagtion<T>(this IQueryable<T> query, BaseSpecifications<T> specifications) where T : class
    {
        if (specifications == null)
        {
           
            return query;
        }

        // Modify the IQueryable
        // Apply filter conditions
        if (specifications.FilterCondition != null)
        {
            query = query.Where(specifications.FilterCondition);
        }

        // Includes
        if (specifications.Includes != null)
        {
            query = specifications.Includes
                      .Aggregate(query, (current, include) => current.Include(include));


        }
        // Apply ordering
        if (specifications.OrderBy != null)
        {
            query = query.OrderBy(specifications.OrderBy);
        }
        else if (specifications.OrderByDescending != null)
        {
            query = query.OrderByDescending(specifications.OrderByDescending);
        }

        // Apply GroupBy
        if (specifications.GroupBy != null)
        {
            query = query.GroupBy(specifications.GroupBy).SelectMany(x => x);
        }

        if (specifications.isPagingEnabled && specifications.page != 0 && specifications.pageSize != 0)
        {
            query = query.Skip((specifications.page - 1) * specifications.pageSize).Take(specifications.pageSize);
        }

        return query;
    }


}