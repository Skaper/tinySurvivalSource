using TMPro;
using UnityEngine;

public class DiamondCountUpdater : MonoBehaviour
{
    public TextMeshProUGUI DiamondCountLabel;
    void Start()
    {
        var diamondCount = GameProgress.GetData().diamonds;
        DiamondCountLabel.text = diamondCount.ToString();
        GameProgress.GetData().DiamondsUpdated += OnDiamondsUpdated;
    }

    private void OnDiamondsUpdated(int count)
    {
        DiamondCountLabel.text = count.ToString();
    }

    
}
