﻿using NewLibCore.Data.SQL.InternalDataStore;
using NewLibCore.Data.SQL.MapperExtension;
using NewLibCore.Data.SQL.PropertyExtension;
using System;
using System.ComponentModel;

namespace NewLibCore.Run
{
    internal class Program
    {
        private static void Main(String[] args)
        {
            SwitchDatabase.SwitchTo(Database.MYSQL);
            using (var context = new SqlContext())
            {
                context.InnerJoin<Member, User>((a, b) => a.UserId == b.Id).Count<Member>(d => d.Id == 1);
            }
        }
    }

    [Description("用户"), Serializable]
    public partial class User : DomainModelBase
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [PropertyRequired, PropertyInputRange(4, 10)]
        public String Name { get; private set; }

        /// <summary>
        /// 登陆密码
        /// </summary>
        [PropertyRequired]
        public String LoginPassword { get; private set; }

        /// <summary>
        /// 锁屏密码
        /// </summary>
        [PropertyRequired]
        public String LockScreenPassword { get; private set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        [PropertyDefaultValue(typeof(Boolean))]
        public Boolean IsDisable { get; private set; }

        /// <summary>
        /// 最后一次登录的时间
        /// </summary>
        [DateTimeDefaultValue]
        public DateTime LastLoginTime { get; private set; }

        /// <summary>
        /// 是否在线
        /// </summary>
        [PropertyDefaultValue(typeof(Boolean))]
        public Boolean IsOnline { get; private set; }

        /// <summary>
        /// 是否为管理员
        /// </summary>
        [PropertyDefaultValue(typeof(Boolean))]
        public Boolean IsAdmin { get; private set; }

        /// <summary>
        /// 配置Id
        /// </summary>
        [PropertyRequired]
        public Int32 ConfigId { get; private set; }

        /// <summary>
        /// 账户头像
        /// </summary>
        public String UserFace { get; private set; }


        public Boolean IsModifyUserFace { get; private set; }

        /// <summary>
        /// 实例化一个用户对象
        /// </summary>
        public User(String name, String password)
        {
            Name = name;
            LoginPassword = password;
            IsDisable = false;
            LastLoginTime = DateTime.Now;
            LockScreenPassword = password;
            IsOnline = false;
        }

        public User() { }
    }

    public partial class User
    {
        /// <summary>
        /// 修改登陆密码
        /// </summary>
        public User ModifyLoginPassword(String password)
        {
            if (String.IsNullOrEmpty(password))
            {
                throw new ArgumentException($@"{nameof(LoginPassword)}不能为空");
            }

            LoginPassword = password;
            OnPropertyChanged(nameof(LoginPassword));
            return this;
        }

        /// <summary>
        /// 修改锁屏密码
        /// </summary>
        public User ModifyLockScreenPassword(String password)
        {
            if (String.IsNullOrEmpty(password))
            {
                throw new ArgumentException($@"{nameof(LockScreenPassword)}不能为空");
            }

            LockScreenPassword = password;
            OnPropertyChanged(nameof(LockScreenPassword));
            return this;
        }

        /// <summary>
        /// 修改关联配置文件Id
        /// </summary>
        public User ModifyConfigId(Int32 configId)
        {
            ConfigId = configId;
            OnPropertyChanged(nameof(ConfigId));
            return this;
        }

        /// <summary>
        /// 账户启用
        /// </summary>
        public User Enable()
        {
            IsDisable = false;
            OnPropertyChanged(nameof(IsDisable));
            return this;
        }

        /// <summary>
        /// 账户禁用
        /// </summary>
        public User Disable()
        {
            IsDisable = true;
            OnPropertyChanged(nameof(IsDisable));
            return this;
        }

        /// <summary>
        /// 上线
        /// </summary>
        /// <returns></returns>
        public User Online()
        {
            IsOnline = true;
            OnPropertyChanged(nameof(IsOnline));

            LastLoginTime = DateTime.Now;
            OnPropertyChanged(nameof(LastLoginTime));
            return this;
        }

        /// <summary>
        /// 下线
        /// </summary>
        /// <returns></returns>
        public User Offline()
        {
            IsOnline = false;
            OnPropertyChanged(nameof(IsOnline));
            return this;
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        public User ModifyRoles(params Int32[] roleIds)
        {
            if (roleIds.Length == 0)
            {
                return this;
            }
            return this;
        }

        /// <summary>
        /// 去除管理员角色
        /// </summary>
        public User DetachAdminRole()
        {
            IsAdmin = false;
            OnPropertyChanged(nameof(IsAdmin));
            return this;
        }

        /// <summary>
        /// 附加管理员角色
        /// </summary>
        /// <returns></returns>
        public User AttachAdminRole()
        {
            IsAdmin = true;
            OnPropertyChanged(nameof(IsAdmin));
            return this;
        }
    }

