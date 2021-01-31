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
    public bool Winner = false;

    [EnumFlags]
    public PackageProperties Properties;

    public int Type = -1;
    public Renderer Renderer;

    private void Start()
    {
        Renderer = GetComponent<Renderer>();
    }
}
