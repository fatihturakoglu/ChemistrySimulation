using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class IngredientManager : MonoBehaviour
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs {
        public LabObject labObject;
    }

    [SerializeField] private Transform addingPositionTransform; //cauldron altýndaki addingposition nesnesi
    private Transform selectedIngredient, highlight;

    private float addingTime = 1.5f;
    private float addingTimeCounter;

    private Vector3 lastIngredientPosition;
    private Quaternion lastIngredientRotation;

    private bool isAdding;

    private void Start() {
        selectedIngredient = null;
        highlight = null;
        isAdding = false;
    }

    private void Update() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!EventSystem.current.IsPointerOverGameObject() &&
                Physics.Raycast(ray, out RaycastHit raycastHit)) 
        {
            highlight = raycastHit.transform;
            if (Input.GetMouseButtonDown(0) && highlight.CompareTag("Item")) {
                if (!isAdding) {
                    if (highlight.TryGetComponent(out Rigidbody rb)) {
                        AddSolidIngredient(highlight, rb);
                    }
                    else {
                        AddIngredient(highlight);
                    }
                }
            }
        }
        else { //raycast ile bir nesne algýlanmýyorsa
            highlight = null;
        }

        HandleAddingIngredient();

    }
    private void AddIngredient(Transform highlight) {
        selectedIngredient = highlight;

        //malzemenin masadaki konumunu ve rotasyonunu tutuyor
        lastIngredientPosition = selectedIngredient.position;
        lastIngredientRotation = selectedIngredient.rotation;

        selectedIngredient.position = addingPositionTransform.position;
        selectedIngredient.transform.rotation = Quaternion.Euler(0, 0, 0);

        isAdding = true;
        addingTimeCounter = addingTime;
        Debug.Log("Malzeme ekleniyor...");

        PlayAddAnimation();
        TryAddingIngredientToBeaker(selectedIngredient);
    }
    private void AddSolidIngredient(Transform highlight, Rigidbody rb) {
        selectedIngredient = highlight; //- 1.3f
        var ingredientPos = addingPositionTransform.position;
        ingredientPos.z += 0.13f; //beherin merkezi
        selectedIngredient.position = ingredientPos;
        
        rb.isKinematic = false;
        rb.useGravity = true;

        Debug.Log("Katý malzeme eklendi!");
        TryAddingIngredientToBeaker(selectedIngredient); //?
    }

    //selectedingredient deðiþkeni üzerinden if yapýlmamalý çünkü sürekli güncelleniyor!!
    //düzeltilecek
    private void TryAddingIngredientToBeaker(Transform selectedIngredient) {
        if (selectedIngredient != null && selectedIngredient.TryGetComponent(out LabObject labObject)) {
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs {
                labObject = labObject
            });
        }
    }

    private void HandleAddingIngredient() {
        if (isAdding) {
            addingTimeCounter -= Time.deltaTime;
            if (selectedIngredient != null && addingTimeCounter <= 0f) {
                //malzemenin masadaki konumunu ve rotasyonuna geri döndürüyor
                selectedIngredient.position = lastIngredientPosition;
                selectedIngredient.rotation = lastIngredientRotation;

                selectedIngredient = null;
                isAdding = false;
                Debug.Log("Malzeme eklendi!");
            }
        }
    }

    private void PlayAddAnimation() {
        float timeOffset = 0.05f; //eðer animasyon süresi adding süresinden fazla olursa animasyonda takýlý kalýyor
        selectedIngredient.DORotate(Vector3.right * 90f, (addingTime - timeOffset) / 2)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.OutCubic); //Ease.OutCubic //Ease.OutCirc //Ease.OutBack
    }
    
}
