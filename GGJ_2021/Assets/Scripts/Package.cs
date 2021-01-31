using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumFlagsAttribute : PropertyAttribute
{
}

[Flags]
public enum PackageProperties
{
    None = 0,
    Floats = 1 << 0,
    Flammable = 1 << 1, 
    Explosive = 1 << 2,
    Electronic = 1 << 3,
    Glass = 1 << 4,
    Bouncy = 1 << 5,
    Magnetic = 1 << 6,
}

public class Package : MonoBehaviour
{
    [EnumFlags]
    public PackageProperties Properties;

    public bool Winner = false;
}
