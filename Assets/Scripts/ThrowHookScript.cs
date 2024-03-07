using UnityEngine;

public class ThrowHookScript : MonoBehaviour
{
    [SerializeField] private GameObject _hookPrefab;

    [SerializeField] private float _hookSpeed = 15f;
    [SerializeField] private float _returnSpeed = 15f;

    [SerializeField] private float _maxDistance = 15f;

    private GameObject _currentHook;

    private LineRenderer _lineRenderer;

    private TPSController _TPSControllerScript;

    private GameObject _throwHookPosition;

    private bool _isReturning = false;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();

        _TPSControllerScript = GetComponent<TPSController>();

        _throwHookPosition = transform.Find("ThrowHookPosition").gameObject;
    }

    private void Update()
    {
        if (!_isReturning && Input.GetMouseButtonDown(0) && _currentHook == null)
        {
            ThrowHook();
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

    private void ThrowHook()
    {
        _currentHook = Instantiate(_hookPrefab, _throwHookPosition.transform.position, Quaternion.identity);

        _currentHook.tag = _throwHookPosition.tag;

        Vector3 hookDirection = transform.forward;

        _currentHook.transform.forward = hookDirection;

        _isReturning = false;

        _TPSControllerScript.enabled = false;
    }

    private void UpdateLineRenderer()
    {
        _lineRenderer.positionCount = 2;

        _lineRenderer.SetPosition(0, _throwHookPosition.transform.position);
        _lineRenderer.SetPosition(1, _currentHook.transform.position);
    }

    private void MoveHook()
    {
        float step = _hookSpeed * Time.deltaTime;

        _currentHook.transform.position += _currentHook.transform.forward * step;

        if (Vector3.Distance(_throwHookPosition.transform.position, _currentHook.transform.position) >= _maxDistance)
        {
            _isReturning = true;
        }
    }

    private void ReturnHook()
    {
        float step = _returnSpeed * Time.deltaTime;

        _currentHook.transform.position = Vector3.MoveTowards(_currentHook.transform.position, _throwHookPosition.transform.position, step);

        if (Vector3.Distance(_currentHook.transform.position, _throwHookPosition.transform.position) < 0.5f)
        {
            _isReturning = false;

            Destroy(_currentHook);

            _lineRenderer.positionCount = 0;

            _TPSControllerScript.enabled = true;
        }
    }
}