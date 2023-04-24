using System.Text;
using UnityEngine;

namespace Core.Services.Marina
{
    public class MarinaBase64ConverterToTex2D
    {
        //0. [volatile] in the case that the standards change I will need to change it from 128
        //0.5 create a texture of 128 x 128 using the format RGBA32
        //1. take in a base64 string.
        //2. do any decoding according to what my standard is usually UTF8 encoding.
        //3. take the bytes.
        //4. Load Raw texture data
        //5. apply the raw texture
        //6. return the texture2d to wherever.

        private readonly Texture2D _marinaTexture2D;
        private readonly TextureFormat _textureFormat;
        public MarinaBase64ConverterToTex2D( int width, int height,TextureFormat textureFormat = TextureFormat.RGBA32)
        {
            _textureFormat = textureFormat;
            _marinaTexture2D = new Texture2D(width, height, _textureFormat, false);
        }

        public Texture2D Convert(string base64String)
        {
            //Make sure Texture is currently clean
            if (!_marinaTexture2D.Reinitialize(_marinaTexture2D.width, _marinaTexture2D.width, _textureFormat, false))
            {
                Debug.LogError($"could not Reinitialize Texture");
                return null;
            }
            byte[] convertedbytes = System.Convert.FromBase64String(base64String);
            _marinaTexture2D.LoadRawTextureData(convertedbytes);
            _marinaTexture2D.Apply();
            return _marinaTexture2D;
        }
        
    }
}