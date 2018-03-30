using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

    public PlayerWeapon weapon;

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
                CmdPlayerShot(_hit.collider.name);
            }
        }
    }

    [Command]
    void CmdPlayerShot(string _ID)
    {
        Debug.Log(_ID + " a été touché.");
    }

}
