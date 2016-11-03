using UnityEngine;
using System.Collections;

public class PlayerMovement : Photon.PunBehaviour
{
	static int hashSpeed = Animator.StringToHash ("Speed");
	static int hashFallSpeed = Animator.StringToHash ("FallSpeed");
	static int hashGroundDistance = Animator.StringToHash ("GroundDistance");
	static int hashIsCrouch = Animator.StringToHash ("IsCrouch");

	//static int hashDamage = Animator.StringToHash ("Damage");
	//static int hashIsDead = Animator.StringToHash ("IsDead");

	[SerializeField] private float characterHeightOffset = 0.2f;
	[SerializeField] LayerMask groundMask = (LayerMask)0;
    [SerializeField] float maxSpeed = 10;
   // [SerializeField] Vector2 backwardForce = new Vector2(-4.5f, 5.4f);


    [SerializeField, HideInInspector] Animator animator;
	//[SerializeField, HideInInspector]SpriteRenderer spriteRenderer;
	[SerializeField, HideInInspector]Rigidbody2D rig2d;

    
    public int hp = 4;

	void Awake ()
	{
        this.animator = GetComponent<Animator> ();
        //spriteRenderer = GetComponent<SpriteRenderer> ();
        this.rig2d = GetComponent<Rigidbody2D> ();
	}

    void Start()
    {
        if (!this.photonView.isMine)
        {
            this.rig2d.isKinematic = true;
        }
    }

	void Update ()
	{
        if (!this.photonView.isMine)
        {
            return;
        }

		float axis = Input.GetAxisRaw ("Horizontal");
		bool isDown = Input.GetAxisRaw ("Vertical") < 0;

		if (Input.GetButtonDown ("Jump")) {
			rig2d.velocity = new Vector2 (rig2d.velocity.x, 20);
		}

		var distanceFromGround = Physics2D.Raycast (transform.position, Vector3.down, 100, groundMask);

		// update animator parameters
		animator.SetBool (hashIsCrouch, isDown);
		animator.SetFloat (hashGroundDistance, distanceFromGround.distance == 0 ? 99 : distanceFromGround.distance - characterHeightOffset);
		animator.SetFloat (hashFallSpeed, rig2d.velocity.y);
		animator.SetFloat (hashSpeed, Mathf.Abs (axis));

        // flip sprite
       // if (axis != 0)
		//	spriteRenderer.flipX = axis < 0;

        var move = axis;
        if (Mathf.Abs(move) > 0)
        {
            Quaternion rot = transform.rotation;
            transform.rotation = Quaternion.Euler(rot.x, Mathf.Sign(move) == 1 ? 0 : 180, rot.z);
        }

        rig2d.velocity = new Vector2(move * maxSpeed, rig2d.velocity.y);
    }
}
