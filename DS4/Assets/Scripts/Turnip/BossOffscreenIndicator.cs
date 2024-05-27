using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossOffscreenIndicator : MonoBehaviour
{
    /// <summary>
    /// Attach as a child of the boss gameobject. (the gameobject with this script on it must have a sprite renderer with the indicator on it)
    /// </summary>

    Transform _BossTransform;
    Camera _Cam;
    SpriteRenderer _SR;
    Vector3 _ScaleCache;
    private void Awake()
    {
        _Cam = Camera.main;
        _SR = this.GetComponent<SpriteRenderer>();

        _BossTransform = transform.parent;
        _ScaleCache = transform.localScale;
    }
    void Update()
    {
        float distance = Vector3.Distance(this.transform.position, _BossTransform.position);
        MoveIndicator();
        if (distance < 1)
        {
            this.transform.localScale = Vector3.zero; //bad code alert lel, sets transform to zero when indicator is offscreen to make it invisible
        }
        else
        {
            //no need to run these functions if distance check returns too close 
            RotateIndicator();
            this.transform.localScale = _ScaleCache;
        }
    }

    void MoveIndicator()
    {
        Vector3 topR_screenWorldSpace = _Cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        Vector3 botL_screenWorldSpace = _Cam.ScreenToWorldPoint(Vector3.zero);
        float screenedge_offset = 0.75f;
        Vector2 x_Clamp = new Vector2(botL_screenWorldSpace.x + screenedge_offset, topR_screenWorldSpace.x - screenedge_offset);
        Vector2 y_Clamp = new Vector2(botL_screenWorldSpace.y + screenedge_offset, topR_screenWorldSpace.y - screenedge_offset);

        this.transform.position = new Vector3(Mathf.Clamp(_BossTransform.position.x, x_Clamp.x, x_Clamp.y), Mathf.Clamp(_BossTransform.position.y, y_Clamp.x, y_Clamp.y), 0);
    }
    void RotateIndicator()
    {
        Vector2 direction = _BossTransform.position - _Cam.transform.position;
        transform.up = -direction;
    }
}
