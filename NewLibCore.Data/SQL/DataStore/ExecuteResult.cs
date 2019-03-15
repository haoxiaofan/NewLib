﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NewLibCore.Data.SQL.DataStore
{
    internal class TranslationResult
    {
        internal StringBuilder SqlStore { get; private set; }

        internal IList<SqlParameterMapper> ParameterStore { get; private set; }

        internal TranslationResult()
        {
            SqlStore = new StringBuilder();
            ParameterStore = new List<SqlParameterMapper>();
        }

        internal void Append(String sql)
        {
            SqlStore.Append(sql);
        }

        internal void AppendParameter(params SqlParameterMapper[] mapper)
        {
            foreach (var item in mapper)
            {
                ParameterStore.Add(item);
            }
        }

        internal void Clear()
        {
            SqlStore.Clear();
            ParameterStore.Clear();
        }
    }
}