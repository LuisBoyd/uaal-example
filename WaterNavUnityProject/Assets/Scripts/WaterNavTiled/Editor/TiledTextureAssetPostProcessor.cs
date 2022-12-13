using System;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;
using WaterNavTiled.Editor.Jobs;

namespace WaterNavTiled.Editor
{
    public class TiledTextureAssetPostProcessor: AssetPostprocessor
    {
        private void OnPreprocessTexture()
        {
            // if(!assetPath.Contains("WaterNavTiled"))
            //     return;
            // string[] AssetpathArray = assetPath.Split("/");
            // string AssetName = AssetpathArray[^1].Split(".")[0];
            // var factory = new SpriteDataProviderFactories();
            // factory.Init();
            // var dataProvider = factory.GetSpriteEditorDataProviderFromObject(assetImporter);
            // dataProvider.InitSpriteEditorDataProvider();
            // var SpriteNameFileIDDataProvider = dataProvider.GetDataProvider<ISpriteNameFileIdDataProvider>();
            //
            // var spriteRects = dataProvider.GetSpriteRects().ToList();
            // var nameFileIdPairs = SpriteNameFileIDDataProvider.GetNameFileIdPairs().ToList();
            // spriteRects.Clear();
            // nameFileIdPairs.Clear();
            // if (assetImporter is TextureImporter)
            // {
            //     int frameNumber = 0;
            //     int Height = Mathf.FloorToInt(dataProvider.pixelsPerUnit);
            //     int Width = Height;
            //     int MaxTextureSize = (assetImporter as TextureImporter).maxTextureSize;
            //     if (MaxTextureSize % dataProvider.pixelsPerUnit == 0)
            //     {
            //         for (int y = MaxTextureSize; y > 0; y -= Height)
            //         {
            //             for (int x = 0; x < MaxTextureSize; x+=Width)
            //             {
            //                 var newSprite = new SpriteRect()
            //                 {
            //                     name = $"{frameNumber}_temp",
            //                     spriteID = GUID.Generate(),
            //                     rect = new Rect(new Vector2(x, y - Width),
            //                         new Vector2(Width, Height)),
            //                     alignment = 0,
            //                     pivot = Vector2.zero
            //                 };
            //                 nameFileIdPairs.Add(new SpriteNameFileIdPair(
            //                     newSprite.name, newSprite.spriteID));
            //                 spriteRects.Add(newSprite);
            //                 frameNumber++;
            //             }
            //         }
            //     }
            // }
            // SpriteNameFileIDDataProvider.SetNameFileIdPairs(nameFileIdPairs.ToArray());
            // dataProvider.SetSpriteRects(spriteRects.ToArray());
            // dataProvider.Apply();
        }

        private void OnPostprocessTexture(Texture2D texture)
        {
            // if(!assetPath.Contains("WaterNavTiled"))
            //     return;
            // string[] AssetpathArray = assetPath.Split("/");
            // string AssetName = AssetpathArray[^1].Split(".")[0];
            // var factory = new SpriteDataProviderFactories();
            // factory.Init();
            // var dataProvider = factory.GetSpriteEditorDataProviderFromObject(assetImporter);
            // dataProvider.InitSpriteEditorDataProvider();
            // var SpriteNameFileIDDataProvider = dataProvider.GetDataProvider<ISpriteNameFileIdDataProvider>();
            //
            // int frameNumber = 0;
            // NativeArray<int> result = new NativeArray<int>(1, Allocator.TempJob);
            // NativeArray<Color32> TextureData = texture.GetPixelData<Color32>(0);
            // int width = Mathf.FloorToInt(dataProvider.pixelsPerUnit);
            // int height = width;
            // var spriteRects = dataProvider.GetSpriteRects().ToList();
            // //var spriteRects = dataProvider.GetSpriteRects().ToList();
            // var nameFileIdPairs = SpriteNameFileIDDataProvider.GetNameFileIdPairs().ToList();
            // for (int i = 0; i < spriteRects.Count; i++)
            // {
            //     int EndX = Mathf.FloorToInt(spriteRects[i].rect.position.x + spriteRects[i].rect.width);
            //     int EndY = Mathf.FloorToInt(spriteRects[i].rect.position.y + spriteRects[i].rect.height);
            //     if (EndX > texture.width || EndY > texture.height)
            //     {
            //         spriteRects.RemoveAt(i);
            //         nameFileIdPairs.RemoveAt(i);
            //     }
            // }
            //
            // for (int i = 0; i < spriteRects.Count; i++)
            // {
            //     int xPosition = Mathf.FloorToInt(spriteRects[i].rect.position.x);
            //     int yPosition = Mathf.FloorToInt(spriteRects[i].rect.position.y);
            //     CheckTextureRegionJob job = new CheckTextureRegionJob();
            //     job.result = result;
            //     job.height = height;
            //     job.width = width;
            //     job.xpos = xPosition;
            //     job.ypos = yPosition;
            //     job.TextureData = TextureData;
            //     
            //     JobHandle handle = job.Schedule();
            //     handle.Complete();
            //     
            //     if (result[0] != 0)
            //     {
            //         var newSprite = new SpriteRect()
            //         {
            //             name = $"{texture.name}_{frameNumber}",
            //         };
            //             
            //         nameFileIdPairs.Add(new SpriteNameFileIdPair(
            //             newSprite.name, newSprite.spriteID));
            //         spriteRects.Add(newSprite);
            //         frameNumber++;
            //     }
            //     else
            //     {
            //         spriteRects.RemoveAt(i);
            //         nameFileIdPairs.RemoveAt(i);
            //     }
            // }
            //
            // SpriteNameFileIDDataProvider.SetNameFileIdPairs(nameFileIdPairs.ToArray());
            // dataProvider.SetSpriteRects(spriteRects.ToArray());
            //
            // //TODO make changes
            //
            // dataProvider.Apply();
        }
    }
}