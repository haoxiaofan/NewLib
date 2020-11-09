﻿using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;
using NewLibCore.Storage.SQL.EMapper;
using NewLibCore.Storage.SQL.Extension;
using NewLibCore.Storage.SQL.ProcessorFactory;
using NewLibCore.Storage.SQL.Store;
using NewLibCore.Validate;

namespace NewLibCore.Storage.SQL
{
    /// <summary>
    /// 将对应的操作翻译为sql并执行
    /// </summary>
    public sealed class EntityMapper : IDisposable
    {
        private readonly IServiceProvider _provider;

        private readonly MapperDbContextBase _contextBase;
        private EntityMapper()
        {
            _provider = new EntityMapperConfig().InitDependency();
            _contextBase = _provider.GetService<MapperDbContextBase>();
        }

        static EntityMapper()
        {
        }

        /// <summary>
        /// 初始化EntityMapper类的新实例
        /// </summary>
        /// <returns></returns>
        public static EntityMapper CreateMapper()
        {
            return new EntityMapper();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">要新增的对象</param>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public TModel Add<TModel>(TModel model) where TModel : EntityBase, new()
        {
            Check.IfNullOrZero(model);

            return RunDiagnosis.Watch(() =>
            {
                var store = new ExpressionStore();
                store.AddModel(model);

                var processor = FindProcessor(nameof(InsertProcessor));
                model.Id = processor.Process(store).GetModifyRowCount();
                return model;
            });
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">要修改的对象</param>
        /// <param name="expression">查询条件</param>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public Boolean Update<TModel>(TModel model, Expression<Func<TModel, Boolean>> expression) where TModel : EntityBase, new()
        {
            Check.IfNullOrZero(model);
            Check.IfNullOrZero(expression);

            return RunDiagnosis.Watch(() =>
            {
                var store = new ExpressionStore();
                store.AddWhere(expression);
                store.AddModel(model);
                var processor = FindProcessor(nameof(UpdateProcessor));
                return processor.Process(store).GetModifyRowCount() > 0;
            });
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public QueryWrapper<TModel> Query<TModel>() where TModel : EntityBase, new()
        {
            var expressionStore = new ExpressionStore();
            expressionStore.AddFrom<TModel>();

            var processor = FindProcessor(nameof(QueryProcessor));
            return new QueryWrapper<TModel>(expressionStore, processor);
        }

        /// <summary>
        /// 执行原生的SQL语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">实体参数</param>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public SqlExecuteResultConvert SqlQuery(String sql, params MapperParameter[] parameters)
        {
            Check.IfNullOrZero(sql);

            return RunDiagnosis.Watch(() =>
            {
                var store = new ExpressionStore();
                store.AddDirectSql(sql, parameters);
                var processor = FindProcessor(nameof(RawSqlProcessor));
                return processor.Process(store);
            });
        }

        public void Commit()
        {
            _contextBase.Commit();
        }

        public void Rollback()
        {
            _contextBase.Rollback();
        }

        public void OpenTransaction()
        {
            _contextBase.UseTransaction = true;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            (_provider as ServiceProvider).Dispose();
        }

        private Processor FindProcessor(String target)
        {
            Check.IfNullOrZero(target);
            var result = _provider.GetServices<Processor>().FirstOrDefault(w => w.CurrentId == target);
            if (result != null)
            {
                return result;
            }
            throw new ArgumentException($@"没有找到{target}所注册的实现类");
        }
    }
}
