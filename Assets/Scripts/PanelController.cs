using UnityEngine;

public class PanelController : MonoBehaviour
{
    public GameObject[] panels; // Panel GameObject'lerini tutar

    public void ActivatePanel(int panelIndex)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            // Týklanan panelin index'i dýþýndaki tüm panelleri deaktif et
            panels[i].SetActive(i == panelIndex); // Eðer index eþleþiyorsa aktif et, deðilse deaktif et
        }
    }
}