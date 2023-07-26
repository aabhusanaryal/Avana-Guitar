using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FretPosDebug : MonoBehaviour
{
    [SerializeField]
    public float gizmosize = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public class Grid : MonoBehaviour
    {
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(gameObject.transform.position, gizmosize);
    }
}
