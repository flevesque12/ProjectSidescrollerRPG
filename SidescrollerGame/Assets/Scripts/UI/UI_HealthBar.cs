using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private Entity entity;
    private RectTransform rectTransform;
    private Slider slider;
    private CharacterStats stats;


    private void Start()
    {
        entity = GetComponentInParent<Entity>();
        rectTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();
        stats = GetComponentInParent<CharacterStats>();

        entity.OnFlipped += FlipUI;
        stats.onHealthChanged += UpdateHealthUI;

        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = stats.GetMaxHealthValue();
        slider.value = stats.currentHealth;
    }

    private void FlipUI() => rectTransform.Rotate(0, 180, 0);

    private void OnDisable() 
    { 
        entity.OnFlipped -= FlipUI; 
        stats.onHealthChanged -= UpdateHealthUI;
    } 
    
}