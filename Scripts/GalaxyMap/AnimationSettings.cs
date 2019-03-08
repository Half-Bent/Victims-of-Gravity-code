using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSettings : MonoBehaviour
{
    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetAnimationBool()
    {
        animator.SetBool("IdleAnimation", true);
        Invoke("InvokeLater", 0.2f);
    }

    void InvokeLater()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);

    }
}
