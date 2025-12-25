 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OutlineSelection : MonoBehaviour {
    private Transform highlight;
    private Transform selection;
    private RaycastHit raycastHit;
    [SerializeField] private LayerMask interactableLayer = new LayerMask();

    void Update() {
        // Highlight
        if (highlight != null) {
            highlight.gameObject.GetComponent<Outline>().enabled = false;
            highlight = null;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!EventSystem.current.IsPointerOverGameObject()
            && Physics.Raycast(ray, out raycastHit, Mathf.Infinity, interactableLayer)) 
        {
            highlight = raycastHit.transform;
            if (highlight != selection) { //highlight.GetComponent<LabObject>() highlight.CompareTag("Item")
                if (highlight.gameObject.GetComponent<Outline>() != null) {
                    highlight.gameObject.GetComponent<Outline>().enabled = true;
                }
                else {
                    Outline outline = highlight.gameObject.AddComponent<Outline>();
                    outline.enabled = true;
                }
            }
            else {
                highlight = null;
            }
        }

        //// Selection
        //if (Input.GetMouseButtonDown(0)) {
        //    if (highlight) {
        //        if (selection != null) {
        //            selection.gameObject.GetComponent<Outline>().enabled = false;
        //        }
        //        selection = raycastHit.transform;
        //        selection.gameObject.GetComponent<Outline>().enabled = true;
        //        highlight = null;
        //    }
        //    else {
        //        if (selection) {
        //            selection.gameObject.GetComponent<Outline>().enabled = false;
        //            selection = null;
        //        }
        //    }
        //}
    }

}
