using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Player_Controller : MonoBehaviour
{
    [Header("Movement Attributes")]
    [SerializeField] private float _moveSpeed = 50f;

    [Header("Dependencies")]
    [SerializeField] private Rigidbody2D _rigidbody;

    [Header("Animation")]
    [SerializeField] private Animator _animator;

    [Header("Sprite Renderer")]
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private Vector2 _moveDir = Vector2.zero;

    private void Update()
    {
        GatherInput();
    }

    private void FixedUpdate()
    {
        MovementUpdate();
        HandleAnimation();
    }

    private void GatherInput()
    {
        _moveDir.x = Input.GetAxisRaw("Horizontal");
        _moveDir.y = Input.GetAxisRaw("Vertical");
    }

    private void MovementUpdate()
    {
        _rigidbody.velocity = _moveDir * _moveSpeed * Time.fixedDeltaTime;
    }

    private void HandleAnimation()
    {
        // Atualiza os parâmetros do Animator com base na direção do movimento
        _animator.SetFloat("moveX", _moveDir.x);
        _animator.SetFloat("moveY", _moveDir.y);

        // Ajustar a direção do sprite baseado no movimento horizontal
        if (_moveDir.x != 0)
        {
            _spriteRenderer.flipX = _moveDir.x < 0;
        }
    }
}
