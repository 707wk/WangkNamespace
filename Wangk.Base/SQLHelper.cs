using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using static Wangk.Base.SearchFilter;

namespace Wangk.Base
{
    /// <summary>
    /// SQL 帮助类
    /// </summary>
    public class SQLHelper
    {
        #region Oracle SQL 安全性验证
        /// <summary>
        /// Oracle SQL 安全性验证
        /// </summary>
        public static void ValidateOracleSqlSafety(string sql)
        {
            if (sql.Length > 10000)
            {
                throw new System.Exception("SQL语句过长");
            }

            var dangerousPatterns = new[]
            {
                @"\b(INSERT|UPDATE|DELETE|DROP|CREATE|ALTER|TRUNCATE)\b",
                @"\b(EXEC|EXECUTE|xp_cmdshell)\b"
            };

            var upperSql = sql.ToUpper();

            foreach (var pattern in dangerousPatterns)
            {
                if (Regex.IsMatch(upperSql, pattern, RegexOptions.IgnoreCase))
                {
                    throw new System.Exception("SQL语句包含不安全操作");
                }
            }
        }
        #endregion

        #region 创建 Oracle where 子句和参数, 不含 where
        /// <summary>
        /// 创建 Oracle where 子句和参数, 不含 where
        /// </summary>
        /// <param name="filter">搜索过滤器</param>
        public static (string WhereClause, Dictionary<string, object> Parameters) BuildOracleWhereClauseAndParameters(SearchFilter filter)
        {
            if (filter?.Filters is null || !filter.Filters.Any())
            {
                return (string.Empty, new Dictionary<string, object>());
            }

            var parameters = new Dictionary<string, object>();

            var paramList = new List<string>();
            var paramIndex = 0;

            foreach (var filteItem in filter.Filters)
            {
                if (filteItem.Items is null || !filteItem.Items.Any())
                {
                    continue;
                }

                var filteExpression = string.Empty;

                foreach (var item in filteItem.Items)
                {
                    var FieldFilterItemExpression = BuildFieldFilterItemExpression(filteItem.FieldName, filteItem.DataType, item, ref paramIndex, parameters);

                    if (string.IsNullOrWhiteSpace(filteExpression))
                    {
                        filteExpression = FieldFilterItemExpression;
                    }
                    else
                    {
                        string logicalOp = item.Condition == SearchFilter.SearchFilterCondition.Or ? " or " : " and ";
                        filteExpression = $"{filteExpression} {logicalOp} {FieldFilterItemExpression}";
                    }

                }

                if (!string.IsNullOrWhiteSpace(filteExpression))
                {
                    paramList.Add($"({filteExpression})");
                }
            }

            return (paramList.Any() ? " and " + string.Join(" and ", paramList) : string.Empty, parameters);
        }

        private static string BuildFieldFilterItemExpression(string fieldName, string dataType, SearchFilter.FieldFilterItem item, ref int paramIndex, Dictionary<string, object> parameters)
        {
            if (item.CompareOperator == SearchFilterCompareOperator.IsNull)
            {
                return $"{fieldName} is null";
            }
            if (item.CompareOperator == SearchFilterCompareOperator.IsNotNull)
            {
                return $"{fieldName} is not null";
            }


            string paramName = $"wp{paramIndex++}";
            parameters.Add(paramName, ConvertValue(dataType, item.Value));

            switch (item.CompareOperator)
            {
                case SearchFilter.SearchFilterCompareOperator.Equals:
                    return $"{fieldName} = :{paramName}";
                case SearchFilter.SearchFilterCompareOperator.Contains:
                    return $"{fieldName} LIKE '%' || :{paramName} || '%' ESCAPE '\\'";
                case SearchFilter.SearchFilterCompareOperator.StartsWith:
                    return $"{fieldName} LIKE :{paramName} || '%' ESCAPE '\\'";
                case SearchFilter.SearchFilterCompareOperator.EndsWith:
                    return $"{fieldName} LIKE '%' || :{paramName}";
                case SearchFilter.SearchFilterCompareOperator.GreaterThan:
                    return $"{fieldName} > :{paramName}";
                case SearchFilter.SearchFilterCompareOperator.LessThan:
                    return $"{fieldName} < :{paramName}";
                case SearchFilter.SearchFilterCompareOperator.GreaterThanOrEquals:
                    return $"{fieldName} >= :{paramName}";
                case SearchFilter.SearchFilterCompareOperator.LessThanOrEquals:
                    return $"{fieldName} <= :{paramName}";
                case SearchFilter.SearchFilterCompareOperator.NotEquals:
                    return $"{fieldName} <> :{paramName}";
                case SearchFilter.SearchFilterCompareOperator.NotContains:
                    return $"{fieldName} NOT LIKE '%' || :{paramName} || '%' ESCAPE '\\'";
                default:
                    throw new NotSupportedException($"不支持的比较运算符: {item.CompareOperator}");
            }
        }

        private static object ConvertValue(string dataType, string value)
        {
            if (string.IsNullOrEmpty(value))
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
                case "string":
                default:
                    return value;
            }
        }

        #endregion

    }
}
