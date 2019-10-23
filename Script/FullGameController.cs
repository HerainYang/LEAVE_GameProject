using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FullGameController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject HP_UI;
    public GameObject Intellect_UI;

    public GameObject HP_Sprite_UI;
    public GameObject Intellect_Sprite_UI;

    public GameObject DieInterface;

    private float HP_UI_Length;
    private float Intellect_UI_Length;
    void Start()
    {
        HP_UI_Length = HP_UI.GetComponent<UISprite>().localSize.x;
        Intellect_UI_Length = Intellect_UI.GetComponent<UISprite>().localSize.x;
    }

    // Update is called once per frame
    void Update()
    {
        float HPRemain_Length = HP_UI_Length * (BasicInformation.GetRateHP() - 1f);
        float IntellectRemain_Length = Intellect_UI_Length * (BasicInformation.GetRateIntellect() - 1f);
        //Debug.Log(BasicInformation.GetRateHP());
        HP_Sprite_UI.GetComponent<UISprite>().rightAnchor.absolute = (int)HPRemain_Length;
        Intellect_Sprite_UI.GetComponent<UISprite>().rightAnchor.absolute = (int)IntellectRemain_Length;
        //Debug.Log(BasicInformation.GetRemainHP());
        if(BasicInformation.GetRemainHP()<=0.5)
        {
            
            BasicInformation.IsDie = true;
            BasicInformation.HP = 1;
        }
        if(BasicInformation.Day > 15)
        {
            if (MissionController.CheckMissionState(24) == 1)
            {
                SceneManager.LoadScene("EndVideo");
            }
            else
                Debug.Log("请完成任务");
        }
        if(BasicInformation.Health<=0)
        {
            Debug.Log("Game Over");
            DieInterface.SetActive(true);                                                                                                                                                   
        }
    }
}
