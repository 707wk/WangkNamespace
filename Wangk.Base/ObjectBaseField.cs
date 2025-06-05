using System;
using System.Collections.Generic;
using System.Text;

namespace Wangk.Base
{
    /// <summary>
    /// 对象基本字段
    /// </summary>
    public class ObjectBaseField
    {
        /// <summary>
        /// 数据主键
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }
    }
}
