﻿using System.ComponentModel;

namespace NewLibCore.UnitTest.ValueObject
{
    /// <summary>
    /// 日志等级
    /// </summary>
    public enum LogLevel
    {
        [Description("信息")]
        Info = 1,

        [Description("警告")]
        Warning = 2,

        [Description("调试")]
        Debug = 3,

        [Description("业务错误")]
        Error = 4,

        [Description("代码异常")]
        Exception = 5
    }
}
