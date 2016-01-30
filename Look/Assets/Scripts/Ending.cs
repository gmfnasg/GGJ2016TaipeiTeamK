using UnityEngine;
using System.Collections;

public class Ending : MonoBehaviour 
{
    public float y_end = 0.0f;
    public float speed = 10.0f;

    RectTransform m_loadingRectTransform;

    // Use this for initialization
    void Awake () 
    {
        m_loadingRectTransform = this.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void FixedUpdate () 
    {

        //text's new position
        float x = 0;
        float y = 0;

        y += speed * Time.deltaTime;
    
        if(m_loadingRectTransform.anchoredPosition3D.y < y_end)
        {
            m_loadingRectTransform.Translate(new Vector3(x, y, 0));
        }

        print(y);
    }
}
