using UnityEngine;
using System.Collections;
using System.Text;
public class LogMan {

    public static void print(params  object[] logs)
    {
        StringBuilder allLogStr = new StringBuilder();
        int length = logs.Length;
        for (int i = 0; i < length; i++)
        {
            allLogStr.Append(logs[i]).Append("@");
        }

        Debug.Log(allLogStr);
    }
}
