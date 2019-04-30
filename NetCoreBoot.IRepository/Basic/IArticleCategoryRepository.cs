using NetCoreBoot.Core.Repository;
using NetCoreBoot.Entity;
using System;
using System.Threading.Tasks;

namespace NetCoreBoot.IRepository
{
    public partial interface IArticleCategoryRepository : IBaseRepository<ArticleCategory, Int32>
    {
    }
}