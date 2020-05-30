﻿using System;
using NewLibCore.Data.SQL;
using NewLibCore.Data.SQL.Validate;

namespace NewLibCore.UnitTest.Entitys.Agent
{
    [TableName("newcrm_user_role")]
    public class UserRole : EntityBase
    {
        [Required]
        public Int32 UserId { get; private set; }

        [Required]
        public Int32 RoleId { get; private set; }

        public UserRole(Int32 userId, Int32 roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }

        public UserRole() { }
    }
}
