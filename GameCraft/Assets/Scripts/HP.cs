using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HP : MonoBehaviour
{
    [SerializeField] private int maxHP = 5; 
    [SerializeField] private GameObject heartPrefab; 
    [SerializeField] private Transform heartContainer;
    [SerializeField] private UnityEvent onDeath;

    private int currentHP; 
    private GameObject[] hearts; 
    private bool isDead = false;

    void Start()
    {
      
        currentHP = maxHP;

      
        hearts = new GameObject[maxHP];
        for (int i = 0; i < maxHP; i++)
        {
            hearts[i] = Instantiate(heartPrefab, heartContainer);
        }
    }

    
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0); 
        UpdateHearts();
        if (currentHP == 0 && !isDead)
        {
            isDead = true;
            if (onDeath != null) onDeath.Invoke();
        }
    }


    private void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < currentHP); 
        }
    }
}