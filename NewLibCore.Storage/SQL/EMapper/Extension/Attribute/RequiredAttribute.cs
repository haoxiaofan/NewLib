﻿using System;
using NewLibCore.Validate;

namespace NewLibCore.Storage.SQL.Validate
{
    /// <summary>
    /// 标记被修饰的属性不能为空
    /// </summary>
    public class RequiredAttribute : PropertyValidateAttribute
    {
        internal override Int32 Order
        {
            get { return 3; }
        }

        internal override String FailReason(String fieldName)
        {
            return $@"{fieldName} 为必填项!";
        }

        internal override Boolean IsValidate(ChangedProperty property)
        {
            try
            {
                Check.IfNullOrZero(property);
                Check.IfNullOrZero(property.Value);

                var type = property.Value.GetType();

                if (type.IsValueType && type.IsNumeric())
                {
                    Check.IfNullOrZero((ValueType)property.Value);
                }
                else
                {
                    Check.IfNullOrZero(property.Value);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
