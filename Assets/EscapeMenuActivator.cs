using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EscapeMenuActivator : MonoBehaviour
{

    public Button quit;
    public Button restart;
    public bool EscapeMenuOpen;

    // Start is called before the first frame update
    void Start()
    {
        RectTransform rectTransform = quit.GetComponent<RectTransform>();
        RectTransform rectTransformRestart = restart.GetComponent<RectTransform>();
        //GameObject.FindGameObjectWithTag("menuCanvas").GetComponent<RectTransform>().anchoredPosition = new Vector2((float)1,(float)1);
        rectTransform.anchoredPosition = new Vector2((float)0.5,(float) 0.5);
       // rectTransformRestart.position = new Vector2((float)0.5, (float)0.25);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (EscapeMenuOpen == false)
            {
                //SceneManager.LoadScene(1);
                EscapeMenuOpen = true;
               // GameObject.Find("escape").SetActive(true);
               // GameObject.Find("restart").SetActive(true);

               quit.gameObject.SetActive(true);
               restart.gameObject.SetActive(true);
            }
            else
            {
                //SceneManager.LoadScene(0);
                EscapeMenuOpen = false;
               // GameObject.Find("Quit").SetActive(false);
                //GameObject.Find("Restart").SetActive(false);
                quit.gameObject.SetActive(false);
                restart.gameObject.SetActive(false);
            }
        }
    }

    public void Escape()
    {
        Application.Quit();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }

}
