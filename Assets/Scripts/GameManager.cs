using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public NetworkGameManager networkGameManager;
    public int correctNumber = 0;
    public bool isStudent = true;
    public int panelIndex=0;

    public GameObject Question;
    public string confirmName;


    //以下はScriptableObjectによるPanel管理方法
    public ItemPanelData currentItemPanelData;
    public GameObject itemPanelParent;
    public GameObject itemButtonPrefab;
    public ItemPanelData[] allPanelData;
    public Text switchCategoryButtonText;

   


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
           // Debug.LogWarning("Multiple GameManager instances detected. Destroying duplicate.");
            Destroy(this);
        }
    }

    private void Start()
    {
       ResultPanelManager.Instance.SetupItemPanel();
    }



    public void OnClickPanel(int number)
    {
        if (!ResultPanelManager.Instance.isCanClick) return;
        confirmName = currentItemPanelData.items[number].itemName;
        Debug.Log(confirmName);
        if (number == correctNumber)
        {
            //Debug.Log("OnClickPanel:"+number);
            networkGameManager.RPC_ShowSuccess();
           //Debug.Log(currentItemPanelData.items[number].itemName);
        }
        else
        {
          //  Debug.Log("失敗");
            networkGameManager.RPC_ShowFailure();
            //Debug.Log("OnClickPanel:" + number);
           // Debug.Log(currentItemPanelData.items[number].itemName);
        }
    }

    //回答の決定
    public void OnClickDecideButton()
    {
        
        int itemCount = currentItemPanelData.items.Length;
        // 現在のパネルのアイテム数に応じて正解番号を切り替える
        correctNumber =(correctNumber + 1) % itemCount;
        networkGameManager.RPC_DecideButton(correctNumber);

        Text questionText = Question.GetComponent<Text>();

        if (correctNumber >= 0 && correctNumber < currentItemPanelData.items.Length)
        {
            questionText.text = currentItemPanelData.items[correctNumber].itemName;
        }
        else
        {
            questionText.text = "Unknown";
        }
       // Debug.Log("correctNumber"+correctNumber);
    }

    public void OnClickStartGame()
    {
       networkGameManager.RPC_StartGameWithNumber(correctNumber);
    }

    public void OnClickNext()
    {
        networkGameManager.RPC_Next();
    }



    public void OnClickSwitchCategory()
    {
        networkGameManager.RPC_SetPanelIndexAndSetup(panelIndex);
       //networkGameManager.RPC_SwitchCategory();

    }
    }

