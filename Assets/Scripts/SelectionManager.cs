using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public LabObject labObject;
    }

    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private Transform addingPositionTransform;
    [SerializeField] private Transform spawnPositionTransform;

    private Transform selectedIngredient, highlight;
    private float addingTime = 2f;
    private float addingTimeCounter;
    private Vector3 lastIngredientPosition;
    private Quaternion lastIngredientRotation;
    private bool isAdding;

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!EventSystem.current.IsPointerOverGameObject() &&
            Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, interactableLayer))
        {
            highlight = raycastHit.transform;

            if (Input.GetMouseButtonDown(0))
            {
                if (highlight.TryGetComponent<LabObject>(out LabObject labObj))
                {
                    if (!isAdding)
                    {
                        var so = labObj.GetLabObjectSO();

                        if (so.animationType == LabAnimationType.None)
                        {
                            AddSolidIngredient(highlight);
                        }
                        else
                        {
                            AddReusableIngredient(highlight, so.animationType);
                        }
                    }
                }
            }
        }
        else
        {
            highlight = null;
        }

        HandleAddingIngredient();
    }

    private void AddSolidIngredient(Transform target)
    {
        selectedIngredient = target;
        Vector3 pos = addingPositionTransform.position;
        pos.z += 0.13f;
        selectedIngredient.position = pos;

        var rb = selectedIngredient.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        addingTimeCounter = addingTime / 4;
        isAdding = true;
    }

    private void AddReusableIngredient(Transform target, LabAnimationType animType)
    {
        selectedIngredient = target;
        lastIngredientPosition = selectedIngredient.position;
        lastIngredientRotation = selectedIngredient.rotation;

        selectedIngredient.position = addingPositionTransform.position;
        selectedIngredient.rotation = Quaternion.identity;

        addingTimeCounter = addingTime;
        isAdding = true;

        PlayAddAnimation(selectedIngredient, animType);
    }

    private void PlayAddAnimation(Transform target, LabAnimationType animType)
    {
        float duration = (addingTime - 0.05f) / 2;
        LabObject labObj = target.GetComponent<LabObject>();
        if (labObj == null) return;

        target.DORotate(new Vector3(90, 0, 0), duration)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.OutCubic)
            .OnStepComplete(() => {
                // Şişe 90 dereceye ulaştığında bardağa ekleme metodunu sadece 1 kez çağırıyoruz
                var beaker = FindObjectOfType<BeakerManager>();
                if (beaker != null)
                {
                    beaker.AddIngredient(labObj, duration);
                }
            });

        if (animType == LabAnimationType.LiquidPour)
        {
            var ps = target.GetComponentInChildren<ParticleSystem>();
            if (ps != null) StartCoroutine(PlayEffectRoutine(ps, duration * 0.3f));
        }
        else if (animType == LabAnimationType.SolidSpill)
        {
            StartCoroutine(ReleaseSolidPelletsAfterDelay(labObj, duration * 0.5f));
        }
    }

    // KRİTİK: Bu metodun sadece BİR KEZ tanımlandığından emin olun
    private void HandleAddingIngredient()
    {
        if (isAdding)
        {
            addingTimeCounter -= Time.deltaTime;
            if (selectedIngredient != null && addingTimeCounter <= 0f)
            {
                if (selectedIngredient.TryGetComponent<LabObject>(out LabObject labObj))
                {
                    if (labObj.GetLabObjectSO().isReusable)
                    {
                        selectedIngredient.position = lastIngredientPosition;
                        selectedIngredient.rotation = lastIngredientRotation;
                    }
                    // Not: AddIngredient zaten OnStepComplete içinde çağrıldığı için burada tekrar tetiklemiyoruz.
                }
                selectedIngredient = null;
                isAdding = false;
            }
        }
    }

    private IEnumerator ReleaseSolidPelletsAfterDelay(LabObject bottle, float delay)
    {
        yield return new WaitForSeconds(delay);
        LabObjectSO so = bottle.GetLabObjectSO();

        if (so.visualPrefabInBeaker != null)
        {
            Transform spawnedVisual = Instantiate(so.visualPrefabInBeaker, spawnPositionTransform.position, spawnPositionTransform.rotation);
            StartCoroutine(AnimatePowderSpam(spawnedVisual, 1f));

            if (spawnedVisual.TryGetComponent<LabObject>(out LabObject powderLabObj))
            {
                OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs { labObject = powderLabObj });
            }
        }

        var spillEffect = bottle.GetComponentInChildren<ParticleSystem>();
        if (spillEffect != null) spillEffect.Play();
    }

    private IEnumerator AnimatePowderSpam(Transform powderParent, float totalDuration)
    {
        var pellets = powderParent.GetComponentsInChildren<MeshRenderer>().ToList();
        foreach (var pellet in pellets) pellet.enabled = false;

        System.Random rng = new System.Random();
        int n = pellets.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            var value = pellets[k];
            pellets[k] = pellets[n];
            pellets[n] = value;
        }

        float delayBetweenSpams = totalDuration / pellets.Count;
        foreach (var pellet in pellets)
        {
            if (pellet == null) continue;
            pellet.enabled = true;
            yield return new WaitForSeconds(delayBetweenSpams);
        }
    }

    private IEnumerator PlayEffectRoutine(ParticleSystem effect, float delay)
    {
        yield return new WaitForSeconds(delay);
        effect.Play();
    }
}