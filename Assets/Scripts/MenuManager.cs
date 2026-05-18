using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip click;
    [SerializeField] private GameObject instructions;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(string levelName)
    {
        StartCoroutine(LoadSceneCoroutine(levelName));
    }
    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        audioSource.PlayOneShot(click);
        if (click != null)
        {
            yield return new WaitForSecondsRealtime(click.length);
        }
        else
        {
            yield return null;
        }
        if (string.IsNullOrEmpty(sceneName))
        {
            sceneName = SceneManager.GetActiveScene().name;
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    //loads the original game scene from the game jam
    public void LoadGame() //this will eventually be an outdated method 
    {
        StartCoroutine(LoadGameCoroutine());
    }
    private IEnumerator LoadGameCoroutine()
    {
        audioSource.PlayOneShot(click);
        if (click != null)
        {
            yield return new WaitForSeconds(click.length);
        }
        else
        {
            yield return null;
        }
        SceneManager.LoadScene("MainScene");
    }
    public void ShowInstruct()
    {
        StartCoroutine(PlaySound());
        instructions.SetActive(true);
    }
    public void HideInstruct()
    {
        StartCoroutine(PlaySound());
        instructions.SetActive(false);
    }
    public IEnumerator PlaySound()
    {
        audioSource.PlayOneShot(click);
        if (click != null)
        {
            yield return new WaitForSeconds(click.length);
        }
        else
        {
            yield return null;
        }
        
    }
}
