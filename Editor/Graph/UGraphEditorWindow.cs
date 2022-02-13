/**Copyright 2016 - 2022, Dream Machine Game Studio. All Right Reserved.*/

using UnityEditor;
using UnityEngine;

namespace DreamMachineGameStudio.Dreamworks.Graph.Editor
{
    public class UGraphEditorWindow : EditorWindow
    {
        #region Fields
        public static UGraphEditorWindow CurrentWindow;

        private IGraphEditor _graphEditor = new FGraphEditor();

        private GraphActivity _currentActivity = GraphActivity.Idle;

        private float _zoom = 1;

        private Vector2 _panOffset;

        private Rect _selectionRect;
        #endregion

        #region UEditor Methods
        private void OnGUI()
        {
            Controls();

            DrawGrid(position, _panOffset, _zoom);

            DrawSelectionBox();
        }
        #endregion

        #region Private Methods
        private void Controls()
        {
            Event currentEvent = Event.current;

            switch (currentEvent.type)
            {
                case EventType.ScrollWheel:
                    OnScrollWheel(currentEvent);
                    break;
                case EventType.MouseDown:
                    OnMouseDown(currentEvent);
                    break;
                case EventType.MouseUp:
                    OnMouseUp(currentEvent);
                    break;
                case EventType.MouseDrag:
                    OnMouseDrag(currentEvent);
                    break;
            }

            Repaint();
        }

        private void OnScrollWheel(Event currentEvent)
        {
            float oldZoom = _zoom;

            if (currentEvent.delta.y > 0)
            {
                _zoom += _zoom * FGraphEditorPrefrences.Setting.ZoomStep;
            }
            else
            {
                _zoom += _zoom * FGraphEditorPrefrences.Setting.ZoomStep * -1;
            }

            _zoom = Mathf.Clamp(_zoom, FGraphEditorPrefrences.Setting.MinZoom, FGraphEditorPrefrences.Setting.MaxZoom);

            if (FGraphEditorPrefrences.Setting.ShouldZoomToMouse)
            {
                _panOffset += (1 - oldZoom / _zoom) * (WindowToGridPosition(currentEvent.mousePosition) + _panOffset);
            }
        }

        private void OnMouseDown(Event currentEvent)
        {
            if (currentEvent.button == 0)
            {
                _currentActivity = GraphActivity.HoldGrid;
                _selectionRect.position = WindowToGridPosition(currentEvent.mousePosition);
            }
            else if (currentEvent.button == 2)
            {
                _panOffset += currentEvent.delta * _zoom;
            }
        }

        private void OnMouseUp(Event currentEvent)
        {
            if (currentEvent.button == 0)
            {
                _currentActivity = GraphActivity.Idle;
            }
        }

        private void OnMouseDrag(Event currentEvent)
        {
            if (currentEvent.button == 0)
            {
                OnLeftMouseButtonDrag(currentEvent);
            }
            else if (currentEvent.button == 2)
            {
                _panOffset += currentEvent.delta * _zoom;
            }
        }

        private void OnLeftMouseButtonDrag(Event currentEvent)
        {
            _currentActivity = GraphActivity.DragGrid;
            Vector2 currentMousePosition = WindowToGridPosition(currentEvent.mousePosition);
            _selectionRect.size = currentMousePosition - _selectionRect.position;
        }

        private void DrawSelectionBox()
        {
            if (_currentActivity == GraphActivity.DragGrid)
            {
                Rect rect = _selectionRect;
                rect.position = GridToWindowPosition(rect.position);
                rect.size /= _zoom;
                Handles.DrawSolidRectangleWithOutline(rect, new Color(0, 0, 0, 0.1f), new Color(1, 1, 1, 0.6f));
            }
        }

        private void DrawGrid(Rect rect, Vector2 panOffset, float zoom)
        {
            rect.position = Vector2.zero;

            Vector2 center = rect.size / 2;
            Texture2D gridTexture = _graphEditor.GridTexture;
            Texture2D crossTexture = _graphEditor.CrossTexture;

            float xOffset = -(center.x * zoom + panOffset.x) / gridTexture.width;
            float yOffset = ((center.y - rect.size.y) * zoom + panOffset.y) / gridTexture.height;

            Vector2 tileOffset = new Vector2(xOffset, yOffset);

            float tileAmountX = Mathf.Round(rect.size.x * zoom) / gridTexture.width;
            float tileAmountY = Mathf.Round(rect.size.y * zoom) / gridTexture.height;

            Vector2 tileAmount = new Vector2(tileAmountX, tileAmountY);

            GUI.DrawTextureWithTexCoords(rect, gridTexture, new Rect(tileOffset, tileAmount));
            GUI.DrawTextureWithTexCoords(rect, crossTexture, new Rect(tileOffset + new Vector2(0.5f, 0.5f), tileAmount));
        }

        private Vector2 WindowToGridPosition(Vector2 windowPosition)
        {
            return (windowPosition - (position.size * 0.5f) - (_panOffset / _zoom)) * _zoom;
        }

        private Vector2 GridToWindowPosition(Vector2 gridPosition)
        {
            return (position.size * 0.5f) + (_panOffset / _zoom) + (gridPosition / _zoom);
        }
        #endregion

        #region Nested Types
        private enum GraphActivity : byte
        {
            Idle = 0,
            HoldGrid = 1,
            DragGrid = 2
        }
        #endregion
    }
}