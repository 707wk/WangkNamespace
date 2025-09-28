using System;
using System.Collections.Generic;
using System.Text;

namespace Wangk.Base
{
    /// <summary>
    /// 提交的数据, 用于钉钉调用外部接口时的单个参数封装
    /// </summary>
    public class PostData<T>
    {
        /// <summary>
        /// 提交的实体数据
        /// </summary>
        public T Data { get; set; }
    }
}
