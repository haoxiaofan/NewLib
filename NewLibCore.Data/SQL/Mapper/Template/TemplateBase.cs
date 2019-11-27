﻿using System;
using System.Collections.Generic;
using NewLibCore.Validate;

namespace NewLibCore.Data.SQL.Mapper.Template
{
    /// <summary>
    /// 为相应的数据库实例提供对应的模板化SQL
    /// </summary>
    internal abstract class TemplateBase
    {
        /// <summary>
        /// 谓词关系映射
        /// </summary>
        protected readonly IDictionary<PredicateType, String> PredicateMapper = new Dictionary<PredicateType, String>();

        /// <summary>
        /// 连接关系映射
        /// </summary>
        protected readonly IDictionary<JoinRelation, String> JoinMapper = new Dictionary<JoinRelation, String>();

        /// <summary>
        /// 排序方式映射
        /// </summary>
        protected readonly IDictionary<OrderByType, String> OrderTypeMapper = new Dictionary<OrderByType, String>();

        /// <summary>
        /// 初始化TemplateBase类的新实例
        /// </summary>
        protected TemplateBase()
        {
            JoinMapper.Clear();
            PredicateMapper.Clear();
            OrderTypeMapper.Clear();

            InitPredicateType();
            InitJoinType();
            InitOrderType();
        }

        /// <summary>
        /// 查询模板
        /// </summary>
        internal virtual String SelectTemplate
        {
            get
            {
                return "SELECT {0} FROM {1} AS {2} ";
            }
        }

        /// <summary>
        /// 添加模板
        /// </summary>
        internal virtual String InsertTemplate
        {
            get
            {
                return "INSERT {0} ({1}) VALUES({2}) {3}";
            }
        }

        /// <summary>
        /// 更新模板
        /// </summary>
        internal abstract String UpdateTemplate { get; }

        /// <summary>
        /// 追加关系类型
        /// </summary>
        protected abstract void AppendPredicateType();

        /// <summary>
        /// 自增主键
        /// </summary>
        /// <value></value>
        internal abstract String Identity { get; }

        /// <summary>
        /// 执行受影响的行数
        /// </summary>
        /// <value></value>
        internal abstract String RowCount { get; }

        /// <summary>
        /// 追加分页语句
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="parserResult"></param>
        /// <returns></returns>
        internal abstract ParserResult CreatePagination(Int32 pageIndex, Int32 pageSize, String orderBy, ParserResult parserResult);

        /// <summary>
        /// 创建谓词关系
        /// </summary>
        /// <param name="predicateType">关系的类型</param>
        /// <param name="left">左语句</param>
        /// <param name="right">右语句</param>
        /// <returns></returns>
        internal abstract String CreatePredicate(PredicateType predicateType, String left, String right);

        /// <summary>
        /// 创建连接关系
        /// </summary>
        /// <param name="joinRelation">连接类型</param>
        /// <param name="left">左语句</param>
        /// <param name="right">右语句</param>
        /// <returns></returns>
        internal String CreateJoin(JoinRelation joinRelation, String left, String right)
        {
            Parameter.Validate(joinRelation);
            Parameter.Validate(left);
            Parameter.Validate(right);

            if (!JoinMapper.ContainsKey(joinRelation))
            {
                throw new ArgumentNullException($@"{joinRelation}不存在");
            }

            return String.Format(JoinMapper[joinRelation], left, right);
        }

        /// <summary>
        /// 排序语句构建
        /// </summary>
        /// <param name="orderByType">排序方向</param>
        /// <param name="left">左语句</param>
        /// <returns></returns>
        internal String CreateOrderBy(OrderByType orderByType, String left)
        {
            Parameter.Validate(orderByType);
            Parameter.Validate(left);

            if (!OrderTypeMapper.ContainsKey(orderByType))
            {
                throw new ArgumentNullException($@"{orderByType}不存在");
            }
            return String.Format(OrderTypeMapper[orderByType], left);
        }

        /// <summary>
        /// 初始化默认逻辑关系
        /// </summary>
        private void InitPredicateType()
        {
            PredicateMapper.Add(PredicateType.AND, " {0} AND {1} ");
            PredicateMapper.Add(PredicateType.OR, " {0} OR {1} ");
            PredicateMapper.Add(PredicateType.EQ, " {0} = {1} ");
            PredicateMapper.Add(PredicateType.NQ, " {0} <> {1} ");
            PredicateMapper.Add(PredicateType.GT, " {0} < {1} ");
            PredicateMapper.Add(PredicateType.LT, " {0} > {1} ");
            PredicateMapper.Add(PredicateType.GE, " {0} <= {1} ");
            PredicateMapper.Add(PredicateType.LE, " {0} >= {1} ");

            AppendPredicateType();
        }

        /// <summary>
        /// 初始化默认连接语句
        /// </summary>
        private void InitJoinType()
        {
            JoinMapper.Add(JoinRelation.NONE, "");
            JoinMapper.Add(JoinRelation.INNER, " INNER JOIN {0} AS {1} ON ");
            JoinMapper.Add(JoinRelation.LEFT, " LEFT JOIN {0} AS {1} ON ");
            JoinMapper.Add(JoinRelation.RIGHT, " RIGHT JOIN {0} AS {1} ON ");
        }

        /// <summary>
        /// 初始化排序类型
        /// </summary>
        private void InitOrderType()
        {
            OrderTypeMapper.Add(OrderByType.ASC, " ORDER BY {0} ASC ");
            OrderTypeMapper.Add(OrderByType.DESC, " ORDER BY {0} DESC ");
        }
    }
}