using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

    [SerializeField]
    private PlayerWeapon weapon;
    [SerializeField]
    private GameObject weaponGFX;
    [SerializeField]
    private string weaponLayerName = "Weapon";

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

	void Start () {
		if(cam == null)
        {
            Debug.LogError("Pas de caméra référencée.");
            this.enabled = false;
        }

        weaponGFX.layer = LayerMask.NameToLayer(weaponLayerName);
	}

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    [Client]
    private void Shoot()
    {
        RaycastHit _hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask))
        {
            if(_hit.collider.tag == "Player")
            {
                CmdPlayerShot(_hit.collider.name, weapon.damage);
            }
        }
    }

    [Command]
    void CmdPlayerShot(string _playerID, int _damage)
    {
        Debug.Log(_playerID + " a été touché.");

        Player _player = GameManager.GetPlayer(_playerID);
        _player.RpcTakeDamage(_damage);
    }

}
