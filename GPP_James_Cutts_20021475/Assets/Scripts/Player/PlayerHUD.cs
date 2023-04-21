using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    /*[SerializeField] private TextMeshProUGUI currentHealthText;
    [SerializeField] private TextMeshProUGUI maxHealthText;*/

    [SerializeField] private ProgressBar healthBar;

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        /* currentHealthText.text = currentHealth.ToString();
         maxHealthText.text = maxHealth.ToString();*/
        healthBar.SetValues(currentHealth, maxHealth);
    }

}
