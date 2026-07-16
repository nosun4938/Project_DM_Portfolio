using UnityEngine;
using static Define;

public class EffectManager
{
    public GameObject Play(string key, Vector2 pos, bool flip = false)
    {
        GameObject go = Managers.Resource.Instantiate(key, pooling: true);

        if (go == null)
            return null;

        go.transform.position = pos;
        go.transform.rotation = Quaternion.identity;

        Vector3 scale = go.transform.localScale;
        scale.x = flip ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        go.transform.localScale = scale;

        Effect effect = go.GetOrAddComponent<Effect>();
        if (effect != null)
            effect.Play(key);

        return go;
    }

    public GameObject Play(string key, Vector2 pos, float angle, bool flip = false)
    {
        GameObject go = Managers.Resource.Instantiate(key, pooling: true);

        if (go == null)
            return null;

        go.transform.position = pos;
        go.transform.rotation = Quaternion.Euler(0, 0, angle);

        Vector3 scale = go.transform.localScale;
        scale.x = flip ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        go.transform.localScale = scale;

        Effect effect = go.GetOrAddComponent<Effect>();
        if (effect != null)
            effect.Play(key);

        return go;
    }
}
