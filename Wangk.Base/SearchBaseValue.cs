using System;
using System.Collections.Generic;
using System.Text;

namespace Wangk.Base
{
    /// <summary>
    /// 基础搜索信息
    /// </summary>
    [Obsolete("请使用 SearchFilter 类替代 SearchBaseValue 类")]
    public class SearchBaseValue
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }
    }
}
