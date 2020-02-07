using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapPosHint : MonoBehaviour
{
    public RectTransform hint;
    Image hintImg;
    float alpha = 0f;
    // Start is called before the first frame update
    void Start()
    {
        hintImg = hint.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        alpha = Mathf.Lerp(alpha, 0.6f, Time.deltaTime);
        hintImg.color = new Color(1, 1, 1, alpha);
    }
}
