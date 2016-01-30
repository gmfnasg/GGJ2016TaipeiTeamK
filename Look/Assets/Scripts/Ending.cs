using UnityEngine;

public class Ending : MonoBehaviour 
{
    public float y_end = 0.0f;
    public float speed = 10.0f;

    RectTransform m_loadingRectTransform;
    
    void Awake () 
    {
        m_loadingRectTransform = this.GetComponent<RectTransform>();
    }
    
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
