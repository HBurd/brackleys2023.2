using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumetricLightManager : MonoBehaviour
{
    [SerializeField]
    int passes = 1;
    void Awake()
    {
        GameObject[] light_objects = GameObject.FindGameObjectsWithTag("VolumetricLight");
        VolumetricLight[] lights = new VolumetricLight[light_objects.Length];

        for (int i = 0; i < light_objects.Length; ++i)
        {
            lights[i] = light_objects[i].GetComponent<VolumetricLight>();
        }

        foreach (VolumetricLight light in lights)
        {
            if (light.root)
            {
                light.last_pass = light.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity;
                light.intensity = light.last_pass;
            }
        }

        for (int i = 0; i < passes; ++i)
        {
            foreach (VolumetricLight light in lights)
            {
                if (light.intensity > 0.0f)
                {
                    continue;
                }

                List<VolumetricLight> neighbours = GetNeighbours(light, lights);

                float radius = light.GetComponent<UnityEngine.Rendering.Universal.Light2D>().pointLightOuterRadius;

                foreach (VolumetricLight other in neighbours)
                {
                    float distance = (other.transform.position - light.transform.position).magnitude;
                    float t = Mathf.Clamp(distance / radius, 0.0f, 1.0f);
                    light.last_pass = Mathf.Max(light.last_pass, Mathf.Lerp(other.intensity, 0, t*t));
                }
            }

            foreach (VolumetricLight light in lights)
            {
                light.intensity = light.last_pass;
            }
        }

        GameObject.Find("Sky/Global Light").GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = 0.0f;
    }

    List<VolumetricLight> GetNeighbours(VolumetricLight light, VolumetricLight[] lights)
    {
        float radius = light.GetComponent<UnityEngine.Rendering.Universal.Light2D>().pointLightOuterRadius;

        List<VolumetricLight> result = new List<VolumetricLight>();

        foreach (VolumetricLight other_light in lights)
        {
            if (Object.ReferenceEquals(light, other_light))
            {
                continue;
            }
            if ((other_light.transform.position - light.transform.position).sqrMagnitude < radius * radius)
            {
                result.Add(other_light);
            }
        }

        return result;
    }
}
