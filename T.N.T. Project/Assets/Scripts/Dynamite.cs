using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamite : MonoBehaviour
{
    [SerializeField]
    private Transform fuseStart, fuseFinish, sparks, boom;

    private LineRenderer lineRend;

    private bool coroutineAllowed;

    // Start is called before the first frame update
    private void Start()
    {
        lineRend = GetComponent<LineRenderer>();
        lineRend.positionCount = 2;
        lineRend.SetPosition(0, new Vector2(fuseStart.position.x, fuseStart.position.y));
        lineRend.SetPosition(1, new Vector2(fuseFinish.position.x, fuseFinish.position.y));
        sparks.position = new Vector3(fuseStart.position.x, fuseStart.position.y, 0f);
        sparks.gameObject.SetActive(false);
        boom.position = transform.position;
        boom.gameObject.SetActive(false);
        coroutineAllowed = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && coroutineAllowed)
        {
            StartCoroutine(FireItUp());
        }
    }

    private IEnumerator FireItUp()
    {
        coroutineAllowed = false;
        sparks.gameObject.SetActive(true);

        while (fuseStart.position.x <= fuseFinish.position.x)
        {
            fuseStart.Translate(+0.05f, 0f, 0f);
            lineRend.SetPosition(0, new Vector2(fuseStart.position.x, fuseStart.position.y));
            sparks.position = new Vector3(fuseStart.position.x, fuseStart.position.y, 0f);
            yield return new WaitForSeconds(0.025f);
        }

        sparks.gameObject.SetActive(false);
        boom.gameObject.SetActive(true);
        Destroy(boom.gameObject, 0.5f);
        lineRend.positionCount = 0;
        Destroy(gameObject);
    }
}
