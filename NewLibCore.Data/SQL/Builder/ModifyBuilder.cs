﻿using System;
using System.Linq;
using NewLibCore.Data.SQL.Mapper;
using NewLibCore.Data.SQL.Mapper.Config;
using NewLibCore.Data.SQL.Mapper.Extension;
using NewLibCore.Data.SQL.Mapper.Translation;
using NewLibCore.Validate;

namespace NewLibCore.Data.SQL.Builder
{
	internal class ModifyBuilder<TModel> : IBuilder<TModel> where TModel : PropertyMonitor, new()
	{
		private readonly Boolean _isValidate;
		private readonly StatementStore _statementStore;
		private readonly TModel _model;

		public ModifyBuilder(TModel model, StatementStore statementStore, Boolean isValidate = false)
		{
			Parameter.Validate(model);
			Parameter.Validate(statementStore);

			_isValidate = isValidate;
			_statementStore = statementStore;
			_model = model;
		}

		public TranslationCoreResult Build()
		{
			var properties = _model.PropertyInfos;
			if (!properties.Any())
			{
				throw new ArgumentNullException("没有找到需要更新的字段");
			}
			_model.SetUpdateTime();
			if (_isValidate)
			{
				_model.Validate(properties);
			}

			var translation = new TranslationCore(_statementStore);
			translation.Result.Append($@"UPDATE {typeof(TModel).Name} SET {String.Join(",", properties.Select(s => $@"{s.Name}=@{s.Name}"))}", properties.Select(c => new EntityParameter($@"@{c.Name}", c.GetValue(_model))));

			if (_statementStore.Where != null)
			{
				translation.Translate();
			}
			translation.Result.Append($@"{MapperFactory.Instance.Extension.RowCount}");
			return translation.Result;
		}
	}
}
