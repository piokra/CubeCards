using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeObject : MonoBehaviour
{
    public enum Stat
    {
        Strength,
        Intelligence,
        Agillity,
        Age,
        Health,
        MaxHealth
    }

    private Dictionary<Stat, float> _defaultStatistics = new Dictionary<Stat, float>();
    private Dictionary<string, float> _customStatistics = new Dictionary<string, float>();

    // Update is called once per frame
    public virtual void Update()
    {
        if (GetStat(Stat.Health) <= Mathf.Epsilon)
        {
            OnDeath();
        }
    }

    public virtual void OnDeath()
    {
        Debug.Log("CubeObject died");
        Destroy(gameObject);
    }

    public float GetStat(Stat stat)
    {
        float ret;
        return _defaultStatistics.TryGetValue(stat, out ret) ? ret : 0;
    }

    public float GetStat(string stat)
    {
        float ret;
        return _customStatistics.TryGetValue(stat, out ret) ? ret : 0;
    }

    public void ModifyStat(string stat, float diff)
    {
        _customStatistics[stat] = GetStat(stat) + diff;
    }

    public void ModifyStat(Stat stat, float diff)
    {
        _defaultStatistics[stat] = GetStat(stat) + diff;
    }

    public void SetStat(string stat, float val)
    {
        _customStatistics[stat] = val;
    }

    public virtual void SetStat(Stat stat, float val)
    {
        _defaultStatistics[stat] = val;
    }
    
    public virtual void Damage(float dmg)
    {
        Debug.Log("Taking damage!");

        var particles = Instantiate(EffectLibrary.Instance.SimpleParticleSystem);
        var p = transform.position;
        p.z = -0.5f;
        particles.transform.position = p;
        
        var system = particles.GetComponent<ParticleSystem>();
        system.startColor = Color.red;
        
        Destroy(particles, 0.4f);
        ModifyStat(Stat.Health, -dmg);
    }

    public virtual void Heal(float heal)
    {
        
        Debug.Log("Healing up!");
        var particles = Instantiate(EffectLibrary.Instance.SimpleParticleSystem);
        var p = transform.position;
        p.z = -0.5f;
        particles.transform.position = p;
        
        var system = particles.GetComponent<ParticleSystem>();
        system.startColor = Color.green;
        Destroy(particles, 0.4f);
        
        ModifyStat(Stat.Health, heal);
        SetStat(Stat.Health, Mathf.Min(GetStat(Stat.Health), GetStat(Stat.MaxHealth)));
    }
}