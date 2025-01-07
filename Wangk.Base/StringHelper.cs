using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Wangk.Base
{
    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static class StringHelper
    {
        #region 获取安全的文件/文件夹名
        /// <summary>
        /// 获取安全的文件/文件夹名
        /// </summary>
        /// <param name="name">文件/文件夹名</param>
        public static string GetSafeFileOrFolderName(this string name)
        {
            name= name.Trim();

            foreach (char s in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(s, '_');
            }

            foreach (char s in Path.GetInvalidPathChars())
            {
                name = name.Replace(s, '_');
            }

            while (name.EndsWith("."))
            {
                name = name.Remove(name.Length - 1);
            }

            return name;
        }
        #endregion

        #region 保留 Markdown 的转义字符
        /// <summary>
        /// 保留 Markdown 的转义字符
        /// </summary>
        public static string ToMarkdownText(this string text)
        {
            return text.Replace("*", "\\*");
        }
        #endregion

    }
}
