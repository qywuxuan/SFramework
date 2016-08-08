using UnityEngine;
using System.Collections;
using System.Text;
using System;

public static class StringBuilderExtension
{
    public static void TimeAppend(this StringBuilder stringBuilder, int delta, string timeMeasureWord, bool inEnglish = false)
    {
        if (delta > 0)
        {
            stringBuilder.AppendFormat("[{0}]{1}", delta, timeMeasureWord);
            if (inEnglish && delta > 1)
            {
                stringBuilder.Append('s');
            }
        }
    }

    public static void TimeAppend(this StringBuilder stringBuilder, TimeSpan timeSpan, bool inEnglish)
    {
        if(inEnglish)
        {
            stringBuilder.TimeAppend(timeSpan.Days, "day", inEnglish);
            stringBuilder.TimeAppend(timeSpan.Hours, "hour", inEnglish);
            stringBuilder.TimeAppend(timeSpan.Minutes, "minute", inEnglish);
            stringBuilder.TimeAppend(timeSpan.Seconds, "second", inEnglish);
            stringBuilder.TimeAppend(timeSpan.Milliseconds, "millisecond", inEnglish);
        }
        else
        {
            stringBuilder.TimeAppend(timeSpan.Days, "天", inEnglish);
            stringBuilder.TimeAppend(timeSpan.Hours, "时", inEnglish);
            stringBuilder.TimeAppend(timeSpan.Minutes, "分", inEnglish);
            stringBuilder.TimeAppend(timeSpan.Seconds, "秒", inEnglish);
            stringBuilder.TimeAppend(timeSpan.Milliseconds, "毫秒", inEnglish);
        }
    }
}
