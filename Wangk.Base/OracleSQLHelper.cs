using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using static Wangk.Base.SearchFilter;

namespace Wangk.Base
{
    /// <summary>
    /// Oracle SQL 帮助类
    /// </summary>
    public class OracleSQLHelper
    {
        #region Oracle SQL 安全性验证
        /// <summary>
        /// Oracle SQL 安全性验证
        /// </summary>
        public static void ValidateSQLSafety(string sql)
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
        public static (string WhereClause, Dictionary<string, object> Parameters) BuildWhereClauseAndParameters(SearchFilter filter)
        {
            var parameters = new Dictionary<string, object>();

            if (!string.IsNullOrWhiteSpace(filter.Id))
            {
                parameters.Add(":Id", filter.Id);
            }

            if (filter?.Filters is null || !filter.Filters.Any())
            {
                return (string.Empty, parameters);
            }

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

        private static readonly HashSet<string> OracleKeywords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
{
    "ACCESS", "ADD", "ALL", "ALTER", "AND", "ANY", "AS", "ASC", "AUDIT", "BETWEEN", "BY", "CHAR", "CHECK", "CLUSTER", "COLUMN", "COMMENT", "COMPRESS", "CONNECT", "CREATE", "CURRENT", "DATE", "DECIMAL", "DEFAULT", "DELETE", "DESC", "DISTINCT", "DROP", "ELSE", "EXCLUSIVE", "EXISTS", "FILE", "FLOAT", "FOR", "FROM", "GRANT", "GROUP", "HAVING", "IDENTIFIED", "IMMEDIATE", "IN", "INCREMENT", "INDEX", "INITIAL", "INSERT", "INTEGER", "INTERSECT", "INTO", "IS", "LEVEL", "LIKE", "LOCK", "LONG", "MAXEXTENTS", "MINUS", "MLSLABEL", "MODE", "MODIFY", "NOAUDIT", "NOCOMPRESS", "NOT", "NOWAIT", "NULL", "NUMBER", "OF", "OFFLINE", "ON", "ONLINE", "OPTION", "OR", "ORDER", "PCTFREE", "PRIOR", "PRIVILEGES", "PUBLIC", "RAW", "RENAME", "RESOURCE", "REVOKE", "ROW", "ROWID", "ROWNUM", "ROWS", "SELECT", "SESSION", "SET", "SHARE", "SIZE", "SMALLINT", "START", "SUCCESSFUL", "SYNONYM", "SYSDATE", "TABLE", "THEN", "TO", "TRIGGER", "UID", "UNION", "UNIQUE", "UPDATE", "USER", "VALIDATE", "VALUES", "VARCHAR", "VARCHAR2", "VIEW", "WHENEVER", "WHERE", "WITH"
};

        /// <summary>
        /// 转义 Oracle 关键字
        /// </summary>
        private static string EscapeOracleIdentifier(string fieldName)
        {
            if (fieldName.Contains('"'))
                return fieldName;

            var parts = fieldName.Split('.');
            for (int i = 0; i < parts.Length; i++)
            {
                string part = parts[i];
                if (string.IsNullOrWhiteSpace(part))
                    continue;

                if (OracleKeywords.Contains(part))
                {
                    parts[i] = "\"" + part + "\"";
                }
            }
            return string.Join(".", parts);
        }

        private static string BuildFieldFilterItemExpression(string fieldName, string dataType, SearchFilter.FieldFilterItem item, ref int paramIndex, Dictionary<string, object> parameters)
        {
            fieldName = EscapeOracleIdentifier(fieldName);

            if (item.CompareOperator == SearchFilterCompareOperator.IsNull)
            {
                return $"{fieldName} is null";
            }
            if (item.CompareOperator == SearchFilterCompareOperator.IsNotNull)
            {
                return $"{fieldName} is not null";
            }

            if (dataType.Equals("string", StringComparison.OrdinalIgnoreCase) && string.IsNullOrWhiteSpace(item.Value))
            {
                if (item.CompareOperator == SearchFilterCompareOperator.Equals)
                    return $"{fieldName} is null";
                if (item.CompareOperator == SearchFilterCompareOperator.NotEquals)
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
                    return $"{fieldName} LIKE '%' || :{paramName} ESCAPE '\\'";
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
                case "string":
                default:
                    return value;
            }
        }

        #endregion

        /// <summary>
        /// 生成 SELECT 语句
        /// </summary>
        public static string BuildSelectSql(object entity, string schema = null)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Type type = entity.GetType();
            string tableName = type.Name;

            // 获取所有公共实例属性（排除索引器）
            var allProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.GetIndexParameters().Length == 0)
                .ToList();

