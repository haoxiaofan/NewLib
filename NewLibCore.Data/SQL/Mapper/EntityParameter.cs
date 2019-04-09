﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using NewLibCore.Data.SQL.Mapper.Config;
using NewLibCore.InternalExtension;
using NewLibCore.Security;

namespace NewLibCore.Data.SQL.Mapper
{
    public class EntityParameter
    {
        public EntityParameter(String key, Object value)
        {
            Key = key;
            Value = ParseValueType(value);
        }

        public String Key { get; private set; }

        public Object Value { get; private set; }

        public static implicit operator DbParameter(EntityParameter entityParameter)
        {
            var parameter = MapperFactory.Instance.GetParameterInstance();
            parameter.ParameterName = entityParameter.Key;
            parameter.Value = entityParameter.Value;
            return parameter;
        }

        private Object ParseValueType(Object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException($@"SQL参数:{Key}的值为null");
            }

            if (obj.GetType() == typeof(String))
            {
                return UnlegalChatDetection.FilterBadChat(obj.ToString());
            }

            if (obj.GetType() == typeof(Boolean))
            {
                return (Boolean)obj ? 1 : 0;
            }

            if (obj.IsComplexType())
            {
                var objType = obj.GetType();
                if (objType.IsArray || objType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    if (objType.GetGenericArguments()[0] == typeof(String))
                    {
                        return String.Join(",", ((IList<String>)obj).Select(s => $@"'{s}'"));
                    }
                    return String.Join(",", (IList<Int32>)obj);
                }
            }

            return obj;
        }
    }
}