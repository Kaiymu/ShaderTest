using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintWithMouse : MonoBehaviour
{

    [SerializeField]
    private Camera _cam;
    [SerializeField]
    private Shader _drawShader;

    [Header("Shader properties")]
    [SerializeField]
    [Range(1, 500)]
    private float _size;
    [SerializeField]
    [Range(0, 1)]
    private float _strength;

    private RenderTexture _splatMap;
    private Material _currentMaterial;
    private Material _drawMaterial;
    private RaycastHit _hit;

    private void Start()
    {
        _drawMaterial = new Material(_drawShader);
        _drawMaterial.SetVector("_Color", Color.red);

        _currentMaterial = GetComponent<MeshRenderer>().material;

        _splatMap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat);
        _currentMaterial.SetTexture("_SplatMap", _splatMap);

    }

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            if(Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), out _hit))
            {
                _drawMaterial.SetVector("_Coordinates", new Vector4(_hit.textureCoord.x, _hit.textureCoord.y, 0, 0));
                _drawMaterial.SetFloat("_Strength", _strength);
                _drawMaterial.SetFloat("_Size", _size);

                RenderTexture temp = RenderTexture.GetTemporary(_splatMap.width, _splatMap.height, 0, RenderTextureFormat.ARGBFloat);
                Graphics.Blit(_splatMap, temp);
                Graphics.Blit(temp, _splatMap, _drawMaterial);

                RenderTexture.ReleaseTemporary(temp);
            }
        }
    }
}
