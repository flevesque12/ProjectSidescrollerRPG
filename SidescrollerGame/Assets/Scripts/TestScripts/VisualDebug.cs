using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestScripts
{

    public class VisualDebug : MonoBehaviour
    {   
        [SerializeField] private float groundDistance = 1.4f;
        [SerializeField] private Transform groundTransform;
        public void OnDrawGizmos() {
            Gizmos.DrawLine(groundTransform.position, new Vector3(groundTransform.position.x, groundTransform.position.y - groundDistance));    
        }
    }
}
