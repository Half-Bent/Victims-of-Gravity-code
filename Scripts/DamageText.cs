using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour {

    private Animator animator;
    private Text damage;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        // Get the only animation clip from the child's Animator
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, clipInfo[0].clip.length);
        damage = GetComponentInChildren<Text>();
    }

    public void SetValue(string value)
    {
        damage.text = "-" + value;
    }
}
