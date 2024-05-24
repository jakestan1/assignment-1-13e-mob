using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Enemy : MonoBehaviour
{
    public Player playerScript;

    public GameObject enemyMessage;

    public TextMeshProUGUI enemyHealth;

    public float smoothing = 1f;
    public Transform target;

    public float Timer;

    public Slider slider3;

    public Vector3 jump;

    public float jumpForce = 2.0f;

    public bool isGrounded;

    Rigidbody rb;

    public void SetMaxHealth(int health)
    {
        slider3.maxValue = health;
        slider3.value = health;
    }

    public void SetHealth(int health)
    {
        slider3.value = health;
    }



    public void Damage()
    {
        Timer = 1;
        playerScript.slider1.value -= 0.1f;
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (Timer <= 0)
            {
                Damage();
            }
        }

        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    private string currentState;

    private string nextState;

    private void SwitchState()
    {
        StartCoroutine(currentState);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = "MyCoroutine";
        nextState = currentState;
        rb = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 2.0f, 0.0f);
        playerScript = GameObject.FindObjectOfType<Player>();
        Debug.Log(playerScript == null);
        SwitchState();
    }


    IEnumerator MyCoroutine()
    {
        bool condition = true;
        while (condition)
        {
            if (Vector3.Distance(transform.position, target.position) > 0.05f)
            {
                transform.position = Vector3.Lerp(transform.position, target.position, smoothing * Time.deltaTime);
                enemyHealth.text = slider3.value.ToString();
            }

            if (isGrounded)
            {
                Debug.Log(rb == null);
                rb.AddForce(jump * jumpForce, ForceMode.Impulse);
                isGrounded = false;
            }

            if (slider3.value <= 0.4f)
            {
                nextState = "MyCoroutineC";
                condition = false;
            }
            yield return new WaitForEndOfFrame();

        }
        SwitchState();
    }

    IEnumerator MyCoroutineC()
    {
        bool condition = true;
        while (condition)
        {
            if (Vector3.Distance(transform.position, target.position) > 0.05f)
            {
                smoothing = 3;
                jump = new Vector3(0.0f, 4.0f, 0.0f);
                transform.position = Vector3.Lerp(transform.position, target.position, smoothing * Time.deltaTime);
                enemyHealth.text = slider3.value.ToString();
            }

            if (isGrounded)
            {
                Debug.Log(rb == null);
                rb.AddForce(jump * jumpForce, ForceMode.Impulse);
                isGrounded = false;
            }
            yield return null;

        }
    }


    private IEnumerator Wait()
    {
        enemyMessage.SetActive(true);

        yield return new WaitForSeconds(5);

        enemyMessage.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (nextState != currentState)
        {
            currentState = nextState;
        }

        if (Timer > 0)
        {
            Timer -= Time.deltaTime;
        }

        if (slider3.value == 0f)
        {
            StartCoroutine(Wait());
            Destroy(gameObject);
        }
    }
}
