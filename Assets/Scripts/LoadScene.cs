using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string musicName;
    public void LoadGameScene()
    {
        AudioManagment.Instance.PlayMusic(musicName);
        SceneManager.LoadScene("GymAngie");
    }
       
        
}
