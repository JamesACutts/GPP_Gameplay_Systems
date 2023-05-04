using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject startPlatform;
    public Vector3 endPosition;
    public float platformDelay = 1f;
    public float platformSpeed = 2f;

    public Transform spawnPoint;

    private GameObject currentPlatform;
    private Vector3 spawnPosition;

    void Start()
    {
        spawnPosition = spawnPoint.position; // assign the position of the spawn point
        StartCoroutine("SpawnPlatform");
    }

    IEnumerator SpawnPlatform()
    {
        yield return new WaitForSeconds(platformDelay);

        GameObject newPlatform = Instantiate(startPlatform, spawnPosition, Quaternion.identity) as GameObject;
        currentPlatform = newPlatform;

        spawnPosition = spawnPoint.position; // always spawn at the spawn point

        StartCoroutine("MovePlatform", currentPlatform);

        // Wait for a delay before spawning the next platform
        yield return new WaitForSeconds(platformDelay);
        StartCoroutine("SpawnPlatform");
    }

    IEnumerator MovePlatform(GameObject platform)
    {
        while (platform.transform.position != endPosition)
        {
            platform.transform.position = Vector3.MoveTowards(platform.transform.position, endPosition, platformSpeed * Time.deltaTime);
            yield return null;
        }

        Destroy(platform);
    }
}