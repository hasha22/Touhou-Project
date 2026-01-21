using UnityEngine;



public class PlayerAnimator : MonoBehaviour
{
    public void Animate(Animator animator, float input) {

        animator.SetFloat("horizontalMovement", input);

    }

}
