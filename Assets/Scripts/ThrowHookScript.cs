using UnityEngine;

using Mirror;

public class ThrowHookScript : NetworkBehaviour
{
    [SerializeField] private GameObject _hookPrefab;

    [SerializeField] private float _hookSpeed = 15f;
    [SerializeField] private float _returnSpeed = 15f;

    [SerializeField] private float _maxDistance = 15f;

    private GameObject _currentHook;
    private GameObject _throwHookPosition;

    private LineRenderer _lineRenderer;
    private TPSController _TPSControllerScript;

    private bool _isReturning = false;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();

        _TPSControllerScript = GetComponent<TPSController>();

        _throwHookPosition = transform.Find("ThrowHookPosition").gameObject;
    }

    private void Update()
    {
        if (!_isReturning && Input.GetMouseButtonDown(0) && _currentHook == null && isLocalPlayer)
        {
            CmdThrowHook();
        }

        if (_currentHook != null)
        {
            if (!_isReturning)
            {
                UpdateLineRenderer();
                MoveHook();
            }
            else
            {
                UpdateLineRenderer();
                ReturnHook();
            }
        }
    }

    [Command]
    private void CmdThrowHook()
    {
        _currentHook = Instantiate(_hookPrefab, _throwHookPosition.transform.position, Quaternion.identity);
        _currentHook.tag = _throwHookPosition.tag;

        Vector3 hookDirection = transform.forward;

        _currentHook.transform.forward = hookDirection;
        _isReturning = false;

        NetworkServer.Spawn(_currentHook);

        RpcDisableController();
    }

    private void UpdateLineRenderer()
    {
        if (_currentHook != null)
        {
            if (isServer)
            {
                RpcUpdateLineRenderer(_throwHookPosition.transform.position, _currentHook.transform.position);
            }
        }
    }

    [ClientRpc]
    private void RpcUpdateLineRenderer(Vector3 startPos, Vector3 endPos)
    {
        if (_lineRenderer != null)
        {
            _lineRenderer.positionCount = 2;

            _lineRenderer.SetPosition(0, startPos);
            _lineRenderer.SetPosition(1, endPos);
        }
    }

    private void MoveHook()
    {
        if (_currentHook == null)
            return;

        float step = _hookSpeed * Time.deltaTime;

        _currentHook.transform.position += _currentHook.transform.forward * step;

        if (Vector3.Distance(_throwHookPosition.transform.position, _currentHook.transform.position) >= _maxDistance)
        {
            _isReturning = true;
        }
    }

    private void ReturnHook()
    {
        if (_currentHook == null)
            return;

        float step = _returnSpeed * Time.deltaTime;

        _currentHook.transform.position = Vector3.MoveTowards(_currentHook.transform.position, _throwHookPosition.transform.position, step);

        if (Vector3.Distance(_currentHook.transform.position, _throwHookPosition.transform.position) < 0.5f)
        {
            _isReturning = false;

            Destroy(_currentHook);

            RpcResetLineRenderer();

            RpcEnableController();

            _currentHook = null;
        }
    }

    [ClientRpc]
    private void RpcResetLineRenderer()
    {
        if (_lineRenderer != null)
        {
            _lineRenderer.positionCount = 0;
        }
    }

    [ClientRpc]
    private void RpcEnableController()
    {
        if (!isLocalPlayer)
            return;

        _TPSControllerScript.enabled = true;
    }

    [ClientRpc]
    private void RpcDisableController()
    {
        if (!isLocalPlayer)
            return;

        _TPSControllerScript.enabled = false;
    }
}