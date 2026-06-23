using UnityEngine;
using UnityEngine.Events;

public class Spawn : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabsToSpawn;
    [SerializeField] private Transform parentObject;

    [Header("Random Local Transform")]
    [SerializeField] private bool useRandomLocalPosition = false;
    [SerializeField] private Vector3 minLocalPosition = Vector3.zero;
    [SerializeField] private Vector3 maxLocalPosition = Vector3.zero;
    [SerializeField] private bool useRandomLocalRotation = false;
    [SerializeField] private Vector3 minLocalEuler = Vector3.zero;
    [SerializeField] private Vector3 maxLocalEuler = Vector3.zero;
    
    // イベント定義
    [SerializeField] private UnityEvent spawnEvent;

    /// <summary>
    /// 指定されたPrefabを指定されたオブジェクトの子として生成する
    /// </summary>
    /// <param name="prefab">生成するPrefab</param>
    /// <param name="parentTransform">親となるTransform</param>
    /// <returns>生成されたGameObject</returns>
    public GameObject InstantiatePrefabAsChild(GameObject prefab, Transform parentTransform)
    {
        if (prefab == null)
        {
            Debug.LogError("Prefabが指定されていません");
            return null;
        }

        if (parentTransform == null)
        {
            Debug.LogError("親のTransformが指定されていません");
            return null;
        }

        GameObject instance = Instantiate(prefab, parentTransform);
        return instance;
    }

    /// <summary>
    /// 指定されたPrefabを指定されたオブジェクトの子として生成（位置と回転指定）
    /// </summary>
    /// <param name="prefab">生成するPrefab</param>
    /// <param name="parentTransform">親となるTransform</param>
    /// <param name="localPosition">ローカル位置</param>
    /// <param name="localRotation">ローカル回転</param>
    /// <returns>生成されたGameObject</returns>
    public GameObject InstantiatePrefabAsChild(GameObject prefab, Transform parentTransform, 
        Vector3 localPosition, Quaternion localRotation)
    {
        if (prefab == null)
        {
            Debug.LogError("Prefabが指定されていません");
            return null;
        }

        if (parentTransform == null)
        {
            Debug.LogError("親のTransformが指定されていません");
            return null;
        }

        GameObject instance = Instantiate(prefab, parentTransform);
        instance.transform.localPosition = localPosition;
        instance.transform.localRotation = localRotation;
        return instance;
    }

    /// <summary>
    /// 複数のPrefabの中からランダムに1つを選択する
    /// </summary>
    /// <returns>ランダムに選択されたPrefab</returns>
    private GameObject GetRandomPrefab()
    {
        if (prefabsToSpawn == null || prefabsToSpawn.Length == 0)
        {
            Debug.LogError("Prefabが指定されていません");
            return null;
        }

        int randomIndex = Random.Range(0, prefabsToSpawn.Length);
        return prefabsToSpawn[randomIndex];
    }

    /// <summary>
    /// ランダムに選択したPrefabを親オブジェクトの子として生成
    /// </summary>
    public GameObject SpawnRandomPrefab()
    {
        GameObject randomPrefab = GetRandomPrefab();
        if (randomPrefab != null && parentObject != null)
        {
            if (useRandomLocalPosition || useRandomLocalRotation)
            {
                return InstantiatePrefabAsChild(
                    randomPrefab,
                    parentObject,
                    GetRandomLocalPosition(),
                    GetRandomLocalRotation());
            }

            return InstantiatePrefabAsChild(randomPrefab, parentObject);
        }
        return null;
    }

    private Vector3 GetRandomLocalPosition()
    {
        if (!useRandomLocalPosition)
        {
            return Vector3.zero;
        }

        return new Vector3(
            Random.Range(minLocalPosition.x, maxLocalPosition.x),
            Random.Range(minLocalPosition.y, maxLocalPosition.y),
            Random.Range(minLocalPosition.z, maxLocalPosition.z));
    }

    private Quaternion GetRandomLocalRotation()
    {
        if (!useRandomLocalRotation)
        {
            return Quaternion.identity;
        }

        Vector3 randomEuler = new Vector3(
            Random.Range(minLocalEuler.x, maxLocalEuler.x),
            Random.Range(minLocalEuler.y, maxLocalEuler.y),
            Random.Range(minLocalEuler.z, maxLocalEuler.z));

        return Quaternion.Euler(randomEuler);
    }

    /// <summary>
    /// イベントを発火させて、ランダムなPrefabを生成
    /// </summary>
    public void TriggerSpawn()
    {
        SpawnRandomPrefab();
        spawnEvent?.Invoke();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
