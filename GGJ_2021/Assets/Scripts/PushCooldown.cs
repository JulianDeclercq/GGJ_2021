using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PushCooldown : MonoBehaviour
{
    private Image _image;
    void Start()
    {
        _image = GetComponent<Image>();
    }

    void Update()
    {
        _image.fillAmount = GameManager.Instance.CooldownPercentage;
    }
}
