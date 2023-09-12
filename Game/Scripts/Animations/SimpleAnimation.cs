using UnityEngine;

public class SimpleAnimation : MonoBehaviour
{
    public Sprite[] MySpriteSequence;
    public float FramesPerSecond = 12;
    public bool Loop;
    public bool PlayOnStart;
    public SpriteRenderer Renderer;
    
    private int frame;
    private int frameCount;
    private void Start()
    {
        frameCount = MySpriteSequence.Length;
    }

    private void Update()
    {
        if(Loop && frame >= frameCount - 1)
            return;
            
        if(PlayOnStart == false)
            return;
        
        frame = (int)(Time.time * FramesPerSecond);
        
        frame %= MySpriteSequence.Length;
        
        Renderer.sprite = MySpriteSequence[frame];
    }

    public float GetTotalTime()
    {
        return 1f / FramesPerSecond * frameCount;
    }
    
    private void OnEnable()
    {
        frame = 0;
        Renderer.sprite = MySpriteSequence[0];
    }
}
