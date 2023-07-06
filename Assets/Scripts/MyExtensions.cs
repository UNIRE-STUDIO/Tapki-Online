using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class MyExtensions
{
    public static Vector3 Parse(this Vector3 v, string str)
    {
        StringBuilder strBuilder = new StringBuilder();
        Vector3 vec = new Vector3();
        for (int i = 1; i < str.Length; i++)
        {
            if (i == str.Length - 1)
            {
                strBuilder.Append(str[i]);
                vec.z = float.Parse(strBuilder.ToString());
            }
            else if (str[i] == 'z')
            {
                vec.y = float.Parse(strBuilder.ToString());
                strBuilder.Clear();
            }
            else if (str[i] == 'y')
            {
                vec.x = float.Parse(strBuilder.ToString());
                strBuilder.Clear();
            }
            else
            {
                strBuilder.Append(str[i]);
            }
        }
        return vec;
    }

    public static string ToString(this Vector3 v)
    {
        return $"x{Math.Round(v.x, 1)}y{Math.Round(v.y, 1)}z{Math.Round(v.z, 1)}";
    }

}
