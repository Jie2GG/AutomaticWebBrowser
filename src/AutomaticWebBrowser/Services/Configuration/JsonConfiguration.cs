using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

using AutomaticWebBrowser.Services.Configuration.Attributes;
using AutomaticWebBrowser.Services.Configuration.Converter;
using AutomaticWebBrowser.Services.Configuration.Exceptions;
using AutomaticWebBrowser.Services.Configuration.Models;

namespace AutomaticWebBrowser.Services.Configuration
{
    static class JsonConfiguration
    {
        #region --公开方法--
        /// <summary>
        /// 从文件中加载配置
        /// </summary>
        public static AWConfig Load (FileInfo file)
        {
            if (!file.Exists)
            {
                throw new FileNotFoundException ($"配置文件 ({file.Name}) 加载失败. 原因: 文件不存在", file.Name);
            }
            return JsonSerializer.Deserialize<AWConfig> (file.OpenRead (), Global.DefaultJsonSerializerOptions) ?? throw new ConfigurationException ($"配置文件 ({file.Name}) 加载失败. {Environment.NewLine} 原因: 未知错误");
        }

        /// <summary>
        /// 从文件中加载配置并完全解析
        /// </summary>
        public static AWConfig LoadWithFull (FileInfo file)
        {
            AWConfig config = Load (file);
            ConverterObjects (config);
            return config;
        }
        #endregion

        #region --私有方法--
        private static void ConverterObjects (object? obj)
        {
            if (obj is not null)
            {
                Type type = obj.GetType ();
                if (type == typeof (AWCondition) && obj is AWCondition condition)
                {
                    if (ConverterValue (condition.Type, condition.Value, out object? outObj))
                    {
                        condition.Value = outObj;
                    }
                }
                else if (type == typeof (AWElement) && obj is AWElement element)
                {
                    if (ConverterValue (element.Type, element.Value, out object? outObj))
                    {
                        element.Value = outObj;
                    }
                }
                else if (type == typeof (AWAction) && obj is AWAction action)
                {
                    if (ConverterValue (action.Type, action.Value, out object? outObj))
                    {
                        action.Value = outObj;
                    }
                }
                else
                {
                    foreach (PropertyInfo propertyInfo in type.GetProperties ())
                    {
                        if (propertyInfo.GetCustomAttribute<JsonPropertyNameAttribute> () is not null)
                        {
                            if (propertyInfo.PropertyType.IsArray)
                            {
                                IEnumerable? enumerable = (IEnumerable?)propertyInfo.GetValue (obj, null);
                                if (enumerable is not null)
                                {
                                    foreach (object item in enumerable)
                                    {
                                        ConverterObjects (item);
                                    }
                                }
                            }
                            else
                            {
                                ConverterObjects (propertyInfo.GetValue (obj, null));
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 转换值
        /// </summary>
        private static bool ConverterValue (Enum enumValue, object? sourceObj, out object? outObj)
        {
            outObj = null;
            ObjectConverterAttribute? objectConverterAttribute = GetEnumCustomAttribute<ObjectConverterAttribute> (enumValue);
            if (objectConverterAttribute is not null)
            {
                return ObjectConverterDispatcher.IfConveter (objectConverterAttribute.ConverterType, sourceObj, out outObj);
            }
            return false;
        }

        /// <summary>
        /// 获取枚举成员中指定类型的 <see cref="Attribute"/> 特性
        /// </summary>
        /// <typeparam name="T">要从枚举成员中获取的特性类型</typeparam>
        /// <param name="enumValue">枚举成员的值</param>
        /// <param name="inherit">是否要检索其父类</param>
        /// <returns>匹配 <typeparamref name="T"/> 的自定义属性, 如果没有找到这样的属性, 则为 <see langword="null"/></returns>
        private static T? GetEnumCustomAttribute<T> (Enum enumValue, bool inherit = false)
            where T : Attribute
        {
            string value = enumValue.ToString ();
            FieldInfo? enumField = enumValue.GetType ().GetField (value);
            return enumField?.GetCustomAttribute<T> (inherit);
        }
        #endregion
    }
}
