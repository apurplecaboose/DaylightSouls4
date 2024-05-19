using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class StringCleanup : MonoBehaviour
{
    void Start()
    {
        string input = "B0__B1152__Chat_Eric_used_chatGPT";
        string output = BossEnumStringCleanup(input);
        Debug.Log(output);

        string input1 = "B0__B1152__Chat_Eric_used_chatGPT";
        int output1 = ExtractNumberFromInput(input1);
        Debug.Log(output1);
    }

    public string BossEnumStringCleanup(string inputString)
    {
        string cleanedString = Regex.Replace(inputString, @"^[^_]*__", "");

        cleanedString = cleanedString.Replace("_", " ");

        return cleanedString;
    }

    public int ExtractNumberFromInput(string inputString)
    {
        Match match = Regex.Match(inputString, @"B(\d+)");
        if (match.Success)
        {
            return int.Parse(match.Groups[1].Value);
        }
        else
        {
            Debug.LogError("null");
            return -1;
        }
    }
}
