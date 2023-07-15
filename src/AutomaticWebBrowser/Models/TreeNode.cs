using System.Collections.ObjectModel;

namespace AutomaticWebBrowser.Models
{
    /// <summary>
    /// 树节点模型
    /// </summary>
    class TreeNode
    {
        #region --属性--
        /// <summary>
        /// 节点唯一标识
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 节点的夫节点标识
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 当前树的节点名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 当前树节点的子节点
        /// </summary>
        public ObservableCollection<TreeNode> Children { get; set; } = new ObservableCollection<TreeNode> ();

        /// <summary>
        /// 获取当前树节点的值
        /// </summary>
        public object? Value { get; set; }
        #endregion
    }
}
