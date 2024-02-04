using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "General Methods", menuName = "General Methods", order = 0)]
public class GeneralMethods : ScriptableObject{
    public void LoadScene(string sceneName){
        SceneManager.LoadScene(sceneName);
    }
    
    public void OpenUrl(string url){
        Application.OpenURL(url);
    }
}