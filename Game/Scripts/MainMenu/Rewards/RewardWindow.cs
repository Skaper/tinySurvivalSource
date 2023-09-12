using System;
using FMODUnity;
using UnityEngine;

public class RewardWindow : MonoBehaviour
{
    public static RewardWindow instance;
    public GameObject Context;
    public Animator ChestAnimator;
    public SkinUpdater PlayerSkin;
    public EventReference RewardSound;
    private static readonly int Common = Animator.StringToHash("common");
    private static readonly int Rare = Animator.StringToHash("rare");
    private static readonly int Epic = Animator.StringToHash("epic");
    private static readonly int Legendary = Animator.StringToHash("legendary");


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        Context.SetActive(false);
    }
    

    public void Initialization()
    {
        
    }

    public void Show(ChestsRewardData.RewardType type)
    {
        PlayerSkin.HideSkin();
        Context.SetActive(true);
        RuntimeManager.PlayOneShot(RewardSound);
        switch (type)
        {
            case ChestsRewardData.RewardType.CommonChest:
                ChestAnimator.SetTrigger(Common);
                break;
            case ChestsRewardData.RewardType.RareChest:
                ChestAnimator.SetTrigger(Rare);
                break;
            case ChestsRewardData.RewardType.EpicChest:
                ChestAnimator.SetTrigger(Epic);
                break;
            case ChestsRewardData.RewardType.LegendaryChest:
                ChestAnimator.SetTrigger(Legendary);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    public void ChestAnimationEnd()
    {
        Hide();
    }

    public void Hide()
    {
        Context.SetActive(false);
        PlayerSkin.ShowSkin();
    }
}
