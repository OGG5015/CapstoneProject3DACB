using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class achmanager : MonoBehaviour
{

    public GameObject achievementPrefab;

   

    
    void Start()
    {
        CreateAchievement("General");
    }

  

    public void CreateAchievement(string category)
    {
        GameObject achievement = (GameObject)Instantiate(achievementPrefab);

        SetAchInfo(category,achievement);

    }
    public void SetAchInfo(string category, GameObject achievement)
    {
        achievement.transform.SetParent(GameObject.Find(category).transform);
        achievement.transform.localScale = new Vector3(1, 1, 1);
       // achievement.transform.GetChild(0).GetComponent<Text>().text = title;
       // achievement.transform.GetChild(1).GetComponent<Text>().text = description;
    }
    public void ChangeCategory(GameObject button)
    {
        
    }


}
