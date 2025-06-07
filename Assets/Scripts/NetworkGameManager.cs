using Fusion;
using Unity.VisualScripting;
using UnityEngine;

public class NetworkGameManager : NetworkBehaviour
{
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SetPanelIndexAndSetup(int panelIndex)
    {
        GameManager.Instance.currentItemPanelData = GameManager.Instance.allPanelData[panelIndex];
        ResultPanelManager.Instance.SwitchCategory();
        ResultPanelManager.Instance.SyncPanel(panelIndex);
        ResultPanelManager.Instance.SetupItemPanel();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_ShowSuccess()
    {
        ResultPanelManager.Instance.ShowSuccess();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_ShowFailure()
    {
        ResultPanelManager.Instance.ShowFailure();
    }

    //どのアイテムが正解かを決める
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_DecideButton(int number) 
    {
       GameManager.Instance.correctNumber= number;

        
       // Debug.Log("RPC_DecideButtonのGameManager.Instance.correctNumber"+GameManager.Instance.correctNumber);


    }
    [Rpc(RpcSources.All, RpcTargets.All)]

    public void RPC_Next()
    {
        ResultPanelManager.Instance.NextButton();
    }
  

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_StartGameWithNumber(int number)
    {        
       
       ResultPanelManager.Instance.StartGame(number);
    }
    /*
     public override void Spawned()
     {
         if (HasStateAuthority)
         {
            ResultPanelManager.Instance.ChoosePanelAndSync();
         }
     }
    

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SyncPanel(int panelIndex)
    {

        ResultPanelManager.Instance.SyncPanel(panelIndex);
      
    }
   

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SetItemPanel(int panelIndex)
    {
        GameManager.Instance.currentItemPanelData = GameManager.Instance.allPanelData[panelIndex];

        //各アイテムボタンのクローン
        ResultPanelManager.Instance.SetupItemPanel(); // 各クライアントでパネルを表示
    }
   */


    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SwitchCategory()
    {

        ResultPanelManager.Instance.SwitchCategory();

    }
}