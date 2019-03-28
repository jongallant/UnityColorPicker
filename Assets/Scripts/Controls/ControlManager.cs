using System.Collections.Generic;
using UnityEngine;

public class ControlManager {

    BaseControl SelectedControl;
    public List<BaseControl> Controls;

    Camera Camera;
    RaycastHit2D[] HitsBuffer = new RaycastHit2D[1];
    BaseControl ControlBuffer;

    public ControlManager()
    {
        GetControls();
    }

    private void GetControls()
    {       
        Camera = Camera.main;
        Controls = new List<BaseControl>();

        GameObject[] temp = GameObject.FindGameObjectsWithTag("Slider");

        if (temp != null)
        {
            for (int n = 0; n < temp.Length; n++)
            {
                Controls.Add(temp[n].GetComponent<Slider>());
            }
        }
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.ScreenToWorldPoint(Input.mousePosition);
            int hitCount = Physics2D.RaycastNonAlloc(pos, new Vector2(0, 0), HitsBuffer, 0.01f);

            for (int i = 0; i < hitCount; i++)
            {
                ControlBuffer = CheckCollider(HitsBuffer[i].collider);
                if (ControlBuffer != null)
                {
                    SelectedControl = ControlBuffer;
                    SelectedControl.Select(pos);
                }                
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 pos = Camera.ScreenToWorldPoint(Input.mousePosition);
            int hitCount = Physics2D.RaycastNonAlloc(pos, new Vector2(0, 0), HitsBuffer, 0.01f);

            for (int i = 0; i < hitCount; i++)
            {
                ControlBuffer = CheckCollider(HitsBuffer[i].collider);
                if (ControlBuffer != null)
                {
                    ControlBuffer.Submit();
                }
            }

            if (SelectedControl != null)
            {
                SelectedControl.Reset();
                SelectedControl = null;
            }
        }
    }
    
    private BaseControl CheckCollider(Collider2D collider)
    {
        if (collider.gameObject.tag == "Slider")
        {
            return collider.gameObject.GetComponent<BaseControl>();
        }
        return null;
    }

}
