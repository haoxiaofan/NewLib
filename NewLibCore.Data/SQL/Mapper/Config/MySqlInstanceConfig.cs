﻿using System;
using System.Data.Common;
using MySql.Data.MySqlClient;
using NewLibCore.Data.SQL.Mapper.Extension;

namespace NewLibCore.Data.SQL.Mapper.Config
{
    internal class MySqlInstanceConfig : DatabaseInstanceConfig
    {
        protected internal MySqlInstanceConfig(ILogger logger) : base(logger)
        {

        }

        protected override void AppendRelationType()
        {
            RelationMapper.Add(RelationType.FULL_LIKE, "{0} LIKE CONCAT('%',{1},'%')");
            RelationMapper.Add(RelationType.START_LIKE, "{0} LIKE CONCAT('',{1},'%')");
            RelationMapper.Add(RelationType.END_LIKE, "{0} LIKE CONCAT('%',{1},'')");
            RelationMapper.Add(RelationType.IN, "FIND_IN_SET({0},{1})");
        }

        internal override DbConnection GetConnectionInstance()
        {
            return new MySqlConnection(ConnectionString);
        }

        internal override DbParameter GetParameterInstance()
        {
            return new MySqlParameter();
        }

        internal override String RelationBuilder(RelationType relationType, String left, Object right)
        {
            return String.Format(RelationMapper[relationType], left, right);
        }

        internal override InstanceExtension Extension
        {
            get
            {
                return new InstanceExtension
                {
                    Identity = " ; SELECT CAST(@@IDENTITY AS SIGNED) AS c ",
                    RowCount = " ; SELECT CAST(ROW_COUNT() AS SIGNED) AS c ",
                    Page = " LIMIT {value},{pageSize}"
                };
            }
        }
    }
}