    /// <summary>
    /// 桌面应用
    /// </summary>
    [Serializable, Description("桌面应用")]
    public partial class Member : DomainModelBase
    {
        /// <summary>
        /// 应用Id
        /// </summary>
        [PropertyRequired]
        public Int32 AppId { get; private set; }

        /// <summary>
        /// 桌面应用的宽
        /// </summary>
        [PropertyRequired]
        public Int32 Width { get; private set; }

        /// <summary>
        /// 桌面应用的高
        /// </summary>
        [PropertyRequired]
        public Int32 Height { get; private set; }

        /// <summary>
        /// 文件夹Id
        /// </summary>
        [PropertyDefaultValue(typeof(Int32))]
        public Int32 FolderId { get; private set; }

        /// <summary>
        /// 名称
        /// </summary>
        [PropertyRequired, PropertyInputRange(10)]
        public String Name { get; private set; }

        /// <summary>
        /// 图标地址
        /// </summary>
        [PropertyRequired, PropertyInputRange(150)]
        public String IconUrl { get; private set; }

        /// <summary>
        /// app地址
        /// </summary>
        [PropertyInputRange(150), PropertyDefaultValue(typeof(String))]
        public String AppUrl { get; private set; }

        /// <summary>
        /// 桌面应用是否在应用码头上
        /// </summary>
        [PropertyDefaultValue(typeof(Boolean))]
        public Boolean IsOnDock { get; private set; }

        /// <summary>
        /// 是否显示app底部的按钮
        /// </summary>
        [PropertyDefaultValue(typeof(Boolean))]
        public Boolean IsSetbar { get; private set; }

        /// <summary>
        /// 是否打开最大化
        /// </summary>
        [PropertyDefaultValue(typeof(Boolean))]
        public Boolean IsOpenMax { get; private set; }

        /// <summary>
        /// 是否为福莱希
        /// </summary>
        [PropertyDefaultValue(typeof(Boolean))]
        public Boolean IsFlash { get; private set; }

        /// <summary>
        /// 是否可以拉伸
        /// </summary>
        [PropertyDefaultValue(typeof(Boolean))]
        public Boolean IsResize { get; private set; }

        /// <summary>
        /// 桌面索引
        /// </summary>
        [PropertyDefaultValue(typeof(Int32), 1)]
        public Int32 DeskIndex { get; private set; }

        /// <summary>
        /// 账户Id
        /// </summary>
        [PropertyRequired]
        public Int32 UserId { get; private set; }

        /// <summary>
        /// 图标是否来自上传
        /// </summary>
        [PropertyDefaultValue(typeof(Boolean))]
        public Boolean IsIconByUpload { get; private set; }

        public Double StarCount { get; set; }

        /// <summary>
        /// 实例化一个桌面应用对象
        /// </summary>
        public Member(
                    String name,
                    String iconUrl,
                    String appUrl,
                    Int32 appId,
                    Int32 width,
                    Int32 height,
                    Int32 userId,
                    Int32 deskIndex,
                    Boolean isIconByUpload = default(Boolean),
                    Boolean isSetbar = default(Boolean),
                    Boolean isOpenMax = default(Boolean),
                    Boolean isFlash = default(Boolean),
                    Boolean isResize = default(Boolean))
        {
            AppId = appId;
            Width = width > 800 ? 800 : width;
            Height = height > 600 ? 600 : height;
            IsOpenMax = isOpenMax;
            IsSetbar = isSetbar;
            IsFlash = isFlash;
            IsResize = isResize;
            Name = name;
            IconUrl = iconUrl;
            AppUrl = appUrl;
            DeskIndex = 1;
            IsIconByUpload = isIconByUpload;
            UserId = userId;
        }

        /// <summary>
        /// 实例化一个桌面应用对象
        /// </summary>
        public Member(String name,
            String iconUrl,
            Int32 appId,
            Int32 userId,
            Int32 deskIndex,
            Boolean isIconByUpload = default(Boolean))
        {
            AppId = appId;
            Width = 800;
            Height = 600;
            IsOpenMax = false;
            Name = name;
            IconUrl = iconUrl;
            DeskIndex = deskIndex;
            IsIconByUpload = isIconByUpload;
            UserId = userId;
        }

