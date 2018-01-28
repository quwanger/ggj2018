using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour {

    public EventSystem ES;
    private GameObject StoreSelected;

    void Start()
    {
        StoreSelected = ES.firstSelectedGameObject;
    }

    void Update()
    {
        if (ES.currentSelectedGameObject != StoreSelected)
        {
            if (ES.currentSelectedGameObject == null)
            {
                ES.SetSelectedGameObject(StoreSelected);
            }
            else
            {
                StoreSelected = ES.currentSelectedGameObject;
            }
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
