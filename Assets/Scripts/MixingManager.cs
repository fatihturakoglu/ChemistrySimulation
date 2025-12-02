using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class MixingManager : MonoBehaviour
{
    [SerializeField] private Transform cauldronPosition; //cauldron altýndaki addingposition nesnesi
    private RaycastHit raycastHit;
    private Transform selectedIngredient, highlight;

    private float addingTime = 1.5f;
    private float addingTimeCounter;

    private Vector3 lastIngredientPosition;
    private Quaternion lastIngredientRotation;

    private bool isAdding;
    private void Start()
    {
        
    }

    private void Update() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!EventSystem.current.IsPointerOverGameObject() &&
                Physics.Raycast(ray, out raycastHit)) 
        {
            highlight = raycastHit.transform;
            if (Input.GetMouseButtonDown(0) && highlight.CompareTag("Item")) {
                AddIngredient();
            }
        }
        else { //raycast ile bir nesne algýlanmýyorsa
            highlight = null;
        }

        HandleAddingIngredient();

    }
    private void AddIngredient() {
        selectedIngredient = highlight;

        //malzemenin masadaki konumunu ve rotasyonunu tutuyor
        lastIngredientPosition = selectedIngredient.position;
        lastIngredientRotation = selectedIngredient.rotation;

        selectedIngredient.position = cauldronPosition.position;
        selectedIngredient.transform.rotation = Quaternion.Euler(0, 0, 0);

        isAdding = true;
        addingTimeCounter = addingTime;
        Debug.Log("Malzeme ekleniyor...");

        PlayAddAnimation();
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
        selectedIngredient.DORotate(Vector3.right * 90f, (addingTime - 0.1f) / 2)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.OutCubic); //Ease.OutCubic //Ease.OutCirc //Ease.OutBack
    }
    
}
