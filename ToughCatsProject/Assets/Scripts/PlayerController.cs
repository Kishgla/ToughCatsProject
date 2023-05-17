using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController characterController;

    [Header("----- Player Attributes -----")]
    [SerializeField][Range(1, 10)] int currentHP;
    [SerializeField][Range(1, 10)] int maxHP;
    [SerializeField][Range(1, 5)] float playerSpeed;
    [SerializeField][Range(2, 5)] float sprintMod;
    [SerializeField][Range(10, 50)] float jumpHeight;
    [SerializeField][Range(9.81f, 20)] float gravityValue;
    [SerializeField][Range(1, 3)] int jumpMax;

    [Header("----- Weapon Attributes -----")]
    [SerializeField][Range(2, 300)] int shootDistance;
    [SerializeField][Range(0.1f, 3)] float shootRate;
    [SerializeField][Range(1, 10)] int shootDamage;

    private Vector3 move;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private int jumpedTimes;
    private bool isSprinting;
    private bool isShooting;

    //Properties
    public int CurrentHP { 
        get
        {
            return currentHP;
        }
        set
        {
            if(value < 0)
            {
                currentHP = 0;
            }else if(value > maxHP)
            {
                currentHP = maxHP;
            }
            else
            {
                currentHP = value;
            }
        }
    }
    public int MaxHP => maxHP;
    public float PlayerSpeed => playerSpeed;
    public float SprintMod => sprintMod;
    public float JumpHeight => jumpHeight;
    public float GravityValue => gravityValue;
    public int JumpMax => jumpMax;
    public int ShootDistance => shootDistance;
    public float ShootRate => shootRate;
    public int ShootDamage => shootDamage;

    private void Start()
    {
        SpawnPlayer();
    }

    private void Update()
    {
        Movement();
        Sprint();

        if(Input.GetButton("Shoot") && !isShooting)
        {
            StartCoroutine(Shoot());
        }
        else if (Input.GetButtonDown("Grenade"))
        {
            Debug.Log("Throwing grenade!");
        }
        else if (Input.GetButtonDown("Melee"))
        {
            Debug.Log("High five to the face!");
        }
    }

    void Movement()
    {
        groundedPlayer = characterController.isGrounded;
        if(groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
            jumpedTimes = 0;
        }

        move = (transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical"));
        characterController.Move(move * Time.deltaTime * PlayerSpeed);

        if (Input.GetButtonDown("Jump") && jumpedTimes < JumpMax)
        {
            jumpedTimes++;
            playerVelocity.y = JumpHeight;
        }

        playerVelocity.y -= GravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }

    void Sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            isSprinting = true;
            playerSpeed *= SprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            isSprinting = false;
            playerSpeed /= SprintMod;
        }
    }

    public void takeDamage(int damage)
    {
        CurrentHP -= damage;

        if (CurrentHP <= 0)
        {
            //kill player and respawn
            gameManager.instance.youLose();
        }
    }

    public void HealPlayer(int amount)
    {
        CurrentHP += amount;
    }

    IEnumerator Shoot()
    {
        isShooting = true;

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, ShootDistance))
        {
            IDamage damageable = hit.collider.GetComponent<IDamage>();
            if (damageable != null)
            {
                damageable.takeDamage(ShootDamage);
            }
        }

        yield return new WaitForSeconds(ShootRate);

        isShooting = false;
    }

    public void SpawnPlayer()
    {
        characterController.enabled = false;
        CurrentHP = MaxHP;
        transform.position = gameManager.instance.playerSpawnPOS.transform.position;
        characterController.enabled = true;
    }
}