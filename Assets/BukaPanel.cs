using System.Collections;
using UnityEngine;

public class BukaPanel : MonoBehaviour
{
    public GameObject panelStart, panelPendahuluan;
    private void OnEnable()
    {
        StartCoroutine(proses());
        IEnumerator proses()
        {
            yield return null;
            bool bukaPendahuluan = PengaturanPanelStart.instance.IsBukaPendahuluan();
            panelPendahuluan.SetActive(bukaPendahuluan);
            panelStart.SetActive(!bukaPendahuluan);
        }
        
    }
}
