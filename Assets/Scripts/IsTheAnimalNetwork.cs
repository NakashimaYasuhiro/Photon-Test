using UnityEngine;
using Fusion;

public class IsTheAnimalNetwork : NetworkBehaviour
{
    private GameManager gameManager;
    private bool isReady = false;

    private GameObject succeedPanel;
    private GameObject failPanel;
    [SerializeField] GameObject sphereFailed;

    public void Initialize(GameManager gm, GameObject succeed, GameObject fail)
    {
        gameManager = gm;
        succeedPanel = succeed;
        failPanel = fail;
    }
    public void SetUIReferences(GameObject success, GameObject failure)
    {
        succeedPanel = success;
        failPanel = failure;
    }

    public override void Spawned()
    {
        gameManager = GameManager.Instance;
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_ShowSuccess()
    {
        if (succeedPanel != null)
            succeedPanel.SetActive(true);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_ShowFailure()
    {
        if (failPanel != null)
            failPanel.SetActive(true);
    }
}
