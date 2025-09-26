using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using Dapper;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

using MoreLinq;

using Newtonsoft.Json;

using Abc1.QueryBuilder.Interfaces;

namespace Abc1.QueryBuilder
{
    public class FolderQueryBuilder : QueryBuilder<Folder, IFolderQueryBuilder>, IFolderQueryBuilder
    {
        private static string _noMediaQuery;

        public FolderQueryBuilder(FoldersDbContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public async Task<List<TOutput>> GetPagedFoldersViewAsync<TOutput>(ViewSearchCriteriaDataModel searchCriteria)
            where TOutput : class
        {
            string sql = $"select * from abc.crm(" +
                         $"{searchCriteria.PageSize}, {searchCriteria.PageNumber}, '{searchCriteria.From}', '{searchCriteria.To}');";

            return await ExecuteFunction<TOutput>(sql);
        }

        #region Private methods

        private async Task<List<TOutput>> ExecuteFunction<TOutput>(string sql, object param = null)
        {
            DbConnection conn = Context.Database.GetDbConnection();
            IEnumerable<TOutput> results = await conn.QueryAsync<TOutput>(sql, param);

            return results.ToList();
        }

        #endregion

    }
}
