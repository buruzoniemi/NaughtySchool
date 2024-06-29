using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * Scene名の列挙型
 * [Description("Scene名 + Scene"](アセットと同名) Scene名
 * 
 */
public enum Name
{
    [Description("TitleScene")] Title = 0,
    [Description("ModeSelectScene")] ModeSelect = 1,
    [Description("TutorialScene")] Tutorial = 2,
    [Description("InGameScene")] InGame = 3,
    [Description("ResultScene")] Result = 4
}
public static class Scene_EnumManager
{
    /// <summary>
    /// 列挙体フィールドのDescriptionを取得する。
    /// </summary>
    /// <param name="value">列挙体値</param>
    /// <returns>Description文字列</returns>
    public static string GetSceneName<T>(T value) where T : Enum
    {
        string description = string.Empty;

        if (value != null)
        {
            string strValue = value.ToString();

            description = strValue;

            if (strValue.Length > 0)
            {
                FieldInfo? fieldInfo = typeof(T).GetField(strValue);
                if (fieldInfo != null)
                {
                    Attribute? attribute = Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute));
                    if (attribute != null)
                    {
                        DescriptionAttribute descriptionAttribute = (DescriptionAttribute)attribute;
                        description = descriptionAttribute.Description;
                    }
                }

            }
        }

        return description;
    }
}


