using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    const float Threshold = 0.1f;
    enum State {Idle, Turn, Active};

    [SerializeField]
    AudioClip[] launchAudio;

    [SerializeField]
    AudioClip[] deathAudio;

    [SerializeField]
    UnityEvent turn;

    [SerializeField]
    UnityEvent acceptValue;

    [SerializeField]
    GameObject explosion;

    State state = State.Idle;
    Rigidbody2D rb;
    Computer cpu;

    public PlayerManager manager = null;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cpu = GetComponent<Computer>();
    }

    void FixedUpdate()
    {
        if (state == State.Active && rb.velocity.magnitude < Threshold)
        {
            EndTurn();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        other.SendMessage("PlayerCollision", null, SendMessageOptions.DontRequireReceiver);
    }

    void OutOfBounds()
    {
        Die();
    }

    void PlayerCollision()
    {
        if (state == State.Idle)
        {
            Die();
        }
    }

    public void TakeTurn()
    {
        state = State.Turn;
        turn.Invoke();
    }

    public void AcceptValue()
    {
        acceptValue.Invoke();
    }

    public void Launch(float power)
    {
        rb.AddForce(transform.up * power * rb.mass, ForceMode2D.Impulse);
        state = State.Active;
        Camera.main.GetComponent<AudioSource>()
            .PlayOneShot(launchAudio[Random.Range(0, launchAudio.Length)]);
    }

    void EndTurn()
    {
        state = State.Idle;
        rb.Sleep();
        manager.NextTurn();
    }

    void Die()
    {
        if (gameObject.activeSelf)
        {
            Camera.main.GetComponent<AudioSource>()
                .PlayOneShot(deathAudio[Random.Range(0, deathAudio.Length)]);
            gameObject.SetActive(false);
            Destroy(gameObject);
            if (state == State.Turn || state == State.Active)
            {
                EndTurn();
            }
        }
    }

    void OnDestroy()
    {
        if (gameObject.scene.isLoaded)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
        }
    }
}
