﻿using System;
using System.Collections.Generic;
using System.Text;
using NewLibCore.Data.SQL.Mapper.Component.Cache;
using NewLibCore.Data.SQL.Mapper.Extension;
using NewLibCore.Validate;

namespace NewLibCore.Data.SQL.Mapper
{

    /// <summary>
    /// 执行Expression翻译后的sql语句
    /// </summary>
    internal sealed class ParserResult
    {
        private StringBuilder _originSql;

        private readonly IList<MapperParameter> _parameters;

        private readonly MapperDbContextBase _mapperDbContextBase;

        private readonly QueryCacheBase _queryCacheBase;

        /// <summary>
        /// 初始化一个ResultExecutor类的实例
        /// </summary>
        /// <param name="mapperDbContextBase"></param>
        public ParserResult(MapperDbContextBase mapperDbContextBase, QueryCacheBase queryCacheBase)
        {
            _originSql = new StringBuilder();
            _parameters = new List<MapperParameter>();

            _queryCacheBase = queryCacheBase;
            _mapperDbContextBase = mapperDbContextBase;
        }


        /// <summary>
        /// 追加一个sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="entityParameters"></param>
        /// <returns></returns>
        internal void Append(String sql)
        {
            Parameter.Validate(sql);
            _originSql.Append($@" {sql} ");
        }

        /// <summary>
        /// 追加一组参数
        /// </summary>
        /// <param name="parameters"></param>
        internal void Append(params MapperParameter[] parameters)
        {
            Parameter.Validate(parameters);

            foreach (var item in parameters)
            {
                _parameters.Add(item);
            }
        }

        internal void Append(String sql, params MapperParameter[] parameters)
        {
            Append(sql);
            Append(parameters);
        }

        /// <summary>
        /// 执行表达式翻译出的sql语句
        /// </summary>
        /// <returns></returns>
        internal ExecuteResult Execute()
        {
            var dbContext = _mapperDbContextBase;
            var executeType = dbContext.GetExecuteType(ToString());

            var executeResult = GetCache();
            if (executeResult == null)
            {
                executeResult = dbContext.RawExecute(ToString(), _parameters);
                SetCache(executeType, executeResult);
            }

            return executeResult;
        }

        /// <summary>
        /// 清空上次使用后留下的语句
        /// </summary>
        internal void ClearSql()
        {
            _originSql.Clear();
        }

        /// <summary>
        /// 清空上次使用后留下的参数
        /// </summary>
        internal void ClearParameter()
        {
            _parameters.Clear();
        }

        /// <summary>
        /// 获取作为要缓存的sql语句的key
        /// </summary>
        /// <returns></returns>
        private String PrepareCacheKey()
        {
            Parameter.Validate(_originSql);
            var cacheKey = ToString();
            foreach (var item in _parameters)
            {
                cacheKey = cacheKey.Replace(item.Key, item.Value.ToString());
            }
            return MD.GetMD5(cacheKey);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="executeType"></param>
        /// <param name="executeResult"></param>
        private void SetCache(ExecuteType executeType, ExecuteResult executeResult)
        {
            if (executeType != ExecuteType.SELECT)
            {
                return;
            }

            if (_queryCacheBase != null)
            {
                _queryCacheBase.Add(PrepareCacheKey(), executeResult);
            }
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <returns></returns>
        private ExecuteResult GetCache()
        {
            if (_queryCacheBase != null)
            {
                var cacheResult = _queryCacheBase.Get(PrepareCacheKey());
                if (cacheResult != null)
                {
                    RunDiagnosis.Info($@"SQL查询缓存返回:{cacheResult}");
                    return (ExecuteResult)cacheResult;
                }
            }
            RunDiagnosis.Warn($@"SQL查询缓存为NULL,将进行实际的SQL查询");
            return null;
        }

        /// <summary>
        /// 返回存储的sql语句
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            Parameter.Validate(_originSql);
            _originSql = _originSql.Replace("   ", " ").Replace("  ", " ");
            return _originSql.ToString().Trim();
        }
    }
}
