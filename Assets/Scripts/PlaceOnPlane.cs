using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;

/// <summary>
/// Listens for touch events and performs an AR raycast from the screen touch point.
/// AR raycasts will only hit detected trackables like feature points and planes.
///
/// If a raycast hits a trackable, the <see cref="placedPrefab"/> is instantiated
/// and moved to the hit position.
/// </summary>
[RequireComponent(typeof(ARRaycastManager))]
public class PlaceOnPlane : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject m_PlacedPrefab;
    private float minY; // -380
    private float minX; // -133

    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
        GameObject canvas = GameObject.Find("Canvas");
        float cHeight = canvas.GetComponent<Canvas>().pixelRect.height;
        float cWidth = canvas.GetComponent<Canvas>().pixelRect.width;
        minY = cHeight - 390;
        minX = cWidth - 140;
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            var mousePosition = Input.mousePosition;
            touchPosition = new Vector2(mousePosition.x, mousePosition.y);
            return true;
        }
#else
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
#endif

        touchPosition = default;
        return false;
    }

    void Update()
    {

        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;

        if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon) && !DrawerInteractions.drawerIsUp && touchPosition.y > (210) && !(touchPosition.y > (minY) && touchPosition.x > (minX)))
        {

            // Raycast hits are sorted by distance, so the first one
            // will be the closest hit.
            var hitPose = s_Hits[0].pose;
            Quaternion quat = Quaternion.Euler(0, 0, 0);


            if (spawnedObject == null)
            {
                // spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position + new Vector3(0, 0.3f, 0), new Quaternion(0, 1, 0, 0));
                spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position + new Vector3(0, 0.3f, 0), quat);
            }
            else
            {
                Destroy(spawnedObject);
                spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position + new Vector3(0, 0.3f, 0), quat);
                // spawnedObject.transform.position = hitPose.position + new Vector3(0, 0.3f, 0);
                // // spawnedObject.transform.rotation = new Quaternion(0, 1, 0, 0);
                // spawnedObject.transform.rotation = quat;
                // spawnedObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                // spawnedObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }
        }
    }

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    ARRaycastManager m_RaycastManager;
}
