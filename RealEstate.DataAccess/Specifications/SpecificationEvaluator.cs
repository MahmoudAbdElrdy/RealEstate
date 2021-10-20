using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Microsoft.EntityFrameworkCore;

namespace RealEstate.Specifications
{
    public class SpecificationEvaluator<TEntity> where TEntity : class
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> query, BaseSpecifications<TEntity> specifications)
        {
            // Do not apply anything if specifications is null
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
       public static TEntity GetFristQuery(IQueryable<TEntity> query, BaseSpecifications<TEntity> specifications)
        {
            // Do not apply anything if specifications is null
            TEntity entity = null;
            if (specifications == null)
            {
                return entity;
            }

            // Modify the IQueryable
            // Apply filter conditions
            if (specifications.FilterCondition != null)
            {
                entity = query.FirstOrDefault();
            }

            return entity;
        }


    }
}
