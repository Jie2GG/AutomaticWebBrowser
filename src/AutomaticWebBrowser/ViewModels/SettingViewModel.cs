using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Windows.Input;

using AutomaticWebBrowser.Controls.InputValueDialog.ViewModels;
using AutomaticWebBrowser.Controls.InputValueDialog.Views;
using AutomaticWebBrowser.Models;
using AutomaticWebBrowser.Services.Configuration;
using AutomaticWebBrowser.Services.Configuration.Models;
using AutomaticWebBrowser.Services.Startup;
using AutomaticWebBrowser.Views;

using HandyControl.Controls;
using HandyControl.Tools.Extension;

using Prism.Commands;
using Prism.Mvvm;

using Unity;

namespace AutomaticWebBrowser.ViewModels
{
    class SettingViewModel : BindableBase
    {
        #region --字段--
        private ObservableCollection<FileInfo> configSources;
        private int configCurrentIndex;
        private ObservableCollection<TreeNode> treeNodes;
        #endregion

        #region --属性--
        /// <summary>
        /// 配置文件源
        /// </summary>
        public ObservableCollection<FileInfo> ConfigSource
        {
            get => this.configSources;
            set => this.SetProperty (ref this.configSources, value);
        }

        /// <summary>
        /// 配置文件选择项
        /// </summary>
        public int ConfigCurrentIndex
        {
            get => this.configCurrentIndex;
            set => this.SetProperty (ref this.configCurrentIndex, value);
        }

        /// <summary>
        /// 树节点列表
        /// </summary>
        public ObservableCollection<TreeNode> TreeNodes
        {
            get => this.treeNodes;
            set => this.SetProperty (ref this.treeNodes, value);
        }

        /// <summary>
        /// 启动参数
        /// </summary>
        public BootParameters BootParameters { get; }
        #endregion

        #region --命令--
        /// <summary>
        /// 初始化命令
        /// </summary>
        public ICommand InitializeCommand => new DelegateCommand (() =>
        {
            DirectoryInfo dir = new (Path.GetFullPath (this.BootParameters.ConfigDirectory));
            if (dir.Exists)
            {
                FileInfo[] files = dir.GetFiles ("*.json");
                for (int i = 0; i < files.Length; i++)
                {
                    this.ConfigSource.Add (files[i]);
                    if (string.Compare (files[i].Name, this.BootParameters.ConfigName, true) == 0)
                    {
                        this.ConfigCurrentIndex = i;
                    }
                }

                this.LoadConfig ();
            }
        });

        /// <summary>
        /// 创建新配置命令
        /// </summary>
        public ICommand CreateNewCommand => new DelegateCommand (() =>
        {
            /*
             DialogResult = await Dialog.Show<InteractiveDialog>()
                .Initialize<InteractiveDialogViewModel>(vm => vm.Message = DialogResult)
                .GetResultAsync<string>();
             */
            Dialog.Show<InputValueDialogView> (nameof (SettingView))
                .GetResultAsync<string> ()
                .ContinueWith (task =>
                {
                    // 创建文件

                    // 加入配置列表
                });
        });

        /// <summary>
        /// 切换配置命令
        /// </summary>
        public ICommand SwitchConfigCommand => new DelegateCommand (() =>
        {
            if (this.ConfigCurrentIndex >= 0 && this.ConfigCurrentIndex < this.ConfigSource.Count)
            {
                this.LoadConfig ();
            }
        });
        #endregion

        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="SettingViewModel"/> 类的新实例
        /// </summary>
        public SettingViewModel ([Dependency] BootParameters bootParameters)
        {
            this.BootParameters = bootParameters;

            this.configSources = new ObservableCollection<FileInfo> ();
            this.treeNodes = new ObservableCollection<TreeNode> ();
        }
        #endregion

        #region --私有方法--
        /// <summary>
        /// 加载配置文件
        /// </summary>
        private void LoadConfig ()
        {
            FileInfo fileInfo = this.ConfigSource[this.ConfigCurrentIndex];
            //try
            {
                AWConfig config = JsonConfiguration.LoadWithFull (fileInfo);

                // 创建根节点
                int nodeId = 1;
                TreeNode rootNode = new () { Id = nodeId, ParentId = 0, Name = "config" };

                // 读取属性
                CreateTreeNode (ref nodeId, rootNode, config);

                // 更新树
                this.TreeNodes.Clear ();
                this.TreeNodes.Add (rootNode);
            }
            //catch (Exception ex)
            {
                //MessageBox.Show (ex.Message);
            }
        }

        private static void CreateTreeNode (ref int nodeId, TreeNode rootNode, object instance)
        {
            if (rootNode is null)
            {
                throw new ArgumentNullException (nameof (rootNode));
            }

            if (instance is null)
            {
                throw new ArgumentNullException (nameof (instance));
            }

            PropertyInfo[] propertyInfos = instance.GetType ().GetProperties ();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                if (propertyInfo.MemberType == MemberTypes.Property)
                {
                    // 获取属性的标记的 Json 字段的特性
                    JsonPropertyNameAttribute? jsonPropertyNameAttribute = propertyInfo.GetCustomAttribute<JsonPropertyNameAttribute> ();
                    if (jsonPropertyNameAttribute is not null)
                    {
                        object? value = propertyInfo.GetMethod?.Invoke (instance, null);
                        if (value is not null)
                        {
                            // 创建一个 Node 用于承载当前属性
                            TreeNode node = new ()
                            {
                                Id = ++nodeId,
                                ParentId = rootNode.Id,
                                Name = jsonPropertyNameAttribute.Name,
                            };

                            Type valueType = value.GetType ();
                            if (!valueType.IsArray)
                            {
                                node.Value = value;

                                // 获取值属性的值, 然后递归获取值的树结构
                                if (value is not null)
                                {
                                    CreateTreeNode (ref nodeId, node, value);
                                }
                            }
                            else
                            {
                                if (value is IEnumerable enumable)
                                {
                                    int i = 0;
                                    foreach (object item in enumable)
                                    {
                                        // 获取属性之前, 应该先创建一个 Node 用于承载属性
                                        TreeNode itemNode = new ()
                                        {
                                            Id = ++nodeId,
                                            ParentId = rootNode.Id,
                                            Name = $"{jsonPropertyNameAttribute.Name}[{i++}]",
                                            Value = item
                                        };

                                        if (itemNode.Value is not null)
                                        {
                                            // 获取子属性
                                            CreateTreeNode (ref nodeId, itemNode, itemNode.Value);
                                        }

                                        node.Children.Add (itemNode);
                                    }
                                }
                            }
                            rootNode.Children.Add (node);
                        }
                    }
                }
            }
        }
        #endregion
    }
}
