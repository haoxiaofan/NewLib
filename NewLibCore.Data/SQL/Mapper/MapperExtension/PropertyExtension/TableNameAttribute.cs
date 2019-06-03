using System;

namespace NewLibCore.Data.SQL.Mapper.Extension.PropertyExtension
{
    public class TableNameAttribute : Attribute
    {
        public String TableName { get; private set; }

        /// <summary>
        /// �����ݱ��ʱ������ʧЧ
        /// </summary>
        public Boolean InvalidCacheThenUpdate { get; private set; }

        public TableNameAttribute(String name, Boolean invalidCacheThenUpdate) : this(name)
        {
            InvalidCacheThenUpdate = invalidCacheThenUpdate;
        }

        public TableNameAttribute(String name)
        {
            TableName = name;
        }
    }
}