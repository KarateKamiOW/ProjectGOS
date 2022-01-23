using UnityEngine;
using UnityEngine.Events;
public class PlayerController : MonoBehaviour
{


	public static PlayerController instance;

	public IInteractable Interactable { get; set; }
	//[SerializeField] DialogUI dialogUI;
	public DialogUI DialogueUI { get; set; }
	public QuestLogObject QuestLog { get; set; }
	public InventoryObject inventory;

	[SerializeField] public float m_JumpForce;                          // Amount of force added when the player jumps. Was 400
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = 1f; //was .35f         // Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	//[SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
	//[SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching

	[Header("Checkers")]
	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool isGrounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	public Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;


	[Header("Movement, Layers, and Animators")]
	float horizontalMove = 0f;
	public float runSpeed;
	public float jumpHangTime = .2f;
	public Animator animator;
	//public Animator squashAndStretchAnim;
	//public LayerMask interactableLayer;
	public ParticleSystem dust;





	bool jump = false;
	bool playerjumped = false;
	bool crouch = false;
	float hangCounter;

	//Dashing Variables
	float dashTime = .2f;
	bool isDashing = false;


	//End Dashing Variables
	public string areaTransitionName { get; set; }
	public bool Paused { get; set; }

	public bool Grounded { get { return isGrounded; } }

	public bool FacingRight { get { return m_FacingRight; } }


	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;
	bool prev_Grounded = false;



	private void Awake()
	{

		Paused = false;

		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();

		if (instance == null)
		{
			instance = this;
			//SingletonData.CharacterController2DInstanceData = this;
		}
		else if (instance != this && instance == null)
		{
			//instance = SingletonData.CharacterController2DInstanceData;
		}
		else if (instance != this && instance != null)
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);

		QuestLog = GetComponent<QuestLogObject>();
	}

	private void Update()
	{
		prev_Grounded = isGrounded;
		//Coyote/Hang Time forgiveness
		if (isGrounded)
			hangCounter = jumpHangTime;
		else
			hangCounter -= Time.deltaTime;

		CheckInteraction();

	}

	private void FixedUpdate()
	{
		//GroundCheck
		if (!Paused)
		{
			Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
			jump = false;
		}
	}
	void GroundCheck()
	{
		isGrounded = false;
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		if (colliders.Length > 0)
			isGrounded = true;
	}


	public void Move(float move, bool crouch, bool jump)
	{

			//only control the player if grounded or airControl is turned on
			if (isGrounded || m_AirControl)
			{

				// Move the character by finding the target velocity
				Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
				// And then smoothing it out and applying it to the character
				m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

				// If the input is moving the player right and the player is facing left...
				if (move > 0 && !m_FacingRight)
				{
					// ... flip the player.
					Flip();
				}
				// Otherwise if the input is moving the player left and the player is facing right...
				else if (move < 0 && m_FacingRight)
				{
					// ... flip the player.
					Flip();
				}
			}


			// If the player should jump...
			if (jump && hangCounter > 0f) //was && hangCounter > 0
			{
				playerjumped = true;
				if (hangCounter > 0)
				{
					isGrounded = false;
					m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce);
				}
				else
				{
					isGrounded = false;
					m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce);
				}
				// Add a vertical force to the player.
				//isGrounded = false;
				//m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));		OLD JUMP LOGIC
				//m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce);
			}

		
		
	}

	private void Flip()
	{
		CreateDust();
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;

	}

	public void HandleUpdate()
	{
		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
		animator.SetFloat("MoveX", Mathf.Abs(horizontalMove));
		animator.SetBool("IsGrounded", isGrounded);

		isGrounded = Physics2D.OverlapCircle(m_GroundCheck.position, .15f, m_WhatIsGround);

		if (Paused)
		{
			animator.SetFloat("MoveX", 0f);
			Move(0f, false, false);
		}


		if (Input.GetButtonDown("Jump"))
		{
			if (isGrounded)
			{
				jump = true;
				CreateDust();
			}

		}
		
	}

	public void PauseMovement() 
	{
		Paused = true;
		animator.SetFloat("MoveX", 0f);
		Move(0f, false, false);
	}

	void CheckInteraction() 
	{
        if (Input.GetKeyDown(KeyCode.Z))
		{
			Interactable?.Interact(this);	
		}
	}

	void CreateDust()
	{
		//dust.Play();
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
		var item = other.GetComponent<Item>();
		if (item) 
		{
			inventory.AddItem(item.item, 1);
			Destroy(other.gameObject); 
		}
    }

    private void OnApplicationQuit()
    {
		inventory.Container.Clear();
    }
}
