using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] Vector3 Position;
    [SerializeField] Quaternion Oriantation;
    [SerializeField] float SpawnHeightAdjustment;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 yAdjustment = new Vector3(0, SpawnHeightAdjustment, 0);
            GameManager.instance.UpdatePlayerSpawnPos(Position + yAdjustment, Oriantation);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Destroy(this);
        }
    }
}
