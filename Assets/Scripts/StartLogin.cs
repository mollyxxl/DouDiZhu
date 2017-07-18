using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartLogin : MonoBehaviour {

    private GameObject gameController;
    public Button btnEasy;
    public Button btnNormal;
	// Use this for initialization
	void Start () {
        gameController = GameObject.Find("GameController");
        BindListenEvent();
	}

    private void BindListenEvent()
    {
        if (btnEasy != null)
        {
            btnEasy.onClick.AddListener(() =>
            {
                SwitchScene();
                var contr= gameController.AddComponent<GameController>();
                contr.MatchType = 0;
                Debug.Log("this is Easy Scene");
            });
        }
        if (btnNormal != null)
        {
            btnNormal.onClick.AddListener(() =>
            {
                SwitchScene();
                var contr = gameController.AddComponent<GameController>();
                contr.MatchType = 1;
                SceneManager.LoadScene("Main");
                Debug.Log("this is Normal Scene");
            });
        }
    }

    private void SwitchScene()
    {
        if (gameController != null)
            DontDestroyOnLoad(gameController);
        SceneManager.LoadScene("Main");
        var star = gameController.GetComponent<StartLogin>();
        Destroy(star);
    }
}
