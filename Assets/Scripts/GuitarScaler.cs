using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuitarScaler : MonoBehaviour
{
    [SerializeField]
    public Transform fretStart;
    [SerializeField]
    public Transform fretEnd;

    [SerializeField]
    public Transform fretTop;
    [SerializeField]
    public Transform fretBot;



    [SerializeField]
    public GameObject[] strings;
    private void Awake()
    {
        Vector3 scale = transform.localScale;
        scale.x = (Camera.main.aspect) / (16.0f / 9.0f);
        transform.localScale = scale;
    }
    // Start is called before the first frame update
    void Start()
    {
        float y = fretTop.position.y;
        float diff = (fretTop.position.y - fretBot.position.y) / 5.0f;
        float Ly = fretTop.position.y;
        float Ldiff = (fretTop.position.y - fretBot.position.y) / 5.0f;
        for (int i = 0; i < 6; i++)
        {
            var colliders = strings[i].GetComponents<Collider2D>();
            foreach (var collider in colliders)
            {
                var tx = collider.offset.x;
                collider.offset = new Vector2(tx,0);
            }
            var pos = strings[i].transform.position;
            pos.y = y;
            strings[i].transform.position = pos;
            var LR = strings[i].GetComponent<LineRenderer>();
            for (int j = 0; j < LR.positionCount; j++)
            {
                var Lpos = LR.GetPosition(j);
                Lpos.y = Ly;
                LR.SetPosition(j, Lpos);
            }
            y -= diff;
            Ly -= Ldiff;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
