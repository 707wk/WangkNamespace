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

        #region 转义 Markdown 特殊字符
        /// <summary>
        /// Markdown需要转义的特殊字符
        /// </summary>
        private static readonly char[] _markdownSpecialChars =
            { '\\', '`', '*', '_', '{', '}', '[', ']', '(', ')',
          '#', '+', '-', '.', '!', '|', '~', '>', '<' };

        /// <summary>
        /// HTML特殊字符及其对应实体
        /// </summary>
        private static readonly (string Char, string Entity)[] _htmlSpecialChars =
        {
        ("&", "&amp;"),
        ("<", "&lt;"),
        (">", "&gt;"),
        ("\"", "&quot;"),
        ("'", "&apos;"),
        ("©", "&copy;"),
        ("®", "&reg;"),
        ("™", "&trade;"),
        (" ", "&nbsp;")
    };

        /// <summary>
        /// 转义字符串中的Markdown特殊字符和HTML特殊字符
        /// </summary>
        /// <param name="input">要转义的字符串</param>
        /// <param name="escapeHtml">是否转义HTML特殊字符</param>
        /// <returns>转义后的字符串</returns>
        public static string EscapeMarkdown(this string input, bool escapeHtml = true)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            var sb = new StringBuilder(input.Length * 2);

            foreach (char c in input)
            {
                // 转义Markdown特殊字符
                if (Array.IndexOf(_markdownSpecialChars, c) >= 0)
                {
                    sb.Append('\\');
                }

                // 转义HTML特殊字符
                if (escapeHtml)
                {
                    var entity = GetHtmlEntity(c.ToString());
                    sb.Append(entity ?? c.ToString());
                }
                else
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 将HTML特殊字符转换为实体
        /// </summary>
        private static string GetHtmlEntity(string character)
        {
            foreach (var (Char, Entity) in _htmlSpecialChars)
            {
                if (Char == character)
                {
                    return Entity;
                }
            }
            return null;
        }
        #endregion

    }
}
