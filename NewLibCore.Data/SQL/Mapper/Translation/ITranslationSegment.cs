﻿namespace NewLibCore.Data.SQL.Mapper.Translation
{
    /// <summary>
    /// 翻译表达式
    /// </summary>
    internal interface ITranslationSegment
    {
        /// <summary>
        /// 翻译
        /// </summary>
        /// <returns></returns>
        void Translate();
    }
}