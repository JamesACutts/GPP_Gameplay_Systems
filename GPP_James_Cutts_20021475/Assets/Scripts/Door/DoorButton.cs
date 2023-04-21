using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DoorButton : MonoBehaviour
{
    public static DoorButton button;
    public PlayerController controller;

    public GameObject door;

    private Animator doorAnim;
    private Animator buttAnim;
    [SerializeField] private TextMeshProUGUI buttonText;

    void Start()
    {
        doorAnim = door.GetComponent<Animator>();
        buttAnim = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            controller.canPress = true;
            controller.isButton = true;
            Debug.Log("Can Press");
            if (!controller.isUsed)
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
            controller.isButton = false;
            Debug.Log("Can Press");
            buttonText.gameObject.SetActive(false);
        }
    }
    public void DoorOpen()
    {
        StartCoroutine(Open(3f));
    }

    IEnumerator Open(float timeDelay)
    {
        controller.cam1.SetActive(false);
        controller.cam2.SetActive(true);
        doorAnim.SetTrigger("Door");
        buttAnim.SetTrigger("Button");
        controller.OnDisable();
        buttonText.gameObject.SetActive(false);

        yield return new WaitForSeconds(timeDelay);

        controller.cam1.SetActive(true);
        controller.cam2.SetActive(false);
        controller.OnEnable();
        buttonText.gameObject.SetActive(true);
    }

}
