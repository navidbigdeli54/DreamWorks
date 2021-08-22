/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using System;
using UnityEngine;

namespace DreamMachineGameStudio.Dreamworks.Graph.Editor
{
    [Serializable]
    public class FSetting
    {
        #region Fields
        [SerializeField]
        private float _maxZoom = 5.0f;

        [SerializeField]
        private float _minZoom = 1.0f;

        [SerializeField]
        private float _zoomStep = 0.1f;

        [SerializeField]
        private bool _shouldZoomToMouse = true;

        [SerializeField]
        private bool _isPortTooltipEnable = true;

        [SerializeField]
        private bool _isSnapEnable = true;

        [SerializeField]
        private bool _isAutoSaveEnable = true;

        [SerializeField]
        private Color32 _gridLineColor = new Color(0.45f, 0.45f, 0.45f);

        [SerializeField]
        private Color32 _gridBackgroundColor = new Color(0.18f, 0.18f, 0.18f);

        [SerializeField]
        private Color32 _highlightColor = new Color32(255, 255, 255, 255);

        [SerializeField]
        private Texture2D _gridTexture;

        [SerializeField]
        private Texture2D _crossTexture;
        #endregion

        #region Properties
        public float MaxZoom => _maxZoom;

        public float MinZoom => _minZoom;

        public float ZoomStep => _zoomStep;

        public bool ShouldZoomToMouse => _shouldZoomToMouse;

        public bool IsPortTooltipEnable => _isPortTooltipEnable;

        public bool IsSnapEnable => _isSnapEnable;

        public bool IsAutoSaveEnable => _isAutoSaveEnable;

        public Color32 GridLineColor => _gridLineColor;

        public Color32 GridBackgroundColor => _gridBackgroundColor;

        public Color32 HighlightColor => _highlightColor;

        public Texture2D GridTexture
        {
            get
            {
                if (_gridTexture == null)
                {
                    _gridTexture = FGraphEditorResources.GenerateGridTexture(_gridLineColor, _gridBackgroundColor);
                }
                return _gridTexture;
            }
        }

        public Texture2D CrossTexture
        {
            get
            {
                if (_crossTexture == null)
                {
                    _crossTexture = FGraphEditorResources.GenerateCrossTexture(_gridLineColor);
                }
                return _crossTexture;
            }
        }
        #endregion
    }
}