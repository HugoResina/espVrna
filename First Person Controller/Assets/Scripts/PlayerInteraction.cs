using TMPro;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Camera _mainCamera;

    [Tooltip("Interact distance")]
    [SerializeField] private float rayDistance = 2f;

    [SerializeField] private TextMeshProUGUI interactText;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        CheckInteractable();
    }

    void CheckInteractable()
    {
        RaycastHit hit;
        Vector3 rayOrigin = _mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f));

        if (Physics.Raycast(rayOrigin, _mainCamera.transform.forward, out hit, rayDistance))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                //Debug.Log("Looking at: " + interactable.name);
                interactText.text = interactable.name;

            }
            else
            {
                interactText.text = "";
            }
        }
        else
        {
            interactText.text = "";
        }
    }
}
