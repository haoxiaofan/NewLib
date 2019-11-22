﻿using System;
using System.Collections.Generic;
using NewLibCore.Data.SQL.Mapper.Cache;

namespace NewLibCore.Data.SQL.Mapper
{
    /// <summary>
    /// 为相应的数据库实例提供对应的模板化SQL
    /// </summary>
    internal abstract class InstanceConfig
    {
        /// <summary>
        /// 逻辑关系映射
        /// </summary>
        protected readonly IDictionary<RelationType, String> LogicRelationMapper = new Dictionary<RelationType, String>();

        /// <summary>
        /// 连接关系映射
        /// </summary>
        protected readonly IDictionary<JoinRelation, String> JoinRelationMapper = new Dictionary<JoinRelation, String>();

        /// <summary>
        /// 排序方式映射
        /// </summary>
        protected readonly IDictionary<OrderByType, String> OrderTypeMapper = new Dictionary<OrderByType, String>();

        /// <summary>
        /// 初始化InstanceConfig类的新实例
        /// </summary>
        protected InstanceConfig()
        {
            LogicRelationMapper.Clear();
            JoinRelationMapper.Clear();
            OrderTypeMapper.Clear();

            InitRelationType();
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
        protected abstract void AppendRelationType();

        /// <summary>
        /// 模板扩展
        /// </summary>
        /// <value></value>
        internal virtual InstanceExtension Extension { get; }

        /// <summary>
        /// 创建谓词关系
        /// </summary>
        /// <param name="relationType">关系的类型</param>
        /// <param name="left">左语句</param>
        /// <param name="right">右语句</param>
        /// <returns></returns>
        internal abstract String CreatePredicate(RelationType relationType, String left, String right);

        /// <summary>
        /// 连接语句构建
        /// </summary>
        /// <param name="joinRelation">连接类型</param>
        /// <param name="left">左语句</param>
        /// <param name="right">右语句</param>
        /// <returns></returns>
        internal String JoinBuilder(JoinRelation joinRelation, String left, String right)
        {
            return String.Format(JoinRelationMapper[joinRelation], left, right);
        }

        /// <summary>
        /// 排序语句构建
        /// </summary>
        /// <param name="orderByType">排序方向</param>
        /// <param name="left">左语句</param>
        /// <returns></returns>
        internal String OrderByBuilder(OrderByType orderByType, String left)
        {
            return String.Format(OrderTypeMapper[orderByType], left);
        }

        /// <summary>
        /// 初始化默认逻辑关系
        /// </summary>
        private void InitRelationType()
        {
            LogicRelationMapper.Add(RelationType.AND, " {0} AND {1} ");
            LogicRelationMapper.Add(RelationType.OR, " {0} OR {1} ");
            LogicRelationMapper.Add(RelationType.EQ, " {0} = {1} ");
            LogicRelationMapper.Add(RelationType.NQ, " {0} <> {1} ");
            LogicRelationMapper.Add(RelationType.GT, " {0} < {1} ");
            LogicRelationMapper.Add(RelationType.LT, " {0} > {1} ");
            LogicRelationMapper.Add(RelationType.GE, " {0} <= {1} ");
            LogicRelationMapper.Add(RelationType.LE, " {0} >= {1} ");

            AppendRelationType();
        }

        /// <summary>
        /// 初始化默认连接语句
        /// </summary>
        private void InitJoinType()
        {
            JoinRelationMapper.Add(JoinRelation.NONE, "");
            JoinRelationMapper.Add(JoinRelation.INNER, " INNER JOIN {0} AS {1} ON ");
            JoinRelationMapper.Add(JoinRelation.LEFT, " LEFT JOIN {0} AS {1} ON ");
            JoinRelationMapper.Add(JoinRelation.RIGHT, " RIGHT JOIN {0} AS {1} ON ");
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

    /// <summary>
    /// 数据库实例扩展
    /// </summary>
    internal class InstanceExtension
    {
        /// <summary>
        /// 自增主键
        /// </summary>
        /// <value></value>
        public String Identity { get; internal set; }

        /// <summary>
        /// 执行受影响的行数
        /// </summary>
        /// <value></value>
        public String RowCount { get; internal set; }

        /// <summary>
        /// 分页 每页条数与多少页
        /// </summary>
        /// <value></value>
        public String Page { get; internal set; }
    }
}