            // 找出带有 [Key] 特性的属性（主键）
            var keyProperties = allProperties
                .Where(p => p.IsDefined(typeof(KeyAttribute), inherit: true))
                .ToList();

            if (keyProperties.Count == 0)
                throw new InvalidOperationException("实体类型没有使用 [Key] 特性标记的主键属性，无法生成 SELECT 语句。");

            string fullTableName = string.IsNullOrWhiteSpace(schema) ? tableName : $"{schema}.{tableName}";

            var sql = new StringBuilder();
            sql.Append($"SELECT * FROM {fullTableName} WHERE ");

            var whereClauses = keyProperties.Select(p => $"{p.Name} = :{p.Name}");
            sql.Append(string.Join(" AND ", whereClauses));

            return sql.ToString();
        }

        /// <summary>
        /// 生成 INSERT 语句
        /// </summary>
        public static string BuildInsertSql(object entity, string schema = null)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Type type = entity.GetType();
            string tableName = type.Name; // 类名作为表名

            // 获取所有可写的公共实例属性（排除索引器）
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite && p.GetIndexParameters().Length == 0)
                .ToList();

            if (properties.Count == 0)
                throw new InvalidOperationException("类型没有可写的属性，无法生成INSERT语句。");

            // 构建字段列表和参数列表（不加引号）
            var columnNames = properties.Select(p => p.Name).ToList();
            var paramNames = properties.Select(p => $":{p.Name}").ToList();

            var sql = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(schema))
            {
                sql.Append($"INSERT INTO {schema}.{tableName} (");
            }
            else
            {
                sql.Append($"INSERT INTO {tableName} (");
            }

            sql.Append(string.Join(",", columnNames));
            sql.Append(") VALUES (");
            sql.Append(string.Join(",", paramNames));
            sql.Append(")");

            return sql.ToString();
        }

        /// <summary>
        /// 生成 UPDATE 语句
        /// </summary>
        public static string BuildUpdateSql(object entity, string schema = null)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Type type = entity.GetType();
            string tableName = type.Name; // 类名作为表名

            // 获取所有公共实例属性（包括只读，用于查找主键）
            var allProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.GetIndexParameters().Length == 0) // 排除索引器
                .ToList();

            // 找出带有 [Key] 特性的属性（主键）
            var keyProperties = allProperties
                .Where(p => p.IsDefined(typeof(KeyAttribute), inherit: true))
                .ToList();

            if (keyProperties.Count == 0)
                throw new InvalidOperationException("实体类型没有使用 [Key] 特性标记的主键属性，无法生成 UPDATE 语句。");

            // 获取可写的属性（用于 SET 子句）
            var writableProperties = allProperties
                .Where(p => p.CanWrite)
                .ToList();

            // 用于 SET 的属性 = 可写属性 - 主键属性（通常不更新主键）
            var setProperties = writableProperties
                .Except(keyProperties)
                .ToList();

            if (setProperties.Count == 0)
                throw new InvalidOperationException("没有可更新的非主键属性，无法生成 UPDATE 语句。");

            var sql = new StringBuilder();

            // 构建表名（带可选的 schema）
            string fullTableName = string.IsNullOrWhiteSpace(schema) ? tableName : $"{schema}.{tableName}";
            sql.Append($"UPDATE {fullTableName} SET ");

            // 构建 SET 子句：column = :column
            var setClauses = setProperties.Select(p => $"{p.Name} = :{p.Name}");
            sql.Append(string.Join(", ", setClauses));

            // 构建 WHERE 子句：主键条件用 AND 连接
            var whereClauses = keyProperties.Select(p => $"{p.Name} = :{p.Name}");
            sql.Append(" WHERE ");
            sql.Append(string.Join(" AND ", whereClauses));

            return sql.ToString();
        }

        /// <summary>
        /// 生成 DELETE 语句
        /// </summary>
        public static string BuildDeleteSql(object entity, string schema = null)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Type type = entity.GetType();
            string tableName = type.Name;

            var allProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.GetIndexParameters().Length == 0)
                .ToList();

            var keyProperties = allProperties
                .Where(p => p.IsDefined(typeof(KeyAttribute), inherit: true))
                .ToList();

            if (keyProperties.Count == 0)
                throw new InvalidOperationException("实体类型没有使用 [Key] 特性标记的主键属性，无法生成 DELETE 语句。");

            string fullTableName = string.IsNullOrWhiteSpace(schema) ? tableName : $"{schema}.{tableName}";

            var sql = new StringBuilder();
            sql.Append($"DELETE FROM {fullTableName} WHERE ");

            var whereClauses = keyProperties.Select(p => $"{p.Name} = :{p.Name}");
            sql.Append(string.Join(" AND ", whereClauses));

            return sql.ToString();
        }
    }
}
