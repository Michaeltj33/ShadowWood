using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Esteira : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetAnimator(bool value)
    {
        if (animator.GetBool("moviment") != value)
        {
            animator.SetBool("moviment", value);
        }
    }
}
