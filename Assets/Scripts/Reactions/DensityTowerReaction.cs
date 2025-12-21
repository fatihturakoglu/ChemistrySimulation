using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DensityTowerReaction : MonoBehaviour
{//deney sadece yað, alkol ve bal ile gerçekleþecek þekilde ayarlandý, farklý sývýlar ile yapýlmak isteniyorsa tüm tepkime baþtan geçirilmeli
    [SerializeField] private LabObjectSO[] liquids;
    private MeshRenderer mainLiquid;
    private MeshRenderer[] layeredLiquids;

    private float life = 3f;
    private void Start()
    {
        Debug.Log("Yoðunluk Kulesi tepkimesi");

        mainLiquid = BeakerManager.Instance.GetMainLiquidRenderer();
        mainLiquid.gameObject.SetActive(false);

        layeredLiquids = BeakerManager.Instance.GetLayeredLiquidRenderers();

        for(int i = 0; i < layeredLiquids.Length; i++) {
            layeredLiquids[i].gameObject.SetActive(true);
            layeredLiquids[i].material.color = liquids[i].color;
        }

        Destroy(gameObject, life);
    }

    private void OnDestroy() {
        for (int i = 0; i < layeredLiquids.Length; i++) {
            layeredLiquids[i].gameObject.SetActive(false);
        }
    }


}
