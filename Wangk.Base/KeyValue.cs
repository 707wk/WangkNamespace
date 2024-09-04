using System;
using System.Collections.Generic;
using System.Text;

namespace Wangk.Base
{
    /// <summary>
    /// 键值对, struct 类型使用系统自带的 KeyValuePair
    /// </summary>
    public class KeyValue<TKey, TValue>
    {
        /// <summary>
        /// 无参构造
        /// </summary>
        public KeyValue()
        {
        }

        /// <summary>
        /// 带参构造
        /// </summary>
        public KeyValue(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// 键
        /// </summary>
        public TKey Key { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public TValue Value { get; set; }
    }
}
