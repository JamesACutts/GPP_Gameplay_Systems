using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Lever : MonoBehaviour
{
    public static Lever lever;
    public PlayerController controller;

    public GameObject door;

    private Animator doorAnim;
    private Animator leverAnim;
    [SerializeField] private TextMeshProUGUI buttonText;

    void Start()
    {
        doorAnim = door.GetComponent<Animator>();
        leverAnim = GetComponentInChildren<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            controller.canPress = true;
            controller.isLever = true;
            Debug.Log("Can Press");
            if (!controller.isLeverUsed)
            {
                buttonText.gameObject.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            controller.canPress = false;
            controller.isLever = false;
            Debug.Log("Can Press");
            buttonText.gameObject.SetActive(false);
        }
    }
    public void DoorOpen()
    {
        StartCoroutine(Open(3.5f));
    }

    IEnumerator Open(float timeDelay)
    {
        controller.cam1.SetActive(false);
        controller.cam3.SetActive(true);
        doorAnim.SetTrigger("Door");
        leverAnim.SetTrigger("Lever");
        controller.OnDisable();
        buttonText.gameObject.SetActive(false);

        yield return new WaitForSeconds(timeDelay);

        controller.cam1.SetActive(true);
        controller.cam3.SetActive(false);
        controller.OnEnable();
        buttonText.gameObject.SetActive(true);
    }

}
