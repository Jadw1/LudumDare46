using UnityEngine;

public class Movement : MonoBehaviour
{
    private Animator _animator;
    private CharacterController _characterController;

    [SerializeField]
    public float walkSpeed = 1.0f;
    
    void Start()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
    }

    void FixedUpdate ()
    {
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");

        var direction = new Vector3(x, 0, z).normalized;

        var movement = direction * walkSpeed;

        _characterController.Move(movement * Time.deltaTime);

        _animator.SetFloat("speed", Mathf.Abs(movement.magnitude));
    }
}
