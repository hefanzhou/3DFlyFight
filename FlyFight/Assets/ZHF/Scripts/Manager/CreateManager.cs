using UnityEngine;
using System.Collections;

public class CreateManager : MonoBehaviour {
    public GameObject[] dontDestoryGOs;
    void Awake()
    {
        foreach (var go in dontDestoryGOs)
        {
            if (GameObject.Find("/" + go.name) == null)
            {
                var goTemp = Instantiate(go) as GameObject;
                goTemp.name = go.name;
                DontDestroyOnLoad(goTemp);
            }
        }
    }

  

}
