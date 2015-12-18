using System.Collections.Generic;
using UnityEngine;

public class ControlManager {

    BaseControl SelectedControl;
    public List<BaseControl> Controls;

    public ControlManager()
    {
        GetControls();
    }

    private void GetControls()
    {
        Controls = new List<BaseControl>();

        GameObject[] temp = GameObject.FindGameObjectsWithTag("Slider");

        if (temp != null)
        {
            for (int n = 0; n < temp.Length; n++)
            {
                Slider slider = temp[n].GetComponent<Slider>();
                if (!Controls.Contains(slider))
                    Controls.Add(slider);
            }
        }
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(pos, new Vector2(0, 0), 0.01f);

            for (int i = 0; i < hits.Length; i++)
            {
                BaseControl control = CheckCollider(hits[i].collider);
                if (control != null)
                {
                    SelectedControl = hits[i].collider.gameObject.GetComponent<BaseControl>();
                    SelectedControl.Select(pos);
                }                
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(pos, new Vector2(0, 0), 0.01f);

            for (int i = 0; i < hits.Length; i++)
            {
                BaseControl control = CheckCollider(hits[i].collider);
                if (control != null)
                {
                    control.Submit();
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
