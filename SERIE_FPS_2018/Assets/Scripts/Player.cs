using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(PlayerSetup))]
public class Player : NetworkBehaviour {

    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead;  }
        protected set { _isDead = value; }
    }

    [SerializeField]
    private int maxHealth = 100;

    [SyncVar]
    private int currentHealth;

    [SerializeField]
    private Behaviour[] disableOnDeath;

    [SerializeField]
    private GameObject[] disableGameObjectsOnDeath;

    private bool[] wasEnabled;

    [SerializeField]
    private GameObject deathEffect;

    [SerializeField]
    private GameObject spawnEffect;

    public void Setup()
    {
        wasEnabled = new bool[disableOnDeath.Length];
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }

        SetDefaults();
    }

    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(9999);
        }
    }

    [ClientRpc]
    public void RpcTakeDamage(int _amount)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= _amount;
        Debug.Log(transform.name + " a maintenant : " + currentHealth + " points de vie.");

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        // Désactive les components du joueur
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        // Désactive les GameObjects du joueur
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(false);
        }

        // désactive le collider
        Collider _col = GetComponent<Collider>();
        if (_col != null)
        {
            _col.enabled = false;
        }

        // Apparition de la particule de mort
        GameObject _gfxIns = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(_gfxIns, 3f);

        // Changement de caméra
        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(true);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(false);
        }

        Debug.Log(transform.name + " est mort.");

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);
        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;

        SetDefaults();

        Debug.Log(transform.name + " a respawn.");
    }

    public void SetDefaults()
    {
        isDead = false;
        currentHealth = maxHealth;

        // Active les components du joueur
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        // Active les GameObjects du joueur
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(true);
        }

        // Active le collider
        Collider _col = GetComponent<Collider>();
        if(_col != null)
        {
            _col.enabled = true;
        }

        // Changement de caméra
        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(false);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
        }

        // Apparition des particules de spawn
        GameObject _gfxIns = (GameObject)Instantiate(spawnEffect, transform.position, Quaternion.identity);
        Destroy(_gfxIns, 3f);
    }

}