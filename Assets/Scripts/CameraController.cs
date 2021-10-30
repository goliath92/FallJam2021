using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float cameraSpeed;
    
    private float lookAhead;
    private float yDistance = -1;
    

   private void LateUpdate()
   {
       transform.position = new Vector3(player.position.x + lookAhead, player.position.y + yDistance, transform.position.z);
       lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.x), Time.deltaTime * cameraSpeed);
   }
    
    
}
