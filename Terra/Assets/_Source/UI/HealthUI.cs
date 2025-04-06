using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Terra.Combat;
using Terra.Player;

public class HeartUI : MonoBehaviour
{
    [SerializeField] private GameObject heartPrefab; 
    [SerializeField] private Sprite heartSprite;    

    private HealthController healthController;
    private List<Image> hearts = new();  

    private void Start()
    {
        healthController = PlayerManager.Instance.HealthController;

        healthController.OnHealthChanged += OnHealthChanged;

        CreateHearts();
        UpdateHearts(healthController.CurrentHealth);
    }

    private void OnDestroy()
    {
        if (healthController != null)
            healthController.OnHealthChanged -= OnHealthChanged;
    }

    private void CreateHearts()
    {
        int heartCount = Mathf.CeilToInt(healthController.MaxHealth); 

        for (int i = 0; i < heartCount; i++)
        {
            GameObject heart = Instantiate(heartPrefab, transform);
            Image image = heart.GetComponent<Image>();
            hearts.Add(image);
        }
    }

    private void OnHealthChanged(float currentHealth)
    {
        UpdateHearts(currentHealth);
    }

    private void UpdateHearts(float currentHealth)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        int fullHearts = Mathf.FloorToInt(currentHealth); 

        for (int i = 0; i < fullHearts; i++)
        {
            GameObject heart = Instantiate(heartPrefab, transform); 
            Image image = heart.GetComponent<Image>();
            image.sprite = heartSprite;
        }
    }
}
