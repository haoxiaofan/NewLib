using System;
using System.Data;
using NewLibCore.Logger;
using NewLibCore.Validate;
using NewLibCore.Storage.SQL.Template;
using NewLibCore.Storage.SQL.Extension;
using Microsoft.Extensions.DependencyInjection;

namespace NewLibCore.Storage.SQL.EMapper
{
    public class EntityMapperOptions
    {

        /// <summary>
        /// 连接字符串名称
        /// </summary>
        /// <value></value>
        public String ConnectionStringName { get; set; }

        /// <summary>
        /// 是否在出现异常时抛出异常
        /// </summary>
        /// <value></value>
        internal Boolean ThrowException { get; set; } = true;

        /// <summary>
        /// 启用模型验证
        /// </summary> 
        /// <value></value>
        internal Boolean EnableModelValidate { get; set; } = true;

        /// <summary>
        /// 事务隔离级别
        /// </summary>
        internal IsolationLevel TransactionLevel { get; set; }

        /// <summary> 
        /// 映射的数据库类型
        /// </summary>
        internal MapperType MapperType { get; set; } = MapperType.NONE;

        internal TemplateBase TemplateBase { get; private set; }

        /// <summary>
        /// 切换为mysql
        /// </summary>
        public void UseMySql()
        {
            MapperType = MapperType.MYSQL;
            TemplateBase = new MySqlTemplate();
        }

        /// <summary>
        /// 切换为mssql
        /// </summary>
        public void UseMsSql()
        {
            MapperType = MapperType.MSSQL;
            TemplateBase = new MsSqlTemplate();
        }

        /// <summary>
        /// 设置事务隔离级别
        /// </summary>
        /// <param name="isolationLevel"></param>
        public void SetTransactionLevel(IsolationLevel isolationLevel)
        {
            TransactionLevel = isolationLevel;
        }

        /// <summary>
        /// 设置自定义日志记录组件
        /// </summary>
        /// <param name="logger"></param>
        public void SetLogger(ILogger logger = null)
        {
            RunDiagnosis.SetLoggerInstance(logger ?? new DefaultLogger());
        }
    }
}