namespace AutomaticWebBrowser.Services.Configuration.Models
{
    /// <summary>
    /// 条件类型
    /// </summary>
    enum ConditionType
    {
        /// <summary>
        /// 以 XMLHTTP 请求的状态作为条件判断的依据
        /// </summary>
        Ready,
        /// <summary>
        /// 以超时时间作为当前条件判断的依据
        /// </summary>
        Timeout,
    }
}
