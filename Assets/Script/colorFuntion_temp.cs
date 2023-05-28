using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorFuntion_temp : MonoBehaviour
{
    public Color aa;
    public SpriteRenderer spt;
    // Start is called before the first frame update
    void Start()
    {
        spt = GetComponent<SpriteRenderer>();


        Color fromColor = new Color(150f, 150f, 100f);
        Color toColor = new Color(100f, 150f, 200f);
        Color toColor2 = new Color(20f, 10f, 255f);

        Color qqq = fromColor * toColor;
        aa = qqq * toColor2;
        spt.color = aa;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
