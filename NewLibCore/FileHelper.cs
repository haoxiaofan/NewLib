﻿using System;
using System.IO;
using NewLibCore.Validate;

namespace NewLibCore
{
    /// <summary>
    /// 提供对文件的操作类
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// 检测指定目录是否存在
        /// </summary>
        public static Boolean IsExistDirectory(String directoryPath)
        {
            Check.IfNullOrZero(directoryPath);
            return Directory.Exists(directoryPath);
        }

        #region 检测指定文件是否存在
        /// <summary>
        /// 检测指定文件是否存在,如果存在则返回true。
        /// </summary>
        public static Boolean IsExistFile(String filePath)
        {
            Check.IfNullOrZero(filePath);
            return File.Exists(filePath);
        }
        #endregion

        #region 创建一个目录
        /// <summary>
        /// 创建一个目录
        /// </summary>
        public static void CreateDirectory(String directoryPath)
        {
            Check.IfNullOrZero(directoryPath);

            //如果目录不存在则创建该目录
            if (!IsExistDirectory(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
        #endregion

        #region 获取文本文件的行数
        /// <summary>
        /// 获取文本文件的行数
        /// </summary>
        public static Int32 GetLineCount(String filePath)
        {
            Check.IfNullOrZero(filePath);

            //将文本文件的各行读到一个字符串数组中
            var rows = File.ReadAllLines(filePath);
            if (rows == null || rows.Length == 0)
            {
                throw new Exception("没有从指定的路径中获取文件的行数");
            }
            //返回行数
            return rows.Length;
        }
        #endregion

        #region 获取指定目录中的文件列表
        /// <summary>
        /// 获取指定目录中所有文件列表
        /// </summary>
        public static String[] GetFileNames(String directoryPath)
        {
            Check.IfNullOrZero(directoryPath);
            //如果目录不存在，则抛出异常
            if (!IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }

            //获取文件列表
            return Directory.GetFiles(directoryPath);
        }

        /// <summary>
        /// 获取指定目录及子目录中所有文件列表
        /// </summary>
        public static String[] GetFileNames(String directoryPath, String searchPattern, Boolean isSearchChild)
        {
            Check.IfNullOrZero(directoryPath);
            Check.IfNullOrZero(searchPattern);

            //如果目录不存在，则抛出异常
            if (!IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }
            try
            {
                if (isSearchChild)
                {
                    return Directory.GetFiles(directoryPath, searchPattern, SearchOption.AllDirectories);
                }
                return Directory.GetFiles(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
            }
            catch (IOException ex)
            {
                throw new IOException(ex.Message);
            }
        }
        #endregion

        #region 获取指定目录中的子目录列表
        /// <summary>
        /// 获取指定目录中所有子目录列表,若要搜索嵌套的子目录列表,请使用重载方法.
        /// </summary>
        public static String[] GetDirectories(String directoryPath)
        {
            Check.IfNullOrZero(directoryPath);
            try
            {
                return Directory.GetDirectories(directoryPath);
            }
            catch (IOException ex)
            {
                throw new IOException(ex.Message);
            }
        }

        /// <summary>
        /// 获取指定目录及子目录中所有子目录列表
        /// </summary>
        public static String[] GetDirectories(String directoryPath, String searchPattern, Boolean isSearchChild)
        {
            Check.IfNullOrZero(directoryPath);
            Check.IfNullOrZero(searchPattern);

            try
            {
                if (isSearchChild)
                {
                    return Directory.GetDirectories(directoryPath, searchPattern, SearchOption.AllDirectories);
                }
                return Directory.GetDirectories(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
            }
            catch (IOException ex)
            {
                throw new IOException(ex.Message);
            }
        }
        #endregion

        #region 向文本文件写入内容
        /// <summary>
        /// 向文本文件中写入内容
        /// </summary>
        public static void WriteText(String filePath, String content)
        {
            Check.IfNullOrZero(filePath);
            Check.IfNullOrZero(content);

            File.WriteAllText(filePath, content);
        }
        #endregion

        #region 向文本文件的尾部追加内容
        /// <summary>
        /// 向文本文件的尾部追加内容
        /// </summary>
        public static void AppendText(String filePath, String content)
        {
            Check.IfNullOrZero(filePath);
            Check.IfNullOrZero(content);

            File.AppendAllText(filePath, content);
        }
        #endregion

        #region 将现有文件的内容复制到新文件中
        /// <summary>
        /// 将源文件的内容复制到目标文件中
        /// </summary>
        public static void Copy(String sourceFilePath, String destFilePath)
        {
            Check.IfNullOrZero(sourceFilePath);
            Check.IfNullOrZero(destFilePath);

            File.Copy(sourceFilePath, destFilePath, true);
        }
        #endregion

        #region 将文件移动到指定目录
        /// <summary>
        /// 将文件移动到指定目录
        /// </summary>
        public static void Move(String sourceFilePath, String descDirectoryPath)
        {
            Check.IfNullOrZero(sourceFilePath);
            Check.IfNullOrZero(descDirectoryPath);

            //获取源文件的名称
            var sourceFileName = GetFileName(sourceFilePath);
            if (IsExistDirectory(descDirectoryPath))
            {
                //如果目标中存在同名文件,则删除
                if (IsExistFile(descDirectoryPath + "\\" + sourceFileName))
                {
                    DeleteFile(descDirectoryPath + "\\" + sourceFileName);
                }
                //将文件移动到指定目录
                File.Move(sourceFilePath, descDirectoryPath + "\\" + sourceFileName);
            }
        }
        #endregion

        #region 从文件的绝对路径中获取文件名( 包含扩展名 )
        /// <summary>
        /// 从文件的绝对路径中获取文件名( 包含扩展名 )
        /// </summary>
        public static String GetFileName(String filePath)
        {
            Check.IfNullOrZero(filePath);
            return new FileInfo(filePath).Name;
        }
        #endregion

        #region 从文件的绝对路径中获取文件名( 不包含扩展名 )
        /// <summary>
        /// 从文件的绝对路径中获取文件名( 不包含扩展名 )
        /// </summary>
        public static String GetFileNameNoExtension(String filePath)
        {
            Check.IfNullOrZero(filePath);
            return new FileInfo(filePath).Name.Split('.')[0];
        }
        #endregion

        #region 从文件的绝对路径中获取扩展名
        /// <summary>
        /// 从文件的绝对路径中获取扩展名
        /// </summary>
        public static String GetExtension(String filePath)
        {
            Check.IfNullOrZero(filePath);
            return new FileInfo(filePath).Extension;
        }
        #endregion

        #region 清空指定目录
        /// <summary>
        /// 清空指定目录下所有文件及子目录,但该目录依然保存.
        /// </summary>
        public static void ClearDirectory(String directoryPath)
        {
            Check.IfNullOrZero(directoryPath);
            if (IsExistDirectory(directoryPath))
            {
                //删除目录中所有的文件
                var fileNames = GetFileNames(directoryPath);
                foreach (var t in fileNames)
                {
                    DeleteFile(t);
                }

                //删除目录中所有的子目录
                var directoryNames = GetDirectories(directoryPath);
                foreach (var t in directoryNames)
                {
                    DeleteDirectory(t);
                }
            }
        }
        #endregion

        #region 删除指定文件
        /// <summary>
        /// 删除指定文件
        /// </summary>
        public static void DeleteFile(String filePath)
        {
            Check.IfNullOrZero(filePath);
            if (IsExistFile(filePath))
            {
                File.Delete(filePath);
            }
        }
        #endregion

        #region 删除指定目录
        /// <summary>
        /// 删除指定目录及其所有子目录
        /// </summary>
        public static void DeleteDirectory(String directoryPath)
        {
            Check.IfNullOrZero(directoryPath);
            if (IsExistDirectory(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }
        }
        #endregion
    }
}
