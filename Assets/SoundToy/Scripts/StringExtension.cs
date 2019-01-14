/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// [Unity]カラーコードを変換する(C#)
// https://anz-note.tumblr.com/post/163022237206/unity%E3%82%AB%E3%83%A9%E3%83%BC%E3%82%B3%E3%83%BC%E3%83%89%E3%82%92%E5%A4%89%E6%8F%9B%E3%81%99%E3%82%8Bc
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringExtension
{
    public static Color ToColor(this string self)
    {
        var color = default(Color);
        if (!ColorUtility.TryParseHtmlString(self, out color)) {
            Debug.LogWarning("Unknown color code... " + self);
        }
        return color;
    }
}