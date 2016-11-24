using UnityEngine;
using System.Collections;

public class TrapBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer rd;
    private BoxCollider2D box;
    private BoxCollider2D box2;

    private bool blink = true;
    public bool left;
    private float lastDisplay;
    private float displayDelay = 0.1f;
    private float stayOnGroundTime;
    private bool markedForDeath = false;

    void Start()
    {
        if (gameObject.transform.parent.name.Contains("Dynamic"))
        {
            rb = GetComponent<Rigidbody2D>();
            rd = GetComponent<SpriteRenderer>();
            box = GetComponent<BoxCollider2D>();
            box.enabled = false;
            rb.gravityScale = 0;
            StartCoroutine(WaitForBlink(1));
            if (gameObject.transform.parent.name.Contains("Cave"))
                StartCoroutine(WaitForFall(Random.Range(2, 5)));
            else if (gameObject.transform.parent.name.Contains("Coliseum"))
                StartCoroutine(WaitForThrow(Random.Range(2, 5)));
        }
        else
            blink = false;
    }

    private IEnumerator WaitForBlink(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        blink = false;
        rd.enabled = true;
        box.enabled = true;
    }

    private IEnumerator WaitForFall(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        rb.gravityScale = 2;
    }

    private IEnumerator WaitForDeath(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        GetComponentInParent<DynamicTrapSpawner>().SetDeathTimer(Time.time);
        Destroy(gameObject.transform.parent.gameObject);
    }

    private IEnumerator WaitForThrow(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (left)
            rb.AddForce(Vector2.left * Random.Range(400, 500));
        else
            rb.AddForce(Vector2.right * Random.Range(400, 500));

    }

    void Update()
    {
        if( blink)
        {
            if (rd.enabled && Time.time - lastDisplay >= displayDelay)
            {
                rd.enabled = false;
                lastDisplay = Time.time;
            }
            else if( !rd.enabled && Time.time - lastDisplay >= displayDelay)
            {
                rd.enabled = true;
                lastDisplay = Time.time;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(gameObject.transform.parent.name.Contains("Dynamic"))
        {
            if (col.transform.parent.tag.Equals("Player") && col.gameObject.GetComponentInParent<PlayerStatus>().IsVulnerable() && !markedForDeath)
            {
                col.GetComponentInParent<PlayerStatus>().Hurt();
                GetComponentInParent<DynamicTrapSpawner>().SetDeathTimer(Time.time);
                Destroy(gameObject.transform.parent.gameObject);
            }
        }
        else
        {
            if (col.transform.parent.tag.Equals("Player") && col.GetComponentInParent<PlayerStatus>().IsVulnerable())
                col.GetComponentInParent<PlayerStatus>().Hurt();
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if( gameObject.transform.parent.name.Contains("Dynamic") && !col.transform.parent.tag.Equals("Player"))
        {
            rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            markedForDeath = true;
            StartCoroutine(WaitForDeath(2));
        }
    }
}
