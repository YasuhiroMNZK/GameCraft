using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    [SerializeField] private int maxHP = 5; // 最大血量
    [SerializeField] private GameObject heartPrefab; // 血量图片的预制体
    [SerializeField] private Transform heartContainer; // 血量图片的容器

    private int currentHP; // 当前血量
    private GameObject[] hearts; // 血量图片的数组

    void Start()
    {
        // 初始化血量
        currentHP = maxHP;

        // 创建血量图片
        hearts = new GameObject[maxHP];
        for (int i = 0; i < maxHP; i++)
        {
            hearts[i] = Instantiate(heartPrefab, heartContainer);
        }
    }

    // 扣血逻辑
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0); // 确保血量不会低于 0
        UpdateHearts();
    }

    // 更新血量图片显示
    private void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < currentHP); // 当前血量以内的图片显示，其他隐藏
        }
    }
}