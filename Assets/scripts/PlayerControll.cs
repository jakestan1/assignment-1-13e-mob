/*
 * Author: 
 * Date: 
 * Description: A script to handle player mechanics, including movement, interactions, and health/stamina management.
 */

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// The class responsible for the player mechanics.
/// </summary>
public class PlayerController : MonoBehaviour
{
    // Interactivity and door opening
    Door currentDoor;

    // References to other scripts
    public Enemy enemyScript;

    // UI elements
    public GameObject congratsMessage;
    public Slider staminaSlider;
    public Slider healthSlider;
    public TextMeshProUGUI displayText;
    public TextMeshProUGUI playerHealthText;

    // Player's camera
    public GameObject playerCamera;

    // Player's stats
    float moveSpeed = 5f;
    float rotationSpeed = 30f;
    bool interact = false;
    bool sprintKey = false;
    float staminaRegenTimer = 0f;
    float attackCooldownTimer = 0f;
    int playerScore = 0;
    int coinsCollected = 0;

    // Input vectors
    Vector3 movementInput = Vector3.zero;
    Vector3 rotationInput = Vector3.zero;
    Vector3 headRotationInput = Vector3.zero;

    /// <summary>
    /// Sets the maximum health value for the health slider.
    /// </summary>
    /// <param name="health">The maximum health value.</param>
    public void SetMaxHealth(float health)
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;
    }

    /// <summary>
    /// Sets the maximum stamina value for the stamina slider.
    /// </summary>
    /// <param name="stamina">The maximum stamina value.</param>
    public void SetMaxStamina(float stamina)
    {
        staminaSlider.maxValue = stamina;
        staminaSlider.value = stamina;
    }

    /// <summary>
    /// Updates the health slider value.
    /// </summary>
    /// <param name="health">The current health value.</param>
    public void SetHealth(float health)
    {
        healthSlider.value = health;
    }

    /// <summary>
    /// Updates the stamina slider value.
    /// </summary>
    /// <param name="stamina">The current stamina value.</param>
    public void SetStamina(float stamina)
    {
        staminaSlider.value = stamina;
    }

    /// <summary>
    /// Sets up initial references and UI elements.
    /// </summary>
    public void SetUp()
    {
        playerHealthText = GameObject.FindObjectOfType<FindScoreText>().GetComponent<TextMeshProUGUI>();
        displayText = GameObject.FindObjectOfType<FindScoreText>().GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialization code if needed
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleRotation();
        UpdateTimers();
    }

    /// <summary>
    /// Handles player movement based on input.
    /// </summary>
    void HandleMovement()
    {
        float currentMoveSpeed = moveSpeed;

        if (sprintKey && staminaSlider.value > 0)
        {
            currentMoveSpeed = 80;
            staminaSlider.value -= 0.2f * Time.deltaTime;
        }
        else if (!sprintKey || staminaSlider.value <= 0)
        {
            if (staminaRegenTimer <= 0)
            {
                staminaRegenTimer = 1;
                staminaSlider.value += 1f * Time.deltaTime;
            }
        }

        Vector3 movementVector = transform.forward * movementInput.y + transform.right * movementInput.x;
        transform.position += movementVector * currentMoveSpeed * Time.deltaTime;
        playerHealthText.text = healthSlider.value.ToString("F1"); // Format health value to one decimal place
    }

    /// <summary>
    /// Handles player rotation based on input.
    /// </summary>
    void HandleRotation()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + rotationInput * rotationSpeed * Time.deltaTime);
        playerCamera.transform.rotation = Quaternion.Euler(playerCamera.transform.rotation.eulerAngles + headRotationInput * rotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Updates the interaction and cooldown timers.
    /// </summary>
    void UpdateTimers()
    {
        if (staminaRegenTimer > 0)
        {
            staminaRegenTimer -= Time.deltaTime;
        }

        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
    }

    // Input action handlers
    void OnLook(InputValue value)
    {
        rotationInput.y = value.Get<Vector2>().x;
        headRotationInput.x = value.Get<Vector2>().y * -1;
    }

    void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    void OnSprint(InputValue value)
    {
        sprintKey = value.Get<float>() == 1f;
        Debug.Log(sprintKey);
    }

    void OnFire()
    {
        Debug.Log("Interact");
        interact = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Handle collision with enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemyScript = collision.gameObject.GetComponent<Enemy>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Handle interaction with collectibles
        if (other.CompareTag("Collectibles"))
        {
            Coin coin = other.GetComponent<Coin>();
            playerScore += coin.coinScore;
            coinsCollected++;
            displayText.text = playerScore.ToString();
            if (coinsCollected >= 4)
            {
                congratsMessage.SetActive(true);
            }
            coin.Collected();
        }

        // Decrease health if player collides with an enemy
        if (other.CompareTag("Enemy"))
        {
            enemyScript = other.GetComponent<Enemy>();
            DecreaseHealth(0.1f); // Adjust the damage value to 0.1
        }
        if (other.CompareTag("key"))
        {
            Destroy(other.gameObject);
            Key = true;
        }
    }

    void OnInteract()
    {
        if (currentDoor != null)
        {
            currentDoor.OpenDoor();
            currentDoor = null;
        }
    }

    void OnTriggerStay(Collider other)
    {
        // Handle continuous collision with enemy
        if (other.CompareTag("Enemy"))
        {
            enemyScript = other.GetComponent<Enemy>();
        }
    }

    /// <summary>
    /// Decreases the player's health.
    /// </summary>
    /// 
    public void DecreaseHealth(float damage)
    {
        float newHealth = healthSlider.value - damage;
        SetHealth(newHealth);
    }

    public bool Key = false;

    /// <summary>
    /// Updates the current door the player can interact with.
    /// </summary>
    /// <param name="door">The door the player can interact with.</param>
    public void UpdateDoor(Door door)
    {
        currentDoor = door;
    }
}
