using System;
using System.Collections.Generic;
using System.Text;

namespace Wangk.Base
{
    /// <summary>
    /// 系统日志信息
    /// </summary>
    public class SystemEventLog
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 系统名称
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// 事件名称
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// 事件明细
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// 设备 Id
        /// </summary>
        public string DeviceId { get; set; }

    }
}
