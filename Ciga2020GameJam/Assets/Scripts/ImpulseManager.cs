using Cinemachine;

public class ImpulseManager : Singleton<ImpulseManager>
{
    public CinemachineImpulseSource MinSource;
    public CinemachineImpulseSource MidSource;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void GenerateImpulse(int level)
    {
        if (level == 1)
        {
            if (MinSource)
            {
                MinSource.GenerateImpulse();   
            }
        }else if (level == 2)
        {
            if (MidSource)
            {
                MidSource.GenerateImpulse();   
            }
        }
        else
        {
            if (MidSource)
            {
                MinSource.GenerateImpulse();   
            }
        }
    }
}
