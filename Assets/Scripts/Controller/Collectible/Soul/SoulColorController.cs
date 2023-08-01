using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.Rendering.Universal;

namespace Assets.Scripts.Controller.Collectible.Soul
{
    public class SoulColorController : MonoBehaviour
    {
        [SerializeField]
        Gradient _gradient;

        public void Init(float value)
        {
            Color color = _gradient.Evaluate(value);
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (!spriteRenderer) return;

            foreach (Material mat in spriteRenderer.materials)
            {
                mat.SetColor("_Color", color);
            }

            Light2D light2D = GetComponentInChildren<Light2D>();
            if (!light2D) return;

            light2D.color = color;
            TrailRenderer trailRenderer = GetComponentInChildren<TrailRenderer>();
            if (!trailRenderer) return;

            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(color, 0.0f), new GradientColorKey(Color.white, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(160, 0.0f), new GradientAlphaKey(0, 1.0f) }
            );

            trailRenderer.colorGradient = gradient;
        }
    }
}