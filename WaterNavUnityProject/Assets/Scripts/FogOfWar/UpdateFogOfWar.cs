using System;
using PathCreation;
using RCR.Settings.FogOfWar;
using RCR.Utilities;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace FogOfWar
{
    [RequireComponent(typeof(SpriteMask))]
    [RequireComponent(typeof(PathCreator))]
    public class UpdateFogOfWar : MonoBehaviour
    {

        [SerializeField] private ResolutionsTextureSize m_default_Texture_size;

        [SerializeField] private float PixelsPerUnit;

        [SerializeField] private Sprite m_sprite_mask;

        [SerializeField] private Sprite m_ground;

        private PathCreator m_pathCreator;
        private SpriteMask m_mask;

        private SpriteRenderer own;

        private GameObject BaseGround;

        private bool m_updateFogOfWar = true;

        private int FlatPackedTextureSize
        {
            get
            {
                if (m_sprite_mask == null)
                    return GetDeaultTextureSize * GetDeaultTextureSize;

                return m_sprite_mask.texture.width * m_sprite_mask.texture.height;
            }
        }
        
        private void Awake()
        {
            m_mask = GetComponent<SpriteMask>();
            m_pathCreator = GetComponent<PathCreator>();
            own = GetComponent<SpriteRenderer>();
            InitRawMaskTexture();
            if (!ValidateSizeOf())
            {
                Debug.LogWarning($"{m_sprite_mask.texture.name} is not the same dimensions as {m_ground.texture.name} " +
                                 $"this could make it harder to work out the fog of war");
                this.gameObject.SetActive(false);
                return;
            }
        }

        private void OnEnable()
        {
            m_pathCreator.bezierPath.OnModified += BezierPathOnOnModified;
        }

        private void OnDisable()
        {
            m_pathCreator.bezierPath.OnModified -= BezierPathOnOnModified;
            ClearRawMaskTexture();
        }

        private void BezierPathOnOnModified()
        {
            m_updateFogOfWar = true;
        }

        private void ClearRawMaskTexture()
        {
            m_mask.sprite = null;
            m_sprite_mask = null;
            m_ground = null;
        }

        private void InitRawMaskTexture()
        {
            if (m_sprite_mask == null)
            {
                m_sprite_mask =
                    LBUtilities.SpriteFromTexture(new Texture2D(GetDeaultTextureSize, GetDeaultTextureSize, TextureFormat.RGBA32,
                        false),PixelsPerUnit);
            }

            own.sprite = m_sprite_mask;
            if (BaseGround == null)
            {
                BaseGround = new GameObject("BaseGround");
                BaseGround.transform.SetParent(this.transform);
                BaseGround.transform.localPosition = Vector3.zero;
                SpriteRenderer render = BaseGround.AddComponent<SpriteRenderer>();
                render.sprite = m_ground;
                render.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            }

            m_mask.sprite = m_sprite_mask;
        }

        private bool ValidateSizeOf()
        {
            if (m_sprite_mask == null || m_ground == null)
                return false;

            if (m_sprite_mask.texture.width == m_ground.texture.width &&
                m_ground.texture.width == m_ground.texture.height)
                return true;

            return false;
        }

        private int GetDeaultTextureSize => m_default_Texture_size switch
        {
            ResolutionsTextureSize.x16 => 16,
            ResolutionsTextureSize.x32 => 32,
            ResolutionsTextureSize.x64 => 64,
            ResolutionsTextureSize.x128 => 128,
            ResolutionsTextureSize.x256 => 256,
            ResolutionsTextureSize.x512 => 512,
            ResolutionsTextureSize.x1024 => 1024,
            ResolutionsTextureSize.x2048 => 2048,
            ResolutionsTextureSize.x4096 => 4096,
            _ => 0
        };

        private void Update()
        {
            if (m_updateFogOfWar)
            {
                m_updateFogOfWar = false;

                NativeArray<Vector2> result = new NativeArray<Vector2>(FlatPackedTextureSize, Allocator.TempJob); //hopefully this job lasts no more than 4 frames

                var PJobData = new PixelsToWorldUnitJob
                {
                    PixelsPerUnit = m_sprite_mask.pixelsPerUnit,
                    results = result
                };
                JobHandle PJobHandle = PJobData.Schedule(FlatPackedTextureSize,
                    1);

                NativeArray<int> resultNonAlloc = new NativeArray<int>(FlatPackedTextureSize, Allocator.TempJob);
                NativeArray<Vector3> _VertexPositions =
                    new NativeArray<Vector3>(m_pathCreator.path.localPoints, Allocator.TempJob);
                var IntersectionJobData = new IntersectionNonAllocJob()
                {
                    VertexPositions = _VertexPositions,
                    Origins = result,
                    result = resultNonAlloc
                };

                JobHandle IntersectionJobHandle = IntersectionJobData.Schedule(PJobHandle);

                NativeArray<RGBA32> UpdateResult = new NativeArray<RGBA32>(FlatPackedTextureSize, Allocator.TempJob);
                var UpdateJobData = new FogOfWarJob()
                {
                    StateOfPosition = resultNonAlloc,
                    FogOfWarData = UpdateResult
                };

                JobHandle UpdateJobHandle = UpdateJobData.Schedule(FlatPackedTextureSize, 1,
                    IntersectionJobHandle);
                
                UpdateJobHandle.Complete();
                byte[] RawData = new byte[FlatPackedTextureSize * 4];
                if (RawData.Length == UpdateResult.Length * 4)
                {
                    for (int i = 0; i < UpdateResult.Length; i++)
                    {
                        RawData[i] = UpdateResult[i].R;
                        RawData[i + 1] = UpdateResult[i].G;
                        RawData[i + 2] = UpdateResult[i].B;
                        RawData[i + 3] = UpdateResult[i].A;
                    }
                }

                result.Dispose();
                resultNonAlloc.Dispose();
                _VertexPositions.Dispose();
                UpdateResult.Dispose();
                
                m_sprite_mask.texture.LoadRawTextureData(RawData);
                m_sprite_mask.texture.Apply();

            }
        }
    }
}