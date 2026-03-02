using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace Wangk.Base
{
    /// <summary>
    /// 搜索过滤器
    /// </summary>
    public class SearchFilter
    {
        #region 过滤条件比较运算符
        /// <summary>
        /// 过滤条件比较运算符
        /// </summary>
        public enum SearchFilterCompareOperator
        {
            /// <summary>
            /// 等于
            /// </summary>
            Equals,
            /// <summary>
            /// 包含
            /// </summary>
            Contains,
            /// <summary>
            /// 开头匹配
            /// </summary>
            StartsWith,
            /// <summary>
            /// 结尾匹配
            /// </summary>
            EndsWith,
            /// <summary>
            /// 大于
            /// </summary>
            GreaterThan,
            /// <summary>
            /// 小于
            /// </summary>
            LessThan,
            /// <summary>
            /// 大于等于
            /// </summary>
            GreaterThanOrEquals,
            /// <summary>
            /// 小于等于
            /// </summary>
            LessThanOrEquals,
            /// <summary>
            /// 不等于
            /// </summary>
            NotEquals,
            /// <summary>
            /// 为空
            /// </summary>
            IsNull,
            /// <summary>
            /// 不为空
            /// </summary>
            IsNotNull,
            /// <summary>
            /// 不包含
            /// </summary>
            NotContains
        }
        #endregion

        #region 过滤条件拼接运算符
        /// <summary>
        /// 过滤条件拼接运算符
        /// </summary>
        public enum SearchFilterCondition
        {
            /// <summary>
            /// 或
            /// </summary>
            Or,
            /// <summary>
            /// 且
            /// </summary>
            And
        }
        #endregion

        /// <summary>
        /// 字段过滤条件
        /// </summary>
        public class FieldFilter
        {
            /// <summary>
            /// 字段名称
            /// </summary>
            public string FieldName { get; set; }

            /// <summary>
            /// 数据类型, string, datetime, int, bool, decimal
            /// </summary>
            public string DataType { get; set; }

            /// <summary>
            /// 字段过滤条件项集合
            /// </summary>
            public List<FieldFilterItem> Items { get; set; } = new List<FieldFilterItem>();
        }

        /// <summary>
        /// 字段过滤条件项
        /// </summary>
        public class FieldFilterItem
        {
            /// <summary>
            /// 值
            /// </summary>
            public string Value { get; set; }
            /// <summary>
            /// 比较运算符
            /// </summary>
            public SearchFilterCompareOperator CompareOperator { get; set; }
            /// <summary>
            /// 拼接运算符
            /// </summary>
            public SearchFilterCondition Condition { get; set; }
        }

        /// <summary>
        /// 字段过滤条件集合
        /// </summary>
        public List<FieldFilter> Filters { get; set; } = new List<FieldFilter>();

    }
}
