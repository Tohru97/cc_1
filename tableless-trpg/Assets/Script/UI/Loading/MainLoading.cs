using UnityEngine;

public class MainLoading : MonoBehaviourExtension
{
    public override void Awake()
    {

    }

    public override void Init()
    {

    }
    
    public override void Subscribe()
    {
        
    }

    public override void Unsubscribe()
    {

    }

    public override void Show()
    {
        transform.gameObject.SetActive(true);
    }

    public override void Hide()
    {
        transform.gameObject.SetActive(false);
    }

    public override void Reset()
    {

    }
}