using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
public class Shooter : MonoBehaviour
{

	//list of available bullets
	public GameObject preTankBullet;



	//defined in prefab
	//must create an empty point as a child of the gameobject
	public GameObject aimObj;
		//calculated using aimObj
		private Vector3 shootPos;
		//in case I want to add shooters that can shoot in 3d later
		private Quaternion shootAng;
	//I just feel like this is infinitely more intuitive for both the programmer and the designer than having a firerate
	public float fireDelay;
	//used when initializing bullet
	public float fireForce;
	public float shotDmg;

	//Automatically assembled component references
	//private Rigidbody bulletRB;
	private Projectile bulletProj;
	private BulletDamage bulletDamage;

	private Damage damage;
	private Controller controller;
	public Pawn pawn;

	//Misc Variables
	private float fireTime;
    // Start is called before the first frame update
    void Start()
    {
		//aimObj.transform.GetPositionAndRotation(out shootPos, out shootAng);
        shootPos = aimObj.transform.localPosition;
		shootAng = aimObj.transform.localRotation;

		damage = GetComponent<Damage>();
		pawn = GetComponent<Pawn>();
		//should rewrite this but I wont
		controller = GetComponent<Pawn>().controller;
		//Debug.Log(controller);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	//since other bullets might want to fire differently, this function is specific to the tank bullet, but is in the Shooter class so that any shooter could fire it.
	public virtual void FireTankBullet(int team)
	{
		if(fireTime + fireDelay < Time.time) 
		{
			fireTime = Time.time;
			//Getting bullet references
			GameObject objNewBullet = Instantiate(preTankBullet, aimObj.transform) as GameObject;
			//bulletRB = objNewBullet.GetComponent<Rigidbody>();
			bulletProj = objNewBullet.GetComponent<Projectile>();
			bulletDamage = objNewBullet.GetComponent<BulletDamage>();

			objNewBullet.transform.SetParent(null, true);

			DefineBullet();
			bulletProj.Shoot(fireForce);
			
			//objectNewBullet.
			}
	}

	public virtual void DefineBullet() 
	{
		//Initializes the bullets Proj with the bullet speed and reference to shooter
		bulletProj.speed = fireForce;
		bulletProj.shooter = controller;
		//Debug.Log(bulletProj.shooter);
		//Debug.Log(controller);

		//initializes the bullet's damage with the shooter's dmg and team
		bulletDamage.damage = shotDmg;
		bulletDamage.team = damage.team;

	}
}
