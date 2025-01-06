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
        #region 获取安全的文件名
        /// <summary>
        /// 获取安全的文件名
        /// </summary>
        /// <param name="fileName">文件名</param>
        public static string GetSafeFileName(this string fileName)
        {
            foreach (char s in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(s, '_');
            }

            foreach (char s in Path.GetInvalidPathChars())
            {
                fileName = fileName.Replace(s, '_');
            }

            if (fileName.EndsWith("."))
            {
                fileName = fileName.Replace('.', '_');
            }

            return fileName;
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
