﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewLibCore.Data.SQL.Mapper.Translation
{
    internal class TranslationCoreResult
    {
        internal StringBuilder SqlStore { get; private set; }

        internal IList<EntityParameter> ParameterStore { get; private set; }

        internal TranslationCoreResult()
        {
            SqlStore = new StringBuilder();
            ParameterStore = new List<EntityParameter>();
        }

        internal void Append(String sql, IEnumerable<EntityParameter> entityParameters = null)
        {
            SqlStore.Append(sql);
            if (entityParameters != null)
            {
                foreach (var item in entityParameters)
                {
                    ParameterStore.Add(item);
                }
            }
        }

        internal void Append(params EntityParameter[] entityParameters)
        {
            Append(entityParameters.ToList());
        }

        internal void Append(IEnumerable<EntityParameter> entityParameters)
        {
            if (entityParameters != null)
            {
                foreach (var item in entityParameters)
                {
                    ParameterStore.Add(item);
                }
            }
        }

        internal void Clear()
        {
            SqlStore.Clear();
            ParameterStore.Clear();
        }
    }
}