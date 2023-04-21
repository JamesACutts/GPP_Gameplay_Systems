using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpManager : MonoBehaviour
{
    public bool speedPickUpActive = false;
    public bool jumpPickUpActive = false;
    public bool healthPickUpActive = false;
    public TrailRenderer speedTrail;
    public TrailRenderer jumpTrail;
    private void OnTriggerEnter(Collider other)
    {
        
        if(other.CompareTag("PickUp"))
        {
            Debug.Log("Hit Pickup");

            GameObject pickup = other.gameObject;
            PickUpProperties pickupHit;
            if(pickup.TryGetComponent<PickUpProperties>(out pickupHit))
            {
                if(pickupHit.type == "speed")
                {
                    Debug.Log("Speed Hit");
                    PlayerController playerController;
                    if(TryGetComponent<PlayerController>(out playerController))
                    {
                        playerController.SetSpeed(16f);
                        speedPickUpActive = true;
                        speedTrail.enabled = true;
                    }
                }
                if (pickupHit.type == "jump")
                {
                    jumpPickUpActive = true;
                    jumpTrail.enabled = true;
                }
                if (pickupHit.type == "coin")
                {

                }
                if(pickupHit.type == "health")
                {
                    PlayerStats stats;
                    if(TryGetComponent<PlayerStats> (out stats))
                    {
                        stats.SetHealthTo(100);
                        healthPickUpActive = true;
                    }
                }
                StartCoroutine(SpeedTimer(pickupHit.durationSeconds, pickup));
            }
            
        }
    }
    IEnumerator SpeedTimer(float timeDelay, GameObject obj)
    {
        obj.SetActive(false);

        yield return new WaitForSeconds(timeDelay);

        if(speedPickUpActive)
        {
            speedPickUpActive = false;
            speedTrail.enabled = false;
            PlayerController playerController;
            if (TryGetComponent<PlayerController>(out playerController))
            {
                playerController.SetSpeed(0);
            }
        }
        if(healthPickUpActive)
        {
            healthPickUpActive = false;
        }
        if(jumpPickUpActive)
        {
            jumpPickUpActive = false;
        }
        obj.SetActive(true);
    }
}
