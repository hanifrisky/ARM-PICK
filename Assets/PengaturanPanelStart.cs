using System.Collections;
using UnityEngine;

public class PengaturanPanelStart : MonoBehaviour
{
    public static PengaturanPanelStart instance;
    bool bukapendahuluan;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            bukapendahuluan = true; 
        }
        else
        {
            bukapendahuluan = false;
            Destroy(gameObject);
        }
    }
    public bool IsBukaPendahuluan()
    {
        return bukapendahuluan;
    }
}
