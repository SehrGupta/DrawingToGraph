using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class Constants
{
    public static Dictionary<Function, Color> FunctionColors = new Dictionary<Function, Color>()
    {
        { Function.Empty, Color.white },
        { Function.Kitchen, new Color(55, 93, 126) },
        { Function.LivingRoom, new Color(104, 227, 189) },
        { Function.Bathroom, new Color(198, 109, 134) },
        { Function.Bedroom, new Color(255, 100, 132) },
        { Function.Closet, new Color(239, 131, 41) },
        { Function.Courtyard, new Color(255, 206, 88) },
        { Function.Dining, new Color(114, 90, 122) }
    };
}