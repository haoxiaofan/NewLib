using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq.Expressions;
using NewLibCore.Data.SQL.Builder;
using NewLibCore.Data.SQL.Mapper.Config;
using NewLibCore.Data.SQL.Mapper.EntityExtension;
using NewLibCore.Data.SQL.Mapper.Execute;
using NewLibCore.Data.SQL.Mapper.Translation;
using NewLibCore.Validate;

namespace NewLibCore.Data.SQL.Mapper.OperationProvider.Imp
{
    internal class SearchMapper<TModel> : ISearchMapper<TModel> where TModel : EntityBase, new()
    {
        private readonly ExecutionCore _executionCore;
        private readonly ExpressionSegment _expressionSegment;

        public SearchMapper()
        {
            _executionCore = new ExecutionCore();
            _expressionSegment = new ExpressionSegment();
        }

        public Boolean Exist()
        {
            return Count() > 0;
        }

        public Int32 Count()
        {
            return Watch<Int32>(() =>
            {
                Select(s => "COUNT(*)");
                var executeResult = InternalExecuteSql(ExecuteType.SELECT_SINGLE);
                Int32.TryParse(executeResult.Value.ToString(), out var count);
                return count;
            });
        }

        public TModel FirstOrDefault()
        {
            return Watch<TModel>(() =>
            {
                var executeResult = InternalExecuteSql(ExecuteType.SELECT);
                var dataTable = executeResult.Value as DataTable;
                return dataTable.ToSingle<TModel>();
            });
        }

        public List<TModel> ToList()
        {
            return Watch<List<TModel>>(() =>
            {
                var executeResult = InternalExecuteSql(ExecuteType.SELECT);
                var dataTable = executeResult.Value as DataTable;
                return dataTable.ToList<TModel>();
            });
        }

        public ISearchMapper<TModel> Select<T>(Expression<Func<TModel, T, dynamic>> fields = null) where T : EntityBase, new()
        {
            if (fields != null)
            {
                _expressionSegment.Add(fields);
            }

            return this;
        }

        public ISearchMapper<TModel> Select(Expression<Func<TModel, dynamic>> fields = null)
        {
            if (fields != null)
            {
                _expressionSegment.Add(fields);
            }

            return this;
        }

        public ISearchMapper<TModel> Where(Expression<Func<TModel, Boolean>> expression = null)
        {
            return Where<TModel>(expression);
        }

        public ISearchMapper<TModel> Where<T>(Expression<Func<T, Boolean>> expression = null) where T : EntityBase, new()
        {
            if (expression != null)
            {
                _expressionSegment.Add(expression);
            }

            return this;
        }

        public ISearchMapper<TModel> Where<T>(Expression<Func<TModel, T, Boolean>> expression = null) where T : EntityBase, new()
        {
            if (expression != null)
            {
                _expressionSegment.Add(expression);
            }

            return this;
        }

        public ISearchMapper<TModel> Page(Int32 pageIndex, Int32 pageSize)
        {
            _expressionSegment.AddPage(pageIndex, pageSize);
            return this;
        }

        public ISearchMapper<TModel> LeftJoin<TRight>(Expression<Func<TModel, TRight, Boolean>> expression) where TRight : EntityBase, new()
        {
            Parameter.Validate(expression);
            _expressionSegment.Add(expression, JoinType.LEFT);

            return this;
        }

        public ISearchMapper<TModel> RightJoin<TRight>(Expression<Func<TModel, TRight, Boolean>> expression) where TRight : EntityBase, new()
        {
            Parameter.Validate(expression);
            _expressionSegment.Add(expression, JoinType.RIGHT);

            return this;
        }

        public ISearchMapper<TModel> InnerJoin<TRight>(Expression<Func<TModel, TRight, Boolean>> expression) where TRight : EntityBase, new()
        {
            Parameter.Validate(expression);
            _expressionSegment.Add(expression, JoinType.INNER);

            return this;
        }

        public ISearchMapper<TModel> LeftJoin<TLeft, TRight>(Expression<Func<TLeft, TRight, Boolean>> expression)
          where TLeft : EntityBase, new()
          where TRight : EntityBase, new()
        {
            Parameter.Validate(expression);
            _expressionSegment.Add(expression, JoinType.LEFT);

            return this;
        }

        public ISearchMapper<TModel> RightJoin<TLeft, TRight>(Expression<Func<TLeft, TRight, Boolean>> expression)
            where TLeft : EntityBase, new()
            where TRight : EntityBase, new()
        {
            Parameter.Validate(expression);
            _expressionSegment.Add(expression, JoinType.RIGHT);

            return this;
        }

        public ISearchMapper<TModel> InnerJoin<TLeft, TRight>(Expression<Func<TLeft, TRight, Boolean>> expression)
            where TLeft : EntityBase, new()
            where TRight : EntityBase, new()
        {
            Parameter.Validate(expression);
            _expressionSegment.Add(expression, JoinType.INNER);

            return this;
        }

        public ISearchMapper<TModel> OrderBy<TOrder, TKey>(Expression<Func<TOrder, TKey>> order, OrderByType orderBy = OrderByType.DESC) where TOrder : EntityBase, new()
        {
            Parameter.Validate(order);
            _expressionSegment.AddOrderBy(order, orderBy);

            return this;
        }

        private RawExecuteResult InternalExecuteSql(ExecuteType executeType)
        {
            _expressionSegment.ExecuteType = executeType;
            IBuilder<TModel> builder = new SelectBuilder<TModel>(_expressionSegment);

            var translationResult = builder.CreateTranslateResult();
            var executeResult = GetResultFormCache(translationResult);
            if (executeResult == null)
            {
                executeResult = _executionCore.Execute(translationResult);
                SetCacheFormResult(translationResult, executeResult);
            }
            return executeResult;
        }

        private static void SetCacheFormResult(TranslateResult translationResult, RawExecuteResult executeResult)
        {
            if (MapperConfig.DatabaseConfig.Cache != null)
            {
                MapperConfig.DatabaseConfig.Cache.Add(translationResult.PrepareCacheKey(), executeResult);
            }
        }

        private static RawExecuteResult GetResultFormCache(TranslateResult translationResult)
        {
            if (MapperConfig.DatabaseConfig.Cache != null)
            {
                var cacheResult = MapperConfig.DatabaseConfig.Cache.Get(translationResult.PrepareCacheKey());
                if (cacheResult != null)
                {
                    return (RawExecuteResult)cacheResult;
                }
            }
            return default;
        }

        private T Watch<T>(Func<Object> func)
        {
            var sw = new Stopwatch();
            sw.Start();

            var returnValue = (T)func();

            sw.Stop();
            MapperConfig.DatabaseConfig.Logger.Info($@"共花费{Math.Round(sw.Elapsed.TotalSeconds, 2)}s");

            return returnValue;
        }
    }
}