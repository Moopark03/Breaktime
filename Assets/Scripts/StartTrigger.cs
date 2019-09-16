using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTrigger : MonoBehaviour
{
    [SerializeField] GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.layer = 9;
        } 
    }
}
