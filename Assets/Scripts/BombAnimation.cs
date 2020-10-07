using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAnimation : MonoBehaviour
{
    private Vector3 startSize = new Vector3(1, 1, 1);
    private Vector3 endSize = new Vector3(12, 12, 12);
    private Vector3 currentSize = default;
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private float depth = -1.3f;
    [SerializeField] private SpriteRenderer blastRadius = default;
    private float timer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < 1.0f)
        {
            currentSize = Vector3.Lerp(startSize, endSize, timer);
            transform.localScale = Vector3.Lerp(currentSize, endSize, timer);

            float currentY = Mathf.Lerp(-.9f, depth, timer);
            float expoY = Mathf.Lerp(currentY, depth, timer);
            Vector3 temp = transform.localPosition;
            temp.y = expoY;
            transform.localPosition = temp;

            float inOut = Mathf.Sin(timer * Mathf.PI);
            blastRadius.color = new Color(1f, 1f, 1f, inOut);

            timer += Time.deltaTime * speed;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
