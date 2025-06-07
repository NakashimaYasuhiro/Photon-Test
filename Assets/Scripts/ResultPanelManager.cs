using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanelManager : MonoBehaviour
{
    public static ResultPanelManager Instance;
    //public NetworkGameManager networkGameManager;
    ItemNameHelper helper;

    public bool isCanClick=false;
    public bool isStudentStage = false;

    //パネルシリーズ
    [SerializeField] private GameObject succeedPanel;
    [SerializeField] private GameObject failedPanel;
    [SerializeField] private GameObject firstPanel;
  //  [SerializeField] private GameObject animalPanel;
    [SerializeField] private GameObject waitingPanel;

    [SerializeField] private GameObject correctTexbox;
    [SerializeField] private Text correctTexboxText;

    //ボタンシリーズ
    [SerializeField] private GameObject teacherConer;
    [SerializeField] private GameObject questionButton;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject decideCategoryButton;

    
    [SerializeField] private GameObject areYouReady;
    private Text areYouReadyText;

    //音関係
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip successSound;
    [SerializeField] private AudioClip failureSound;
    [SerializeField] private AudioClip congratulations;
    [SerializeField] private AudioClip areYouReadySound;
    [SerializeField] private AudioClip tenTenTenSound;
    [SerializeField] private AudioClip startSound;


    [SerializeField] private GameObject congratulationsObject;

    public int succeededTimes = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        //animalPanel.SetActive(true);

        firstPanel.SetActive(true);

        succeedPanel.SetActive(false);
        failedPanel.SetActive(false);

        areYouReady.SetActive(false);
        correctTexbox.SetActive(false);


        teacherConer.SetActive(false);
        waitingPanel.SetActive(false);
        nextButton.SetActive(false);
       
    }

    public void SetDecideCategoryButtonText()
    {
        if (decideCategoryButton != null)
        {           
            var textComponent = decideCategoryButton.GetComponentInChildren<Text>();
            
            if (textComponent != null && GameManager.Instance.currentItemPanelData != null)
            {     
                textComponent.text = GameManager.Instance.currentItemPanelData.panelName;
                Text questionText = GameManager.Instance.Question.GetComponent<Text>();
                if (questionText != null)
                {
                    questionText.text = GameManager.Instance.currentItemPanelData.items[0].itemName;
                }
            }
        }
    }


    public void ShowSuccess()
    {
        succeedPanel.SetActive(true);
        failedPanel.SetActive(false);
        congratulationsObject.SetActive(false);
        
        succeededTimes += 1;

        isCanClick = false;

        if (succeededTimes > 0 && succeededTimes % 5 == 0) 
        {
           PlaySound(congratulations);
            congratulationsObject.SetActive(true);


        }
        if (GameManager.Instance.isStudent==false)
        {
            nextButton.SetActive(true);
            startButton.SetActive(false);
        }

        StartCoroutine(SuccessEvent());
    }

    private IEnumerator SuccessEvent()
    {
        PlaySound(successSound);
        yield return new WaitForSeconds(0.7f);
        ShowCorrectTextBox();
        yield return new WaitForSeconds(1.5f);
        HideCorrectTextBox();
    }

    public void ShowFailure()
    {
        succeedPanel.SetActive(false);
        failedPanel.SetActive(true);
        PlaySound(failureSound);
        StartCoroutine(ControllFailedPanel());
    }

    private IEnumerator ControllFailedPanel()
    {
        yield return new WaitForSeconds(1f);
        ShowCorrectTextBox();
        failedPanel.SetActive(false);
        yield return new WaitForSeconds(3f);
        HideCorrectTextBox();
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void StudentStage()
    {
        //Debug.Log("StudentStage");
        firstPanel.SetActive(false);
        //animalPanel.SetActive(true);
        waitingPanel.SetActive(true);
        isStudentStage = true;
        
    }

    public void TeacherStage()
    {
        //Debug.Log("TeacherStage");
        firstPanel.SetActive(false);
        teacherConer.SetActive(true);

        // 初期の問題文を設定（0番目のitemName）
        if (GameManager.Instance.Question != null && GameManager.Instance.currentItemPanelData != null && GameManager.Instance.currentItemPanelData.items.Length > 0)
        {
            Text questionText = GameManager.Instance.Question.GetComponent<Text>();
            if (questionText != null)
            {
                questionText.text = GameManager.Instance.currentItemPanelData.items[0].itemName;
            }
        }

        SetDecideCategoryButtonText();
    }

    
    public void StartGame(int number)
    {
        GameManager.Instance.correctNumber = number;
        Debug.Log("正解のナンバー："+number);
        SyncPanel(GameManager.Instance.panelIndex);

       StartCoroutine(AreYouReady());
        //animalPanel.SetActive(true);
        ShufflePanelChildren(); // ← ここで順番をシャッフル
        isCanClick = true;
        decideCategoryButton.SetActive(false);
       // GameManager.Instance.ChoosePanelAndSync();

    }

    private IEnumerator AreYouReady()
    {
        questionButton.SetActive(false);
        areYouReady.SetActive(true);
        areYouReadyText = areYouReady.GetComponentInChildren<Text>();
        areYouReadyText.text = "Are you ready?";
        waitingPanel.SetActive(false);

        PlaySound(startSound);
        yield return new WaitForSeconds(2f);

        for (int i = 3; i >= 0; i--)
        {

            if (i >= 1)
            {
                areYouReadyText.text = i.ToString();
                PlaySound(tenTenTenSound);
                yield return new WaitForSeconds(1f);
            }
            if (i == 0)
            {
                areYouReadyText.text = "Start!";
                PlaySound(areYouReadySound);
                yield return new WaitForSeconds(1f);
            }

        }
        areYouReady.SetActive(false);


        
    }
    private void ShufflePanelChildren()
    {
        // 子要素を一時的なリストにコピー
        List<Transform> children = new List<Transform>();
        for (int i = 0; i < GameManager.Instance.itemPanelParent.transform.childCount; i++)
        {
            children.Add(GameManager.Instance.itemPanelParent.transform.GetChild(i));
        }

        // 順番をシャッフル
        for (int i = 0; i < children.Count; i++)
        {
            Transform temp = children[i];
            int randomIndex = Random.Range(i, children.Count);
            children[i] = children[randomIndex];
            children[randomIndex] = temp;
        }

        // シャッフル後の順番で再設定（親から一度切り離して再追加）
        foreach (Transform child in children)
        {
            child.SetSiblingIndex(children.IndexOf(child));
        }
    }

    public void NextButton()
    {
        GameManager.Instance.correctNumber = 0;
        areYouReady.SetActive(false);
        startButton.SetActive(true);

        succeedPanel.SetActive(false);
        failedPanel.SetActive(false);
        congratulationsObject.SetActive(false);
        decideCategoryButton.SetActive(true);
        
        nextButton.SetActive(false);
        questionButton.SetActive(true);
        if(isStudentStage)
        {
            waitingPanel.SetActive(false);
        }
        
    }

    //画面所のアイテム一覧を対応するItemDataにあわせて変更する
    public void SetupItemPanel()
    {
        // 既存のボタンをクリア
        foreach (Transform child in GameManager.Instance.itemPanelParent.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < GameManager.Instance.currentItemPanelData.items.Length; i++)
        {
            GameObject buttonObj = Instantiate(GameManager.Instance.itemButtonPrefab, GameManager.Instance.itemPanelParent.transform);
            Image img = buttonObj.GetComponent<Image>();
            Button btn = buttonObj.GetComponent<Button>();
           

            // 表示画像を設定
            img.sprite = GameManager.Instance.currentItemPanelData.items[i].itemImage;

            // クリック処理を割り当て（クロージャ注意）
            int index = i;
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>  GameManager.Instance.OnClickPanel(index));

            // ItemNameHelper に名前を代入
            helper = buttonObj.GetComponent<ItemNameHelper>();
            if (helper != null)
            {
                helper.itemName = GameManager.Instance.currentItemPanelData.items[i].itemName;
            }

        }
    }
    
 
    public void SyncPanel(int panelIndex)
    {
        var panelData = GameManager.Instance.allPanelData[panelIndex];
       // Debug.Log("panelData" + panelData);
        if (panelData == null)
        {
            Debug.LogError("panelDataがnullです！");
            return;
        }

        //Debug.Log("ChoosePanelAndSync 呼び出し");
        //Debug.Log($"currentItemPanelData: {GameManager.Instance.currentItemPanelData?.panelName}");
        /*
        // 選んだインデックスをRPCでブロードキャスト
        if(GameManager.Instance.networkGameManager != null)
        {
            GameManager.Instance.networkGameManager.RPC_SetItemPanel(GameManager.Instance.panelIndex);
        }
        else
        {
            Debug.Log("networkGameManagerはnullのようです");
        }
        */
       
    }
  
    public void SwitchCategory()
    {
        GameManager.Instance.correctNumber = 0;
        GameManager.Instance.panelIndex = (GameManager.Instance.panelIndex + 1) % GameManager.Instance.allPanelData.Length;  // 範囲内でループ
        GameManager.Instance.currentItemPanelData = GameManager.Instance.allPanelData[GameManager.Instance.panelIndex];
        ResultPanelManager.Instance.SyncPanel(GameManager.Instance.panelIndex);
        ResultPanelManager.Instance.SetupItemPanel();

        if (GameManager.Instance.switchCategoryButtonText != null)
        {
            GameManager.Instance.switchCategoryButtonText.text = GameManager.Instance.currentItemPanelData.panelName;
        }
        ResultPanelManager.Instance.SetDecideCategoryButtonText();
    }

    public void ShowCorrectTextBox()
    {
        correctTexbox.SetActive(true);
        correctTexboxText.text = GameManager.Instance.confirmName;

    }

    public void HideCorrectTextBox()
    {
        correctTexbox.SetActive(false);
    }

}