        public Member()
        {
        }

    }

    public partial class Member
    {

        public Member ModifyWidth(Int32 width)
        {
            if (width <= 0)
            {
                throw new ArgumentException($@"{nameof(width)} 不能小于或等于0");
            }

            if (width == Width)
            {
                return this;
            }

            Width = width;
            OnPropertyChanged(nameof(Width));
            return this;
        }

        public Member ModifyHeight(Int32 height)
        {
            if (height <= 0)
            {
                throw new ArgumentException($@"{nameof(height)} 不能小于或等于0");
            }

            if (height == Height)
            {
                return this;
            }

            Height = height;
            OnPropertyChanged(nameof(Height));
            return this;
        }

        public Member ModifyFolderId(Int32 folderId)
        {
            if (folderId == FolderId)
            {
                return this;
            }

            FolderId = folderId;
            OnPropertyChanged(nameof(FolderId));
            return this;
        }

        public Member ModifyName(String name)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentException($@"{nameof(name)}不能为空");
            }

            if (name == Name)
            {
                return this;
            }

            Name = name;
            OnPropertyChanged(nameof(Name));
            return this;
        }

        public Member ModifyIconUrl(String iconUrl)
        {
            if (String.IsNullOrEmpty(iconUrl))
            {
                throw new ArgumentException($@"{nameof(iconUrl)}不能为空");
            }

            if (iconUrl == IconUrl)
            {
                return this;
            }

            IconUrl = iconUrl;
            OnPropertyChanged(nameof(IconUrl));
            return this;
        }

        public Member ModifyAppUrl(String appUrl)
        {
            if (String.IsNullOrEmpty(appUrl))
            {
                throw new ArgumentException($@"{nameof(appUrl)}不能为空");
            }

            if (appUrl == AppUrl)
            {
                return this;
            }

            AppUrl = appUrl;
            OnPropertyChanged(nameof(AppUrl));
            return this;
        }

        public Member OnDock()
        {
            IsOnDock = true;
            OnPropertyChanged(nameof(IsOnDock));
            return this;
        }

        public Member OutDock()
        {
            IsOnDock = false;
            OnPropertyChanged(nameof(IsOnDock));
            return this;
        }

        public Member Setbar()
        {
            IsSetbar = true;
            OnPropertyChanged(nameof(IsSetbar));
            return this;
        }

        public Member NotSetbar()
        {
            IsSetbar = false;
            OnPropertyChanged(nameof(IsSetbar));
            return this;
        }

        public Member OpenMax()
        {
            IsOpenMax = true;
            OnPropertyChanged(nameof(IsOpenMax));
            return this;
        }

        public Member NotOpenMax()
        {
            IsOpenMax = false;
            OnPropertyChanged(nameof(IsOpenMax));
            return this;
        }

        public Member Flash()
        {
            IsFlash = true;
            OnPropertyChanged(nameof(IsFlash));
            return this;
        }

        public Member NotFlash()
        {
            IsFlash = false;
            OnPropertyChanged(nameof(IsFlash));
            return this;
        }

        public Member Resize()
        {
            IsResize = true;
            OnPropertyChanged(nameof(IsResize));
            return this;
        }

        public Member NotResize()
        {
            IsResize = false;
            OnPropertyChanged(nameof(IsResize));
            return this;
        }

        public Member ModifyDeskIndex(Int32 deskIndex)
        {
            if (deskIndex <= 0)
            {
                throw new ArgumentException($@"{nameof(deskIndex)} 不能小于或等于0");
            }

            if (deskIndex == DeskIndex)
            {
                return this;
            }

            DeskIndex = deskIndex;
            OnPropertyChanged(nameof(DeskIndex));
            return this;
        }

        public Member IconNotFromUpload()
        {
            IsIconByUpload = false;
            OnPropertyChanged(nameof(IsIconByUpload));
            return this;
        }

        public Member IconFromUpload()
        {
            IsIconByUpload = true;
            OnPropertyChanged(nameof(IsIconByUpload));
            return this;
        }
    }

    public partial class VisitorRecord : DomainModelBase
    {
        public Int32 UserId { get; private set; }

        [PropertyRequired, PropertyInputRange(10), PropertyDefaultValue(typeof(String), "11111")]
        public String UserName { get; private set; }

        public VisitorRecord(Int32 userId, String userName)
        {
            UserId = userId;
            UserName = userName;
        }

        public VisitorRecord() { }
    }
}
