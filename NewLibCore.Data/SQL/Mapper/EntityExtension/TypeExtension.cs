using System;
using System.Linq;
using NewLibCore.Data.SQL.Mapper.AttributeExtension;

namespace NewLibCore.Data.SQL.Mapper.EntityExtension
{
    public static class TypeExtension
    {
        /// <summary>
        /// 获取设置在实体的指定表名
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static String GetTableName(this Type t)
        {
            var attrubutes = t.GetCustomAttributes(typeof(TableNameAttribute), true);
            if (!attrubutes.Any())
            {
                return t.Name;
            }

            return ((TableNameAttribute)attrubutes.FirstOrDefault()).TableName;
        }
    }
}