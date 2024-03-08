using Mirror;

using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    private NetworkManager _networkManager;
    public void ConnectToGame()
    {
        SceneManager.LoadScene(1);
    }
}