using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class FireExperimentSelection : MonoBehaviour
{
    // Malzeme eklendi?inde Beaker'a haber veren olay
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public LabObject labObject;
    }

    [SerializeField] private LayerMask interactableLayer; // Sadece LabObject katman?n? seç
    [SerializeField] private Transform addingPositionTransform; // Beherin tam üzerindeki nokta

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!EventSystem.current.IsPointerOverGameObject() &&
                Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, interactableLayer))
            {

                if (hit.transform.TryGetComponent<LabObject>(out LabObject labObject))
                {
                    AddIngredient(hit.transform, labObject);
                }
            }
        }
    }

    private void AddIngredient(Transform ingredient, LabObject labObject)
    {
        // Maddeyi beherin üzerine ta??
        ingredient.position = addingPositionTransform.position;

        // Yerçekimini aç ki beherin içine dü?sün
        Rigidbody rb = ingredient.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        // BeakerManager'a haber ver
        OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs { labObject = labObject });
        Debug.Log(labObject.GetLabObjectSO().objectName + " behere eklendi.");
    }
}