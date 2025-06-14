/*
 * Author: 
 * Date: 
 * Description: A script to handle player mechanics, including movement, interactions, and health/stamina management.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// The class responsible for the player mechanics.
/// </summary>
public class Player : MonoBehaviour
{
    // References to other scripts
    public Enemy enemyScript;

    // UI elements
    public GameObject congratsMessage;
    public Slider slider;          // Stamina slider
    public Slider slider1;         // Health slider
    public TextMeshProUGUI displayText;
    public TextMeshProUGUI playerHealthtext;

    // Player's camera
    public GameObject playerCamera;

    // Player's stats
    float moveSpeed = 5f;
    float rotationSpeed = 30f;
    bool interact = false;
    bool SprintKey = false;
    float Timer = 0f;
    float Timer1 = 0f;
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
    public void SetMaxHealth(int health)
    {
        slider1.maxValue = health;
        slider1.value = health;
    }

    /// <summary>
    /// Sets the maximum stamina value for the stamina slider.
    /// </summary>
    /// <param name="stamina">The maximum stamina value.</param>
    public void SetMaxStamina(int stamina)
    {
        slider.maxValue = stamina;
        slider.value = stamina;
    }

    /// <summary>
    /// Updates the health slider value.
    /// </summary>
    /// <param name="health">The current health value.</param>
    public void SetHealth(int health)
    {
        slider1.value = health;
    }

    /// <summary>
    /// Updates the stamina slider value.
    /// </summary>
    /// <param name="stamina">The current stamina value.</param>
    public void SetStamina(int stamina)
    {
        slider.value = stamina;
    }

    /// <summary>
    /// Applies damage to the current enemy.
    /// </summary>
    public void DoDamage()
    {
        Timer1 = 0.2f;
        enemyScript.slider3.value -= 0.1f;
        Debug.Log("Damage to enemy");
    }

    public void SetUp()
    {
        playerHealthtext = GameObject.FindObjectOfType<FindScoreText>().GetComponent<TextMeshProUGUI>();
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
        float movementSpeed = moveSpeed;

        if (SprintKey && slider.value > 0)
        {
            movementSpeed = 80;
            slider.value -= 0.2f * Time.deltaTime;
        }
        else if (!SprintKey || slider.value <= 0)
        {
            if (Timer <= 0)
            {
                Timer = 1;
                slider.value += 1f * Time.deltaTime;
            }
        }

        Vector3 movementVector = transform.forward * movementInput.y + transform.right * movementInput.x;
        transform.position += movementVector * movementSpeed * Time.deltaTime;
        playerHealthtext.text = slider1.value.ToString();
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
        if (Timer > 0)
        {
            Timer -= Time.deltaTime;
        }

        if (Timer1 > 0)
        {
            Timer1 -= Time.deltaTime;
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
        SprintKey = value.Get<float>() == 1f;
        Debug.Log(SprintKey);
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
            if (interact && Timer1 <= 0)
            {
                DoDamage();
            }
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
    }
}
