using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace FrameIO.Main
{
    public class TextMarkerService : IBackgroundRenderer
    {
        private readonly TextEditor textEditor;
        private readonly TextSegmentCollection<TextMarker> markers;

        public sealed class TextMarker : TextSegment
        {
            public TextMarker(int startOffset, int length)
            {
                StartOffset = startOffset;
                Length = length;
            }

            public Color? BackgroundColor { get; set; }
            public Color MarkerColor { get; set; }
            public string ToolTip { get; set; }
        }

        public TextMarkerService(TextEditor textEditor)
        {
            this.textEditor = textEditor;
            markers = new TextSegmentCollection<TextMarker>(textEditor.Document);
        }

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            if (markers == null || !textView.VisualLinesValid)
            {
                return;
            }
            var visualLines = textView.VisualLines;
            if (visualLines.Count == 0)
            {
                return;
            }
            int viewStart = visualLines.First().FirstDocumentLine.Offset;
            int viewEnd = visualLines.Last().LastDocumentLine.EndOffset;
            foreach (TextMarker marker in markers.FindOverlappingSegments(viewStart, viewEnd - viewStart))
            {
                //if (marker.Length == 0) continue;
                if (marker.BackgroundColor != null)
                {
                    var geoBuilder = new BackgroundGeometryBuilder { AlignToWholePixels = true, CornerRadius = 3 };
                    geoBuilder.AddSegment(textView, marker);
                    Geometry geometry = geoBuilder.CreateGeometry();
                    if (geometry != null)
                    {
                        Color color = marker.BackgroundColor.Value;
                        var brush = new SolidColorBrush(color);
                        brush.Freeze();
                        drawingContext.DrawGeometry(brush, null, geometry);
                    }
                }
                foreach (Rect r in BackgroundGeometryBuilder.GetRectsForSegment(textView, marker))
                {
                    Point startPoint = r.BottomLeft;
                    Point endPoint = r.BottomRight;

                    var usedPen = new Pen(new SolidColorBrush(marker.MarkerColor), 1);
                    usedPen.Freeze();
                    const double offset = 2.5;

                    int count = Math.Max((int)((endPoint.X - startPoint.X) / offset) + 1, 4);

                    var geometry = new StreamGeometry();

                    using (StreamGeometryContext ctx = geometry.Open())
                    {
                        ctx.BeginFigure(startPoint, false, false);
                        ctx.PolyLineTo(CreatePoints(startPoint, endPoint, offset, count).ToArray(), true, false);
                    }

                    geometry.Freeze();

                    drawingContext.DrawGeometry(Brushes.Transparent, usedPen, geometry);
                    break;
                }
            }
        }

        public KnownLayer Layer
        {
            get { return KnownLayer.Selection; }
        }

        private IEnumerable<Point> CreatePoints(Point start, Point end, double offset, int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new Point(start.X + (i * offset), start.Y - ((i + 1) % 2 == 0 ? offset : 0));
            }
        }

        public void Clear()
        {
            var m = markers.LastSegment;
            while (m != null)
            {
                Remove(m);
                m = markers.LastSegment;
            }
        }

        private void Remove(TextMarker marker)
        {
            if (markers.Remove(marker))
            {
                Redraw(marker);
            }
            else
            {
                Debug.Assert(false);
            }
        }

        private void Redraw(ISegment segment)
        {
            textEditor.TextArea.TextView.Redraw(segment);
        }

        //static int iiii = 0;

        public void Create(int offset, int length, string message)
        {
            if (length <= 0)
                return;
            //foreach(var mk in markers)
            //{
            //    if (offset >= mk.StartOffset && offset <= mk.EndOffset) return;
            //    if (offset + length >= mk.StartOffset && offset + length <= mk.EndOffset) return;
            //}
            var m = new TextMarker(offset, length);
            if (markers.FindOverlappingSegments(m).Count == 0)
            {
                markers.Add(m);
                m.MarkerColor = Colors.Red;
                m.ToolTip = message;

                Redraw(m);
            }

            //m.BackgroundColor = iiii%2==0?Colors.Yellow:Colors.YellowGreen;
            //iiii += 1;
            //textEditor.TextArea.TextView.InvalidateLayer(KnownLayer.Background);
        }

        public IEnumerable<TextMarker> GetMarkersAtOffset(int offset)
        {
            return markers == null ? Enumerable.Empty<TextMarker>() : markers.FindSegmentsContaining(offset);
        }
    }
}
