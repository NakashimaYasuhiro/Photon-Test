using UnityEngine;
using UnityEngine.UI;
using static Unity.Collections.Unicode;

public class FirstCondition : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] ResultPanelManager resultPanelManager;
    public void OnClickStudent()
    {

    
            gameManager.isStudent = true;
            //Debug.Log(gameManager.isStudent);
            resultPanelManager.StudentStage();

            // 通常の処理
 

    }
    public void OnClickTeacher()
    {
        gameManager.isStudent = false;
        //Debug.Log(gameManager.isStudent);
        resultPanelManager.TeacherStage();
        
    }
}
