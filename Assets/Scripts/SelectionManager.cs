using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs {
        public LabObject labObject;
    }
    [SerializeField] private LayerMask interactableLayer = new LayerMask();
    [SerializeField] private Transform addingPositionTransform; //cauldron altýndaki addingposition nesnesi
    private Transform selectedIngredient, highlight;

    private float addingTime = 2f;
    private float addingTimeCounter;

    private Vector3 lastIngredientPosition;
    private Quaternion lastIngredientRotation;

    private bool isAdding;
    

    private void Update() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!EventSystem.current.IsPointerOverGameObject() &&
                Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, interactableLayer)) 
        {
            highlight = raycastHit.transform;
            if (Input.GetMouseButtonDown(0) && highlight.TryGetComponent<LabObject>(out LabObject labObject)) { //highlight.CompareTag("Item")
                if (!isAdding) {
                    if (!labObject.GetLabObjectSO().isLiquid)
                        AddSolidIngredient(highlight);
                    else 
                        AddReusableIngredient(highlight);
                }
            }
        }
        else { //raycast ile bir nesne algýlanmýyorsa
            highlight = null;
        }

        HandleAddingIngredient();

    }
    private void AddReusableIngredient(Transform highlight) {
        selectedIngredient = highlight;

        //malzemenin masadaki konumunu ve rotasyonunu tutuyor
        lastIngredientPosition = selectedIngredient.position;
        lastIngredientRotation = selectedIngredient.rotation;

        selectedIngredient.position = addingPositionTransform.position;
        selectedIngredient.transform.rotation = Quaternion.Euler(0, 0, 0);

        addingTimeCounter = addingTime;
        isAdding = true;
        
        Debug.Log("Malzeme ekleniyor...");

        PlayAddAnimation(selectedIngredient);
    }
    private void AddSolidIngredient(Transform highlight) {
        selectedIngredient = highlight; //- 1.3f
        var ingredientPos = addingPositionTransform.position;
        ingredientPos.z += 0.13f; //beherin merkezi
        selectedIngredient.position = ingredientPos;

        AdjustRigidbodies(highlight);

        addingTimeCounter = addingTime / 4;
        isAdding = true;
        
        Debug.Log("Katý malzeme ekleniyor...");
    }

    private void AdjustRigidbodies(Transform transform) {
        var rb = transform.GetComponent<Rigidbody>();

        if (transform.GetComponent<LabObject>().GetLabObjectSO().hasMultipleMeshes) {
            Destroy(rb); //parent'deki rb
            Destroy(transform.GetComponent<Collider>()); //parent'de collider olmak zorunda

            BoxCollider c = transform.AddComponent<BoxCollider>();
            c.size = new Vector3(0.01f, 0.01f, 0.01f);

            var colliders = transform.GetComponentsInChildren<Collider>();
            foreach (Collider col in colliders) {
                if (col.transform == transform) continue;

                col.enabled = true;
                col.transform.AddComponent<Rigidbody>();
            }


        }
        else {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }

    private void HandleAddingIngredient() {
        if (isAdding) {
            addingTimeCounter -= Time.deltaTime;
            if (selectedIngredient != null && addingTimeCounter <= 0f) {
                //malzemenin masadaki konumunu ve rotasyonuna geri döndürüyor
                if (selectedIngredient.TryGetComponent(out LabObject labObject) 
                    && labObject.GetLabObjectSO().isLiquid) {
                    selectedIngredient.position = lastIngredientPosition;
                    selectedIngredient.rotation = lastIngredientRotation;
                }

                Debug.Log("Malzeme eklendi!");

                TryAddingIngredientToBeaker(selectedIngredient);
                selectedIngredient = null;
                isAdding = false;

            }
        }
    }
    private void TryAddingIngredientToBeaker(Transform selectedIngredient) { //karýþým hazýrlanmak istiyorsa
        if (selectedIngredient.TryGetComponent(out LabObject labObject)) {   //muhakkak nesnelerde labobject scripti olmalý
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs { //yoksa malzeme eklendi sayýlmaz
                labObject = labObject
            });
        }
    }
    private void PlayAddAnimation(Transform selectedIngredient) {
        float timeOffset = 0.05f; //eðer animasyon süresi adding süresinden fazla olursa animasyonda takýlý kalýyor
        float spillAnimationCycleDuration = (addingTime - timeOffset) / 2;

        selectedIngredient.DORotate(Vector3.right * 90f, spillAnimationCycleDuration)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.OutCubic); //Ease.OutCubic //Ease.OutCirc //Ease.OutBack

        var liquidSpillAnimation = selectedIngredient.GetComponentInChildren<ParticleSystem>();

        if (liquidSpillAnimation != null) 
            StartCoroutine(PlayEffectRoutine(liquidSpillAnimation, .2f));

    }

    private IEnumerator PlayEffectRoutine(ParticleSystem effect, float duration) {
        yield return new WaitForSeconds(duration);
        effect.Play();
    }

}
