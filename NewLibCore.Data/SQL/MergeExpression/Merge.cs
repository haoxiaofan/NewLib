﻿using System;
using System.Linq.Expressions;
using NewLibCore.Data.SQL.Mapper.EntityExtension;
using NewLibCore.Validate;

namespace NewLibCore.Data.SQL.MergeExpression
{
    /// <summary>
    /// 合并作为查询条件的表达式树
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Merge<T> where T : EntityBase
    {
        public Expression<Func<T, Boolean>> MergeExpression { get; set; }

        /// <summary>
        /// 追加一个表达式树对象
        /// </summary>
        /// <param name="right"></param>
        /// <typeparam name="T1"></typeparam>
        /// <returns></returns>
        public Expression<Func<T, T1, Boolean>> Append<T1>(Merge<T1> right) where T1 : EntityBase
        {
            Parameter.Validate(right);
            Parameter.Validate(right.MergeExpression);

            Expression leftBody, rightBody;
            ParameterExpression leftParameter, rightParameter;
            {
                var type = typeof(T);
                leftParameter = Expression.Parameter(type, type.GetTableName().AliasName);
                var parameterVister = new ParameterVisitor(leftParameter);
                leftBody = parameterVister.Replace(MergeExpression.Body);
            }

            {
                var type = typeof(T1);
                rightParameter = Expression.Parameter(type, type.GetTableName().AliasName);
                var parameterVister = new ParameterVisitor(rightParameter);
                rightBody = parameterVister.Replace(right.MergeExpression.Body);
            }

            var newExpression = Expression.AndAlso(leftBody, rightBody);
            return Expression.Lambda<Func<T, T1, Boolean>>(newExpression, leftParameter, rightParameter);
        }

        /// <summary>
        /// 隐式转换为一个表达式树
        /// </summary>
        /// <param name="combination"></param>
        /// <returns></returns>
        public static implicit operator Expression<Func<T, Boolean>>(Merge<T> combination)
        {
            Parameter.Validate(combination);
            return combination.MergeExpression;
        }
    }
}