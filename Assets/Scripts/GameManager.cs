using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject succeedPanel;
    [SerializeField] private GameObject failedPanel;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject GetSucceedPanel() => succeedPanel;
    public GameObject GetFailedPanel() => failedPanel;


}
