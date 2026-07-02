using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;

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
        /// [已废弃] 通用透传参数（单值）。仅旧接口使用，2026-07月及之后的新接口请通过 Filters 传参。
        /// 绑定为数据库对应的参数（如 Oracle :Id），仅加入参数集，不生成 WHERE 条件
        /// </summary>
        [Obsolete("请改用 Parameters 字典传递多个透传参数", false)]
        public string Id { get; set; }

        /// <summary>
        /// 通用透传参数集合。所有键值对仅加入参数集，不生成 WHERE 条件。
        /// 由后端从 Filters 提权填充，前端不应直接传递。
        /// Key = 参数名, 框架自动添加 Oracle : 或 SQL Server @ 前缀。
        /// </summary>
        [JsonIgnore]
        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// 将指定字段从 Filters 提权到 Parameters。提权后该字段仅绑参，不生成 WHERE 条件。
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <returns>提取的参数原始值（字符串），未找到则返回 null</returns>
        public string PromoteFilterToParameter(string fieldName)
        {
            var filter = Filters.FirstOrDefault(f => f.FieldName == fieldName);
            var value = filter?.Items?.FirstOrDefault()?.Value;
            if (filter != null)
                Filters.Remove(filter);
            if (value != null)
                Parameters[fieldName] = ConvertValue(filter.DataType, value);
            return value;
        }

        /// <summary>
        /// 按数据类型转换字符串值
        /// </summary>
        internal static object ConvertValue(string dataType, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            switch (dataType?.ToLower())
            {
                case "int":
                case "integer":
                    return int.Parse(value);
                case "decimal":
                    return decimal.Parse(value);
                case "bool":
                case "boolean":
                    return value.Trim().ToLower() == "true" ? 1 : 0;
                case "datetime":
                    return DateTime.Parse(value);
                default:
                    return value;
            }
        }

    }
}
