using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsHandler
{
	public const string PLAYER_KEY_F = "playerKey";
	public const string MUTE_INT = "mute";
	public const string VOLUME_F = "volume";
	public const string RECORD_PERS_INT = "recordPersonale";
	public const string NUM_PARTITE_INT = "numPartiteTotali";
	public const string MONETE_TOT_INT = "moneteTotaliGuadagnate";
	public const string MONETE_INT = "monete";
	public const string PERSONAGGIO_ATTUALE_S = "personaggioAttuale";




	public void RestorePreferences()
	{
		SetMuted(GetIsMuted());
		SetVolume(GetVolume());
	}

	public void InitializePreferences()
	{
		SetMuted(false);
		SetVolume(1.0f);
	}

    public void CreateFirstTimePref()
    {
		InitializePreferences();
		SetPlayerKey(Random.Range(1, 1000000));
		SetMonete(0);
		SetRecordPersonale(0);
		SetPersonaggioAttuale("Medico");
		SetNumPartiteTotali(0);
		SetMoneteTotali(0);
	}

    public bool isFirstTime()
    {
		if (GetPlayerKey() != 0) return false;
		else return true;
    }

	public float GetPlayerKey()
	{
		return PlayerPrefs.GetFloat(PLAYER_KEY_F);
	}

	public void SetPlayerKey(float val)
	{
		PlayerPrefs.SetFloat(PLAYER_KEY_F, val);
	}

	public int GetMoneteTotali()
	{
		return PlayerPrefs.GetInt(MONETE_TOT_INT);
	}

	public void SetMoneteTotali(int val)
	{
		PlayerPrefs.SetInt(MONETE_TOT_INT, val);
	}

	public int GetMonete()
	{
		return PlayerPrefs.GetInt(MONETE_INT);
	}

	public void SetMonete(int val)
	{
		PlayerPrefs.SetInt(MONETE_INT, val);
	}

	public int GetRecordPersonale()
    {
		return PlayerPrefs.GetInt(RECORD_PERS_INT);
    }

	public void SetRecordPersonale(int val)
	{
		PlayerPrefs.SetInt(RECORD_PERS_INT, val);
	}

	public int GetNumPartiteTotali()
	{
		return PlayerPrefs.GetInt(NUM_PARTITE_INT);
	}

	public void SetNumPartiteTotali(int val)
	{
		PlayerPrefs.SetInt(NUM_PARTITE_INT, val);
	}

	public void SetMuted(bool muted)
	{
		PlayerPrefs.SetInt(MUTE_INT, muted ? 1 : 0);

		// Pausing the AudioListener will disable all sounds.
		AudioListener.pause = muted;
	}

	public bool GetIsMuted()
	{
		// If the value of the MUTE_INT key is 1 then sound is muted, otherwise it is not muted.
		// The default value of the MUTE_INT key is 0 (i.e. not muted).
		return PlayerPrefs.GetInt(MUTE_INT, 0) == 1;
	}

	public void SetVolume(float volume)
	{
		// Prevent values less than 0 and greater than 1 from
		// being stored in the PlayerPrefs (AudioListener.volume expects a value between 0 and 1).
		volume = Mathf.Clamp(volume, 0, 1);
		PlayerPrefs.SetFloat(VOLUME_F, volume);
		AudioListener.volume = volume;
	}

	public float GetVolume()
	{
		return Mathf.Clamp(PlayerPrefs.GetFloat(VOLUME_F, 1), 0, 1);
	}

	public string GetPersonaggioAttuale()
	{
		return PlayerPrefs.GetString(PERSONAGGIO_ATTUALE_S);
	}

	public void SetPersonaggioAttuale(string val)
	{
		PlayerPrefs.SetString(PERSONAGGIO_ATTUALE_S, val);
	}

}
