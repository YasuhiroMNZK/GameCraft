using UnityEngine;

public class Clear : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private bool clearOnStart = true;

    public void ClearChildren()
    {
        Transform root = target != null ? target : transform;

        for (int i = root.childCount - 1; i >= 0; i--)
        {
            Destroy(root.GetChild(i).gameObject);
        }
    }
}
