using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLV2 : EnemyBase
{
    [SerializeField]
    private Vector3 growthAmount = new Vector3(0.4f, 0.4f, 0.4f); // amount to grow each time

    [SerializeField]
    private Renderer visualRenderer;

    [SerializeField]
    private Color flashColor = Color.red;

    private Color originalColor;
    private bool isFlashing = false;

    // Override Start to modify the timer interval
    protected override void Start()
    {
        base.Start();
        timerTotal = 5f; 

        if (visualRenderer != null)
        {
            originalColor = visualRenderer.material.color;
        }
    }

    protected override void Update()
    {
        base.Update();
        nav.SetDestination(target.position); //Chase player 
    }

    protected override void TimerContent()
    {
        base.TimerContent(); 
        transform.localScale += growthAmount;
        StartCoroutine(FlashVisual());
    }

    public override void Damaged(int damage)
    {
        damage = 10;
        gameManager.score = Mathf.Max(0, gameManager.score - damage);
        Debug.Log("lose point");
    }


    private System.Collections.IEnumerator FlashVisual()
    {
        if (visualRenderer == null) yield break;

        isFlashing = true;
        visualRenderer.material.color = flashColor;
        yield return new WaitForSeconds(0.2f); 
        visualRenderer.material.color = originalColor;
        isFlashing = false;
    }
}
