using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rb;
    private AudioSource _audioSource;

    [SerializeField]
    private float speed = 1f;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        var velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        
        if (velocity.magnitude > 1f)
        {
            velocity.Normalize();
        }
        
        _rb.velocity = velocity * speed;
    }

    void Update()
    {
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) return;
        
        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
    }
}