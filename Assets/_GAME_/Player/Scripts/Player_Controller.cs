using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
public class Player_Controller : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private NavMeshAgent _agent;

    [Header("Animation")]
    [SerializeField] private Animator _animator;

    [Header("Sprite Renderer")]
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private Camera _cam;
    private Vector3 _target;
    private bool _isMovingToTarget = false;

    public bool canMove = true; // Variável para controlar a capacidade de movimento do jogador

    private void Start()
    {
        _cam = Camera.main;
        _agent.speed = 3.5f;
        _agent.stoppingDistance = 0.5f;
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    private void Update()
    {
        if (canMove && Input.GetMouseButtonDown(0))
        {
            _target = _cam.ScreenToWorldPoint(Input.mousePosition);
            _target.z = transform.position.z;
            _agent.SetDestination(_target);
            _isMovingToTarget = true;
        }

        if (_isMovingToTarget && canMove)
        {
            if (!_agent.pathPending)
            {
                if (_agent.remainingDistance <= _agent.stoppingDistance)
                {
                    if (_agent.hasPath && _agent.velocity.sqrMagnitude == 0f)
                    {
                        _isMovingToTarget = false;
                        _agent.ResetPath(); // Reset the path to stop the agent
                    }
                }
            }
        }

        if (canMove)
        {
            HandleAnimation();
        }
    }

    private void HandleAnimation()
    {
        Vector2 moveDir = _agent.desiredVelocity;

        if (_isMovingToTarget)
        {
            _animator.SetFloat("moveX", moveDir.x);
            _animator.SetFloat("moveY", moveDir.y);
            //_animator.SetBool("isWalking", moveDir.magnitude > 0);

            _spriteRenderer.flipX = moveDir.x < 0;
        }
        else
        {
            //_animator.SetBool("isWalking", false);
        }
    }
}
