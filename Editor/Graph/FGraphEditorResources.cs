/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using UnityEngine;

namespace DreamMachineGameStudio.Dreamworks.Graph.Editor
{
    public static class FGraphEditorResources
    {
        #region Properties
        public static Texture2D Dot { get; } = Resources.Load<Texture2D>("SPR_Graph_Dot");

        public static Texture2D DotOutline { get; } = Resources.Load<Texture2D>("SPR_Graph_Dot_Outer");

        public static Texture2D Node { get; } = Resources.Load<Texture2D>("SPR_Graph_Node");

        public static Texture2D NodeHighlight { get; } = Resources.Load<Texture2D>("SPR_Graph_Node_Highlight");
        #endregion

        #region Methods
        public static Texture2D GenerateGridTexture(Color lineColor, Color backgroundColor)
        {
            Color[] colors = new Color[64 * 64];
            for (int y = 0; y < 64; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    Color col = backgroundColor;
                    if (y % 16 == 0 || x % 16 == 0) col = Color.Lerp(lineColor, backgroundColor, 0.65f);
                    if (y == 63 || x == 63) col = Color.Lerp(lineColor, backgroundColor, 0.35f);
                    colors[(y * 64) + x] = col;
                }
            }

            Texture2D texture = new Texture2D(64, 64);
            texture.SetPixels(colors);
            texture.wrapMode = TextureWrapMode.Repeat;
            texture.filterMode = FilterMode.Bilinear;
            texture.name = "GridTexture";
            texture.Apply();
            return texture;
        }

        public static Texture2D GenerateCrossTexture(Color lineColor)
        {
            Color[] colors = new Color[64 * 64];
            for (int y = 0; y < 64; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    Color col = lineColor;
                    if (y != 31 && x != 31) col.a = 0;
                    colors[(y * 64) + x] = col;
                }
            }

            Texture2D rextures = new Texture2D(64, 64);
            rextures.SetPixels(colors);
            rextures.wrapMode = TextureWrapMode.Clamp;
            rextures.filterMode = FilterMode.Bilinear;
            rextures.name = "CrossTexture";
            rextures.Apply();
            return rextures;
        }
        #endregion
    }
}