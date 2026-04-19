using System.Collections.Generic;
using System.ComponentModel;

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
            [Description("等于")]
            Equals,
            /// <summary>
            /// 包含
            /// </summary>
            [Description("包含")]
            Contains,
            /// <summary>
            /// 开头匹配
            /// </summary>
            [Description("开头匹配")]
            StartsWith,
            /// <summary>
            /// 结尾匹配
            /// </summary>
            [Description("结尾匹配")]
            EndsWith,
            /// <summary>
            /// 大于
            /// </summary>
            [Description("大于")]
            GreaterThan,
            /// <summary>
            /// 小于
            /// </summary>
            [Description("小于")]
            LessThan,
            /// <summary>
            /// 大于等于
            /// </summary>
            [Description("大于等于")]
            GreaterThanOrEquals,
            /// <summary>
            /// 小于等于
            /// </summary>
            [Description("小于等于")]
            LessThanOrEquals,
            /// <summary>
            /// 不等于
            /// </summary>
            [Description("不等于")]
            NotEquals,
            /// <summary>
            /// 为空
            /// </summary>
            [Description("为空")]
            IsNull,
            /// <summary>
            /// 不为空
            /// </summary>
            [Description("不为空")]
            IsNotNull,
            /// <summary>
            /// 不包含
            /// </summary>
            [Description("不包含")]
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
            [Description("或")]
            Or,
            /// <summary>
            /// 且
            /// </summary>
            [Description("且")]
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
        /// 字段历史值过滤条件
        /// </summary>
        public class FieldHistoryFilter
        {
            /// <summary>
            /// 字段名称
            /// </summary>
            public string FieldName { get; set; }

            /// <summary>
            /// 关键字
            /// </summary>
            public string Keyword { get; set; }
        }

        /// <summary>
        /// 字段过滤条件集合, 用于复杂查询
        /// </summary>
        public List<FieldFilter> Filters { get; set; } = new List<FieldFilter>();

        /// <summary>
        /// 记录主键, 用于简单查询
        /// </summary>
        public string Id { get; set; }

    }
}
