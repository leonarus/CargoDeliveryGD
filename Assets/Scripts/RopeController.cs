using System.Collections;
using UnityEngine;

public class RopeController : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Transform _ropeTransform;
    [SerializeField] private BoxController _boxController;
    [SerializeField] private Transform _finishPointTransform;
    [SerializeField] private float _moveSpeed = 5f;

    private bool _isDrawingLine;
    private Camera _mainCamera;
    private int _currentLineIndex;
    private Vector3 _lastMousePosition;
    private bool _isMovingRope;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        HandleMouseInput();
    }

    private void HandleMouseInput()
    {
        if (_isMovingRope) return;

        if (Input.GetMouseButtonDown(0))
        {
            _isDrawingLine = true;
            StartDrawingLine();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _isDrawingLine = false;
            StartCoroutine(MoveRopeAlongPath());
        }

        if (_isDrawingLine)
        {
            UpdateLinePath();
        }
    }

    private void StartDrawingLine()
    {
        if (_lineRenderer.positionCount == 0)
        {
            _lineRenderer.positionCount++;
            _lineRenderer.SetPosition(0, _ropeTransform.position);
        }
        _lastMousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    private void UpdateLinePath()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Mathf.Abs(_mainCamera.transform.position.z);
        Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(mousePosition);

        if (Vector3.Distance(worldPosition, _lastMousePosition) > 0.1f)
        {
            _lineRenderer.positionCount++;
            _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, worldPosition);
            _lastMousePosition = worldPosition;
        }
    }

    private IEnumerator MoveRopeAlongPath()
    {
        _isMovingRope = true;
        while (_currentLineIndex < _lineRenderer.positionCount)
        {
            Vector3 targetPosition = _lineRenderer.GetPosition(_currentLineIndex);
            _ropeTransform.position = Vector3.MoveTowards(_ropeTransform.position, targetPosition, _moveSpeed * Time.deltaTime);

            if (Vector3.Distance(_ropeTransform.position, targetPosition) < 0.01f)
            {
                _currentLineIndex++;
            }

            yield return null;
        }
        _isMovingRope = false;
    }

    public void Stop()
    {
        StopAllCoroutines();
        _isMovingRope = false;
    }
}
