using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    //public string musicName;
    public void LoadGameScene()
    {
        //AudioManager.Instance.PlayMusic(musicName);
        SceneManager.LoadScene("");
    }
       
        
}
