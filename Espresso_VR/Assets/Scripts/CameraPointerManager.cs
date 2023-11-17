using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class CameraPointerManager : MonoBehaviour
{
    [SerializeField] private GameObject _pointer;
    [SerializeField] private float _maxDistancePointer = 4.5f;
    [Range(0,1)]
    [SerializeField] private float distancePointerObject = 0.95f;
    private const float _maxDistance = 10;
    private GameObject _gazedAtObject = null;

    private readonly string interactableTag = "interactable";
    private float scaleSize = 0.025f;

    private void Start()
    {
        GazeManager.Instance.OnGazeSelection += GazeSelection;
    }

    private void GazeSelection()
    {
        _gazedAtObject?.SendMessage("OnPointerClick", null, SendMessageOptions.DontRequireReceiver);
    }
    public void Update()
    {
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, _maxDistance))
        {
            // GameObject detected in front of the camera.
            if (_gazedAtObject != hit.transform.gameObject)
            {
                // New GameObject.
                _gazedAtObject?.SendMessage("OnPointerExit", null, SendMessageOptions.DontRequireReceiver);
                _gazedAtObject = hit.transform.gameObject;
                _gazedAtObject.SendMessage("OnPointerEnter", null, SendMessageOptions.DontRequireReceiver);
                GazeManager.Instance.StartGazeSelection();
            }
            if(hit.transform.CompareTag(interactableTag))
            {
                PointerOnGaze(hit.point);
            }
            else
            {
                PointerOutGaze();
            }
        }
        else
        {
            // No GameObject detected in front of the camera.
            _gazedAtObject?.SendMessage("OnPointerExit", null, SendMessageOptions.DontRequireReceiver);
            _gazedAtObject = null;
        }

        // Checks for screen touches.
        if (Google.XR.Cardboard.Api.IsTriggerPressed)
        {
            _gazedAtObject?.SendMessage("OnPointerClick", null, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void PointerOnGaze(Vector3 hitPoint)
    {
        float scaleFactor = scaleSize * Vector3.Distance(transform.position, hitPoint);
        _pointer.transform.localScale = Vector3.one * scaleFactor;
        _pointer.transform.parent.position = CalculatePointerPosition(transform.position,hitPoint,distancePointerObject);
    }

    private void PointerOutGaze()
    {
        _pointer.transform.localScale = Vector3.one * 0.1f;
        _pointer.transform.parent.transform.localPosition = new Vector3(0, 0, _maxDistancePointer);
        _pointer.transform.parent.parent.transform.rotation = transform.rotation;
        GazeManager.Instance.CancelGazeSelection();
    }
    private Vector3 CalculatePointerPosition(Vector3 p0, Vector3 p1, float t)
    {
        float x = p0.x +t*(p1.x - p0.x);
        float y = p0.y +t*(p1.y - p0.y);
        float z = p0.z +t*(p1.z - p0.z);
        return new Vector3(x, y, z);
    }
}
