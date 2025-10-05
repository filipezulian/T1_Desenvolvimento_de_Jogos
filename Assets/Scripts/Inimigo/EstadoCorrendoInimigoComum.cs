using UnityEngine;

public class EstadoCorrendoInimigoComum : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("podeAndar", true);
    }

}
