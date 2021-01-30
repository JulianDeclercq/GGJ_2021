using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagTester : MonoBehaviour
{
    private Package _package;
    // Start is called before the first frame update
    void Start()
    {
        _package = GetComponent<Package>();
    }

    // Update is called once per frame
    void Update()
    {
        if((_package.Properties & PackageProperties.Magnetic) == PackageProperties.Magnetic)
        {
            print("magnetic");
        }
        else
        {
            print("NOT magnetic");
        }

        if ((_package.Properties & PackageProperties.Flammable) == PackageProperties.Flammable)
        {
            print("Flammable");
        }
        else
        {
            print("NOT Flammable");
        }
    }
}
