using System.Collections.Generic;
using System.Linq;
using System.Collections;

using UnityEngine;

using Mirror;

public class SelectTeam : NetworkBehaviour
{
    [SerializeField] private GameObject _playerPrefab;

    [SerializeField] private Camera _startCamera;

    [SerializeField] private List<GameObject> _radiantSpawnPoints;
    [SerializeField] private List<GameObject> _direSpawnPoints;

    private List<CharacterController> _characterControllers;

    private GameObject _playerGameObject;

    private string _teamSelected;

    private void Start()
    {
        StartCoroutine(FindPlayerObject());
    }

    public void RadiantSelected()
    {
        if (_playerGameObject.GetComponent<NetworkBehaviour>().isLocalPlayer)
        {
            GameObject spawnPoint = _radiantSpawnPoints[Random.Range(0, _radiantSpawnPoints.Count)];

            _teamSelected = "Radiant";

            SpawnPlayer(spawnPoint, _teamSelected);

        }
    }

    public void DireSelected()
    {
        if (_playerGameObject.GetComponent<NetworkBehaviour>().isLocalPlayer)
        {
            GameObject spawnPoint = _direSpawnPoints[Random.Range(0, _direSpawnPoints.Count)];

            _teamSelected = "Dire";

            SpawnPlayer(spawnPoint, _teamSelected);
        }
    }

    private IEnumerator FindPlayerObject()
    {
        while (_playerGameObject == null)
        {
            _characterControllers = FindObjectsOfType<CharacterController>().ToList();

            foreach (CharacterController characterController in _characterControllers)
            {
                if (characterController.gameObject.GetComponent<NetworkBehaviour>().isOwned)
                    _playerGameObject = characterController.gameObject;
            }
            yield return new WaitForFixedUpdate();
        }

        _playerGameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void SpawnPlayer(GameObject spawnPoint, string tag)
    {
        _playerGameObject.transform.position = spawnPoint.transform.position;
        _playerGameObject.transform.rotation = spawnPoint.transform.rotation;

        _playerGameObject.tag = tag;

        SetTagToChildObject(_playerGameObject, "ThrowHookPosition", tag);

        SwitchCameraToPlayer(_playerGameObject, tag);
    }

    private void SetTagToChildObject(GameObject parentObject, string childObjectName, string tag)
    {
        Transform childTransform = parentObject.transform.Find(childObjectName);
        childTransform.gameObject.tag = tag;
    }

    private void SwitchCameraToPlayer(GameObject player, string tag)
    {
        player.SetActive(true);
        _startCamera.enabled = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}