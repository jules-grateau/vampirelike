using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleController : MonoBehaviour
{
    private ParticleSystem m_GibEffect;
    const int k_splitPixelSize = 4;

    // Start is called before the first frame update
    void Awake()
    {
        this.m_GibEffect = Instantiate(Resources.Load<ParticleSystem>("Prefabs/Particles/destructible_gibs"), gameObject.transform.position, Quaternion.identity, gameObject.transform);

        this.buildMaterial();
    }

    public void onDestroy()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
        //AudioSource.PlayClipAtPoint(this.m_PuffSound, transform.position, 1);
        this.m_GibEffect.transform.parent = null;
        this.m_GibEffect.transform.localScale = Vector3.one;
        this.m_GibEffect.Play();
        Destroy(this.m_GibEffect, this.m_GibEffect.main.startLifetime.constant);
    }

    private void buildMaterial()
    {
        Sprite s = this.gameObject.GetComponent<SpriteRenderer>().sprite;
        Vector2 size = s.rect.size;
        int maxX = (int)(size.x / k_splitPixelSize);
        int maxY = (int)(size.y / k_splitPixelSize);

        var croppedTexture = new Texture2D((int)size.x, (int)size.y);
        var pixels = s.texture.GetPixels((int)s.rect.position.x, (int)s.rect.position.y, (int)size.x, (int)size.y);
        croppedTexture.filterMode = FilterMode.Point;
        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply();

        Material mat = this.gameObject.GetComponent<SpriteRenderer>().material;
        mat.mainTexture = croppedTexture;
        this.gameObject.GetComponentInChildren<ParticleSystemRenderer>().material = mat;

        // Change the number of sprite pool
        ParticleSystem.TextureSheetAnimationModule ts = this.m_GibEffect.textureSheetAnimation;
        ts.numTilesX = maxX;
        ts.numTilesY = maxY;
        ts.animation = ParticleSystemAnimationType.WholeSheet;
        ts.startFrame = new ParticleSystem.MinMaxCurve(0, maxX + maxY);

        this.m_GibEffect.startSize = 0.25f;
        this.m_GibEffect.startColor = this.gameObject.GetComponent<SpriteRenderer>().color;

        ParticleSystem.SizeOverLifetimeModule solm = this.m_GibEffect.sizeOverLifetime;
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0.0f, 1.0f);
        curve.AddKey(1.0f, 0.0f);
        solm.size = new ParticleSystem.MinMaxCurve(1f, curve);

        // Change the number of particles emmited
        ParticleSystem.EmissionModule em = this.m_GibEffect.emission;
        ParticleSystem.Burst b = em.GetBurst(0);
        b.count = maxX + maxY;
        em.SetBurst(0, b);

        // Change the size of the emmiter shape
        ParticleSystem.ShapeModule sm = this.m_GibEffect.shape;
        sm.radius = sm.radius * (Mathf.Max(size.x, size.y) / 16);
    }
}
