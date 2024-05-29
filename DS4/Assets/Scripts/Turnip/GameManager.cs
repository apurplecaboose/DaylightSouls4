using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool TryndamereMode;
    public enum G_State { Playing, Selecting };
    public G_State PlayState;
    public GameObject ComboSelectionUI_Instance;
    Boss_Master _BossMasterRef;
    public GameObject B_DeathCutScenePrefab, P_DeathCutScenePrefab;
    void Awake()
    {
        _BossMasterRef = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss_Master>();
    }
    void Update()
    {
        if(ComboSelectionUI_Instance == null) PlayState = G_State.Playing;
        else PlayState = G_State.Selecting;
    }
    /// <summary>
    /// if return false then tryndamere mode is false, if true tryndamere is true then dont run death stuff
    /// </summary>
    /// <returns></returns>
    public bool PlayerDead()
    {
        PlayState = G_State.Selecting;
        if (TryndamereMode) return true;
        Destroy(ComboSelectionUI_Instance);
        _BossMasterRef.DestroyEverythingonBoss();
        Instantiate(P_DeathCutScenePrefab);
        foreach (Transform t in _BossMasterRef.transform)
        {
            Destroy(t.gameObject);
        }
        float deathcutscenelength = 4;
        Invoke("ReloadSceneBcDead", deathcutscenelength);
        return TryndamereMode;
    }
    /// <summary>
    /// if return false then tryndamere mode is false, if true tryndamere is true then dont run death stuff
    /// </summary>
    /// <returns></returns>
    public bool BossDead()  
    {
        PlayState = G_State.Selecting;
        if (TryndamereMode) return true;
        Instantiate(B_DeathCutScenePrefab);
        float deathcutscenelength = 4;
        Invoke("ToMenuSceneBcBossDead", deathcutscenelength);
        return TryndamereMode;
    }
    void ReloadSceneBcDead()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void ToMenuSceneBcBossDead()
    {
        SceneManager.LoadScene(1);
    }
}