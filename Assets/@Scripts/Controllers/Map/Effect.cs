using UnityEngine;
using UnityEngine.ParticleSystemJobs;
using static Define;

public class Effect : InitBase
{
    private float _lifeTime = 0.3f;
    private float _timer;

    private SpriteRenderer _spriteRenderer;
    private ParticleSystem _particleSystem;
    private Animator _animator;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _particleSystem = GetComponent<ParticleSystem>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sortingOrder = SortingLayers.SKILL_EFFECT;
        return true;
    }

    public void Play(string animName)
    {
        _timer = 0f;
        gameObject.SetActive(true);

        if (_particleSystem != null)
            _particleSystem.Play(true);

        if (_animator != null)
            _animator.Play($"{animName}", -1, 0f);
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_particleSystem != null)
        {
            if (!_particleSystem.IsAlive())
            {
                Managers.Resource.Destroy(gameObject);
            }
            return;
        }

        if (_timer > _lifeTime)
        {
            Managers.Resource.Destroy(gameObject);
        }
    }
}
