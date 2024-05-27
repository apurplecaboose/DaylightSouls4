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
        if (TryndamereMode) return true;
        Destroy(ComboSelectionUI_Instance);
        _BossMasterRef.DestroyEverythingonBoss();
        Instantiate(B_DeathCutScenePrefab);
        float deathcutscenelength = 2f;
        Invoke("ReloadSceneBcDead", deathcutscenelength);
        if (1 == 2) ReloadSceneBcDead();//stop ide from yelling at me for using invoke
        //load you died
        return TryndamereMode;
        void ReloadSceneBcDead()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    /// <summary>
    /// if return false then tryndamere mode is false, if true tryndamere is true then dont run death stuff
    /// </summary>
    /// <returns></returns>
    public bool BossDead()  
    {
        if(TryndamereMode) return true;
        Instantiate(B_DeathCutScenePrefab);
        float deathcutscenelength = 2f;
        Invoke("ToMenuSceneBcBossDead", deathcutscenelength);
        return TryndamereMode;
        if (1 == 2) ToMenuSceneBcBossDead();
        void ToMenuSceneBcBossDead()
        {
            SceneManager.LoadScene(1);
        }
    }
}