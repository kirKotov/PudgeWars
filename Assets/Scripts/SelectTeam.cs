using System.Collections.Generic;
using UnityEngine;

public class SelectTeam : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;

    [SerializeField] private List<GameObject> _radiantSpawnPoints;
    [SerializeField] private List<GameObject> _direSpawnPoints;

    private GameObject _currentPlayer;

    private Camera _playerCamera;

    public void RadiantSelected()
    {
        GameObject spawnPoint = _radiantSpawnPoints[Random.Range(0, _radiantSpawnPoints.Count)];

        _currentPlayer = InstantiatePlayer(spawnPoint);
        _currentPlayer.tag = "Radiant";

        SetTagToChildObject(_currentPlayer, "ThrowHookPosition", "Radiant");

        SwitchCameraToPlayer();
    }

    public void DireSelected()
    {
        GameObject spawnPoint = _direSpawnPoints[Random.Range(0, _direSpawnPoints.Count)];

        _currentPlayer = InstantiatePlayer(spawnPoint);
        _currentPlayer.tag = "Dire";

        SetTagToChildObject(_currentPlayer, "ThrowHookPosition", "Dire");

        SwitchCameraToPlayer();
    }

    private GameObject InstantiatePlayer(GameObject spawnPoint)
    {
        GameObject player = Instantiate(_playerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);

        _playerCamera = player.GetComponentInChildren<Camera>();

        return player;
    }

    private void SwitchCameraToPlayer()
    {
        _playerCamera.enabled = true;
        Camera.main.enabled = false;
    }

    private void SetTagToChildObject(GameObject parentObject, string childObjectName, string tag)
    {
        Transform childTransform = parentObject.transform.Find(childObjectName);

        childTransform.gameObject.tag = tag;
    }
}