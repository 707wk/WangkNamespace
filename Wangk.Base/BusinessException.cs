using System;
using System.Collections.Generic;

namespace Wangk.Base
{
    /// <summary>
    /// 业务异常
    /// </summary>
    public class BusinessException : Exception
    {
        /// <summary>
        /// 资源的键名
        /// </summary>
        public string ErrorKey { get; }
        /// <summary>
        /// 格式化参数
        /// </summary>
        public Dictionary<string, object> Parameters { get; }

        /// <summary>
        /// 多个参数的构造函数
        /// </summary>
        public BusinessException(string errorKey, Dictionary<string, object> parameters = null)
        : base(errorKey)
        {
            ErrorKey = errorKey;
            Parameters = parameters ?? new Dictionary<string, object>();
        }

        /// <summary>
        /// 单个参数的构造函数
        /// </summary>
        public BusinessException(string errorKey, string paramName, object paramValue)
            : this(errorKey, new Dictionary<string, object> { { paramName, paramValue } })
        {
        }
    }
}
