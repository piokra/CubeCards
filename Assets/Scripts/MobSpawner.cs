using System.Collections;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    public GameObject Mob;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(SpawnMobs());
    }

    IEnumerator SpawnMobs()
    {
        yield return new WaitForEndOfFrame();
        while (true)
        {
            var mob = Instantiate(Mob);
            var script = mob.GetComponent<Follow>();
            script.What = PlayerLogic.Instance.gameObject;
            float theta = Random.Range(0, Mathf.PI*2);
            var p = PlayerLogic.Instance.transform.position + 10 * new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);
            p.z = -1;
            mob.transform.position = p;
            yield return new WaitForSeconds(SpawnDelay());
        }
    }

    public float SpawnDelay()
    {
        return 20f / Mathf.Log(2 + Time.fixedTime);
    }
}