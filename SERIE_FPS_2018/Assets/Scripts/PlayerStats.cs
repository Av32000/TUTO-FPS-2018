using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {

    public Text killCount;
    public Text deathCount;

	// Use this for initialization
	void Start () {
        if (UserAccountManager.IsLoggedIn)
        {
            UserAccountManager.instance.GetData(OnReceivedData);
        }
	}

    public void OnReceivedData(string data)
    {
        if(killCount == null ||deathCount == null)
        {
            return;
        }

        killCount.text = DataTranslator.DataToKills(data).ToString() + " KILLS";
        deathCount.text = DataTranslator.DataToDeaths(data).ToString() + " DEATHS";
    }

}
