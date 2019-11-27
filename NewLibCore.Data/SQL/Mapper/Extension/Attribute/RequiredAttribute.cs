﻿using System;
using NewLibCore.Validate;

namespace NewLibCore.Data.SQL.Mapper
{
    /// <summary>
    /// 标记被修饰的属性不能为空
    /// </summary>
    public class RequiredAttribute : PropertyValidate
    {
        public override Int32 Order
        {
            get { return 3; }
        }

        public override String FailReason(String fieldName)
        {
            return $@"{fieldName} 为必填项!";
        }

        public override Boolean IsValidate(Object value)
        {
            var type = value.GetType();

            try
            {
                if (type.IsValueType && type.IsNumeric())
                {
                    Parameter.Validate((ValueType)value);
                }
                else
                {
                    Parameter.Validate(value);
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