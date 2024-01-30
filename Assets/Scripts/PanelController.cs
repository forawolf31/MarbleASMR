using UnityEngine;

public class PanelController : MonoBehaviour
{
    public GameObject[] panels; // Panel GameObject'lerini tutar

    public void ActivatePanel(int panelIndex)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            // T�klanan panelin index'i d���ndaki t�m panelleri deaktif et
            panels[i].SetActive(i == panelIndex); // E�er index e�le�iyorsa aktif et, de�ilse deaktif et
        }
    }
}