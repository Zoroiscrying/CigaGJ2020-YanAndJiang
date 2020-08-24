using UnityEngine;

namespace UnityCore.Data
{
    public class PlayerPrefsManager : MonoBehaviour
    {
        //Graphic Settings
        public static readonly string graphicSettings = "GraphicSettings";
        // 0 for low
        // 1 for medium
        // 2 for high
        public int GraphicSettingsasd
        {
            get => PlayerPrefs.GetInt(graphicSettings);
            set
            {
                int tier = Mathf.Clamp(value, 0, 2);
                PlayerPrefs.SetInt(graphicSettings, tier);
                QualitySettings.SetQualityLevel(tier);
                // Graphics.activeTier = (GraphicsTier) tier;
            }
        }

        //Screen Settings
        public static readonly string screenSettings = "ScreenSettings";
        public static readonly string fullScreenMode = "FullScreenMode";
        public int ScreenSettings
        {
            get => PlayerPrefs.GetInt(screenSettings);
            set
            {
                int ind = Mathf.Clamp(value, 0, 3);
                PlayerPrefs.SetInt(screenSettings, value);
                Screen.SetResolution(1920, 1080, true);
            }
        }
        
        //Player Controls
        
        //Sound Settings
        public static readonly string sfxSound = "SfxSound";
        public static readonly string bgMusic = "BgMusic";

        public bool SfxSound
        {
            get => PlayerPrefs.GetInt(sfxSound) != 0;
            set
            {
                //SfxSound on => Value == 1
                int setter = value ? 1 : 0;
                PlayerPrefs.SetInt(sfxSound, setter);
            }
        }
        
        //Game Settings
        
        //Other Settings
        
        #region Public Methods
    
        
    
        #endregion
    
        #region Unity Functions
    
        private void Awake()
        {
            
        }
    
        #endregion
    
        #region Private Functions
    
        
    
        #endregion
        
    }
}

